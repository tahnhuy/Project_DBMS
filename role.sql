-- ========================================
-- ROLE.SQL - PHÂN QUYỀN HỆ THỐNG QUẢN LÝ BÁN HÀNG MINIMART
-- ========================================

USE Minimart_SalesDB
GO

-- ========================================
-- 1. TẠO CÁC ROLE TÙY CHỈNH TRƯỚC
-- ========================================

-- Tạo các role tùy chỉnh theo cấu trúc thực tế
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Manager' AND type = 'R')
    CREATE ROLE MiniMart_Manager;

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Saler' AND type = 'R')
    CREATE ROLE MiniMart_Saler;

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Customer' AND type = 'R')
    CREATE ROLE MiniMart_Customer;

-- ========================================
-- 2. TẠO CÁC LOGIN VÀ USER CHO HỆ THỐNG
-- ========================================

-- Tạo Login cho các role chính (theo bảng Account: manager, saler, customer)
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'ManagerLogin')
    CREATE LOGIN ManagerLogin WITH PASSWORD = 'Manager@123!', CHECK_POLICY = OFF;

IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'SalerLogin')
    CREATE LOGIN SalerLogin WITH PASSWORD = 'Saler@123!', CHECK_POLICY = OFF;

IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'CustomerLogin')
    CREATE LOGIN CustomerLogin WITH PASSWORD = 'Customer@123!', CHECK_POLICY = OFF;

-- Tạo User trong database
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'ManagerUser')
    CREATE USER ManagerUser FOR LOGIN ManagerLogin;

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'SalerUser')
    CREATE USER SalerUser FOR LOGIN SalerLogin;

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'CustomerUser')
    CREATE USER CustomerUser FOR LOGIN CustomerLogin;

-- Gán user vào role (sau khi đã tạo cả role và user)
ALTER ROLE MiniMart_Manager ADD MEMBER ManagerUser;
ALTER ROLE MiniMart_Saler ADD MEMBER SalerUser;
ALTER ROLE MiniMart_Customer ADD MEMBER CustomerUser;

-- ========================================
-- 3. PHÂN QUYỀN CHO ROLE MANAGER (QUẢN LÝ)
-- ========================================

-- === QUYỀN TRÊN CÁC BẢNG ===
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Products TO MiniMart_Manager;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Customers TO MiniMart_Manager;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Sales TO MiniMart_Manager;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.SaleDetails TO MiniMart_Manager;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Discounts TO MiniMart_Manager;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Transactions TO MiniMart_Manager;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Account TO MiniMart_Manager;

-- === STORED PROCEDURES - QUẢN LÝ SẢN PHẨM ===
GRANT EXECUTE ON dbo.GetAllProducts TO MiniMart_Manager;
GRANT EXECUTE ON dbo.AddProduct TO MiniMart_Manager;
GRANT EXECUTE ON dbo.UpdateProduct TO MiniMart_Manager;
GRANT EXECUTE ON dbo.DeleteProduct TO MiniMart_Manager;

-- === STORED PROCEDURES - QUẢN LÝ KHÁCH HÀNG ===
GRANT EXECUTE ON dbo.GetAllCustomers TO MiniMart_Manager;
GRANT EXECUTE ON dbo.GetCustomerByName TO MiniMart_Manager;
GRANT EXECUTE ON dbo.GetCustomerByID TO MiniMart_Manager;
GRANT EXECUTE ON dbo.AddCustomer TO MiniMart_Manager;
GRANT EXECUTE ON dbo.UpdateCustomer TO MiniMart_Manager;
GRANT EXECUTE ON dbo.DeleteCustomer TO MiniMart_Manager;
GRANT SELECT ON dbo.SearchCustomers TO MiniMart_Manager;
GRANT EXECUTE ON dbo.GetCustomerByUsername TO MiniMart_Manager;
GRANT EXECUTE ON dbo.UpdateCustomerByUsername TO MiniMart_Manager;

-- === STORED PROCEDURES - QUẢN LÝ BÁN HÀNG ===
GRANT EXECUTE ON dbo.GetAllSales TO MiniMart_Manager;
GRANT EXECUTE ON dbo.GetSaleByID TO MiniMart_Manager;
GRANT EXECUTE ON dbo.GetSaleDetails TO MiniMart_Manager;
GRANT EXECUTE ON dbo.CreateSale TO MiniMart_Manager;
GRANT EXECUTE ON dbo.AddSaleDetail TO MiniMart_Manager;
GRANT EXECUTE ON dbo.UpdateSale TO MiniMart_Manager;
GRANT EXECUTE ON dbo.DeleteSale TO MiniMart_Manager;

