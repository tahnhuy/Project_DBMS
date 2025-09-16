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
    public partial class ProductForm : Form
    {
        public ProductForm()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                DataTable dt = DatabaseConnection.ExecuteQuery("GetAllProducts", CommandType.StoredProcedure, null);
                dgv_Products.DataSource = dt;

                if (dgv_Products.Columns.Count > 0)
                {
                    dgv_Products.Columns["ProductID"].HeaderText = "Mã sản phẩm";
                    dgv_Products.Columns["ProductName"].HeaderText = "Tên sản phẩm";
                    dgv_Products.Columns["StockQuantity"].HeaderText = "Số lượng tồn";
                    dgv_Products.Columns["Price"].HeaderText = "Giá";
                    dgv_Products.Columns["Unit"].HeaderText = "Đơn vị";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu của hàng hóa: " + ex.Message);
            }
        }
    }
}
