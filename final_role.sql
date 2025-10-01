USE Minimart_SalesDB;
GO

/*
  Role provisioning generated from current final_proc.sql and final_func.sql
  - Roles: MiniMart_Manager, MiniMart_Saler
  - Manager: Full execute on all stored procedures; SELECT on all TVFs; SELECT on core tables
  - Saler: Execute on sales and password procedures; SELECT on read TVFs; SELECT on needed tables
  - Views: same as new_role.sql
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
    IF OBJECT_ID('dbo.GetDailyRevenue','FN')    IS NOT NULL GRANT EXECUTE ON dbo.GetDailyRevenue    TO MiniMart_Manager;
    IF OBJECT_ID('dbo.GetMonthlyRevenue','FN')  IS NOT NULL GRANT EXECUTE ON dbo.GetMonthlyRevenue  TO MiniMart_Manager;
    IF OBJECT_ID('dbo.GetDiscountedPrice','FN') IS NOT NULL GRANT EXECUTE ON dbo.GetDiscountedPrice TO MiniMart_Manager;
    IF OBJECT_ID('dbo.IsStockAvailable','FN')   IS NOT NULL GRANT EXECUTE ON dbo.IsStockAvailable   TO MiniMart_Manager;

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

    /* Base Tables (Manager: SELECT, INSERT, UPDATE) */
    IF OBJECT_ID('dbo.Products','U')     IS NOT NULL BEGIN GRANT SELECT, INSERT, UPDATE ON dbo.Products     TO MiniMart_Manager; END
    IF OBJECT_ID('dbo.Customers','U')    IS NOT NULL BEGIN GRANT SELECT, INSERT, UPDATE ON dbo.Customers    TO MiniMart_Manager; END
    IF OBJECT_ID('dbo.Sales','U')        IS NOT NULL BEGIN GRANT SELECT, INSERT, UPDATE ON dbo.Sales        TO MiniMart_Manager; END
    IF OBJECT_ID('dbo.SaleDetails','U')  IS NOT NULL BEGIN GRANT SELECT, INSERT, UPDATE ON dbo.SaleDetails  TO MiniMart_Manager; END
    IF OBJECT_ID('dbo.Discounts','U')    IS NOT NULL BEGIN GRANT SELECT, INSERT, UPDATE ON dbo.Discounts    TO MiniMart_Manager; END
    IF OBJECT_ID('dbo.Account','U')      IS NOT NULL BEGIN GRANT SELECT, INSERT, UPDATE ON dbo.Account      TO MiniMart_Manager; END
    IF OBJECT_ID('dbo.Employees','U')    IS NOT NULL BEGIN GRANT SELECT, INSERT, UPDATE ON dbo.Employees    TO MiniMart_Manager; END

    /* Views (SELECT) - keep as in new_role.sql */
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

    /* TVFs (SELECT) */
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

    /* Base Tables (Saler) */
    -- Read access for product/customer/discount master data
    IF OBJECT_ID('dbo.Products','U')     IS NOT NULL BEGIN GRANT SELECT ON dbo.Products     TO MiniMart_Saler; END
    IF OBJECT_ID('dbo.Customers','U')    IS NOT NULL BEGIN GRANT SELECT ON dbo.Customers    TO MiniMart_Saler; END
    IF OBJECT_ID('dbo.Discounts','U')    IS NOT NULL BEGIN GRANT SELECT ON dbo.Discounts    TO MiniMart_Saler; END
    -- Sales DML: allow creating/updating sales and their details
    IF OBJECT_ID('dbo.Sales','U')        IS NOT NULL BEGIN GRANT SELECT, INSERT, UPDATE ON dbo.Sales        TO MiniMart_Saler; END
    IF OBJECT_ID('dbo.SaleDetails','U')  IS NOT NULL BEGIN GRANT SELECT, INSERT, UPDATE ON dbo.SaleDetails  TO MiniMart_Saler; END

    /* Views (SELECT) */
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


