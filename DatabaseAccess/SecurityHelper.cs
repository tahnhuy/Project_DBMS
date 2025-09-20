using System;
using System.Data;
using System.Data.SqlClient;

namespace Sale_Management.DatabaseAccess
{
    /// <summary>
    /// Helper class để xử lý bảo mật và phân quyền
    /// </summary>
    public static class SecurityHelper
    {
        /// <summary>
        /// Kiểm tra quyền đăng nhập và trả về role
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <param name="password">Mật khẩu</param>
        /// <returns>Role của user hoặc thông báo lỗi</returns>
        public static string CheckUserRole(string username, string password)
        {
            try
            {
                // Sử dụng EXECUTE cho scalar function
                string query = "SELECT dbo.fn_CheckUserRole(@Username, @Password) as UserRole";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username },
                    new SqlParameter("@Password", SqlDbType.NVarChar, 255) { Value = password }
                };

                DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
                if (result.Rows.Count > 0)
                {
                    return result.Rows[0]["UserRole"].ToString();
                }
                return "Lỗi không xác định";
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi kiểm tra quyền: " + ex.Message);
            }
        }

        /// <summary>
        /// Kiểm tra xem user có quyền thực hiện hành động không
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <param name="action">Hành động cần kiểm tra</param>
        /// <returns>True nếu có quyền</returns>
        public static bool HasPermission(string username, string action)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username },
                    new SqlParameter("@Action", SqlDbType.NVarChar, 50) { Value = action }
                };

                DataTable result = DatabaseConnection.ExecuteQuery("CheckUserPermission", CommandType.StoredProcedure, parameters);
                if (result.Rows.Count > 0)
                {
                    return Convert.ToBoolean(result.Rows[0]["HasPermission"]);
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi kiểm tra quyền: " + ex.Message);
            }
        }

        /// <summary>
        /// Kiểm tra role của user
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <returns>Role của user</returns>
        public static string GetUserRole(string username)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username }
                };

                DataTable result = DatabaseConnection.ExecuteQuery("GetAccountDetails", CommandType.StoredProcedure, parameters);
                if (result.Rows.Count > 0)
                {
                    return result.Rows[0]["Role"].ToString();
                }
                return "unknown";
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy role: " + ex.Message);
            }
        }

        /// <summary>
        /// Kiểm tra xem user có phải manager không
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <returns>True nếu là manager</returns>
        public static bool IsManager(string username)
        {
            return GetUserRole(username) == "manager";
        }

        /// <summary>
        /// Kiểm tra xem user có phải saler không
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <returns>True nếu là saler</returns>
        public static bool IsSaler(string username)
        {
            return GetUserRole(username) == "saler";
        }

        /// <summary>
        /// Kiểm tra xem user có phải customer không
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <returns>True nếu là customer</returns>
        public static bool IsCustomer(string username)
        {
            return GetUserRole(username) == "customer";
        }

        /// <summary>
        /// Kiểm tra quyền quản lý sản phẩm
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <returns>True nếu có quyền</returns>
        public static bool CanManageProducts(string username)
        {
            string role = GetUserRole(username);
            return role == "manager" || role == "saler";
        }

        /// <summary>
        /// Kiểm tra quyền xóa sản phẩm
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <returns>True nếu có quyền</returns>
        public static bool CanDeleteProducts(string username)
        {
            return IsManager(username);
        }

        /// <summary>
        /// Kiểm tra quyền quản lý khách hàng
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <returns>True nếu có quyền</returns>
        public static bool CanManageCustomers(string username)
        {
            string role = GetUserRole(username);
            return role == "manager" || role == "saler";
        }

        /// <summary>
        /// Kiểm tra quyền xóa khách hàng
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <returns>True nếu có quyền</returns>
        public static bool CanDeleteCustomers(string username)
        {
            return IsManager(username);
        }

        /// <summary>
        /// Kiểm tra quyền quản lý giảm giá
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <returns>True nếu có quyền</returns>
        public static bool CanManageDiscounts(string username)
        {
            return IsManager(username);
        }

        /// <summary>
        /// Kiểm tra quyền quản lý tài khoản
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <returns>True nếu có quyền</returns>
        public static bool CanManageAccounts(string username)
        {
            return IsManager(username);
        }

        /// <summary>
        /// Kiểm tra quyền xem báo cáo doanh thu
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <returns>True nếu có quyền</returns>
        public static bool CanViewRevenueReports(string username)
        {
            string role = GetUserRole(username);
            return role == "manager" || role == "saler";
        }

        /// <summary>
        /// Kiểm tra quyền xem báo cáo chi tiết
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <returns>True nếu có quyền</returns>
        public static bool CanViewDetailedReports(string username)
        {
            return IsManager(username);
        }

        /// <summary>
        /// Validate username theo quy tắc
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <returns>True nếu hợp lệ</returns>
        public static bool ValidateUsername(string username)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username }
                };

                DataTable result = DatabaseConnection.ExecuteQuery("ValidateUsername", CommandType.StoredProcedure, parameters);
                if (result.Rows.Count > 0)
                {
                    return Convert.ToBoolean(result.Rows[0]["IsValid"]);
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi validate username: " + ex.Message);
            }
        }

        /// <summary>
        /// Validate password theo quy tắc
        /// </summary>
        /// <param name="password">Mật khẩu</param>
        /// <returns>True nếu hợp lệ</returns>
        public static bool ValidatePassword(string password)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Password", SqlDbType.NVarChar, 255) { Value = password }
                };

                DataTable result = DatabaseConnection.ExecuteQuery("ValidatePassword", CommandType.StoredProcedure, parameters);
                if (result.Rows.Count > 0)
                {
                    return Convert.ToBoolean(result.Rows[0]["IsValid"]);
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi validate password: " + ex.Message);
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết tài khoản
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <returns>DataTable chứa thông tin tài khoản</returns>
        public static DataTable GetAccountDetails(string username)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username }
                };

                return DatabaseConnection.ExecuteQuery("GetAccountDetails", CommandType.StoredProcedure, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin tài khoản: " + ex.Message);
            }
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <param name="oldPassword">Mật khẩu cũ</param>
        /// <param name="newPassword">Mật khẩu mới</param>
        /// <returns>True nếu thành công</returns>
        public static bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username },
                    new SqlParameter("@OldPassword", SqlDbType.NVarChar, 255) { Value = oldPassword },
                    new SqlParameter("@NewPassword", SqlDbType.NVarChar, 255) { Value = newPassword }
                };

                DataTable result = DatabaseConnection.ExecuteQuery("ChangePassword", CommandType.StoredProcedure, parameters);
                if (result.Rows.Count > 0)
                {
                    string resultStatus = result.Rows[0]["Result"].ToString();
                    if (resultStatus == "SUCCESS")
                        return true;
                    else
                        throw new Exception(result.Rows[0]["Message"].ToString());
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi đổi mật khẩu: " + ex.Message);
            }
        }
    }
}
