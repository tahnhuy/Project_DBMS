using System;
using System.Data;
using System.Data.SqlClient;

namespace Sale_Management.DatabaseAccess
{
    internal class AccountRepository
    {
        public static DataTable CheckLogin(string username, string password)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username },
                new SqlParameter("@Password", SqlDbType.NVarChar, 255) { Value = password }
            };
            // Use TVF fnAuth_User
            return DatabaseConnection.ExecuteQuery("SELECT * FROM dbo.fnAuth_User(@Username, @Password)", CommandType.Text, parameters);
        }

        public static DataTable ChangePassword(string username, string oldPassword, string newPassword)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username },
                new SqlParameter("@OldPassword", SqlDbType.NVarChar, 255) { Value = oldPassword },
                new SqlParameter("@NewPassword", SqlDbType.NVarChar, 255) { Value = newPassword }
            };
            return DatabaseConnection.ExecuteQuery("ChangePassword", CommandType.StoredProcedure, parameters);
        }

        // Lấy tất cả tài khoản
        public DataTable GetAllAccounts()
        {
            return DatabaseConnection.ExecuteQuery("SELECT * FROM dbo.fnAccounts_All()", CommandType.Text);
        }

        // Lấy tài khoản theo username
        public DataTable GetAccountByUsername(string username)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username }
            };
            return DatabaseConnection.ExecuteQuery("SELECT * FROM dbo.fnAccounts_ByUsername(@Username)", CommandType.Text, parameters);
        }

        // Lấy tài khoản theo role
        public DataTable GetAccountsByRole(string role)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@Role", SqlDbType.NVarChar, 20) { Value = role }
            };
            return DatabaseConnection.ExecuteQuery("SELECT * FROM dbo.fnAccounts_ByRole(@Role)", CommandType.Text, parameters);
        }

        // Tìm kiếm tài khoản theo username
        public DataTable SearchAccountsByUsername(string searchText)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = "%" + searchText + "%" }
            };
            return DatabaseConnection.ExecuteQuery("SELECT * FROM dbo.fnAccounts_All() WHERE Username LIKE @Username", CommandType.Text, parameters);
        }

        // Tìm kiếm tài khoản theo tên
        public DataTable SearchAccountsByName(string searchText)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@SearchText", SqlDbType.NVarChar, 100) { Value = "%" + searchText + "%" }
            };
            return DatabaseConnection.ExecuteQuery("SELECT * FROM dbo.fnAccounts_All() WHERE FullName LIKE @SearchText", CommandType.Text, parameters);
        }

        // Thêm tài khoản
        public bool AddAccount(string username, string password, string role, int? customerId = null, int? employeeId = null)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username },
                new SqlParameter("@Password", SqlDbType.NVarChar, 255) { Value = password },
                new SqlParameter("@Role", SqlDbType.NVarChar, 20) { Value = role },
                new SqlParameter("@CustomerID", SqlDbType.Int) { Value = customerId ?? (object)DBNull.Value },
                new SqlParameter("@EmployeeID", SqlDbType.Int) { Value = employeeId ?? (object)DBNull.Value }
            };

            try
            {
                DataTable result = DatabaseConnection.ExecuteQuery("AddAccount", CommandType.StoredProcedure, parameters);

                if (result != null && result.Rows.Count > 0)
                {
                    string resultStatus = result.Rows[0]["Result"].ToString();
                    string message = result.Rows[0]["Message"].ToString();

                    if (resultStatus == "SUCCESS")
                    {
                        return true;
                    }
                    else
                    {
                        // Throw exception với message từ stored procedure
                        throw new Exception(message);
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo tài khoản: {ex.Message}", ex);
            }
        }

        // Cập nhật tài khoản
        public bool UpdateAccount(string username, string password = null, string role = null, int? customerId = null, int? employeeId = null)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username },
                new SqlParameter("@Password", SqlDbType.NVarChar, 255) { Value = password ?? (object)DBNull.Value },
                new SqlParameter("@Role", SqlDbType.NVarChar, 20) { Value = role ?? (object)DBNull.Value },
                new SqlParameter("@CustomerID", SqlDbType.Int) { Value = customerId ?? (object)DBNull.Value },
                new SqlParameter("@EmployeeID", SqlDbType.Int) { Value = employeeId ?? (object)DBNull.Value }
            };

            DataTable result = DatabaseConnection.ExecuteQuery("UpdateAccount", CommandType.StoredProcedure, parameters);

            if (result != null && result.Rows.Count > 0)
            {
                string resultStatus = result.Rows[0]["Result"].ToString();
                return resultStatus == "SUCCESS";
            }
            return false;
        }

        // Xóa tài khoản
        public bool DeleteAccount(string username)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username }
            };

            DataTable result = DatabaseConnection.ExecuteQuery("DeleteAccount", CommandType.StoredProcedure, parameters);

            if (result != null && result.Rows.Count > 0)
            {
                string resultStatus = result.Rows[0]["Result"].ToString();
                return resultStatus == "SUCCESS";
            }
            return false;
        }

        // Lấy danh sách khách hàng để tạo tài khoản
        public DataTable GetCustomersForAccount()
        {
            return DatabaseConnection.ExecuteQuery("SELECT CustomerID, CustomerName, Phone FROM dbo.Customers ORDER BY CustomerName", CommandType.Text);
        }

        // Lấy danh sách nhân viên để tạo tài khoản
        public DataTable GetEmployeesForAccount()
        {
            return DatabaseConnection.ExecuteQuery("SELECT EmployeeID, EmployeeName, Phone, Position FROM dbo.Employees ORDER BY EmployeeName", CommandType.Text);
        }

        // Lấy số lượng Customer hiện tại để tính ID mặc định
        public int GetCustomerCount()
        {
            DataTable result = DatabaseConnection.ExecuteQuery("SELECT COUNT(*) AS Count FROM dbo.Customers", CommandType.Text);
            if (result != null && result.Rows.Count > 0)
            {
                return Convert.ToInt32(result.Rows[0]["Count"]);
            }
            return 0;
        }

        // Lấy số lượng Employee hiện tại để tính ID mặc định
        public int GetEmployeeCount()
        {
            DataTable result = DatabaseConnection.ExecuteQuery("SELECT COUNT(*) AS Count FROM dbo.Employees", CommandType.Text);
            if (result != null && result.Rows.Count > 0)
            {
                return Convert.ToInt32(result.Rows[0]["Count"]);
            }
            return 0;
        }

        // Kiểm tra username đã tồn tại chưa
        public bool IsUsernameExists(string username)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username }
            };

            DataTable result = DatabaseConnection.ExecuteQuery("SELECT COUNT(*) as Count FROM dbo.Account WHERE Username = @Username", CommandType.Text, parameters);

            if (result != null && result.Rows.Count > 0)
            {
                int count = Convert.ToInt32(result.Rows[0]["Count"]);
                return count > 0;
            }
            return false;
        }

        // Xóa SQL Account (Login và User)
        public bool DeleteSQLAccount(string username)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username }
            };

            try
            {
                DataTable result = DatabaseConnection.ExecuteQuery("DeleteSQLAccount", CommandType.StoredProcedure, parameters);

                if (result != null && result.Rows.Count > 0)
                {
                    string resultStatus = result.Rows[0]["Result"].ToString();
                    string message = result.Rows[0]["Message"].ToString();

                    if (resultStatus == "SUCCESS")
                    {
                        return true;
                    }
                    else
                    {
                        throw new Exception(message);
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa SQL Account: {ex.Message}", ex);
            }
        }

        // Tạo SQL Account (Login và User)
        public bool CreateSQLAccount(string username, string password, string role)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username },
                new SqlParameter("@Password", SqlDbType.NVarChar, 255) { Value = password },
                new SqlParameter("@Role", SqlDbType.NVarChar, 20) { Value = role }
            };

            try
            {
                DataTable result = DatabaseConnection.ExecuteQuery("CreateSQLAccount", CommandType.StoredProcedure, parameters);

                if (result != null && result.Rows.Count > 0)
                {
                    string resultStatus = result.Rows[0]["Result"].ToString();
                    string message = result.Rows[0]["Message"].ToString();

                    if (resultStatus == "SUCCESS")
                    {
                        return true;
                    }
                    else
                    {
                        throw new Exception(message);
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo SQL Account: {ex.Message}", ex);
            }
        }
    }
}