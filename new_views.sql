-- =============================================
-- NEW VIEWS - Only Active Usage Objects
-- Based on FunctionSummary.md analysis
-- Total: 2 Database Views
-- =============================================

USE Minimart_SalesDB;
GO

-- ========== DATABASE VIEWS (2 views) ==========

-- 1. LowStockProducts - Products with low stock
CREATE OR ALTER VIEW LowStockProducts
AS
SELECT 
    p.ProductID,
    p.ProductName,
    p.Price,
    p.StockQuantity,
    p.Unit,
    CASE 
        WHEN p.StockQuantity = 0 THEN N'Hết hàng'
        WHEN p.StockQuantity <= 5 THEN N'Rất thấp'
        WHEN p.StockQuantity <= 10 THEN N'Thấp'
        ELSE N'Bình thường'
    END AS StockStatus,
    ISNULL(SUM(sd.Quantity), 0) AS TotalSold
FROM dbo.Products p
LEFT JOIN dbo.SaleDetails sd ON p.ProductID = sd.ProductID
LEFT JOIN dbo.Sales s ON sd.SaleID = s.SaleID 
    AND s.SaleDate >= DATEADD(DAY, -30, GETDATE())
WHERE p.StockQuantity <= 20
GROUP BY p.ProductID, p.ProductName, p.Price, p.StockQuantity, p.Unit;
GO

-- 2. SalesSummary - Sales summary with customer details
CREATE OR ALTER VIEW SalesSummary
AS
SELECT 
    s.SaleID,
    s.SaleDate,
    s.TotalAmount,
    s.PaymentMethod,
    c.CustomerID,
    c.CustomerName,
    c.Phone,
    c.Address,
    c.LoyaltyPoints,
    COUNT(sd.ProductID) AS TotalItems,
    SUM(sd.Quantity) AS TotalQuantity
FROM dbo.Sales s
LEFT JOIN dbo.Customers c ON s.CustomerID = c.CustomerID
LEFT JOIN dbo.SaleDetails sd ON s.SaleID = sd.SaleID
GROUP BY s.SaleID, s.SaleDate, s.TotalAmount, s.PaymentMethod,
         c.CustomerID, c.CustomerName, c.Phone, c.Address, c.LoyaltyPoints;
GO

PRINT N'✅ Đã tạo thành công 2 views cần thiết!'
PRINT N'📋 Tổng cộng:'
PRINT N'   - LowStockProducts - Sản phẩm sắp hết hàng'
PRINT N'   - SalesSummary - Tóm tắt hóa đơn với thông tin khách hàng'
GO
