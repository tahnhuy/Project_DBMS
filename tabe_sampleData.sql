-- Drop and recreate database
if db_id('Minimart_SalesDB') is not null
begin
    alter database Minimart_SalesDB set single_user with rollback immediate;
    drop database Minimart_SalesDB;
end
go

create database Minimart_SalesDB;
go

use [Minimart_SalesDB]
go

-- Drop tables in dependency order if they exist
if object_id('dbo.SaleDetails', 'U') is not null drop table dbo.SaleDetails;
if object_id('dbo.Transactions', 'U') is not null drop table dbo.Transactions;
if object_id('dbo.Sales', 'U') is not null drop table dbo.Sales;
if object_id('dbo.Account', 'U') is not null drop table dbo.Account;
if object_id('dbo.Products', 'U') is not null drop table dbo.Products;
if object_id('dbo.Customers', 'U') is not null drop table dbo.Customers;
go

-- San pham
create table Products (
	ProductID int primary key identity(1, 1),
	ProductName nvarchar(100) not null unique,
	Price decimal(18, 2) not null check (Price > 0),
	StockQuantity int not null check(StockQuantity >= 0),
	Unit nvarchar(50) not null
);
go

-- Khach hang
create table Customers (
	CustomerID int primary key identity(1, 1),
	CustomerName nvarchar(100) not null,
	Phone varchar(20) not null unique,
	Address nvarchar(255) null,
	LoyaltyPoints int not null default(0) check(LoyaltyPoints >= 0)
);
go

-- Hoa Don
create table Sales (
	SaleID int primary key identity(1, 1),
	CustomerID int null foreign key references Customers(CustomerID),
	SaleDate datetime not null default(getdate()),
	TotalAmount decimal(18, 2) not null default(0) check(TotalAmount >= 0),
	PaymentMethod nvarchar(50) null
);
go

-- Chi tiet hoa don
create table SaleDetails (
	SaleID int not null,
	ProductID int not null,
	Quantity int not null check(Quantity > 0),
	SalePrice decimal(18, 2) not null check(SalePrice > 0),
	LineTotal decimal(18, 2) not null check(LineTotal >= 0),
	primary key(SaleID, ProductID),
	constraint FK_SaleDetails_Sales foreign key(SaleID) references Sales(SaleID),
	constraint FK_SaleDetails_Products foreign key(ProductID) references Products(ProductID)
);
go

-- Tài khoản người dùng
create table Account (
	Username nvarchar(50) primary key,
	Password nvarchar(255) not null,
	Role nvarchar(20) not null check(Role in ('manager', 'saler', 'customer')),
	CreatedDate datetime not null default(getdate()),
	CustomerID int null
);
go

-- Ensure FK for Account -> Customers
alter table dbo.Account
add constraint FK_Account_Customers_CustomerID foreign key (CustomerID) references dbo.Customers(CustomerID);
go

-- Bảng giao dịch
create table Transactions (
	TransactionID int primary key identity(1, 1),
	TransactionType nvarchar(20) not null check(TransactionType in ('income', 'expense', 'transfer')),
	Amount decimal(18, 2) not null check(Amount > 0),
	Description nvarchar(255) null,
	TransactionDate datetime not null default(getdate()),
	CreatedBy nvarchar(50) not null,
	ReferenceID int null, -- ID tham chiếu đến SaleID hoặc các giao dịch khác
	ReferenceType nvarchar(20) null check(ReferenceType in ('sale', 'purchase', 'refund', 'other'))
);
go


-- Ensure FK for Transactions -> Account
alter table dbo.Transactions
add constraint FK_Transactions_Account_CreatedBy foreign key (CreatedBy) references dbo.Account(Username);
go

-- Bảng giảm giá sản phẩm
create table Discounts (
	DiscountID int primary key identity(1, 1),
	ProductID int not null,
	DiscountType nvarchar(20) not null check(DiscountType in ('percentage', 'fixed')),
	DiscountValue decimal(18, 2) not null check(DiscountValue > 0),
	StartDate datetime not null,
	EndDate datetime not null,
	IsActive bit not null default(1),
	CreatedDate datetime not null default(getdate()),
	CreatedBy nvarchar(50) not null,
	constraint FK_Discounts_Products foreign key (ProductID) references Products(ProductID) on delete cascade,
	constraint FK_Discounts_Account_CreatedBy foreign key (CreatedBy) references Account(Username),
	constraint CK_Discount_DateRange check (EndDate > StartDate),
	constraint CK_Discount_Percentage check (DiscountType != 'percentage' or DiscountValue <= 100)
);
go