-- === STORED PROCEDURES - QUẢN LÝ GIẢM GIÁ ===
GRANT EXECUTE ON dbo.GetAllDiscounts TO MiniMart_Manager;
GRANT EXECUTE ON dbo.GetDiscountsByProduct TO MiniMart_Manager;
GRANT EXECUTE ON dbo.GetActiveDiscounts TO MiniMart_Manager;
GRANT EXECUTE ON dbo.AddDiscount TO MiniMart_Manager;
GRANT EXECUTE ON dbo.UpdateDiscount TO MiniMart_Manager;
GRANT EXECUTE ON dbo.DeleteDiscount TO MiniMart_Manager;

-- === STORED PROCEDURES - QUẢN LÝ TÀI KHOẢN ===
GRANT EXECUTE ON dbo.GetAllAccounts TO MiniMart_Manager;
GRANT EXECUTE ON dbo.CheckLogin TO MiniMart_Manager;
GRANT EXECUTE ON dbo.AddAccount TO MiniMart_Manager;
GRANT EXECUTE ON dbo.UpdateAccount TO MiniMart_Manager;
GRANT EXECUTE ON dbo.DeleteAccount TO MiniMart_Manager;
GRANT EXECUTE ON dbo.GetAccountDetails TO MiniMart_Manager;
GRANT EXECUTE ON dbo.CheckAccountExists TO MiniMart_Manager;
GRANT EXECUTE ON dbo.SearchAccounts TO MiniMart_Manager;
GRANT EXECUTE ON dbo.GetAccountsByRole TO MiniMart_Manager;
GRANT EXECUTE ON dbo.CountAccountsByRole TO MiniMart_Manager;
GRANT EXECUTE ON dbo.CheckUserPermission TO MiniMart_Manager;
GRANT EXECUTE ON dbo.ChangePassword TO MiniMart_Manager;
GRANT EXECUTE ON dbo.ResetPassword TO MiniMart_Manager;
GRANT EXECUTE ON dbo.GetAccountStatistics TO MiniMart_Manager;
GRANT EXECUTE ON dbo.GetAccountActivity TO MiniMart_Manager;
GRANT EXECUTE ON dbo.ValidateUsername TO MiniMart_Manager;
GRANT EXECUTE ON dbo.ValidatePassword TO MiniMart_Manager;
GRANT EXECUTE ON dbo.GetAccountsWithDetails TO MiniMart_Manager;
GRANT EXECUTE ON dbo.BackupAccounts TO MiniMart_Manager;

-- === STORED PROCEDURES - GIAO DỊCH ===
GRANT EXECUTE ON dbo.GetTransactionsByUsername TO MiniMart_Manager;
GRANT EXECUTE ON dbo.GetSalesByCustomerUsername TO MiniMart_Manager;
GRANT EXECUTE ON dbo.LinkAccountToCustomer TO MiniMart_Manager;

-- === FUNCTIONS ===
-- Scalar Functions (returns single value) - sử dụng EXECUTE
GRANT EXECUTE ON dbo.GetDiscountedPrice TO MiniMart_Manager;           -- returns decimal(18, 2)
GRANT EXECUTE ON dbo.GetDailyRevenue TO MiniMart_Manager;              -- returns decimal(18, 2)
GRANT EXECUTE ON dbo.GetMonthlyRevenue TO MiniMart_Manager;            -- returns decimal(18, 2)
GRANT EXECUTE ON dbo.IsStockAvailable TO MiniMart_Manager;            -- returns bit
GRANT EXECUTE ON dbo.CalculateLoyaltyPoints TO MiniMart_Manager;       -- returns int
GRANT EXECUTE ON dbo.CalculateDiscountPercentage TO MiniMart_Manager;  -- returns decimal(5,2)
GRANT EXECUTE ON dbo.IsValidVietnamesePhone TO MiniMart_Manager;       -- returns bit
GRANT EXECUTE ON dbo.FormatVietnamMoney TO MiniMart_Manager;           -- returns nvarchar(50)
GRANT EXECUTE ON dbo.GetExpenseByType TO MiniMart_Manager;            -- returns decimal(18,2)

