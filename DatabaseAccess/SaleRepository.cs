using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sale_Management.DatabaseAccess
{
    public class SaleRepository
    {
        public DataTable GetSaleById(int saleId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@SaleID", SqlDbType.Int) { Value = saleId }
                };
                return DatabaseConnection.ExecuteQuery("SELECT * FROM dbo.fnSales_ByID(@SaleID)", CommandType.Text, parameters);
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
                return DatabaseConnection.ExecuteQuery("SELECT * FROM dbo.fnSaleDetails_BySaleID(@SaleID)", CommandType.Text, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy chi tiết hóa đơn: " + ex.Message);
            }
        }

        public int CreateSaleWithDetails(int? customerId, string paymentMethod, string createdBy, List<SaleDetailItem> saleDetails)
        {
            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Step 0: Calculate total amount first
                        decimal totalAmount = 0;
                        foreach (var detail in saleDetails)
                        {
                            // Check stock availability
                            if (!IsStockAvailable(detail.ProductId, detail.Quantity, connection, transaction))
                            {
                                throw new Exception($"Không đủ tồn kho cho sản phẩm ID: {detail.ProductId}");
                            }
                            totalAmount += detail.Quantity * detail.SalePrice;
                        }

                        // Step 1: Create sale header with calculated total amount
                        SqlParameter[] saleParameters = new SqlParameter[]
                        {
                            new SqlParameter("@CustomerID", SqlDbType.Int) { Value = customerId ?? (object)DBNull.Value },
                            new SqlParameter("@TotalAmount", SqlDbType.Decimal) { Value = totalAmount },
                            new SqlParameter("@PaymentMethod", SqlDbType.NVarChar, 50) { Value = paymentMethod ?? (object)DBNull.Value },
                            new SqlParameter("@CreatedBy", SqlDbType.NVarChar, 50) { Value = createdBy ?? "system" }
                        };

                        DataTable saleResult = DatabaseConnection.ExecuteQuery("CreateSale", CommandType.StoredProcedure, saleParameters, connection, transaction);
                        
                        if (saleResult.Rows.Count == 0 || saleResult.Rows[0]["Result"].ToString() != "SUCCESS")
                        {
                            throw new Exception(saleResult.Rows.Count > 0 ? saleResult.Rows[0]["Message"].ToString() : "Lỗi khi tạo hóa đơn");
                        }

                        int saleId = Convert.ToInt32(saleResult.Rows[0]["SaleID"]);

                        // Step 2: Add sale details
                        foreach (var detail in saleDetails)
                        {
                            SqlParameter[] detailParameters = new SqlParameter[]
                            {
                                new SqlParameter("@SaleID", SqlDbType.Int) { Value = saleId },
                                new SqlParameter("@ProductID", SqlDbType.Int) { Value = detail.ProductId },
                                new SqlParameter("@Quantity", SqlDbType.Int) { Value = detail.Quantity },
                                new SqlParameter("@SalePrice", SqlDbType.Decimal) { Value = detail.SalePrice }
                            };

                            DataTable detailResult = DatabaseConnection.ExecuteQuery("AddSaleDetail", CommandType.StoredProcedure, detailParameters, connection, transaction);
                            
                            if (detailResult.Rows.Count == 0 || detailResult.Rows[0]["Result"].ToString() != "SUCCESS")
                            {
                                throw new Exception(detailResult.Rows.Count > 0 ? detailResult.Rows[0]["Message"].ToString() : "Lỗi khi thêm chi tiết hóa đơn");
                            }
                        }

                        // Step 3: Update total amount (if needed)
                        SqlParameter[] updateParameters = new SqlParameter[]
                        {
                            new SqlParameter("@SaleID", SqlDbType.Int) { Value = saleId },
                            new SqlParameter("@CustomerID", SqlDbType.Int) { Value = customerId ?? (object)DBNull.Value },
                            new SqlParameter("@TotalAmount", SqlDbType.Decimal) { Value = totalAmount },
                            new SqlParameter("@PaymentMethod", SqlDbType.NVarChar, 50) { Value = paymentMethod ?? (object)DBNull.Value }
                        };

                        DataTable updateResult = DatabaseConnection.ExecuteQuery("UpdateSale", CommandType.StoredProcedure, updateParameters, connection, transaction);
                        
                        if (updateResult.Rows.Count == 0 || updateResult.Rows[0]["Result"].ToString() != "SUCCESS")
                        {
                            throw new Exception(updateResult.Rows.Count > 0 ? updateResult.Rows[0]["Message"].ToString() : "Lỗi khi cập nhật tổng tiền hóa đơn");
                        }

                        transaction.Commit();
                        return saleId;
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            if (transaction != null && transaction.Connection != null)
                            {
                                transaction.Rollback();
                            }
                        }
                        catch (Exception rollbackEx)
                        {
                            // Log rollback error but don't throw it
                            System.Diagnostics.Debug.WriteLine("Rollback error: " + rollbackEx.Message);
                        }
                        throw new Exception("Lỗi khi tạo hóa đơn với chi tiết: " + ex.Message);
                    }
                }
            }
        }

        private bool IsStockAvailable(int productId, int requiredQuantity, SqlConnection connection, SqlTransaction transaction)
        {
            try
            {
                string query = "SELECT dbo.IsStockAvailable(@ProductID, @RequiredQuantity) as IsAvailable";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ProductID", SqlDbType.Int) { Value = productId },
                    new SqlParameter("@RequiredQuantity", SqlDbType.Int) { Value = requiredQuantity }
                };

                DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters, connection, transaction);
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
    }
}

