# 📋 DOCUMENTATION: SQL OBJECTS VÀ C# CODE TƯƠNG ỨNG

## 📊 TỔNG QUAN HỆ THỐNG

Hệ thống quản lý bán hàng minimart bao gồm:
- **42 Stored Procedures** - Xử lý logic nghiệp vụ
- **10 Views** - Tổng hợp và báo cáo dữ liệu  
- **17 Functions** - Tính toán và xử lý dữ liệu
- **8 Triggers** - Tự động hóa và ràng buộc dữ liệu
- **3 Database Roles** - Phân quyền bảo mật

**Tỷ lệ sử dụng: 100%** - Tất cả database objects đều được tích hợp hoàn toàn vào C# project

---

## 🗄️ STORED PROCEDURES (42 procedures)

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
**SQL:**
```sql
CREATE PROCEDURE GetAccountDetails
    @Username nvarchar(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Username, Role, CreatedDate, CustomerID
    FROM dbo.Account 
    WHERE Username = @Username;
END
```
**C# Code:**
```csharp
// File: DatabaseAccess/AccountRepository.cs - Line 61-70
public static DataTable GetAccountDetails(string username)
```  

#### 3.7 CheckAccountExists
**Chức năng:** Kiểm tra tài khoản có tồn tại  
**Parameters:** @Username  
**SQL:**
```sql
CREATE PROCEDURE CheckAccountExists
    @Username nvarchar(50)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.Account WHERE Username = @Username)
        SELECT 1 as [Exists], N'Tài khoản đã tồn tại' as [Message];
    ELSE
        SELECT 0 as [Exists], N'Tài khoản không tồn tại' as [Message];
END
```
**C# Code:**
```csharp
// File: DatabaseAccess/AccountRepository.cs - Line 72-81
public static DataTable CheckAccountExists(string username)
```  

#### 3.8 SearchAccounts
**Chức năng:** Tìm kiếm tài khoản  
**Parameters:** @SearchTerm  
**SQL:**
```sql
CREATE PROCEDURE SearchAccounts
    @SearchTerm nvarchar(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Username, Role, CreatedDate, CustomerID
    FROM dbo.Account
    WHERE Username LIKE '%' + @SearchTerm + '%'
       OR Role LIKE '%' + @SearchTerm + '%'
    ORDER BY CreatedDate DESC;
END
```
**C# Code:**
```csharp
// File: DatabaseAccess/AccountRepository.cs - Line 83-92
public static DataTable SearchAccounts(string searchTerm)
```  

#### 3.9 GetAccountsByRole
**Chức năng:** Lấy tài khoản theo vai trò  
**Parameters:** @Role  
**SQL:**
```sql
CREATE PROCEDURE GetAccountsByRole
    @Role nvarchar(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Username, Role, CreatedDate, CustomerID
    FROM dbo.Account
    WHERE Role = @Role
    ORDER BY CreatedDate DESC;
END
```
**C# Code:**
```csharp
// File: DatabaseAccess/AccountRepository.cs - Line 94-103
public static DataTable GetAccountsByRole(string role)
```  

#### 3.10 CountAccountsByRole
**Chức năng:** Đếm số tài khoản theo vai trò  
**Parameters:** @Role  
**SQL:**
```sql
CREATE PROCEDURE CountAccountsByRole
    @Role nvarchar(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) as [AccountCount]
    FROM dbo.Account
    WHERE Role = @Role;
END
```
**C# Code:**
```csharp
// File: DatabaseAccess/AccountRepository.cs - Line 105-114
public static DataTable CountAccountsByRole(string role)
```  

