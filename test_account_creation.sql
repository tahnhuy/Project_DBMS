-- Test tạo tài khoản mới
DECLARE @Result TABLE (Result NVARCHAR(20), Message NVARCHAR(255));

-- Test tạo tài khoản customer không có CustomerID
INSERT INTO @Result
EXEC AddAccount 'test_customer', '123456', 'customer', NULL, NULL;

SELECT * FROM @Result;

-- Test tạo tài khoản manager không có EmployeeID  
INSERT INTO @Result
EXEC AddAccount 'test_manager', '123456', 'manager', NULL, NULL;

SELECT * FROM @Result;

-- Test tạo tài khoản saler không có EmployeeID
INSERT INTO @Result
EXEC AddAccount 'test_saler', '123456', 'saler', NULL, NULL;

SELECT * FROM @Result;
