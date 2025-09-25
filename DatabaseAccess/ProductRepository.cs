using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Sale_Management.DatabaseAccess;
using System.Linq.Expressions;
using System.Windows.Forms;


namespace Sale_Management.DatabaseAccess
{
    public class ProductRepository
    {
        public DataTable GetAllProducts()
        {
            try
            {
                string query = @"
                    SELECT 
                        ProductID,
                        ProductName,
                        Price,
                        StockQuantity,
                        Unit
                    FROM Products
                    ORDER BY ProductID";
                return DatabaseConnection.ExecuteQuery(query, CommandType.Text, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy danh sách sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable(); // Trả về DataTable rỗng thay vì null
            }
        }

        public DataTable GetProductByName(string productName)
        {
            try
            {
                string query = "SELECT * FROM dbo.GetProductByName(@ProductName)";
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@ProductName", SqlDbType.NVarChar, 100) {Value = productName ?? ""}
                };
                    
                return DatabaseConnection.ExecuteQuery(query, CommandType.Text, para);
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Lỗi khi lấy sản phẩm theo tên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        public DataTable GetProductById(int productId)
        {
            try
            {
                string query = "SELECT * FROM dbo.GetProductByID(@ProductID)";
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@ProductID", SqlDbType.Int) {Value = productId}
                };
                return DatabaseConnection.ExecuteQuery(query, CommandType.Text, para);
            } catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy sản phẩm theo ID: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        public bool AddProduct(string productName, decimal price, int stockQuantity, string unit)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ProductName", SqlDbType.NVarChar, 100) { Value = productName },
                    new SqlParameter("@Price", SqlDbType.Decimal) { Value = price },
                    new SqlParameter("@StockQuantity", SqlDbType.Int) { Value = stockQuantity },
                    new SqlParameter("@Unit", SqlDbType.NVarChar, 50) { Value = unit }
                };

                DataTable result = DatabaseConnection.ExecuteQuery("AddProduct", CommandType.StoredProcedure, parameters);
                
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
                throw new Exception("Lỗi khi thêm sản phẩm: " + ex.Message);
            }
        }

        public bool UpdateProduct(int productId, string productName, decimal price, int stockQuantity, string unit)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ProductID", SqlDbType.Int) { Value = productId },
                    new SqlParameter("@ProductName", SqlDbType.NVarChar, 100) { Value = productName },
                    new SqlParameter("@Price", SqlDbType.Decimal) { Value = price },
                    new SqlParameter("@StockQuantity", SqlDbType.Int) { Value = stockQuantity },
                    new SqlParameter("@Unit", SqlDbType.NVarChar, 50) { Value = unit }
                };

                DataTable result = DatabaseConnection.ExecuteQuery("UpdateProduct", CommandType.StoredProcedure, parameters);
                
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
                throw new Exception("Lỗi khi cập nhật sản phẩm: " + ex.Message);
            }
        }

        public bool DeleteProduct(int productId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ProductID", SqlDbType.Int) { Value = productId }
                };

                DataTable result = DatabaseConnection.ExecuteQuery("DeleteProduct", CommandType.StoredProcedure, parameters);
                
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
                throw new Exception("Lỗi khi xóa sản phẩm: " + ex.Message);
            }
        }

        // Kiểm tra số lượng tồn kho có đủ không
        public bool IsStockAvailable(int productId, int requiredQuantity)
        {
            try
            {
                string query = "SELECT dbo.IsStockAvailable(@ProductID, @RequiredQuantity) as IsAvailable";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ProductID", SqlDbType.Int) { Value = productId },
                    new SqlParameter("@RequiredQuantity", SqlDbType.Int) { Value = requiredQuantity }
                };

                DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
                if (result.Rows.Count > 0)
                {
                    return Convert.ToBoolean(result.Rows[0]["IsAvailable"]);
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi kiểm tra tồn kho: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Lấy top sản phẩm bán chạy
        public DataTable GetTopSellingProducts(int topCount = 10)
        {
            try
            {
                string query = "SELECT * FROM dbo.GetTopSellingProducts(@TopCount)";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@TopCount", SqlDbType.Int) { Value = topCount }
                };

                return DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy top sản phẩm bán chạy: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        // Lấy báo cáo doanh thu theo sản phẩm
        public DataTable GetProductRevenueReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                string query = @"
                    SELECT 
                        p.ProductID,
                        p.ProductName,
                        p.Price,
                        SUM(sd.Quantity) as TotalSold,
                        SUM(sd.Quantity * sd.Price) as TotalRevenue
                    FROM Products p
                    INNER JOIN SaleDetails sd ON p.ProductID = sd.ProductID
                    INNER JOIN Sales s ON sd.SaleID = s.SaleID
                    WHERE s.SaleDate BETWEEN @StartDate AND @EndDate
                    GROUP BY p.ProductID, p.ProductName, p.Price
                    ORDER BY TotalRevenue DESC";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = startDate },
                    new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = endDate }
                };

                return DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy báo cáo doanh thu sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        

        // Lấy danh sách sản phẩm sắp hết hàng
        public DataTable GetLowStockProducts()
        {
            try
            {
                return DatabaseConnection.ExecuteQuery("SELECT * FROM LowStockProducts", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách sản phẩm sắp hết hàng: " + ex.Message);
            }
        }
    }
}
