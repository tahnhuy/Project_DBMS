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
    public partial class ProductEditForm : Form
    {
        private ProductRepository productRepository;
        private bool isEditMode;
        private int productId;
        
        public ProductEditForm(bool isEditMode = false, int productId = 0)
        {
            InitializeComponent();
            this.isEditMode = isEditMode;
            this.productId = productId;
            productRepository = new ProductRepository();
            
            if (isEditMode)
            {
                this.Text = "Sửa sản phẩm";
                LoadProductData();
            }
            else
            {
                this.Text = "Thêm sản phẩm mới";
            }
        }

        private void LoadProductData()
        {
            try
            {
                DataTable dt = productRepository.GetProductById(productId);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txt_ProductName.Text = row["ProductName"].ToString();
                    txt_Price.Text = row["Price"].ToString();
                    txt_StockQuantity.Text = row["StockQuantity"].ToString();
                    txt_Unit.Text = row["Unit"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            // Kiểm tra tên sản phẩm
            if (string.IsNullOrWhiteSpace(txt_ProductName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_ProductName.Focus();
                return false;
            }

            // Kiểm tra giá
            if (!decimal.TryParse(txt_Price.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Vui lòng nhập giá hợp lệ (lớn hơn 0)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_Price.Focus();
                return false;
            }

            // Kiểm tra số lượng tồn
            if (!int.TryParse(txt_StockQuantity.Text, out int stockQuantity) || stockQuantity < 0)
            {
                MessageBox.Show("Vui lòng nhập số lượng tồn hợp lệ (lớn hơn hoặc bằng 0)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_StockQuantity.Focus();
                return false;
            }

            // Kiểm tra đơn vị
            if (string.IsNullOrWhiteSpace(txt_Unit.Text))
            {
                MessageBox.Show("Vui lòng nhập đơn vị!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_Unit.Focus();
                return false;
            }

            return true;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                string productName = txt_ProductName.Text.Trim();
                decimal price = decimal.Parse(txt_Price.Text);
                int stockQuantity = int.Parse(txt_StockQuantity.Text);
                string unit = txt_Unit.Text.Trim();

                if (isEditMode)
                {
                    // Cập nhật sản phẩm
                    bool success = productRepository.UpdateProduct(productId, productName, price, stockQuantity, unit);
                    if (success)
                    {
                        MessageBox.Show("Cập nhật sản phẩm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    // Thêm sản phẩm mới
                    bool success = productRepository.AddProduct(productName, price, stockQuantity, unit);
                    if (success)
                    {
                        MessageBox.Show("Thêm sản phẩm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Chỉ cho phép nhập số cho giá và số lượng
        private void txt_Price_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Chỉ cho phép một dấu chấm
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void txt_StockQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
