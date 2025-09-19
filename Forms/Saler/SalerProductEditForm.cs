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
    public partial class SalerProductEditForm : Form
    {
        private string productId;

        public SalerProductEditForm(string productId)
        {
            InitializeComponent();
            this.productId = productId;
            LoadProductData();
        }

        private void LoadProductData()
        {
            try
            {
                ProductRepository productRepo = new ProductRepository();
                DataTable dt = productRepo.GetProductById(int.Parse(productId));
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txt_ProductId.Text = row["ProductID"].ToString();
                    txt_ProductName.Text = row["ProductName"].ToString();
                    txt_Price.Text = row["Price"].ToString();
                    txt_Quantity.Text = row["Quantity"].ToString();
                    txt_Description.Text = row["Description"].ToString();
                    
                    txt_ProductId.ReadOnly = true; // Không cho sửa mã sản phẩm
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    string productName = txt_ProductName.Text.Trim();
                    decimal price = decimal.Parse(txt_Price.Text);
                    int quantity = int.Parse(txt_Quantity.Text);
                    string description = txt_Description.Text.Trim();

                    ProductRepository productRepo = new ProductRepository();
                    bool success = productRepo.UpdateProduct(int.Parse(productId), productName, price, quantity, "cái");
                    
                    if (success)
                    {
                        MessageBox.Show("Cập nhật sản phẩm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật sản phẩm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txt_ProductName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_ProductName.Focus();
                return false;
            }

            if (!decimal.TryParse(txt_Price.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Vui lòng nhập giá hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_Price.Focus();
                return false;
            }

            if (!int.TryParse(txt_Quantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Vui lòng nhập số lượng hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_Quantity.Focus();
                return false;
            }

            return true;
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