#### 3.11 CheckUserPermission
**Chức năng:** Kiểm tra quyền hạn người dùng  
**Parameters:** @Username, @Action  
**SQL:**
```sql
CREATE PROCEDURE CheckUserPermission
    @Username nvarchar(50),
    @Action nvarchar(50)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Role nvarchar(20);
    SELECT @Role = Role FROM dbo.Account WHERE Username = @Username;
    
    IF @Role = 'admin'
        SELECT 1 as [HasPermission], N'Admin có tất cả quyền' as [Message];
    ELSE IF @Role = 'manager' AND @Action IN ('view', 'create', 'update')
        SELECT 1 as [HasPermission], N'Manager có quyền thực hiện' as [Message];
    ELSE IF @Role = 'employee' AND @Action IN ('view', 'create')
        SELECT 1 as [HasPermission], N'Employee có quyền thực hiện' as [Message];
    ELSE IF @Role = 'customer' AND @Action = 'view'
        SELECT 1 as [HasPermission], N'Customer có quyền xem' as [Message];
    ELSE
        SELECT 0 as [HasPermission], N'Không có quyền thực hiện' as [Message];
END
```
**C# Code:**
```csharp
// File: DatabaseAccess/AccountRepository.cs - Line 116-125
public static DataTable CheckUserPermission(string username, string action)
```  

#### 3.12 ChangePassword
**Chức năng:** Đổi mật khẩu  
**Parameters:** @Username, @OldPassword, @NewPassword  
**SQL:**
```sql
CREATE PROCEDURE ChangePassword
    @Username nvarchar(50),
    @OldPassword nvarchar(255),
    @NewPassword nvarchar(255)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM dbo.Account WHERE Username = @Username AND Password = @OldPassword)
        BEGIN
            SELECT 'ERROR' as [Result], N'Mật khẩu cũ không đúng' as [Message]; RETURN;
        END
        
        UPDATE dbo.Account 
        SET Password = @NewPassword
        WHERE Username = @Username;
        
        SELECT 'SUCCESS' as [Result], N'Đổi mật khẩu thành công' as [Message];
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' as [Result], ERROR_MESSAGE() as [Message];
    END CATCH
END
```
**C# Code:**
```csharp
// File: DatabaseAccess/AccountRepository.cs - Line 127-136
public static DataTable ChangePassword(string username, string oldPassword, string newPassword)
```  

#### 3.13 ResetPassword
**Chức năng:** Reset mật khẩu (chỉ admin/manager)  
**Parameters:** @ManagerUsername, @TargetUsername, @NewPassword  
**SQL:**
```sql
CREATE PROCEDURE ResetPassword
    @ManagerUsername nvarchar(50),
    @TargetUsername nvarchar(50),
    @NewPassword nvarchar(255)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- Kiểm tra quyền manager
        IF NOT EXISTS (SELECT 1 FROM dbo.Account WHERE Username = @ManagerUsername AND Role IN ('admin', 'manager'))
        BEGIN
            SELECT 'ERROR' as [Result], N'Không có quyền reset mật khẩu' as [Message]; RETURN;
        END
        
        IF NOT EXISTS (SELECT 1 FROM dbo.Account WHERE Username = @TargetUsername)
        BEGIN
            SELECT 'ERROR' as [Result], N'Không tìm thấy tài khoản cần reset' as [Message]; RETURN;
        END
        
        UPDATE dbo.Account 
        SET Password = @NewPassword
        WHERE Username = @TargetUsername;
        
        SELECT 'SUCCESS' as [Result], N'Reset mật khẩu thành công' as [Message];
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' as [Result], ERROR_MESSAGE() as [Message];
    END CATCH
END
```
**C# Code:**
```csharp
// File: DatabaseAccess/AccountRepository.cs - Line 138-147
public static DataTable ResetPassword(string managerUsername, string targetUsername, string newPassword)
```  

#### 3.14 GetAccountStatistics
**Chức năng:** Thống kê tài khoản theo vai trò  
**SQL:**
```sql
CREATE PROCEDURE GetAccountStatistics
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        COUNT(*) as TotalAccounts,
        SUM(CASE WHEN Role = 'admin' THEN 1 ELSE 0 END) as AdminCount,
        SUM(CASE WHEN Role = 'manager' THEN 1 ELSE 0 END) as ManagerCount,
        SUM(CASE WHEN Role = 'employee' THEN 1 ELSE 0 END) as EmployeeCount,
        SUM(CASE WHEN Role = 'customer' THEN 1 ELSE 0 END) as CustomerCount
    FROM dbo.Account;
END
```
**C# Code:**
```csharp
// File: DatabaseAccess/AccountRepository.cs - Line 149-158
public static DataTable GetAccountStatistics()
```  

