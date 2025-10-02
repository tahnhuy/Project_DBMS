USE Minimart_SalesDB;
GO
--------------------------------------------------------
-- PRODUCTS
--------------------------------------------------------
CREATE OR ALTER PROCEDURE AddProduct
    @ProductName NVARCHAR(100),
    @Price DECIMAL(18, 2),
    @StockQuantity INT,
    @Unit NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra dữ liệu đầu vào
        IF @ProductName IS NULL OR LTRIM(RTRIM(@ProductName)) = ''
        BEGIN
            SELECT 'ERROR' AS Result, N'Tên sản phẩm không được để trống' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @Price <= 0
        BEGIN
            SELECT 'ERROR' AS Result, N'Giá sản phẩm phải lớn hơn 0' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @StockQuantity < 0
        BEGIN
            SELECT 'ERROR' AS Result, N'Số lượng tồn kho không được âm' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        INSERT INTO dbo.Products (ProductName, Price, StockQuantity, Unit)
        VALUES (@ProductName, @Price, @StockQuantity, @Unit);
        
        COMMIT TRANSACTION;
        SELECT 'SUCCESS' AS Result, N'Thêm sản phẩm thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE UpdateProduct
    @ProductID INT,
    @ProductName NVARCHAR(100),
    @Price DECIMAL(18, 2),
    @StockQuantity INT,
    @Unit NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra dữ liệu đầu vào
        IF @ProductID IS NULL OR @ProductID <= 0
        BEGIN
            SELECT 'ERROR' AS Result, N'ID sản phẩm không hợp lệ' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @ProductName IS NULL OR LTRIM(RTRIM(@ProductName)) = ''
        BEGIN
            SELECT 'ERROR' AS Result, N'Tên sản phẩm không được để trống' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @Price <= 0
        BEGIN
            SELECT 'ERROR' AS Result, N'Giá sản phẩm phải lớn hơn 0' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @StockQuantity < 0
        BEGIN
            SELECT 'ERROR' AS Result, N'Số lượng tồn kho không được âm' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra sản phẩm có tồn tại không
        IF NOT EXISTS(SELECT 1 FROM dbo.Products WHERE ProductID = @ProductID)
        BEGIN
            SELECT 'ERROR' AS Result, N'Không tìm thấy sản phẩm để cập nhật' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        UPDATE dbo.Products
        SET ProductName=@ProductName, Price=@Price,
            StockQuantity=@StockQuantity, Unit=@Unit
        WHERE ProductID=@ProductID;
        
        COMMIT TRANSACTION;
        SELECT 'SUCCESS' AS Result, N'Cập nhật sản phẩm thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE DeleteProduct
    @ProductID INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra dữ liệu đầu vào
        IF @ProductID IS NULL OR @ProductID <= 0
        BEGIN
            SELECT 'ERROR' AS Result, N'ID sản phẩm không hợp lệ' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra sản phẩm có tồn tại không
        IF NOT EXISTS(SELECT 1 FROM dbo.Products WHERE ProductID = @ProductID)
        BEGIN
            SELECT 'ERROR' AS Result, N'Không tìm thấy sản phẩm để xóa' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra sản phẩm có trong hóa đơn không
        IF EXISTS(SELECT 1 FROM SaleDetails WHERE ProductID = @ProductID)
        BEGIN
            SELECT 'ERROR' AS Result, N'Không thể xóa sản phẩm đã có trong hóa đơn' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra sản phẩm có discount không
        IF EXISTS(SELECT 1 FROM Discounts WHERE ProductID = @ProductID)
        BEGIN
            SELECT 'ERROR' AS Result, N'Không thể xóa sản phẩm đang có chương trình giảm giá' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        DELETE FROM dbo.Products WHERE ProductID = @ProductID;
        
        COMMIT TRANSACTION;
        SELECT 'SUCCESS' AS Result, N'Xóa sản phẩm thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

