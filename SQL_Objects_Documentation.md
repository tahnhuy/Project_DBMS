# 📋 DOCUMENTATION: SQL OBJECTS VÀ C# CODE TƯƠNG ỨNG

## 📊 TỔNG QUAN HỆ THỐNG

Hệ thống quản lý bán hàng minimart bao gồm:
- **49 Stored Procedures** - Xử lý logic nghiệp vụ
- **10 Views** - Tổng hợp và báo cáo dữ liệu  
- **16 Functions** - Tính toán và xử lý dữ liệu
- **8 Triggers** - Tự động hóa và ràng buộc dữ liệu
- **3 Database Roles** - Phân quyền bảo mật

---

## 🗄️ STORED PROCEDURES (49 procedures)

### 📦 1. QUẢN LÝ SẢN PHẨM (4 procedures)

#### 1.1 GetAllProducts
**Chức năng:** Lấy danh sách tất cả sản phẩm  
**SQL:**
```sql
CREATE PROCEDURE GetAllProducts 
AS
BEGIN
    SELECT [ProductID], [ProductName], [Price], [StockQuantity], [Unit]
    FROM dbo.Products
    ORDER BY [ProductID]
END
```
**C# Code:**
```csharp
// File: DatabaseAccess/ProductRepository.cs - Line 17-27
public DataTable GetAllProducts()
{
    return DatabaseConnection.ExecuteQuery("GetAllProducts", CommandType.StoredProcedure, null);
}
```

#### 1.2 AddProduct
**Chức năng:** Thêm sản phẩm mới  
**Parameters:** @ProductName, @Price, @StockQuantity, @Unit  
**C# Code:**
```csharp
// File: DatabaseAccess/ProductRepository.cs - Line 63-91
public bool AddProduct(string productName, decimal price, int stockQuantity, string unit)
{
    SqlParameter[] parameters = new SqlParameter[]
    {
        new SqlParameter("@ProductName", SqlDbType.NVarChar, 100) { Value = productName },
        new SqlParameter("@Price", SqlDbType.Decimal) { Value = price },
        new SqlParameter("@StockQuantity", SqlDbType.Int) { Value = stockQuantity },
        new SqlParameter("@Unit", SqlDbType.NVarChar, 50) { Value = unit }
    };
    
    DataTable result = DatabaseConnection.ExecuteQuery("AddProduct", CommandType.StoredProcedure, parameters);
    // Xử lý kết quả...
}
```

#### 1.3 UpdateProduct
**Chức năng:** Cập nhật thông tin sản phẩm  
**Parameters:** @ProductID, @ProductName, @Price, @StockQuantity, @Unit  
**C# Code:**
```csharp
// File: DatabaseAccess/ProductRepository.cs - Line 93-122
public bool UpdateProduct(int productId, string productName, decimal price, int stockQuantity, string unit)
```

#### 1.4 DeleteProduct
**Chức năng:** Xóa sản phẩm (kiểm tra ràng buộc với hóa đơn)  
**Parameters:** @ProductID  
**C# Code:**
```csharp
// File: DatabaseAccess/ProductRepository.cs - Line 124-149
public bool DeleteProduct(int productId)
```

### 👥 2. QUẢN LÝ KHÁCH HÀNG (7 procedures)

#### 2.1 GetAllCustomers
**Chức năng:** Lấy danh sách tất cả khách hàng  
**C# Code:**
```csharp
// File: DatabaseAccess/CustomerRepository.cs - Line 9-19
public DataTable GetAllCustomers()
```

#### 2.2 GetCustomerByName
**Chức năng:** Tìm khách hàng theo tên  
**Parameters:** @CustomerName  
**C# Code:**
```csharp
// File: DatabaseAccess/CustomerRepository.cs - Line 37-51
public DataTable GetCustomerByName(string customerName)
```

#### 2.3 GetCustomerByID
**Chức năng:** Lấy thông tin khách hàng theo ID  
**Parameters:** @CustomerID  
**C# Code:**
```csharp
// File: DatabaseAccess/CustomerRepository.cs - Line 21-35
public DataTable GetCustomerById(int customerId)
```

#### 2.4 AddCustomer
**Chức năng:** Thêm khách hàng mới  
**Parameters:** @CustomerName, @Phone, @Address, @LoyaltyPoints  
**C# Code:**
```csharp
// File: DatabaseAccess/CustomerRepository.cs - Line 53-81
public bool AddCustomer(string customerName, string phone, string address, int loyaltyPoints)
```

#### 2.5 UpdateCustomer
**Chức năng:** Cập nhật thông tin khách hàng  
**Parameters:** @CustomerID, @CustomerName, @Phone, @Address, @LoyaltyPoints  
**C# Code:**
```csharp
// File: DatabaseAccess/CustomerRepository.cs - Line 83-112
public bool UpdateCustomer(int customerId, string customerName, string phone, string address, int loyaltyPoints)
```

#### 2.6 DeleteCustomer
**Chức năng:** Xóa khách hàng (kiểm tra ràng buộc với hóa đơn)  
**Parameters:** @CustomerID  
**C# Code:**
```csharp
// File: DatabaseAccess/CustomerRepository.cs - Line 114-139
public bool DeleteCustomer(int customerId)
```

#### 2.7 SearchCustomers
**Chức năng:** Tìm kiếm khách hàng đa điều kiện  
**Parameters:** @SearchTerm  
**C# Code:**
```csharp
// File: DatabaseAccess/CustomerRepository.cs - Line 180-194
public DataTable SearchCustomers(string searchTerm)
```

### 🔐 3. QUẢN LÝ TÀI KHOẢN (22 procedures)

#### 3.1 CheckLogin
**Chức năng:** Xác thực đăng nhập  
**Parameters:** @Username, @Password  
**C# Code:**
```csharp
// File: DatabaseAccess/AccountRepository.cs - Line 19-27
public static DataTable CheckLogin(string username, string password)
```

#### 3.2 GetAllAccounts
**Chức năng:** Lấy danh sách tất cả tài khoản  
**C# Code:**
```csharp
// File: DatabaseAccess/AccountRepository.cs - Line 13-17
public static DataTable GetAllAccounts()
```

#### 3.3 AddAccount
**Chức năng:** Tạo tài khoản mới  
**Parameters:** @CreatorUsername, @NewUsername, @NewPassword, @NewRole  
**C# Code:**
```csharp
// File: DatabaseAccess/AccountRepository.cs - Line 29-39
public static DataTable AddAccount(string creatorUsername, string newUsername, string newPassword, string newRole)
```

#### 3.4 UpdateAccount
**Chức năng:** Cập nhật tài khoản  
**Parameters:** @Username, @NewPassword, @NewRole  
**C# Code:**
```csharp
// File: DatabaseAccess/AccountRepository.cs - Line 41-50
public static DataTable UpdateAccount(string username, string newPassword = null, string newRole = null)
```

