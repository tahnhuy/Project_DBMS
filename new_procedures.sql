-- =============================================
-- NEW PROCEDURES - Only Active Usage Objects
-- Based on FunctionSummary.md analysis
-- Total: 17 Stored Procedures
-- =============================================

USE Minimart_SalesDB;
GO

-- ========== PRODUCTS MANAGEMENT (4 procedures) ==========

-- 1. GetAllProducts
CREATE OR ALTER PROCEDURE GetAllProducts 
AS
BEGIN
    SELECT [ProductID], [ProductName], [Price], [StockQuantity], [Unit]
    FROM dbo.Products
    ORDER BY [ProductID]
END
GO

-- 2. AddProduct
CREATE OR ALTER PROCEDURE AddProduct
    @ProductName NVARCHAR(100),
    @Price DECIMAL(18, 2),
    @StockQuantity INT,
    @Unit NVARCHAR(50)
AS
BEGIN
    BEGIN TRY
        INSERT INTO dbo.Products (ProductName, Price, StockQuantity, Unit)
        VALUES (@ProductName, @Price, @StockQuantity, @Unit)
        SELECT 'SUCCESS' AS [Result], N'Thêm sản phẩm thành công' AS [Message]
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' AS [Result], ERROR_MESSAGE() AS [Message]
    END CATCH
END
GO

-- 3. UpdateProduct
CREATE OR ALTER PROCEDURE UpdateProduct
    @ProductID INT,
    @ProductName NVARCHAR(100),
    @Price DECIMAL(18, 2),
    @StockQuantity INT,
    @Unit NVARCHAR(50)
AS
BEGIN
    BEGIN TRY
        UPDATE dbo.Products 
        SET ProductName = @ProductName,
            Price = @Price,
            StockQuantity = @StockQuantity,
            Unit = @Unit
        WHERE ProductID = @ProductID

        IF @@ROWCOUNT = 0
            SELECT 'ERROR' AS [Result], N'Không tìm thấy sản phẩm để cập nhật' AS [Message]
        ELSE
            SELECT 'SUCCESS' AS [Result], N'Cập nhật sản phẩm thành công' AS [Message]
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' AS [Result], ERROR_MESSAGE() AS [Message]
    END CATCH
END
GO

-- 4. DeleteProduct
CREATE OR ALTER PROCEDURE DeleteProduct
    @ProductID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check if product exists
        IF NOT EXISTS (SELECT 1 FROM Products WHERE ProductID = @ProductID)
        BEGIN
            SELECT 'ERROR' AS Result, 'Sản phẩm không tồn tại.' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Check if product has been used in sales
        IF EXISTS (SELECT 1 FROM SaleDetails WHERE ProductID = @ProductID)
        BEGIN
            SELECT 'ERROR' AS Result, 'Không thể xóa sản phẩm đã có trong hóa đơn bán hàng.' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Check if product has active discounts
        IF EXISTS (SELECT 1 FROM Discounts WHERE ProductID = @ProductID AND EndDate >= GETDATE())
        BEGIN
            SELECT 'ERROR' AS Result, 'Không thể xóa sản phẩm đang có chương trình giảm giá hoạt động.' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Delete product
        DELETE FROM Products WHERE ProductID = @ProductID;
        
        IF @@ROWCOUNT > 0
        BEGIN
            SELECT 'SUCCESS' AS Result, 'Xóa sản phẩm thành công.' AS Message;
        END
        ELSE
        BEGIN
            SELECT 'ERROR' AS Result, 'Không thể xóa sản phẩm.' AS Message;
        END
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, 'Lỗi khi xóa sản phẩm: ' + ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- ========== CUSTOMERS MANAGEMENT (6 procedures) ==========

-- 5. GetAllCustomers
CREATE OR ALTER PROCEDURE GetAllCustomers
AS
BEGIN
    SELECT [CustomerID], [CustomerName], [Phone], [Address], [LoyaltyPoints]
    FROM dbo.Customers
    ORDER BY [CustomerID]
END
GO

-- 6. GetCustomerByName
CREATE OR ALTER PROCEDURE GetCustomerByName
    @CustomerName NVARCHAR(100)
AS
BEGIN
    SELECT *
    FROM dbo.Customers
    WHERE CustomerName COLLATE Vietnamese_CI_AI LIKE N'%' + @CustomerName + '%'
    ORDER BY CustomerID