-- Table-valued Functions (returns table) - sử dụng SELECT
GRANT SELECT ON dbo.GetProductByName TO MiniMart_Manager;              -- returns table
GRANT SELECT ON dbo.GetProductByID TO MiniMart_Manager;               -- returns table
GRANT SELECT ON dbo.GetTopSellingProducts TO MiniMart_Manager;        -- returns table
GRANT SELECT ON dbo.GetCustomerPurchaseHistory TO MiniMart_Manager;    -- returns table
GRANT SELECT ON dbo.SearchCustomers TO MiniMart_Manager;              -- returns table
GRANT SELECT ON dbo.GetProductRevenueReport TO MiniMart_Manager;      -- returns table
GRANT SELECT ON dbo.GetDashboardStats TO MiniMart_Manager;            -- returns table

-- === VIEWS ===
GRANT SELECT ON dbo.ProductsWithDiscounts TO MiniMart_Manager;
GRANT SELECT ON dbo.SalesSummary TO MiniMart_Manager;
GRANT SELECT ON dbo.ProductSalesStats TO MiniMart_Manager;
GRANT SELECT ON dbo.CustomerPurchaseSummary TO MiniMart_Manager;
GRANT SELECT ON dbo.MonthlySalesReport TO MiniMart_Manager;
GRANT SELECT ON dbo.DailySalesReport TO MiniMart_Manager;
GRANT SELECT ON dbo.LowStockProducts TO MiniMart_Manager;
GRANT SELECT ON dbo.ActiveDiscountsDetail TO MiniMart_Manager;
GRANT SELECT ON dbo.TransactionSummary TO MiniMart_Manager;
GRANT SELECT ON dbo.AccountSummary TO MiniMart_Manager;

-- ========================================
-- 4. PHÂN QUYỀN CHO ROLE SALER (NHÂN VIÊN BÁN HÀNG)
-- ========================================

-- === QUYỀN TRÊN CÁC BẢNG (CHỈ ĐỌC/THÊM/SỬA, KHÔNG XÓA) ===
GRANT SELECT, INSERT, UPDATE ON dbo.Products TO MiniMart_Saler;
GRANT SELECT, INSERT, UPDATE ON dbo.Customers TO MiniMart_Saler;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Sales TO MiniMart_Saler;
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.SaleDetails TO MiniMart_Saler;
GRANT SELECT ON dbo.Discounts TO MiniMart_Saler;
GRANT SELECT, INSERT ON dbo.Transactions TO MiniMart_Saler;
GRANT SELECT ON dbo.Account TO MiniMart_Saler;

-- === STORED PROCEDURES - QUẢN LÝ SẢN PHẨM (CHỈ XEM VÀ CẬP NHẬT) ===
GRANT EXECUTE ON dbo.GetAllProducts TO MiniMart_Saler;
GRANT EXECUTE ON dbo.AddProduct TO MiniMart_Saler;
GRANT EXECUTE ON dbo.UpdateProduct TO MiniMart_Saler;
-- KHÔNG CÓ quyền DeleteProduct

-- === STORED PROCEDURES - QUẢN LÝ KHÁCH HÀNG ===
GRANT EXECUTE ON dbo.GetAllCustomers TO MiniMart_Saler;
GRANT EXECUTE ON dbo.GetCustomerByName TO MiniMart_Saler;
GRANT EXECUTE ON dbo.GetCustomerByID TO MiniMart_Saler;
GRANT EXECUTE ON dbo.AddCustomer TO MiniMart_Saler;
GRANT EXECUTE ON dbo.UpdateCustomer TO MiniMart_Saler;
GRANT EXECUTE ON dbo.GetCustomerByUsername TO MiniMart_Saler;
GRANT EXECUTE ON dbo.UpdateCustomerByUsername TO MiniMart_Saler;
-- KHÔNG CÓ quyền DeleteCustomer

-- === STORED PROCEDURES - QUẢN LÝ BÁN HÀNG (TOÀN QUYỀN) ===
GRANT EXECUTE ON dbo.GetAllSales TO MiniMart_Saler;
GRANT EXECUTE ON dbo.GetSaleByID TO MiniMart_Saler;
GRANT EXECUTE ON dbo.GetSaleDetails TO MiniMart_Saler;
GRANT EXECUTE ON dbo.CreateSale TO MiniMart_Saler;
GRANT EXECUTE ON dbo.AddSaleDetail TO MiniMart_Saler;
GRANT EXECUTE ON dbo.UpdateSale TO MiniMart_Saler;
GRANT EXECUTE ON dbo.DeleteSale TO MiniMart_Saler;