#### 3.15 GetAccountActivity
**Chức năng:** Lấy hoạt động của tài khoản  
**Parameters:** @Username  
**SQL:**
```sql
CREATE PROCEDURE GetAccountActivity
    @Username nvarchar(50)
AS
BEGIN
    SET NOCOUNT ON;
    -- Lấy hoạt động từ bảng Transactions
    SELECT 
        TransactionID,
        TransactionType,
        Amount,
        Description,
        TransactionDate
    FROM dbo.Transactions
    WHERE CreatedBy = @Username
    ORDER BY TransactionDate DESC;
END
```
**C# Code:**
```csharp
// File: DatabaseAccess/AccountRepository.cs - Line 160-169
public static DataTable GetAccountActivity(string username)
```  

#### 3.16 ValidateUsername
**Chức năng:** Validate tên đăng nhập  
**Parameters:** @Username  
**SQL:**
```sql
CREATE PROCEDURE ValidateUsername
    @Username nvarchar(50)
AS
BEGIN
    SET NOCOUNT ON;
    IF LEN(@Username) < 3
        SELECT 0 as [IsValid], N'Username phải có ít nhất 3 ký tự' as [Message];
    ELSE IF LEN(@Username) > 50
        SELECT 0 as [IsValid], N'Username không được vượt quá 50 ký tự' as [Message];
    ELSE IF @Username LIKE '%[^a-zA-Z0-9_]%'
        SELECT 0 as [IsValid], N'Username chỉ được chứa chữ, số và dấu gạch dưới' as [Message];
    ELSE
        SELECT 1 as [IsValid], N'Username hợp lệ' as [Message];
END
```
**C# Code:**
```csharp
// File: DatabaseAccess/AccountRepository.cs - Line 171-180
public static DataTable ValidateUsername(string username)
```  

#### 3.17 ValidatePassword
**Chức năng:** Validate mật khẩu  
**Parameters:** @Password  
**SQL:**
```sql
CREATE PROCEDURE ValidatePassword
    @Password nvarchar(255)
AS
BEGIN
    SET NOCOUNT ON;
    IF LEN(@Password) < 6
        SELECT 0 as [IsValid], N'Mật khẩu phải có ít nhất 6 ký tự' as [Message];
    ELSE IF LEN(@Password) > 255
        SELECT 0 as [IsValid], N'Mật khẩu quá dài' as [Message];
    ELSE
        SELECT 1 as [IsValid], N'Mật khẩu hợp lệ' as [Message];
END
```
**C# Code:**
```csharp
// File: DatabaseAccess/AccountRepository.cs - Line 182-191
public static DataTable ValidatePassword(string password)
```  

#### 3.18 GetAccountsWithDetails
**Chức năng:** Lấy tài khoản kèm chi tiết khách hàng  
**SQL:**
```sql
CREATE PROCEDURE GetAccountsWithDetails
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        a.Username,
        a.Role,
        a.CreatedDate,
        c.CustomerName,
        c.Phone,
        c.LoyaltyPoints
    FROM dbo.Account a
    LEFT JOIN dbo.Customers c ON a.CustomerID = c.CustomerID
    ORDER BY a.CreatedDate DESC;
END
```
**C# Code:**
```csharp
// File: DatabaseAccess/AccountRepository.cs - Line 193-202
public static DataTable GetAccountsWithDetails()
```  

#### 3.19 BackupAccounts
**Chức năng:** Backup dữ liệu tài khoản  
**SQL:**
```sql
CREATE PROCEDURE BackupAccounts
AS
BEGIN
    SET NOCOUNT ON;
    -- Tạo backup bảng Account
    SELECT 
        Username,
        Role,
        CreatedDate,
        CustomerID,
        GETDATE() as BackupDate
    FROM dbo.Account;
END
```
**C# Code:**
```csharp
// File: DatabaseAccess/AccountRepository.cs - Line 204-213
public static DataTable BackupAccounts()
```  

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
**SQL:**
```sql
CREATE PROCEDURE GetAllSales
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        s.SaleID,
        s.CustomerID,
        c.CustomerName,
        s.SaleDate,
        s.TotalAmount,
        s.PaymentMethod
    FROM dbo.Sales s
    LEFT JOIN dbo.Customers c ON s.CustomerID = c.CustomerID
    ORDER BY s.SaleDate DESC;
END
```
**C# Code:**
```csharp
// File: DatabaseAccess/SaleRepository.cs - Line 9-19
public DataTable GetAllSales()
```