END
GO

-- 7. GetCustomerByID
CREATE OR ALTER PROCEDURE GetCustomerByID
    @CustomerID INT
AS
BEGIN
    SELECT * FROM dbo.Customers WHERE CustomerID = @CustomerID
END
GO

-- 8. AddCustomer
CREATE OR ALTER PROCEDURE AddCustomer
    @CustomerName NVARCHAR(100),
    @Phone NVARCHAR(20) = NULL,
    @Address NVARCHAR(200) = NULL,
    @LoyaltyPoints INT = 0
AS
BEGIN
    BEGIN TRY
        INSERT INTO dbo.Customers (CustomerName, Phone, Address, LoyaltyPoints)
        VALUES (@CustomerName, @Phone, @Address, @LoyaltyPoints)
        SELECT 'SUCCESS' AS [Result], N'Thêm khách hàng thành công' AS [Message]
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' AS [Result], ERROR_MESSAGE() AS [Message]
    END CATCH
END
GO

-- 9. UpdateCustomer
CREATE OR ALTER PROCEDURE UpdateCustomer
    @CustomerID INT,
    @CustomerName NVARCHAR(100),
    @Phone NVARCHAR(20) = NULL,
    @Address NVARCHAR(200) = NULL,
    @LoyaltyPoints INT
AS
BEGIN
    BEGIN TRY
        UPDATE dbo.Customers 
        SET CustomerName = @CustomerName,
            Phone = @Phone,
            Address = @Address,
            LoyaltyPoints = @LoyaltyPoints
        WHERE CustomerID = @CustomerID

        IF @@ROWCOUNT = 0
            SELECT 'ERROR' AS [Result], N'Không tìm thấy khách hàng để cập nhật' AS [Message]
        ELSE
            SELECT 'SUCCESS' AS [Result], N'Cập nhật khách hàng thành công' AS [Message]
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' AS [Result], ERROR_MESSAGE() AS [Message]
    END CATCH
END
GO

-- 10. DeleteCustomer
CREATE OR ALTER PROCEDURE DeleteCustomer
    @CustomerID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check if customer exists
        IF NOT EXISTS (SELECT 1 FROM Customers WHERE CustomerID = @CustomerID)
        BEGIN
            SELECT 'ERROR' AS Result, 'Khách hàng không tồn tại.' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Check if customer has sales history
        IF EXISTS (SELECT 1 FROM Sales WHERE CustomerID = @CustomerID)
        BEGIN
            SELECT 'ERROR' AS Result, 'Không thể xóa khách hàng đã có lịch sử mua hàng.' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Check if customer has account
        IF EXISTS (SELECT 1 FROM Account WHERE CustomerID = @CustomerID)
        BEGIN
            SELECT 'ERROR' AS Result, 'Không thể xóa khách hàng đã có tài khoản đăng nhập.' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Delete customer
        DELETE FROM Customers WHERE CustomerID = @CustomerID;
        
        IF @@ROWCOUNT > 0
        BEGIN
            SELECT 'SUCCESS' AS Result, 'Xóa khách hàng thành công.' AS Message;
        END
        ELSE
        BEGIN
            SELECT 'ERROR' AS Result, 'Không thể xóa khách hàng.' AS Message;
        END
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, 'Lỗi khi xóa khách hàng: ' + ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- ========== SALES MANAGEMENT (5 procedures) ==========

-- 11. CreateSale
CREATE OR ALTER PROCEDURE CreateSale
    @CustomerID INT = NULL,
    @TotalAmount DECIMAL(18,2),
    @PaymentMethod NVARCHAR(50) = NULL,
    @CreatedBy NVARCHAR(50) = 'system'
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @SaleID INT;
        
        INSERT INTO dbo.Sales (CustomerID, SaleDate, TotalAmount, PaymentMethod)
        VALUES (@CustomerID, GETDATE(), @TotalAmount, @PaymentMethod);
        
        SET @SaleID = SCOPE_IDENTITY();
        
        -- Tạo transaction với CreatedBy từ tham số
        INSERT INTO dbo.Transactions (TransactionType, Amount, Description, TransactionDate, CreatedBy, ReferenceID, ReferenceType)
        VALUES ('income', @TotalAmount, N'Thu tiền từ bán hàng - Hóa đơn #' + CAST(@SaleID AS NVARCHAR(20)), GETDATE(), @CreatedBy, @SaleID, 'sale');
        
        SELECT 'SUCCESS' AS [Result], N'Tạo hóa đơn thành công' AS [Message], @SaleID AS SaleID;
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' AS [Result], ERROR_MESSAGE() AS [Message], 0 AS SaleID;
    END CATCH