--------------------------------------------------------
-- CUSTOMERS
--------------------------------------------------------
CREATE OR ALTER PROCEDURE AddCustomer
    @CustomerName NVARCHAR(100),
    @Phone NVARCHAR(20),
    @Address NVARCHAR(255) = NULL,
    @LoyaltyPoints INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra dữ liệu đầu vào
        IF @CustomerName IS NULL OR LTRIM(RTRIM(@CustomerName)) = ''
        BEGIN
            SELECT 'ERROR' AS Result, N'Tên khách hàng không được để trống' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @Phone IS NULL OR LTRIM(RTRIM(@Phone)) = ''
        BEGIN
            SELECT 'ERROR' AS Result, N'Số điện thoại không được để trống' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra số điện thoại đã tồn tại chưa
        IF EXISTS(SELECT 1 FROM dbo.Customers WHERE Phone = @Phone)
        BEGIN
            SELECT 'ERROR' AS Result, N'Số điện thoại đã được sử dụng' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        INSERT INTO dbo.Customers (CustomerName, Phone, Address, LoyaltyPoints)
        VALUES (@CustomerName, @Phone, @Address, @LoyaltyPoints);
        
        COMMIT TRANSACTION;
        SELECT 'SUCCESS' AS Result, N'Thêm khách hàng thành công' AS Message, SCOPE_IDENTITY() AS CustomerID;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE UpdateCustomer
    @CustomerID INT,
    @CustomerName NVARCHAR(100),
    @Phone NVARCHAR(20),
    @Address NVARCHAR(255),
    @LoyaltyPoints INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra dữ liệu đầu vào
        IF @CustomerID IS NULL OR @CustomerID <= 0
        BEGIN
            SELECT 'ERROR' AS Result, N'ID khách hàng không hợp lệ' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @CustomerName IS NULL OR LTRIM(RTRIM(@CustomerName)) = ''
        BEGIN
            SELECT 'ERROR' AS Result, N'Tên khách hàng không được để trống' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @Phone IS NULL OR LTRIM(RTRIM(@Phone)) = ''
        BEGIN
            SELECT 'ERROR' AS Result, N'Số điện thoại không được để trống' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @LoyaltyPoints < 0
        BEGIN
            SELECT 'ERROR' AS Result, N'Điểm tích lũy không được âm' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra khách hàng có tồn tại không
        IF NOT EXISTS(SELECT 1 FROM dbo.Customers WHERE CustomerID = @CustomerID)
        BEGIN
            SELECT 'ERROR' AS Result, N'Không tìm thấy khách hàng để cập nhật' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra số điện thoại đã tồn tại chưa (trừ khách hàng hiện tại)
        IF EXISTS(SELECT 1 FROM dbo.Customers WHERE Phone = @Phone AND CustomerID != @CustomerID)
        BEGIN
            SELECT 'ERROR' AS Result, N'Số điện thoại đã được sử dụng bởi khách hàng khác' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        UPDATE dbo.Customers
        SET CustomerName=@CustomerName, Phone=@Phone,
            Address=@Address, LoyaltyPoints=@LoyaltyPoints
        WHERE CustomerID=@CustomerID;
        
        COMMIT TRANSACTION;
        SELECT 'SUCCESS' AS Result, N'Cập nhật khách hàng thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE DeleteCustomer
    @CustomerID INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra dữ liệu đầu vào
        IF @CustomerID IS NULL OR @CustomerID <= 0
        BEGIN
            SELECT 'ERROR' AS Result, N'ID khách hàng không hợp lệ' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra khách hàng có tồn tại không
        IF NOT EXISTS(SELECT 1 FROM dbo.Customers WHERE CustomerID = @CustomerID)
        BEGIN
            SELECT 'ERROR' AS Result, N'Không tìm thấy khách hàng để xóa' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra khách hàng có mua hàng không
        IF EXISTS(SELECT 1 FROM Sales WHERE CustomerID = @CustomerID)
        BEGIN
            SELECT 'ERROR' AS Result, N'Không thể xóa khách hàng đã mua hàng' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        DELETE FROM dbo.Customers WHERE CustomerID = @CustomerID;
        
        COMMIT TRANSACTION;
        SELECT 'SUCCESS' AS Result, N'Xóa khách hàng thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