#### 3.5 DeleteAccount
**Chức năng:** Xóa tài khoản  
**Parameters:** @Username  
**C# Code:**
```csharp
// File: DatabaseAccess/AccountRepository.cs - Line 52-59
public static DataTable DeleteAccount(string username)
```

#### 3.6 GetAccountDetails
**Chức năng:** Lấy chi tiết tài khoản  
**Parameters:** @Username  

#### 3.7 CheckAccountExists
**Chức năng:** Kiểm tra tài khoản có tồn tại  
**Parameters:** @Username  

#### 3.8 SearchAccounts
**Chức năng:** Tìm kiếm tài khoản  
**Parameters:** @SearchTerm  

#### 3.9 GetAccountsByRole
**Chức năng:** Lấy tài khoản theo vai trò  
**Parameters:** @Role  

#### 3.10 CountAccountsByRole
**Chức năng:** Đếm số tài khoản theo vai trò  
**Parameters:** @Role  

#### 3.11 CheckUserPermission
**Chức năng:** Kiểm tra quyền hạn người dùng  
**Parameters:** @Username, @Action  

#### 3.12 ChangePassword
**Chức năng:** Đổi mật khẩu  
**Parameters:** @Username, @OldPassword, @NewPassword  

#### 3.13 ResetPassword
**Chức năng:** Reset mật khẩu (chỉ admin/manager)  
**Parameters:** @ManagerUsername, @TargetUsername, @NewPassword  

#### 3.14 GetAccountStatistics
**Chức năng:** Thống kê tài khoản theo vai trò  

#### 3.15 GetAccountActivity
**Chức năng:** Lấy hoạt động của tài khoản  
**Parameters:** @Username  

#### 3.16 ValidateUsername
**Chức năng:** Validate tên đăng nhập  
**Parameters:** @Username  

#### 3.17 ValidatePassword
**Chức năng:** Validate mật khẩu  
**Parameters:** @Password  

#### 3.18 GetAccountsWithDetails
**Chức năng:** Lấy tài khoản kèm chi tiết khách hàng  

#### 3.19 BackupAccounts
**Chức năng:** Backup dữ liệu tài khoản  

#### 3.20 GetCustomerByUsername
**Chức năng:** Lấy thông tin khách hàng theo username  
**Parameters:** @Username  
**C# Code:**
```csharp
// File: DatabaseAccess/CustomerRepository.cs - Line 141-148
public DataTable GetCustomerByUsername(string username)
```

#### 3.21 LinkAccountToCustomer
**Chức năng:** Liên kết tài khoản với khách hàng  
**Parameters:** @Username, @CustomerID  

#### 3.22 UpdateCustomerByUsername
**Chức năng:** Cập nhật thông tin khách hàng theo username  
**Parameters:** @Username, @CustomerName, @Phone, @Address  
**C# Code:**
```csharp
// File: DatabaseAccess/CustomerRepository.cs - Line 159-178
public bool UpdateCustomerByUsername(string username, string customerName, string phone, string address)
```

### 💰 4. QUẢN LÝ BÁN HÀNG (7 procedures)

#### 4.1 GetAllSales
**Chức năng:** Lấy danh sách tất cả hóa đơn  
**C# Code:**
```csharp
// File: DatabaseAccess/SaleRepository.cs - Line 9-19
public DataTable GetAllSales()
```

#### 4.2 GetSaleByID
**Chức năng:** Lấy hóa đơn theo ID  
**Parameters:** @SaleID  
**C# Code:**
```csharp
// File: DatabaseAccess/SaleRepository.cs - Line 21-35
public DataTable GetSaleById(int saleId)
```

#### 4.3 GetSaleDetails
**Chức năng:** Lấy chi tiết hóa đơn  
**Parameters:** @SaleID  
**C# Code:**
```csharp
// File: DatabaseAccess/SaleRepository.cs - Line 37-51
public DataTable GetSaleDetails(int saleId)
```

#### 4.4 CreateSale
**Chức năng:** Tạo hóa đơn mới  
**Parameters:** @CustomerID, @TotalAmount, @PaymentMethod  
**C# Code:**
```csharp
// File: DatabaseAccess/SaleRepository.cs - Line 53-80
public int CreateSale(int? customerId, decimal totalAmount, string paymentMethod)
```

#### 4.5 AddSaleDetail
**Chức năng:** Thêm chi tiết vào hóa đơn  
**Parameters:** @SaleID, @ProductID, @Quantity, @SalePrice  
**C# Code:**
```csharp
// File: DatabaseAccess/SaleRepository.cs - Line 82-110
public bool AddSaleDetail(int saleId, int productId, int quantity, decimal salePrice)
```

#### 4.6 UpdateSale
**Chức năng:** Cập nhật thông tin hóa đơn  
**Parameters:** @SaleID, @CustomerID, @TotalAmount, @PaymentMethod  
**C# Code:**
```csharp
// File: DatabaseAccess/SaleRepository.cs - Line 112-140
public bool UpdateSale(int saleId, int? customerId, decimal totalAmount, string paymentMethod)
```

#### 4.7 DeleteSale
**Chức năng:** Xóa hóa đơn và chi tiết  
**Parameters:** @SaleID  
**C# Code:**
```csharp
// File: DatabaseAccess/SaleRepository.cs - Line 142-167
public bool DeleteSale(int saleId)
```

### 🎯 5. QUẢN LÝ GIẢM GIÁ (6 procedures)

#### 5.1 GetAllDiscounts
**Chức năng:** Lấy tất cả chương trình giảm giá  
**C# Code:**
```csharp
// File: DatabaseAccess/DiscountRepository.cs - Line 9-19
public DataTable GetAllDiscounts()
```

#### 5.2 GetDiscountsByProduct
**Chức năng:** Lấy giảm giá theo sản phẩm  
**Parameters:** @ProductID  
**C# Code:**
```csharp
// File: DatabaseAccess/DiscountRepository.cs - Line 21-35
public DataTable GetDiscountsByProduct(int productId)
```

#### 5.3 GetActiveDiscounts
**Chức năng:** Lấy giảm giá đang hoạt động  
**C# Code:**
```csharp
// File: DatabaseAccess/DiscountRepository.cs - Line 37-47
public DataTable GetActiveDiscounts()
```

#### 5.4 AddDiscount
**Chức năng:** Thêm chương trình giảm giá  
**Parameters:** @ProductID, @DiscountType, @DiscountValue, @StartDate, @EndDate, @IsActive, @CreatedBy  
**C# Code:**
```csharp
// File: DatabaseAccess/DiscountRepository.cs - Line 61-93
public bool AddDiscount(int productId, string discountType, decimal discountValue, 
                      DateTime startDate, DateTime endDate, bool isActive, string createdBy)
```

