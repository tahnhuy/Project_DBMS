/* ===========================================================
   FILE: final_functions.sql
   DB  : Minimart_SalesDB
   DESC: 4 Scalar Functions + 11 TVFs (đã refactor)
   =========================================================== */
USE Minimart_SalesDB;
GO

-- Password validation function
USE Minimart_SalesDB;
GO

CREATE OR ALTER FUNCTION dbo.ValidatePassword
(
    @Password NVARCHAR(255)
)
RETURNS BIT
AS
BEGIN
    DECLARE @IsValid BIT;

    -- Kiểm tra điều kiện hợp lệ: độ dài phải từ 6 đến 255 ký tự
    IF LEN(@Password) >= 6 AND LEN(@Password) <= 255
        SET @IsValid = 1; -- Hợp lệ
    ELSE
        SET @IsValid = 0; -- Không hợp lệ

    RETURN @IsValid;
END
GO

/* ============================
   SCALAR FUNCTIONS (4)
   ============================ */

-- 1) GetDailyRevenue(@Date)
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

-- 2) GetMonthlyRevenue(@Year, @Month)
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

-- 3) GetDiscountedPrice(@ProductID, @OriginalPrice)
CREATE OR ALTER FUNCTION dbo.GetDiscountedPrice
(
    @ProductID INT,
    @OriginalPrice DECIMAL(18, 2)
)
RETURNS DECIMAL(18, 2)
AS
BEGIN
    DECLARE @DiscountedPrice DECIMAL(18, 2) = @OriginalPrice;
    DECLARE @DiscountType NVARCHAR(20);
    DECLARE @DiscountValue DECIMAL(18, 2);

    -- Sử dụng READ UNCOMMITTED để tránh lock khi đọc dữ liệu discount
    SELECT TOP 1
        @DiscountType = DiscountType,
        @DiscountValue = DiscountValue
    FROM dbo.Discounts WITH (READUNCOMMITTED)
    WHERE ProductID = @ProductID
      AND IsActive = 1
      AND GETDATE() BETWEEN StartDate AND EndDate
    ORDER BY CreatedDate DESC;

    IF @DiscountType IS NOT NULL
    BEGIN
        IF @DiscountType = N'percentage'
            SET @DiscountedPrice = @OriginalPrice * (100 - @DiscountValue) / 100;
        ELSE IF @DiscountType = N'fixed'
        BEGIN
            SET @DiscountedPrice = @OriginalPrice - @DiscountValue;
            IF @DiscountedPrice < 0 SET @DiscountedPrice = 0;
        END
    END

    RETURN @DiscountedPrice;
END
GO

-- 4) IsStockAvailable(@ProductID, @RequiredQuantity)
CREATE OR ALTER FUNCTION dbo.IsStockAvailable(@ProductID INT, @RequiredQuantity INT)
RETURNS BIT
AS
BEGIN
    DECLARE @Available BIT = 0;
    DECLARE @CurrentStock INT;

    -- Sử dụng READ UNCOMMITTED để tránh lock khi kiểm tra stock
    SELECT @CurrentStock = StockQuantity
    FROM dbo.Products WITH (READUNCOMMITTED)
    WHERE ProductID = @ProductID;

    IF @CurrentStock IS NOT NULL AND @CurrentStock >= @RequiredQuantity
        SET @Available = 1;

    RETURN @Available;
END
GO

USE Minimart_SalesDB;
GO

-- 1. Hàm GetSalesGrowthRate_Monthly
CREATE OR ALTER FUNCTION dbo.GetSalesGrowthRate_Monthly
(
    @CP_Year INT,   -- Năm của kỳ hiện tại
    @CP_Month INT,  -- Tháng của kỳ hiện tại
    @PP_Year INT,   -- Năm của kỳ gốc (thường là tháng trước)
    @PP_Month INT   -- Tháng của kỳ gốc
)
RETURNS DECIMAL(18, 2)
AS
BEGIN
    DECLARE @CP_Revenue DECIMAL(18, 2);
    DECLARE @PP_Revenue DECIMAL(18, 2);
    DECLARE @GrowthRate DECIMAL(18, 2) = 0.0;

    -- Lấy doanh thu Kỳ Hiện Tại (CP)
    SET @CP_Revenue = dbo.GetMonthlyRevenue(@CP_Year, @CP_Month);

    -- Lấy doanh thu Kỳ Gốc (PP)
    SET @PP_Revenue = dbo.GetMonthlyRevenue(@PP_Year, @PP_Month);

    -- Tính toán tỷ lệ tăng trưởng
    IF @PP_Revenue > 0
        SET @GrowthRate = (@CP_Revenue - @PP_Revenue) / @PP_Revenue;
    ELSE IF @CP_Revenue > 0 AND @PP_Revenue = 0
        -- Trường hợp tăng trưởng vô hạn (từ 0 lên một con số dương)
        SET @GrowthRate = 999.99; 
    
    RETURN @GrowthRate;
