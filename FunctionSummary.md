# Function and Procedure Summary - Active Usage Only

This document maps all actively used database objects that are actually called from C# repositories and forms. Read/list/detail queries now use TVFs. Create/update/delete actions remain stored procedures.

**Project Status**: Reads via TVFs; mutations via stored procedures. Full Account management with automatic Customer/Employee creation.

---

## Manager Role (MiniMart_Manager)

### Stored Procedures (mutations)

#### Products
1. AddProduct(@ProductName, @Price, @StockQuantity, @Unit)
   - Repository: `ProductRepository.AddProduct(...)`
   - Forms: `ProductEditForm`
2. UpdateProduct(@ProductID, @ProductName, @Price, @StockQuantity, @Unit)
   - Repository: `ProductRepository.UpdateProduct(...)`
   - Forms: `ProductEditForm`
3. DeleteProduct(@ProductID)
   - Repository: `ProductRepository.DeleteProduct(int productId)`
   - Forms: `ProductForm`

#### Customers
4. AddCustomer(@CustomerName, @Phone, @Address, @LoyaltyPoints)
   - Repository: `CustomerRepository.AddCustomer(...)`
   - Forms: `CustomerManageEditForm`
5. UpdateCustomer(@CustomerID, @CustomerName, @Phone, @Address, @LoyaltyPoints)
   - Repository: `CustomerRepository.UpdateCustomer(...)`
   - Forms: `CustomerManageEditForm`
6. DeleteCustomer(@CustomerID)
   - Repository: `CustomerRepository.DeleteCustomer(int customerId)`
   - Forms: `CustomerManageForm`

#### Discounts
7. AddDiscount(@ProductID, @DiscountType, @DiscountValue, @StartDate, @EndDate, @IsActive, @CreatedBy)
   - Repository: `DiscountRepository.AddDiscount(...)`
   - Forms: `AdminDiscountForm`, `AdminDiscountEditForm`
8. UpdateDiscount(@DiscountID, @ProductID, @DiscountType, @DiscountValue, @StartDate, @EndDate, @IsActive)
   - Repository: `DiscountRepository.UpdateDiscount(...)`
   - Forms: `AdminDiscountEditForm`
9. DeleteDiscount(@DiscountID)
   - Repository: `DiscountRepository.DeleteDiscount(int discountId)`
   - Forms: `AdminDiscountForm`

#### Sales
10. CreateSale(@CustomerID, @TotalAmount, @PaymentMethod)
    - Repository: `SaleRepository.CreateSaleWithDetails(...)`
    - Forms: `SalerInvoiceForm`
11. AddSaleDetail(@SaleID, @ProductID, @Quantity, @SalePrice)
    - Repository: `SaleRepository.CreateSaleWithDetails()` (internal)
    - Forms: `SalerInvoiceForm`
12. UpdateSale(@SaleID, @CustomerID, @TotalAmount, @PaymentMethod)
    - Repository: `SaleRepository.CreateSaleWithDetails()` and `UpdateSale(...)`
    - Forms: `SalerInvoiceForm`

#### Accounts
13. AddAccount(@Username, @Password, @Role, @CustomerID, @EmployeeID)
    - Repository: `AccountRepository.AddAccount(...)`
    - Forms: `AccountCreateForm`
14. UpdateAccount(@Username, @Password, @Role, @CustomerID, @EmployeeID)
    - Repository: `AccountRepository.UpdateAccount(...)`
    - Forms: `AccountManageEditForm`
15. DeleteAccount(@Username)
    - Repository: `AccountRepository.DeleteAccount(string username)`
    - Forms: `AccountForm`
16. ChangePassword(@Username, @OldPassword, @NewPassword)
    - Repository: `AccountRepository.ChangePassword(...)`
    - Forms: `AccountInfoForm`

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
- Procedures: `AddAccount`, `UpdateAccount`, `DeleteAccount`, `ChangePassword`
- Functions: `fnAccounts_All`, `fnAccounts_ByUsername`, `fnAccounts_ByRole`, `fnAuth_User`
- Helper Methods: `GetCustomerCount()`, `GetEmployeeCount()`, `IsUsernameExists()`

### ReportRepository.cs
- Functions: `GetDailyRevenue`, `GetMonthlyRevenue`
- Views: `LowStockProducts`

---

## Database Objects Summary

### Stored Procedures (13)
- Products: AddProduct, UpdateProduct, DeleteProduct
- Customers: AddCustomer, UpdateCustomer, DeleteCustomer
- Sales: CreateSale, AddSaleDetail, UpdateSale
- Accounts: AddAccount, UpdateAccount, DeleteAccount, ChangePassword

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

### Total Active Objects: 33

---

## Notes

- **Account Management**: Full CRUD operations for user accounts with automatic Customer/Employee creation
- **Role-based Access**: Manager, Saler, and Customer roles with proper constraints
- **Auto-creation**: Accounts automatically create linked Customer/Employee records with default values
- **Transaction Safety**: All operations include comprehensive transaction management and rollback
- **Search Functionality**: Most GetAll* methods support optional search parameters
- **Error Handling**: All repository methods include comprehensive error handling with detailed messages
- **Security**: Login authentication and password change functionality maintained for all roles
- **Database Constraints**: Proper CHECK constraints ensure data integrity (Position values, Role validation)

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