#### 5.5 UpdateDiscount
**Chức năng:** Cập nhật giảm giá  
**Parameters:** @DiscountID, @ProductID, @DiscountType, @DiscountValue, @StartDate, @EndDate, @IsActive  
**C# Code:**
```csharp
// File: DatabaseAccess/DiscountRepository.cs - Line 95-127
public bool UpdateDiscount(int discountId, int productId, string discountType, decimal discountValue,
                         DateTime startDate, DateTime endDate, bool isActive)
```

#### 5.6 DeleteDiscount
**Chức năng:** Xóa chương trình giảm giá  
**Parameters:** @DiscountID  
**C# Code:**
```csharp
// File: DatabaseAccess/DiscountRepository.cs - Line 129-154
public bool DeleteDiscount(int discountId)
```

### 📊 6. QUẢN LÝ GIAO DỊCH (2 procedures)

#### 6.1 GetTransactionsByUsername
**Chức năng:** Lấy giao dịch theo username  
**Parameters:** @Username  

#### 6.2 GetSalesByCustomerUsername
**Chức năng:** Lấy hóa đơn của khách hàng theo username  
**Parameters:** @Username  
**C# Code:**
```csharp
// File: DatabaseAccess/CustomerRepository.cs - Line 150-157
public DataTable GetSalesByCustomerUsername(string username)
```

---

## 👁️ VIEWS (10 views)

### 📊 1. ProductsWithDiscounts
**Chức năng:** Hiển thị sản phẩm kèm giá sau giảm  
**SQL:**
```sql
CREATE VIEW ProductsWithDiscounts AS
SELECT 
    p.ProductID, p.ProductName, p.Price as OriginalPrice,
    dbo.GetDiscountedPrice(p.ProductID, p.Price) as DiscountedPrice,
    CASE WHEN dbo.GetDiscountedPrice(p.ProductID, p.Price) < p.Price THEN 1 ELSE 0 END as HasDiscount,
    p.StockQuantity, p.Unit, d.DiscountType, d.DiscountValue,
    d.StartDate as DiscountStartDate, d.EndDate as DiscountEndDate
FROM dbo.Products p
LEFT JOIN dbo.Discounts d ON p.ProductID = d.ProductID 
    AND d.IsActive = 1 AND GETDATE() BETWEEN d.StartDate AND d.EndDate
```
**C# Code:**
```csharp
// File: DatabaseAccess/ProductRepository.cs - Line 216-226
public DataTable GetProductsWithDiscounts()
{
    return DatabaseConnection.ExecuteQuery("SELECT * FROM ProductsWithDiscounts", CommandType.Text, null);
}

// File: DatabaseAccess/DiscountRepository.cs - Line 49-59
public DataTable GetProductsWithDiscounts()
{
    return DatabaseConnection.ExecuteQuery("SELECT * FROM ProductsWithDiscounts", CommandType.Text, null);
}
```

### 📈 2. SalesSummary
**Chức năng:** Tóm tắt hóa đơn với thông tin khách hàng  
**C# Code:**
```csharp
// File: DatabaseAccess/SaleRepository.cs - Line 170-180
public DataTable GetSalesSummary()
{
    return DatabaseConnection.ExecuteQuery("SELECT * FROM SalesSummary ORDER BY SaleDate DESC", CommandType.Text, null);
}
```

### 📊 3. ProductSalesStats
**Chức năng:** Thống kê bán hàng theo sản phẩm  
**C# Code:**
```csharp
// File: DatabaseAccess/ProductRepository.cs - Line 229-239
public DataTable GetProductSalesStats()
{
    return DatabaseConnection.ExecuteQuery("SELECT * FROM ProductSalesStats", CommandType.Text, null);
}
```

### 👥 4. CustomerPurchaseSummary
**Chức năng:** Tóm tắt mua hàng của khách hàng  
**C# Code:**
```csharp
// File: DatabaseAccess/CustomerRepository.cs - Line 233-243
public DataTable GetCustomerPurchaseSummary()
{
    return DatabaseConnection.ExecuteQuery("SELECT * FROM CustomerPurchaseSummary", CommandType.Text, null);
}

// Lấy theo ID cụ thể
// File: DatabaseAccess/CustomerRepository.cs - Line 246-261
public DataTable GetCustomerPurchaseSummaryById(int customerId)
{
    string query = "SELECT * FROM CustomerPurchaseSummary WHERE CustomerID = @CustomerID";
    // ...
}
```

### 📅 5. MonthlySalesReport
**Chức năng:** Báo cáo bán hàng theo tháng  
**C# Code:**
```csharp
// File: DatabaseAccess/SaleRepository.cs - Line 196-206
public DataTable GetMonthlySalesReport()
{
    return DatabaseConnection.ExecuteQuery("SELECT * FROM MonthlySalesReport ORDER BY SalesYear DESC, SalesMonth DESC", CommandType.Text, null);
}

// Lấy theo tháng cụ thể
// File: DatabaseAccess/SaleRepository.cs - Line 227-243
public DataTable GetMonthlySalesReportByMonth(int year, int month)
```

### 📅 6. DailySalesReport
**Chức năng:** Báo cáo bán hàng theo ngày  
**C# Code:**
```csharp
// File: DatabaseAccess/SaleRepository.cs - Line 183-193
public DataTable GetDailySalesReport()
{
    return DatabaseConnection.ExecuteQuery("SELECT * FROM DailySalesReport ORDER BY SalesDate DESC", CommandType.Text, null);
}

// Lấy theo ngày cụ thể
// File: DatabaseAccess/SaleRepository.cs - Line 209-224
public DataTable GetDailySalesReportByDate(DateTime date)
```

### ⚠️ 7. LowStockProducts
**Chức năng:** Danh sách sản phẩm sắp hết hàng  
**C# Code:**
```csharp
// File: DatabaseAccess/ProductRepository.cs - Line 242-252
public DataTable GetLowStockProducts()
{
    return DatabaseConnection.ExecuteQuery("SELECT * FROM LowStockProducts", CommandType.Text, null);
}
```

### 🎯 8. ActiveDiscountsDetail
**Chức năng:** Chi tiết chương trình giảm giá đang hoạt động  
**C# Code:**
```csharp
// File: DatabaseAccess/ReportRepository.cs - Line 73-83
public DataTable GetActiveDiscountsDetail()
{
    return DatabaseConnection.ExecuteQuery("SELECT * FROM ActiveDiscountsDetail", CommandType.Text, null);
}
```

### 💼 9. TransactionSummary
**Chức năng:** Tóm tắt giao dịch với mô tả chi tiết  
**C# Code:**
```csharp
// File: DatabaseAccess/ReportRepository.cs - Line 86-96
public DataTable GetTransactionSummary()
{
    return DatabaseConnection.ExecuteQuery("SELECT * FROM TransactionSummary ORDER BY TransactionDate DESC", CommandType.Text, null);
}
```

