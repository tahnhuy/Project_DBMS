if db_id('Minimart_SalesDB') is null
begin
    create database Minimart_SalesDB;
end
go

use Minimart_SalesDB
go

-- Drop all stored procedures
declare @sql nvarchar(max) = N'';
select @sql = @sql + N'DROP PROCEDURE [' + s.name + N'].[' + p.name + N'];' + CHAR(10)
from sys.procedures p
inner join sys.schemas s on s.schema_id = p.schema_id;
if len(@sql) > 0 exec sp_executesql @sql;
go

-- Ensure Account.CustomerID FK exists (do not drop any tables)
if not exists (
    select 1 from sys.columns 
    where object_id = object_id('dbo.Account') and name = 'CustomerID'
)
begin
    alter table dbo.Account add CustomerID int null;
    alter table dbo.Account add constraint FK_Account_Customers foreign key (CustomerID) references dbo.Customers(CustomerID);
end
go

-- ========== Products ==========
create or alter procedure GetAllProducts 
as
begin
    select [ProductID], [ProductName], [Price], [StockQuantity], [Unit]
    from dbo.Products
    order by [ProductID]
end
go

create or alter procedure AddProduct
    @ProductName nvarchar(100),
    @Price decimal(18, 2),
    @StockQuantity int,
    @Unit nvarchar(50)
as
begin
    begin try
        insert into dbo.Products (ProductName, Price, StockQuantity, Unit)
        values (@ProductName, @Price, @StockQuantity, @Unit)
        select 'SUCCESS' as [Result], N'Thêm sản phẩm thành công' as [Message]
    end try
    begin catch
        select 'ERROR' as [Result], ERROR_MESSAGE() as [Message]
    end catch
end
go

create or alter procedure UpdateProduct
    @ProductID int,
    @ProductName nvarchar(100),
    @Price decimal(18, 2),
    @StockQuantity int,
    @Unit nvarchar(50)
as
begin
    begin try
        update dbo.Products 
        set ProductName = @ProductName,
            Price = @Price,
            StockQuantity = @StockQuantity,
            Unit = @Unit
        where ProductID = @ProductID

        if @@ROWCOUNT = 0
            select 'ERROR' as [Result], N'Không tìm thấy sản phẩm để cập nhật' as [Message]
        else
            select 'SUCCESS' as [Result], N'Cập nhật sản phẩm thành công' as [Message]
    end try
    begin catch
        select 'ERROR' as [Result], ERROR_MESSAGE() as [Message]
    end catch
end
go

create or alter procedure DeleteProduct
    @ProductID int
as
begin
    begin try
        if exists (select 1 from dbo.SaleDetails where ProductID = @ProductID)
        begin
            select 'ERROR' as [Result], N'Không thể xóa sản phẩm vì đã có trong hóa đơn' as [Message]
            return
        end
        delete from dbo.Products where ProductID = @ProductID

        if @@ROWCOUNT = 0
            select 'ERROR' as [Result], N'Không tìm thấy sản phẩm để xóa' as [Message]
        else
            select 'SUCCESS' as [Result], N'Xóa sản phẩm thành công' as [Message]
    end try
    begin catch
        select 'ERROR' as [Result], ERROR_MESSAGE() as [Message]
    end catch
end
go

-- ========== Customers ==========
create or alter procedure GetAllCustomers
as
begin
    select [CustomerID], [CustomerName], [Phone], [Address], [LoyaltyPoints]
    from dbo.Customers
    order by [CustomerID]
end
go

create or alter procedure GetCustomerByName
    @CustomerName nvarchar(100)
as
begin
    select *
    from dbo.Customers
    where CustomerName collate Vietnamese_CI_AI like N'%' + @CustomerName + '%'
    order by CustomerID
end
go

create or alter procedure GetCustomerByID
    @CustomerID int
as
begin
    select * from dbo.Customers where CustomerID = @CustomerID
end
go

create or alter procedure AddCustomer
    @CustomerName nvarchar(100),
    @Phone nvarchar(20) = null,
    @Address nvarchar(200) = null,
    @LoyaltyPoints int = 0
