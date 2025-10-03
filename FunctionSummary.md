# Function and Procedure Summary - Active Usage Only

This document maps all actively used database objects that are actually called from C# repositories and forms. Read/list/detail queries now use TVFs. Create/update/delete actions remain stored procedures.

**Project Status**: Reads via TVFs; mutations via stored procedures. Full Account management for manager/saler with automatic Employee creation and SQL Login/User provisioning.

---

## Manager Role (MiniMart_Manager)

### Stored Procedures (mutations)

#### Procedures WITH transactions
- Products:
  - DeleteProduct(@ProductID)
    - Repository: `ProductRepository.DeleteProduct(int productId)`
    - Forms: `ProductForm`
- Sales:
  - CreateSale(@CustomerID, @TotalAmount, @PaymentMethod)
    - Repository: `SaleRepository.CreateSaleWithDetails(...)`
    - Forms: `SalerInvoiceForm`
  - AddSaleDetail(@SaleID, @ProductID, @Quantity, @SalePrice)
    - Repository: `SaleRepository.CreateSaleWithDetails()` (internal)
    - Forms: `SalerInvoiceForm`
  - UpdateSale(@SaleID, @CustomerID, @TotalAmount, @PaymentMethod)
    - Repository: `SaleRepository.CreateSaleWithDetails()` và `UpdateSale(...)`
    - Forms: `SalerInvoiceForm`
- Accounts:
  - AddAccount(@Username, @Password, @Role, @CustomerID, @EmployeeID)
    - Repository: `AccountRepository.AddAccount(...)`
    - Forms: `AccountCreateForm`

#### Procedures WITHOUT transactions
- Products:
  - AddProduct(@ProductName, @Price, @StockQuantity, @Unit)
  - UpdateProduct(@ProductID, @ProductName, @Price, @StockQuantity, @Unit)
- Customers:
  - AddCustomer(@CustomerName, @Phone, @Address, @LoyaltyPoints)
  - UpdateCustomer(@CustomerID, @CustomerName, @Phone, @Address, @LoyaltyPoints)
  - DeleteCustomer(@CustomerID)
- Discounts:
  - AddDiscount(@ProductID, @DiscountType, @DiscountValue, @StartDate, @EndDate, @IsActive, @CreatedBy)
  - UpdateDiscount(@DiscountID, @DiscountType, @DiscountValue, @StartDate, @EndDate, @IsActive)
  - DeleteDiscount(@DiscountID)
- Accounts:
  - UpdateAccount(@Username, @Password, @Role, @CustomerID, @EmployeeID)
  - DeleteAccount(@Username)
  - ChangePassword(@Username, @OldPassword, @NewPassword)
  - CreateSQLAccount(@Username, @Password, @Role)
  - DeleteSQLAccount(@Username)

### Table-Valued Functions (reads)

#### Products (read)
1. fnProducts_All()
   - Repository: `ProductRepository.GetAllProducts()`
   - Forms: `ProductForm`, `SalerProductForm`, `AdminDiscountForm`, `AdminDiscountEditForm`, `SalerInvoiceForm`
2. fnProducts_ByID(@ProductID)
   - Repository: `ProductRepository.GetProductById(int productId)`
   - Forms: `ProductForm`, `ProductEditForm`
3. GetProductByName(@ProductName)
   - Repository: `ProductRepository.GetProductByName(string productName)`
   - Forms: `ProductForm`, `SalerProductForm`

#### Customers (read)
4. fnCustomers_All()
   - Repository: `CustomerRepository.GetAllCustomers()`
   - Forms: `CustomerManageForm`, `SalerInvoiceForm`, `SalerCustomerViewForm`
5. fnCustomers_ByName(@CustomerName)
   - Repository: `CustomerRepository.GetCustomerByName(string customerName)`
   - Forms: `CustomerManageForm`, `SalerCustomerViewForm`
6. fnCustomers_ByID(@CustomerID)
   - Repository: `CustomerRepository.GetCustomerById(int customerId)`
   - Forms: `CustomerManageForm`, `CustomerManageEditForm`

#### Sales (read)
7. fnSales_ByID(@SaleID)
   - Repository: `SaleRepository.GetSaleById(int saleId)`
   - Forms: `SalerInvoiceHistoryForm`
8. fnSaleDetails_BySaleID(@SaleID)
   - Repository: `SaleRepository.GetSaleDetails(int saleId)`
   - Forms: `SalerInvoiceHistoryForm`

#### Discounts (read)
9. fnDiscounts_Active()
   - Repository: `DiscountRepository.GetActiveDiscounts()`
   - Forms: `AdminDiscountForm`
10. fnDiscounts_ByProduct(@ProductID)
    - Repository: `DiscountRepository.GetDiscountsByProduct(int productId)`
    - Forms: `AdminDiscountEditForm`

#### Accounts (read)
11. fnAccounts_All()
    - Repository: `AccountRepository.GetAllAccounts()`
    - Forms: `AccountForm`
12. fnAccounts_ByUsername(@Username)
    - Repository: `AccountRepository.GetAccountByUsername(string username)`
    - Forms: `AccountForm`, `AccountManageEditForm`
13. fnAccounts_ByRole(@Role)
    - Repository: `AccountRepository.GetAccountsByRole(string role)`
    - Forms: `AccountForm`
