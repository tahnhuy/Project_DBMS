if db_id('Minimart_SalesDB') is null
begin
    create database Minimart_SalesDB;
end
go

use Minimart_SalesDB
go

-- Drop all functions (scalar, inline TVF, multi-statement TVF)
declare @sql nvarchar(max) = N'';
select @sql = @sql + N'DROP FUNCTION [' + s.name + N'].[' + o.name + N'];' + CHAR(10)
from sys.objects o
inner join sys.schemas s on s.schema_id = o.schema_id
where o.type in ('FN','IF','TF');

if len(@sql) > 0 exec sp_executesql @sql;
go

-- ========== FUNCTION 1: Tìm sản phẩm theo tên (đã có, cải tiến) ==========
create or alter function dbo.GetProductByName(@Name nvarchar(100))
returns table
as
    return (
        select 
            p.ProductID,
            p.ProductName,
            p.Price,
            p.StockQuantity,
            p.Unit,
            dbo.GetDiscountedPrice(p.ProductID, p.Price) as DiscountedPrice,
            case when dbo.GetDiscountedPrice(p.ProductID, p.Price) < p.Price then 1 else 0 end as HasDiscount
        from dbo.Products p
        where p.ProductName collate Vietnamese_CI_AI like N'%' + @Name + '%'
    );
go

-- ========== FUNCTION 2: Tìm sản phẩm theo ID (đã có, cải tiến) ==========
create or alter function dbo.GetProductByID(@id int)
returns table 
as
return
(
    select 
        p.ProductID,
        p.ProductName,
        p.Price,
        p.StockQuantity,
        p.Unit,
        dbo.GetDiscountedPrice(p.ProductID, p.Price) as DiscountedPrice,
        case when dbo.GetDiscountedPrice(p.ProductID, p.Price) < p.Price then 1 else 0 end as HasDiscount
    from dbo.Products p
    where p.ProductID = @id
);
go

-- ========== FUNCTION 3: Tính giá sau giảm giá (đã có từ procedure.sql) ==========
create or alter function dbo.GetDiscountedPrice(
    @ProductID int,
    @OriginalPrice decimal(18, 2)
)
returns decimal(18, 2)
as
begin
    declare @DiscountedPrice decimal(18, 2) = @OriginalPrice;
    declare @DiscountType nvarchar(20);
    declare @DiscountValue decimal(18, 2);
    
    -- Get active discount for the product
    select top 1 @DiscountType = DiscountType, @DiscountValue = DiscountValue
    from dbo.Discounts
    where ProductID = @ProductID
      and IsActive = 1
      and getdate() >= StartDate
      and getdate() <= EndDate
    order by CreatedDate desc; -- Get the most recent discount if multiple exist
    
    if @DiscountType is not null
    begin
        if @DiscountType = 'percentage'
        begin
            set @DiscountedPrice = @OriginalPrice * (100 - @DiscountValue) / 100;
        end
        else if @DiscountType = 'fixed'
        begin
            set @DiscountedPrice = @OriginalPrice - @DiscountValue;
            if @DiscountedPrice < 0 set @DiscountedPrice = 0;
        end
    end
    
    return @DiscountedPrice;
end
go

-- ========== FUNCTION 4: Tính tổng doanh thu theo ngày ==========
create or alter function dbo.GetDailyRevenue(@Date date)
returns decimal(18, 2)
as
begin
    declare @Revenue decimal(18, 2) = 0;
    
    select @Revenue = isnull(sum(TotalAmount), 0)
    from dbo.Sales
    where cast(SaleDate as date) = @Date;
    
    return @Revenue;
end
go

-- ========== FUNCTION 5: Tính tổng doanh thu theo tháng ==========
create or alter function dbo.GetMonthlyRevenue(@Year int, @Month int)
returns decimal(18, 2)
as
begin
    declare @Revenue decimal(18, 2) = 0;
    
    select @Revenue = isnull(sum(TotalAmount), 0)
    from dbo.Sales
    where year(SaleDate) = @Year 
      and month(SaleDate) = @Month;
    
    return @Revenue;
end
go

-- ========== FUNCTION 6: Lấy top sản phẩm bán chạy ==========
create or alter function dbo.GetTopSellingProducts(@TopCount int = 10)
returns table
as
return
(
    select top (@TopCount)
        p.ProductID,
        p.ProductName,
        p.Price,
        p.Unit,
        sum(sd.Quantity) as TotalSold,
        sum(sd.LineTotal) as TotalRevenue
    from dbo.Products p
    inner join dbo.SaleDetails sd on p.ProductID = sd.ProductID
    group by p.ProductID, p.ProductName, p.Price, p.Unit
    order by sum(sd.Quantity) desc
);
go

