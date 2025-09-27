# 📋 Forms Documentation - Sale Management System

## 🎯 Overview
This document provides a comprehensive overview of all forms used in the Sale_Management project, organized by functional areas and user roles.

---

## 📁 Form Structure

```
Forms/
├── Common/           # Shared forms across all roles
├── Manager/          # Manager-specific forms
├── Saler/           # Salesperson-specific forms
└── Customer/        # Customer-related forms
```

---

## 🔐 Common Forms (Shared)

### 1. **LoginForm** 
- **File**: `Forms/Common/LoginForm.cs`
- **Purpose**: User authentication and role-based access control
- **Key Features**:
  - Username/password input with validation
  - Show/hide password toggle
  - Role-based redirection (Manager → AdminForm, Saler → SalerForm)
  - Error handling with user-friendly messages
- **Database Integration**: `AccountRepository.CheckLogin()` → `SP CheckLogin`
- **UI Components**: TextBox (username, password), CheckBox (show password), Buttons (login, cancel)

### 2. **AccountInfoForm**
- **File**: `Forms/Common/AccountInfoForm.cs`
- **Purpose**: User account information display and password management
- **Key Features**:
  - Display current user information (username, role)
  - Change password functionality
  - Account statistics display
- **Database Integration**: `AccountRepository.ChangePassword()` → `SP ChangePassword`
- **UI Components**: Labels (user info), TextBox (password fields), Button (change password)

---

## 👨‍💼 Manager Forms

### 1. **AdminForm** (Main Menu)
- **File**: `Forms/Manager/AdminForm.cs`
- **Purpose**: Main dashboard and navigation hub for managers
- **Key Features**:
  - Welcome message with current user info
  - Menu system for accessing all manager functions
  - Dynamic form loading in panel container
  - Role-based access control
- **Menu Items**:
  - `msi_Products` → Opens ProductForm
  - `msi_Customers` → Opens CustomerManageForm
  - `msi_Discounts` → Opens AdminDiscountForm
  - `msi_Statistics` → Opens StatisticsForm
  - `msi_AccountInfo` → Opens AccountInfoForm
- **UI Components**: MenuStrip, Panel (container), Label (welcome)

### 2. **ProductForm**
- **File**: `Forms/Manager/ProductForm.cs`
- **Purpose**: Product management with search and CRUD operations
- **Key Features**:
  - Display all products in DataGridView
  - Search functionality by product name
  - Add/Edit/Delete product operations
  - Stock level monitoring
  - Discount information display
- **Database Integration**: 
  - `ProductRepository.GetAllProducts(searchQuery)` → `SP GetAllProducts` or `dbo.GetProductByName`
  - `ProductRepository.AddProduct()` → `SP AddProduct`
  - `ProductRepository.UpdateProduct()` → `SP UpdateProduct`
- **UI Components**: DataGridView, TextBox (search), Buttons (add, edit, delete, search)

### 3. **ProductEditForm**
- **File**: `Forms/Manager/ProductEditForm.cs`
- **Purpose**: Add/Edit product details
- **Key Features**:
  - Product information input (name, price, stock, unit, description)
  - Validation for required fields
  - Save/Cancel operations
- **Database Integration**: `ProductRepository.AddProduct()` / `UpdateProduct()`
- **UI Components**: TextBox, NumericUpDown, ComboBox, RichTextBox, Buttons

### 4. **CustomerManageForm**
- **File**: `Forms/Manager/CustomerManageForm.cs`
- **Purpose**: Customer management with search and CRUD operations
- **Key Features**:
  - Display all customers in DataGridView
  - Search functionality by customer name
  - Add/Edit customer operations
  - Customer information management
- **Database Integration**:
  - `CustomerRepository.GetAllCustomers(searchQuery)` → `SP GetAllCustomers` or `GetCustomerByName`
  - `CustomerRepository.AddCustomer()` → `SP AddCustomer`
  - `CustomerRepository.UpdateCustomer()` → `SP UpdateCustomer`
- **UI Components**: DataGridView, TextBox (search), Buttons (add, edit, search)

### 5. **CustomerManageEditForm**
- **File**: `Forms/Manager/CustomerManageEditForm.cs`
- **Purpose**: Add/Edit customer details
- **Key Features**:
  - Customer information input (name, phone, address, email)
  - Validation for required fields
  - Save/Cancel operations
