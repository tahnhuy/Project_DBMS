using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sale_Management.DatabaseAccess;

namespace Sale_Management.Forms
{
    public partial class SalerInvoiceForm : Form
    {
        private List<InvoiceItem> invoiceItems = new List<InvoiceItem>();
        private string currentUsername;

        public SalerInvoiceForm(string username = null)
        {
            InitializeComponent();
            currentUsername = username ?? "system";
            LoadProducts();
            LoadCustomers();
            LoadPaymentMethods();
        }

        private decimal GetDiscountedPrice(int productId, decimal originalPrice)
        {
            try
            {
                string query = "SELECT dbo.GetDiscountedPrice(@ProductID, @OriginalPrice) as DiscountedPrice";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@ProductID", SqlDbType.Int) { Value = productId },
                    new SqlParameter("@OriginalPrice", SqlDbType.Decimal) { Value = originalPrice }
                };

                DataTable result = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
                if (result.Rows.Count > 0)
                {
                    return Convert.ToDecimal(result.Rows[0]["DiscountedPrice"]);
                }
                return originalPrice; // Fallback to original price if function fails
            }
            catch (Exception ex)
            {
                // Log error and return original price
                System.Diagnostics.Debug.WriteLine($"Error getting discounted price: {ex.Message}");
                return originalPrice;
            }
        }

        private void LoadProducts()
        {
            try
            {
                ProductRepository productRepo = new ProductRepository();
                DataTable dt = productRepo.GetAllProducts();
                
                // Tạo cột hiển thị kết hợp tên sản phẩm và đơn vị
                dt.Columns.Add("ProductDisplayName", typeof(string), "ProductName + ' (' + Unit + ')'");
                
                cmb_Product.DataSource = dt;
                cmb_Product.DisplayMember = "ProductDisplayName";
                cmb_Product.ValueMember = "ProductID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCustomers()
        {
            try
            {
                CustomerRepository customerRepo = new CustomerRepository();
                DataTable dt = customerRepo.GetAllCustomers();
                cmb_Customer.DataSource = dt;
                cmb_Customer.DisplayMember = "CustomerName";
                cmb_Customer.ValueMember = "CustomerID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPaymentMethods()
        {
            try
            {
                // Thêm các phương thức thanh toán phổ biến
                cmb_PaymentMethod.Items.Add("Tiền mặt");
                cmb_PaymentMethod.Items.Add("Chuyển khoản");
                cmb_PaymentMethod.Items.Add("Thẻ tín dụng");
                cmb_PaymentMethod.Items.Add("Ví điện tử");
                cmb_PaymentMethod.Items.Add("Thanh toán khi nhận hàng");
                
                // Chọn mặc định là "Tiền mặt"
                cmb_PaymentMethod.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải phương thức thanh toán: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_AddProduct_Click(object sender, EventArgs e)
        {
            if (cmb_Product.SelectedValue == null || string.IsNullOrEmpty(txt_Quantity.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm và nhập số lượng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txt_Quantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Vui lòng nhập số lượng hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ProductRepository productRepo = new ProductRepository();
                DataTable dt = productRepo.GetProductById(int.Parse(cmb_Product.SelectedValue.ToString()));
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    string productId = row["ProductID"].ToString();
                    string productName = row["ProductName"].ToString();
                    decimal originalPrice = decimal.Parse(row["Price"].ToString());
                    int availableQuantity = int.Parse(row["StockQuantity"].ToString());
                    string unit = row["Unit"].ToString();

                    // Tính giá giảm sử dụng function GetDiscountedPrice
                    decimal discountedPrice = GetDiscountedPrice(int.Parse(productId), originalPrice);

                    // Sử dụng function để kiểm tra tồn kho
                    ProductRepository productRepo2 = new ProductRepository();
                    if (!productRepo2.IsStockAvailable(int.Parse(productId), quantity))
                    {
                        MessageBox.Show($"Số lượng không đủ. Còn lại: {availableQuantity} {unit}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Kiểm tra xem sản phẩm đã có trong hóa đơn chưa
                    var existingItem = invoiceItems.FirstOrDefault(x => x.ProductId == productId);
                    if (existingItem != null)
                    {
                        existingItem.Quantity += quantity;
                    }
                    else
                    {
                        invoiceItems.Add(new InvoiceItem
                        {
                            ProductId = productId,
                            ProductName = productName,
                            Price = discountedPrice, // Sử dụng giá đã giảm
                            OriginalPrice = originalPrice, // Lưu giá gốc để hiển thị
                            Quantity = quantity,
                            Unit = unit
                        });
                    }

                    UpdateInvoiceGrid();
                    CalculateTotal();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateInvoiceGrid()
        {
            dgv_InvoiceItems.DataSource = null;
            dgv_InvoiceItems.DataSource = invoiceItems.Select(x => new
            {
                Tên_sản_phẩm = x.ProductName,
                Giá_gốc = x.OriginalPrice.ToString("N0"),
                Giá_giảm = x.Price.ToString("N0"),
                Số_lượng = x.Quantity,
                Đơn_vị = x.Unit,
                Thành_tiền = (x.Price * x.Quantity).ToString("N0")
            }).ToList();
        }

        private void CalculateTotal()
        {
            decimal total = invoiceItems.Sum(x => x.Price * x.Quantity);
            lbl_Total.Text = $"Tổng cộng: {total:N0} VNĐ";
        }

        private void btn_CreateInvoice_Click(object sender, EventArgs e)
        {
            if (cmb_Customer.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (invoiceItems.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string customerId = cmb_Customer.SelectedValue.ToString();
                string paymentMethod = cmb_PaymentMethod.SelectedItem?.ToString() ?? "Tiền mặt";

                // Convert InvoiceItem to SaleDetailItem
                List<SaleDetailItem> saleDetails = new List<SaleDetailItem>();
                foreach (var item in invoiceItems)
                {
                    saleDetails.Add(new SaleDetailItem(
                        int.Parse(item.ProductId), 
                        item.Quantity, 
                        item.Price, 
                        item.ProductName, 
                        item.Unit
                    ));
                }

                // Tạo hóa đơn với chi tiết trong một transaction
                SaleRepository saleRepo = new SaleRepository();
                int saleId = saleRepo.CreateSaleWithDetails(
                    int.Parse(customerId), 
                    paymentMethod, 
                    currentUsername, 
                    saleDetails
                );
                
                if (saleId > 0)
                {
                    MessageBox.Show($"Tạo hóa đơn thành công! Mã hóa đơn: {saleId}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Tạo hóa đơn thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            invoiceItems.Clear();
            dgv_InvoiceItems.DataSource = null;
            lbl_Total.Text = "Tổng cộng: 0 VNĐ";
            txt_Quantity.Text = "";
            cmb_Customer.SelectedIndex = -1;
            cmb_Product.SelectedIndex = -1;
            cmb_PaymentMethod.SelectedIndex = 0; // Reset về "Tiền mặt"
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btn_RemoveSelected_Click(object sender, EventArgs e)
        {
            if (dgv_InvoiceItems.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn hàng cần xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Lấy tên sản phẩm của hàng được chọn
                string productName = dgv_InvoiceItems.SelectedRows[0].Cells["Tên_sản_phẩm"].Value.ToString();
                
                // Xác nhận xóa
                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa sản phẩm '{productName}'?", "Xác nhận xóa", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    // Tìm và xóa item khỏi danh sách
                    var itemToRemove = invoiceItems.FirstOrDefault(x => x.ProductName == productName);
                    if (itemToRemove != null)
                    {
                        invoiceItems.Remove(itemToRemove);
                        UpdateInvoiceGrid();
                        CalculateTotal();
                        MessageBox.Show("Đã xóa sản phẩm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_ViewHistory_Click(object sender, EventArgs e)
        {
            try
            {
                SalerInvoiceHistoryForm historyForm = new SalerInvoiceHistoryForm();
                historyForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở lịch sử hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public class InvoiceItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
    }
}
