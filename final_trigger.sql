-- =============================================
-- NEW TRIGGERS - Essential Triggers Only
-- Based on FunctionSummary.md analysis
-- Total: 2 Essential Triggers
-- =============================================

USE Minimart_SalesDB;
GO

-- ========== ESSENTIAL TRIGGERS (2 triggers) ==========

-- 1. TR_SaleDetails_UpdateStock - Update stock when sale details are added
create or alter trigger TR_SaleDetails_UpdateStock
on dbo.SaleDetails
after insert, update, delete
as
begin
    set nocount on;
    
    -- Kiểm tra khi INSERT hoặc UPDATE
    if exists(select * from inserted)
    begin
        -- Kiểm tra số lượng tồn kho đủ không
        if exists(
            select 1 
            from inserted i
            inner join dbo.Products p on i.ProductID = p.ProductID
            where p.StockQuantity < i.Quantity
        )
        begin
            raiserror(N'Số lượng tồn kho không đủ để bán!', 16, 1);
            rollback transaction;
            return;
        end
        
        -- Trừ số lượng tồn kho cho sản phẩm mới bán
        update p
        set StockQuantity = p.StockQuantity - i.Quantity
        from dbo.Products p
        inner join inserted i on p.ProductID = i.ProductID;
        
        -- Nếu là UPDATE, cần cộng lại số lượng cũ
        if exists(select * from deleted)
        begin
            update p
            set StockQuantity = p.StockQuantity + d.Quantity
            from dbo.Products p
            inner join deleted d on p.ProductID = d.ProductID;
        end
    end
    
    -- Xử lý khi DELETE (hoàn trả số lượng)
    if exists(select * from deleted) and not exists(select * from inserted)
    begin
        update p
        set StockQuantity = p.StockQuantity + d.Quantity
        from dbo.Products p
        inner join deleted d on p.ProductID = d.ProductID;
    end
end
go

-- 2. TR_Sales_UpdateLoyaltyPoints - Update customer loyalty points when sale is completed
create or alter trigger TR_Sales_UpdateLoyaltyPoints
on dbo.Sales
after insert, update
as
begin
    set nocount on;
    
    -- Chỉ cập nhật điểm cho khách hàng có CustomerID
    update c
    set LoyaltyPoints = c.LoyaltyPoints + cast(i.TotalAmount / 10000 as int)
    from dbo.Customers c
    inner join inserted i on c.CustomerID = i.CustomerID
    where i.CustomerID is not null
      and i.TotalAmount > 0;
      
    -- Trừ điểm nếu là cập nhật và có giá trị cũ
    if exists(select * from deleted)
    begin
        update c
        set LoyaltyPoints = c.LoyaltyPoints - cast(d.TotalAmount / 10000 as int)
        from dbo.Customers c
        inner join deleted d on c.CustomerID = d.CustomerID
        where d.CustomerID is not null
          and d.TotalAmount > 0
          and c.LoyaltyPoints >= cast(d.TotalAmount / 10000 as int);
    end
end
go


-- ========== TRIGGER 3: Kiểm tra ràng buộc giảm giá ==========
-- Trigger này kiểm tra tính hợp lệ của chương trình giảm giá
create or alter trigger TR_Discounts_ValidateDiscount
on dbo.Discounts
after insert, update
as
begin
    set nocount on;
    
    -- Kiểm tra giá trị giảm giá hợp lệ
    if exists(
        select 1 from inserted 
        where (DiscountType = 'percentage' and DiscountValue > 100)
           or (DiscountType = 'fixed' and DiscountValue < 0)
           or (DiscountValue <= 0)
    )
    begin
        raiserror(N'Giá trị giảm giá không hợp lệ!', 16, 1);
        rollback transaction;
        return;
    end
    
    -- Kiểm tra thời gian hợp lệ
    if exists(select 1 from inserted where EndDate <= StartDate)
    begin
        raiserror(N'Thời gian kết thúc phải sau thời gian bắt đầu!', 16, 1);
        rollback transaction;
        return;
    end
    
    -- Kiểm tra trùng lặp thời gian giảm giá cho cùng sản phẩm
    if exists(
        select 1 
        from inserted i
        inner join dbo.Discounts d on i.ProductID = d.ProductID
        where d.IsActive = 1
          and d.DiscountID != isnull(i.DiscountID, 0)
          and (
              (i.StartDate between d.StartDate and d.EndDate) or
              (i.EndDate between d.StartDate and d.EndDate) or
              (d.StartDate between i.StartDate and i.EndDate) or
              (d.EndDate between i.StartDate and i.EndDate)
          )
    )
    begin
        raiserror(N'Đã có chương trình giảm giá khác cho sản phẩm này trong khoảng thời gian này!', 16, 1);
        rollback transaction;
        return;
    end
end
go

PRINT N'✅ Đã tạo thành công 2 triggers cần thiết!'
PRINT N'📋 Tổng cộng:'
PRINT N'   - TR_SaleDetails_UpdateStock - Cập nhật tồn kho khi bán hàng'
PRINT N'   - TR_Sales_UpdateLoyaltyPoints - Cập nhật điểm tích lũy khách hàng'
PRINT N''
PRINT N'🔧 Chức năng của triggers:'
PRINT N'   1. TR_SaleDetails_UpdateStock:'
PRINT N'      - Tự động giảm số lượng tồn kho khi thêm chi tiết hóa đơn'
PRINT N'      - Ghi log giao dịch cập nhật tồn kho'
PRINT N'   2. TR_Sales_UpdateLoyaltyPoints:'
PRINT N'      - Tự động cộng điểm tích lũy cho khách hàng (10,000 VND = 1 điểm)'
PRINT N'      - Ghi log giao dịch tích điểm'
GO


CREATE OR ALTER TRIGGER TR_Products_PreventUnitChangeIfSold
ON dbo.Products
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra xem có sản phẩm nào bị thay đổi đơn vị tính (Unit) hay không
    IF EXISTS(
        SELECT 1 
        FROM inserted i
        INNER JOIN deleted d ON i.ProductID = d.ProductID
        -- Lọc ra những dòng có Unit thay đổi
        WHERE d.Unit <> i.Unit
          -- VÀ đã từng được bán
          AND EXISTS (SELECT 1 FROM dbo.SaleDetails sd WHERE sd.ProductID = i.ProductID)
    )
    BEGIN
        -- Báo lỗi và ROLLBACK TRANSACTION
        RAISERROR(N'Không thể thay đổi đơn vị tính của sản phẩm đã có giao dịch bán hàng.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

USE Minimart_SalesDB;
GO

USE Minimart_SalesDB;
GO

CREATE OR ALTER TRIGGER TR_Account_SyncEmployeePosition
ON dbo.Account
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Chỉ thực hiện nếu Role HOẶC EmployeeID bị thay đổi
    IF UPDATE(Role) OR UPDATE(EmployeeID)
    BEGIN
        -- Cập nhật bảng Employees
        UPDATE e
        SET e.Position = i.Role 
        FROM dbo.Employees e
        INNER JOIN inserted i ON e.EmployeeID = i.EmployeeID
        -- FIX: Sử dụng Username làm khóa chính để JOIN giữa inserted và deleted
        INNER JOIN deleted d ON i.Username = d.Username 
        
        -- Chỉ cập nhật nếu Role thay đổi HOẶC EmployeeID thay đổi, VÀ Role mới là manager/saler
        WHERE i.Role IN ('manager', 'saler') 
          AND (i.Role <> d.Role OR i.EmployeeID <> d.EmployeeID);
          
    END
END
GO