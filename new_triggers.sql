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
