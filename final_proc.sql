USE Minimart_SalesDB;
GO

-- RestoreProduct: Khôi phục sản phẩm nếu có cơ chế soft-delete hoặc bảng lưu trữ
USE Minimart_SalesDB;
GO

CREATE OR ALTER PROCEDURE RestoreProduct
    @ProductID INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        
        -- 1. Kiểm tra dữ liệu đầu vào
        IF @ProductID IS NULL OR @ProductID <= 0
        BEGIN
            SELECT 'ERROR' AS Result, N'ID sản phẩm không hợp lệ' AS Message;
            RETURN;
        END
        
        -- 2. Kiểm tra sản phẩm có tồn tại VÀ đang bị xóa mềm (IsDeleted = 1) không
        IF NOT EXISTS(SELECT 1 FROM dbo.Products WHERE ProductID = @ProductID AND IsDeleted = 1)
        BEGIN
             -- Kiểm tra nếu sản phẩm đang hoạt động
            IF EXISTS(SELECT 1 FROM dbo.Products WHERE ProductID = @ProductID AND IsDeleted = 0)
                 SELECT 'ERROR' AS Result, N'Sản phẩm này đang hoạt động, không cần khôi phục.' AS Message;
            ELSE
                 SELECT 'ERROR' AS Result, N'Không tìm thấy sản phẩm đã xóa mềm để khôi phục.' AS Message;
            
            RETURN;
        END
        
        -- 3. Khôi phục sản phẩm (đặt IsDeleted = 0)
        -- Lưu ý: Không có TRANSACTION, nếu UPDATE thành công thì hoàn tất.
        UPDATE dbo.Products
        SET IsDeleted = 0
        WHERE ProductID = @ProductID;
        
        SELECT 'SUCCESS' AS Result, N'Khôi phục sản phẩm thành công' AS Message;
        
    END TRY
    BEGIN CATCH
        -- Bắt lỗi hệ thống
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
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
        -- Validate
        IF @ProductName IS NULL OR LTRIM(RTRIM(@ProductName)) = ''
        BEGIN
            SELECT 'ERROR' AS Result, N'Tên sản phẩm không được để trống' AS Message; RETURN;
        END
        IF @Price <= 0
        BEGIN
            SELECT 'ERROR' AS Result, N'Giá sản phẩm phải lớn hơn 0' AS Message; RETURN;
        END
        IF @StockQuantity < 0
        BEGIN
            SELECT 'ERROR' AS Result, N'Số lượng tồn kho không được âm' AS Message; RETURN;
        END

        INSERT INTO dbo.Products (ProductName, Price, StockQuantity, Unit)
        VALUES (@ProductName, @Price, @StockQuantity, @Unit);

        SELECT 'SUCCESS' AS Result, N'Thêm sản phẩm thành công' AS Message;
    END TRY
    BEGIN CATCH
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
        -- Validate
        IF @ProductID IS NULL OR @ProductID <= 0
        BEGIN SELECT 'ERROR' AS Result, N'ID sản phẩm không hợp lệ' AS Message; RETURN; END
        IF @ProductName IS NULL OR LTRIM(RTRIM(@ProductName)) = ''
        BEGIN SELECT 'ERROR' AS Result, N'Tên sản phẩm không được để trống' AS Message; RETURN; END
        IF @Price <= 0
        BEGIN SELECT 'ERROR' AS Result, N'Giá sản phẩm phải lớn hơn 0' AS Message; RETURN; END
        IF @StockQuantity < 0
        BEGIN SELECT 'ERROR' AS Result, N'Số lượng tồn kho không được âm' AS Message; RETURN; END
        IF NOT EXISTS(SELECT 1 FROM dbo.Products WHERE ProductID = @ProductID)
        BEGIN SELECT 'ERROR' AS Result, N'Không tìm thấy sản phẩm để cập nhật' AS Message; RETURN; END

        UPDATE dbo.Products
        SET ProductName=@ProductName, Price=@Price, StockQuantity=@StockQuantity, Unit=@Unit
        WHERE ProductID=@ProductID;

        SELECT 'SUCCESS' AS Result, N'Cập nhật sản phẩm thành công' AS Message;
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE AddCustomer
    @CustomerName NVARCHAR(100),
    @Phone NVARCHAR(20),
    @Address NVARCHAR(255) = NULL,
    @LoyaltyPoints INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @CustomerName IS NULL OR LTRIM(RTRIM(@CustomerName)) = ''
        BEGIN SELECT 'ERROR' AS Result, N'Tên khách hàng không được để trống' AS Message; RETURN; END
        IF @Phone IS NULL OR LTRIM(RTRIM(@Phone)) = ''
        BEGIN SELECT 'ERROR' AS Result, N'Số điện thoại không được để trống' AS Message; RETURN; END
        IF EXISTS(SELECT 1 FROM dbo.Customers WHERE Phone = @Phone)
        BEGIN SELECT 'ERROR' AS Result, N'Số điện thoại đã được sử dụng' AS Message; RETURN; END

        INSERT INTO dbo.Customers (CustomerName, Phone, Address, LoyaltyPoints)
        VALUES (@CustomerName, @Phone, @Address, @LoyaltyPoints);

        SELECT 'SUCCESS' AS Result, N'Thêm khách hàng thành công' AS Message, SCOPE_IDENTITY() AS CustomerID;
    END TRY
    BEGIN CATCH
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
        IF @CustomerID IS NULL OR @CustomerID <= 0
        BEGIN SELECT 'ERROR' AS Result, N'ID khách hàng không hợp lệ' AS Message; RETURN; END
        IF @CustomerName IS NULL OR LTRIM(RTRIM(@CustomerName)) = ''
        BEGIN SELECT 'ERROR' AS Result, N'Tên khách hàng không được để trống' AS Message; RETURN; END
        IF @Phone IS NULL OR LTRIM(RTRIM(@Phone)) = ''
        BEGIN SELECT 'ERROR' AS Result, N'Số điện thoại không được để trống' AS Message; RETURN; END
        IF @LoyaltyPoints < 0
        BEGIN SELECT 'ERROR' AS Result, N'Điểm tích lũy không được âm' AS Message; RETURN; END
        IF NOT EXISTS(SELECT 1 FROM dbo.Customers WHERE CustomerID = @CustomerID)
        BEGIN SELECT 'ERROR' AS Result, N'Không tìm thấy khách hàng để cập nhật' AS Message; RETURN; END
        IF EXISTS(SELECT 1 FROM dbo.Customers WHERE Phone = @Phone AND CustomerID != @CustomerID)
        BEGIN SELECT 'ERROR' AS Result, N'Số điện thoại đã được sử dụng bởi khách hàng khác' AS Message; RETURN; END

        UPDATE dbo.Customers
        SET CustomerName=@CustomerName, Phone=@Phone, Address=@Address, LoyaltyPoints=@LoyaltyPoints
        WHERE CustomerID=@CustomerID;

        SELECT 'SUCCESS' AS Result, N'Cập nhật khách hàng thành công' AS Message;
    END TRY
    BEGIN CATCH
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
        IF @CustomerID IS NULL OR @CustomerID <= 0
        BEGIN SELECT 'ERROR' AS Result, N'ID khách hàng không hợp lệ' AS Message; RETURN; END
        IF NOT EXISTS(SELECT 1 FROM dbo.Customers WHERE CustomerID = @CustomerID)
        BEGIN SELECT 'ERROR' AS Result, N'Không tìm thấy khách hàng để xóa' AS Message; RETURN; END
        IF EXISTS(SELECT 1 FROM dbo.Sales WHERE CustomerID = @CustomerID)
        BEGIN SELECT 'ERROR' AS Result, N'Không thể xóa khách hàng đã mua hàng' AS Message; RETURN; END

        DELETE FROM dbo.Customers WHERE CustomerID = @CustomerID;

        SELECT 'SUCCESS' AS Result, N'Xóa khách hàng thành công' AS Message;
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO


CREATE OR ALTER PROCEDURE AddDiscount
    @ProductID INT, @DiscountType NVARCHAR(20), @DiscountValue DECIMAL(18,2),
    @StartDate DATETIME, @EndDate DATETIME, @IsActive BIT = 1, @CreatedBy NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @ProductID IS NULL OR @ProductID <= 0
        BEGIN SELECT 'ERROR' AS Result, N'ID sản phẩm không hợp lệ' AS Message; RETURN; END
        IF @DiscountType IS NULL OR @DiscountType NOT IN ('percentage','fixed')
        BEGIN SELECT 'ERROR' AS Result, N'Loại giảm giá phải là "percentage" hoặc "fixed"' AS Message; RETURN; END
        IF @DiscountValue IS NULL OR @DiscountValue <= 0
        BEGIN SELECT 'ERROR' AS Result, N'Giá trị giảm giá phải lớn hơn 0' AS Message; RETURN; END
        IF @StartDate IS NULL OR @EndDate IS NULL OR @StartDate >= @EndDate
        BEGIN SELECT 'ERROR' AS Result, N'Ngày bắt đầu phải nhỏ hơn ngày kết thúc' AS Message; RETURN; END
        IF NOT EXISTS(SELECT 1 FROM dbo.Products WHERE ProductID=@ProductID)
        BEGIN SELECT 'ERROR' AS Result, N'Không tìm thấy sản phẩm' AS Message; RETURN; END
        IF @DiscountType='percentage' AND @DiscountValue > 100
        BEGIN SELECT 'ERROR' AS Result, N'Giảm giá phần trăm không được vượt quá 100%' AS Message; RETURN; END

        INSERT INTO dbo.Discounts(ProductID,DiscountType,DiscountValue,StartDate,EndDate,IsActive,CreatedDate,CreatedBy)
        VALUES(@ProductID,@DiscountType,@DiscountValue,@StartDate,@EndDate,@IsActive,GETDATE(),@CreatedBy);

        SELECT 'SUCCESS' AS Result, N'Thêm chương trình giảm giá thành công' AS Message;
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE UpdateDiscount
    @DiscountID INT, @DiscountType NVARCHAR(20), @DiscountValue DECIMAL(18,2),
    @StartDate DATETIME, @EndDate DATETIME, @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @DiscountID IS NULL OR @DiscountID <= 0
        BEGIN SELECT 'ERROR' AS Result, N'ID giảm giá không hợp lệ' AS Message; RETURN; END
        IF @DiscountType IS NULL OR @DiscountType NOT IN ('percentage','fixed')
        BEGIN SELECT 'ERROR' AS Result, N'Loại giảm giá phải là "percentage" hoặc "fixed"' AS Message; RETURN; END
        IF @DiscountValue IS NULL OR @DiscountValue <= 0
        BEGIN SELECT 'ERROR' AS Result, N'Giá trị giảm giá phải lớn hơn 0' AS Message; RETURN; END
        IF @StartDate IS NULL OR @EndDate IS NULL OR @StartDate >= @EndDate
        BEGIN SELECT 'ERROR' AS Result, N'Ngày bắt đầu phải nhỏ hơn ngày kết thúc' AS Message; RETURN; END
        IF NOT EXISTS(SELECT 1 FROM dbo.Discounts WHERE DiscountID=@DiscountID)
        BEGIN SELECT 'ERROR' AS Result, N'Không tìm thấy chương trình giảm giá để cập nhật' AS Message; RETURN; END
        IF @DiscountType='percentage' AND @DiscountValue > 100
        BEGIN SELECT 'ERROR' AS Result, N'Giảm giá phần trăm không được vượt quá 100%' AS Message; RETURN; END

        UPDATE dbo.Discounts
        SET DiscountType=@DiscountType, DiscountValue=@DiscountValue,
            StartDate=@StartDate, EndDate=@EndDate, IsActive=@IsActive
        WHERE DiscountID=@DiscountID;

        SELECT 'SUCCESS' AS Result, N'Cập nhật chương trình giảm giá thành công' AS Message;
    END TRY
    BEGIN CATCH
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
        IF @DiscountID IS NULL OR @DiscountID <= 0
        BEGIN SELECT 'ERROR' AS Result, N'ID giảm giá không hợp lệ' AS Message; RETURN; END
        IF NOT EXISTS(SELECT 1 FROM dbo.Discounts WHERE DiscountID=@DiscountID)
        BEGIN SELECT 'ERROR' AS Result, N'Không tìm thấy chương trình giảm giá để xóa' AS Message; RETURN; END

        DELETE FROM dbo.Discounts WHERE DiscountID=@DiscountID;

        SELECT 'SUCCESS' AS Result, N'Xóa chương trình giảm giá thành công' AS Message;
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE ChangePassword
    @Username NVARCHAR(50), @OldPassword NVARCHAR(255), @NewPassword NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @Username IS NULL OR LTRIM(RTRIM(@Username)) = ''
        BEGIN SELECT 'ERROR' AS Result, N'Tên đăng nhập không được để trống' AS Message; RETURN; END
        IF @OldPassword IS NULL OR LTRIM(RTRIM(@OldPassword)) = ''
        BEGIN SELECT 'ERROR' AS Result, N'Mật khẩu cũ không được để trống' AS Message; RETURN; END
        IF @NewPassword IS NULL OR LTRIM(RTRIM(@NewPassword)) = ''
        BEGIN SELECT 'ERROR' AS Result, N'Mật khẩu mới không được để trống' AS Message; RETURN; END
        IF LEN(@NewPassword) < 6
        BEGIN SELECT 'ERROR' AS Result, N'Mật khẩu mới phải có ít nhất 6 ký tự' AS Message; RETURN; END
        IF NOT EXISTS(SELECT 1 FROM dbo.Account WHERE Username=@Username AND Password=@OldPassword)
        BEGIN SELECT 'ERROR' AS Result, N'Mật khẩu cũ không đúng' AS Message; RETURN; END

        UPDATE dbo.Account SET Password=@NewPassword WHERE Username=@Username;

        SELECT 'SUCCESS' AS Result, N'Đổi mật khẩu thành công' AS Message;
    END TRY
    BEGIN CATCH
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
        IF @Username IS NULL OR LTRIM(RTRIM(@Username)) = ''
        BEGIN SELECT 'ERROR' AS Result, N'Tên đăng nhập không được để trống' AS Message; RETURN; END
        IF NOT EXISTS(SELECT 1 FROM dbo.Account WHERE Username=@Username)
        BEGIN SELECT 'ERROR' AS Result, N'Không tìm thấy tài khoản để cập nhật' AS Message; RETURN; END
        IF @Password IS NOT NULL AND LEN(@Password) < 6
        BEGIN SELECT 'ERROR' AS Result, N'Mật khẩu phải có ít nhất 6 ký tự' AS Message; RETURN; END
        IF @Role IS NOT NULL AND @Role NOT IN ('manager','saler')
        BEGIN SELECT 'ERROR' AS Result, N'Vai trò phải là manager hoặc saler' AS Message; RETURN; END

        DECLARE @CurrentRole NVARCHAR(20);
        SELECT @CurrentRole = Role FROM dbo.Account WHERE Username=@Username;
        IF @Role IS NULL SET @Role = @CurrentRole;

        IF @Role IN ('manager','saler') AND @EmployeeID IS NULL
        BEGIN
            DECLARE @UniquePhone NVARCHAR(20);
            DECLARE @PhoneCounter INT = 1;

            WHILE EXISTS (SELECT 1 FROM dbo.Employees WHERE Phone = '000000000' + CAST(@PhoneCounter AS NVARCHAR(10)))
            BEGIN
                SET @PhoneCounter += 1;
            END
            SET @UniquePhone = '000000000' + CAST(@PhoneCounter AS NVARCHAR(10));

            INSERT INTO dbo.Employees (EmployeeName, Phone, Address, Position, HireDate, Salary)
            VALUES (N'Nhân viên mới', @UniquePhone, N'Chưa cập nhật',
                    CASE WHEN @Role='manager' THEN 'manager' ELSE 'saler' END,
                    GETDATE(), 0);

            SET @EmployeeID = SCOPE_IDENTITY();
            SET @CustomerID = NULL;
        END

        UPDATE dbo.Account
        SET Password = ISNULL(@Password, Password),
            Role = @Role,
            CustomerID = @CustomerID,
            EmployeeID = @EmployeeID
        WHERE Username = @Username;

        SELECT 'SUCCESS' AS Result, N'Cập nhật tài khoản thành công' AS Message;
    END TRY
    BEGIN CATCH
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
        IF @Username IS NULL OR LTRIM(RTRIM(@Username)) = ''
        BEGIN SELECT 'ERROR' AS Result, N'Tên đăng nhập không được để trống' AS Message; RETURN; END
        IF NOT EXISTS(SELECT 1 FROM dbo.Account WHERE Username=@Username)
        BEGIN SELECT 'ERROR' AS Result, N'Không tìm thấy tài khoản để xóa' AS Message; RETURN; END
        IF EXISTS(SELECT 1 FROM dbo.Transactions WHERE CreatedBy=@Username)
        BEGIN SELECT 'ERROR' AS Result, N'Không thể xóa tài khoản đã có giao dịch' AS Message; RETURN; END

        DELETE FROM dbo.Account WHERE Username=@Username;

        SELECT 'SUCCESS' AS Result, N'Xóa tài khoản thành công' AS Message;
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO
