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

        public decimal GetSalesGrowthRateMonthly(int cpYear, int cpMonth, int ppYear, int ppMonth)
        {
            try
            {
                string query = "SELECT dbo.GetSalesGrowthRate_Monthly(@CP_Year, @CP_Month, @PP_Year, @PP_Month) as GrowthRate";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CP_Year", SqlDbType.Int) { Value = cpYear },
                    new SqlParameter("@CP_Month", SqlDbType.Int) { Value = cpMonth },
                    new SqlParameter("@PP_Year", SqlDbType.Int) { Value = ppYear },
                    new SqlParameter("@PP_Month", SqlDbType.Int) { Value = ppMonth }
                };

                DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
                if (result.Rows.Count > 0 && result.Rows[0]["GrowthRate"] != DBNull.Value)
                {
                    // Hàm SQL trả về giá trị 999.99 nếu tăng trưởng vô hạn, sẽ được xử lý ở UI
                    return Convert.ToDecimal(result.Rows[0]["GrowthRate"]);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính tỷ lệ tăng trưởng doanh số: " + ex.Message);
            }
        }

        // NEW METHOD: Top Sản phẩm Bán chạy
        public DataTable GetTopSellingProducts(DateTime startDate, DateTime endDate, int topN)
        {
            try
            {
                string query = "SELECT * FROM dbo.fnReport_TopSellingProducts(@StartDate, @EndDate, @TopN)";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@StartDate", SqlDbType.Date) { Value = startDate.Date },
                    new SqlParameter("@EndDate", SqlDbType.Date) { Value = endDate.Date },
                    new SqlParameter("@TopN", SqlDbType.Int) { Value = topN }
                };

                return DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách sản phẩm bán chạy nhất: " + ex.Message);
            }
        }

        // NEW METHOD: Xếp hạng Khách hàng
        public DataTable GetCustomerRanking(DateTime startDate, DateTime endDate)
        {
            try
            {
                string query = "SELECT * FROM dbo.fnReport_CustomerRanking(@StartDate, @EndDate)";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@StartDate", SqlDbType.Date) { Value = startDate.Date },
                    new SqlParameter("@EndDate", SqlDbType.Date) { Value = endDate.Date }
                };

                return DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy bảng xếp hạng khách hàng: " + ex.Message);
            }
        }

        // Trong file DatabaseAccess/ReportRepository.cs
        public DataTable GetProductSalesTrend(int productId, DateTime startDate, DateTime endDate)
        {
            try
            {
                string query = "SELECT * FROM dbo.fnReport_DailyProductSalesTrend(@ProductID, @StartDate, @EndDate)";
                SqlParameter[] parameters = new SqlParameter[]
                {
            new SqlParameter("@ProductID", SqlDbType.Int) { Value = productId },
            new SqlParameter("@StartDate", SqlDbType.Date) { Value = startDate.Date },
            new SqlParameter("@EndDate", SqlDbType.Date) { Value = endDate.Date }
                };

                return DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy xu hướng bán hàng sản phẩm: " + ex.Message);
            }
        }
    }
}