-- Sample data
INSERT INTO Products (ProductName, Price, StockQuantity, Unit) VALUES
	(N'Gạo ST25', 25000, 100, N'kg'),
	(N'Nước ngọt Coca-Cola 500ml', 12000, 200, N'chai'),
	(N'Sữa tươi Vinamilk 1L', 35000, 150, N'hộp'),
	(N'Bột giặt OMO 3kg', 120000, 50, N'gói'),
	(N'Nước rửa chén Sunlight 1L', 45000, 80, N'chai'),
	(N'Trái cây - Táo Mỹ', 60000, 60, N'kg'),
	(N'Bánh quy Cosy 500g', 55000, 90, N'gói'),
	(N'Kem đánh răng Colgate 180g', 32000, 120, N'tuýp'),
	(N'Dầu gội Sunsilk 650ml', 85000, 70, N'chai'),
	(N'Thịt heo thăn', 150000, 40, N'kg');
GO

-- Customers
INSERT INTO Customers (CustomerName, Phone, Address, LoyaltyPoints) VALUES
	(N'Nguyễn Văn An', '0901234567', N'123 Đường Láng, Hà Nội', 10),
	(N'Trần Thị Bình', '0912345678', N'456 Nguyễn Trãi, TP.HCM', 5),
	(N'Lê Minh Châu', '0923456789', NULL, 0),
	(N'Phạm Quốc Dũng', '0934567890', N'789 Lê Lợi, Đà Nẵng', 15),
	(N'Hoàng Thị Mai', '0945678901', N'101 Trần Phú, Nha Trang', 8);
GO

-- Sales
INSERT INTO Sales (CustomerID, SaleDate, PaymentMethod) VALUES
	(1, '2025-09-14 10:00:00', N'Tiền mặt'),
	(2, '2025-09-14 12:30:00', N'Thẻ'),
	(NULL, '2025-09-15 09:15:00', N'Tiền mặt');
GO

-- SaleDetails
INSERT INTO SaleDetails (SaleID, ProductID, Quantity, SalePrice, LineTotal) VALUES
	(1, 1, 2, 25000, 50000),
	(1, 2, 3, 12000, 36000),
	(2, 3, 1, 35000, 35000),
	(2, 4, 1, 120000, 120000),
	(3, 6, 1, 60000, 60000),
	(3, 7, 2, 55000, 110000);
GO

-- Accounts
INSERT INTO Account (Username, Password, Role, CustomerID) VALUES
	('admin', 'admin123', 'manager', NULL),
	('saler001', 'saler123', 'saler', NULL),
	('saler002', 'saler456', 'saler', NULL),
	('customer001', 'cust123', 'customer', 1),
	('customer002', 'cust456', 'customer', 2);
GO

-- Transactions
INSERT INTO Transactions (TransactionType, Amount, Description, TransactionDate, CreatedBy, ReferenceID, ReferenceType) VALUES
	('income', 86000, N'Thu tiền từ bán hàng - Hóa đơn #1', '2025-09-14 10:00:00', 'saler001', 1, 'sale'),
	('income', 155000, N'Thu tiền từ bán hàng - Hóa đơn #2', '2025-09-14 12:30:00', 'saler001', 2, 'sale'),
	('income', 170000, N'Thu tiền từ bán hàng - Hóa đơn #3', '2025-09-15 09:15:00', 'saler002', 3, 'sale'),
	('expense', 500000, N'Chi phí nhập hàng - Gạo ST25', '2025-09-13 08:00:00', 'admin', NULL, 'purchase'),
	('expense', 240000, N'Chi phí nhập hàng - Coca-Cola', '2025-09-13 08:30:00', 'admin', NULL, 'purchase'),
	('expense', 525000, N'Chi phí nhập hàng - Sữa Vinamilk', '2025-09-13 09:00:00', 'admin', NULL, 'purchase'),
	('expense', 6000000, N'Chi phí nhập hàng - Bột giặt OMO', '2025-09-13 09:30:00', 'admin', NULL, 'purchase'),
	('expense', 360000, N'Chi phí nhập hàng - Nước rửa chén', '2025-09-13 10:00:00', 'admin', NULL, 'purchase'),
	('expense', 360000, N'Chi phí nhập hàng - Táo Mỹ', '2025-09-13 10:30:00', 'admin', NULL, 'purchase'),
	('expense', 495000, N'Chi phí nhập hàng - Bánh quy Cosy', '2025-09-13 11:00:00', 'admin', NULL, 'purchase'),
	('expense', 384000, N'Chi phí nhập hàng - Kem đánh răng', '2025-09-13 11:30:00', 'admin', NULL, 'purchase'),
	('expense', 595000, N'Chi phí nhập hàng - Dầu gội Sunsilk', '2025-09-13 12:00:00', 'admin', NULL, 'purchase'),
	('expense', 600000, N'Chi phí nhập hàng - Thịt heo thăn', '2025-09-13 12:30:00', 'admin', NULL, 'purchase'),
	('expense', 2000000, N'Chi phí thuê mặt bằng tháng 9', '2025-09-01 00:00:00', 'admin', NULL, 'other'),
	('expense', 15000000, N'Chi phí lương nhân viên tháng 9', '2025-09-01 00:00:00', 'admin', NULL, 'other'),
	('expense', 500000, N'Chi phí điện nước tháng 9', '2025-09-15 00:00:00', 'admin', NULL, 'other');
