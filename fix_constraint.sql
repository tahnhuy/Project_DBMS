-- Sửa CHECK constraint để cho phép tạo tài khoản độc lập
USE Minimart_SalesDB;
GO

-- Xóa constraint cũ
ALTER TABLE dbo.Account DROP CONSTRAINT CK_Account_Role_ID;
GO

-- Tạo constraint mới cho phép tài khoản độc lập
ALTER TABLE dbo.Account 
ADD CONSTRAINT CK_Account_Role_ID 
CHECK (
    ([Role]='saler' OR [Role]='manager') AND 
    ([EmployeeID] IS NOT NULL OR ([EmployeeID] IS NULL AND [CustomerID] IS NULL)) AND 
    [CustomerID] IS NULL
    OR 
    [Role]='customer' AND 
    ([CustomerID] IS NOT NULL OR ([CustomerID] IS NULL AND [EmployeeID] IS NULL)) AND 
    [EmployeeID] IS NULL
);
GO

-- Test lại
EXEC AddAccount 'saler3', '123456', 'saler', NULL, NULL;
