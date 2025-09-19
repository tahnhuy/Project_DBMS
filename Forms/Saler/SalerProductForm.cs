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
    public partial class SalerProductForm : Form
    {
        public SalerProductForm()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                ProductRepository productRepo = new ProductRepository();
                DataTable dt = productRepo.GetAllProducts();
                dgv_Products.DataSource = dt;
                
                // Định dạng cột
                if (dgv_Products.Columns.Count > 0)
                {
                    dgv_Products.Columns[0].HeaderText = "Mã sản phẩm";
                    dgv_Products.Columns[1].HeaderText = "Tên sản phẩm";
                    dgv_Products.Columns[2].HeaderText = "Giá";
                    dgv_Products.Columns[3].HeaderText = "Số lượng";
                    dgv_Products.Columns[4].HeaderText = "Mô tả";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            if (dgv_Products.SelectedRows.Count > 0)
            {
                string productId = dgv_Products.SelectedRows[0].Cells[0].Value.ToString();
                SalerProductEditForm editForm = new SalerProductEditForm(productId);
                editForm.ShowDialog();
                LoadProducts(); // Refresh sau khi edit
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần chỉnh sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