### 👤 10. AccountSummary
**Chức năng:** Tóm tắt tài khoản với thống kê giao dịch  
**C# Code:**
```csharp
// File: DatabaseAccess/ReportRepository.cs - Line 99-109
public DataTable GetAccountSummary()
{
    return DatabaseConnection.ExecuteQuery("SELECT * FROM AccountSummary ORDER BY CreatedDate DESC", CommandType.Text, null);
}
```

---

## 🔧 FUNCTIONS (16 functions)

### 🔍 1. GetProductByName(@Name)
**Chức năng:** Tìm sản phẩm theo tên (Table-Valued Function)  
**Return:** Table với thông tin sản phẩm và giá giảm  
**C# Code:**
```csharp
// File: DatabaseAccess/ProductRepository.cs - Line 29-45
public DataTable GetProductByName(string productName)
{
    string query = "SELECT * FROM dbo.GetProductByName(@ProductName)";
    SqlParameter[] para = new SqlParameter[]
    {
        new SqlParameter("@ProductName", SqlDbType.NVarChar, 100) {Value = productName ?? (object)DBNull.Value}
    };
    return DatabaseConnection.ExecuteQuery(query, CommandType.Text, para);
}
```

### 🔍 2. GetProductByID(@id)
**Chức năng:** Lấy sản phẩm theo ID (Table-Valued Function)  
**Return:** Table với thông tin sản phẩm và giá giảm  
**C# Code:**
```csharp
// File: DatabaseAccess/ProductRepository.cs - Line 47-61
public DataTable GetProductById(int productId)
{
    string query = "SELECT * FROM dbo.GetProductByID(@ProductID)";
    SqlParameter[] para = new SqlParameter[]
    {
        new SqlParameter("@ProductID", SqlDbType.Int) {Value = productId}
    };
    return DatabaseConnection.ExecuteQuery(query, CommandType.Text, para);
}
```

### 💰 3. GetDiscountedPrice(@ProductID, @OriginalPrice)
**Chức năng:** Tính giá sau giảm (Scalar Function)  
**Return:** decimal - Giá sau khi giảm  
**SQL:**
```sql
CREATE FUNCTION GetDiscountedPrice(@ProductID int, @OriginalPrice decimal(18,2))
RETURNS decimal(18,2)
AS BEGIN
    DECLARE @DiscountedPrice decimal(18,2) = @OriginalPrice;
    DECLARE @DiscountType nvarchar(20), @DiscountValue decimal(18,2);
    
    -- Lấy giảm giá đang hoạt động
    SELECT TOP 1 @DiscountType = DiscountType, @DiscountValue = DiscountValue
    FROM dbo.Discounts
    WHERE ProductID = @ProductID AND IsActive = 1 
      AND GETDATE() BETWEEN StartDate AND EndDate
    ORDER BY CreatedDate DESC;
    
    IF @DiscountType IS NOT NULL
    BEGIN
        IF @DiscountType = 'percentage'
            SET @DiscountedPrice = @OriginalPrice * (100 - @DiscountValue) / 100;
        ELSE IF @DiscountType = 'fixed'
        BEGIN
            SET @DiscountedPrice = @OriginalPrice - @DiscountValue;
            IF @DiscountedPrice < 0 SET @DiscountedPrice = 0;
        END
    END
    
    RETURN @DiscountedPrice;
END
```
**C# Code:**
```csharp
// File: DatabaseAccess/DiscountRepository.cs - Line 156-179
public decimal GetDiscountedPrice(int productId, decimal originalPrice)
{
    SqlParameter[] parameters = new SqlParameter[]
    {
        new SqlParameter("@ProductID", SqlDbType.Int) { Value = productId },
        new SqlParameter("@OriginalPrice", SqlDbType.Decimal) { Value = originalPrice }
    };

    string query = "SELECT dbo.GetDiscountedPrice(@ProductID, @OriginalPrice) as DiscountedPrice";
    DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
    
    if (result.Rows.Count > 0)
        return Convert.ToDecimal(result.Rows[0]["DiscountedPrice"]);
    return originalPrice;
}
```

### 📊 4. GetDailyRevenue(@Date)
**Chức năng:** Tính doanh thu theo ngày (Scalar Function)  
**Return:** decimal - Tổng doanh thu trong ngày  
**C# Code:**
```csharp
// File: DatabaseAccess/ReportRepository.cs - Line 10-31
public decimal GetDailyRevenue(DateTime date)
{
    string query = "SELECT dbo.GetDailyRevenue(@Date) as Revenue";
    SqlParameter[] parameters = new SqlParameter[]
    {
        new SqlParameter("@Date", SqlDbType.Date) { Value = date.Date }
    };
    
    DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
    if (result.Rows.Count > 0)
        return Convert.ToDecimal(result.Rows[0]["Revenue"]);
    return 0;
}
```

### 📊 5. GetMonthlyRevenue(@Year, @Month)
**Chức năng:** Tính doanh thu theo tháng (Scalar Function)  
**Return:** decimal - Tổng doanh thu trong tháng  
**C# Code:**
```csharp
// File: DatabaseAccess/ReportRepository.cs - Line 34-56
public decimal GetMonthlyRevenue(int year, int month)
{
    string query = "SELECT dbo.GetMonthlyRevenue(@Year, @Month) as Revenue";
    SqlParameter[] parameters = new SqlParameter[]
    {
        new SqlParameter("@Year", SqlDbType.Int) { Value = year },
        new SqlParameter("@Month", SqlDbType.Int) { Value = month }
    };
    
    DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
    if (result.Rows.Count > 0)
        return Convert.ToDecimal(result.Rows[0]["Revenue"]);
    return 0;
}
```

### 🏆 6. GetTopSellingProducts(@TopCount)
**Chức năng:** Lấy top sản phẩm bán chạy (Table-Valued Function)  
**Return:** Table - Danh sách sản phẩm bán chạy nhất  
**C# Code:**
```csharp
// File: DatabaseAccess/ProductRepository.cs - Line 177-193
public DataTable GetTopSellingProducts(int topCount = 10)
{
    string query = "SELECT * FROM dbo.GetTopSellingProducts(@TopCount)";
    SqlParameter[] parameters = new SqlParameter[]
    {
        new SqlParameter("@TopCount", SqlDbType.Int) { Value = topCount }
    };
    
    return DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
}
```

### ✅ 7. IsStockAvailable(@ProductID, @RequiredQuantity)
**Chức năng:** Kiểm tra tồn kho có đủ không (Scalar Function)  
**Return:** bit - 1 nếu đủ hàng, 0 nếu không đủ  
**C# Code:**
```csharp
// File: DatabaseAccess/ProductRepository.cs - Line 152-174
public bool IsStockAvailable(int productId, int requiredQuantity)
{
    string query = "SELECT dbo.IsStockAvailable(@ProductID, @RequiredQuantity) as IsAvailable";
    SqlParameter[] parameters = new SqlParameter[]
    {
        new SqlParameter("@ProductID", SqlDbType.Int) { Value = productId },
        new SqlParameter("@RequiredQuantity", SqlDbType.Int) { Value = requiredQuantity }
    };

    DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
    if (result.Rows.Count > 0)
        return Convert.ToBoolean(result.Rows[0]["IsAvailable"]);
    return false;
}
```

### 🎯 8. CalculateLoyaltyPoints(@Amount)
**Chức năng:** Tính điểm tích lũy từ số tiền (Scalar Function)  
**Return:** int - Số điểm tích lũy (10,000 VND = 1 điểm)  
**C# Code:**
```csharp
// File: DatabaseAccess/ReportRepository.cs - Line 138-159
public int CalculateLoyaltyPoints(decimal amount)
{
    string query = "SELECT dbo.CalculateLoyaltyPoints(@Amount) as Points";
    SqlParameter[] parameters = new SqlParameter[]
    {
        new SqlParameter("@Amount", SqlDbType.Decimal) { Value = amount }
    };

    DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
    if (result.Rows.Count > 0)
        return Convert.ToInt32(result.Rows[0]["Points"]);
    return 0;
}
```

### 📋 9. GetCustomerPurchaseHistory(@CustomerID)
**Chức năng:** Lấy lịch sử mua hàng của khách hàng (Table-Valued Function)  
**Return:** Table - Chi tiết lịch sử mua hàng  
**C# Code:**
```csharp
// File: DatabaseAccess/CustomerRepository.cs - Line 215-230
public DataTable GetCustomerPurchaseHistory(int customerId)
{
    string query = "SELECT * FROM dbo.GetCustomerPurchaseHistory(@CustomerID)";
    SqlParameter[] parameters = new SqlParameter[]
    {
        new SqlParameter("@CustomerID", SqlDbType.Int) { Value = customerId }
    };
    return DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
}
```

### 🔍 10. SearchCustomers(@SearchTerm)
**Chức năng:** Tìm kiếm khách hàng đa điều kiện (Table-Valued Function)  
**Return:** Table - Danh sách khách hàng thỏa mãn điều kiện  
**C# Code:**
```csharp
// File: DatabaseAccess/CustomerRepository.cs - Line 197-212
public DataTable SearchCustomersAdvanced(string searchTerm)
{
    string query = "SELECT * FROM dbo.SearchCustomers(@SearchTerm)";
    SqlParameter[] parameters = new SqlParameter[]
    {
        new SqlParameter("@SearchTerm", SqlDbType.NVarChar, 100) { Value = searchTerm ?? (object)DBNull.Value }
    };
    return DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
}
```

### 📊 11. GetProductRevenueReport(@StartDate, @EndDate)
**Chức năng:** Báo cáo doanh thu theo sản phẩm trong khoảng thời gian (Table-Valued Function)  
**Return:** Table - Thống kê doanh thu từng sản phẩm  
**C# Code:**
```csharp
// File: DatabaseAccess/ProductRepository.cs - Line 196-213
public DataTable GetProductRevenueReport(DateTime startDate, DateTime endDate)
{
    string query = "SELECT * FROM dbo.GetProductRevenueReport(@StartDate, @EndDate)";
    SqlParameter[] parameters = new SqlParameter[]
    {
        new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = startDate },
        new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = endDate }
    };
    
    return DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
}
```

### 🧮 12. CalculateDiscountPercentage(@OriginalPrice, @DiscountedPrice)
**Chức năng:** Tính tỷ lệ giảm giá (Scalar Function)  
**Return:** decimal(5,2) - Phần trăm giảm giá  
**C# Code:**
```csharp
// File: DatabaseAccess/ReportRepository.cs - Line 162-184
public decimal CalculateDiscountPercentage(decimal originalPrice, decimal discountedPrice)
{
    string query = "SELECT dbo.CalculateDiscountPercentage(@OriginalPrice, @DiscountedPrice) as DiscountPercentage";
    SqlParameter[] parameters = new SqlParameter[]
    {
        new SqlParameter("@OriginalPrice", SqlDbType.Decimal) { Value = originalPrice },
        new SqlParameter("@DiscountedPrice", SqlDbType.Decimal) { Value = discountedPrice }
    };

    DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
    if (result.Rows.Count > 0)
        return Convert.ToDecimal(result.Rows[0]["DiscountPercentage"]);
    return 0;
}
```

### 📱 13. IsValidVietnamesePhone(@Phone)
**Chức năng:** Validate số điện thoại Việt Nam (Scalar Function)  
**Return:** bit - 1 nếu hợp lệ, 0 nếu không hợp lệ  
**C# Code:**
```csharp
// File: DatabaseAccess/ReportRepository.cs - Line 187-208
public bool IsValidVietnamesePhone(string phone)
{
    string query = "SELECT dbo.IsValidVietnamesePhone(@Phone) as IsValid";
    SqlParameter[] parameters = new SqlParameter[]
    {
        new SqlParameter("@Phone", SqlDbType.VarChar, 20) { Value = phone ?? (object)DBNull.Value }
    };

    DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
    if (result.Rows.Count > 0)
        return Convert.ToBoolean(result.Rows[0]["IsValid"]);
    return false;
}
```

### 📊 14. GetDashboardStats()
**Chức năng:** Lấy thống kê tổng quan hệ thống (Table-Valued Function)  
**Return:** Table - Số liệu tổng quan (sản phẩm, khách hàng, doanh thu...)  
**C# Code:**
```csharp
// File: DatabaseAccess/ReportRepository.cs - Line 59-70
public DataTable GetDashboardStats()
{
    string query = "SELECT * FROM dbo.GetDashboardStats()";
    return DatabaseConnection.ExecuteQuery(query, CommandType.Text, null);
}
```

### 💵 15. FormatVietnamMoney(@Amount)
**Chức năng:** Format tiền tệ Việt Nam (Scalar Function)  
**Return:** nvarchar(50) - Chuỗi định dạng tiền VN  
**C# Code:**
```csharp
// File: DatabaseAccess/ReportRepository.cs - Line 211-232
public string FormatVietnamMoney(decimal amount)
{
    string query = "SELECT dbo.FormatVietnamMoney(@Amount) as FormattedAmount";
    SqlParameter[] parameters = new SqlParameter[]
    {
        new SqlParameter("@Amount", SqlDbType.Decimal) { Value = amount }
    };

    DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
    if (result.Rows.Count > 0)
        return result.Rows[0]["FormattedAmount"].ToString();
    return amount.ToString("N0") + " ₫";
}
```