as
begin
    begin try
        insert into dbo.Customers (CustomerName, Phone, Address, LoyaltyPoints)
        values (@CustomerName, @Phone, @Address, @LoyaltyPoints)
        select 'SUCCESS' as [Result], N'Thêm khách hàng thành công' as [Message]
    end try
    begin catch
        select 'ERROR' as [Result], ERROR_MESSAGE() as [Message]
    end catch
end
go

create or alter procedure UpdateCustomer
    @CustomerID int,
    @CustomerName nvarchar(100),
    @Phone nvarchar(20) = null,
    @Address nvarchar(200) = null,
    @LoyaltyPoints int
as
begin
    begin try
        update dbo.Customers 
        set CustomerName = @CustomerName,
            Phone = @Phone,
            Address = @Address,
            LoyaltyPoints = @LoyaltyPoints
        where CustomerID = @CustomerID

        if @@ROWCOUNT = 0
            select 'ERROR' as [Result], N'Không tìm thấy khách hàng để cập nhật' as [Message]
        else
            select 'SUCCESS' as [Result], N'Cập nhật khách hàng thành công' as [Message]
    end try
    begin catch
        select 'ERROR' as [Result], ERROR_MESSAGE() as [Message]
    end catch
end
go

create or alter procedure DeleteCustomer
    @CustomerID int
as
begin
    begin try
        if exists (select 1 from dbo.Sales where CustomerID = @CustomerID)
        begin
            select 'ERROR' as [Result], N'Không thể xóa khách hàng vì đã phát sinh giao dịch' as [Message]
            return
        end
        delete from dbo.Customers where CustomerID = @CustomerID

        if @@ROWCOUNT = 0
            select 'ERROR' as [Result], N'Không tìm thấy khách hàng để xóa' as [Message]
        else
            select 'SUCCESS' as [Result], N'Xóa khách hàng thành công' as [Message]
    end try
    begin catch
        select 'ERROR' as [Result], ERROR_MESSAGE() as [Message]
    end catch
end
go

-- ========== Account & Auth ==========
create or alter procedure GetAllAccounts
as
begin
    select [Username], [Role], [CreatedDate]
    from dbo.Account
    order by [CreatedDate]
end
go

create or alter procedure CheckLogin
    @Username nvarchar(50),
    @Password nvarchar(255)
as
begin
    declare @Role nvarchar(20)
    select @Role = Role from dbo.Account where Username = @Username and Password = @Password
    if @Role is null
        select 'ERROR' as [Result], N'Tên đăng nhập hoặc mật khẩu không đúng' as [Message]
    else
        select 'SUCCESS' as [Result], N'Đăng nhập thành công' as [Message], @Username as Username, @Role as Role
end
go

-- ========== Customer-Account mapping for customer app ==========
create or alter procedure GetCustomerByUsername
    @Username nvarchar(50)
as
begin
    set nocount on;
    select top 1 c.CustomerID, c.CustomerName, c.Phone, c.Address, c.LoyaltyPoints
    from dbo.Account a
    left join dbo.Customers c on c.CustomerID = a.CustomerID
    where a.Username = @Username;
end
go

create or alter procedure LinkAccountToCustomer
    @Username nvarchar(50),
    @CustomerID int
as
begin
    set nocount on;
    if not exists (select 1 from dbo.Account where Username = @Username)
    begin
        select 'ERROR' as [Result], N'Không tìm thấy tài khoản' as [Message]; return;
    end
    if not exists (select 1 from dbo.Customers where CustomerID = @CustomerID)
    begin
        select 'ERROR' as [Result], N'Không tìm thấy khách hàng' as [Message]; return;
    end
    update dbo.Account set CustomerID = @CustomerID where Username = @Username;
    select 'SUCCESS' as [Result], N'Liên kết tài khoản với khách hàng thành công' as [Message];
end
go

create or alter procedure GetSalesByCustomerUsername
    @Username nvarchar(50)
as
begin
    set nocount on;
    declare @CustomerID int;
    select @CustomerID = CustomerID from dbo.Account where Username = @Username;
    if @CustomerID is null
    begin
        select cast(null as int) as SaleID, cast(null as datetime) as SaleDate,
               cast(null as decimal(18,2)) as TotalAmount, cast(null as nvarchar(50)) as PaymentMethod
        where 1 = 0; return;
    end
    select s.SaleID, s.SaleDate, s.TotalAmount, s.PaymentMethod
    from dbo.Sales s
    where s.CustomerID = @CustomerID
    order by s.SaleDate desc, s.SaleID desc;
end
go

-- ========== Transactions filter by creator username ==========
create or alter procedure GetTransactionsByUsername
    @Username nvarchar(50)
as
begin
    set nocount on;
    select TransactionID, TransactionType, Amount, Description, TransactionDate, CreatedBy, ReferenceID, ReferenceType
    from dbo.Transactions
    where CreatedBy = @Username
    order by TransactionDate desc, TransactionID desc;
end
go

create or alter procedure UpdateCustomerByUsername
    @Username nvarchar(50),
    @CustomerName nvarchar(100),
    @Phone nvarchar(20) = null,
    @Address nvarchar(200) = null
as
begin
    set nocount on;
    declare @CustomerID int;
    select @CustomerID = CustomerID from dbo.Account where Username = @Username;
    if @CustomerID is null
    begin
        select 'ERROR' as [Result], N'Tài khoản chưa liên kết với khách hàng' as [Message]; return;
    end
    update dbo.Customers
    set CustomerName = @CustomerName,
        Phone = @Phone,
        Address = @Address
    where CustomerID = @CustomerID;
    if @@ROWCOUNT = 0
        select 'ERROR' as [Result], N'Không cập nhật được thông tin khách hàng' as [Message];
    else
        select 'SUCCESS' as [Result], N'Cập nhật thông tin khách hàng thành công' as [Message];
end
go

-- ========== Discounts Management ==========
create or alter procedure GetAllDiscounts
as
begin
    set nocount on;
    select d.DiscountID, d.ProductID, p.ProductName, d.DiscountType, d.DiscountValue, 
           d.StartDate, d.EndDate, d.IsActive, d.CreatedDate, d.CreatedBy
    from dbo.Discounts d
    inner join dbo.Products p on p.ProductID = d.ProductID
    order by d.CreatedDate desc, d.DiscountID desc;
end
go

create or alter procedure GetDiscountsByProduct
    @ProductID int
as
begin
    set nocount on;
    select d.DiscountID, d.ProductID, p.ProductName, d.DiscountType, d.DiscountValue, 
           d.StartDate, d.EndDate, d.IsActive, d.CreatedDate, d.CreatedBy
    from dbo.Discounts d
    inner join dbo.Products p on p.ProductID = d.ProductID
    where d.ProductID = @ProductID
    order by d.CreatedDate desc;
end
go

create or alter procedure GetActiveDiscounts
as
begin
    set nocount on;
    select d.DiscountID, d.ProductID, p.ProductName, d.DiscountType, d.DiscountValue, 
           d.StartDate, d.EndDate, d.IsActive, d.CreatedDate, d.CreatedBy
    from dbo.Discounts d
    inner join dbo.Products p on p.ProductID = d.ProductID
    where d.IsActive = 1 
      and getdate() >= d.StartDate 
      and getdate() <= d.EndDate
    order by p.ProductName;
end
go

create or alter procedure AddDiscount
    @ProductID int,
    @DiscountType nvarchar(20),
    @DiscountValue decimal(18, 2),
    @StartDate datetime,
    @EndDate datetime,
    @IsActive bit = 1,
    @CreatedBy nvarchar(50)
as
begin
    set nocount on;
    begin try
        -- Validate product exists
        if not exists (select 1 from dbo.Products where ProductID = @ProductID)
        begin
            select 'ERROR' as [Result], N'Sản phẩm không tồn tại' as [Message]; return;
        end
        
        -- Validate date range
        if @EndDate <= @StartDate
        begin
            select 'ERROR' as [Result], N'Ngày kết thúc phải sau ngày bắt đầu' as [Message]; return;
        end
        
        -- Validate percentage discount
        if @DiscountType = 'percentage' and @DiscountValue > 100
        begin
            select 'ERROR' as [Result], N'Giảm giá phần trăm không được vượt quá 100%' as [Message]; return;
        end
        
        -- Check for overlapping active discounts for the same product
        if exists (
            select 1 from dbo.Discounts 
            where ProductID = @ProductID 
              and IsActive = 1
              and (
                  (@StartDate between StartDate and EndDate) or
                  (@EndDate between StartDate and EndDate) or
                  (StartDate between @StartDate and @EndDate) or
                  (EndDate between @StartDate and @EndDate)
              )
        )
        begin
            select 'ERROR' as [Result], N'Đã có chương trình giảm giá khác cho sản phẩm này trong khoảng thời gian này' as [Message]; return;
        end
        
        insert into dbo.Discounts (ProductID, DiscountType, DiscountValue, StartDate, EndDate, IsActive, CreatedBy)
        values (@ProductID, @DiscountType, @DiscountValue, @StartDate, @EndDate, @IsActive, @CreatedBy);
        
        select 'SUCCESS' as [Result], N'Thêm chương trình giảm giá thành công' as [Message];
    end try
    begin catch
        select 'ERROR' as [Result], ERROR_MESSAGE() as [Message];
    end catch
end
go

create or alter procedure UpdateDiscount
    @DiscountID int,
    @ProductID int,
    @DiscountType nvarchar(20),
    @DiscountValue decimal(18, 2),
    @StartDate datetime,
    @EndDate datetime,
    @IsActive bit
as
begin
    set nocount on;
    begin try
        -- Validate discount exists
        if not exists (select 1 from dbo.Discounts where DiscountID = @DiscountID)
        begin
            select 'ERROR' as [Result], N'Không tìm thấy chương trình giảm giá' as [Message]; return;
        end
        
        -- Validate product exists
        if not exists (select 1 from dbo.Products where ProductID = @ProductID)
        begin
            select 'ERROR' as [Result], N'Sản phẩm không tồn tại' as [Message]; return;
        end
        
        -- Validate date range
        if @EndDate <= @StartDate
        begin
            select 'ERROR' as [Result], N'Ngày kết thúc phải sau ngày bắt đầu' as [Message]; return;
        end
        
        -- Validate percentage discount
        if @DiscountType = 'percentage' and @DiscountValue > 100
        begin
            select 'ERROR' as [Result], N'Giảm giá phần trăm không được vượt quá 100%' as [Message]; return;
        end
        
        -- Check for overlapping active discounts for the same product (excluding current discount)
        if exists (
            select 1 from dbo.Discounts 
            where ProductID = @ProductID 
              and DiscountID != @DiscountID
              and IsActive = 1
              and (
                  (@StartDate between StartDate and EndDate) or
                  (@EndDate between StartDate and EndDate) or
                  (StartDate between @StartDate and @EndDate) or
                  (EndDate between @StartDate and @EndDate)
              )
        )
        begin
            select 'ERROR' as [Result], N'Đã có chương trình giảm giá khác cho sản phẩm này trong khoảng thời gian này' as [Message]; return;
        end
        
        update dbo.Discounts 
        set ProductID = @ProductID,
            DiscountType = @DiscountType,
            DiscountValue = @DiscountValue,
            StartDate = @StartDate,
            EndDate = @EndDate,
            IsActive = @IsActive
        where DiscountID = @DiscountID;
        
        if @@ROWCOUNT = 0
            select 'ERROR' as [Result], N'Không thể cập nhật chương trình giảm giá' as [Message];
        else
            select 'SUCCESS' as [Result], N'Cập nhật chương trình giảm giá thành công' as [Message];
    end try
    begin catch
        select 'ERROR' as [Result], ERROR_MESSAGE() as [Message];
    end catch
end
go

create or alter procedure DeleteDiscount
    @DiscountID int
as
begin
    set nocount on;
    begin try
        if not exists (select 1 from dbo.Discounts where DiscountID = @DiscountID)
        begin
            select 'ERROR' as [Result], N'Không tìm thấy chương trình giảm giá để xóa' as [Message]; return;
        end
        
        delete from dbo.Discounts where DiscountID = @DiscountID;
        
        select 'SUCCESS' as [Result], N'Xóa chương trình giảm giá thành công' as [Message];
    end try
    begin catch
        select 'ERROR' as [Result], ERROR_MESSAGE() as [Message];
    end catch
end
go