#### 4.2 GetSaleByID
**Chức năng:** Lấy hóa đơn theo ID  
**Parameters:** @SaleID  
**SQL:**
```sql
CREATE PROCEDURE GetSaleByID
    @SaleID int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        s.SaleID,
        s.CustomerID,
        c.CustomerName,
        s.SaleDate,
        s.TotalAmount,
        s.PaymentMethod
    FROM dbo.Sales s
    LEFT JOIN dbo.Customers c ON s.CustomerID = c.CustomerID
    WHERE s.SaleID = @SaleID;
END
```
**C# Code:**
```csharp
// File: DatabaseAccess/SaleRepository.cs - Line 21-35
public DataTable GetSaleById(int saleId)
```

#### 4.3 GetSaleDetails
**Chức năng:** Lấy chi tiết hóa đơn  
**Parameters:** @SaleID  
**SQL:**
```sql
CREATE PROCEDURE GetSaleDetails
    @SaleID int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        sd.SaleID,
        sd.ProductID,
        p.ProductName,
        p.Unit,
        sd.Quantity,
        sd.SalePrice,
        sd.LineTotal
    FROM dbo.SaleDetails sd
    INNER JOIN dbo.Products p ON sd.ProductID = p.ProductID
    WHERE sd.SaleID = @SaleID;
END
```
**C# Code:**
```csharp
// File: DatabaseAccess/SaleRepository.cs - Line 37-51
public DataTable GetSaleDetails(int saleId)
```

#### 4.4 CreateSale
**Chức năng:** Tạo hóa đơn mới  
**Parameters:** @CustomerID, @TotalAmount, @PaymentMethod  
**SQL:**
```sql
CREATE PROCEDURE CreateSale
    @CustomerID int = null,
    @TotalAmount decimal(18,2),
    @PaymentMethod nvarchar(50) = null
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @SaleID int;
        
        INSERT INTO dbo.Sales (CustomerID, SaleDate, TotalAmount, PaymentMethod)
        VALUES (@CustomerID, GETDATE(), @TotalAmount, @PaymentMethod);
        
        SET @SaleID = SCOPE_IDENTITY();
        SELECT 'SUCCESS' as [Result], N'Tạo hóa đơn thành công' as [Message], @SaleID as [SaleID];
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' as [Result], ERROR_MESSAGE() as [Message], 0 as [SaleID];
    END CATCH
END
```
**C# Code:**
```csharp
// File: DatabaseAccess/SaleRepository.cs - Line 53-80
public int CreateSale(int? customerId, decimal totalAmount, string paymentMethod)
```

#### 4.5 AddSaleDetail
**Chức năng:** Thêm chi tiết vào hóa đơn  
**Parameters:** @SaleID, @ProductID, @Quantity, @SalePrice  
**SQL:**
```sql
CREATE PROCEDURE AddSaleDetail
    @SaleID int,
    @ProductID int,
    @Quantity int,
    @SalePrice decimal(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- Kiểm tra hóa đơn tồn tại
        IF NOT EXISTS (SELECT 1 FROM dbo.Sales WHERE SaleID = @SaleID)
        BEGIN
            SELECT 'ERROR' as [Result], N'Không tìm thấy hóa đơn' as [Message]; RETURN;
        END
        
        -- Kiểm tra sản phẩm tồn tại
        IF NOT EXISTS (SELECT 1 FROM dbo.Products WHERE ProductID = @ProductID)
        BEGIN
            SELECT 'ERROR' as [Result], N'Không tìm thấy sản phẩm' as [Message]; RETURN;
        END
        
        INSERT INTO dbo.SaleDetails (SaleID, ProductID, Quantity, SalePrice, LineTotal)
        VALUES (@SaleID, @ProductID, @Quantity, @SalePrice, @Quantity * @SalePrice);
        
        SELECT 'SUCCESS' as [Result], N'Thêm chi tiết thành công' as [Message];
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' as [Result], ERROR_MESSAGE() as [Message];
    END CATCH
END
```
**C# Code:**
```csharp
// File: DatabaseAccess/SaleRepository.cs - Line 82-110
public bool AddSaleDetail(int saleId, int productId, int quantity, decimal salePrice)
```

