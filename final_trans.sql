USE Minimart_SalesDB;
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
        
        -- Kiểm tra sản phẩm có tồn tại và CHƯA BỊ XÓA mềm không
        IF NOT EXISTS(SELECT 1 FROM dbo.Products WHERE ProductID = @ProductID AND IsDeleted = 0)
        BEGIN
            IF EXISTS(SELECT 1 FROM dbo.Products WHERE ProductID = @ProductID AND IsDeleted = 1)
                 SELECT 'ERROR' AS Result, N'Sản phẩm này đã bị xóa mềm trước đó.' AS Message;
            ELSE
                 SELECT 'ERROR' AS Result, N'Không tìm thấy sản phẩm để xóa' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra sản phẩm có trong hóa đơn không (Giữ nguyên ràng buộc)
        IF EXISTS(SELECT 1 FROM SaleDetails WHERE ProductID = @ProductID)
        BEGIN
            SELECT 'ERROR' AS Result, N'Không thể xóa sản phẩm đã có trong hóa đơn' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Kiểm tra sản phẩm có discount không (Giữ nguyên ràng buộc)
        IF EXISTS(SELECT 1 FROM Discounts WHERE ProductID = @ProductID)
        BEGIN
            SELECT 'ERROR' AS Result, N'Không thể xóa sản phẩm đang có chương trình giảm giá' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- THAY ĐỔI: Chuyển sang XÓA MỀM (Soft Delete)
        UPDATE dbo.Products 
        SET IsDeleted = 1 
        WHERE ProductID = @ProductID;
        
        COMMIT TRANSACTION;
        SELECT 'SUCCESS' AS Result, N'Xóa mềm sản phẩm thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE CreateSale
    @CustomerID INT = NULL,
    @TotalAmount DECIMAL(18,2),
    @PaymentMethod NVARCHAR(50) = NULL,
    @CreatedBy NVARCHAR(50) = 'system'
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @SaleID INT;

    BEGIN TRY
        -- Validate trước khi mở TRAN
        IF @TotalAmount IS NULL OR @TotalAmount <= 0
        BEGIN SELECT 'ERROR' AS Result, N'Tổng tiền phải lớn hơn 0' AS Message; RETURN; END
        IF @CustomerID IS NOT NULL
           AND NOT EXISTS(SELECT 1 FROM dbo.Customers WHERE CustomerID = @CustomerID)
        BEGIN SELECT 'ERROR' AS Result, N'Không tìm thấy khách hàng' AS Message; RETURN; END

        BEGIN TRAN;
            INSERT INTO dbo.Sales(CustomerID, SaleDate, TotalAmount, PaymentMethod)
            VALUES(@CustomerID, GETDATE(), @TotalAmount, @PaymentMethod);

            SET @SaleID = SCOPE_IDENTITY();

            INSERT INTO dbo.Transactions(
                TransactionType, Amount, Description, TransactionDate, CreatedBy, ReferenceID, ReferenceType)
            VALUES(
                'income', @TotalAmount,
                N'Thu tiền từ bán hàng - Hóa đơn #' + CAST(@SaleID AS NVARCHAR(20)),
                GETDATE(), @CreatedBy, @SaleID, 'sale'
            );
        COMMIT TRAN;

        SELECT 'SUCCESS' AS Result, N'Tạo hóa đơn thành công' AS Message, @SaleID AS SaleID;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE AddSaleDetail
    @SaleID INT,
    @ProductID INT,
    @Quantity INT,
    @SalePrice DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        -- Validate trước khi mở TRAN
        IF @SaleID IS NULL OR @SaleID <= 0
        BEGIN SELECT 'ERROR' AS Result, N'ID hóa đơn không hợp lệ' AS Message; RETURN; END
        IF @ProductID IS NULL OR @ProductID <= 0
        BEGIN SELECT 'ERROR' AS Result, N'ID sản phẩm không hợp lệ' AS Message; RETURN; END
        IF @Quantity IS NULL OR @Quantity <= 0
        BEGIN SELECT 'ERROR' AS Result, N'Số lượng phải lớn hơn 0' AS Message; RETURN; END
        IF @SalePrice IS NULL OR @SalePrice <= 0
        BEGIN SELECT 'ERROR' AS Result, N'Giá bán phải lớn hơn 0' AS Message; RETURN; END
        IF NOT EXISTS(SELECT 1 FROM dbo.Sales WHERE SaleID = @SaleID)
        BEGIN SELECT 'ERROR' AS Result, N'Không tìm thấy hóa đơn' AS Message; RETURN; END
        IF NOT EXISTS(SELECT 1 FROM dbo.Products WHERE ProductID = @ProductID)
        BEGIN SELECT 'ERROR' AS Result, N'Không tìm thấy sản phẩm' AS Message; RETURN; END
        IF NOT EXISTS(SELECT 1 FROM dbo.Products WHERE ProductID = @ProductID AND StockQuantity >= @Quantity)
        BEGIN SELECT 'ERROR' AS Result, N'Không đủ hàng trong kho' AS Message; RETURN; END

        BEGIN TRAN;
            INSERT INTO dbo.SaleDetails(SaleID, ProductID, Quantity, SalePrice, LineTotal)
            VALUES(@SaleID, @ProductID, @Quantity, @SalePrice, @Quantity * @SalePrice);

            UPDATE dbo.Products
            SET StockQuantity = StockQuantity - @Quantity
            WHERE ProductID = @ProductID;
        COMMIT TRAN;

        SELECT 'SUCCESS' AS Result, N'Thêm chi tiết hóa đơn thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE UpdateSale
    @SaleID INT,
    @CustomerID INT = NULL,
    @TotalAmount DECIMAL(18,2),
    @PaymentMethod NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        -- Validate trước khi mở TRAN
        IF @SaleID IS NULL OR @SaleID <= 0
        BEGIN SELECT 'ERROR' AS Result, N'ID hóa đơn không hợp lệ' AS Message; RETURN; END
        IF @TotalAmount IS NULL OR @TotalAmount <= 0
        BEGIN SELECT 'ERROR' AS Result, N'Tổng tiền phải lớn hơn 0' AS Message; RETURN; END
        IF NOT EXISTS(SELECT 1 FROM dbo.Sales WHERE SaleID = @SaleID)
        BEGIN SELECT 'ERROR' AS Result, N'Không tìm thấy hóa đơn' AS Message; RETURN; END
        IF @CustomerID IS NOT NULL
           AND NOT EXISTS(SELECT 1 FROM dbo.Customers WHERE CustomerID = @CustomerID)
        BEGIN SELECT 'ERROR' AS Result, N'Không tìm thấy khách hàng' AS Message; RETURN; END

        DECLARE @OldTotalAmount DECIMAL(18,2);
        SELECT @OldTotalAmount = TotalAmount FROM dbo.Sales WHERE SaleID = @SaleID;

        BEGIN TRAN;
            UPDATE dbo.Sales
            SET CustomerID=@CustomerID, TotalAmount=@TotalAmount, PaymentMethod=@PaymentMethod
            WHERE SaleID=@SaleID;

            IF @OldTotalAmount <> @TotalAmount
            BEGIN
                UPDATE dbo.Transactions
                SET Amount = @TotalAmount,
                    Description = N'Thu tiền từ bán hàng - Hóa đơn #' + CAST(@SaleID AS NVARCHAR(20))
                WHERE ReferenceID = @SaleID AND ReferenceType = 'sale';
            END
        COMMIT TRAN;

        SELECT 'SUCCESS' AS Result, N'Cập nhật hóa đơn thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
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
    SET XACT_ABORT ON;

    BEGIN TRY
        -- Validate trước khi mở TRAN
        IF @Username IS NULL OR LTRIM(RTRIM(@Username)) = ''
        BEGIN SELECT 'ERROR' AS Result, N'Tên đăng nhập không được để trống' AS Message; RETURN; END
        IF @Password IS NULL OR LTRIM(RTRIM(@Password)) = ''
        BEGIN SELECT 'ERROR' AS Result, N'Mật khẩu không được để trống' AS Message; RETURN; END
        IF @Role IS NULL OR @Role NOT IN ('manager','saler','customer')
        BEGIN SELECT 'ERROR' AS Result, N'Vai trò phải là manager, saler hoặc customer' AS Message; RETURN; END
        IF LEN(@Password) < 6
        BEGIN SELECT 'ERROR' AS Result, N'Mật khẩu phải có ít nhất 6 ký tự' AS Message; RETURN; END
        IF EXISTS(SELECT 1 FROM dbo.Account WHERE Username=@Username)
        BEGIN SELECT 'ERROR' AS Result, N'Tên đăng nhập đã được sử dụng' AS Message; RETURN; END

        BEGIN TRAN;
            -- Auto-create Employee nếu là manager/saler
            IF @Role IN ('manager','saler')
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

            INSERT INTO dbo.Account (Username, Password, Role, CustomerID, EmployeeID)
            VALUES (@Username, @Password, @Role, @CustomerID, @EmployeeID);
        COMMIT TRAN;

        SELECT 'SUCCESS' AS Result, N'Tạo tài khoản thành công' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        SELECT 'ERROR' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO
