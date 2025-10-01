-- Test AddAccount stored procedure
USE Minimart_SalesDB;
GO

-- Test tạo tài khoản saler
DECLARE @Result TABLE (Result NVARCHAR(20), Message NVARCHAR(255));

INSERT INTO @Result
EXEC AddAccount 'test_saler_new', '123456', 'saler', NULL, NULL;

SELECT * FROM @Result;

-- Kiểm tra tài khoản đã được tạo
SELECT Username, Role, CustomerID, EmployeeID, CreatedDate 
FROM dbo.Account 
WHERE Username = 'test_saler_new';