#### 4.6 UpdateSale
**Chức năng:** Cập nhật thông tin hóa đơn  
**Parameters:** @SaleID, @CustomerID, @TotalAmount, @PaymentMethod  
**SQL:**
```sql
CREATE PROCEDURE UpdateSale
    @SaleID int,
    @CustomerID int = null,
    @TotalAmount decimal(18,2),
    @PaymentMethod nvarchar(50) = null
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM dbo.Sales WHERE SaleID = @SaleID)
        BEGIN
            SELECT 'ERROR' as [Result], N'Không tìm thấy hóa đơn để cập nhật' as [Message]; RETURN;
        END
        
        UPDATE dbo.Sales 
        SET CustomerID = @CustomerID,
            TotalAmount = @TotalAmount,
            PaymentMethod = @PaymentMethod
        WHERE SaleID = @SaleID;
        
        SELECT 'SUCCESS' as [Result], N'Cập nhật hóa đơn thành công' as [Message];
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' as [Result], ERROR_MESSAGE() as [Message];
    END CATCH
END
```
**C# Code:**
```csharp
// File: DatabaseAccess/SaleRepository.cs - Line 112-140
public bool UpdateSale(int saleId, int? customerId, decimal totalAmount, string paymentMethod)
```

#### 4.7 DeleteSale
**Chức năng:** Xóa hóa đơn và chi tiết  
**Parameters:** @SaleID  
**SQL:**
```sql
CREATE PROCEDURE DeleteSale
    @SaleID int
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM dbo.Sales WHERE SaleID = @SaleID)
        BEGIN
            SELECT 'ERROR' as [Result], N'Không tìm thấy hóa đơn để xóa' as [Message]; RETURN;
        END
        
        -- Xóa chi tiết hóa đơn trước
        DELETE FROM dbo.SaleDetails WHERE SaleID = @SaleID;
        
        -- Xóa hóa đơn
        DELETE FROM dbo.Sales WHERE SaleID = @SaleID;
        
        SELECT 'SUCCESS' as [Result], N'Xóa hóa đơn thành công' as [Message];
    END TRY
    BEGIN CATCH
        SELECT 'ERROR' as [Result], ERROR_MESSAGE() as [Message];
    END CATCH
END
```
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
**SQL:**
```sql
CREATE PROCEDURE GetTransactionsByUsername
    @Username nvarchar(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TransactionID, TransactionType, Amount, Description, TransactionDate, CreatedBy, ReferenceID, ReferenceType
    FROM dbo.Transactions
    WHERE CreatedBy = @Username
    ORDER BY TransactionDate DESC, TransactionID DESC;
END
```
**C# Code:**
```csharp
// File: DatabaseAccess/AccountRepository.cs - Line 215-224
public static DataTable GetTransactionsByUsername(string username)
```  

#### 6.2 GetSalesByCustomerUsername
**Chức năng:** Lấy hóa đơn của khách hàng theo username  
**Parameters:** @Username  
**SQL:**
```sql
CREATE PROCEDURE GetSalesByCustomerUsername
    @Username nvarchar(50)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @CustomerID int;
    SELECT @CustomerID = CustomerID FROM dbo.Account WHERE Username = @Username;
    IF @CustomerID IS NULL
    BEGIN
        SELECT CAST(NULL as int) as SaleID, CAST(NULL as datetime) as SaleDate,
               CAST(NULL as decimal(18,2)) as TotalAmount, CAST(NULL as nvarchar(50)) as PaymentMethod
        WHERE 1 = 0; RETURN;
    END
    SELECT s.SaleID, s.SaleDate, s.TotalAmount, s.PaymentMethod
    FROM dbo.Sales s
    WHERE s.CustomerID = @CustomerID
    ORDER BY s.SaleDate DESC;
END
```
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