END
GO

/* ============================
   TABLE-VALUED FUNCTIONS (11)
   ============================ */

-- PRODUCTS
-- 5) fnProducts_All()
CREATE OR ALTER FUNCTION dbo.fnProducts_All()
RETURNS TABLE
AS
RETURN
(
    SELECT ProductID, ProductName, Price, StockQuantity, Unit
    FROM dbo.Products
);
GO

-- 6) fnProducts_ByID(@ProductID)
CREATE OR ALTER FUNCTION dbo.fnProducts_ByID(@ProductID INT)
RETURNS TABLE
AS
RETURN
(
    SELECT
        p.ProductID,
        p.ProductName,
        p.Price,
        p.StockQuantity,
        p.Unit,
        dbo.GetDiscountedPrice(p.ProductID, p.Price) AS DiscountedPrice,
        CASE WHEN dbo.GetDiscountedPrice(p.ProductID, p.Price) < p.Price THEN 1 ELSE 0 END AS HasDiscount
    FROM dbo.Products p
    WHERE p.ProductID = @ProductID
);
GO


-- CUSTOMERS
-- 7) fnCustomers_All()
CREATE OR ALTER FUNCTION dbo.fnCustomers_All()
RETURNS TABLE
AS
RETURN
(
    SELECT CustomerID, CustomerName, Phone, Address, LoyaltyPoints
    FROM dbo.Customers
);
GO


-- 8) fnCustomers_ByName(@CustomerName)
CREATE OR ALTER FUNCTION dbo.fnCustomers_ByName(@CustomerName NVARCHAR(100))
RETURNS TABLE
AS
RETURN
(
    SELECT *
    FROM dbo.Customers
    -- Nếu cần bỏ dấu/không phân biệt dấu, dùng collation phù hợp tại đây:
    -- WHERE CustomerName COLLATE Vietnamese_CI_AI LIKE N'%' + @CustomerName + N'%'
	WHERE CustomerName COLLATE Vietnamese_CI_AI LIKE N'%' + @CustomerName + N'%'
);
GO

-- 9) fnCustomers_ByID(@CustomerID)
CREATE OR ALTER FUNCTION dbo.fnCustomers_ByID(@CustomerID INT)
RETURNS TABLE
AS
RETURN
(
    SELECT *
    FROM dbo.Customers
    WHERE CustomerID = @CustomerID
);
GO


-- SALES
-- 10) fnSales_ByID(@SaleID)
CREATE OR ALTER FUNCTION dbo.fnSales_ByID(@SaleID INT)
RETURNS TABLE
AS
RETURN
(
    SELECT
        s.SaleID,
        s.CustomerID,
        c.CustomerName,
        s.SaleDate,
        s.TotalAmount,
        s.PaymentMethod
    FROM dbo.Sales s
    LEFT JOIN dbo.Customers c ON s.CustomerID = c.CustomerID
    WHERE s.SaleID = @SaleID
);
GO

-- 11) fnSaleDetails_BySaleID(@SaleID)
CREATE OR ALTER FUNCTION dbo.fnSaleDetails_BySaleID(@SaleID INT)
RETURNS TABLE
AS
RETURN
(
    SELECT
        sd.SaleID,
        sd.ProductID,
        p.ProductName,
        p.Unit,
        sd.Quantity,
        sd.SalePrice,
        sd.LineTotal
    FROM dbo.SaleDetails sd
    INNER JOIN dbo.Products p ON sd.ProductID = p.ProductID
    WHERE sd.SaleID = @SaleID
);
GO


