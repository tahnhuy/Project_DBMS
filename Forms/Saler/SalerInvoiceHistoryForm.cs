using System;
using System.Data;
using System.Windows.Forms;
using Sale_Management.DatabaseAccess;

namespace Sale_Management.Forms
{
    public partial class SalerInvoiceHistoryForm : Form
    {
        public SalerInvoiceHistoryForm()
        {
            InitializeComponent();
            LoadInvoiceHistory();
        }

        private void LoadInvoiceHistory()
        {
            try
            {
                SaleRepository saleRepo = new SaleRepository();
                DataTable dt = saleRepo.GetSalesSummary();
                
                dgv_InvoiceHistory.DataSource = dt;
                
                // Định dạng cột
                if (dgv_InvoiceHistory.Columns.Count > 0)
                {
                    dgv_InvoiceHistory.Columns["SaleID"].HeaderText = "Mã HĐ";
                    dgv_InvoiceHistory.Columns["CustomerName"].HeaderText = "Khách hàng";
                    dgv_InvoiceHistory.Columns["SaleDate"].HeaderText = "Ngày bán";
                    dgv_InvoiceHistory.Columns["TotalAmount"].HeaderText = "Tổng tiền";
                    dgv_InvoiceHistory.Columns["PaymentMethod"].HeaderText = "Phương thức TT";
                    
                    // Định dạng cột tổng tiền
                    if (dgv_InvoiceHistory.Columns["TotalAmount"] != null)
                    {
                        dgv_InvoiceHistory.Columns["TotalAmount"].DefaultCellStyle.Format = "N0";
                    }
                    
                    // Định dạng cột ngày
                    if (dgv_InvoiceHistory.Columns["SaleDate"] != null)
                    {
                        dgv_InvoiceHistory.Columns["SaleDate"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải lịch sử hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            LoadInvoiceHistory();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgv_InvoiceHistory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    int saleId = Convert.ToInt32(dgv_InvoiceHistory.Rows[e.RowIndex].Cells["SaleID"].Value);
                    ShowInvoiceDetails(saleId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xem chi tiết hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ShowInvoiceDetails(int saleId)
        {
            try
            {
                SaleRepository saleRepo = new SaleRepository();
                DataTable saleInfo = saleRepo.GetSaleById(saleId);
                DataTable saleDetails = saleRepo.GetSaleDetails(saleId);

                if (saleInfo.Rows.Count > 0 && saleDetails.Rows.Count > 0)
                {
                    string message = $"=== CHI TIẾT HÓA ĐƠN #{saleId} ===\n\n";
                    
                    // Thông tin hóa đơn
                    DataRow sale = saleInfo.Rows[0];
                    message += $"Khách hàng: {sale["CustomerName"]}\n";
                    message += $"Ngày bán: {Convert.ToDateTime(sale["SaleDate"]):dd/MM/yyyy HH:mm}\n";
                    message += $"Phương thức TT: {sale["PaymentMethod"]}\n";
                    message += $"Tổng tiền: {Convert.ToDecimal(sale["TotalAmount"]):N0} VNĐ\n\n";
                    
                    // Chi tiết sản phẩm
                    message += "=== CHI TIẾT SẢN PHẨM ===\n";
                    decimal total = 0;
                    foreach (DataRow detail in saleDetails.Rows)
                    {
                        string productName = detail["ProductName"].ToString();
                        int quantity = Convert.ToInt32(detail["Quantity"]);
                        decimal price = Convert.ToDecimal(detail["SalePrice"]);
                        decimal lineTotal = Convert.ToDecimal(detail["LineTotal"]);
                        
                        message += $"{productName} x {quantity} = {lineTotal:N0} VNĐ\n";
                        total += lineTotal;
                    }
                    
                    message += $"\nTỔNG CỘNG: {total:N0} VNĐ";

                    MessageBox.Show(message, $"Chi tiết hóa đơn #{saleId}", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị chi tiết: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