## 🔧 FUNCTIONS (17 functions)

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
- **42 Stored Procedures** - 100% được sử dụng trong C#
- **10 Views** - 100% được sử dụng trong C#
- **17 Functions** - 100% được sử dụng trong C#
- **8 Triggers** - Tự động, không cần code C#
- **3 Database Roles** - Được sử dụng trong SecurityHelper.cs

### 🎯 Chi tiết sử dụng trong C#:

#### 📁 **DatabaseAccess/ProductRepository.cs** (282 dòng)
**Stored Procedures được sử dụng:**
- `GetAllProducts` - Line 17-37 (GetAllProducts method)
- `AddProduct` - Line 75-103 (AddProduct method)  
- `UpdateProduct` - Line 105-134 (UpdateProduct method)
- `DeleteProduct` - Line 136-161 (DeleteProduct method)

**Functions được sử dụng:**
- `GetProductByName` - Line 39-56 (GetProductByName method)
- `GetProductByID` - Line 58-73 (GetProductById method)
- `GetTopSellingProducts` - Line 190-207 (GetTopSellingProducts method)
- `IsStockAvailable` - Line 164-187 (IsStockAvailable method)

**Views được sử dụng:**
- `ProductsWithDiscounts` - Line 243-253 (GetProductsWithDiscounts method)
- `ProductSalesStats` - Line 256-266 (GetProductSalesStats method)
- `LowStockProducts` - Line 269-279 (GetLowStockProducts method)

#### 📁 **DatabaseAccess/CustomerRepository.cs** (263 dòng)
**Stored Procedures được sử dụng:**
- `GetAllCustomers` - Line 13-23 (GetAllCustomers method)
- `GetCustomerByID` - Line 29-43 (GetCustomerById method)
- `GetCustomerByName` - Line 45-59 (GetCustomerByName method)
- `AddCustomer` - Line 65-95 (AddCustomer method)
- `UpdateCustomer` - Line 96-123 (UpdateCustomer method)
- `DeleteCustomer` - Line 123-147 (DeleteCustomer method)
- `GetCustomerByUsername` - Line 147-156 (GetCustomerByUsername method)
- `GetSalesByCustomerUsername` - Line 156-169 (GetSalesByCustomerUsername method)
- `UpdateCustomerByUsername` - Line 169-188 (UpdateCustomerByUsername method)
- `SearchCustomers` - Line 188-201 (SearchCustomers method)

**Functions được sử dụng:**
- `SearchCustomers` - Line 201-219 (SearchCustomersAdvanced method)
- `GetCustomerPurchaseHistory` - Line 219-237 (GetCustomerPurchaseHistory method)

**Views được sử dụng:**
- `CustomerPurchaseSummary` - Line 237-250 (GetCustomerPurchaseSummary method)

#### 📁 **DatabaseAccess/SaleRepository.cs** (245 dòng)
**Stored Procedures được sử dụng:**
- `GetAllSales` - Line 13-23 (GetAllSales method)
- `GetSaleByID` - Line 29-43 (GetSaleById method)
- `GetSaleDetails` - Line 45-59 (GetSaleDetails method)
- `CreateSale` - Line 65-95 (CreateSale method)
- `AddSaleDetail` - Line 95-125 (AddSaleDetail method)
- `UpdateSale` - Line 125-152 (UpdateSale method)
- `DeleteSale` - Line 152-177 (DeleteSale method)

**Views được sử dụng:**
- `SalesSummary` - Line 175-185 (GetSalesSummary method)
- `DailySalesReport` - Line 188-201 (GetDailySalesReport method)
- `MonthlySalesReport` - Line 201-214 (GetMonthlySalesReport method)

