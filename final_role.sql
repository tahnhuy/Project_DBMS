USE Minimart_SalesDB;
GO

/*
  Role provisioning UPDATED:
  - Added SP: dbo.RestoreProduct
  - Added SFs: dbo.ValidatePassword, dbo.fnGetRevenueByPeriod, dbo.GetSalesGrowthRate_Monthly
  - Added Advanced TVFs: fnReport_TopSellingProducts, fnReport_CustomerRanking, fnReport_DailyProductSalesTrend, 
                         fnReport_MonthlySalesAggregate, fnReport_ProductSalesComparison
*/

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Manager' AND type = 'R')
    CREATE ROLE MiniMart_Manager;

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Saler' AND type = 'R')
    CREATE ROLE MiniMart_Saler;

BEGIN TRY
    BEGIN TRAN;

    /* ========================= MANAGER ========================= */
    /* Stored Procedures (EXECUTE) */
    -- Products
    IF OBJECT_ID('dbo.AddProduct','P')       IS NOT NULL GRANT EXECUTE ON dbo.AddProduct       TO MiniMart_Manager;
    IF OBJECT_ID('dbo.UpdateProduct','P')    IS NOT NULL GRANT EXECUTE ON dbo.UpdateProduct    TO MiniMart_Manager;
    IF OBJECT_ID('dbo.DeleteProduct','P')    IS NOT NULL GRANT EXECUTE ON dbo.DeleteProduct    TO MiniMart_Manager;
    IF OBJECT_ID('dbo.RestoreProduct','P')   IS NOT NULL GRANT EXECUTE ON dbo.RestoreProduct   TO MiniMart_Manager;   -- SP MỚI
    -- Customers
    IF OBJECT_ID('dbo.AddCustomer','P')      IS NOT NULL GRANT EXECUTE ON dbo.AddCustomer      TO MiniMart_Manager;
    IF OBJECT_ID('dbo.UpdateCustomer','P')   IS NOT NULL GRANT EXECUTE ON dbo.UpdateCustomer   TO MiniMart_Manager;
    IF OBJECT_ID('dbo.DeleteCustomer','P')   IS NOT NULL GRANT EXECUTE ON dbo.DeleteCustomer   TO MiniMart_Manager;
    -- Sales
    IF OBJECT_ID('dbo.CreateSale','P')       IS NOT NULL GRANT EXECUTE ON dbo.CreateSale       TO MiniMart_Manager;
    IF OBJECT_ID('dbo.AddSaleDetail','P')    IS NOT NULL GRANT EXECUTE ON dbo.AddSaleDetail    TO MiniMart_Manager;
    IF OBJECT_ID('dbo.UpdateSale','P')       IS NOT NULL GRANT EXECUTE ON dbo.UpdateSale       TO MiniMart_Manager;
    -- Discounts
    IF OBJECT_ID('dbo.AddDiscount','P')      IS NOT NULL GRANT EXECUTE ON dbo.AddDiscount      TO MiniMart_Manager;
    IF OBJECT_ID('dbo.UpdateDiscount','P')   IS NOT NULL GRANT EXECUTE ON dbo.UpdateDiscount   TO MiniMart_Manager;
    IF OBJECT_ID('dbo.DeleteDiscount','P')   IS NOT NULL GRANT EXECUTE ON dbo.DeleteDiscount   TO MiniMart_Manager;
    -- Accounts
    IF OBJECT_ID('dbo.AddAccount','P')       IS NOT NULL GRANT EXECUTE ON dbo.AddAccount       TO MiniMart_Manager;
    IF OBJECT_ID('dbo.UpdateAccount','P')    IS NOT NULL GRANT EXECUTE ON dbo.UpdateAccount    TO MiniMart_Manager;
    IF OBJECT_ID('dbo.DeleteAccount','P')    IS NOT NULL GRANT EXECUTE ON dbo.DeleteAccount    TO MiniMart_Manager;
    IF OBJECT_ID('dbo.ChangePassword','P')   IS NOT NULL GRANT EXECUTE ON dbo.ChangePassword   TO MiniMart_Manager;
    IF OBJECT_ID('dbo.CreateSQLAccount','P') IS NOT NULL GRANT EXECUTE ON dbo.CreateSQLAccount TO MiniMart_Manager;
    IF OBJECT_ID('dbo.DeleteSQLAccount','P') IS NOT NULL GRANT EXECUTE ON dbo.DeleteSQLAccount TO MiniMart_Manager;

    /* Scalar Functions (EXECUTE) */
    IF OBJECT_ID('dbo.GetDailyRevenue','FN')         IS NOT NULL GRANT EXECUTE ON dbo.GetDailyRevenue         TO MiniMart_Manager;
    IF OBJECT_ID('dbo.GetMonthlyRevenue','FN')       IS NOT NULL GRANT EXECUTE ON dbo.GetMonthlyRevenue       TO MiniMart_Manager;
    IF OBJECT_ID('dbo.GetDiscountedPrice','FN')      IS NOT NULL GRANT EXECUTE ON dbo.GetDiscountedPrice      TO MiniMart_Manager;
    IF OBJECT_ID('dbo.IsStockAvailable','FN')        IS NOT NULL GRANT EXECUTE ON dbo.IsStockAvailable        TO MiniMart_Manager;
    IF OBJECT_ID('dbo.ValidatePassword','FN')        IS NOT NULL GRANT EXECUTE ON dbo.ValidatePassword        TO MiniMart_Manager;            -- SF MỚI/CẬP NHẬT
    IF OBJECT_ID('dbo.fnGetRevenueByPeriod','FN')    IS NOT NULL GRANT EXECUTE ON dbo.fnGetRevenueByPeriod    TO MiniMart_Manager;            -- SF MỚI
    IF OBJECT_ID('dbo.GetSalesGrowthRate_Monthly','FN') IS NOT NULL GRANT EXECUTE ON dbo.GetSalesGrowthRate_Monthly TO MiniMart_Manager; -- SF MỚI

    /* TVFs (SELECT) */
    -- Products
    IF OBJECT_ID('dbo.fnProducts_All','IF')           IS NOT NULL OR OBJECT_ID('dbo.fnProducts_All','TF')           IS NOT NULL GRANT SELECT ON dbo.fnProducts_All           TO MiniMart_Manager;
    IF OBJECT_ID('dbo.fnProducts_ByID','IF')          IS NOT NULL OR OBJECT_ID('dbo.fnProducts_ByID','TF')          IS NOT NULL GRANT SELECT ON dbo.fnProducts_ByID          TO MiniMart_Manager;
    IF OBJECT_ID('dbo.GetProductByName','IF')         IS NOT NULL OR OBJECT_ID('dbo.GetProductByName','TF')         IS NOT NULL GRANT SELECT ON dbo.GetProductByName         TO MiniMart_Manager;
    -- Customers
    IF OBJECT_ID('dbo.fnCustomers_All','IF')          IS NOT NULL OR OBJECT_ID('dbo.fnCustomers_All','TF')          IS NOT NULL GRANT SELECT ON dbo.fnCustomers_All          TO MiniMart_Manager;
    IF OBJECT_ID('dbo.fnCustomers_ByID','IF')         IS NOT NULL OR OBJECT_ID('dbo.fnCustomers_ByID','TF')         IS NOT NULL GRANT SELECT ON dbo.fnCustomers_ByID         TO MiniMart_Manager;
    IF OBJECT_ID('dbo.fnCustomers_ByName','IF')       IS NOT NULL OR OBJECT_ID('dbo.fnCustomers_ByName','TF')       IS NOT NULL GRANT SELECT ON dbo.fnCustomers_ByName       TO MiniMart_Manager;
    -- Sales
    IF OBJECT_ID('dbo.fnSales_ByID','IF')             IS NOT NULL OR OBJECT_ID('dbo.fnSales_ByID','TF')             IS NOT NULL GRANT SELECT ON dbo.fnSales_ByID             TO MiniMart_Manager;
    IF OBJECT_ID('dbo.fnSaleDetails_BySaleID','IF')   IS NOT NULL OR OBJECT_ID('dbo.fnSaleDetails_BySaleID','TF')   IS NOT NULL GRANT SELECT ON dbo.fnSaleDetails_BySaleID   TO MiniMart_Manager;
    -- Discounts
    IF OBJECT_ID('dbo.fnDiscounts_Active','IF')       IS NOT NULL OR OBJECT_ID('dbo.fnDiscounts_Active','TF')       IS NOT NULL GRANT SELECT ON dbo.fnDiscounts_Active       TO MiniMart_Manager;
    IF OBJECT_ID('dbo.fnDiscounts_ByProduct','IF')    IS NOT NULL OR OBJECT_ID('dbo.fnDiscounts_ByProduct','TF')    IS NOT NULL GRANT SELECT ON dbo.fnDiscounts_ByProduct    TO MiniMart_Manager;
    -- Accounts
    IF OBJECT_ID('dbo.fnAccounts_All','IF')           IS NOT NULL OR OBJECT_ID('dbo.fnAccounts_All','TF')           IS NOT NULL GRANT SELECT ON dbo.fnAccounts_All           TO MiniMart_Manager;
    IF OBJECT_ID('dbo.fnAccounts_ByUsername','IF')    IS NOT NULL OR OBJECT_ID('dbo.fnAccounts_ByUsername','TF')    IS NOT NULL GRANT SELECT ON dbo.fnAccounts_ByUsername    TO MiniMart_Manager;
    IF OBJECT_ID('dbo.fnAccounts_ByRole','IF')        IS NOT NULL OR OBJECT_ID('dbo.fnAccounts_ByRole','TF')        IS NOT NULL GRANT SELECT ON dbo.fnAccounts_ByRole        TO MiniMart_Manager;
    IF OBJECT_ID('dbo.fnAuth_User','IF')              IS NOT NULL OR OBJECT_ID('dbo.fnAuth_User','TF')              IS NOT NULL GRANT SELECT ON dbo.fnAuth_User              TO MiniMart_Manager;
    
    -- TVFs MỚI (Báo cáo/Nâng cao)
    IF OBJECT_ID('dbo.fnReport_TopSellingProducts','IF')    IS NOT NULL OR OBJECT_ID('dbo.fnReport_TopSellingProducts','TF')    IS NOT NULL GRANT SELECT ON dbo.fnReport_TopSellingProducts TO MiniMart_Manager; 
    IF OBJECT_ID('dbo.fnReport_CustomerRanking','IF')       IS NOT NULL OR OBJECT_ID('dbo.fnReport_CustomerRanking','TF')       IS NOT NULL GRANT SELECT ON dbo.fnReport_CustomerRanking    TO MiniMart_Manager;
    IF OBJECT_ID('dbo.fnReport_DailyProductSalesTrend','IF') IS NOT NULL OR OBJECT_ID('dbo.fnReport_DailyProductSalesTrend','TF') IS NOT NULL GRANT SELECT ON dbo.fnReport_DailyProductSalesTrend TO MiniMart_Manager;
    IF OBJECT_ID('dbo.fnReport_MonthlySalesAggregate','IF') IS NOT NULL OR OBJECT_ID('dbo.fnReport_MonthlySalesAggregate','TF') IS NOT NULL GRANT SELECT ON dbo.fnReport_MonthlySalesAggregate TO MiniMart_Manager;
    IF OBJECT_ID('dbo.fnReport_ProductSalesComparison','IF') IS NOT NULL OR OBJECT_ID('dbo.fnReport_ProductSalesComparison','TF') IS NOT NULL GRANT SELECT ON dbo.fnReport_ProductSalesComparison TO MiniMart_Manager;

    /* Base Tables (Manager: SELECT, INSERT, UPDATE) */
    IF OBJECT_ID('dbo.Products','U')     IS NOT NULL BEGIN GRANT SELECT, INSERT, UPDATE ON dbo.Products     TO MiniMart_Manager; END
    IF OBJECT_ID('dbo.Customers','U')    IS NOT NULL BEGIN GRANT SELECT, INSERT, UPDATE ON dbo.Customers    TO MiniMart_Manager; END
    IF OBJECT_ID('dbo.Sales','U')        IS NOT NULL BEGIN GRANT SELECT, INSERT, UPDATE ON dbo.Sales        TO MiniMart_Manager; END
    IF OBJECT_ID('dbo.SaleDetails','U')  IS NOT NULL BEGIN GRANT SELECT, INSERT, UPDATE ON dbo.SaleDetails  TO MiniMart_Manager; END
    IF OBJECT_ID('dbo.Discounts','U')    IS NOT NULL BEGIN GRANT SELECT, INSERT, UPDATE ON dbo.Discounts    TO MiniMart_Manager; END
    IF OBJECT_ID('dbo.Account','U')      IS NOT NULL BEGIN GRANT SELECT, INSERT, UPDATE ON dbo.Account      TO MiniMart_Manager; END
    IF OBJECT_ID('dbo.Employees','U')    IS NOT NULL BEGIN GRANT SELECT, INSERT, UPDATE ON dbo.Employees    TO MiniMart_Manager; END

    /* Views (SELECT) - Giữ nguyên */
    IF OBJECT_ID('dbo.LowStockProducts','V') IS NOT NULL GRANT SELECT ON dbo.LowStockProducts TO MiniMart_Manager;
    IF OBJECT_ID('dbo.SalesSummary','V')     IS NOT NULL GRANT SELECT ON dbo.SalesSummary     TO MiniMart_Manager;

    /* ========================= SALER ========================= */
    /* Stored Procedures (EXECUTE) */
    IF OBJECT_ID('dbo.CreateSale','P')     IS NOT NULL GRANT EXECUTE ON dbo.CreateSale     TO MiniMart_Saler;
    IF OBJECT_ID('dbo.AddSaleDetail','P')  IS NOT NULL GRANT EXECUTE ON dbo.AddSaleDetail  TO MiniMart_Saler;
    IF OBJECT_ID('dbo.UpdateSale','P')     IS NOT NULL GRANT EXECUTE ON dbo.UpdateSale     TO MiniMart_Saler;
    IF OBJECT_ID('dbo.ChangePassword','P') IS NOT NULL GRANT EXECUTE ON dbo.ChangePassword TO MiniMart_Saler;

    /* Scalar Functions (EXECUTE) */
    IF OBJECT_ID('dbo.GetDailyRevenue','FN')    IS NOT NULL GRANT EXECUTE ON dbo.GetDailyRevenue    TO MiniMart_Saler;
    IF OBJECT_ID('dbo.GetMonthlyRevenue','FN')  IS NOT NULL GRANT EXECUTE ON dbo.GetMonthlyRevenue  TO MiniMart_Saler;
    IF OBJECT_ID('dbo.GetDiscountedPrice','FN') IS NOT NULL GRANT EXECUTE ON dbo.GetDiscountedPrice TO MiniMart_Saler;
    IF OBJECT_ID('dbo.IsStockAvailable','FN')   IS NOT NULL GRANT EXECUTE ON dbo.IsStockAvailable   TO MiniMart_Saler;
    IF OBJECT_ID('dbo.ValidatePassword','FN')   IS NOT NULL GRANT EXECUTE ON dbo.ValidatePassword   TO MiniMart_Saler;          -- SF MỚI/CẬP NHẬT
    IF OBJECT_ID('dbo.fnGetRevenueByPeriod','FN')    IS NOT NULL GRANT EXECUTE ON dbo.fnGetRevenueByPeriod    TO MiniMart_Saler; -- SF MỚI
    IF OBJECT_ID('dbo.GetSalesGrowthRate_Monthly','FN') IS NOT NULL GRANT EXECUTE ON dbo.GetSalesGrowthRate_Monthly TO MiniMart_Saler; -- SF MỚI

    /* TVFs (SELECT) - Giữ nguyên các hàm cơ bản */
    -- Products
    IF OBJECT_ID('dbo.fnProducts_All','IF')           IS NOT NULL OR OBJECT_ID('dbo.fnProducts_All','TF')           IS NOT NULL GRANT SELECT ON dbo.fnProducts_All           TO MiniMart_Saler;
    IF OBJECT_ID('dbo.fnProducts_ByID','IF')          IS NOT NULL OR OBJECT_ID('dbo.fnProducts_ByID','TF')          IS NOT NULL GRANT SELECT ON dbo.fnProducts_ByID          TO MiniMart_Saler;
    IF OBJECT_ID('dbo.GetProductByName','IF')         IS NOT NULL OR OBJECT_ID('dbo.GetProductByName','TF')         IS NOT NULL GRANT SELECT ON dbo.GetProductByName         TO MiniMart_Saler;
    -- Customers
    IF OBJECT_ID('dbo.fnCustomers_All','IF')          IS NOT NULL OR OBJECT_ID('dbo.fnCustomers_All','TF')          IS NOT NULL GRANT SELECT ON dbo.fnCustomers_All          TO MiniMart_Saler;
    IF OBJECT_ID('dbo.fnCustomers_ByID','IF')         IS NOT NULL OR OBJECT_ID('dbo.fnCustomers_ByID','TF')         IS NOT NULL GRANT SELECT ON dbo.fnCustomers_ByID         TO MiniMart_Saler;
    IF OBJECT_ID('dbo.fnCustomers_ByName','IF')       IS NOT NULL OR OBJECT_ID('dbo.fnCustomers_ByName','TF')       IS NOT NULL GRANT SELECT ON dbo.fnCustomers_ByName       TO MiniMart_Saler;
    -- Sales
    IF OBJECT_ID('dbo.fnSales_ByID','IF')             IS NOT NULL OR OBJECT_ID('dbo.fnSales_ByID','TF')             IS NOT NULL GRANT SELECT ON dbo.fnSales_ByID             TO MiniMart_Saler;
    IF OBJECT_ID('dbo.fnSaleDetails_BySaleID','IF')   IS NOT NULL OR OBJECT_ID('dbo.fnSaleDetails_BySaleID','TF')   IS NOT NULL GRANT SELECT ON dbo.fnSaleDetails_BySaleID   TO MiniMart_Saler;
    -- Discounts
    IF OBJECT_ID('dbo.fnDiscounts_Active','IF')       IS NOT NULL OR OBJECT_ID('dbo.fnDiscounts_Active','TF')       IS NOT NULL GRANT SELECT ON dbo.fnDiscounts_Active       TO MiniMart_Saler;
    IF OBJECT_ID('dbo.fnDiscounts_ByProduct','IF')    IS NOT NULL OR OBJECT_ID('dbo.fnDiscounts_ByProduct','TF')    IS NOT NULL GRANT SELECT ON dbo.fnDiscounts_ByProduct    TO MiniMart_Saler;
    -- Auth
    IF OBJECT_ID('dbo.fnAuth_User','IF')              IS NOT NULL OR OBJECT_ID('dbo.fnAuth_User','TF')              IS NOT NULL GRANT SELECT ON dbo.fnAuth_User              TO MiniMart_Saler;

    /* Base Tables (Saler) - Giữ nguyên */
    IF OBJECT_ID('dbo.Products','U')     IS NOT NULL BEGIN GRANT SELECT ON dbo.Products     TO MiniMart_Saler; END
    IF OBJECT_ID('dbo.Customers','U')    IS NOT NULL BEGIN GRANT SELECT ON dbo.Customers    TO MiniMart_Saler; END
    IF OBJECT_ID('dbo.Discounts','U')    IS NOT NULL BEGIN GRANT SELECT ON dbo.Discounts    TO MiniMart_Saler; END
    IF OBJECT_ID('dbo.Sales','U')        IS NOT NULL BEGIN GRANT SELECT, INSERT, UPDATE ON dbo.Sales        TO MiniMart_Saler; END
    IF OBJECT_ID('dbo.SaleDetails','U')  IS NOT NULL BEGIN GRANT SELECT, INSERT, UPDATE ON dbo.SaleDetails  TO MiniMart_Saler; END

    /* Views (SELECT) - Giữ nguyên */
    IF OBJECT_ID('dbo.LowStockProducts','V') IS NOT NULL GRANT SELECT ON dbo.LowStockProducts TO MiniMart_Saler;
    IF OBJECT_ID('dbo.SalesSummary','V')     IS NOT NULL GRANT SELECT ON dbo.SalesSummary     TO MiniMart_Saler;

    COMMIT TRAN;
    PRINT N'✅ Đã phân quyền (final_role.sql) cho MiniMart_Manager và MiniMart_Saler.';
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRAN;
    PRINT N'❌ Lỗi khi phân quyền (final_role.sql).';
    PRINT ERROR_MESSAGE();
END CATCH
GO

-- KHÔNG CẦN THAY ĐỔI CÁC STORED PROCEDURE CREATE/DELETE SQL ACCOUNT DƯỚI ĐÂY
-- VÌ CHÚNG VẪN CÓ TRONG PHẦN CUỐI CỦA FILE GỐC.

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