### 💰 16. GetExpenseByType(@TransactionType, @StartDate, @EndDate)
**Chức năng:** Tính tổng chi phí theo loại giao dịch (Scalar Function)  
**Return:** decimal(18,2) - Tổng chi phí  
**C# Code:**
```csharp
// File: DatabaseAccess/ReportRepository.cs - Line 112-135
public decimal GetExpenseByType(string transactionType, DateTime? startDate = null, DateTime? endDate = null)
{
    string query = "SELECT dbo.GetExpenseByType(@TransactionType, @StartDate, @EndDate) as TotalExpense";
    SqlParameter[] parameters = new SqlParameter[]
    {
        new SqlParameter("@TransactionType", SqlDbType.NVarChar, 20) { Value = transactionType },
        new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = startDate ?? (object)DBNull.Value },
        new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = endDate ?? (object)DBNull.Value }
    };

    DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
    if (result.Rows.Count > 0)
        return Convert.ToDecimal(result.Rows[0]["TotalExpense"]);
    return 0;
}
```

---

## ⚡ TRIGGERS (8 triggers)

### 🧮 1. TR_SaleDetails_UpdateTotalAmount
**Chức năng:** Tự động cập nhật tổng tiền hóa đơn khi thêm/sửa/xóa chi tiết  
**Bảng:** SaleDetails  
**Sự kiện:** AFTER INSERT, UPDATE, DELETE  
**SQL:**
```sql
CREATE TRIGGER TR_SaleDetails_UpdateTotalAmount
ON dbo.SaleDetails
AFTER INSERT, UPDATE, DELETE
AS BEGIN
    SET NOCOUNT ON;
    
    -- Tập hợp tất cả SaleID bị ảnh hưởng
    DECLARE @affected_sales TABLE (SaleID int);
    
    -- Lấy SaleID từ inserted và deleted
    IF EXISTS(SELECT * FROM inserted)
        INSERT INTO @affected_sales (SaleID) SELECT DISTINCT SaleID FROM inserted;
    
    IF EXISTS(SELECT * FROM deleted)
        INSERT INTO @affected_sales (SaleID) 
        SELECT DISTINCT SaleID FROM deleted WHERE SaleID NOT IN (SELECT SaleID FROM @affected_sales);
    
    -- Cập nhật tổng tiền
    UPDATE s SET TotalAmount = ISNULL((SELECT SUM(LineTotal) FROM dbo.SaleDetails sd WHERE sd.SaleID = s.SaleID), 0)
    FROM dbo.Sales s
    INNER JOIN @affected_sales affected ON s.SaleID = affected.SaleID;
END
```
**Mục đích:** Đảm bảo tổng tiền hóa đơn luôn chính xác sau mọi thay đổi chi tiết

### 📦 2. TR_SaleDetails_UpdateStock
**Chức năng:** Kiểm tra và cập nhật số lượng tồn kho khi bán hàng  
**Bảng:** SaleDetails  
**Sự kiện:** AFTER INSERT, UPDATE, DELETE  
**Logic:**
- INSERT: Kiểm tra tồn kho đủ không, sau đó trừ tồn kho
- UPDATE: Hoàn trả số lượng cũ, trừ số lượng mới
- DELETE: Hoàn trả số lượng về tồn kho
**Ràng buộc:** Không cho phép bán khi tồn kho không đủ

### 🧮 3. TR_SaleDetails_CalculateLineTotal
**Chức năng:** Tự động tính LineTotal = Quantity × SalePrice  
**Bảng:** SaleDetails  
**Sự kiện:** INSTEAD OF INSERT, UPDATE  
**Logic:** Thay thế việc INSERT/UPDATE thông thường để tự động tính LineTotal

### 🎯 4. TR_Sales_UpdateLoyaltyPoints
**Chức năng:** Tự động cộng điểm tích lũy cho khách hàng khi mua hàng  
**Bảng:** Sales  
**Sự kiện:** AFTER INSERT, UPDATE  
**Logic:** 
- Cộng điểm: 10,000 VND = 1 điểm
- Chỉ áp dụng cho khách hàng có CustomerID
- UPDATE: Trừ điểm cũ, cộng điểm mới

### 💰 5. TR_Sales_CreateTransaction
**Chức năng:** Tự động tạo giao dịch thu tiền khi có bán hàng  
**Bảng:** Sales  
**Sự kiện:** AFTER INSERT  
**Logic:** Tạo record trong bảng Transactions với TransactionType = 'income'

### ✅ 6. TR_Discounts_ValidateDiscount
**Chức năng:** Kiểm tra tính hợp lệ của chương trình giảm giá  
**Bảng:** Discounts  
**Sự kiện:** AFTER INSERT, UPDATE  
**Kiểm tra:**
- Giá trị giảm giá hợp lệ (percentage ≤ 100%, fixed ≥ 0)
- Thời gian kết thúc > thời gian bắt đầu
- Không trùng lặp thời gian giảm giá cho cùng sản phẩm

### 📱 7. TR_Customers_ValidatePhone
**Chức năng:** Kiểm tra và chuẩn hóa số điện thoại khách hàng  
**Bảng:** Customers  
**Sự kiện:** AFTER INSERT, UPDATE  
**Logic:**
- Validate định dạng SĐT Việt Nam (10-11 số, bắt đầu bằng 0)
- Chuẩn hóa: loại bỏ khoảng trắng, ký tự đặc biệt

### 📊 8. TR_Products_PriceHistory
**Chức năng:** Ghi log lịch sử thay đổi giá sản phẩm  
**Bảng:** Products  
**Sự kiện:** AFTER UPDATE  
**Logic:** Khi có thay đổi giá, ghi vào bảng ProductPriceHistory (OldPrice, NewPrice, ChangeDate, ChangedBy)

**Bảng ProductPriceHistory được tạo tự động:**
```sql
CREATE TABLE ProductPriceHistory (
    HistoryID int IDENTITY(1,1) PRIMARY KEY,
    ProductID int NOT NULL,
    OldPrice decimal(18,2),
    NewPrice decimal(18,2),
    ChangeDate datetime NOT NULL DEFAULT(GETDATE()),
    ChangedBy nvarchar(50),
    ChangeReason nvarchar(255)
);
```

---

## 🔗 MỐI LIÊN HỆ GIỮA SQL OBJECTS VÀ C# CODE

### 📁 Cấu trúc thư mục DatabaseAccess:
- **DatabaseConnection.cs** - Class kết nối cơ sở dữ liệu chung
- **ProductRepository.cs** - Quản lý sản phẩm (255 dòng)
- **CustomerRepository.cs** - Quản lý khách hàng (263 dòng)
- **SaleRepository.cs** - Quản lý bán hàng (245 dòng)
- **AccountRepository.cs** - Quản lý tài khoản (183 dòng)
- **DiscountRepository.cs** - Quản lý giảm giá (181 dòng)
- **ReportRepository.cs** - Báo cáo và thống kê (234 dòng)
- **SecurityHelper.cs** - Hỗ trợ bảo mật và phân quyền (324 dòng)
- **SecurityUsageGuide.cs** - Hướng dẫn sử dụng phân quyền (378 dòng)