-- ========== FUNCTION 7: Kiểm tra số lượng tồn kho đủ bán không ==========
create or alter function dbo.IsStockAvailable(@ProductID int, @RequiredQuantity int)
returns bit
as
begin
    declare @Available bit = 0;
    declare @CurrentStock int;
    
    select @CurrentStock = StockQuantity 
    from dbo.Products 
    where ProductID = @ProductID;
    
    if @CurrentStock >= @RequiredQuantity
        set @Available = 1;
    
    return @Available;
end
go

-- ========== FUNCTION 8: Tính điểm tích lũy từ số tiền ==========
create or alter function dbo.CalculateLoyaltyPoints(@Amount decimal(18, 2))
returns int
as
begin
    -- Quy đổi: 10,000 VND = 1 điểm
    return cast(@Amount / 10000 as int);
end
go

-- ========== FUNCTION 9: Lấy lịch sử mua hàng của khách hàng ==========
create or alter function dbo.GetCustomerPurchaseHistory(@CustomerID int)
returns table
as
return
(
    select 
        s.SaleID,
        s.SaleDate,
        s.TotalAmount,
        s.PaymentMethod,
        sd.ProductID,
        p.ProductName,
        p.Unit,
        sd.Quantity,
        sd.SalePrice,
        sd.LineTotal
    from dbo.Sales s
    inner join dbo.SaleDetails sd on s.SaleID = sd.SaleID
    inner join dbo.Products p on sd.ProductID = p.ProductID
    where s.CustomerID = @CustomerID
);
go

-- ========== FUNCTION 10: Tìm kiếm khách hàng đa điều kiện ==========
create or alter function dbo.SearchCustomers(@SearchTerm nvarchar(100))
returns table
as
return
(
    select 
        CustomerID,
        CustomerName,
        Phone,
        Address,
        LoyaltyPoints
    from dbo.Customers
    where CustomerName collate Vietnamese_CI_AI like N'%' + @SearchTerm + '%'
       or Phone like '%' + @SearchTerm + '%'
       or Address collate Vietnamese_CI_AI like N'%' + @SearchTerm + '%'
);
go

-- ========== FUNCTION 11: Lấy báo cáo doanh thu theo sản phẩm ==========
create or alter function dbo.GetProductRevenueReport(@StartDate datetime, @EndDate datetime)
returns table
as
return
(
    select 
        p.ProductID,
        p.ProductName,
        p.Unit,
        sum(sd.Quantity) as TotalQuantitySold,
        sum(sd.LineTotal) as TotalRevenue,
        avg(sd.SalePrice) as AveragePrice,
        count(distinct s.SaleID) as NumberOfOrders
    from dbo.Products p
    inner join dbo.SaleDetails sd on p.ProductID = sd.ProductID
    inner join dbo.Sales s on sd.SaleID = s.SaleID
    where s.SaleDate between @StartDate and @EndDate
    group by p.ProductID, p.ProductName, p.Unit
);
go

-- ========== FUNCTION 12: Tính tỷ lệ giảm giá ==========
create or alter function dbo.CalculateDiscountPercentage(@OriginalPrice decimal(18,2), @DiscountedPrice decimal(18,2))
returns decimal(5,2)
as
begin
    declare @DiscountPercentage decimal(5,2) = 0;
    
    if @OriginalPrice > 0 and @DiscountedPrice < @OriginalPrice
    begin
        set @DiscountPercentage = ((@OriginalPrice - @DiscountedPrice) / @OriginalPrice) * 100;
    end
    
    return @DiscountPercentage;
end
go

-- ========== FUNCTION 13: Validate số điện thoại Việt Nam ==========
create or alter function dbo.IsValidVietnamesePhone(@Phone varchar(20))
returns bit
as
begin
    declare @IsValid bit = 0;
    
    -- Kiểm tra định dạng số điện thoại VN: 10-11 số, bắt đầu bằng 0
    if @Phone is not null 
       and len(@Phone) between 10 and 11
       and @Phone like '0[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]%'
       and @Phone not like '%[^0-9]%'
    begin
        set @IsValid = 1;
    end
    
    return @IsValid;
