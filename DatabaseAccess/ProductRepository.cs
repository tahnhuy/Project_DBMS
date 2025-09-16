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
    }
}
