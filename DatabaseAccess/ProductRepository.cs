using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace Sale_Management.DatabaseAccess
{
    public class ProductRepository
    {
        public DataTable GetAllProducts()
        {
            try
            {
                return DatabaseConnection.ExecuteQuery("SELECT * FROM dbo.fnProducts_All()", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách hàng hóa: " + ex.Message);
            }
        }

        public DataTable GetProductByName(string productName)
        {
            try
            {
                string query = "SELECT * FROM dbo.GetProductByName(@ProductName)";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ProductName", SqlDbType.NVarChar, 100) { Value = productName ?? string.Empty }
                };
                return DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm sản phẩm theo tên: " + ex.Message);
            }
        }

        public DataTable GetProductById(int productId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ProductID", SqlDbType.Int) { Value = productId }
                };
                string query = "SELECT * FROM dbo.fnProducts_ByID(@ProductID)";
                return DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy sản phẩm theo ID: " + ex.Message);
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
                throw new Exception("Lỗi khi kiểm tra tồn kho: " + ex.Message);
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
                throw new Exception("Lỗi khi xóa sản phẩm: " + ex.Message);
            }
        }

        // Lấy danh sách sản phẩm đã xóa (IsDeleted = 1)
        public DataTable GetDeletedProducts()
        {
            try
            {
                string query = "SELECT ProductID, ProductName, Price, StockQuantity, Unit FROM dbo.Products WHERE IsDeleted = 1";
                return DatabaseConnection.ExecuteQuery(query, CommandType.Text, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách sản phẩm đã xóa: " + ex.Message);
            }
        }

        // Khôi phục sản phẩm bằng Stored Procedure RestoreProduct
        public bool RestoreProduct(int productId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ProductID", SqlDbType.Int) { Value = productId }
                };

                DataTable result = DatabaseConnection.ExecuteQuery("RestoreProduct", CommandType.StoredProcedure, parameters);

                if (result.Rows.Count > 0)
                {
                    string resultStatus = result.Rows[0]["Result"].ToString();
                    string message = result.Rows[0]["Message"].ToString();
                    if (resultStatus == "SUCCESS" || resultStatus == "WARNING")
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
                throw new Exception("Lỗi khi khôi phục sản phẩm: " + ex.Message);
            }
        }
    }
}
