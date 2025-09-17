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
    (1, 1, 2, 25000, 50000),   -- Gạo ST25
    (1, 2, 3, 12000, 36000),   -- Coca-Cola
    (2, 3, 1, 35000, 35000),   -- Sữa tươi
    (2, 4, 1, 120000, 120000), -- Bột giặt OMO
    (3, 6, 1, 60000, 60000),   -- Táo Mỹ
    (3, 7, 2, 55000, 110000);  -- Bánh quy Cosy
GO


use Minimart_SalesDB
select * from Products