END
GO

-- 12. AddSaleDetail
CREATE OR ALTER PROCEDURE AddSaleDetail
    @SaleID INT,
    @ProductID INT,
    @Quantity INT,
    @SalePrice DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- Kiểm tra hóa đơn tồn tại
        IF NOT EXISTS (SELECT 1 FROM dbo.Sales WHERE SaleID = @SaleID)
        BEGIN
            SELECT 'ERROR' AS [Result], N'Không tìm thấy hóa đơn' AS [Message]; RETURN;
        END
        
        -- Kiểm tra sản phẩm tồn tại
        IF NOT EXISTS (SELECT 1 FROM dbo.Products WHERE ProductID = @ProductID)
        BEGIN
            SELECT 'ERROR' AS [Result], N'Không tìm thấy sản phẩm' AS [Message]; RETURN;
        END
        
        INSERT INTO dbo.SaleDetails (SaleID, ProductID, Quantity, SalePrice, LineTotal)
        VALUES (@SaleID, @ProductID, @Quantity, @SalePrice, @Quantity * @SalePrice);
        
        SELECT 'SUCCESS' AS [Result], N'Thêm chi tiết hóa đơn thành công' AS [Message];
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' AS [Result], ERROR_MESSAGE() AS [Message];
    END CATCH
END
GO

-- 13. UpdateSale
CREATE OR ALTER PROCEDURE UpdateSale
    @SaleID INT,
    @CustomerID INT = NULL,
    @TotalAmount DECIMAL(18,2),
    @PaymentMethod NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM dbo.Sales WHERE SaleID = @SaleID)
        BEGIN
            SELECT 'ERROR' AS [Result], N'Không tìm thấy hóa đơn để cập nhật' AS [Message]; RETURN;
        END
        
        UPDATE dbo.Sales 
        SET CustomerID = @CustomerID,
            TotalAmount = @TotalAmount,
            PaymentMethod = @PaymentMethod
        WHERE SaleID = @SaleID;
        
        SELECT 'SUCCESS' AS [Result], N'Cập nhật hóa đơn thành công' AS [Message];
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' AS [Result], ERROR_MESSAGE() AS [Message];
    END CATCH
END
GO

-- 14. GetSaleByID
CREATE OR ALTER PROCEDURE GetSaleByID
    @SaleID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        s.SaleID,
        s.CustomerID,
        c.CustomerName,
        s.SaleDate,
        s.TotalAmount,
        s.PaymentMethod
    FROM dbo.Sales s
    LEFT JOIN dbo.Customers c ON s.CustomerID = c.CustomerID
    WHERE s.SaleID = @SaleID;
END
GO

-- 15. GetSaleDetails
CREATE OR ALTER PROCEDURE GetSaleDetails
    @SaleID INT
AS
BEGIN
    SET NOCOUNT ON;
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
    ORDER BY sd.ProductID;
END
GO

-- ========== DISCOUNTS MANAGEMENT (5 procedures) ==========

-- 16. GetActiveDiscounts
CREATE OR ALTER PROCEDURE GetActiveDiscounts
AS
BEGIN
    SET NOCOUNT ON;
    SELECT d.DiscountID, d.ProductID, p.ProductName, d.DiscountType, d.DiscountValue, 
           d.StartDate, d.EndDate, d.IsActive, d.CreatedDate, d.CreatedBy
    FROM dbo.Discounts d
    INNER JOIN dbo.Products p ON p.ProductID = d.ProductID
    WHERE d.IsActive = 1 
      AND GETDATE() >= d.StartDate 
      AND GETDATE() <= d.EndDate
    ORDER BY p.ProductName;
END
GO

