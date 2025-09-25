if db_id('Minimart_SalesDB') is null
begin
    create database Minimart_SalesDB;
end
go

use Minimart_SalesDB
go

-- Drop all views in current database
declare @sql nvarchar(max) = N'';
select @sql = @sql + N'DROP VIEW [' + s.name + N'].[' + v.name + N'];' + CHAR(10)
from sys.views v
inner join sys.schemas s on s.schema_id = v.schema_id;

if len(@sql) > 0 exec sp_executesql @sql;
go

-- ========== VIEW 1: Products with their discounted prices (đã có từ procedure.sql) ==========
/*create or alter view ProductsWithDiscounts
as
select 
    p.ProductID,
    p.ProductName,
    p.Price as OriginalPrice,
    dbo.GetDiscountedPrice(p.ProductID, p.Price) as DiscountedPrice,
    case 
        when dbo.GetDiscountedPrice(p.ProductID, p.Price) < p.Price then 1 
        else 0 
    end as HasDiscount,
    p.StockQuantity,
    p.Unit,
    d.DiscountType,
    d.DiscountValue,
    d.StartDate as DiscountStartDate,
    d.EndDate as DiscountEndDate
from dbo.Products p
left join dbo.Discounts d on p.ProductID = d.ProductID 
    and d.IsActive = 1 
    and getdate() >= d.StartDate 
    and getdate() <= d.EndDate;
go*/

--select * from ProductsWithDiscounts

-- ========== VIEW 2: Sales Summary with Customer Details ==========
create or alter view SalesSummary
as
select 
    s.SaleID,
    s.SaleDate,
    s.TotalAmount,
    s.PaymentMethod,
    c.CustomerID,
    c.CustomerName,
    c.Phone,
    c.Address,
    c.LoyaltyPoints,
    count(sd.ProductID) as TotalItems,
    sum(sd.Quantity) as TotalQuantity
from dbo.Sales s
left join dbo.Customers c on s.CustomerID = c.CustomerID
left join dbo.SaleDetails sd on s.SaleID = sd.SaleID
group by s.SaleID, s.SaleDate, s.TotalAmount, s.PaymentMethod,
         c.CustomerID, c.CustomerName, c.Phone, c.Address, c.LoyaltyPoints;
go

-- ========== VIEW 3: Product Sales Statistics ==========
/*
create or alter view ProductSalesStats
as
select 
    p.ProductID,
    p.ProductName,
    p.Price,
    p.StockQuantity,
    p.Unit,
    isnull(sum(sd.Quantity), 0) as TotalSold,
    isnull(sum(sd.LineTotal), 0) as TotalRevenue,
    isnull(count(distinct s.SaleID), 0) as NumberOfOrders,
    case 
        when sum(sd.Quantity) > 0 then avg(sd.SalePrice)
        else p.Price 
    end as AverageSellingPrice
from dbo.Products p
left join dbo.SaleDetails sd on p.ProductID = sd.ProductID
left join dbo.Sales s on sd.SaleID = s.SaleID
group by p.ProductID, p.ProductName, p.Price, p.StockQuantity, p.Unit;
go 
*/

-- ========== VIEW 4: Customer Purchase Summary ==========
/*create or alter view CustomerPurchaseSummary
as
select 
    c.CustomerID,
    c.CustomerName,
    c.Phone,
    c.Address,
    c.LoyaltyPoints,
    isnull(count(distinct s.SaleID), 0) as TotalOrders,
    isnull(sum(s.TotalAmount), 0) as TotalSpent,
    isnull(sum(sd.Quantity), 0) as TotalItemsPurchased,
    case 
        when count(distinct s.SaleID) > 0 then avg(s.TotalAmount)
        else 0 
    end as AverageOrderValue,
    max(s.SaleDate) as LastPurchaseDate
from dbo.Customers c
left join dbo.Sales s on c.CustomerID = s.CustomerID
left join dbo.SaleDetails sd on s.SaleID = sd.SaleID
group by c.CustomerID, c.CustomerName, c.Phone, c.Address, c.LoyaltyPoints;
go

use Minimart_SalesDB;
go

select * from CustomerPurchaseSummary
*/

-- ========== VIEW 5: Monthly Sales Report ==========
/*create or alter view MonthlySalesReport
as
select 
    year(s.SaleDate) as SalesYear,
    month(s.SaleDate) as SalesMonth,
    datename(month, s.SaleDate) as MonthName,
    count(distinct s.SaleID) as TotalOrders,
    sum(s.TotalAmount) as TotalRevenue,
    avg(s.TotalAmount) as AverageOrderValue,
    sum(sd.Quantity) as TotalItemsSold,
    count(distinct s.CustomerID) as UniqueCustomers
from dbo.Sales s
inner join dbo.SaleDetails sd on s.SaleID = sd.SaleID
group by year(s.SaleDate), month(s.SaleDate), datename(month, s.SaleDate);
go*/

