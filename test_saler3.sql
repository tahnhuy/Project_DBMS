-- Test AddAccount với username "saler3"
USE Minimart_SalesDB;
GO

-- Test tạo tài khoản saler3
EXEC AddAccount 'saler3', '123456', 'saler', NULL, NULL;