-- === STORED PROCEDURES - GIẢM GIÁ (CHỈ XEM) ===
GRANT EXECUTE ON dbo.GetAllDiscounts TO MiniMart_Saler;
GRANT EXECUTE ON dbo.GetDiscountsByProduct TO MiniMart_Saler;
GRANT EXECUTE ON dbo.GetActiveDiscounts TO MiniMart_Saler;
-- KHÔNG CÓ quyền thêm/sửa/xóa discount

-- === STORED PROCEDURES - TÀI KHOẢN (GIỚI HẠN) ===
GRANT EXECUTE ON dbo.CheckLogin TO MiniMart_Saler;
GRANT EXECUTE ON dbo.GetAccountDetails TO MiniMart_Saler;
GRANT EXECUTE ON dbo.ChangePassword TO MiniMart_Saler;
GRANT EXECUTE ON dbo.ValidateUsername TO MiniMart_Saler;
GRANT EXECUTE ON dbo.ValidatePassword TO MiniMart_Saler;
-- KHÔNG CÓ quyền quản lý account khác

-- === STORED PROCEDURES - GIAO DỊCH ===
GRANT EXECUTE ON dbo.GetTransactionsByUsername TO MiniMart_Saler;
GRANT EXECUTE ON dbo.GetSalesByCustomerUsername TO MiniMart_Saler;

-- === FUNCTIONS ===
-- Scalar Functions (returns single value) - sử dụng EXECUTE
GRANT EXECUTE ON dbo.GetDiscountedPrice TO MiniMart_Saler;           -- returns decimal(18, 2)
GRANT EXECUTE ON dbo.GetDailyRevenue TO MiniMart_Saler;              -- returns decimal(18, 2)
GRANT EXECUTE ON dbo.IsStockAvailable TO MiniMart_Saler;              -- returns bit
GRANT EXECUTE ON dbo.CalculateLoyaltyPoints TO MiniMart_Saler;      -- returns int
GRANT EXECUTE ON dbo.IsValidVietnamesePhone TO MiniMart_Saler;        -- returns bit
GRANT EXECUTE ON dbo.FormatVietnamMoney TO MiniMart_Saler;           -- returns nvarchar(50)

-- Table-valued Functions (returns table) - sử dụng SELECT
GRANT SELECT ON dbo.GetProductByName TO MiniMart_Saler;              -- returns table
GRANT SELECT ON dbo.GetProductByID TO MiniMart_Saler;                -- returns table
GRANT SELECT ON dbo.GetTopSellingProducts TO MiniMart_Saler;         -- returns table
GRANT SELECT ON dbo.GetCustomerPurchaseHistory TO MiniMart_Saler;    -- returns table
GRANT SELECT ON dbo.SearchCustomers TO MiniMart_Saler;                -- returns table
GRANT SELECT ON dbo.GetDashboardStats TO MiniMart_Saler;             -- returns table

-- === VIEWS ===
GRANT SELECT ON dbo.ProductsWithDiscounts TO MiniMart_Saler;
GRANT SELECT ON dbo.SalesSummary TO MiniMart_Saler;
GRANT SELECT ON dbo.ProductSalesStats TO MiniMart_Saler;
GRANT SELECT ON dbo.CustomerPurchaseSummary TO MiniMart_Saler;
GRANT SELECT ON dbo.DailySalesReport TO MiniMart_Saler;
GRANT SELECT ON dbo.LowStockProducts TO MiniMart_Saler;
GRANT SELECT ON dbo.ActiveDiscountsDetail TO MiniMart_Saler;

-- ========================================
-- 5. PHÂN QUYỀN CHO ROLE CUSTOMER (KHÁCH HÀNG)
-- ========================================

-- === QUYỀN TRÊN CÁC BẢNG (CHỈ ĐỌC THÔNG TIN CỦA MÌNH) ===
GRANT SELECT ON dbo.Products TO MiniMart_Customer;
GRANT SELECT ON dbo.Discounts TO MiniMart_Customer;
-- KHÔNG CÓ quyền trực tiếp trên các bảng khác