--------------------------------------------------------
-- SALES
--------------------------------------------------------
CREATE OR ALTER PROCEDURE CreateSale
    @CustomerID INT=NULL,
    @TotalAmount DECIMAL(18,2),
    @PaymentMethod NVARCHAR(50)=NULL,
    @CreatedBy NVARCHAR(50)='system'
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @SaleID INT;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra dữ liệu đầu vào
        IF @TotalAmount IS NULL OR @TotalAmount <= 0
        BEGIN
            SELECT 'ERROR' AS Result, N'Tổng tiền phải lớn hơn 0' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra khách hàng có tồn tại không (nếu có)
        IF @CustomerID IS NOT NULL AND NOT EXISTS(SELECT 1 FROM dbo.Customers WHERE CustomerID = @CustomerID)
        BEGIN
            SELECT 'ERROR' AS Result, N'Không tìm thấy khách hàng' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Tạo hóa đơn
        INSERT INTO dbo.Sales(CustomerID,SaleDate,TotalAmount,PaymentMethod)
        VALUES(@CustomerID,GETDATE(),@TotalAmount,@PaymentMethod);
        SET @SaleID=SCOPE_IDENTITY();
        
        -- Tạo giao dịch thu tiền
        INSERT INTO dbo.Transactions(TransactionType,Amount,Description,TransactionDate,CreatedBy,ReferenceID,ReferenceType)
        VALUES('income',@TotalAmount,N'Thu tiền từ bán hàng - Hóa đơn #'+CAST(@SaleID AS NVARCHAR(20)),GETDATE(),@CreatedBy,@SaleID,'sale');
        
        COMMIT TRANSACTION;
        SELECT 'SUCCESS' AS Result, N'Tạo hóa đơn thành công' AS Message, @SaleID AS SaleID;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE AddSaleDetail
    @SaleID INT,@ProductID INT,@Quantity INT,@SalePrice DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra dữ liệu đầu vào
        IF @SaleID IS NULL OR @SaleID <= 0
        BEGIN
            SELECT 'ERROR' AS Result, N'ID hóa đơn không hợp lệ' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @ProductID IS NULL OR @ProductID <= 0
        BEGIN
            SELECT 'ERROR' AS Result, N'ID sản phẩm không hợp lệ' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @Quantity IS NULL OR @Quantity <= 0
        BEGIN
            SELECT 'ERROR' AS Result, N'Số lượng phải lớn hơn 0' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @SalePrice IS NULL OR @SalePrice <= 0
        BEGIN
            SELECT 'ERROR' AS Result, N'Giá bán phải lớn hơn 0' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra hóa đơn có tồn tại không
        IF NOT EXISTS(SELECT 1 FROM Sales WHERE SaleID = @SaleID)
        BEGIN
            SELECT 'ERROR' AS Result, N'Không tìm thấy hóa đơn' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra sản phẩm có tồn tại không
        IF NOT EXISTS(SELECT 1 FROM dbo.Products WHERE ProductID = @ProductID)
        BEGIN
            SELECT 'ERROR' AS Result, N'Không tìm thấy sản phẩm' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra tồn kho
        IF NOT EXISTS(SELECT 1 FROM dbo.Products WHERE ProductID = @ProductID AND StockQuantity >= @Quantity)
        BEGIN
            SELECT 'ERROR' AS Result, N'Không đủ hàng trong kho' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Thêm chi tiết hóa đơn
        INSERT INTO dbo.SaleDetails(SaleID,ProductID,Quantity,SalePrice,LineTotal)
        VALUES(@SaleID,@ProductID,@Quantity,@SalePrice,@Quantity*@SalePrice);
        
        -- Cập nhật tồn kho
        UPDATE dbo.Products 
        SET StockQuantity = StockQuantity - @Quantity 
        WHERE ProductID = @ProductID;
        
        COMMIT TRANSACTION;
        SELECT 'SUCCESS' AS Result, N'Thêm chi tiết hóa đơn thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE UpdateSale
    @SaleID INT,@CustomerID INT=NULL,@TotalAmount DECIMAL(18,2),@PaymentMethod NVARCHAR(50)=NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra dữ liệu đầu vào
        IF @SaleID IS NULL OR @SaleID <= 0
        BEGIN
            SELECT 'ERROR' AS Result, N'ID hóa đơn không hợp lệ' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @TotalAmount IS NULL OR @TotalAmount <= 0
        BEGIN
            SELECT 'ERROR' AS Result, N'Tổng tiền phải lớn hơn 0' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra hóa đơn có tồn tại không
        IF NOT EXISTS(SELECT 1 FROM Sales WHERE SaleID = @SaleID)
        BEGIN
            SELECT 'ERROR' AS Result, N'Không tìm thấy hóa đơn' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra khách hàng có tồn tại không (nếu có)
        IF @CustomerID IS NOT NULL AND NOT EXISTS(SELECT 1 FROM dbo.Customers WHERE CustomerID = @CustomerID)
        BEGIN
            SELECT 'ERROR' AS Result, N'Không tìm thấy khách hàng' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Lấy tổng tiền cũ để cập nhật transaction
        DECLARE @OldTotalAmount DECIMAL(18,2);
        SELECT @OldTotalAmount = TotalAmount FROM Sales WHERE SaleID = @SaleID;
        
        -- Cập nhật hóa đơn
        UPDATE dbo.Sales SET CustomerID=@CustomerID,TotalAmount=@TotalAmount,PaymentMethod=@PaymentMethod WHERE SaleID=@SaleID;
        
        -- Cập nhật transaction nếu tổng tiền thay đổi
        IF @OldTotalAmount != @TotalAmount
        BEGIN
            UPDATE dbo.Transactions 
            SET Amount = @TotalAmount,
                Description = N'Thu tiền từ bán hàng - Hóa đơn #'+CAST(@SaleID AS NVARCHAR(20))
            WHERE ReferenceID = @SaleID AND ReferenceType = 'sale';
        END
        
        COMMIT TRANSACTION;
        SELECT 'SUCCESS' AS Result, N'Cập nhật hóa đơn thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

--------------------------------------------------------
-- DISCOUNTS
--------------------------------------------------------
CREATE OR ALTER PROCEDURE AddDiscount
    @ProductID INT,@DiscountType NVARCHAR(20),@DiscountValue DECIMAL(18,2),
    @StartDate DATETIME,@EndDate DATETIME,@IsActive BIT=1,@CreatedBy NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra dữ liệu đầu vào
        IF @ProductID IS NULL OR @ProductID <= 0
        BEGIN
            SELECT 'ERROR' AS Result, N'ID sản phẩm không hợp lệ' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @DiscountType IS NULL OR @DiscountType NOT IN ('percentage', 'fixed')
        BEGIN
            SELECT 'ERROR' AS Result, N'Loại giảm giá phải là "percentage" hoặc "fixed"' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @DiscountValue IS NULL OR @DiscountValue <= 0
        BEGIN
            SELECT 'ERROR' AS Result, N'Giá trị giảm giá phải lớn hơn 0' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @StartDate IS NULL OR @EndDate IS NULL OR @StartDate >= @EndDate
        BEGIN
            SELECT 'ERROR' AS Result, N'Ngày bắt đầu phải nhỏ hơn ngày kết thúc' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra sản phẩm có tồn tại không
        IF NOT EXISTS(SELECT 1 FROM dbo.Products WHERE ProductID = @ProductID)
        BEGIN
            SELECT 'ERROR' AS Result, N'Không tìm thấy sản phẩm' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra giảm giá percentage không vượt quá 100%
        IF @DiscountType = 'percentage' AND @DiscountValue > 100
        BEGIN
            SELECT 'ERROR' AS Result, N'Giảm giá phần trăm không được vượt quá 100%' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        INSERT INTO dbo.Discounts(ProductID,DiscountType,DiscountValue,StartDate,EndDate,IsActive,CreatedDate,CreatedBy)
        VALUES(@ProductID,@DiscountType,@DiscountValue,@StartDate,@EndDate,@IsActive,GETDATE(),@CreatedBy);
        
        COMMIT TRANSACTION;
        SELECT 'SUCCESS' AS Result, N'Thêm chương trình giảm giá thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE UpdateDiscount
    @DiscountID INT,@DiscountType NVARCHAR(20),@DiscountValue DECIMAL(18,2),
    @StartDate DATETIME,@EndDate DATETIME,@IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra dữ liệu đầu vào
        IF @DiscountID IS NULL OR @DiscountID <= 0
        BEGIN
            SELECT 'ERROR' AS Result, N'ID giảm giá không hợp lệ' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @DiscountType IS NULL OR @DiscountType NOT IN ('percentage', 'fixed')
        BEGIN
            SELECT 'ERROR' AS Result, N'Loại giảm giá phải là "percentage" hoặc "fixed"' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @DiscountValue IS NULL OR @DiscountValue <= 0
        BEGIN
            SELECT 'ERROR' AS Result, N'Giá trị giảm giá phải lớn hơn 0' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @StartDate IS NULL OR @EndDate IS NULL OR @StartDate >= @EndDate
        BEGIN
            SELECT 'ERROR' AS Result, N'Ngày bắt đầu phải nhỏ hơn ngày kết thúc' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra discount có tồn tại không
        IF NOT EXISTS(SELECT 1 FROM dbo.Discounts WHERE DiscountID = @DiscountID)
        BEGIN
            SELECT 'ERROR' AS Result, N'Không tìm thấy chương trình giảm giá để cập nhật' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra giảm giá percentage không vượt quá 100%
        IF @DiscountType = 'percentage' AND @DiscountValue > 100
        BEGIN
            SELECT 'ERROR' AS Result, N'Giảm giá phần trăm không được vượt quá 100%' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        UPDATE dbo.Discounts SET DiscountType=@DiscountType,DiscountValue=@DiscountValue,StartDate=@StartDate,EndDate=@EndDate,IsActive=@IsActive
        WHERE DiscountID=@DiscountID;
        
        COMMIT TRANSACTION;
        SELECT 'SUCCESS' AS Result, N'Cập nhật chương trình giảm giá thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE DeleteDiscount
    @DiscountID INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra dữ liệu đầu vào
        IF @DiscountID IS NULL OR @DiscountID <= 0
        BEGIN
            SELECT 'ERROR' AS Result, N'ID giảm giá không hợp lệ' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra discount có tồn tại không
        IF NOT EXISTS(SELECT 1 FROM dbo.Discounts WHERE DiscountID = @DiscountID)
        BEGIN
            SELECT 'ERROR' AS Result, N'Không tìm thấy chương trình giảm giá để xóa' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        DELETE FROM dbo.Discounts WHERE DiscountID = @DiscountID;
        
        COMMIT TRANSACTION;
        SELECT 'SUCCESS' AS Result, N'Xóa chương trình giảm giá thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