-- DISCOUNTS
-- 12) fnDiscounts_Active()
CREATE OR ALTER FUNCTION dbo.fnDiscounts_Active()
RETURNS TABLE
AS
RETURN
(
    SELECT
        d.DiscountID,
        d.ProductID,
        p.ProductName,
        d.DiscountType,
        d.DiscountValue,
        d.StartDate,
        d.EndDate,
        d.IsActive,
        d.CreatedDate,
        d.CreatedBy
    FROM dbo.Discounts d
    INNER JOIN dbo.Products p ON p.ProductID = d.ProductID
    WHERE d.IsActive = 1
      AND GETDATE() BETWEEN d.StartDate AND d.EndDate
);
GO

-- 13) fnDiscounts_ByProduct(@ProductID)
CREATE OR ALTER FUNCTION dbo.fnDiscounts_ByProduct(@ProductID INT)
RETURNS TABLE
AS
RETURN
(
    SELECT
        d.DiscountID,
        d.ProductID,
        p.ProductName,
        d.DiscountType,
        d.DiscountValue,
        d.StartDate,
        d.EndDate,
        d.IsActive,
        d.CreatedDate,
        d.CreatedBy
    FROM dbo.Discounts d
    INNER JOIN dbo.Products p ON p.ProductID = d.ProductID
    WHERE d.ProductID = @ProductID
);
GO


-- ACCOUNTS / AUTH
-- 14) fnAuth_User(@Username, @Password)
CREATE OR ALTER FUNCTION dbo.fnAuth_User
(
    @Username NVARCHAR(50),
    @Password NVARCHAR(255)
)
RETURNS TABLE
AS
RETURN
(
    SELECT Username, Role
    FROM dbo.Account
    WHERE Username = @Username
      AND Password = @Password
);
GO

CREATE OR ALTER FUNCTION dbo.fnAccounts_All()
RETURNS TABLE
AS
RETURN
(
    SELECT
        a.Username,
        a.Role,
        a.CreatedDate,
        a.CustomerID,
        a.EmployeeID,
        ISNULL(e.EmployeeName, N'Không xác định') AS FullName,
        ISNULL(e.Phone, N'Không có')               AS Phone,
        ISNULL(e.Address, N'Không có')             AS Address,
        ISNULL(e.Position, N'Không xác định')      AS Position
    FROM dbo.Account a
    LEFT JOIN dbo.Employees e ON a.EmployeeID = e.EmployeeID
);
GO

-- 16) fnAccounts_ByUsername(@Username)
CREATE OR ALTER FUNCTION dbo.fnAccounts_ByUsername(@Username NVARCHAR(50))
RETURNS TABLE
AS
RETURN
(
    SELECT
        a.Username,
        a.Role,
        a.CreatedDate,
        a.CustomerID,
        a.EmployeeID,
        ISNULL(e.EmployeeName, N'Không xác định') AS FullName,
        ISNULL(e.Phone, N'Không có')               AS Phone,
        ISNULL(e.Address, N'Không có')             AS Address,
        ISNULL(e.Position, N'Không xác định')      AS Position
    FROM dbo.Account a
    LEFT JOIN dbo.Employees e ON a.EmployeeID = e.EmployeeID
    WHERE a.Username = @Username
);
GO

-- 17) fnAccounts_ByRole(@Role)
CREATE OR ALTER FUNCTION dbo.fnAccounts_ByRole(@Role NVARCHAR(20))
RETURNS TABLE
AS
RETURN
(
    SELECT
        a.Username,
        a.Role,
        a.CreatedDate,
        a.CustomerID,
        a.EmployeeID,
        ISNULL(e.EmployeeName, N'Không xác định') AS FullName,
        ISNULL(e.Phone, N'Không có')               AS Phone,
        ISNULL(e.Address, N'Không có')             AS Address,
        ISNULL(e.Position, N'Không xác định')      AS Position
    FROM dbo.Account a
    LEFT JOIN dbo.Employees e ON a.EmployeeID = e.EmployeeID
    WHERE a.Role = @Role
);
GO


-- ORIGINAL TVF GIỮ LẠI
-- 15) GetProductByName(@Name)
CREATE OR ALTER FUNCTION dbo.GetProductByName(@Name NVARCHAR(100))
RETURNS TABLE
AS
RETURN
(
    SELECT
        p.ProductID,
        p.ProductName,
        p.Price,
        p.StockQuantity,
        p.Unit,
        dbo.GetDiscountedPrice(p.ProductID, p.Price) AS DiscountedPrice,
        CASE WHEN dbo.GetDiscountedPrice(p.ProductID, p.Price) < p.Price THEN 1 ELSE 0 END AS HasDiscount
    FROM dbo.Products p
    WHERE p.ProductName COLLATE Vietnamese_CI_AI LIKE N'%' + @Name + N'%'
);
GO

