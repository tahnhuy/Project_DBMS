-- SQL Script to create missing database objects for Sale_Management
-- Run this script in SQL Server Management Studio

-- 1. Create a simple GetDiscountedPrice function
IF OBJECT_ID('dbo.GetDiscountedPrice', 'FN') IS NOT NULL
BEGIN
    DROP FUNCTION dbo.GetDiscountedPrice;
END
GO

CREATE FUNCTION dbo.GetDiscountedPrice
(
    @ProductID INT,
    @OriginalPrice DECIMAL(18, 2)
)
RETURNS DECIMAL(18, 2)
AS
BEGIN
    -- For now, just return the original price
    -- This can be enhanced later when we know the actual column structure
    RETURN @OriginalPrice;
END
GO

-- 2. Create LowStockProducts view if it doesn't exist
IF OBJECT_ID('LowStockProducts', 'V') IS NOT NULL
BEGIN
    DROP VIEW LowStockProducts;
END
GO

CREATE VIEW LowStockProducts AS
SELECT
    p.ProductID,
    p.ProductName,
    p.StockQuantity,
    p.MinStockLevel,
    p.Unit,
    CASE 
        WHEN p.StockQuantity <= p.MinStockLevel THEN 'Cần nhập hàng'
        WHEN p.StockQuantity <= (p.MinStockLevel * 1.5) THEN 'Sắp hết hàng'
        ELSE 'Đủ hàng'
    END AS StockStatus
FROM
    Products p
WHERE
    p.StockQuantity <= (p.MinStockLevel * 1.5); -- Show products that are low or very low on stock
GO

-- 3. Create SalesSummary view if it doesn't exist
IF OBJECT_ID('SalesSummary', 'V') IS NOT NULL
BEGIN
    DROP VIEW SalesSummary;
END
GO

CREATE VIEW SalesSummary AS
SELECT
    s.SaleID,
    s.SaleDate,
    c.CustomerName,
    s.TotalAmount,
    s.PaymentMethod,
    s.CreatedBy,
    COUNT(sd.SaleDetailID) AS ItemCount
FROM
    Sales s
LEFT JOIN
    Customers c ON s.CustomerID = c.CustomerID
LEFT JOIN
    SaleDetails sd ON s.SaleID = sd.SaleID
GROUP BY
    s.SaleID, s.SaleDate, c.CustomerName, s.TotalAmount, s.PaymentMethod, s.CreatedBy;
GO

-- 5. Verify the objects were created successfully
SELECT 'GetDiscountedPrice Function' AS ObjectName, 'Function' AS ObjectType
WHERE OBJECT_ID('dbo.GetDiscountedPrice', 'FN') IS NOT NULL
UNION ALL
SELECT 'ProductsWithDiscounts View' AS ObjectName, 'View' AS ObjectType
WHERE OBJECT_ID('ProductsWithDiscounts', 'V') IS NOT NULL
UNION ALL
SELECT 'LowStockProducts View' AS ObjectName, 'View' AS ObjectType
WHERE OBJECT_ID('LowStockProducts', 'V') IS NOT NULL
UNION ALL
SELECT 'SalesSummary View' AS ObjectName, 'View' AS ObjectType
WHERE OBJECT_ID('SalesSummary', 'V') IS NOT NULL;

PRINT 'Database objects created successfully!';