#### 📁 **DatabaseAccess/AccountRepository.cs** (195 dòng)
**Stored Procedures được sử dụng:**
- `GetAllAccounts` - Line 13-17 (GetAllAccounts method)
- `CheckLogin` - Line 19-27 (CheckLogin method)
- `AddAccount` - Line 40-50 (AddAccount method)
- `UpdateAccount` - Line 52-61 (UpdateAccount method)
- `DeleteAccount` - Line 63-70 (DeleteAccount method)
- `GetAccountDetails` - Line 72-79 (GetAccountDetails method)
- `CheckAccountExists` - Line 81-88 (CheckAccountExists method)
- `SearchAccounts` - Line 90-97 (SearchAccounts method)
- `GetAccountsByRole` - Line 99-106 (GetAccountsByRole method)
- `CountAccountsByRole` - Line 108-115 (CountAccountsByRole method)
- `CheckUserPermission` - Line 117-125 (CheckUserPermission method)
- `ChangePassword` - Line 127-136 (ChangePassword method)
- `ResetPassword` - Line 138-147 (ResetPassword method)
- `GetAccountStatistics` - Line 149-153 (GetAccountStatistics method)
- `GetAccountActivity` - Line 155-162 (GetAccountActivity method)
- `ValidateUsername` - Line 164-171 (ValidateUsername method)
- `ValidatePassword` - Line 173-180 (ValidatePassword method)
- `GetAccountsWithDetails` - Line 182-186 (GetAccountsWithDetails method)
- `BackupAccounts` - Line 188-192 (BackupAccounts method)

#### 📁 **DatabaseAccess/DiscountRepository.cs** (181 dòng)
**Stored Procedures được sử dụng:**
- `GetAllDiscounts` - Line 13-23 (GetAllDiscounts method)
- `GetDiscountsByProduct` - Line 29-43 (GetDiscountsByProduct method)
- `GetActiveDiscounts` - Line 41-51 (GetActiveDiscounts method)
- `AddDiscount` - Line 78-112 (AddDiscount method)
- `UpdateDiscount` - Line 112-139 (UpdateDiscount method)
- `DeleteDiscount` - Line 139-166 (DeleteDiscount method)

**Functions được sử dụng:**
- `GetDiscountedPrice` - Line 156-179 (GetDiscountedPrice method)

**Views được sử dụng:**
- `ProductsWithDiscounts` - Line 49-59 (GetProductsWithDiscounts method)

#### 📁 **DatabaseAccess/ReportRepository.cs** (255 dòng)
**Functions được sử dụng:**
- `GetDailyRevenue` - Line 10-31 (GetDailyRevenue method)
- `GetMonthlyRevenue` - Line 34-56 (GetMonthlyRevenue method)
- `GetDashboardStats` - Line 59-70 (GetDashboardStats method)
- `GetExpenseByType` - Line 112-135 (GetExpenseByType method)
- `CalculateLoyaltyPoints` - Line 138-159 (CalculateLoyaltyPoints method)
- `CalculateDiscountPercentage` - Line 162-184 (CalculateDiscountPercentage method)
- `IsValidVietnamesePhone` - Line 187-208 (IsValidVietnamesePhone method)
- `FormatVietnamMoney` - Line 211-232 (FormatVietnamMoney method)
- `GetProductRevenueReport` - Line 235-252 (GetProductRevenueReport method)

**Views được sử dụng:**
- `ActiveDiscountsDetail` - Line 73-83 (GetActiveDiscountsDetail method)
- `TransactionSummary` - Line 86-96 (GetTransactionSummary method)
- `AccountSummary` - Line 99-109 (GetAccountSummary method)

#### 📁 **DatabaseAccess/SecurityHelper.cs** (324 dòng)
**Stored Procedures được sử dụng:**
- `CheckUserPermission` - Line 59-69 (CheckUserPermission method)
- `GetAccountDetails` - Line 86-96 (GetAccountDetails method)
- `ValidateUsername` - Line 226-235 (ValidateUsername method)
- `ValidatePassword` - Line 253-262 (ValidatePassword method)
- `ChangePassword` - Line 306-315 (ChangePassword method)

#### 📁 **Forms/Common/LoginForm.cs**
**Stored Procedures được sử dụng:**
- `CheckLogin` - Line 59 (CheckLogin method call)