end
go

-- ========== FUNCTION 14: Lấy thống kê tổng quan ==========
create or alter function dbo.GetDashboardStats()
returns table
as
return
(
    select 
        (select count(*) from dbo.Products) as TotalProducts,
        (select count(*) from dbo.Customers) as TotalCustomers,
        (select count(*) from dbo.Sales where cast(SaleDate as date) = cast(getdate() as date)) as TodaySales,
        (select isnull(sum(TotalAmount), 0) from dbo.Sales where cast(SaleDate as date) = cast(getdate() as date)) as TodayRevenue,
        (select count(*) from dbo.Products where StockQuantity < 10) as LowStockProducts,
        (select count(*) from dbo.Discounts where IsActive = 1 and getdate() between StartDate and EndDate) as ActiveDiscounts
);
go

-- ========== FUNCTION 15: Format tiền Việt Nam ==========
create or alter function dbo.FormatVietnamMoney(@Amount decimal(18,2))
returns nvarchar(50)
as
begin
    declare @FormattedAmount nvarchar(50);
    
    -- Định dạng số với dấu phẩy ngăn cách hàng nghìn
    set @FormattedAmount = format(@Amount, 'N0', 'vi-VN') + N' ₫';
    
    return @FormattedAmount;
end
go

-- ========== FUNCTION 16: Tính tổng chi phí theo loại giao dịch ==========
create or alter function dbo.GetExpenseByType(@TransactionType nvarchar(20), @StartDate datetime = null, @EndDate datetime = null)
returns decimal(18,2)
as
begin
    declare @TotalExpense decimal(18,2) = 0;
    
    if @StartDate is null set @StartDate = '1900-01-01';
    if @EndDate is null set @EndDate = getdate();
    
    select @TotalExpense = isnull(sum(Amount), 0)
    from dbo.Transactions
    where TransactionType = @TransactionType
      and TransactionDate between @StartDate and @EndDate;
    
    return @TotalExpense;
end
go

print N'✅ Đã tạo thành công tất cả các function cần thiết!'
print N'📋 Danh sách function đã tạo:'
print N'   1. GetProductByName - Tìm sản phẩm theo tên (có giảm giá)'
print N'   2. GetProductByID - Tìm sản phẩm theo ID (có giảm giá)'
print N'   3. GetDiscountedPrice - Tính giá sau giảm giá'
print N'   4. GetDailyRevenue - Tính doanh thu theo ngày'
print N'   5. GetMonthlyRevenue - Tính doanh thu theo tháng'
print N'   6. GetTopSellingProducts - Top sản phẩm bán chạy'
print N'   7. IsStockAvailable - Kiểm tra tồn kho'
print N'   8. CalculateLoyaltyPoints - Tính điểm tích lũy'
print N'   9. GetCustomerPurchaseHistory - Lịch sử mua hàng'
print N'   10. SearchCustomers - Tìm kiếm khách hàng'
print N'   11. GetProductRevenueReport - Báo cáo doanh thu sản phẩm'
print N'   12. CalculateDiscountPercentage - Tính tỷ lệ giảm giá'
print N'   13. IsValidVietnamesePhone - Validate SĐT Việt Nam'
print N'   14. GetDashboardStats - Thống kê tổng quan'
print N'   15. FormatVietnamMoney - Format tiền tệ VN'
print N'   16. GetExpenseByType - Tính tổng chi phí theo loại'
go

-- ========== CÁC FUNCTION CẦN THIẾT CHO PRODUCTREPOSITORY ==========
-- Chạy file này trong SQL Server Management Studio hoặc sqlcmd

USE MInimart_SalesDB;
GO

-- Function 1: GetDiscountedPrice - Tính giá sau giảm giá
CREATE OR ALTER FUNCTION dbo.GetDiscountedPrice(@ProductID INT, @OriginalPrice DECIMAL(18,2))
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @DiscountedPrice DECIMAL(18,2) = @OriginalPrice;
    
    IF EXISTS (
        SELECT 1 FROM Discounts 
        WHERE ProductID = @ProductID 
        AND GETDATE() BETWEEN StartDate AND EndDate
    )
    BEGIN
        SELECT @DiscountedPrice = 
            CASE 
                WHEN DiscountType = 'percentage' 
                THEN @OriginalPrice * (1 - DiscountValue/100)
                ELSE @OriginalPrice - DiscountValue
            END
        FROM Discounts 
        WHERE ProductID = @ProductID 
        AND GETDATE() BETWEEN StartDate AND EndDate;
    END
    
    RETURN @DiscountedPrice;
