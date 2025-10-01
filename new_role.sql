USE Minimart_SalesDB;
GO

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Manager' AND type = 'R')
    CREATE ROLE MiniMart_Manager;

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Saler' AND type = 'R')
    CREATE ROLE MiniMart_Saler;

BEGIN TRY
  BEGIN TRAN;

  /* -------- STORED PROCEDURES -------- */
  -- Products
  IF OBJECT_ID('dbo.GetAllProducts','P') IS NOT NULL GRANT EXECUTE ON dbo.GetAllProducts TO MiniMart_Manager;
  IF OBJECT_ID('dbo.AddProduct','P')    IS NOT NULL GRANT EXECUTE ON dbo.AddProduct    TO MiniMart_Manager;
  IF OBJECT_ID('dbo.UpdateProduct','P') IS NOT NULL GRANT EXECUTE ON dbo.UpdateProduct TO MiniMart_Manager;
  IF OBJECT_ID('dbo.DeleteProduct','P') IS NOT NULL GRANT EXECUTE ON dbo.DeleteProduct TO MiniMart_Manager;
  IF OBJECT_ID('dbo.GetProductByID','P') IS NOT NULL GRANT EXECUTE ON dbo.GetProductByID TO MiniMart_Manager;
  -- Customers
  IF OBJECT_ID('dbo.GetAllCustomers','P')   IS NOT NULL GRANT EXECUTE ON dbo.GetAllCustomers   TO MiniMart_Manager;
  IF OBJECT_ID('dbo.GetCustomerByID','P')   IS NOT NULL GRANT EXECUTE ON dbo.GetCustomerByID   TO MiniMart_Manager;
  IF OBJECT_ID('dbo.GetCustomerByName','P') IS NOT NULL GRANT EXECUTE ON dbo.GetCustomerByName TO MiniMart_Manager;
  IF OBJECT_ID('dbo.AddCustomer','P')       IS NOT NULL GRANT EXECUTE ON dbo.AddCustomer       TO MiniMart_Manager;
  IF OBJECT_ID('dbo.UpdateCustomer','P')    IS NOT NULL GRANT EXECUTE ON dbo.UpdateCustomer    TO MiniMart_Manager;
  IF OBJECT_ID('dbo.DeleteCustomer','P')    IS NOT NULL GRANT EXECUTE ON dbo.DeleteCustomer    TO MiniMart_Manager;

  -- Sales
  IF OBJECT_ID('dbo.CreateSale','P')     IS NOT NULL GRANT EXECUTE ON dbo.CreateSale     TO MiniMart_Manager;
  IF OBJECT_ID('dbo.AddSaleDetail','P')  IS NOT NULL GRANT EXECUTE ON dbo.AddSaleDetail  TO MiniMart_Manager;
  IF OBJECT_ID('dbo.UpdateSale','P')     IS NOT NULL GRANT EXECUTE ON dbo.UpdateSale     TO MiniMart_Manager;
  IF OBJECT_ID('dbo.GetSaleByID','P')    IS NOT NULL GRANT EXECUTE ON dbo.GetSaleByID    TO MiniMart_Manager;
  IF OBJECT_ID('dbo.GetSaleDetails','P') IS NOT NULL GRANT EXECUTE ON dbo.GetSaleDetails TO MiniMart_Manager;

  -- Discounts
  IF OBJECT_ID('dbo.GetActiveDiscounts','P')      IS NOT NULL GRANT EXECUTE ON dbo.GetActiveDiscounts      TO MiniMart_Manager;
  IF OBJECT_ID('dbo.GetDiscountsByProduct','P')   IS NOT NULL GRANT EXECUTE ON dbo.GetDiscountsByProduct   TO MiniMart_Manager;
  IF OBJECT_ID('dbo.AddDiscount','P')             IS NOT NULL GRANT EXECUTE ON dbo.AddDiscount             TO MiniMart_Manager;
  IF OBJECT_ID('dbo.UpdateDiscount','P')          IS NOT NULL GRANT EXECUTE ON dbo.UpdateDiscount          TO MiniMart_Manager;
  IF OBJECT_ID('dbo.DeleteDiscount','P')          IS NOT NULL GRANT EXECUTE ON dbo.DeleteDiscount          TO MiniMart_Manager;

  -- Accounts
  IF OBJECT_ID('dbo.CheckLogin','P')      IS NOT NULL GRANT EXECUTE ON dbo.CheckLogin      TO MiniMart_Manager;
  IF OBJECT_ID('dbo.ChangePassword','P')  IS NOT NULL GRANT EXECUTE ON dbo.ChangePassword  TO MiniMart_Manager;

  /* -------- SCALAR FUNCTIONS (EXECUTE) -------- */
  IF OBJECT_ID('dbo.GetDailyRevenue','FN')    IS NOT NULL GRANT EXECUTE ON dbo.GetDailyRevenue    TO MiniMart_Manager;
  IF OBJECT_ID('dbo.GetMonthlyRevenue','FN')  IS NOT NULL GRANT EXECUTE ON dbo.GetMonthlyRevenue  TO MiniMart_Manager;
  IF OBJECT_ID('dbo.GetDiscountedPrice','FN') IS NOT NULL GRANT EXECUTE ON dbo.GetDiscountedPrice TO MiniMart_Manager;
  IF OBJECT_ID('dbo.IsStockAvailable','FN')   IS NOT NULL GRANT EXECUTE ON dbo.IsStockAvailable   TO MiniMart_Manager;

  /* -------- TABLE-VALUED FUNCTIONS (SELECT) -------- */
  IF OBJECT_ID('dbo.GetProductByName','IF') IS NOT NULL
     OR OBJECT_ID('dbo.GetProductByName','TF') IS NOT NULL
     GRANT SELECT ON dbo.GetProductByName TO MiniMart_Manager;

  /* -------- VIEWS (SELECT) -------- */
  IF OBJECT_ID('dbo.LowStockProducts','V') IS NOT NULL GRANT SELECT ON dbo.LowStockProducts TO MiniMart_Manager;
  IF OBJECT_ID('dbo.SalesSummary','V')     IS NOT NULL GRANT SELECT ON dbo.SalesSummary     TO MiniMart_Manager;

  COMMIT TRAN;
  PRINT N'✅ Đã cấp quyền cho MiniMart_Manager.';
