if db_id('Minimart_SalesDB') is null
begin
    create database Minimart_SalesDB;
end
go

use Minimart_SalesDB
go

-- Drop all object/table triggers in current database
declare @sql nvarchar(max) = N'';
select @sql = @sql + N'DROP TRIGGER [' + s.name + N'].[' + t.name + N'];' + CHAR(10)
from sys.triggers t
inner join sys.objects o on t.object_id = o.object_id
inner join sys.schemas s on o.schema_id = s.schema_id
where t.parent_class_desc = 'OBJECT_OR_COLUMN';

if len(@sql) > 0 exec sp_executesql @sql;
go

-- ========== TRIGGER 1: Tự động cập nhật tổng tiền hóa đơn ==========
-- Trigger này sẽ tự động tính tổng tiền khi thêm/sửa/xóa chi tiết hóa đơn
create or alter trigger TR_SaleDetails_UpdateTotalAmount
on dbo.SaleDetails
after insert, update, delete
as
begin
    set nocount on;
    
    -- Tập hợp tất cả SaleID bị ảnh hưởng
    declare @affected_sales table (SaleID int);
    
    -- Lấy SaleID từ bản ghi được insert hoặc update
    if exists(select * from inserted)
    begin
        insert into @affected_sales (SaleID)
        select distinct SaleID from inserted;
    end
    
    -- Lấy SaleID từ bản ghi bị delete
    if exists(select * from deleted)
    begin
        insert into @affected_sales (SaleID)
        select distinct SaleID from deleted
        where SaleID not in (select SaleID from @affected_sales);
    end
    
    -- Cập nhật tổng tiền cho tất cả hóa đơn bị ảnh hưởng
    update s
    set TotalAmount = isnull(
        (select sum(LineTotal) 
         from dbo.SaleDetails sd 
         where sd.SaleID = s.SaleID), 0)
    from dbo.Sales s
    inner join @affected_sales affected on s.SaleID = affected.SaleID;
end
go

-- ========== TRIGGER 2: Kiểm tra và cập nhật số lượng tồn kho ==========
-- Trigger này kiểm tra số lượng tồn kho khi bán hàng và cập nhật số lượng
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

-- ========== TRIGGER 3: Tự động tính LineTotal trong SaleDetails ==========
-- Trigger này đảm bảo LineTotal = Quantity * SalePrice
create or alter trigger TR_SaleDetails_CalculateLineTotal
on dbo.SaleDetails
instead of insert, update
as
begin
    set nocount on;
    
    -- Xử lý INSERT
    if not exists(select * from deleted)
    begin
        insert into dbo.SaleDetails (SaleID, ProductID, Quantity, SalePrice, LineTotal)
        select SaleID, ProductID, Quantity, SalePrice, Quantity * SalePrice
        from inserted;
    end
    -- Xử lý UPDATE  
    else
    begin
        update sd
        set Quantity = i.Quantity,
            SalePrice = i.SalePrice,
            LineTotal = i.Quantity * i.SalePrice
        from dbo.SaleDetails sd
        inner join inserted i on sd.SaleID = i.SaleID and sd.ProductID = i.ProductID;
    end
end
go

-- ========== TRIGGER 4: Cập nhật điểm tích lũy khách hàng ==========
-- Trigger này tự động cộng điểm tích lũy khi khách hàng mua hàng
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

-- ========== TRIGGER 5: Tự động tạo giao dịch khi có bán hàng ==========
-- Trigger này tự động tạo transaction khi có sale mới
create or alter trigger TR_Sales_CreateTransaction
on dbo.Sales
after insert
as
begin
    set nocount on;
    
    -- Tạo transaction cho mỗi sale mới
    insert into dbo.Transactions (TransactionType, Amount, Description, TransactionDate, CreatedBy, ReferenceID, ReferenceType)
    select 
        'income',
        i.TotalAmount,
        N'Thu tiền từ bán hàng - Hóa đơn #' + cast(i.SaleID as nvarchar(20)),
        i.SaleDate,
        'system', -- Có thể cập nhật để lưu user thực tế
        i.SaleID,
        'sale'
    from inserted i
    where i.TotalAmount > 0;
end
go

-- ========== TRIGGER 6: Kiểm tra ràng buộc giảm giá ==========
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

-- ========== TRIGGER 7: Kiểm tra số điện thoại khách hàng ==========
-- Trigger này chuẩn hóa và kiểm tra số điện thoại  
create or alter trigger TR_Customers_ValidatePhone
on dbo.Customers
after insert, update
as
begin
    set nocount on;
    
    -- Kiểm tra định dạng số điện thoại Việt Nam
    if exists(
        select 1 from inserted 
        where Phone is not null 
          and (len(ltrim(rtrim(Phone))) not between 10 and 11
               or ltrim(rtrim(Phone)) not like '0[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]%'
               or ltrim(rtrim(Phone)) like '%[^0-9]%')
    )
    begin
        raiserror(N'Số điện thoại không đúng định dạng! (Phải có 10-11 số và bắt đầu bằng 0)', 16, 1);
        rollback transaction;
        return;
    end
    
    -- Chuẩn hóa số điện thoại (loại bỏ khoảng trắng, ký tự đặc biệt)
    update c
    set Phone = ltrim(rtrim(replace(replace(replace(i.Phone, ' ', ''), '-', ''), '.', '')))
    from dbo.Customers c
    inner join inserted i on c.CustomerID = i.CustomerID;
end
go

-- ========== TRIGGER 8: Audit log cho thay đổi giá sản phẩm ==========
-- Trigger này ghi log khi có thay đổi giá sản phẩm
if not exists (select * from sys.tables where name = 'ProductPriceHistory')
begin
    create table ProductPriceHistory (
        HistoryID int identity(1,1) primary key,
        ProductID int not null,
        OldPrice decimal(18,2),
        NewPrice decimal(18,2),
        ChangeDate datetime not null default(getdate()),
        ChangedBy nvarchar(50),
        ChangeReason nvarchar(255)
    );
end
go

create or alter trigger TR_Products_PriceHistory
on dbo.Products
after update
as
begin
    set nocount on;
    
    -- Chỉ ghi log khi giá thay đổi
    if update(Price)
    begin
        insert into ProductPriceHistory (ProductID, OldPrice, NewPrice, ChangedBy, ChangeReason)
        select 
            i.ProductID,
            d.Price,
            i.Price,
            system_user,
            N'Cập nhật giá sản phẩm'
        from inserted i
        inner join deleted d on i.ProductID = d.ProductID
        where i.Price != d.Price;
    end
end
go

print N'✅ Đã tạo thành công tất cả các trigger cần thiết!'
print N'📋 Danh sách trigger đã tạo:'
print N'   1. TR_SaleDetails_UpdateTotalAmount - Tự động cập nhật tổng tiền hóa đơn'
print N'   2. TR_SaleDetails_UpdateStock - Kiểm tra và cập nhật số lượng tồn kho'  
print N'   3. TR_SaleDetails_CalculateLineTotal - Tự động tính LineTotal'
print N'   4. TR_Sales_UpdateLoyaltyPoints - Cập nhật điểm tích lũy khách hàng'
print N'   5. TR_Sales_CreateTransaction - Tự động tạo giao dịch khi bán hàng'
print N'   6. TR_Discounts_ValidateDiscount - Kiểm tra ràng buộc giảm giá'
print N'   7. TR_Customers_ValidatePhone - Kiểm tra số điện thoại khách hàng'
print N'   8. TR_Products_PriceHistory - Audit log cho thay đổi giá sản phẩm'
go