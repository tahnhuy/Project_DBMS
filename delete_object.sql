--========================================================
-- Target DB
--========================================================
USE Minimart_SalesDB;
GO

/* =======================================================
   1) DROP FUNCTIONS thừa
   - Bạn chỉ giữ: GetDailyRevenue, GetMonthlyRevenue,
                  GetDiscountedPrice, IsStockAvailable (scalar)
                  GetProductByName (table-valued)
   ======================================================= */

-- Scalar functions dư
DROP FUNCTION IF EXISTS dbo.CalculateDiscountPercentage;
DROP FUNCTION IF EXISTS dbo.CalculateLoyaltyPoints;
DROP FUNCTION IF EXISTS dbo.fn_diraqmobietcs;
DROP FUNCTION IF EXISTS dbo.FormatVietnamMoney;
DROP FUNCTION IF EXISTS dbo.GetExpenseByType;
DROP FUNCTION IF EXISTS dbo.IsValidVietnamesePhone;

-- Table-valued / inline table-valued functions dư
DROP FUNCTION IF EXISTS dbo.GetCustomerPurchaseHistory;
DROP FUNCTION IF EXISTS dbo.GetDashboardStats;
DROP FUNCTION IF EXISTS dbo.GetProductByID;
DROP FUNCTION IF EXISTS dbo.GetProductRevenueReport;
DROP FUNCTION IF EXISTS dbo.GetTopSellingProducts;
DROP FUNCTION IF EXISTS dbo.SearchCustomers;

-- (GIỮ) Không drop:
-- dbo.GetDailyRevenue, dbo.GetMonthlyRevenue, dbo.GetDiscountedPrice, dbo.IsStockAvailable
-- dbo.GetProductByName

/* =======================================================
   2) DROP STORED PROCEDURES thừa
   - Bạn chỉ giữ:
     Products   : GetAllProducts, AddProduct, UpdateProduct, DeleteProduct
     Customers  : GetAllCustomers, GetCustomerByID, GetCustomerByName, AddCustomer, UpdateCustomer, DeleteCustomer
     Sales      : CreateSale, AddSaleDetail, UpdateSale, GetSaleByID, GetSaleDetails
     Discounts  : GetActiveDiscounts, GetDiscountsByProduct, AddDiscount, UpdateDiscount, DeleteDiscount
     Accounts   : CheckLogin, ChangePassword
   ======================================================= */

-- Accounts & misc dư
DROP PROCEDURE IF EXISTS dbo.AddAccount;
DROP PROCEDURE IF EXISTS dbo.BackupAccounts;
DROP PROCEDURE IF EXISTS dbo.CheckAccountExists;
DROP PROCEDURE IF EXISTS dbo.CheckUserPermission;
DROP PROCEDURE IF EXISTS dbo.CountAccountsByRole;
DROP PROCEDURE IF EXISTS dbo.DeleteAccount;
DROP PROCEDURE IF EXISTS dbo.GetAccountActivity;
DROP PROCEDURE IF EXISTS dbo.GetAccountDetails;
DROP PROCEDURE IF EXISTS dbo.GetAccountInfo;
DROP PROCEDURE IF EXISTS dbo.GetAccountsByRole;
DROP PROCEDURE IF EXISTS dbo.GetAccountStatistics;
DROP PROCEDURE IF EXISTS dbo.GetAccountsWithDetails;
DROP PROCEDURE IF EXISTS dbo.GetAllAccounts;
DROP PROCEDURE IF EXISTS dbo.GetCustomerByUsername;
DROP PROCEDURE IF EXISTS dbo.GetEmployeeInfo;
DROP PROCEDURE IF EXISTS dbo.GetSalesByCustomerUsername;
DROP PROCEDURE IF EXISTS dbo.GetTransactionsByUsername;
DROP PROCEDURE IF EXISTS dbo.LinkAccountToCustomer;
DROP PROCEDURE IF EXISTS dbo.ResetPassword;
DROP PROCEDURE IF EXISTS dbo.SearchAccounts;
DROP PROCEDURE IF EXISTS dbo.UpdateAccount;
DROP PROCEDURE IF EXISTS dbo.UpdateCustomerByUsername;
DROP PROCEDURE IF EXISTS dbo.ValidatePassword;
DROP PROCEDURE IF EXISTS dbo.ValidateUsername;
DROP PROCEDURE IF EXISTS dbo.DeleteSale;

-- Hệ thống diagram (không cần cho project)
DROP PROCEDURE IF EXISTS dbo.sp_alterdiagram;
DROP PROCEDURE IF EXISTS dbo.sp_creatediagram;
DROP PROCEDURE IF EXISTS dbo.sp_dropdiagram;
DROP PROCEDURE IF EXISTS dbo.sp_helpdiagramdefinition;
DROP PROCEDURE IF EXISTS dbo.sp_helpdiagrams;
DROP PROCEDURE IF EXISTS dbo.sp_renamediagram;
DROP PROCEDURE IF EXISTS dbo.sp_upgraddiagrams;

-- (GIỮ) Không drop các proc bạn đã chọn:
-- Products   : GetAllProducts, AddProduct, UpdateProduct, DeleteProduct
-- Customers  : GetAllCustomers, GetCustomerByID, GetCustomerByName, AddCustomer, UpdateCustomer, DeleteCustomer
-- Sales      : CreateSale, AddSaleDetail, UpdateSale, GetSaleByID, GetSaleDetails
-- Discounts  : GetActiveDiscounts, GetDiscountsByProduct, AddDiscount, UpdateDiscount, DeleteDiscount
-- Accounts   : CheckLogin, ChangePassword

/* =======================================================
   3) DROP VIEWS thừa
   - Bạn chỉ giữ: LowStockProducts, SalesSummary
   ======================================================= */
DROP VIEW IF EXISTS dbo.AccountSummary;
DROP VIEW IF EXISTS dbo.ActiveDiscountsDetail;
DROP VIEW IF EXISTS dbo.ProductsWithDiscounts;
DROP VIEW IF EXISTS dbo.TransactionSummary;

-- (GIỮ) Không drop: dbo.LowStockProducts, dbo.SalesSummary
GO

IF OBJECT_ID('dbo.TR_Customers_ValidatePhone', 'TR') IS NOT NULL
    DROP TRIGGER dbo.TR_Customers_ValidatePhone;
GO

IF OBJECT_ID('dbo.TR_SaleDetails_CalculateLineTotal', 'TR') IS NOT NULL
    DROP TRIGGER dbo.TR_SaleDetails_CalculateLineTotal;
GO

IF OBJECT_ID('dbo.TR_SaleDetails_UpdateTotalAmount', 'TR') IS NOT NULL
    DROP TRIGGER dbo.TR_SaleDetails_UpdateTotalAmount;
GO

IF OBJECT_ID('dbo.trg_CreateSQLAccount', 'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_CreateSQLAccount;
GO