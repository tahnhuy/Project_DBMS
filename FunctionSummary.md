# Function and Procedure Summary - Active Usage Only

This document maps all **actively used** stored procedures and SQL functions in the current Sale_Management project. Only procedures/functions that are actually called from C# repositories and forms are included.

**Project Status**: Refactored to focus on Sales, Customers, Products, Discounts, and basic Reports. Customer role removed.

---

## Manager Role (MiniMart_Manager)

### Stored Procedures

#### Products Management
1. **GetAllProducts**
   - **Repository**: `ProductRepository.GetAllProducts(string searchQuery = null)`
   - **Forms**: `ProductForm`, `AdminDiscountForm`, `AdminDiscountEditForm`
   - **Usage**: Load product list, search products

2. **AddProduct(@ProductName, @Price, @StockQuantity, @Unit)**
   - **Repository**: `ProductRepository.AddProduct(string productName, decimal price, int stockQuantity, string unit)`
   - **Forms**: `ProductEditForm`
   - **Usage**: Add new products

3. **UpdateProduct(@ProductID, @ProductName, @Price, @StockQuantity, @Unit)**
   - **Repository**: `ProductRepository.UpdateProduct(int productId, string productName, decimal price, int stockQuantity, string unit)`
   - **Forms**: `ProductEditForm`
   - **Usage**: Update existing products

4. **DeleteProduct(@ProductID)**
   - **Repository**: `ProductRepository.DeleteProduct(int productId)`
   - **Forms**: `ProductForm`
   - **Usage**: Delete products (with safety checks)

#### Customer Management
5. **GetAllCustomers**
   - **Repository**: `CustomerRepository.GetAllCustomers(string searchQuery = null)`
   - **Forms**: `CustomerManageForm`
   - **Usage**: Load customer list

6. **GetCustomerByName(@CustomerName)**
   - **Repository**: `CustomerRepository.GetCustomerByName(string customerName)`
   - **Forms**: `CustomerManageForm`
   - **Usage**: Search customers by name

7. **GetCustomerByID(@CustomerID)**
   - **Repository**: `CustomerRepository.GetCustomerById(int customerId)`
   - **Forms**: `CustomerManageForm`, `CustomerManageEditForm`
   - **Usage**: Get specific customer details

8. **AddCustomer(@CustomerName, @Phone, @Address, @LoyaltyPoints)**
   - **Repository**: `CustomerRepository.AddCustomer(string customerName, string phone, string address, int loyaltyPoints)`
   - **Forms**: `CustomerManageEditForm`
   - **Usage**: Add new customers

9. **UpdateCustomer(@CustomerID, @CustomerName, @Phone, @Address, @LoyaltyPoints)**
   - **Repository**: `CustomerRepository.UpdateCustomer(int customerId, string customerName, string phone, string address, int loyaltyPoints)`
   - **Forms**: `CustomerManageEditForm`
   - **Usage**: Update customer information

10. **DeleteCustomer(@CustomerID)**
    - **Repository**: `CustomerRepository.DeleteCustomer(int customerId)`
    - **Forms**: `CustomerManageForm`
    - **Usage**: Delete customers (with safety checks)

#### Discount Management
11. **GetActiveDiscounts**
    - **Repository**: `DiscountRepository.GetActiveDiscounts()`
    - **Forms**: `AdminDiscountForm`
    - **Usage**: Load active discounts

12. **GetDiscountsByProduct(@ProductID)**
    - **Repository**: `DiscountRepository.GetDiscountsByProduct(int productId)`
    - **Forms**: `AdminDiscountEditForm`
    - **Usage**: Get discounts for specific product

13. **AddDiscount(@ProductID, @DiscountType, @DiscountValue, @StartDate, @EndDate, @IsActive, @CreatedBy)**
    - **Repository**: `DiscountRepository.AddDiscount(int productId, string discountType, decimal discountValue, DateTime startDate, DateTime endDate, bool isActive, string createdBy)`
    - **Forms**: `AdminDiscountForm`, `AdminDiscountEditForm`
    - **Usage**: Create new discounts