GO

-- Discounts sample data
INSERT INTO Discounts (ProductID, DiscountType, DiscountValue, StartDate, EndDate, IsActive, CreatedBy) VALUES
	(1, 'percentage', 10, '2025-09-01', '2025-09-30', 1, 'admin'), -- Gạo ST25 giảm 10%
	(2, 'fixed', 2000, '2025-09-15', '2025-10-15', 1, 'admin'), -- Coca-Cola giảm 2000 VND
	(6, 'percentage', 15, '2025-09-10', '2025-09-25', 1, 'admin'), -- Táo Mỹ giảm 15%
	(7, 'percentage', 20, '2025-08-01', '2025-08-31', 0, 'admin'), -- Bánh quy đã hết hạn
	(9, 'fixed', 5000, '2025-09-20', '2025-10-20', 1, 'admin'); -- Dầu gội giảm 5000 VND
GO


use [Minimart_SalesDB]
go

-- Drop Employees table if it exists (for re-running)
if object_id('dbo.Employees', 'U') is not null drop table dbo.Employees;
go

-- Create Employees table
create table Employees (
    EmployeeID int primary key identity(1, 1),
    EmployeeName nvarchar(100) not null,
    Phone varchar(20) not null unique,
    Address nvarchar(255) null,
    Position nvarchar(20) not null check(Position in ('manager', 'saler')),  -- Position: manager or saler
    HireDate datetime not null default(getdate()),  -- Hire date
    Salary decimal(18, 2) null check(Salary >= 0)  -- Salary (optional)
);
go

-- Insert sample data for Employees
INSERT INTO Employees (EmployeeName, Phone, Address, Position, HireDate, Salary) VALUES
(N'Admin Chính', '0987654321', N'1 Đường ABC, Hà Nội', 'manager', '2025-01-01', 20000000),
(N'Nhân viên bán hàng 1', '0987654322', N'2 Đường DEF, TP.HCM', 'saler', '2025-02-01', 10000000),
(N'Nhân viên bán hàng 2', '0987654323', N'3 Đường GHI, Đà Nẵng', 'saler', '2025-03-01', 10000000);
GO

-- Add EmployeeID column to Account (if not already added)
if not exists (select * from sys.columns where object_id = object_id('dbo.Account') and name = 'EmployeeID')
begin
    alter table dbo.Account
    add EmployeeID int null;
end
go

-- Add foreign key from Account to Employees (if not already added)
if not exists (select * from sys.foreign_keys where name = 'FK_Account_Employees_EmployeeID')
begin
    alter table dbo.Account
    add constraint FK_Account_Employees_EmployeeID foreign key (EmployeeID) references dbo.Employees(EmployeeID);
end
go

-- Update existing Account rows with EmployeeID (links to the new Employees)
UPDATE Account SET EmployeeID = 1 WHERE Username = 'admin';  -- Link to manager
UPDATE Account SET EmployeeID = 2 WHERE Username = 'saler001';  -- Link to saler
UPDATE Account SET EmployeeID = 3 WHERE Username = 'saler002';  -- Link to saler
GO

-- Now add the CHECK constraint (after updates, so no violation)
if not exists (select * from sys.check_constraints where name = 'CK_Account_Role_ID')
begin
    alter table dbo.Account
    add constraint CK_Account_Role_ID check (
        (Role in ('manager', 'saler') and EmployeeID is not null and CustomerID is null) or
        (Role = 'customer' and CustomerID is not null and EmployeeID is null)
    );
end
go

-- Verification queries
select count(*) as EmployeesCount from Employees;
select * from Employees;  -- View employee info
select * from Account;  -- View accounts after linking (check EmployeeID column)

use Minimart_SalesDB
select count(*) as ProductsCount from Products;
select count(*) as CustomersCount from Customers;
select count(*) as SalesCount from Sales;
select count(*) as SaleDetailsCount from SaleDetails;
select count(*) as AccountsCount from Account;
select count(*) as TransactionsCount from Transactions;
select count(*) as DiscountsCount from Discounts;