### 🎯 Nguyên tắc thiết kế:
1. **Mỗi bảng có một Repository class** tương ứng
2. **Stored Procedures** được gọi qua `CommandType.StoredProcedure`
3. **Functions và Views** được gọi qua `CommandType.Text`
4. **Triggers** tự động chạy, không cần code C#
5. **Error handling** thống nhất với try-catch và thông báo lỗi tiếng Việt
6. **Parameter validation** đầy đủ với SqlParameter[]

### 📊 Thống kê sử dụng:
- **49 Stored Procedures** - 100% được sử dụng trong C#
- **10 Views** - 100% được sử dụng trong C#
- **16 Functions** - 100% được sử dụng trong C#
- **8 Triggers** - Tự động, không cần code C#
- **3 Database Roles** - Được sử dụng trong SecurityHelper.cs

---

---

## 🔐 DATABASE ROLES & SECURITY (3 roles)

### 🟠 1. MiniMart_Manager (Quản lý)
**Chức năng:** Quản lý toàn bộ hoạt động kinh doanh  
**Role trong Account:** 'manager'  
**Quyền hạn:**
- ✅ Toàn quyền trên tất cả bảng (SELECT, INSERT, UPDATE, DELETE)
- ✅ Thực thi tất cả 49 stored procedures
- ✅ Sử dụng tất cả 16 functions (9 scalar + 7 table-valued)
- ✅ Truy cập tất cả 10 views
- ✅ Quản lý tài khoản và phân quyền
- ✅ Truy cập báo cáo và thống kê chi tiết

**Login mặc định:** ManagerLogin / Manager@123!  
**User:** ManagerUser  
**Dữ liệu mẫu:** admin (role='manager')

### 🟡 2. MiniMart_Saler (Nhân viên bán hàng)
**Chức năng:** Thực hiện bán hàng và hỗ trợ khách hàng  
**Role trong Account:** 'saler'  
**Quyền hạn:**
- ✅ Quản lý sản phẩm (SELECT, INSERT, UPDATE - KHÔNG DELETE)
- ✅ Quản lý khách hàng (SELECT, INSERT, UPDATE - KHÔNG DELETE)
- ✅ Toàn quyền bán hàng (tất cả operations trên Sales, SaleDetails)
- ✅ Xem giảm giá (SELECT trên Discounts)
- ✅ Đổi mật khẩu cá nhân
- ✅ Truy cập báo cáo cơ bản
- ❌ KHÔNG có quyền DELETE sản phẩm, khách hàng
- ❌ KHÔNG có quyền quản lý tài khoản khác
- ❌ KHÔNG có quyền quản lý giảm giá

**Login mặc định:** SalerLogin / Saler@123!  
**User:** SalerUser  
**Dữ liệu mẫu:** saler001, saler002 (role='saler')

### 🟢 3. MiniMart_Customer (Khách hàng)
**Chức năng:** Xem thông tin cá nhân và sản phẩm  
**Role trong Account:** 'customer'  
**Quyền hạn:**
- ✅ Xem sản phẩm và giảm giá (SELECT trên Products, Discounts)
- ✅ Xem/cập nhật thông tin cá nhân (SELECT, UPDATE trên Customers, Account)
- ✅ Xem lịch sử mua hàng của mình
- ✅ Xem giao dịch của mình
- ✅ Đổi mật khẩu cá nhân
- ❌ KHÔNG có quyền xem thông tin khách hàng khác
- ❌ KHÔNG có quyền tạo/sửa hóa đơn
- ❌ KHÔNG có quyền truy cập báo cáo doanh thu

**Login mặc định:** CustomerLogin / Customer@123!  
**User:** CustomerUser  
**Dữ liệu mẫu:** customer001, customer002 (role='customer')

---

## 🔐 HỆ THỐNG PHÂN QUYỀN (ROLES & SECURITY)

### 📁 File: role.sql
Hệ thống phân quyền được thiết kế với 3 role chính tương ứng với các vai trò THỰC TẾ trong bảng Account:

#### 🟠 1. MiniMart_Manager (Quản lý)  
**Quyền hạn:** Quản lý toàn bộ hoạt động kinh doanh (role: 'manager')
- ✅ Toàn quyền quản lý sản phẩm (48 procedures)
- ✅ Toàn quyền quản lý khách hàng  
- ✅ Toàn quyền quản lý bán hàng
- ✅ Toàn quyền quản lý giảm giá
- ✅ Quản lý tài khoản (thêm/sửa/xóa)
- ✅ Truy cập tất cả báo cáo và thống kê
- ✅ Sử dụng tất cả 16 functions và 10 views
- ✅ DELETE quyền trên tất cả bảng

**Login mặc định:** ManagerLogin / Manager@123!  
**Dữ liệu mẫu:** admin (role='manager')

#### 🟡 2. MiniMart_Saler (Nhân viên bán hàng)
**Quyền hạn:** Thực hiện bán hàng và hỗ trợ khách hàng (role: 'saler')
- ✅ Quản lý sản phẩm (thêm/sửa, KHÔNG xóa)
- ✅ Quản lý khách hàng (thêm/sửa, KHÔNG xóa)  
- ✅ Toàn quyền tạo và xử lý hóa đơn bán hàng
- ✅ Xem thông tin giảm giá (KHÔNG thêm/sửa/xóa)
- ✅ Đổi mật khẩu cá nhân
- ✅ Truy cập các báo cáo cơ bản
- ❌ KHÔNG có quyền xóa sản phẩm, khách hàng
- ❌ KHÔNG có quyền quản lý tài khoản
- ❌ KHÔNG có quyền quản lý giảm giá

**Login mặc định:** SalerLogin / Saler@123!  
**Dữ liệu mẫu:** saler001, saler002 (role='saler')

#### 🟢 3. MiniMart_Customer (Khách hàng)
**Quyền hạn:** Xem thông tin cá nhân và sản phẩm
- ✅ Xem danh sách sản phẩm và giảm giá
- ✅ Xem/cập nhật thông tin cá nhân
- ✅ Xem lịch sử mua hàng của mình
- ✅ Xem giao dịch của mình
- ✅ Đổi mật khẩu cá nhân
- ❌ KHÔNG có quyền xem thông tin khách hàng khác
- ❌ KHÔNG có quyền tạo/sửa hóa đơn
- ❌ KHÔNG có quyền truy cập báo cáo doanh thu

**Login mặc định:** CustomerLogin / Customer@123!  
**Dữ liệu mẫu:** customer001, customer002 (role='customer')

### ⚡ Trigger tự động: trg_CreateSQLAccount
**Chức năng:** Tự động tạo Login và User SQL khi thêm record vào bảng Account
**Logic:**
1. Lấy username, password, role từ record mới
2. Tạo SQL Login với password (có kiểm tra tồn tại)
3. Tạo SQL User trong database (có kiểm tra tồn tại)
4. Gán User vào Role tương ứng theo role: 'manager' → MiniMart_Manager, 'saler' → MiniMart_Saler, 'customer' → MiniMart_Customer
5. Có xử lý lỗi TRY-CATCH để tránh conflict