14. **UpdateDiscount(@DiscountID, @ProductID, @DiscountType, @DiscountValue, @StartDate, @EndDate, @IsActive)**
    - **Repository**: `DiscountRepository.UpdateDiscount(int discountId, int productId, string discountType, decimal discountValue, DateTime startDate, DateTime endDate, bool isActive)`
    - **Forms**: `AdminDiscountEditForm`
    - **Usage**: Update existing discounts

15. **DeleteDiscount(@DiscountID)**
    - **Repository**: `DiscountRepository.DeleteDiscount(int discountId)`
    - **Forms**: `AdminDiscountForm`
    - **Usage**: Delete discounts (with safety checks)

#### Account Management
16. **CheckLogin(@Username, @Password)**
    - **Repository**: `AccountRepository.CheckLogin(string username, string password)`
    - **Forms**: `LoginForm`
    - **Usage**: User authentication

17. **ChangePassword(@Username, @OldPassword, @NewPassword)**
    - **Repository**: `AccountRepository.ChangePassword(string username, string oldPassword, string newPassword)`
    - **Forms**: `AccountInfoForm`
    - **Usage**: Change user password

### Scalar Functions

1. **GetDailyRevenue(@Date)**
   - **Repository**: `ReportRepository.GetDailyRevenue(DateTime date)`
   - **Forms**: `StatisticsForm`
   - **Usage**: Calculate daily revenue

2. **GetMonthlyRevenue(@Year, @Month)**
   - **Repository**: `ReportRepository.GetMonthlyRevenue(int year, int month)`
   - **Forms**: `StatisticsForm`
   - **Usage**: Calculate monthly revenue

3. **GetDiscountedPrice(@ProductID, @OriginalPrice)**
   - **Repository**: `DiscountRepository.GetDiscountedPrice(int productId, decimal originalPrice)`
   - **Forms**: `SalerProductForm`, `SalerInvoiceForm`
   - **Usage**: Calculate discounted price for products

### Table-Valued Functions

1. **GetProductByName(@ProductName)**
   - **Repository**: `ProductRepository.GetAllProducts(string searchQuery)` (when searchQuery provided)
   - **Forms**: `ProductForm`, `SalerProductForm`
   - **Usage**: Search products by name

### Database Views

1. **LowStockProducts**
   - **Repository**: `ProductRepository.GetLowStockProducts()`, `ReportRepository.GetLowStockProducts()`
   - **Forms**: `StatisticsForm`
   - **Usage**: Display products with low stock

---

## Saler Role (MiniMart_Saler)

### Stored Procedures

#### Product Access (Read-Only)
1. **GetAllProducts**
   - **Repository**: `ProductRepository.GetAllProducts(string searchQuery = null)`
   - **Forms**: `SalerProductForm`, `SalerInvoiceForm`
   - **Usage**: View product catalog

#### Customer Access (Read-Only)
2. **GetAllCustomers**
   - **Repository**: `CustomerRepository.GetAllCustomers(string searchQuery = null)`
   - **Forms**: `SalerInvoiceForm`, `SalerCustomerViewForm`
   - **Usage**: Select customers for invoices

#### Sales Management
3. **CreateSale(@CustomerID, @TotalAmount, @PaymentMethod)**
   - **Repository**: `SaleRepository.CreateSaleWithDetails(int? customerId, string paymentMethod, string createdBy, List<SaleDetailItem> saleDetails)`
   - **Forms**: `SalerInvoiceForm`
   - **Usage**: Create new sales transactions

4. **AddSaleDetail(@SaleID, @ProductID, @Quantity, @SalePrice)**
   - **Repository**: `SaleRepository.CreateSaleWithDetails()` (internal call)
   - **Forms**: `SalerInvoiceForm`
   - **Usage**: Add items to sales

5. **UpdateSale(@SaleID, @CustomerID, @TotalAmount, @PaymentMethod)**
   - **Repository**: `SaleRepository.CreateSaleWithDetails()` (internal call)
   - **Forms**: `SalerInvoiceForm`
   - **Usage**: Update sale totals

6. **GetSaleByID(@SaleID)**
   - **Repository**: `SaleRepository.GetSaleById(int saleId)`
   - **Forms**: `SalerInvoiceHistoryForm`
   - **Usage**: View sale details