-- === STORED PROCEDURES - SẢN PHẨM (CHỈ XEM) ===
GRANT EXECUTE ON dbo.GetAllProducts TO MiniMart_Customer;

-- === STORED PROCEDURES - KHÁCH HÀNG (CHỈ XEM/CẬP NHẬT THÔNG TIN MÌNH) ===
GRANT EXECUTE ON dbo.GetCustomerByUsername TO MiniMart_Customer;
GRANT EXECUTE ON dbo.UpdateCustomerByUsername TO MiniMart_Customer;
-- KHÔNG CÓ quyền xem thông tin khách hàng khác

-- === STORED PROCEDURES - BÁN HÀNG (CHỈ XEM HÓA ĐƠN CỦA MÌNH) ===
GRANT EXECUTE ON dbo.GetSalesByCustomerUsername TO MiniMart_Customer;
-- KHÔNG CÓ quyền tạo/sửa/xóa hóa đơn

-- === STORED PROCEDURES - GIẢM GIÁ (CHỈ XEM) ===
GRANT EXECUTE ON dbo.GetActiveDiscounts TO MiniMart_Customer;

-- === STORED PROCEDURES - TÀI KHOẢN (GIỚI HẠN) ===
GRANT EXECUTE ON dbo.CheckLogin TO MiniMart_Customer;
GRANT EXECUTE ON dbo.GetAccountDetails TO MiniMart_Customer;
GRANT EXECUTE ON dbo.ChangePassword TO MiniMart_Customer;

-- === STORED PROCEDURES - GIAO DỊCH (CHỈ CỦA MÌNH) ===
GRANT EXECUTE ON dbo.GetTransactionsByUsername TO MiniMart_Customer;

-- === FUNCTIONS (GIỚI HẠN) ===
-- Scalar Functions (returns single value) - sử dụng EXECUTE
GRANT EXECUTE ON dbo.GetDiscountedPrice TO MiniMart_Customer;       -- returns decimal(18, 2)
GRANT EXECUTE ON dbo.CalculateLoyaltyPoints TO MiniMart_Customer;    -- returns int
GRANT EXECUTE ON dbo.IsValidVietnamesePhone TO MiniMart_Customer;   -- returns bit
GRANT EXECUTE ON dbo.FormatVietnamMoney TO MiniMart_Customer;       -- returns nvarchar(50)

-- Table-valued Functions (returns table) - sử dụng SELECT
GRANT SELECT ON dbo.GetProductByName TO MiniMart_Customer;          -- returns table
GRANT SELECT ON dbo.GetProductByID TO MiniMart_Customer;             -- returns table
GRANT SELECT ON dbo.GetCustomerPurchaseHistory TO MiniMart_Customer; -- returns table

-- === VIEWS (GIỚI HẠN) ===
GRANT SELECT ON dbo.ProductsWithDiscounts TO MiniMart_Customer;
GRANT SELECT ON dbo.ActiveDiscountsDetail TO MiniMart_Customer;
go
-- KHÔNG CÓ quyền xem các báo cáo doanh thu, thông tin khách hàng khác

-- ========================================
-- 6. TẠO TRIGGER TỰ ĐỘNG TẠO TÀI KHOẢN SQL
-- ========================================

