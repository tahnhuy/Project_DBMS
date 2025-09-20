using System;
using System.Data;
using System.Data.SqlClient;

namespace Sale_Management.DatabaseAccess
{
    public class SaleRepository
    {
        public DataTable GetAllSales()
        {
            try
            {
                return DatabaseConnection.ExecuteQuery("GetAllSales", CommandType.StoredProcedure, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách hóa đơn: " + ex.Message);
            }
        }

        public DataTable GetSaleById(int saleId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@SaleID", SqlDbType.Int) { Value = saleId }
                };
                return DatabaseConnection.ExecuteQuery("GetSaleByID", CommandType.StoredProcedure, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy hóa đơn theo ID: " + ex.Message);
            }
        }

        public DataTable GetSaleDetails(int saleId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@SaleID", SqlDbType.Int) { Value = saleId }
                };
                return DatabaseConnection.ExecuteQuery("GetSaleDetails", CommandType.StoredProcedure, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy chi tiết hóa đơn: " + ex.Message);
            }
        }

        public int CreateSale(int? customerId, decimal totalAmount, string paymentMethod)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CustomerID", SqlDbType.Int) { Value = customerId ?? (object)DBNull.Value },
                    new SqlParameter("@TotalAmount", SqlDbType.Decimal) { Value = totalAmount },
                    new SqlParameter("@PaymentMethod", SqlDbType.NVarChar, 50) { Value = paymentMethod ?? (object)DBNull.Value }
                };

                DataTable result = DatabaseConnection.ExecuteQuery("CreateSale", CommandType.StoredProcedure, parameters);
                
                if (result.Rows.Count > 0)
                {
                    string resultStatus = result.Rows[0]["Result"].ToString();
                    if (resultStatus == "SUCCESS")
                        return Convert.ToInt32(result.Rows[0]["SaleID"]);
                    else
                        throw new Exception(result.Rows[0]["Message"].ToString());
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo hóa đơn: " + ex.Message);
            }
        }

        public bool AddSaleDetail(int saleId, int productId, int quantity, decimal salePrice)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@SaleID", SqlDbType.Int) { Value = saleId },
                    new SqlParameter("@ProductID", SqlDbType.Int) { Value = productId },
                    new SqlParameter("@Quantity", SqlDbType.Int) { Value = quantity },
                    new SqlParameter("@SalePrice", SqlDbType.Decimal) { Value = salePrice }
                };

                DataTable result = DatabaseConnection.ExecuteQuery("AddSaleDetail", CommandType.StoredProcedure, parameters);
                
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
                throw new Exception("Lỗi khi thêm chi tiết hóa đơn: " + ex.Message);
            }
        }

        public bool UpdateSale(int saleId, int? customerId, decimal totalAmount, string paymentMethod)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@SaleID", SqlDbType.Int) { Value = saleId },
                    new SqlParameter("@CustomerID", SqlDbType.Int) { Value = customerId ?? (object)DBNull.Value },
                    new SqlParameter("@TotalAmount", SqlDbType.Decimal) { Value = totalAmount },
                    new SqlParameter("@PaymentMethod", SqlDbType.NVarChar, 50) { Value = paymentMethod ?? (object)DBNull.Value }
                };

                DataTable result = DatabaseConnection.ExecuteQuery("UpdateSale", CommandType.StoredProcedure, parameters);
                
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
                throw new Exception("Lỗi khi cập nhật hóa đơn: " + ex.Message);
            }
        }

        public bool DeleteSale(int saleId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@SaleID", SqlDbType.Int) { Value = saleId }
                };

                DataTable result = DatabaseConnection.ExecuteQuery("DeleteSale", CommandType.StoredProcedure, parameters);
                
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
                throw new Exception("Lỗi khi xóa hóa đơn: " + ex.Message);
            }
        }

        // Lấy tóm tắt hóa đơn với thông tin khách hàng
        public DataTable GetSalesSummary()
        {
            try
            {
                return DatabaseConnection.ExecuteQuery("SELECT * FROM SalesSummary ORDER BY SaleDate DESC", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy tóm tắt hóa đơn: " + ex.Message);
            }
        }

        // Lấy báo cáo bán hàng theo ngày
        public DataTable GetDailySalesReport()
        {
            try
            {
                return DatabaseConnection.ExecuteQuery("SELECT * FROM DailySalesReport ORDER BY SalesDate DESC", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy báo cáo bán hàng theo ngày: " + ex.Message);
            }
        }

        // Lấy báo cáo bán hàng theo tháng
        public DataTable GetMonthlySalesReport()
        {
            try
            {
                return DatabaseConnection.ExecuteQuery("SELECT * FROM MonthlySalesReport ORDER BY SalesYear DESC, SalesMonth DESC", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy báo cáo bán hàng theo tháng: " + ex.Message);
            }
        }

        // Lấy báo cáo bán hàng cho một ngày cụ thể
        public DataTable GetDailySalesReportByDate(DateTime date)
        {
            try
            {
                string query = "SELECT * FROM DailySalesReport WHERE SalesDate = @Date";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Date", SqlDbType.Date) { Value = date.Date }
                };
                return DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy báo cáo bán hàng theo ngày: " + ex.Message);
            }
        }

        // Lấy báo cáo bán hàng cho một tháng cụ thể
        public DataTable GetMonthlySalesReportByMonth(int year, int month)
        {
            try
            {
                string query = "SELECT * FROM MonthlySalesReport WHERE SalesYear = @Year AND SalesMonth = @Month";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Year", SqlDbType.Int) { Value = year },
                    new SqlParameter("@Month", SqlDbType.Int) { Value = month }
                };
                return DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy báo cáo bán hàng theo tháng: " + ex.Message);
            }
        }
    }
}