### 🔧 Function kiểm tra: fn_CheckUserRole(@username, @password)
**Chức năng:** Xác thực đăng nhập và trả về role từ bảng Account
**Return:** Role thực tế ('manager', 'saler', 'customer') hoặc thông báo lỗi
**Logic:** Kiểm tra trong bảng Account, không dùng sys.database_role_members
**C# Usage:**
```csharp
// Có thể sử dụng trong DatabaseAccess/AccountRepository.cs
string query = "SELECT dbo.fn_CheckUserRole(@Username, @Password) as UserRole";
```

### 🛡️ Nguyên tắc bảo mật:
1. **Principle of Least Privilege** - Mỗi role chỉ có quyền tối thiểu cần thiết
2. **Role-based Access Control** - Phân quyền theo vai trò, không theo cá nhân
3. **Separation of Duties** - Tách biệt quyền quản trị và quyền thao tác
4. **Audit Trail** - Trigger ghi log thay đổi giá sản phẩm

### 📊 Ma trận phân quyền (theo cấu trúc thực tế):

| Chức năng | Manager | Saler | Customer |
|-----------|---------|-------|----------|
| Quản lý sản phẩm | ✅ Toàn quyền | 🟡 Thêm/Sửa | ❌ Chỉ xem |
| Quản lý khách hàng | ✅ Toàn quyền | 🟡 Thêm/Sửa | 🟡 Chỉ của mình |
| Quản lý bán hàng | ✅ Toàn quyền | ✅ Toàn quyền | 🟡 Xem của mình |
| Quản lý giảm giá | ✅ Toàn quyền | ❌ Chỉ xem | ❌ Chỉ xem |
| Quản lý tài khoản | ✅ Toàn quyền | 🟡 Đổi MK mình | 🟡 Đổi MK mình |
| Báo cáo doanh thu | ✅ Tất cả | 🟡 Cơ bản | ❌ Không |
| Xem thống kê | ✅ Tất cả | 🟡 Hạn chế | ❌ Không |

**Bảng thực tế:** Products, Customers, Sales, SaleDetails, Account, Transactions, Discounts  
**Role thực tế:** 'manager', 'saler', 'customer' (không có 'admin')

---

## 🚀 KẾT LUẬN

Hệ thống được thiết kế hoàn chỉnh với:
- ✅ **Tách biệt rõ ràng** giữa Database Logic (SQL) và Application Logic (C#)
- ✅ **Tái sử dụng cao** với Functions và Views
- ✅ **Tự động hóa** với Triggers đảm bảo tính toàn vẹn dữ liệu
- ✅ **Bảo mật tốt** với Stored Procedures chống SQL Injection
- ✅ **Hiệu suất cao** với các function chuyên biệt
- ✅ **Dễ bảo trì** với cấu trúc Repository pattern
- ✅ **Phân quyền chặt chẽ** với 3 role theo principle of least privilege
- ✅ **Tự động hóa bảo mật** với trigger tạo tài khoản SQL

Mọi SQL object đều có code C# tương ứng, đảm bảo hệ thống hoạt động đồng bộ, hiệu quả và bảo mật.

---

## 🛡️ SECURITYHELPER.CS - HỖ TRỢ BẢO MẬT

### 📋 Các method chính:

#### 🔐 Kiểm tra đăng nhập và phân quyền:
```csharp
// Kiểm tra đăng nhập và trả về role
string userRole = SecurityHelper.CheckUserRole(username, password);

// Kiểm tra quyền cụ thể
bool canManage = SecurityHelper.CanManageProducts(username);
bool canDelete = SecurityHelper.CanDeleteProducts(username);
bool canViewReports = SecurityHelper.CanViewRevenueReports(username);

// Kiểm tra role
bool isManager = SecurityHelper.IsManager(username);
bool isSaler = SecurityHelper.IsSaler(username);
bool isCustomer = SecurityHelper.IsCustomer(username);
```

#### 🔧 Validate và quản lý tài khoản:
```csharp
// Validate username/password
bool isValidUsername = SecurityHelper.ValidateUsername(username);
bool isValidPassword = SecurityHelper.ValidatePassword(password);

// Đổi mật khẩu
bool success = SecurityHelper.ChangePassword(username, oldPassword, newPassword);

// Lấy thông tin tài khoản
DataTable accountDetails = SecurityHelper.GetAccountDetails(username);
```

### 🎯 Cách sử dụng trong Forms:

#### 🔑 LoginForm:
```csharp
string userRole = SecurityHelper.CheckUserRole(txtUsername.Text, txtPassword.Text);
if (userRole == "manager")
{
    // Mở form Manager với đầy đủ quyền
    ManagerForm managerForm = new ManagerForm(txtUsername.Text);
    managerForm.Show();
}
else if (userRole == "saler")
{
    // Mở form Saler với quyền hạn chế
    SalerForm salerForm = new SalerForm(txtUsername.Text);
    salerForm.Show();
}
else if (userRole == "customer")
{
    // Mở form Customer với quyền tối thiểu
    CustomerForm customerForm = new CustomerForm(txtUsername.Text);
    customerForm.Show();
}
```

#### 🛡️ Các Form khác:
```csharp
// Kiểm tra quyền và ẩn/hiện controls
if (!SecurityHelper.CanDeleteProducts(currentUser))
{
    btnDeleteProduct.Enabled = false;
    btnDeleteProduct.Visible = false;
}

// Kiểm tra quyền trước khi thực hiện hành động
if (!SecurityHelper.CanManageProducts(currentUser))
{
    MessageBox.Show("Bạn không có quyền quản lý sản phẩm!");
    return;
}
```

---

## 📚 SECURITYUSAGEGUIDE.CS - HƯỚNG DẪN SỬ DỤNG

File này chứa các ví dụ chi tiết về cách sử dụng hệ thống phân quyền trong từng loại form:

### 📋 Nội dung chính:
- **ExampleLoginForm()** - Hướng dẫn xử lý đăng nhập
- **ExampleProductForm()** - Hướng dẫn phân quyền trong form sản phẩm
- **ExampleCustomerForm()** - Hướng dẫn phân quyền trong form khách hàng
- **ExampleDiscountForm()** - Hướng dẫn phân quyền trong form giảm giá
- **ExampleReportForm()** - Hướng dẫn phân quyền trong form báo cáo
- **ExampleAccountForm()** - Hướng dẫn phân quyền trong form tài khoản
- **ExampleUsingFunctions()** - Ví dụ sử dụng functions
- **ExampleUsingViews()** - Ví dụ sử dụng views
- **ExampleErrorHandling()** - Xử lý lỗi phân quyền

### 🎯 Mục đích:
- Cung cấp template code cho developers
- Đảm bảo tính nhất quán trong việc xử lý phân quyền
- Giảm thiểu lỗi bảo mật do thiếu kiểm tra quyền