--------------------------------------------------------
-- ACCOUNTS
--------------------------------------------------------
CREATE OR ALTER PROCEDURE ChangePassword
    @Username NVARCHAR(50),@OldPassword NVARCHAR(255),@NewPassword NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra dữ liệu đầu vào
        IF @Username IS NULL OR LTRIM(RTRIM(@Username)) = ''
        BEGIN
            SELECT 'ERROR' AS Result, N'Tên đăng nhập không được để trống' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @OldPassword IS NULL OR LTRIM(RTRIM(@OldPassword)) = ''
        BEGIN
            SELECT 'ERROR' AS Result, N'Mật khẩu cũ không được để trống' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @NewPassword IS NULL OR LTRIM(RTRIM(@NewPassword)) = ''
        BEGIN
            SELECT 'ERROR' AS Result, N'Mật khẩu mới không được để trống' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra độ dài mật khẩu mới
        IF LEN(@NewPassword) < 6
        BEGIN
            SELECT 'ERROR' AS Result, N'Mật khẩu mới phải có ít nhất 6 ký tự' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra tài khoản và mật khẩu cũ
        IF NOT EXISTS(SELECT 1 FROM dbo.Account WHERE Username = @Username AND Password = @OldPassword)
        BEGIN
            SELECT 'ERROR' AS Result, N'Mật khẩu cũ không đúng' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Cập nhật mật khẩu mới
        UPDATE dbo.Account SET Password = @NewPassword WHERE Username = @Username;
        
        COMMIT TRANSACTION;
        SELECT 'SUCCESS' AS Result, N'Đổi mật khẩu thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE AddAccount
    @Username NVARCHAR(50),
    @Password NVARCHAR(255),
    @Role NVARCHAR(20),
    @CustomerID INT = NULL,
    @EmployeeID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra dữ liệu đầu vào
        IF @Username IS NULL OR LTRIM(RTRIM(@Username)) = ''
        BEGIN
            SELECT 'ERROR' AS Result, N'Tên đăng nhập không được để trống' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @Password IS NULL OR LTRIM(RTRIM(@Password)) = ''
        BEGIN
            SELECT 'ERROR' AS Result, N'Mật khẩu không được để trống' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @Role IS NULL OR @Role NOT IN ('manager', 'saler', 'customer')
        BEGIN
            SELECT 'ERROR' AS Result, N'Vai trò phải là manager, saler hoặc customer' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra độ dài mật khẩu
        IF LEN(@Password) < 6
        BEGIN
            SELECT 'ERROR' AS Result, N'Mật khẩu phải có ít nhất 6 ký tự' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra username đã tồn tại chưa
        IF EXISTS(SELECT 1 FROM dbo.Account WHERE Username = @Username)
        BEGIN
            SELECT 'ERROR' AS Result, N'Tên đăng nhập đã được sử dụng' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra ràng buộc theo role và tạo Employee nếu cần
        IF @Role IN ('manager', 'saler')
        BEGIN
            -- Tạo số điện thoại duy nhất để tránh UNIQUE constraint
            DECLARE @UniquePhone NVARCHAR(20);
            DECLARE @PhoneCounter INT = 1;
            
            -- Tìm số điện thoại duy nhất
            WHILE EXISTS (SELECT 1 FROM dbo.Employees WHERE Phone = '000000000' + CAST(@PhoneCounter AS NVARCHAR(10)))
            BEGIN
                SET @PhoneCounter = @PhoneCounter + 1;
            END
            
            SET @UniquePhone = '000000000' + CAST(@PhoneCounter AS NVARCHAR(10));
            
            -- Tạo Employee mới với thông tin mặc định (không chỉ định EmployeeID để để SQL tự động tạo)
            INSERT INTO dbo.Employees (EmployeeName, Phone, Address, Position, HireDate, Salary)
            VALUES (N'Nhân viên mới', @UniquePhone, N'Chưa cập nhật', 
                    CASE WHEN @Role = 'manager' THEN 'manager' ELSE 'saler' END, 
                    GETDATE(), 0);
            
            -- Lấy EmployeeID vừa được tạo
            SET @EmployeeID = SCOPE_IDENTITY();
            SET @CustomerID = NULL;
        END
        
        INSERT INTO dbo.Account (Username, Password, Role, CustomerID, EmployeeID)
        VALUES (@Username, @Password, @Role, @CustomerID, @EmployeeID);
        
        COMMIT TRANSACTION;
        SELECT 'SUCCESS' AS Result, N'Tạo tài khoản thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE UpdateAccount
    @Username NVARCHAR(50),
    @Password NVARCHAR(255) = NULL,
    @Role NVARCHAR(20) = NULL,
    @CustomerID INT = NULL,
    @EmployeeID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra dữ liệu đầu vào
        IF @Username IS NULL OR LTRIM(RTRIM(@Username)) = ''
        BEGIN
            SELECT 'ERROR' AS Result, N'Tên đăng nhập không được để trống' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra tài khoản có tồn tại không
        IF NOT EXISTS(SELECT 1 FROM dbo.Account WHERE Username = @Username)
        BEGIN
            SELECT 'ERROR' AS Result, N'Không tìm thấy tài khoản để cập nhật' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra mật khẩu nếu có cập nhật
        IF @Password IS NOT NULL
        BEGIN
            IF LEN(@Password) < 6
            BEGIN
                SELECT 'ERROR' AS Result, N'Mật khẩu phải có ít nhất 6 ký tự' AS Message;
                ROLLBACK TRANSACTION;
                RETURN;
            END
        END
        
        -- Kiểm tra role nếu có cập nhật
        IF @Role IS NOT NULL AND @Role NOT IN ('manager', 'saler')
        BEGIN
            SELECT 'ERROR' AS Result, N'Vai trò phải là manager hoặc saler' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Lấy role hiện tại nếu không cập nhật
        DECLARE @CurrentRole NVARCHAR(20);
        SELECT @CurrentRole = Role FROM dbo.Account WHERE Username = @Username;
        
        IF @Role IS NULL
            SET @Role = @CurrentRole;
        
        -- Kiểm tra ràng buộc theo role và tạo Employee nếu cần
        IF @Role IN ('manager', 'saler')
        BEGIN
            -- Nếu không có EmployeeID thì tạo Employee mới
            IF @EmployeeID IS NULL
            BEGIN
                -- Tạo số điện thoại duy nhất để tránh UNIQUE constraint
                DECLARE @UniquePhone NVARCHAR(20);
                DECLARE @PhoneCounter INT = 1;
                
                -- Tìm số điện thoại duy nhất
                WHILE EXISTS (SELECT 1 FROM dbo.Employees WHERE Phone = '000000000' + CAST(@PhoneCounter AS NVARCHAR(10)))
                BEGIN
                    SET @PhoneCounter = @PhoneCounter + 1;
                END
                
                SET @UniquePhone = '000000000' + CAST(@PhoneCounter AS NVARCHAR(10));
                
                -- Tạo Employee mới với thông tin mặc định (không chỉ định EmployeeID để để SQL tự động tạo)
                INSERT INTO dbo.Employees (EmployeeName, Phone, Address, Position, HireDate, Salary)
                VALUES (N'Nhân viên mới', @UniquePhone, N'Chưa cập nhật', 
                        CASE WHEN @Role = 'manager' THEN 'manager' ELSE 'saler' END, 
                        GETDATE(), 0);
                
                -- Lấy EmployeeID vừa được tạo
                SET @EmployeeID = SCOPE_IDENTITY();
            END
            
            SET @CustomerID = NULL;
        END
        
        -- Cập nhật tài khoản
        UPDATE dbo.Account 
        SET Password = ISNULL(@Password, Password),
            Role = @Role,
            CustomerID = @CustomerID,
            EmployeeID = @EmployeeID
        WHERE Username = @Username;
        
        COMMIT TRANSACTION;
        SELECT 'SUCCESS' AS Result, N'Cập nhật tài khoản thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE DeleteAccount
    @Username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra dữ liệu đầu vào
        IF @Username IS NULL OR LTRIM(RTRIM(@Username)) = ''
        BEGIN
            SELECT 'ERROR' AS Result, N'Tên đăng nhập không được để trống' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra tài khoản có tồn tại không
        IF NOT EXISTS(SELECT 1 FROM dbo.Account WHERE Username = @Username)
        BEGIN
            SELECT 'ERROR' AS Result, N'Không tìm thấy tài khoản để xóa' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra tài khoản có được sử dụng trong Transactions không
        IF EXISTS(SELECT 1 FROM dbo.Transactions WHERE CreatedBy = @Username)
        BEGIN
            SELECT 'ERROR' AS Result, N'Không thể xóa tài khoản đã có giao dịch' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        DELETE FROM dbo.Account WHERE Username = @Username;
        
        COMMIT TRANSACTION;
        SELECT 'SUCCESS' AS Result, N'Xóa tài khoản thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