END;
GO

-- Function 2: GetProductByName - Tìm sản phẩm theo tên
CREATE OR ALTER FUNCTION dbo.GetProductByName(@Name NVARCHAR(100))
RETURNS TABLE
AS
RETURN (
    SELECT 
        p.ProductID,
        p.ProductName,
        p.Price,
        p.StockQuantity,
        p.Unit,
        dbo.GetDiscountedPrice(p.ProductID, p.Price) as DiscountedPrice,
        CASE WHEN dbo.GetDiscountedPrice(p.ProductID, p.Price) < p.Price THEN 1 ELSE 0 END as HasDiscount
    FROM dbo.Products p
    WHERE p.ProductName LIKE N'%' + @Name + '%'
);
GO

-- Function 3: GetProductByID - Tìm sản phẩm theo ID
CREATE OR ALTER FUNCTION dbo.GetProductByID(@ID INT)
RETURNS TABLE 
AS
RETURN (
    SELECT 
        p.ProductID,
        p.ProductName,
        p.Price,
        p.StockQuantity,
        p.Unit,
        dbo.GetDiscountedPrice(p.ProductID, p.Price) as DiscountedPrice,
        CASE WHEN dbo.GetDiscountedPrice(p.ProductID, p.Price) < p.Price THEN 1 ELSE 0 END as HasDiscount
    FROM dbo.Products p
    WHERE p.ProductID = @ID
);
GO

-- Function 4: IsStockAvailable - Kiểm tra tồn kho
CREATE OR ALTER FUNCTION dbo.IsStockAvailable(@ProductID INT, @RequiredQuantity INT)
RETURNS BIT
AS
BEGIN
    DECLARE @Available BIT = 0;
    
    IF EXISTS (
        SELECT 1 FROM Products 
        WHERE ProductID = @ProductID 
        AND StockQuantity >= @RequiredQuantity
    )
    BEGIN
        SET @Available = 1;
    END
    
    RETURN @Available;
END;
GO

-- Function 5: GetTopSellingProducts - Top sản phẩm bán chạy
CREATE OR ALTER FUNCTION dbo.GetTopSellingProducts(@TopCount INT)
RETURNS TABLE
AS
RETURN (
    SELECT TOP (@TopCount)
        p.ProductID,
        p.ProductName,
        p.Price,
        p.StockQuantity,
        p.Unit,
        ISNULL(SUM(sd.Quantity), 0) as TotalSold
    FROM Products p
    LEFT JOIN SaleDetails sd ON p.ProductID = sd.ProductID
    LEFT JOIN Sales s ON sd.SaleID = s.SaleID
    GROUP BY p.ProductID, p.ProductName, p.Price, p.StockQuantity, p.Unit
    ORDER BY TotalSold DESC
);
GO

-- Function 6: GetProductRevenueReport - Báo cáo doanh thu sản phẩm
/*CREATE OR ALTER FUNCTION dbo.GetProductRevenueReport(@StartDate DATETIME, @EndDate DATETIME)
RETURNS TABLE
AS
RETURN (
    SELECT 
        p.ProductID,
        p.ProductName,
        p.Price,
        SUM(sd.Quantity) as TotalSold,
        SUM(sd.Quantity * sd.Price) as TotalRevenue
    FROM Products p
    INNER JOIN SaleDetails sd ON p.ProductID = sd.ProductID
    INNER JOIN Sales s ON sd.SaleID = s.SaleID
    WHERE s.SaleDate BETWEEN @StartDate AND @EndDate
    GROUP BY p.ProductID, p.ProductName, p.Price
);
GO */

PRINT 'Đã tạo thành công tất cả các function cần thiết!';
PRINT 'Danh sách function đã tạo:';
PRINT '   1. GetDiscountedPrice - Tính giá sau giảm giá';
PRINT '   2. GetProductByName - Tìm sản phẩm theo tên';
PRINT '   3. GetProductByID - Tìm sản phẩm theo ID';
PRINT '   4. IsStockAvailable - Kiểm tra tồn kho';
PRINT '   5. GetTopSellingProducts - Top sản phẩm bán chạy';
PRINT '   6. GetProductRevenueReport - Báo cáo doanh thu sản phẩm';
