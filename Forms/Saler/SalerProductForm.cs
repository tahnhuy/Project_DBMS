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
                ConfigureDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureDataGridView()
        {
            try
            {
                if (dgv_Products == null) return;
                
                if (dgv_Products.Columns != null && dgv_Products.Columns.Count > 0)
                {
                    // Kiểm tra từng cột có tồn tại không trước khi thiết lập
                    if (dgv_Products.Columns.Contains("ProductID"))
                        dgv_Products.Columns["ProductID"].HeaderText = "Mã sản phẩm";
                    if (dgv_Products.Columns.Contains("ProductName"))
                        dgv_Products.Columns["ProductName"].HeaderText = "Tên sản phẩm";
                    if (dgv_Products.Columns.Contains("Price"))
                    {
                        dgv_Products.Columns["Price"].HeaderText = "Giá";
                        dgv_Products.Columns["Price"].DefaultCellStyle.Format = "N0";
                    }
                    if (dgv_Products.Columns.Contains("StockQuantity"))
                        dgv_Products.Columns["StockQuantity"].HeaderText = "Số lượng tồn";
                    if (dgv_Products.Columns.Contains("Unit"))
                        dgv_Products.Columns["Unit"].HeaderText = "Đơn vị";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thiết lập DataGridView: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            LoadProducts();
        }

        // Tìm kiếm sản phẩm theo tên
        private void SearchProductsByName(string searchText)
        {
            try
            {
                ProductRepository productRepo = new ProductRepository();
                DataTable dt;
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    dt = productRepo.GetAllProducts();
                }
                else
                {
                    dt = productRepo.GetProductByName(searchText);
                }
                
                dgv_Products.DataSource = dt;
                ConfigureDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm sản phẩm: " + ex.Message);
            }
        }

        // Tìm kiếm sản phẩm theo ID
        private void SearchProductsByID(string searchText)
        {
            try
            {
                ProductRepository productRepo = new ProductRepository();
                DataTable dt;
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    dt = productRepo.GetAllProducts();
                }
                else
                {
                    if (int.TryParse(searchText, out int productId))
                    {
                        dt = productRepo.GetProductById(productId);
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng nhập ID hợp lệ (số nguyên)", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                dgv_Products.DataSource = dt;
                ConfigureDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm sản phẩm: " + ex.Message);
            }
        }

        private void txt_nameSearch_TextChanged(object sender, EventArgs e)
        {
            SearchProductsByName(txt_nameSearch.Text.Trim());

            if (txt_nameSearch.Focused)
            {
                txt_idSearch.Text = string.Empty;
            }
        }

        private void txt_idSearch_TextChanged(object sender, EventArgs e)
        {
            SearchProductsByID(txt_idSearch.Text);

            if (txt_idSearch.Focused)
            {
                txt_nameSearch.Text = string.Empty;
            }
        }

        //private void btn_Edit_Click(object sender, EventArgs e)
        //{
        //    if (dgv_Products.SelectedRows.Count > 0)
        //    {
        //        string productId = dgv_Products.SelectedRows[0].Cells[0].Value.ToString();
        //        SalerProductEditForm editForm = new SalerProductEditForm(productId);
        //        editForm.ShowDialog();
        //        LoadProducts(); // Refresh sau khi edit
        //    }
        //    else
        //    {
        //        MessageBox.Show("Vui lòng chọn sản phẩm cần chỉnh sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }
        //}
    }
}