--------------------------------------------------------
-- SQL ACCOUNT MANAGEMENT
--------------------------------------------------------
CREATE OR ALTER PROCEDURE CreateSQLAccount
    @Username NVARCHAR(50),
    @Password NVARCHAR(255),
    @Role NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @sqlString NVARCHAR(MAX);
    
    BEGIN TRY
        -- Kiểm tra dữ liệu đầu vào
        IF @Username IS NULL OR LTRIM(RTRIM(@Username)) = ''
        BEGIN
            SELECT 'ERROR' AS Result, N'Tên đăng nhập không được để trống' AS Message;
            RETURN;
        END
        
        IF @Password IS NULL OR LTRIM(RTRIM(@Password)) = ''
        BEGIN
            SELECT 'ERROR' AS Result, N'Mật khẩu không được để trống' AS Message;
            RETURN;
        END
        
        IF @Role IS NULL OR @Role NOT IN ('manager', 'saler')
        BEGIN
            SELECT 'ERROR' AS Result, N'Vai trò phải là manager hoặc saler' AS Message;
            RETURN;
        END
        
        -- Tạo Login (kiểm tra không tồn tại)
        IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = @Username)
        BEGIN
            SET @sqlString = 'CREATE LOGIN [' + @Username + '] WITH PASSWORD = ''' + @Password + 
                            ''', DEFAULT_DATABASE = [Minimart_SalesDB], CHECK_EXPIRATION = OFF, CHECK_POLICY = OFF';
            EXEC (@sqlString);
        END
        ELSE
        BEGIN
            SELECT 'WARNING' AS Result, N'Login đã tồn tại: ' + @Username AS Message;
            RETURN;
        END

        -- Tạo User (kiểm tra không tồn tại)
        IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = @Username)
        BEGIN
            SET @sqlString = 'CREATE USER [' + @Username + '] FOR LOGIN [' + @Username + ']';
            EXEC (@sqlString);
        END
        ELSE
        BEGIN
            SELECT 'WARNING' AS Result, N'User đã tồn tại: ' + @Username AS Message;
            RETURN;
        END

        -- Gán vào Role tương ứng theo role trong bảng Account
        IF (@Role = N'manager')
        BEGIN
            -- Kiểm tra role tồn tại trước khi gán
            IF EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Manager' AND type = 'R')
            BEGIN
                SET @sqlString = 'ALTER ROLE [MiniMart_Manager] ADD MEMBER [' + @Username + ']';
                EXEC (@sqlString);
            END
            ELSE
            BEGIN
                SELECT 'ERROR' AS Result, N'Role MiniMart_Manager không tồn tại' AS Message;
                RETURN;
            END
        END
        ELSE IF (@Role = N'saler')
        BEGIN
            -- Kiểm tra role tồn tại trước khi gán
            IF EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Saler' AND type = 'R')
            BEGIN
                SET @sqlString = 'ALTER ROLE [MiniMart_Saler] ADD MEMBER [' + @Username + ']';
                EXEC (@sqlString);
            END
            ELSE
            BEGIN
                SELECT 'ERROR' AS Result, N'Role MiniMart_Saler không tồn tại' AS Message;
                RETURN;
            END
        END
        
        SELECT 'SUCCESS' AS Result, N'Tạo SQL Account thành công cho user: ' + @Username AS Message;
        
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' AS Result, N'Lỗi khi tạo SQL Account: ' + ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE DeleteSQLAccount
    @Username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @sqlString NVARCHAR(MAX);
    
    BEGIN TRY
        -- Kiểm tra dữ liệu đầu vào
        IF @Username IS NULL OR LTRIM(RTRIM(@Username)) = ''
        BEGIN
            SELECT 'ERROR' AS Result, N'Tên đăng nhập không được để trống' AS Message;
            RETURN;
        END
        
        -- Xóa User khỏi Database Role trước
        IF EXISTS (SELECT * FROM sys.database_principals WHERE name = @Username)
        BEGIN
            -- Xóa khỏi tất cả roles
            DECLARE @roleName NVARCHAR(128);
            DECLARE role_cursor CURSOR FOR
                SELECT name FROM sys.database_principals 
                WHERE type = 'R' AND name IN ('MiniMart_Manager', 'MiniMart_Saler');
            
            OPEN role_cursor;
            FETCH NEXT FROM role_cursor INTO @roleName;
            
            WHILE @@FETCH_STATUS = 0
            BEGIN
                IF EXISTS (SELECT * FROM sys.database_role_members rm
                          JOIN sys.database_principals r ON rm.role_principal_id = r.principal_id
                          JOIN sys.database_principals m ON rm.member_principal_id = m.principal_id
                          WHERE r.name = @roleName AND m.name = @Username)
                BEGIN
                    SET @sqlString = 'ALTER ROLE [' + @roleName + '] DROP MEMBER [' + @Username + ']';
                    EXEC (@sqlString);
                END
                FETCH NEXT FROM role_cursor INTO @roleName;
            END
            
            CLOSE role_cursor;
            DEALLOCATE role_cursor;
            
            -- Xóa User
            SET @sqlString = 'DROP USER [' + @Username + ']';
            EXEC (@sqlString);
        END
        
        -- Xóa Login
        IF EXISTS (SELECT * FROM sys.server_principals WHERE name = @Username)
        BEGIN
            SET @sqlString = 'DROP LOGIN [' + @Username + ']';
            EXEC (@sqlString);
        END
        
        SELECT 'SUCCESS' AS Result, N'Xóa SQL Account thành công cho user: ' + @Username AS Message;
        
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' AS Result, N'Lỗi khi xóa SQL Account: ' + ERROR_MESSAGE() AS Message;
    END CATCH
END
GO