USE Minimart_SalesDB;
GO

-- 1. fnReport_TopSellingProducts
CREATE OR ALTER FUNCTION dbo.fnReport_TopSellingProducts
(
    @StartDate DATE,
    @EndDate DATE,
    @TopN INT
)
RETURNS TABLE
AS
RETURN
(
    SELECT TOP (@TopN)
        p.ProductID,
        p.ProductName,
        p.Unit,
        SUM(sd.Quantity) AS TotalQuantitySold,
        SUM(sd.LineTotal) AS TotalRevenue
    FROM dbo.SaleDetails sd
    INNER JOIN dbo.Sales s ON sd.SaleID = s.SaleID
    INNER JOIN dbo.Products p ON sd.ProductID = p.ProductID
    -- Lọc theo khoảng thời gian
    WHERE CAST(s.SaleDate AS DATE) BETWEEN @StartDate AND @EndDate
    -- Không tính các sản phẩm đã bị xóa (nếu bạn đã thêm IsDeleted)
    -- AND p.IsDeleted = 0 
    
    GROUP BY
        p.ProductID,
        p.ProductName,
        p.Unit
    ORDER BY
        TotalQuantitySold DESC, TotalRevenue DESC
);
GO

USE Minimart_SalesDB;
GO

-- 2. fnReport_CustomerRanking
CREATE OR ALTER FUNCTION dbo.fnReport_CustomerRanking
(
    @StartDate DATE,
    @EndDate DATE
)
RETURNS TABLE
AS
RETURN
(
    SELECT
        c.CustomerID,
        c.CustomerName,
        c.Phone,
        c.LoyaltyPoints,
        SUM(s.TotalAmount) AS TotalSpending,
        -- Sử dụng ROW_NUMBER() để xếp hạng
        ROW_NUMBER() OVER (ORDER BY SUM(s.TotalAmount) DESC) AS Ranking
    FROM dbo.Customers c
    INNER JOIN dbo.Sales s ON c.CustomerID = s.CustomerID
    -- Lọc theo khoảng thời gian
    WHERE CAST(s.SaleDate AS DATE) BETWEEN @StartDate AND @EndDate
    
    GROUP BY
        c.CustomerID,
        c.CustomerName,
        c.Phone,
        c.LoyaltyPoints
);
GO

USE Minimart_SalesDB;
GO

CREATE OR ALTER FUNCTION dbo.fnReport_DailyProductSalesTrend
(
    @ProductID INT,
    @StartDate DATE,
    @EndDate DATE
)
RETURNS TABLE
AS
RETURN
(
    -- 1. Lấy tổng quan doanh thu hàng ngày của sản phẩm
    WITH DailySales AS
    (
        SELECT
            CAST(s.SaleDate AS DATE) AS SaleDay,
            SUM(sd.Quantity) AS TotalQuantity,
            SUM(sd.LineTotal) AS DailyRevenue
        FROM dbo.Sales s
        INNER JOIN dbo.SaleDetails sd ON s.SaleID = sd.SaleID
        WHERE sd.ProductID = @ProductID
          AND CAST(s.SaleDate AS DATE) BETWEEN @StartDate AND @EndDate
        GROUP BY CAST(s.SaleDate AS DATE)
    )
    
    -- 2. Áp dụng Window Functions để tính toán nâng cao
    SELECT
        ds.SaleDay,
        ds.TotalQuantity,
        ds.DailyRevenue,
        -- Tính Doanh thu Lũy kế (Running Total)
        SUM(ds.DailyRevenue) OVER (ORDER BY ds.SaleDay ROWS UNBOUNDED PRECEDING) AS CumulativeRevenue,
        -- Tính Trung bình Trượt 7 ngày (7-Day Rolling Average)
        AVG(ds.DailyRevenue) OVER (ORDER BY ds.SaleDay ROWS BETWEEN 6 PRECEDING AND CURRENT ROW) AS RollingAverage7Day
    FROM DailySales ds
    -- Sắp xếp theo ngày
    --ORDER BY ds.SaleDay
);
GO