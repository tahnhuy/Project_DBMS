-- =============================================================
-- Database: Minimart_SalesDB
-- Purpose : Clean DDL to create tables and constraints only
-- Source  : Refactored from tabe_sampleData.sql (no sample data)
-- Note    : Safe (non-destructive) DB create. Drops existing tables
--           in dependency order, then recreates all objects.
-- =============================================================

-- Create database if not exists and switch context
IF DB_ID('Minimart_SalesDB') IS NULL
BEGIN
	CREATE DATABASE Minimart_SalesDB;
END
GO

USE Minimart_SalesDB;
GO

-- =============================
-- Drop existing tables (if any)
-- =============================
IF OBJECT_ID('dbo.SaleDetails', 'U')    IS NOT NULL DROP TABLE dbo.SaleDetails;
IF OBJECT_ID('dbo.Transactions', 'U')   IS NOT NULL DROP TABLE dbo.Transactions;
IF OBJECT_ID('dbo.Sales', 'U')          IS NOT NULL DROP TABLE dbo.Sales;
IF OBJECT_ID('dbo.Discounts', 'U')      IS NOT NULL DROP TABLE dbo.Discounts;
IF OBJECT_ID('dbo.ProductPriceHistory','U') IS NOT NULL DROP TABLE dbo.ProductPriceHistory;
IF OBJECT_ID('dbo.Account', 'U')        IS NOT NULL DROP TABLE dbo.Account;
IF OBJECT_ID('dbo.Employees', 'U')      IS NOT NULL DROP TABLE dbo.Employees;
IF OBJECT_ID('dbo.Products', 'U')       IS NOT NULL DROP TABLE dbo.Products;
IF OBJECT_ID('dbo.Customers', 'U')      IS NOT NULL DROP TABLE dbo.Customers;
GO

-- =============================
-- Tables
-- =============================

-- Products -----------------------------------------------------

CREATE TABLE dbo.Products
(
	ProductID      INT            IDENTITY(1, 1) PRIMARY KEY,
	ProductName    NVARCHAR(100)  NOT NULL UNIQUE,
	Price          DECIMAL(18, 2) NOT NULL CHECK (Price > 0),
	StockQuantity  INT            NOT NULL CHECK (StockQuantity >= 0),
	Unit           NVARCHAR(50)   NOT NULL
);
GO

-- Customers ----------------------------------------------------
CREATE TABLE dbo.Customers
(
	CustomerID     INT            IDENTITY(1, 1) PRIMARY KEY,
	CustomerName   NVARCHAR(100)  NOT NULL,
	Phone          VARCHAR(20)    NOT NULL UNIQUE,
	Address        NVARCHAR(255)  NULL,
	LoyaltyPoints  INT            NOT NULL CONSTRAINT DF_Customers_LoyaltyPoints DEFAULT(0)
		CHECK (LoyaltyPoints >= 0)
);
GO

-- Employees ----------------------------------------------------
CREATE TABLE dbo.Employees
(
	EmployeeID   INT            IDENTITY(1, 1) PRIMARY KEY,
	EmployeeName NVARCHAR(100)  NOT NULL,
	Phone        VARCHAR(20)    NOT NULL UNIQUE,
	Address      NVARCHAR(255)  NULL,
	Position     NVARCHAR(20)   NOT NULL CHECK (Position IN ('manager', 'saler')),
	HireDate     DATETIME       NOT NULL CONSTRAINT DF_Employees_HireDate DEFAULT (GETDATE()),
	Salary       DECIMAL(18, 2) NULL CHECK (Salary >= 0)
);
GO

-- Account ------------------------------------------------------
CREATE TABLE dbo.Account
(
	Username    NVARCHAR(50)   NOT NULL PRIMARY KEY,
	Password    NVARCHAR(255)  NOT NULL,
	Role        NVARCHAR(20)   NOT NULL CHECK (Role IN ('manager', 'saler', 'customer')),
	CreatedDate DATETIME       NOT NULL CONSTRAINT DF_Account_CreatedDate DEFAULT (GETDATE()),
	CustomerID  INT            NULL,
	EmployeeID  INT            NULL,
	CONSTRAINT FK_Account_Customers_CustomerID FOREIGN KEY (CustomerID)
		REFERENCES dbo.Customers(CustomerID),
	CONSTRAINT FK_Account_Employees_EmployeeID FOREIGN KEY (EmployeeID)
		REFERENCES dbo.Employees(EmployeeID),
	CONSTRAINT CK_Account_Role_ID CHECK (
		(Role IN ('manager', 'saler') AND EmployeeID IS NOT NULL AND CustomerID IS NULL)
		OR (Role = 'customer' AND CustomerID IS NOT NULL AND EmployeeID IS NULL)
	)
);
GO