-- Function to calculate discounted price for a product
create or alter function GetDiscountedPrice(
    @ProductID int,
    @OriginalPrice decimal(18, 2)
)
returns decimal(18, 2)
as
begin
    declare @DiscountedPrice decimal(18, 2) = @OriginalPrice;
    declare @DiscountType nvarchar(20);
    declare @DiscountValue decimal(18, 2);
    
    -- Get active discount for the product
    select top 1 @DiscountType = DiscountType, @DiscountValue = DiscountValue
    from dbo.Discounts
    where ProductID = @ProductID
      and IsActive = 1
      and getdate() >= StartDate
      and getdate() <= EndDate
    order by CreatedDate desc; -- Get the most recent discount if multiple exist
    
    if @DiscountType is not null
    begin
        if @DiscountType = 'percentage'
        begin
            set @DiscountedPrice = @OriginalPrice * (100 - @DiscountValue) / 100;
        end
        else if @DiscountType = 'fixed'
        begin
            set @DiscountedPrice = @OriginalPrice - @DiscountValue;
            if @DiscountedPrice < 0 set @DiscountedPrice = 0;
        end
    end
    
    return @DiscountedPrice;
end
go

-- View to get products with their discounted prices
create or alter view ProductsWithDiscounts
as
select 
    p.ProductID,
    p.ProductName,
    p.Price as OriginalPrice,
    dbo.GetDiscountedPrice(p.ProductID, p.Price) as DiscountedPrice,
    case 
        when dbo.GetDiscountedPrice(p.ProductID, p.Price) < p.Price then 1 
        else 0 
    end as HasDiscount,
    p.StockQuantity,
    p.Unit,
    d.DiscountType,
    d.DiscountValue,
    d.StartDate as DiscountStartDate,
    d.EndDate as DiscountEndDate
from dbo.Products p
left join dbo.Discounts d on p.ProductID = d.ProductID 
    and d.IsActive = 1 
    and getdate() >= d.StartDate 
    and getdate() <= d.EndDate;
go

-- Stored procedure để tìm kiếm khách hàng
CREATE PROCEDURE SearchCustomers
    @SearchTerm NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        c.CustomerID,
        c.CustomerName,
        c.Phone,
        c.Address,
        c.LoyaltyPoints
    FROM Customers c
    WHERE c.CustomerName LIKE '%' + @SearchTerm + '%'
       OR c.Phone LIKE '%' + @SearchTerm + '%'
       OR c.Address LIKE '%' + @SearchTerm + '%'
    ORDER BY c.CustomerName;
END
go

-- ========== Account Management Procedures ==========
create or alter procedure AddAccount
    @CreatorUsername nvarchar(50),
    @NewUsername nvarchar(50),
    @NewPassword nvarchar(255),
    @NewRole nvarchar(20)
as
begin
    set nocount on;
    begin try
        -- Kiểm tra quyền của người tạo
        if not exists (select 1 from dbo.Account where Username = @CreatorUsername and Role in ('admin', 'manager'))
        begin
            select 'ERROR' as [Result], N'Không có quyền tạo tài khoản' as [Message]; return;
        end
        
        -- Kiểm tra username đã tồn tại chưa
        if exists (select 1 from dbo.Account where Username = @NewUsername)
        begin
            select 'ERROR' as [Result], N'Tên đăng nhập đã tồn tại' as [Message]; return;
        end
        
        insert into dbo.Account (Username, Password, Role, CreatedDate)
        values (@NewUsername, @NewPassword, @NewRole, getdate());
        
        select 'SUCCESS' as [Result], N'Tạo tài khoản thành công' as [Message];
    end try
    begin catch
        select 'ERROR' as [Result], ERROR_MESSAGE() as [Message];
    end catch
end
go

create or alter procedure UpdateAccount
    @Username nvarchar(50),
    @NewPassword nvarchar(255) = null,
    @NewRole nvarchar(20) = null
as
begin
    set nocount on;
    begin try
        if not exists (select 1 from dbo.Account where Username = @Username)
        begin
            select 'ERROR' as [Result], N'Không tìm thấy tài khoản' as [Message]; return;
        end
        
        update dbo.Account 
        set Password = isnull(@NewPassword, Password),
            Role = isnull(@NewRole, Role)
        where Username = @Username;
        
        select 'SUCCESS' as [Result], N'Cập nhật tài khoản thành công' as [Message];
    end try
    begin catch
        select 'ERROR' as [Result], ERROR_MESSAGE() as [Message];
    end catch
end
go

create or alter procedure DeleteAccount
    @Username nvarchar(50)
