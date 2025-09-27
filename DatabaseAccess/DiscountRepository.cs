using System;
using System.Data;
using System.Data.SqlClient;

namespace Sale_Management.DatabaseAccess
{
    public class DiscountRepository
    {
        public DataTable GetActiveDiscounts()
        {
            try
            {
                return DatabaseConnection.ExecuteQuery("GetActiveDiscounts", CommandType.StoredProcedure, null);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách chương trình giảm giá đang hoạt động: " + ex.Message);
            }
        }

        public DataTable GetDiscountsByProduct(int productId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ProductID", SqlDbType.Int) { Value = productId }
                };
                return DatabaseConnection.ExecuteQuery("GetDiscountsByProduct", CommandType.StoredProcedure, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy chương trình giảm giá theo sản phẩm: " + ex.Message);
            }
        }


        public bool AddDiscount(int productId, string discountType, decimal discountValue, 
                              DateTime startDate, DateTime endDate, bool isActive, string createdBy)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ProductID", SqlDbType.Int) { Value = productId },
                    new SqlParameter("@DiscountType", SqlDbType.NVarChar, 20) { Value = discountType },
                    new SqlParameter("@DiscountValue", SqlDbType.Decimal) { Value = discountValue },
                    new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = startDate },
                    new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = endDate },
                    new SqlParameter("@IsActive", SqlDbType.Bit) { Value = isActive },
                    new SqlParameter("@CreatedBy", SqlDbType.NVarChar, 50) { Value = createdBy }
                };

                DataTable result = DatabaseConnection.ExecuteQuery("AddDiscount", CommandType.StoredProcedure, parameters);

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
                throw new Exception("Lỗi khi thêm chương trình giảm giá: " + ex.Message);
            }
        }

        public bool UpdateDiscount(int discountId, int productId, string discountType, decimal discountValue,
                                 DateTime startDate, DateTime endDate, bool isActive)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@DiscountID", SqlDbType.Int) { Value = discountId },
                    new SqlParameter("@ProductID", SqlDbType.Int) { Value = productId },
                    new SqlParameter("@DiscountType", SqlDbType.NVarChar, 20) { Value = discountType },
                    new SqlParameter("@DiscountValue", SqlDbType.Decimal) { Value = discountValue },
                    new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = startDate },
                    new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = endDate },
                    new SqlParameter("@IsActive", SqlDbType.Bit) { Value = isActive }
                };

                DataTable result = DatabaseConnection.ExecuteQuery("UpdateDiscount", CommandType.StoredProcedure, parameters);

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
                throw new Exception("Lỗi khi cập nhật chương trình giảm giá: " + ex.Message);
            }
        }

        public decimal GetDiscountedPrice(int productId, decimal originalPrice)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ProductID", SqlDbType.Int) { Value = productId },
                    new SqlParameter("@OriginalPrice", SqlDbType.Decimal) { Value = originalPrice }
                };

                string query = "SELECT dbo.GetDiscountedPrice(@ProductID, @OriginalPrice) as DiscountedPrice";
                DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);

                if (result.Rows.Count > 0)
                {
                    return Convert.ToDecimal(result.Rows[0]["DiscountedPrice"]);
                }
                return originalPrice;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính giá sau giảm: " + ex.Message);
            }
        }

        public bool DeleteDiscount(int discountId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@DiscountID", SqlDbType.Int) { Value = discountId }
                };

                DataTable result = DatabaseConnection.ExecuteQuery("DeleteDiscount", CommandType.StoredProcedure, parameters);
                
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
                throw new Exception("Lỗi khi xóa chương trình giảm giá: " + ex.Message);
            }
        }
    }
}