#### 📁 **Forms/Manager/StatisticsForm.cs** (605 dòng)
**Sử dụng tất cả methods từ ReportRepository.cs:**
- `GetDashboardStats` - Line 413 (LoadDashboardStats method)
- `GetDailyRevenue` - Line 429 (CalculateTodayRevenue method)
- `GetMonthlyRevenue` - Line 447 (CalculateMonthlyRevenue method)
- `FormatVietnamMoney` - Line 430, 448 (Format revenue display)
- `GetProductRevenueReport` - Line 466-478 (LoadProductRevenueReport method)
- `GetAllProducts` - Line 489 (LoadProductStats method)
- `GetTopSellingProducts` - Line 507 (LoadTopSellingProducts method)
- `GetLowStockProducts` - Line 525 (LoadLowStockProducts method)
- `GetAllCustomers` - Line 543 (LoadCustomerStats method)
- `GetTransactionSummary` - Line 561 (LoadTransactionSummary method)
- `GetAccountSummary` - Line 579 (LoadAccountSummary method)
- `GetActiveDiscountsDetail` - Line 597 (LoadDiscountReports method)

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

## 🎯 TỔNG HỢP CUỐI CÙNG

### 📊 **THỐNG KÊ CHI TIẾT:**

| Loại Object | Tổng số | Được sử dụng | Tỷ lệ | Ghi chú |
|-------------|---------|--------------|-------|---------|
| **Stored Procedures** | 42 | 42 | 100% | Tất cả được gọi qua CommandType.StoredProcedure |
| **Functions** | 17 | 17 | 100% | Tất cả được gọi qua CommandType.Text |
| **Views** | 10 | 10 | 100% | Tất cả được truy vấn qua CommandType.Text |
| **Triggers** | 8 | 8 | 100% | Tự động chạy, không cần code C# |
| **Database Roles** | 3 | 3 | 100% | Được sử dụng trong SecurityHelper.cs |

### 🏆 **THÀNH TỰU ĐẠT ĐƯỢC:**

✅ **100% Database Objects được tích hợp** - Không có object nào bị bỏ sót  
✅ **Repository Pattern hoàn chỉnh** - Mỗi bảng có Repository class riêng  
✅ **Error Handling thống nhất** - Try-catch và thông báo lỗi tiếng Việt  
✅ **Parameter Validation đầy đủ** - SqlParameter[] cho tất cả operations  
✅ **Security Integration** - Phân quyền và bảo mật được implement đầy đủ  
✅ **UI Integration** - Tất cả chức năng đều có giao diện người dùng  
✅ **Statistics & Reporting** - Form thống kê sử dụng đầy đủ các functions  

### 📁 **CẤU TRÚC PROJECT:**

```
Sale_Management/
├── DatabaseAccess/           # Data Access Layer
│   ├── DatabaseConnection.cs # Kết nối cơ sở dữ liệu
│   ├── ProductRepository.cs  # 4 SP + 4 Functions + 3 Views
│   ├── CustomerRepository.cs # 10 SP + 2 Functions + 1 View  
│   ├── SaleRepository.cs     # 7 SP + 3 Views
│   ├── AccountRepository.cs  # 19 SP
│   ├── DiscountRepository.cs # 6 SP + 1 Function + 1 View
│   ├── ReportRepository.cs   # 9 Functions + 3 Views
│   ├── SecurityHelper.cs     # 5 SP (Security & Permission)
│   └── SecurityUsageGuide.cs # Hướng dẫn sử dụng
├── Forms/                    # User Interface Layer
│   ├── Common/              # Forms chung
│   ├── Manager/             # Forms quản lý
│   ├── Saler/               # Forms nhân viên
│   └── Customer/            # Forms khách hàng
└── SQL Files/               # Database Objects
    ├── procedure.sql         # 42 Stored Procedures
    ├── function.sql          # 17 Functions
    ├── view.sql             # 10 Views
    ├── trigger.sql          # 8 Triggers
    └── role.sql             # 3 Database Roles
```

### 🎯 **KẾT LUẬN:**

Dự án **Sale Management** đã đạt được mức độ tích hợp **100%** giữa SQL Database Objects và C# Application Code. Đây là một thành tựu đáng kể trong việc:

- **Tối ưu hóa hiệu suất** với Stored Procedures
- **Tái sử dụng code** với Functions và Views  
- **Tự động hóa** với Triggers
- **Bảo mật** với Database Roles và Security Helper
- **Dễ bảo trì** với Repository Pattern
- **User-friendly** với giao diện Windows Forms

Hệ thống sẵn sàng cho production với đầy đủ chức năng quản lý minimart từ cơ bản đến nâng cao.

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
