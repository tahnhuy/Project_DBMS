using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Sale_Management.DatabaseAccess
{
    public class CustomerRepository
    {
        public DataTable GetAllCustomers(string searchQuery = null)
        {
            try
            {
                if (string.IsNullOrEmpty(searchQuery))
                {
                    // Gọi stored procedure GetAllCustomers
                    return DatabaseConnection.ExecuteQuery("GetAllCustomers", CommandType.StoredProcedure);
                }
                else
                {
                    // Gọi stored procedure GetCustomerByName với search query
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@CustomerName", SqlDbType.NVarChar, 100) { Value = searchQuery }
                    };
                    return DatabaseConnection.ExecuteQuery("GetCustomerByName", CommandType.StoredProcedure, parameters);
                }
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
                throw new Exception("Lỗi khi lấy thông tin khách hàng: " + ex.Message);
            }
        }

        public DataTable GetCustomerByName(string customerName)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CustomerName", SqlDbType.NVarChar, 100) { Value = customerName }
                };
                return DatabaseConnection.ExecuteQuery("GetCustomerByName", CommandType.StoredProcedure, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm khách hàng: " + ex.Message);
            }
        }

        public int AddCustomer(string customerName, string phone, string address, int loyaltyPoints = 0)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CustomerName", SqlDbType.NVarChar, 100) { Value = customerName },
                    new SqlParameter("@Phone", SqlDbType.NVarChar, 20) { Value = phone },
                    new SqlParameter("@Address", SqlDbType.NVarChar, 200) { Value = address },
                    new SqlParameter("@LoyaltyPoints", SqlDbType.Int) { Value = loyaltyPoints }
                };

                DataTable result = DatabaseConnection.ExecuteQuery("AddCustomer", CommandType.StoredProcedure, parameters);
                
                if (result.Rows.Count > 0 && result.Rows[0]["Result"].ToString() == "SUCCESS")
                {
                    return Convert.ToInt32(result.Rows[0]["CustomerID"]);
                }
                else
                {
                    throw new Exception("Không thể thêm khách hàng mới");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm khách hàng: " + ex.Message);
            }
        }

        public bool UpdateCustomer(int customerId, string customerName, string phone, string address, int loyaltyPoints = 0)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CustomerID", SqlDbType.Int) { Value = customerId },
                    new SqlParameter("@CustomerName", SqlDbType.NVarChar, 100) { Value = customerName },
                    new SqlParameter("@Phone", SqlDbType.NVarChar, 20) { Value = phone },
                    new SqlParameter("@Address", SqlDbType.NVarChar, 200) { Value = address },
                    new SqlParameter("@LoyaltyPoints", SqlDbType.Int) { Value = loyaltyPoints }
                };

                DataTable result = DatabaseConnection.ExecuteQuery("UpdateCustomer", CommandType.StoredProcedure, parameters);
                
                return result.Rows.Count > 0 && result.Rows[0]["Result"].ToString() == "SUCCESS";
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
                else
                {
                    throw new Exception("Không có phản hồi từ server");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa khách hàng: " + ex.Message);
            }
        }
    }
}