END TRY
BEGIN CATCH
  IF XACT_STATE() <> 0 ROLLBACK TRAN;
  PRINT N'❌ Lỗi khi cấp quyền cho MiniMart_Manager.';
  PRINT ERROR_MESSAGE();
END CATCH
GO


USE Minimart_SalesDB;
GO
BEGIN TRY
  BEGIN TRAN;

  /* -------- STORED PROCEDURES -------- */
  -- Products (không có DeleteProduct)
  IF OBJECT_ID('dbo.GetAllProducts','P') IS NOT NULL GRANT EXECUTE ON dbo.GetAllProducts TO MiniMart_Saler;
  IF OBJECT_ID('dbo.AddProduct','P')    IS NOT NULL GRANT EXECUTE ON dbo.AddProduct    TO MiniMart_Saler;
  IF OBJECT_ID('dbo.UpdateProduct','P') IS NOT NULL GRANT EXECUTE ON dbo.UpdateProduct TO MiniMart_Saler;
  IF OBJECT_ID('dbo.GetProductByID','P') IS NOT NULL GRANT EXECUTE ON dbo.GetProductByID TO MiniMart_Manager;

  -- Customers (không có DeleteCustomer)
  IF OBJECT_ID('dbo.GetAllCustomers','P')   IS NOT NULL GRANT EXECUTE ON dbo.GetAllCustomers   TO MiniMart_Saler;
  IF OBJECT_ID('dbo.GetCustomerByID','P')   IS NOT NULL GRANT EXECUTE ON dbo.GetCustomerByID   TO MiniMart_Saler;
  IF OBJECT_ID('dbo.GetCustomerByName','P') IS NOT NULL GRANT EXECUTE ON dbo.GetCustomerByName TO MiniMart_Saler;
  IF OBJECT_ID('dbo.AddCustomer','P')       IS NOT NULL GRANT EXECUTE ON dbo.AddCustomer       TO MiniMart_Saler;
  IF OBJECT_ID('dbo.UpdateCustomer','P')    IS NOT NULL GRANT EXECUTE ON dbo.UpdateCustomer    TO MiniMart_Saler;

  -- Sales (đầy đủ)
  IF OBJECT_ID('dbo.CreateSale','P')     IS NOT NULL GRANT EXECUTE ON dbo.CreateSale     TO MiniMart_Saler;
  IF OBJECT_ID('dbo.AddSaleDetail','P')  IS NOT NULL GRANT EXECUTE ON dbo.AddSaleDetail  TO MiniMart_Saler;
  IF OBJECT_ID('dbo.UpdateSale','P')     IS NOT NULL GRANT EXECUTE ON dbo.UpdateSale     TO MiniMart_Saler;
  IF OBJECT_ID('dbo.GetSaleByID','P')    IS NOT NULL GRANT EXECUTE ON dbo.GetSaleByID    TO MiniMart_Saler;
  IF OBJECT_ID('dbo.GetSaleDetails','P') IS NOT NULL GRANT EXECUTE ON dbo.GetSaleDetails TO MiniMart_Saler;

  -- Discounts (chỉ truy vấn, không Add/Update/Delete)
  IF OBJECT_ID('dbo.GetActiveDiscounts','P')    IS NOT NULL GRANT EXECUTE ON dbo.GetActiveDiscounts    TO MiniMart_Saler;
  IF OBJECT_ID('dbo.GetDiscountsByProduct','P') IS NOT NULL GRANT EXECUTE ON dbo.GetDiscountsByProduct TO MiniMart_Saler;

  -- Accounts
  IF OBJECT_ID('dbo.CheckLogin','P')     IS NOT NULL GRANT EXECUTE ON dbo.CheckLogin     TO MiniMart_Saler;
  IF OBJECT_ID('dbo.ChangePassword','P') IS NOT NULL GRANT EXECUTE ON dbo.ChangePassword TO MiniMart_Saler;

  /* -------- SCALAR FUNCTIONS (EXECUTE) -------- */
  IF OBJECT_ID('dbo.GetDailyRevenue','FN')    IS NOT NULL GRANT EXECUTE ON dbo.GetDailyRevenue    TO MiniMart_Saler;
  IF OBJECT_ID('dbo.GetMonthlyRevenue','FN')  IS NOT NULL GRANT EXECUTE ON dbo.GetMonthlyRevenue  TO MiniMart_Saler;
  IF OBJECT_ID('dbo.GetDiscountedPrice','FN') IS NOT NULL GRANT EXECUTE ON dbo.GetDiscountedPrice TO MiniMart_Saler;
  IF OBJECT_ID('dbo.IsStockAvailable','FN')   IS NOT NULL GRANT EXECUTE ON dbo.IsStockAvailable   TO MiniMart_Saler;

  /* -------- TABLE-VALUED FUNCTIONS (SELECT) -------- */
  IF OBJECT_ID('dbo.GetProductByName','IF') IS NOT NULL
     OR OBJECT_ID('dbo.GetProductByName','TF') IS NOT NULL
     GRANT SELECT ON dbo.GetProductByName TO MiniMart_Saler;

  /* -------- VIEWS (SELECT) -------- */
  IF OBJECT_ID('dbo.LowStockProducts','V') IS NOT NULL GRANT SELECT ON dbo.LowStockProducts TO MiniMart_Saler;
  IF OBJECT_ID('dbo.SalesSummary','V')     IS NOT NULL GRANT SELECT ON dbo.SalesSummary     TO MiniMart_Saler;

  COMMIT TRAN;
  PRINT N'✅ Đã cấp quyền cho MiniMart_Saler.';
END TRY
BEGIN CATCH
  IF XACT_STATE() <> 0 ROLLBACK TRAN;
  PRINT N'❌ Lỗi khi cấp quyền cho MiniMart_Saler.';
  PRINT ERROR_MESSAGE();
END CATCH
GO