7. **GetSaleDetails(@SaleID)**
   - **Repository**: `SaleRepository.GetSaleDetails(int saleId)`
   - **Forms**: `SalerInvoiceHistoryForm`
   - **Usage**: View sale item details

#### Account Management
8. **CheckLogin(@Username, @Password)**
   - **Repository**: `AccountRepository.CheckLogin(string username, string password)`
   - **Forms**: `LoginForm`
   - **Usage**: User authentication

9. **ChangePassword(@Username, @OldPassword, @NewPassword)**
   - **Repository**: `AccountRepository.ChangePassword(string username, string oldPassword, string newPassword)`
   - **Forms**: `AccountInfoForm`
   - **Usage**: Change user password

### Scalar Functions

1. **GetDiscountedPrice(@ProductID, @OriginalPrice)**
   - **Repository**: `DiscountRepository.GetDiscountedPrice(int productId, decimal originalPrice)`
   - **Forms**: `SalerProductForm`, `SalerInvoiceForm`
   - **Usage**: Calculate discounted prices for customers

2. **IsStockAvailable(@ProductID, @RequiredQuantity)**
   - **Repository**: `ProductRepository.IsStockAvailable(int productId, int requiredQuantity)`
   - **Forms**: `SalerInvoiceForm`
   - **Usage**: Check stock availability before sale

### Table-Valued Functions

1. **GetProductByName(@ProductName)**
   - **Repository**: `ProductRepository.GetAllProducts(string searchQuery)` (when searchQuery provided)
   - **Forms**: `SalerProductForm`
   - **Usage**: Search products by name

### Database Views

1. **SalesSummary**
   - **Repository**: `SaleRepository.GetSalesSummary()`
   - **Forms**: `SalerInvoiceHistoryForm`
   - **Usage**: Display sales history

---

## Repository Method Index

### ProductRepository.cs
- **Procedures**: `GetAllProducts`, `AddProduct`, `UpdateProduct`, `DeleteProduct`
- **Functions**: `GetProductByName` (via GetAllProducts), `IsStockAvailable`
- **Views**: `LowStockProducts`

### CustomerRepository.cs
- **Procedures**: `GetAllCustomers`, `GetCustomerByID`, `GetCustomerByName`, `AddCustomer`, `UpdateCustomer`, `DeleteCustomer`

### SaleRepository.cs
- **Procedures**: `GetSaleByID`, `GetSaleDetails`, `CreateSale`, `AddSaleDetail`, `UpdateSale`
- **Views**: `SalesSummary`
- **Special**: `CreateSaleWithDetails()` - Transaction-based sale creation

### DiscountRepository.cs
- **Procedures**: `GetActiveDiscounts`, `GetDiscountsByProduct`, `AddDiscount`, `UpdateDiscount`, `DeleteDiscount`
- **Functions**: `GetDiscountedPrice`

### AccountRepository.cs
- **Procedures**: `CheckLogin`, `ChangePassword`

### ReportRepository.cs
- **Functions**: `GetDailyRevenue`, `GetMonthlyRevenue`
- **Views**: `LowStockProducts`

---

## Database Objects Summary

### Stored Procedures (17)
- **Products**: GetAllProducts, AddProduct, UpdateProduct, DeleteProduct
- **Customers**: GetAllCustomers, GetCustomerByID, GetCustomerByName, AddCustomer, UpdateCustomer, DeleteCustomer
- **Sales**: CreateSale, AddSaleDetail, UpdateSale, GetSaleByID, GetSaleDetails
- **Discounts**: GetActiveDiscounts, GetDiscountsByProduct, AddDiscount, UpdateDiscount, DeleteDiscount
- **Accounts**: CheckLogin, ChangePassword

### Scalar Functions (4)
- GetDailyRevenue, GetMonthlyRevenue, GetDiscountedPrice, IsStockAvailable

### Table-Valued Functions (1)
- GetProductByName

### Database Views (2)
- LowStockProducts, SalesSummary

### Total Active Objects: 24

---

## Notes

- **Customer role removed**: No customer-specific forms or procedures
- **Transaction safety**: All delete operations include safety checks
- **Search functionality**: Most GetAll* methods support optional search parameters
- **Error handling**: All repository methods include comprehensive error handling
- **Security**: Login and password change functionality maintained for both roles
