using System;
using System.Data;
using System.Data.SqlClient;

namespace Sale_Management.DatabaseAccess
{
    public class ReportRepository
    {
        public decimal GetDailyRevenue(DateTime date)
        {
            try
            {
                string query = "SELECT dbo.GetDailyRevenue(@Date) as DailyRevenue";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Date", SqlDbType.Date) { Value = date.Date }
                };

                DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
                if (result.Rows.Count > 0)
                {
                    return Convert.ToDecimal(result.Rows[0]["DailyRevenue"]);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy doanh thu ngày: " + ex.Message);
            }
        }

        public decimal GetMonthlyRevenue(int year, int month)
        {
            try
            {
                string query = "SELECT dbo.GetMonthlyRevenue(@Year, @Month) as MonthlyRevenue";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Year", SqlDbType.Int) { Value = year },
                    new SqlParameter("@Month", SqlDbType.Int) { Value = month }
                };

                DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
                if (result.Rows.Count > 0)
                {
                    return Convert.ToDecimal(result.Rows[0]["MonthlyRevenue"]);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy doanh thu tháng: " + ex.Message);
            }
        }

        public DataTable GetLowStockProducts()
        {
            try
            {
                // Try to use the view first
                return DatabaseConnection.ExecuteQuery("SELECT * FROM LowStockProducts", CommandType.Text, null);
            }
            catch (Exception)
            {
                // If view doesn't exist, create a query that mimics the view functionality
                try
                {
                    string query = @"
                        SELECT
                            p.ProductID,
                            p.ProductName,
                            p.StockQuantity,
                            p.MinStockLevel,
                            p.Unit,
                            CASE 
                                WHEN p.StockQuantity <= p.MinStockLevel THEN 'Cần nhập hàng'
                                WHEN p.StockQuantity <= (p.MinStockLevel * 1.5) THEN 'Sắp hết hàng'
                                ELSE 'Đủ hàng'
                            END AS StockStatus
                        FROM
                            Products p
                        WHERE
                            p.StockQuantity <= (p.MinStockLevel * 1.5)";
                    
                    return DatabaseConnection.ExecuteQuery(query, CommandType.Text, null);
                }
                catch (Exception innerEx)
                {
                    throw new Exception("Lỗi khi lấy danh sách sản phẩm sắp hết hàng: " + innerEx.Message);
                }
            }
        }

    }
}