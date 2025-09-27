-- =============================================
-- NEW FUNCTIONS - Only Active Usage Objects
-- Based on FunctionSummary.md analysis
-- Total: 4 Scalar Functions + 1 Table-Valued Function
-- =============================================

USE Minimart_SalesDB;
GO

-- ========== SCALAR FUNCTIONS (4 functions) ==========

-- 1. GetDailyRevenue - Calculate daily revenue
CREATE OR ALTER FUNCTION dbo.GetDailyRevenue(@Date DATE)
RETURNS DECIMAL(18, 2)
AS
BEGIN
    DECLARE @Revenue DECIMAL(18, 2) = 0;
    
    SELECT @Revenue = ISNULL(SUM(TotalAmount), 0)
    FROM dbo.Sales
    WHERE CAST(SaleDate AS DATE) = @Date;
    
    RETURN @Revenue;
END
GO

-- 2. GetMonthlyRevenue - Calculate monthly revenue
CREATE OR ALTER FUNCTION dbo.GetMonthlyRevenue(@Year INT, @Month INT)
RETURNS DECIMAL(18, 2)
AS
BEGIN
    DECLARE @Revenue DECIMAL(18, 2) = 0;
    
    SELECT @Revenue = ISNULL(SUM(TotalAmount), 0)
    FROM dbo.Sales
    WHERE YEAR(SaleDate) = @Year 
      AND MONTH(SaleDate) = @Month;
    
    RETURN @Revenue;
END
GO

-- 3. GetDiscountedPrice - Calculate discounted price for products
CREATE OR ALTER FUNCTION dbo.GetDiscountedPrice(
    @ProductID INT,
    @OriginalPrice DECIMAL(18, 2)
)
RETURNS DECIMAL(18, 2)
AS
BEGIN
    DECLARE @DiscountedPrice DECIMAL(18, 2) = @OriginalPrice;
    DECLARE @DiscountType NVARCHAR(20);
    DECLARE @DiscountValue DECIMAL(18, 2);
    
    -- Get active discount for the product
    SELECT TOP 1 @DiscountType = DiscountType, @DiscountValue = DiscountValue
    FROM dbo.Discounts
    WHERE ProductID = @ProductID
      AND IsActive = 1
      AND GETDATE() >= StartDate
      AND GETDATE() <= EndDate
    ORDER BY CreatedDate DESC; -- Get the most recent discount if multiple exist
    
    IF @DiscountType IS NOT NULL
    BEGIN
        IF @DiscountType = 'percentage'
        BEGIN
            SET @DiscountedPrice = @OriginalPrice * (100 - @DiscountValue) / 100;
        END
        ELSE IF @DiscountType = 'fixed'
        BEGIN
            SET @DiscountedPrice = @OriginalPrice - @DiscountValue;
            IF @DiscountedPrice < 0 SET @DiscountedPrice = 0;
        END
    END
    
    RETURN @DiscountedPrice;
END
GO

-- 4. IsStockAvailable - Check stock availability
CREATE OR ALTER FUNCTION dbo.IsStockAvailable(@ProductID INT, @RequiredQuantity INT)
RETURNS BIT
AS
BEGIN
    DECLARE @Available BIT = 0;
    DECLARE @CurrentStock INT;
    
    SELECT @CurrentStock = StockQuantity 
    FROM dbo.Products 
    WHERE ProductID = @ProductID;
    
    IF @CurrentStock >= @RequiredQuantity
        SET @Available = 1;
    
    RETURN @Available;
END
GO

-- ========== TABLE-VALUED FUNCTIONS (1 function) ==========

-- 5. GetProductByName - Search products by name
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
            dbo.GetDiscountedPrice(p.ProductID, p.Price) AS DiscountedPrice,
            CASE WHEN dbo.GetDiscountedPrice(p.ProductID, p.Price) < p.Price THEN 1 ELSE 0 END AS HasDiscount
        FROM dbo.Products p
        WHERE p.ProductName COLLATE Vietnamese_CI_AI LIKE N'%' + @Name + '%'
    );
GO

PRINT N'✅ Đã tạo thành công 5 functions cần thiết!'
PRINT N'📋 Tổng cộng:'
PRINT N'   - Scalar Functions: 4'
PRINT N'     * GetDailyRevenue - Tính doanh thu theo ngày'
PRINT N'     * GetMonthlyRevenue - Tính doanh thu theo tháng'
PRINT N'     * GetDiscountedPrice - Tính giá sau giảm giá'
PRINT N'     * IsStockAvailable - Kiểm tra tồn kho'
PRINT N'   - Table-Valued Functions: 1'
PRINT N'     * GetProductByName - Tìm sản phẩm theo tên'
GO
