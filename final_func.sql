/* ===========================================================
   FILE: final_functions.sql
   DB  : Minimart_SalesDB
   DESC: 4 Scalar Functions + 11 TVFs (đã refactor)
   =========================================================== */
USE Minimart_SalesDB;
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
