use [Minimart_SalesDB]
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

-- Nhân viên bán hàng
create table Salers (
	SalerID int identity(1,1) primary key,
	SalerName nvarchar(100) not null,
	Phone nvarchar(20),
	Email nvarchar(100),
	Address nvarchar(200),
	HireDate datetime not null default(getdate()),
	Salary decimal(10,2),
	IsActive bit not null default(1)
);

-- Tài khoản người dùng
create table Account (
	Username nvarchar(50) primary key,
	Password nvarchar(255) not null,
	Role nvarchar(20) not null check(Role in ('manager', 'saler', 'customer')),
	CreatedDate datetime not null default(getdate()),
	CustomerID int null foreign key references Customers(CustomerID),
	SalerID int null foreign key references Salers(SalerID)
);

GO

-- Ensure columns and FKs exist for existing databases
IF COL_LENGTH('dbo.Account', 'CustomerID') IS NULL
BEGIN
    ALTER TABLE dbo.Account ADD CustomerID INT NULL;
END
GO

IF COL_LENGTH('dbo.Account', 'SalerID') IS NULL
BEGIN
    ALTER TABLE dbo.Account ADD SalerID INT NULL;
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Account_Customers_CustomerID'
)
BEGIN
    ALTER TABLE dbo.Account
    ADD CONSTRAINT FK_Account_Customers_CustomerID
    FOREIGN KEY (CustomerID) REFERENCES dbo.Customers(CustomerID);
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Account_Salers_SalerID'
)
BEGIN
    ALTER TABLE dbo.Account
    ADD CONSTRAINT FK_Account_Salers_SalerID
    FOREIGN KEY (SalerID) REFERENCES dbo.Salers(SalerID);
END
GO

-- Bảng giao dịch
create table Transactions (
	TransactionID int primary key identity(1, 1),
	TransactionType nvarchar(20) not null check(TransactionType in ('income', 'expense', 'transfer')),
	Amount decimal(18, 2) not null check(Amount > 0),
	Description nvarchar(255) null,
	TransactionDate datetime not null default(getdate()),
	CreatedBy nvarchar(50) not null foreign key references Account(Username),
	ReferenceID int null, -- ID tham chiếu đến SaleID hoặc các giao dịch khác
	ReferenceType nvarchar(20) null check(ReferenceType in ('sale', 'purchase', 'refund', 'other'))
);

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

-- Salers
INSERT INTO Salers (SalerName, Phone, Email, Address, HireDate, Salary, IsActive) VALUES
    (N'Nguyễn Thị Lan', '0987654321', 'lan.nguyen@minimart.com', N'234 Hoàng Hoa Thám, Hà Nội', '2024-01-15', 8000000, 1),
    (N'Trần Văn Nam', '0976543210', 'nam.tran@minimart.com', N'567 Điện Biên Phủ, TP.HCM', '2024-03-20', 7500000, 1);
GO

-- Sales
INSERT INTO Sales (CustomerID, SaleDate, PaymentMethod) VALUES
    (1, '2025-09-14 10:00:00', N'Tiền mặt'),
    (2, '2025-09-14 12:30:00', N'Thẻ'),
    (NULL, '2025-09-15 09:15:00', N'Tiền mặt');
GO

-- SaleDetails
INSERT INTO SaleDetails (SaleID, ProductID, Quantity, SalePrice, LineTotal) VALUES
    (1, 1, 2, 25000, 50000),   -- Gạo ST25
    (1, 2, 3, 12000, 36000),   -- Coca-Cola
    (2, 3, 1, 35000, 35000),   -- Sữa tươi
    (2, 4, 1, 120000, 120000), -- Bột giặt OMO
    (3, 6, 1, 60000, 60000),   -- Táo Mỹ
    (3, 7, 2, 55000, 110000);  -- Bánh quy Cosy
GO

-- Account sample data
INSERT INTO Account (Username, Password, Role, CustomerID) VALUES
    ('admin', 'admin123', 'manager', NULL),
    ('saler001', 'saler123', 'saler', NULL),
    ('saler002', 'saler456', 'saler', NULL),
    ('customer001', 'cust123', 'customer', 1),
    ('customer002', 'cust456', 'customer', 2);
GO

-- Update existing accounts to link with customers (if needed)
-- Uncomment and modify these lines if you want to link existing accounts to customers
/*
UPDATE Account SET CustomerID = 1 WHERE Username = 'customer001';
UPDATE Account SET CustomerID = 2 WHERE Username = 'customer002';
*/
GO

-- Transactions sample data
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

use Minimart_SalesDB
select * from Products