as
begin
    set nocount on;
    begin try
        if not exists (select 1 from dbo.Account where Username = @Username)
        begin
            select 'ERROR' as [Result], N'Không tìm thấy tài khoản để xóa' as [Message]; return;
        end
        
        delete from dbo.Account where Username = @Username;
        select 'SUCCESS' as [Result], N'Xóa tài khoản thành công' as [Message];
    end try
    begin catch
        select 'ERROR' as [Result], ERROR_MESSAGE() as [Message];
    end catch
end
go

create or alter procedure GetAccountDetails
    @Username nvarchar(50)
as
begin
    set nocount on;
    select Username, Role, CreatedDate, CustomerID
    from dbo.Account 
    where Username = @Username;
end
go

create or alter procedure CheckAccountExists
    @Username nvarchar(50)
as
begin
    set nocount on;
    if exists (select 1 from dbo.Account where Username = @Username)
        select 1 as [Exists], N'Tài khoản đã tồn tại' as [Message];
    else
        select 0 as [Exists], N'Tài khoản không tồn tại' as [Message];
end
go

create or alter procedure SearchAccounts
    @SearchTerm nvarchar(50)
as
begin
    set nocount on;
    select Username, Role, CreatedDate, CustomerID
    from dbo.Account
    where Username like '%' + @SearchTerm + '%'
       or Role like '%' + @SearchTerm + '%'
    order by CreatedDate desc;
end
go

create or alter procedure GetAccountsByRole
    @Role nvarchar(20)
as
begin
    set nocount on;
    select Username, Role, CreatedDate, CustomerID
    from dbo.Account
    where Role = @Role
    order by CreatedDate desc;
end
go

create or alter procedure CountAccountsByRole
    @Role nvarchar(20)
as
begin
    set nocount on;
    select count(*) as [AccountCount]
    from dbo.Account
    where Role = @Role;
end
go

create or alter procedure CheckUserPermission
    @Username nvarchar(50),
    @Action nvarchar(50)
as
begin
    set nocount on;
    declare @Role nvarchar(20);
    select @Role = Role from dbo.Account where Username = @Username;
    
    if @Role = 'admin'
        select 1 as [HasPermission], N'Admin có tất cả quyền' as [Message];
    else if @Role = 'manager' and @Action in ('view', 'create', 'update')
        select 1 as [HasPermission], N'Manager có quyền thực hiện' as [Message];
    else if @Role = 'employee' and @Action in ('view', 'create')
        select 1 as [HasPermission], N'Employee có quyền thực hiện' as [Message];
    else if @Role = 'customer' and @Action = 'view'
        select 1 as [HasPermission], N'Customer có quyền xem' as [Message];
    else
        select 0 as [HasPermission], N'Không có quyền thực hiện' as [Message];
end
go

create or alter procedure ChangePassword
    @Username nvarchar(50),
    @OldPassword nvarchar(255),
    @NewPassword nvarchar(255)
as
begin
    set nocount on;
    begin try
        if not exists (select 1 from dbo.Account where Username = @Username and Password = @OldPassword)
        begin
            select 'ERROR' as [Result], N'Mật khẩu cũ không đúng' as [Message]; return;
        end
        
        update dbo.Account 
        set Password = @NewPassword
        where Username = @Username;
        
        select 'SUCCESS' as [Result], N'Đổi mật khẩu thành công' as [Message];
    end try
    begin catch
        select 'ERROR' as [Result], ERROR_MESSAGE() as [Message];
    end catch
end
go

create or alter procedure ResetPassword
    @ManagerUsername nvarchar(50),
    @TargetUsername nvarchar(50),
    @NewPassword nvarchar(255)
as
begin
    set nocount on;
    begin try
        -- Kiểm tra quyền manager
        if not exists (select 1 from dbo.Account where Username = @ManagerUsername and Role in ('admin', 'manager'))
        begin
            select 'ERROR' as [Result], N'Không có quyền reset mật khẩu' as [Message]; return;
        end
        
        if not exists (select 1 from dbo.Account where Username = @TargetUsername)
        begin
            select 'ERROR' as [Result], N'Không tìm thấy tài khoản cần reset' as [Message]; return;
        end
        
        update dbo.Account 
        set Password = @NewPassword
        where Username = @TargetUsername;
        
        select 'SUCCESS' as [Result], N'Reset mật khẩu thành công' as [Message];
    end try
    begin catch
        select 'ERROR' as [Result], ERROR_MESSAGE() as [Message];
    end catch