-- Trigger tự động tạo Login và User khi thêm record vào bảng Account
CREATE OR ALTER TRIGGER trg_CreateSQLAccount
ON dbo.Account
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @username NVARCHAR(50), @password NVARCHAR(255), @role NVARCHAR(20);
    DECLARE @sqlString NVARCHAR(MAX);
    
    -- Lấy thông tin từ record mới được thêm
    SELECT @username = i.Username,
           @password = i.Password,
           @role = i.Role
    FROM inserted i;

    BEGIN TRY
        -- Tạo Login (kiểm tra không tồn tại)
        IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = @username)
        BEGIN
            SET @sqlString = 'CREATE LOGIN [' + @username + '] WITH PASSWORD = ''' + @password + 
                            ''', DEFAULT_DATABASE = [Minimart_SalesDB], CHECK_EXPIRATION = OFF, CHECK_POLICY = OFF';
            EXEC (@sqlString);
        END

        -- Tạo User (kiểm tra không tồn tại)
        IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = @username)
        BEGIN
            SET @sqlString = 'CREATE USER [' + @username + '] FOR LOGIN [' + @username + ']';
            EXEC (@sqlString);
        END

        -- Gán vào Role tương ứng theo role trong bảng Account
        IF (@role = N'manager')
        BEGIN
            -- Kiểm tra role tồn tại trước khi gán
            IF EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Manager' AND type = 'R')
            BEGIN
                SET @sqlString = 'ALTER ROLE [MiniMart_Manager] ADD MEMBER [' + @username + ']';
                EXEC (@sqlString);
            END
        END
        ELSE IF (@role = N'saler')
        BEGIN
            -- Kiểm tra role tồn tại trước khi gán
            IF EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Saler' AND type = 'R')
            BEGIN
                SET @sqlString = 'ALTER ROLE [MiniMart_Saler] ADD MEMBER [' + @username + ']';
                EXEC (@sqlString);
            END
        END
        ELSE IF (@role = N'customer')
        BEGIN
            -- Kiểm tra role tồn tại trước khi gán
            IF EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Customer' AND type = 'R')
            BEGIN
                SET @sqlString = 'ALTER ROLE [MiniMart_Customer] ADD MEMBER [' + @username + ']';
                EXEC (@sqlString);
            END
        END
    END TRY
    BEGIN CATCH
        -- Ghi log lỗi nếu cần (có thể bỏ qua lỗi nếu Login/User đã tồn tại)
        PRINT 'Warning: Could not create SQL account for user: ' + @username;
        PRINT ERROR_MESSAGE();
    END CATCH
END;
GO

-- ========================================
-- 7. TẠO FUNCTION KIỂM TRA QUYỀN
-- ========================================

CREATE OR ALTER FUNCTION dbo.fn_CheckUserRole
(
    @username NVARCHAR(50),
    @password NVARCHAR(255)
)
RETURNS NVARCHAR(50) -- Trả về tên vai trò hoặc thông báo lỗi
AS
BEGIN
    DECLARE @role NVARCHAR(20);
    DECLARE @isValid BIT;
    
    -- Kiểm tra username và password có hợp lệ không trong bảng Account
    SELECT @isValid = CASE 
                      WHEN EXISTS (
                          SELECT 1 
                          FROM dbo.Account 
                          WHERE Username = @username 
                            AND Password = @password
                      ) 
                      THEN 1 
                      ELSE 0 
                      END;
    
    IF @isValid = 1
    BEGIN
        -- Nếu đăng nhập hợp lệ, lấy vai trò từ bảng Account
        SELECT @role = Role
        FROM dbo.Account 
        WHERE Username = @username;
        
        -- Nếu không tìm thấy role thì trả về thông báo
        IF @role IS NULL
        BEGIN
            SET @role = N'User này không có role nào';
        END
    END
    ELSE
    BEGIN
        -- Nếu đăng nhập không hợp lệ
        SET @role = N'Tên đăng nhập hoặc mật khẩu không đúng';
    END
    
    RETURN @role;
END
GO

-- ========================================
-- 8. PHÂN QUYỀN CHO BẢNG ProductPriceHistory
-- ========================================

-- Bảng này được tạo bởi trigger, cần phân quyền
IF OBJECT_ID('dbo.ProductPriceHistory', 'U') IS NOT NULL
BEGIN
    -- Kiểm tra role tồn tại trước khi phân quyền
    IF EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Manager' AND type = 'R')
        GRANT SELECT ON dbo.ProductPriceHistory TO MiniMart_Manager;
    
    IF EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Saler' AND type = 'R')
        GRANT SELECT ON dbo.ProductPriceHistory TO MiniMart_Saler;
END

-- ========================================
-- 9. PHÂN QUYỀN FUNCTION KIỂM TRA
-- ========================================

-- Cho phép tất cả các role sử dụng function kiểm tra quyền (kiểm tra role tồn tại)
IF EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Manager' AND type = 'R')
    GRANT EXECUTE ON dbo.fn_CheckUserRole TO MiniMart_Manager;

IF EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Saler' AND type = 'R')
    GRANT EXECUTE ON dbo.fn_CheckUserRole TO MiniMart_Saler;

IF EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Customer' AND type = 'R')
    GRANT EXECUTE ON dbo.fn_CheckUserRole TO MiniMart_Customer;

-- Phân quyền cho public để có thể sử dụng trong login
GRANT EXECUTE ON dbo.fn_CheckUserRole TO public;

-- ========================================
-- 10. TẠO SQL ACCOUNT CHO CÁC TÀI KHOẢN ĐÃ CÓ
-- ========================================

-- Tự động tạo SQL Login/User cho các tài khoản đã có trong bảng Account
DECLARE @username NVARCHAR(50), @password NVARCHAR(255), @role NVARCHAR(20);
DECLARE @sqlString NVARCHAR(MAX);

DECLARE account_cursor CURSOR FOR
SELECT Username, Password, Role FROM dbo.Account;

OPEN account_cursor;
FETCH NEXT FROM account_cursor INTO @username, @password, @role;

WHILE @@FETCH_STATUS = 0
BEGIN
    BEGIN TRY
        -- Tạo Login (kiểm tra không tồn tại)
        IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = @username)
        BEGIN
            SET @sqlString = 'CREATE LOGIN [' + @username + '] WITH PASSWORD = ''' + @password + 
                            ''', DEFAULT_DATABASE = [Minimart_SalesDB], CHECK_EXPIRATION = OFF, CHECK_POLICY = OFF';
            EXEC (@sqlString);
            PRINT 'Created Login: ' + @username;
        END

        -- Tạo User (kiểm tra không tồn tại)
        IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = @username)
        BEGIN
            SET @sqlString = 'CREATE USER [' + @username + '] FOR LOGIN [' + @username + ']';
            EXEC (@sqlString);
            PRINT 'Created User: ' + @username;
        END

        -- Gán vào Role tương ứng
        IF (@role = N'manager' AND EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Manager' AND type = 'R'))
        BEGIN
            SET @sqlString = 'ALTER ROLE [MiniMart_Manager] ADD MEMBER [' + @username + ']';
            EXEC (@sqlString);
            PRINT 'Added ' + @username + ' to MiniMart_Manager role';
        END
        ELSE IF (@role = N'saler' AND EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Saler' AND type = 'R'))
        BEGIN
            SET @sqlString = 'ALTER ROLE [MiniMart_Saler] ADD MEMBER [' + @username + ']';
            EXEC (@sqlString);
            PRINT 'Added ' + @username + ' to MiniMart_Saler role';
        END
        ELSE IF (@role = N'customer' AND EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Customer' AND type = 'R'))
        BEGIN
            SET @sqlString = 'ALTER ROLE [MiniMart_Customer] ADD MEMBER [' + @username + ']';
            EXEC (@sqlString);
            PRINT 'Added ' + @username + ' to MiniMart_Customer role';
        END
    END TRY
    BEGIN CATCH
        PRINT 'Warning: Could not create SQL account for existing user: ' + @username;
        PRINT ERROR_MESSAGE();
    END CATCH

    FETCH NEXT FROM account_cursor INTO @username, @password, @role;
END

CLOSE account_cursor;
DEALLOCATE account_cursor;

-- ========================================
-- 11. THÔNG BÁO HOÀN THÀNH
-- ========================================

PRINT N'✅ Đã thiết lập thành công hệ thống phân quyền!';
PRINT N'📋 Tóm tắt các role đã tạo (theo cấu trúc Database thực tế):';
PRINT N'   🟠 MiniMart_Manager - Quản lý toàn bộ (role: manager)'; 
PRINT N'   🟡 MiniMart_Saler - Nhân viên bán hàng (role: saler)';
PRINT N'   🟢 MiniMart_Customer - Khách hàng (role: customer)';
PRINT N'';
PRINT N'🔐 Login mặc định đã tạo:';
PRINT N'   Manager: ManagerLogin / Manager@123!';
PRINT N'   Saler: SalerLogin / Saler@123!';
PRINT N'   Customer: CustomerLogin / Customer@123!';
PRINT N'';
PRINT N'📊 Dữ liệu mẫu trong bảng Account:';
PRINT N'   admin (manager), saler001/saler002 (saler), customer001/customer002 (customer)';
PRINT N'';
PRINT N'⚡ Trigger tự động tạo tài khoản SQL khi thêm record vào bảng Account';
PRINT N'🔧 Function fn_CheckUserRole để kiểm tra quyền đăng nhập';
PRINT N'';
PRINT N'🎯 Cấu trúc bảng chính: Products, Customers, Sales, SaleDetails, Account, Transactions, Discounts';

GO