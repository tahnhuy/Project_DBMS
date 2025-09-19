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
                return DatabaseConnection.ExecuteQuery("GetAllProducts", CommandType.StoredProcedure, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách sản phẩm: " + ex.Message);
            }
        }

        public DataTable GetProductByName(string productName)
        {
            try
            {
                string query = "select * from GetProductByName(@ProductName)";
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@ProductName", SqlDbType.NVarChar, 100) {Value = productName ?? (object)DBNull.Value}
                };
                    
                return DatabaseConnection.ExecuteQuery(query, CommandType.Text, para);
            }
            catch (Exception ex) 
            {
                throw new Exception("Lỗi khi lấy sản phẩm theo tên: " + ex.Message);
            }
        }

        public DataTable GetProductById(int productId)
        {
            try
            {
                string query = "select * from GetProductByID(@ProductID)";
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@ProductID", SqlDbType.Int) {Value = productId}
                };
                return DatabaseConnection.ExecuteQuery(query, CommandType.Text, para);
            } catch (Exception ex)
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
    }
}