end
go

create or alter procedure GetAccountStatistics
as
begin
    set nocount on;
    select 
        count(*) as TotalAccounts,
        sum(case when Role = 'admin' then 1 else 0 end) as AdminCount,
        sum(case when Role = 'manager' then 1 else 0 end) as ManagerCount,
        sum(case when Role = 'employee' then 1 else 0 end) as EmployeeCount,
        sum(case when Role = 'customer' then 1 else 0 end) as CustomerCount
    from dbo.Account;
end
go

create or alter procedure GetAccountActivity
    @Username nvarchar(50)
as
begin
    set nocount on;
    -- Lấy hoạt động từ bảng Transactions
    select 
        TransactionID,
        TransactionType,
        Amount,
        Description,
        TransactionDate
    from dbo.Transactions
    where CreatedBy = @Username
    order by TransactionDate desc;
end
go

create or alter procedure ValidateUsername
    @Username nvarchar(50)
as
begin
    set nocount on;
    if len(@Username) < 3
        select 0 as [IsValid], N'Username phải có ít nhất 3 ký tự' as [Message];
    else if len(@Username) > 50
        select 0 as [IsValid], N'Username không được vượt quá 50 ký tự' as [Message];
    else if @Username like '%[^a-zA-Z0-9_]%'
        select 0 as [IsValid], N'Username chỉ được chứa chữ, số và dấu gạch dưới' as [Message];
    else
        select 1 as [IsValid], N'Username hợp lệ' as [Message];
end
go

create or alter procedure ValidatePassword
    @Password nvarchar(255)
as
begin
    set nocount on;
    if len(@Password) < 6
        select 0 as [IsValid], N'Mật khẩu phải có ít nhất 6 ký tự' as [Message];
    else if len(@Password) > 255
        select 0 as [IsValid], N'Mật khẩu quá dài' as [Message];
    else
        select 1 as [IsValid], N'Mật khẩu hợp lệ' as [Message];
end
go

create or alter procedure GetAccountsWithDetails
as
begin
    set nocount on;
    select 
        a.Username,
        a.Role,
        a.CreatedDate,
        c.CustomerName,
        c.Phone,
        c.LoyaltyPoints
    from dbo.Account a
    left join dbo.Customers c on a.CustomerID = c.CustomerID
    order by a.CreatedDate desc;
end
go

create or alter procedure BackupAccounts
as
begin
    set nocount on;
    -- Tạo backup bảng Account
    select 
        Username,
        Role,
        CreatedDate,
        CustomerID,
        getdate() as BackupDate
    from dbo.Account;
end
go

-- ========== Account Info Procedures ==========
create or alter procedure GetAccountInfo
    @Username nvarchar(50)
as
begin
    set nocount on;
    select 
        a.Username,
        a.Role,
        a.CreatedDate,
        a.EmployeeID,
        a.CustomerID
    from dbo.Account a
    where a.Username = @Username;
end
go

create or alter procedure GetEmployeeInfo
    @EmployeeID int
as
begin
    set nocount on;
    select 
        EmployeeID,
        EmployeeName,
        Phone,
        Address,
        Position,
        HireDate,
        Salary
    from dbo.Employees
    where EmployeeID = @EmployeeID;
end
go

-- ========== Sales Management Procedures ==========
create or alter procedure GetAllSales
as
begin
    set nocount on;
    select 
        s.SaleID,
        s.CustomerID,
        c.CustomerName,
        s.SaleDate,
        s.TotalAmount,
        s.PaymentMethod
    from dbo.Sales s
    left join dbo.Customers c on s.CustomerID = c.CustomerID
    order by s.SaleDate desc, s.SaleID desc;
end
go

create or alter procedure GetSaleByID
    @SaleID int
as
begin
    set nocount on;
    select 
        s.SaleID,
        s.CustomerID,
        c.CustomerName,
        s.SaleDate,
        s.TotalAmount,
        s.PaymentMethod
    from dbo.Sales s
    left join dbo.Customers c on s.CustomerID = c.CustomerID
    where s.SaleID = @SaleID;
end
go

create or alter procedure GetSaleDetails
    @SaleID int
as
begin
    set nocount on;
    select 
        sd.SaleID,
        sd.ProductID,
        p.ProductName,
        p.Unit,
        sd.Quantity,
        sd.SalePrice,
        sd.LineTotal
    from dbo.SaleDetails sd
    inner join dbo.Products p on sd.ProductID = p.ProductID
    where sd.SaleID = @SaleID
    order by sd.ProductID;
end
go

create or alter procedure CreateSale
    @CustomerID int = null,
    @TotalAmount decimal(18,2),
    @PaymentMethod nvarchar(50) = null
as
begin
    set nocount on;
    begin try
        declare @SaleID int;
        
        insert into dbo.Sales (CustomerID, SaleDate, TotalAmount, PaymentMethod)
        values (@CustomerID, getdate(), @TotalAmount, @PaymentMethod);
        
        set @SaleID = SCOPE_IDENTITY();
        
        select 'SUCCESS' as [Result], N'Tạo hóa đơn thành công' as [Message], @SaleID as SaleID;
    end try
    begin catch
        select 'ERROR' as [Result], ERROR_MESSAGE() as [Message], 0 as SaleID;
    end catch
end
go

create or alter procedure AddSaleDetail
    @SaleID int,
    @ProductID int,
    @Quantity int,
    @SalePrice decimal(18,2)
as
begin
    set nocount on;
    begin try
        -- Kiểm tra hóa đơn tồn tại
        if not exists (select 1 from dbo.Sales where SaleID = @SaleID)
        begin
            select 'ERROR' as [Result], N'Không tìm thấy hóa đơn' as [Message]; return;
        end
        
        -- Kiểm tra sản phẩm tồn tại
        if not exists (select 1 from dbo.Products where ProductID = @ProductID)
        begin
            select 'ERROR' as [Result], N'Không tìm thấy sản phẩm' as [Message]; return;
        end
        
        insert into dbo.SaleDetails (SaleID, ProductID, Quantity, SalePrice, LineTotal)
        values (@SaleID, @ProductID, @Quantity, @SalePrice, @Quantity * @SalePrice);
        
        select 'SUCCESS' as [Result], N'Thêm chi tiết hóa đơn thành công' as [Message];
    end try
    begin catch
        select 'ERROR' as [Result], ERROR_MESSAGE() as [Message];
    end catch
end
go

create or alter procedure UpdateSale
    @SaleID int,
    @CustomerID int = null,
    @TotalAmount decimal(18,2),
    @PaymentMethod nvarchar(50) = null
as
begin
    set nocount on;
    begin try
        if not exists (select 1 from dbo.Sales where SaleID = @SaleID)
        begin
            select 'ERROR' as [Result], N'Không tìm thấy hóa đơn để cập nhật' as [Message]; return;
        end
        
        update dbo.Sales 
        set CustomerID = @CustomerID,
            TotalAmount = @TotalAmount,
            PaymentMethod = @PaymentMethod
        where SaleID = @SaleID;
        
        select 'SUCCESS' as [Result], N'Cập nhật hóa đơn thành công' as [Message];
    end try
    begin catch
        select 'ERROR' as [Result], ERROR_MESSAGE() as [Message];
    end catch
end
go

create or alter procedure DeleteSale
    @SaleID int
as
begin
    set nocount on;
    begin try
        if not exists (select 1 from dbo.Sales where SaleID = @SaleID)
        begin
            select 'ERROR' as [Result], N'Không tìm thấy hóa đơn để xóa' as [Message]; return;
        end
        
        -- Xóa chi tiết hóa đơn trước
        delete from dbo.SaleDetails where SaleID = @SaleID;
        
        -- Xóa hóa đơn
        delete from dbo.Sales where SaleID = @SaleID;
        
        select 'SUCCESS' as [Result], N'Xóa hóa đơn thành công' as [Message];
    end try
    begin catch
        select 'ERROR' as [Result], ERROR_MESSAGE() as [Message];
    end catch
end
go

print N'✅ Đã tạo thành công tất cả các stored procedure cần thiết!'
print N'📋 Tổng cộng đã tạo 48 stored procedures cho hệ thống quản lý bán hàng'
go

