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
        private ProductRepository productRepository;
        
        public ProductForm()
        {
            InitializeComponent();
            productRepository = new ProductRepository();
            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                DataTable dt = DatabaseConnection.ExecuteQuery("GetAllProducts", CommandType.StoredProcedure, null);
                dgv_Products.DataSource = dt;
                ConfigureDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu của hàng hóa: " + ex.Message);
            }
        }
        
        private void ConfigureDataGridView()
        {
            if (dgv_Products.Columns.Count > 0)
            {
                dgv_Products.Columns["ProductID"].HeaderText = "Mã sản phẩm";
                dgv_Products.Columns["ProductName"].HeaderText = "Tên sản phẩm";
                dgv_Products.Columns["StockQuantity"].HeaderText = "Số lượng tồn";
                dgv_Products.Columns["Price"].HeaderText = "Giá";
                dgv_Products.Columns["Unit"].HeaderText = "Đơn vị";
            }
        }
        
        //  Tim kiem san pham theo ten bang function 
        private void SearchProductsByName(string searchText)
        {
            try
            {
                DataTable dt;
                if (string.IsNullOrWhiteSpace(searchText))
                { 
                    dt = DatabaseConnection.ExecuteQuery("GetAllProducts", CommandType.StoredProcedure, null);
                }
                else
                {
                    dt = productRepository.GetProductByName(searchText);
                }
                
                dgv_Products.DataSource = dt;
                ConfigureDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm sản phẩm: " + ex.Message);
            }
        }
        
        private void txt_SearchProduct_TextChanged(object sender, EventArgs e)
        {
            SearchProductsByName(txt_nameSearch.Text.Trim());

            if (txt_nameSearch.Focused)
            {
                txt_idSearch.Text = string.Empty;
            }
        }

        // Tìm kiếm sản phẩm theo ID bằng function
        private void SearchProductsByID(string searchText)
        {
            try
            {
                DataTable dt;
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    dt = DatabaseConnection.ExecuteQuery("GetAllProducts", CommandType.StoredProcedure, null);
                }
                else
                {
                    dt = productRepository.GetProductById(int.Parse(searchText));
                }

                dgv_Products.DataSource = dt;
                ConfigureDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm sản phẩm: " + ex.Message);
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

        private void btn_Add_Click(object sender, EventArgs e)
        {
            ProductEditForm editForm = new ProductEditForm(false);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadProducts(); // Tải lại danh sách sản phẩm
            }
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            if (dgv_Products.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dgv_Products.SelectedRows[0];
            int productId = Convert.ToInt32(selectedRow.Cells["ProductID"].Value);

            ProductEditForm editForm = new ProductEditForm(true, productId);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadProducts(); // Tải lại danh sách sản phẩm
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (dgv_Products.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dgv_Products.SelectedRows[0];
            int productId = Convert.ToInt32(selectedRow.Cells["ProductID"].Value);
            string productName = selectedRow.Cells["ProductName"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa sản phẩm '{productName}' không?\n\nLưu ý: Không thể xóa sản phẩm đã có trong hóa đơn.",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    bool success = productRepository.DeleteProduct(productId);
                    if (success)
                    {
                        MessageBox.Show("Xóa sản phẩm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadProducts(); // Tải lại danh sách sản phẩm
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btn_AddProduct_Click(object sender, EventArgs e)
        {
            ProductEditForm editForm = new ProductEditForm(false);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadProducts(); // Tải lại danh sách sản phẩm
            }
        }

        private void btn_DeleteProduct_Click(object sender, EventArgs e)
        {
            if (dgv_Products.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dgv_Products.SelectedRows[0];
            int productId = Convert.ToInt32(selectedRow.Cells["ProductID"].Value);
            string productName = selectedRow.Cells["ProductName"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa sản phẩm '{productName}' không?\n\nLưu ý: Không thể xóa sản phẩm đã có trong hóa đơn.",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    bool success = productRepository.DeleteProduct(productId);
                    if (success)
                    {
                        MessageBox.Show("Xóa sản phẩm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadProducts(); // Tải lại danh sách sản phẩm
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btn_EditProduct_Click(object sender, EventArgs e)
        {
            if (dgv_Products.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dgv_Products.SelectedRows[0];
            int productId = Convert.ToInt32(selectedRow.Cells["ProductID"].Value);

            ProductEditForm editForm = new ProductEditForm(true, productId);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadProducts(); // Tải lại danh sách sản phẩm
            }
        }
    }
}
