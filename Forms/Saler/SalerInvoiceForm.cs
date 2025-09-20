using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        public SalerInvoiceForm()
        {
            InitializeComponent();
            LoadProducts();
            LoadCustomers();
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
                    decimal price = decimal.Parse(row["Price"].ToString());
                    int availableQuantity = int.Parse(row["StockQuantity"].ToString());
                    string unit = row["Unit"].ToString();

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
                            Price = price,
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
                Giá = x.Price.ToString("N0"),
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
                decimal totalAmount = invoiceItems.Sum(x => x.Price * x.Quantity);

                // Tạo hóa đơn
                SaleRepository saleRepo = new SaleRepository();
                int saleId = saleRepo.CreateSale(int.Parse(customerId), totalAmount, "Tiền mặt");
                
                if (saleId > 0)
                {
                    // Thêm chi tiết hóa đơn
                    bool success = true;
                    foreach (var item in invoiceItems)
                    {
                        success &= saleRepo.AddSaleDetail(saleId, int.Parse(item.ProductId), item.Quantity, item.Price);
                    }
                
                    if (success)
                    {
                        MessageBox.Show("Tạo hóa đơn thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Tạo hóa đơn thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
    }

    public class InvoiceItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
    }
}
