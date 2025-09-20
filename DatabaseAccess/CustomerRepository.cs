using System;
using System.Data;
using System.Data.SqlClient;

namespace Sale_Management.DatabaseAccess
{
    public class CustomerRepository
    {
        public DataTable GetAllCustomers()
        {
            try
            {
                return DatabaseConnection.ExecuteQuery("GetAllCustomers", CommandType.StoredProcedure, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách khách hàng: " + ex.Message);
            }
        }

        public DataTable GetCustomerById(int customerId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CustomerID", SqlDbType.Int) { Value = customerId }
                };
                return DatabaseConnection.ExecuteQuery("GetCustomerByID", CommandType.StoredProcedure, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy khách hàng theo ID: " + ex.Message);
            }
        }

        public DataTable GetCustomerByName(string customerName)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CustomerName", SqlDbType.NVarChar, 100) { Value = customerName ?? (object)DBNull.Value }
                };
                return DatabaseConnection.ExecuteQuery("GetCustomerByName", CommandType.StoredProcedure, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy khách hàng theo tên: " + ex.Message);
            }
        }

        public bool AddCustomer(string customerName, string phone, string address, int loyaltyPoints)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CustomerName", SqlDbType.NVarChar, 100) { Value = customerName },
                    new SqlParameter("@Phone", SqlDbType.NVarChar, 20) { Value = (object)phone ?? DBNull.Value },
                    new SqlParameter("@Address", SqlDbType.NVarChar, 200) { Value = (object)address ?? DBNull.Value },
                    new SqlParameter("@LoyaltyPoints", SqlDbType.Int) { Value = loyaltyPoints }
                };

                DataTable result = DatabaseConnection.ExecuteQuery("AddCustomer", CommandType.StoredProcedure, parameters);

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
                throw new Exception("Lỗi khi thêm khách hàng: " + ex.Message);
            }
        }

        public bool UpdateCustomer(int customerId, string customerName, string phone, string address, int loyaltyPoints)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CustomerID", SqlDbType.Int) { Value = customerId },
                    new SqlParameter("@CustomerName", SqlDbType.NVarChar, 100) { Value = customerName },
                    new SqlParameter("@Phone", SqlDbType.NVarChar, 20) { Value = (object)phone ?? DBNull.Value },
                    new SqlParameter("@Address", SqlDbType.NVarChar, 200) { Value = (object)address ?? DBNull.Value },
                    new SqlParameter("@LoyaltyPoints", SqlDbType.Int) { Value = loyaltyPoints }
                };

                DataTable result = DatabaseConnection.ExecuteQuery("UpdateCustomer", CommandType.StoredProcedure, parameters);

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
                throw new Exception("Lỗi khi cập nhật khách hàng: " + ex.Message);
            }
        }

        public bool DeleteCustomer(int customerId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CustomerID", SqlDbType.Int) { Value = customerId }
                };

                DataTable result = DatabaseConnection.ExecuteQuery("DeleteCustomer", CommandType.StoredProcedure, parameters);

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
                throw new Exception("Lỗi khi xóa khách hàng: " + ex.Message);
            }
        }

        public DataTable GetCustomerByUsername(string username)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username }
            };
            return DatabaseConnection.ExecuteQuery("GetCustomerByUsername", CommandType.StoredProcedure, parameters);
        }

        public DataTable GetSalesByCustomerUsername(string username)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username }
            };
            return DatabaseConnection.ExecuteQuery("GetSalesByCustomerUsername", CommandType.StoredProcedure, parameters);
        }

        public bool UpdateCustomerByUsername(string username, string customerName, string phone, string address)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username },
                new SqlParameter("@CustomerName", SqlDbType.NVarChar, 100) { Value = customerName },
                new SqlParameter("@Phone", SqlDbType.NVarChar, 20) { Value = (object)phone ?? DBNull.Value },
                new SqlParameter("@Address", SqlDbType.NVarChar, 200) { Value = (object)address ?? DBNull.Value }
            };

            DataTable result = DatabaseConnection.ExecuteQuery("UpdateCustomerByUsername", CommandType.StoredProcedure, parameters);
            if (result.Rows.Count > 0)
            {
                string resultStatus = result.Rows[0]["Result"].ToString();
                if (resultStatus == "SUCCESS")
                    return true;
                throw new Exception(result.Rows[0]["Message"].ToString());
            }
            return false;
        }

        public DataTable SearchCustomers(string searchTerm)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@SearchTerm", SqlDbType.NVarChar, 100) { Value = searchTerm ?? (object)DBNull.Value }
                };
                return DatabaseConnection.ExecuteQuery("SearchCustomers", CommandType.StoredProcedure, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm khách hàng: " + ex.Message);
            }
        }

        // Sử dụng function mới để tìm kiếm đa điều kiện
        public DataTable SearchCustomersAdvanced(string searchTerm)
        {
            try
            {
                string query = "SELECT * FROM dbo.SearchCustomers(@SearchTerm)";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@SearchTerm", SqlDbType.NVarChar, 100) { Value = searchTerm ?? (object)DBNull.Value }
                };
                return DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm khách hàng nâng cao: " + ex.Message);
            }
        }

        // Lấy lịch sử mua hàng của khách hàng
        public DataTable GetCustomerPurchaseHistory(int customerId)
        {
            try
            {
                string query = "SELECT * FROM dbo.GetCustomerPurchaseHistory(@CustomerID)";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CustomerID", SqlDbType.Int) { Value = customerId }
                };
                return DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy lịch sử mua hàng: " + ex.Message);
            }
        }

        // Lấy tóm tắt mua hàng của khách hàng
        public DataTable GetCustomerPurchaseSummary()
        {
            try
            {
                return DatabaseConnection.ExecuteQuery("SELECT * FROM CustomerPurchaseSummary", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy tóm tắt mua hàng khách hàng: " + ex.Message);
            }
        }

        // Lấy tóm tắt mua hàng của một khách hàng cụ thể
        public DataTable GetCustomerPurchaseSummaryById(int customerId)
        {
            try
            {
                string query = "SELECT * FROM CustomerPurchaseSummary WHERE CustomerID = @CustomerID";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CustomerID", SqlDbType.Int) { Value = customerId }
                };
                return DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy tóm tắt mua hàng khách hàng: " + ex.Message);
            }
        }
    }
}