- **Database Integration**: `CustomerRepository.AddCustomer()` / `UpdateCustomer()`
- **UI Components**: TextBox, MaskedTextBox (phone), RichTextBox, Buttons

### 6. **AdminDiscountForm**
- **File**: `Forms/Manager/AdminDiscountForm.cs`
- **Purpose**: Discount program management
- **Key Features**:
  - Display active discounts in DataGridView
  - Filter discounts by product
  - Add/Edit discount operations
  - Discount validation and date management
- **Database Integration**:
  - `DiscountRepository.GetActiveDiscounts()` → `SP GetActiveDiscounts`
  - `DiscountRepository.GetDiscountsByProduct()` → `SP GetDiscountsByProduct`
  - `DiscountRepository.AddDiscount()` → `SP AddDiscount`
  - `DiscountRepository.UpdateDiscount()` → `SP UpdateDiscount`
- **UI Components**: DataGridView, ComboBox (product filter), Buttons (add, edit)

### 7. **AdminDiscountEditForm**
- **File**: `Forms/Manager/AdminDiscountEditForm.cs`
- **Purpose**: Add/Edit discount details
- **Key Features**:
  - Discount information input (product, percentage, start/end dates)
  - Product selection from dropdown
  - Date validation
  - Save/Cancel operations
- **Database Integration**: `DiscountRepository.AddDiscount()` / `UpdateDiscount()`
- **UI Components**: ComboBox (product), NumericUpDown (percentage), DateTimePicker, Buttons

### 8. **StatisticsForm**
- **File**: `Forms/Manager/StatisticsForm.cs`
- **Purpose**: Business analytics and reporting dashboard
- **Key Features**:
  - **Tab 1**: Daily Revenue (`ReportRepository.GetDailyRevenue()`)
  - **Tab 2**: Monthly Revenue (`ReportRepository.GetMonthlyRevenue()`)
  - **Tab 3**: Low Stock Products (`view LowStockProducts`)
  - **Tab 4**: Products with Discounts (`view ProductsWithDiscounts`)
- **Database Integration**: `ReportRepository` methods and database views
- **UI Components**: TabControl, DataGridView, DateTimePicker, NumericUpDown, Labels

---

## 💼 Saler Forms

### 1. **SalerForm** (Main Menu)
- **File**: `Forms/Saler/SalerForm.cs`
- **Purpose**: Main dashboard and navigation hub for salespersons
- **Key Features**:
  - Welcome message with current user info
  - Menu system for accessing sales functions
  - Default view shows SalerProductForm
- **Menu Items**:
  - `msi_Product` → Opens SalerProductForm
  - `msi_Customer` → Opens CustomerManageForm
  - `msi_Invoice` → Opens SalerInvoiceForm
  - `msi_History` → Opens SalerInvoiceHistoryForm
  - `msi_AccountInfo` → Opens AccountInfoForm
- **UI Components**: MenuStrip, Panel (container), Label (welcome)

### 2. **SalerProductForm**
- **File**: `Forms/Saler/SalerProductForm.cs`
- **Purpose**: Product catalog view for salespersons
- **Key Features**:
  - Display products with current pricing
  - Show discounted prices using `dbo.GetDiscountedPrice()`
  - Product search functionality
  - Stock availability display
- **Database Integration**: `ProductRepository.GetAllProducts()` → `SP GetAllProducts`
- **UI Components**: DataGridView, TextBox (search), Button (search)

### 3. **SalerCustomerViewForm**
- **File**: `Forms/Saler/SalerCustomerViewForm.cs`
- **Purpose**: Customer information lookup for sales
- **Key Features**:
  - Customer search and selection
  - Customer information display
  - Integration with invoice creation
- **Database Integration**: `CustomerRepository.GetCustomerById()` / `GetCustomerByName()`
- **UI Components**: DataGridView, TextBox (search), Button (search)

### 4. **SalerInvoiceForm**
- **File**: `Forms/Saler/SalerInvoiceForm.cs`
- **Purpose**: Invoice creation and sales transaction processing
- **Key Features**:
  - **4-Step Sales Process**:
    1. Select customer (optional)
    2. Add products to invoice
    3. Review invoice details
    4. Complete transaction
  - Real-time total calculation
  - Stock validation before adding items
  - Payment method selection
  - Transaction handling with rollback on errors
- **Database Integration**:
  - `SaleRepository.CreateSaleWithDetails()` → `SP CreateSale` + `AddSaleDetail` + `UpdateSale`
  - Uses `SqlTransaction` for atomic operations