-- ========== VIEW 6: Daily Sales Report ==========
/*create or alter view DailySalesReport
as
select 
    cast(s.SaleDate as date) as SalesDate,
    count(distinct s.SaleID) as TotalOrders,
    sum(s.TotalAmount) as TotalRevenue,
    avg(s.TotalAmount) as AverageOrderValue,
    sum(sd.Quantity) as TotalItemsSold,
    count(distinct s.CustomerID) as UniqueCustomers,
    count(distinct sd.ProductID) as UniqueProductsSold
from dbo.Sales s
inner join dbo.SaleDetails sd on s.SaleID = sd.SaleID
group by cast(s.SaleDate as date);
go
*/

-- ========== VIEW 7: Low Stock Products ==========
create or alter view LowStockProducts
as
select 
    p.ProductID,
    p.ProductName,
    p.Price,
    p.StockQuantity,
    p.Unit,
    case 
        when p.StockQuantity = 0 then N'Hết hàng'
        when p.StockQuantity <= 5 then N'Rất thấp'
        when p.StockQuantity <= 10 then N'Thấp'
        else N'Bình thường'
    end as StockStatus,
    isnull(sum(sd.Quantity), 0) as TotalSold
from dbo.Products p
left join dbo.SaleDetails sd on p.ProductID = sd.ProductID
left join dbo.Sales s on sd.SaleID = s.SaleID 
    and s.SaleDate >= dateadd(day, -30, getdate())
where p.StockQuantity <= 20
group by p.ProductID, p.ProductName, p.Price, p.StockQuantity, p.Unit;
go


-- ========== VIEW 8: Active Discounts Detail ==========
create or alter view ActiveDiscountsDetail
as
select 
    d.DiscountID,
    d.ProductID,
    p.ProductName,
    p.Price as OriginalPrice,
    d.DiscountType,
    d.DiscountValue,
    case 
        when d.DiscountType = 'percentage' 
        then p.Price * (100 - d.DiscountValue) / 100
        else p.Price - d.DiscountValue
    end as DiscountedPrice,
    case 
        when d.DiscountType = 'percentage' 
        then d.DiscountValue
        else (d.DiscountValue / p.Price) * 100
    end as DiscountPercentage,
    d.StartDate,
    d.EndDate,
    datediff(day, getdate(), d.EndDate) as DaysRemaining,
    d.CreatedBy,
    d.CreatedDate
from dbo.Discounts d
inner join dbo.Products p on d.ProductID = p.ProductID
where d.IsActive = 1 
  and getdate() >= d.StartDate 
  and getdate() <= d.EndDate;
go

-- select * from ActiveDiscountsDetail

-- ========== VIEW 9: Transaction Summary ==========
create or alter view TransactionSummary
as
select 
    t.TransactionID,
    t.TransactionType,
    t.Amount,
    t.Description,
    t.TransactionDate,
    t.CreatedBy,
    t.ReferenceID,
    t.ReferenceType,
    case t.ReferenceType
        when 'sale' then (select concat('HĐ #', s.SaleID, ' - ', c.CustomerName) 
                         from dbo.Sales s 
                         left join dbo.Customers c on s.CustomerID = c.CustomerID 
                         where s.SaleID = t.ReferenceID)
        else 'Giao dịch khác'
    end as ReferenceDescription
from dbo.Transactions t;
go


-- ========== VIEW 10: Account Management Summary ==========
create or alter view AccountSummary
as
select 
    a.Username,
    a.Role,
    a.CreatedDate,
    c.CustomerID,
    c.CustomerName,
    c.Phone,
    c.LoyaltyPoints,
    isnull(sales_count.TotalOrders, 0) as TotalOrders,
    isnull(sales_count.TotalSpent, 0) as TotalSpent,
    isnull(trans_count.TotalTransactions, 0) as TotalTransactions
from dbo.Account a
left join dbo.Customers c on a.CustomerID = c.CustomerID
left join (
    select s.CustomerID, count(*) as TotalOrders, sum(s.TotalAmount) as TotalSpent
    from dbo.Sales s 
    group by s.CustomerID
) sales_count on c.CustomerID = sales_count.CustomerID
left join (
    select t.CreatedBy, count(*) as TotalTransactions
    from dbo.Transactions t 
    group by t.CreatedBy
) trans_count on a.Username = trans_count.CreatedBy;
go

select * from AccountSummary

print N'✅ Đã tạo thành công tất cả các view cần thiết!'
print N'📋 Danh sách view đã tạo:'
print N'   1. ProductsWithDiscounts - Sản phẩm với giá giảm'
print N'   2. SalesSummary - Tóm tắt hóa đơn với thông tin khách hàng'
print N'   3. ProductSalesStats - Thống kê bán hàng theo sản phẩm'
print N'   4. CustomerPurchaseSummary - Tóm tắt mua hàng của khách hàng'
print N'   5. MonthlySalesReport - Báo cáo bán hàng theo tháng'
print N'   6. DailySalesReport - Báo cáo bán hàng theo ngày'
print N'   7. LowStockProducts - Sản phẩm sắp hết hàng'
print N'   8. ActiveDiscountsDetail - Chi tiết chương trình giảm giá'
print N'   9. TransactionSummary - Tóm tắt giao dịch'
print N'   10. AccountSummary - Tóm tắt tài khoản'
go
