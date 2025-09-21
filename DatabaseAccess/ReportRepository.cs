using System;
using System.Data;
using System.Data.SqlClient;

namespace Sale_Management.DatabaseAccess
{
    public static class ReportRepository
    {
        // Lấy doanh thu theo ngày
        public static decimal GetDailyRevenue(DateTime date)
        {
            try
            {
                string query = "SELECT dbo.GetDailyRevenue(@Date) as Revenue";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Date", SqlDbType.Date) { Value = date.Date }
                };

                DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
                if (result.Rows.Count > 0)
                {
                    return Convert.ToDecimal(result.Rows[0]["Revenue"]);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy doanh thu theo ngày: " + ex.Message);
            }
        }

        // Lấy doanh thu theo tháng
        public static decimal GetMonthlyRevenue(int year, int month)
        {
            try
            {
                string query = "SELECT dbo.GetMonthlyRevenue(@Year, @Month) as Revenue";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Year", SqlDbType.Int) { Value = year },
                    new SqlParameter("@Month", SqlDbType.Int) { Value = month }
                };

                DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
                if (result.Rows.Count > 0)
                {
                    return Convert.ToDecimal(result.Rows[0]["Revenue"]);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy doanh thu theo tháng: " + ex.Message);
            }
        }

        // Lấy thống kê tổng quan
        public static DataTable GetDashboardStats()
        {
            try
            {
                string query = "SELECT * FROM dbo.GetDashboardStats()";
                return DatabaseConnection.ExecuteQuery(query, CommandType.Text, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thống kê tổng quan: " + ex.Message);
            }
        }

        // Lấy chi tiết chương trình giảm giá đang hoạt động
        public static DataTable GetActiveDiscountsDetail()
        {
            try
            {
                return DatabaseConnection.ExecuteQuery("SELECT * FROM ActiveDiscountsDetail", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy chi tiết chương trình giảm giá: " + ex.Message);
            }
        }

        // Lấy tóm tắt giao dịch
        public static DataTable GetTransactionSummary()
        {
            try
            {
                return DatabaseConnection.ExecuteQuery("SELECT * FROM TransactionSummary ORDER BY TransactionDate DESC", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy tóm tắt giao dịch: " + ex.Message);
            }
        }

        // Lấy tóm tắt tài khoản
        public static DataTable GetAccountSummary()
        {
            try
            {
                return DatabaseConnection.ExecuteQuery("SELECT * FROM AccountSummary ORDER BY CreatedDate DESC", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy tóm tắt tài khoản: " + ex.Message);
            }
        }

        // Tính tổng chi phí theo loại
        public static decimal GetExpenseByType(string transactionType, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                string query = "SELECT dbo.GetExpenseByType(@TransactionType, @StartDate, @EndDate) as TotalExpense";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@TransactionType", SqlDbType.NVarChar, 20) { Value = transactionType },
                    new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = startDate ?? (object)DBNull.Value },
                    new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = endDate ?? (object)DBNull.Value }
                };

                DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
                if (result.Rows.Count > 0)
                {
                    return Convert.ToDecimal(result.Rows[0]["TotalExpense"]);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy tổng chi phí: " + ex.Message);
            }
        }

        // Tính điểm tích lũy từ số tiền
        public static int CalculateLoyaltyPoints(decimal amount)
        {
            try
            {
                string query = "SELECT dbo.CalculateLoyaltyPoints(@Amount) as Points";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Amount", SqlDbType.Decimal) { Value = amount }
                };

                DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
                if (result.Rows.Count > 0)
                {
                    return Convert.ToInt32(result.Rows[0]["Points"]);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính điểm tích lũy: " + ex.Message);
            }
        }

        // Tính tỷ lệ giảm giá
        public static decimal CalculateDiscountPercentage(decimal originalPrice, decimal discountedPrice)
        {
            try
            {
                string query = "SELECT dbo.CalculateDiscountPercentage(@OriginalPrice, @DiscountedPrice) as DiscountPercentage";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@OriginalPrice", SqlDbType.Decimal) { Value = originalPrice },
                    new SqlParameter("@DiscountedPrice", SqlDbType.Decimal) { Value = discountedPrice }
                };

                DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
                if (result.Rows.Count > 0)
                {
                    return Convert.ToDecimal(result.Rows[0]["DiscountPercentage"]);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính tỷ lệ giảm giá: " + ex.Message);
            }
        }

        // Validate số điện thoại Việt Nam
        public static bool IsValidVietnamesePhone(string phone)
        {
            try
            {
                string query = "SELECT dbo.IsValidVietnamesePhone(@Phone) as IsValid";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Phone", SqlDbType.VarChar, 20) { Value = phone ?? (object)DBNull.Value }
                };

                DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
                if (result.Rows.Count > 0)
                {
                    return Convert.ToBoolean(result.Rows[0]["IsValid"]);
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi validate số điện thoại: " + ex.Message);
            }
        }

        // Format tiền Việt Nam
        public static string FormatVietnamMoney(decimal amount)
        {
            try
            {
                string query = "SELECT dbo.FormatVietnamMoney(@Amount) as FormattedAmount";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Amount", SqlDbType.Decimal) { Value = amount }
                };

                DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
                if (result.Rows.Count > 0)
                {
                    return result.Rows[0]["FormattedAmount"].ToString();
                }
                return amount.ToString("N0") + " ₫";
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi format tiền: " + ex.Message);
            }
        }

        // Lấy báo cáo doanh thu theo sản phẩm
        public static DataTable GetProductRevenueReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                string query = "SELECT * FROM dbo.GetProductRevenueReport(@StartDate, @EndDate)";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = startDate },
                    new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = endDate }
                };

                return DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy báo cáo doanh thu sản phẩm: " + ex.Message);
            }
        }
    }
}