-- 17. GetDiscountsByProduct
CREATE OR ALTER PROCEDURE GetDiscountsByProduct
    @ProductID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT d.DiscountID, d.ProductID, p.ProductName, d.DiscountType, d.DiscountValue, 
           d.StartDate, d.EndDate, d.IsActive, d.CreatedDate, d.CreatedBy
    FROM dbo.Discounts d
    INNER JOIN dbo.Products p ON p.ProductID = d.ProductID
    WHERE d.ProductID = @ProductID
    ORDER BY d.CreatedDate DESC;
END
GO

-- 18. AddDiscount
CREATE OR ALTER PROCEDURE AddDiscount
    @ProductID INT,
    @DiscountType NVARCHAR(20),
    @DiscountValue DECIMAL(18, 2),
    @StartDate DATETIME,
    @EndDate DATETIME,
    @IsActive BIT = 1,
    @CreatedBy NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- Validate product exists
        IF NOT EXISTS (SELECT 1 FROM dbo.Products WHERE ProductID = @ProductID)
        BEGIN
            SELECT 'ERROR' AS [Result], N'Sản phẩm không tồn tại' AS [Message]; RETURN;
        END
        
        -- Validate date range
        IF @EndDate <= @StartDate
        BEGIN
            SELECT 'ERROR' AS [Result], N'Ngày kết thúc phải sau ngày bắt đầu' AS [Message]; RETURN;
        END
        
        -- Validate percentage discount
        IF @DiscountType = 'percentage' AND @DiscountValue > 100
        BEGIN
            SELECT 'ERROR' AS [Result], N'Giảm giá phần trăm không được vượt quá 100%' AS [Message]; RETURN;
        END
        
        -- Check for overlapping active discounts for the same product
        IF EXISTS (
            SELECT 1 FROM dbo.Discounts 
            WHERE ProductID = @ProductID 
              AND IsActive = 1
              AND (	
                  (@StartDate BETWEEN StartDate AND EndDate) OR
                  (@EndDate BETWEEN StartDate AND EndDate) OR
                  (StartDate BETWEEN @StartDate AND @EndDate) OR
                  (EndDate BETWEEN @StartDate AND @EndDate)
              )
        )
        BEGIN
            SELECT 'ERROR' AS [Result], N'Đã có chương trình giảm giá khác cho sản phẩm này trong khoảng thời gian này' AS [Message]; RETURN;
        END
        
        INSERT INTO dbo.Discounts (ProductID, DiscountType, DiscountValue, StartDate, EndDate, IsActive, CreatedBy)
        VALUES (@ProductID, @DiscountType, @DiscountValue, @StartDate, @EndDate, @IsActive, @CreatedBy);
        
        SELECT 'SUCCESS' AS [Result], N'Thêm chương trình giảm giá thành công' AS [Message];
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' AS [Result], ERROR_MESSAGE() AS [Message];
    END CATCH
END
GO

-- 19. UpdateDiscount
CREATE OR ALTER PROCEDURE UpdateDiscount
    @DiscountID INT,
    @ProductID INT,
    @DiscountType NVARCHAR(20),
    @DiscountValue DECIMAL(18, 2),
    @StartDate DATETIME,
    @EndDate DATETIME,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- Validate discount exists
        IF NOT EXISTS (SELECT 1 FROM dbo.Discounts WHERE DiscountID = @DiscountID)
        BEGIN
            SELECT 'ERROR' AS [Result], N'Không tìm thấy chương trình giảm giá' AS [Message]; RETURN;
        END
        
        -- Validate product exists
        IF NOT EXISTS (SELECT 1 FROM dbo.Products WHERE ProductID = @ProductID)
        BEGIN
            SELECT 'ERROR' AS [Result], N'Sản phẩm không tồn tại' AS [Message]; RETURN;
        END
        
        -- Validate date range
        IF @EndDate <= @StartDate
        BEGIN
            SELECT 'ERROR' AS [Result], N'Ngày kết thúc phải sau ngày bắt đầu' AS [Message]; RETURN;
        END
        
        -- Validate percentage discount
        IF @DiscountType = 'percentage' AND @DiscountValue > 100
        BEGIN
            SELECT 'ERROR' AS [Result], N'Giảm giá phần trăm không được vượt quá 100%' AS [Message]; RETURN;
        END
        
        -- Check for overlapping active discounts for the same product (excluding current discount)
        IF EXISTS (
            SELECT 1 FROM dbo.Discounts 
            WHERE ProductID = @ProductID 
              AND DiscountID != @DiscountID
              AND IsActive = 1
              AND (
                  (@StartDate BETWEEN StartDate AND EndDate) OR
                  (@EndDate BETWEEN StartDate AND EndDate) OR
                  (StartDate BETWEEN @StartDate AND @EndDate) OR
                  (EndDate BETWEEN @StartDate AND @EndDate)
              )
        )
        BEGIN
            SELECT 'ERROR' AS [Result], N'Đã có chương trình giảm giá khác cho sản phẩm này trong khoảng thời gian này' AS [Message]; RETURN;
        END
        
        UPDATE dbo.Discounts 
        SET ProductID = @ProductID,
            DiscountType = @DiscountType,
            DiscountValue = @DiscountValue,
            StartDate = @StartDate,
            EndDate = @EndDate,
            IsActive = @IsActive
        WHERE DiscountID = @DiscountID;
        
        IF @@ROWCOUNT = 0
            SELECT 'ERROR' AS [Result], N'Không thể cập nhật chương trình giảm giá' AS [Message];
        ELSE
            SELECT 'SUCCESS' AS [Result], N'Cập nhật chương trình giảm giá thành công' AS [Message];
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' AS [Result], ERROR_MESSAGE() AS [Message];
    END CATCH