- **UI Components**: ComboBox (customer, product, payment), NumericUpDown (quantity), DataGridView (invoice items), Buttons (add item, complete sale)

### 5. **SalerInvoiceHistoryForm**
- **File**: `Forms/Saler/SalerInvoiceHistoryForm.cs`
- **Purpose**: Invoice history lookup and details
- **Key Features**:
  - Search invoices by ID or date range
  - Display invoice details
  - Print invoice functionality
- **Database Integration**:
  - `SaleRepository.GetSaleById()` → `SP GetSaleById`
  - `SaleRepository.GetSaleDetails()` → `SP GetSaleDetails`
- **UI Components**: DataGridView, TextBox (search), DateTimePicker, Buttons (search, print)

---

## 🛒 Customer Forms

### 1. **CustomerForm**
- **File**: `Forms/Customer/CustomerForm.cs`
- **Purpose**: Customer information management
- **Key Features**:
  - Customer registration
  - Profile management
  - Purchase history view
- **Database Integration**: `CustomerRepository` methods
- **UI Components**: TextBox, MaskedTextBox, RichTextBox, Buttons

### 2. **CustomerEditForm**
- **File**: `Forms/Customer/CustomerEditForm.cs`
- **Purpose**: Customer profile editing
- **Key Features**:
  - Edit customer information
  - Validation and save operations
- **Database Integration**: `CustomerRepository.UpdateCustomer()`
- **UI Components**: TextBox, MaskedTextBox, RichTextBox, Buttons

---

## 🗂️ Deprecated/Removed Forms

The following forms were removed during refactoring to simplify the system:

- **AccountCreateForm** - Account creation (removed from project)
- **AccountForm** - Account management (removed from project)  
- **AccountManageForm** - Advanced account management (removed from project)
- **AccountManageEditForm** - Account editing (removed from project)

---

## 🔄 Form Navigation Flow

### Manager Role Flow:
```
LoginForm → AdminForm → [ProductForm | CustomerManageForm | AdminDiscountForm | StatisticsForm]
```

### Saler Role Flow:
```
LoginForm → SalerForm → [SalerProductForm | SalerCustomerViewForm | SalerInvoiceForm | SalerInvoiceHistoryForm]
```

---

## 🎨 UI/UX Standards

### DataGridView Configuration:
- `AutoGenerateColumns = false`
- `DataPropertyName` set to match database column names
- Currency formatting: `DefaultCellStyle.Format = "N0"` or `"N2"`
- Read-only for display forms, editable for management forms

### Error Handling:
- All forms use `try-catch` blocks
- Error messages displayed via `MessageBox.Show(ex.Message, "Error")`
- User-friendly Vietnamese error messages

### Validation:
- Required field validation
- Data type validation (numeric inputs)
- Business rule validation (stock availability, date ranges)

---

## 📊 Database Integration Summary

| Form | Primary Repository | Key Stored Procedures/Functions |
|------|-------------------|----------------------------------|
| LoginForm | AccountRepository | CheckLogin |
| ProductForm | ProductRepository | GetAllProducts, AddProduct, UpdateProduct |
| CustomerManageForm | CustomerRepository | GetAllCustomers, AddCustomer, UpdateCustomer |
| AdminDiscountForm | DiscountRepository | GetActiveDiscounts, AddDiscount, UpdateDiscount |
| StatisticsForm | ReportRepository | GetDailyRevenue, GetMonthlyRevenue |
| SalerInvoiceForm | SaleRepository | CreateSale, AddSaleDetail, UpdateSale |
| SalerInvoiceHistoryForm | SaleRepository | GetSaleById, GetSaleDetails |

---

## 🚀 Key Features Implemented

1. **Role-Based Access Control**: Different interfaces for Manager vs Saler roles
2. **Search Functionality**: All management forms include search capabilities
3. **Transaction Safety**: Invoice creation uses atomic transactions
4. **Real-Time Updates**: Stock levels and pricing updated in real-time
5. **Comprehensive Reporting**: 4-tab statistics dashboard for managers
6. **User-Friendly Interface**: Vietnamese language support throughout
7. **Error Handling**: Comprehensive error handling with user feedback

---

*This documentation reflects the current state of the Sale_Management system after refactoring to focus on core sales, customer, product, discount, and reporting functionality.*
