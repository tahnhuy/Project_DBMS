-- Simple script to check what columns exist in your database
-- Run this in SQL Server Management Studio to see your actual table structure

-- Check Products table columns
PRINT '=== PRODUCTS TABLE COLUMNS ===';
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'Products'
ORDER BY ORDINAL_POSITION;

-- Check Discounts table columns
PRINT '=== DISCOUNTS TABLE COLUMNS ===';
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'Discounts'
ORDER BY ORDINAL_POSITION;

-- Check if tables exist
PRINT '=== EXISTING TABLES ===';
SELECT 
    TABLE_NAME,
    TABLE_TYPE
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME IN ('Products', 'Discounts', 'Sales', 'SaleDetails', 'Customers')
ORDER BY TABLE_NAME;

-- Sample data from Products table (first 3 rows)
PRINT '=== SAMPLE PRODUCTS DATA ===';
SELECT TOP 3 * FROM Products;

-- Sample data from Discounts table (first 3 rows) - if it exists
PRINT '=== SAMPLE DISCOUNTS DATA ===';
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Discounts')
BEGIN
    SELECT TOP 3 * FROM Discounts;
END
ELSE
BEGIN
    PRINT 'Discounts table does not exist';
END