END
GO

-- 20. DeleteDiscount
CREATE OR ALTER PROCEDURE DeleteDiscount
    @DiscountID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check if discount exists
        IF NOT EXISTS (SELECT 1 FROM Discounts WHERE DiscountID = @DiscountID)
        BEGIN
            SELECT 'ERROR' AS Result, 'Chương trình giảm giá không tồn tại.' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Check if discount is currently active
        IF EXISTS (SELECT 1 FROM Discounts WHERE DiscountID = @DiscountID AND StartDate <= GETDATE() AND EndDate >= GETDATE())
        BEGIN
            SELECT 'ERROR' AS Result, 'Không thể xóa chương trình giảm giá đang hoạt động.' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Delete discount
        DELETE FROM Discounts WHERE DiscountID = @DiscountID;
        
        IF @@ROWCOUNT > 0
        BEGIN
            SELECT 'SUCCESS' AS Result, 'Xóa chương trình giảm giá thành công.' AS Message;
        END
        ELSE
        BEGIN
            SELECT 'ERROR' AS Result, 'Không thể xóa chương trình giảm giá.' AS Message;
        END
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, 'Lỗi khi xóa chương trình giảm giá: ' + ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- ========== ACCOUNT MANAGEMENT (2 procedures) ==========

-- 21. CheckLogin
CREATE OR ALTER PROCEDURE CheckLogin
    @Username NVARCHAR(50),
    @Password NVARCHAR(255)
AS
BEGIN
    DECLARE @Role NVARCHAR(20)
    SELECT @Role = Role FROM dbo.Account WHERE Username = @Username AND Password = @Password
    IF @Role IS NULL
        SELECT 'ERROR' AS [Result], N'Tên đăng nhập hoặc mật khẩu không đúng' AS [Message]
    ELSE
        SELECT 'SUCCESS' AS [Result], N'Đăng nhập thành công' AS [Message], @Username AS Username, @Role AS Role
END
GO

-- 22. ChangePassword
CREATE OR ALTER PROCEDURE ChangePassword
    @Username NVARCHAR(50),
    @OldPassword NVARCHAR(255),
    @NewPassword NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM dbo.Account WHERE Username = @Username AND Password = @OldPassword)
        BEGIN
            SELECT 'ERROR' AS [Result], N'Mật khẩu cũ không đúng' AS [Message]; RETURN;
        END
        
        UPDATE dbo.Account 
        SET Password = @NewPassword
        WHERE Username = @Username;
        
        SELECT 'SUCCESS' AS [Result], N'Đổi mật khẩu thành công' AS [Message];
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' AS [Result], ERROR_MESSAGE() AS [Message];
    END CATCH
END
GO

PRINT N'✅ Đã tạo thành công 22 stored procedures cần thiết!'
PRINT N'📋 Tổng cộng:'
PRINT N'   - Products: 4 procedures'
PRINT N'   - Customers: 6 procedures'  
PRINT N'   - Sales: 5 procedures'
PRINT N'   - Discounts: 5 procedures'
PRINT N'   - Accounts: 2 procedures'
GO