14. fnAuth_User(@Username, @Password)
    - Repository: `AccountRepository.CheckLogin(string username, string password)`
    - Forms: `LoginForm`

---

## Scalar Functions

1. GetDailyRevenue(@Date)
   - Repository: `ReportRepository.GetDailyRevenue(DateTime date)`
   - Forms: `StatisticsForm`
2. GetMonthlyRevenue(@Year, @Month)
   - Repository: `ReportRepository.GetMonthlyRevenue(int year, int month)`
   - Forms: `StatisticsForm`
3. GetDiscountedPrice(@ProductID, @OriginalPrice)
   - Repository: `DiscountRepository.GetDiscountedPrice(int productId, decimal originalPrice)`
   - Forms: `SalerProductForm`, `SalerInvoiceForm`
4. IsStockAvailable(@ProductID, @RequiredQuantity)
   - Repository: `ProductRepository.IsStockAvailable(int productId, int requiredQuantity)`
   - Forms: `SalerInvoiceForm`

---

## Saler Role (MiniMart_Saler)

### Stored Procedures (mutations)
Uses the same mutation endpoints as Manager Role for sales (create, add detail, update). Reads use TVFs listed above.

### Database Views
1. SalesSummary
   - Repository: `SaleRepository.GetSalesSummary()`
   - Forms: `SalerInvoiceHistoryForm`

---

## Repository Method Index

### ProductRepository.cs
- Procedures: `AddProduct`, `UpdateProduct`, `DeleteProduct`
- Functions: `fnProducts_All`, `fnProducts_ByID`, `GetProductByName`, `IsStockAvailable`
- Views: `LowStockProducts`

### CustomerRepository.cs
- Procedures: `AddCustomer`, `UpdateCustomer`, `DeleteCustomer`
- Functions: `fnCustomers_All`, `fnCustomers_ByID`, `fnCustomers_ByName`

### SaleRepository.cs
- Procedures: `CreateSale`, `AddSaleDetail`, `UpdateSale`
- Functions: `fnSales_ByID`, `fnSaleDetails_BySaleID`
- Views: `SalesSummary`
- Special: `CreateSaleWithDetails()` - Transaction-based sale creation

### DiscountRepository.cs
- Procedures: `AddDiscount`, `UpdateDiscount`, `DeleteDiscount`
- Functions: `fnDiscounts_Active`, `fnDiscounts_ByProduct`, `GetDiscountedPrice`

### AccountRepository.cs
- Procedures: `AddAccount`, `UpdateAccount`, `DeleteAccount`, `CreateSQLAccount`, `DeleteSQLAccount`, `ChangePassword`
- Functions: `fnAccounts_All`, `fnAccounts_ByUsername`, `fnAccounts_ByRole`, `fnAuth_User`
- Helper Methods: `IsUsernameExists()`

### ReportRepository.cs
- Functions: `GetDailyRevenue`, `GetMonthlyRevenue`
- Views: `LowStockProducts`

---

## Database Objects Summary

### Stored Procedures (15)
- Products: AddProduct, UpdateProduct, DeleteProduct
- Customers: AddCustomer, UpdateCustomer, DeleteCustomer
- Sales: CreateSale, AddSaleDetail, UpdateSale
- Accounts: AddAccount, UpdateAccount, DeleteAccount, CreateSQLAccount, DeleteSQLAccount, ChangePassword

### Scalar Functions (4)
- GetDailyRevenue, GetMonthlyRevenue, GetDiscountedPrice, IsStockAvailable

### Table-Valued Functions (14)
- Products: fnProducts_All, fnProducts_ByID, GetProductByName
- Customers: fnCustomers_All, fnCustomers_ByName, fnCustomers_ByID
- Sales: fnSales_ByID, fnSaleDetails_BySaleID
- Discounts: fnDiscounts_Active, fnDiscounts_ByProduct
- Accounts: fnAccounts_All, fnAccounts_ByUsername, fnAccounts_ByRole, fnAuth_User

### Database Views (2)
- LowStockProducts, SalesSummary

### Total Active Objects: 35

---

## Notes

- **Account Management**: Full CRUD for user accounts. UI additionally provisions SQL Login/User via `CreateSQLAccount`/`DeleteSQLAccount`.
- **Roles**: Only manager and saler are supported for accounts and SQL roles.
- **Auto-creation**: Accounts automatically create linked Employee records (unique phone generator). No Customer linkage is created anymore.
- **Transaction Safety**: All operations include comprehensive transaction management and rollback.
- **Search Functionality**: Most GetAll* methods support optional search parameters.
- **Error Handling**: Repository methods surface detailed messages from stored procedures.
- **Security**: Database roles `MiniMart_Manager` and `MiniMart_Saler` are applied accordingly.
- **Database Constraints**: CHECK constraints enforce valid `Position` values; UNIQUE `Phone` enforced with generated values.

---

## New Forms Added

### Account Management Forms
- **AccountForm**: Main account management interface with search and CRUD operations
- **AccountCreateForm**: Create new accounts with automatic Customer/Employee creation
- **AccountManageEditForm**: Edit existing accounts with role-based field visibility
- **AccountInfoForm**: View and update account information (password change)

### Integration Points
- **AdminForm**: Added "Quản lí tài khoản" menu item for account management access
- **Role-based UI**: Dynamic form fields based on selected account role
- **Auto-linking**: Automatic creation of Customer/Employee records when creating accounts