-- Sales --------------------------------------------------------
CREATE TABLE dbo.Sales
(
	SaleID         INT            IDENTITY(1, 1) PRIMARY KEY,
	CustomerID     INT            NULL,
	SaleDate       DATETIME       NOT NULL CONSTRAINT DF_Sales_SaleDate DEFAULT (GETDATE()),
	TotalAmount    DECIMAL(18, 2) NOT NULL CONSTRAINT DF_Sales_TotalAmount DEFAULT (0)
		CHECK (TotalAmount >= 0),
	PaymentMethod  NVARCHAR(50)   NULL,
	CONSTRAINT FK_Sales_Customers FOREIGN KEY (CustomerID)
		REFERENCES dbo.Customers(CustomerID)
);
GO

-- SaleDetails --------------------------------------------------
CREATE TABLE dbo.SaleDetails
(
	SaleID      INT            NOT NULL,
	ProductID   INT            NOT NULL,
	Quantity    INT            NOT NULL CHECK (Quantity > 0),
	SalePrice   DECIMAL(18, 2) NOT NULL CHECK (SalePrice > 0),
	LineTotal   DECIMAL(18, 2) NOT NULL CHECK (LineTotal >= 0),
	CONSTRAINT PK_SaleDetails PRIMARY KEY (SaleID, ProductID),
	CONSTRAINT FK_SaleDetails_Sales    FOREIGN KEY (SaleID)   REFERENCES dbo.Sales(SaleID),
	CONSTRAINT FK_SaleDetails_Products FOREIGN KEY (ProductID) REFERENCES dbo.Products(ProductID)
);
GO

-- Transactions -------------------------------------------------
CREATE TABLE dbo.Transactions
(
	TransactionID   INT            IDENTITY(1, 1) PRIMARY KEY,
	TransactionType NVARCHAR(20)   NOT NULL CHECK (TransactionType IN ('income', 'expense', 'transfer')),
	Amount          DECIMAL(18, 2) NOT NULL CHECK (Amount > 0),
	Description     NVARCHAR(255)  NULL,
	TransactionDate DATETIME       NOT NULL CONSTRAINT DF_Transactions_Date DEFAULT (GETDATE()),
	CreatedBy       NVARCHAR(50)   NOT NULL,
	ReferenceID     INT            NULL,
	ReferenceType   NVARCHAR(20)   NULL CHECK (ReferenceType IN ('sale', 'purchase', 'refund', 'other')),
	CONSTRAINT FK_Transactions_Account_CreatedBy FOREIGN KEY (CreatedBy)
		REFERENCES dbo.Account(Username)
);
GO

-- Discounts ----------------------------------------------------
CREATE TABLE dbo.Discounts
(
	DiscountID    INT            IDENTITY(1, 1) PRIMARY KEY,
	ProductID     INT            NOT NULL,
	DiscountType  NVARCHAR(20)   NOT NULL CHECK (DiscountType IN ('percentage', 'fixed')),
	DiscountValue DECIMAL(18, 2) NOT NULL CHECK (DiscountValue > 0),
	StartDate     DATETIME       NOT NULL,
	EndDate       DATETIME       NOT NULL,
	IsActive      BIT            NOT NULL CONSTRAINT DF_Discounts_IsActive DEFAULT (1),
	CreatedDate   DATETIME       NOT NULL CONSTRAINT DF_Discounts_CreatedDate DEFAULT (GETDATE()),
	CreatedBy     NVARCHAR(50)   NOT NULL,
	CONSTRAINT FK_Discounts_Products FOREIGN KEY (ProductID)
		REFERENCES dbo.Products(ProductID) ON DELETE CASCADE,
	CONSTRAINT FK_Discounts_Account_CreatedBy FOREIGN KEY (CreatedBy)
		REFERENCES dbo.Account(Username),
	CONSTRAINT CK_Discount_DateRange CHECK (EndDate > StartDate),
	CONSTRAINT CK_Discount_Percentage CHECK (DiscountType <> 'percentage' OR DiscountValue <= 100)
);
GO


-- =============================
-- End of DDL
-- =============================
