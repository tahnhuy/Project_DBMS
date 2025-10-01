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
        private DiscountRepository discountRepository;
        
        public ProductForm()
        {
            try
            {
                InitializeComponent();
                productRepository = new ProductRepository();
                discountRepository = new DiscountRepository();
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProducts()
        {
            try
            {
                DataTable dt = productRepository.GetAllProducts();
                if (dt != null)
                {
                    dgv_Products.DataSource = dt;
                    ConfigureDataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    {
                        var productIdColumn = dgv_Products.Columns["ProductID"];
                        if (productIdColumn != null) productIdColumn.HeaderText = "Mã sản phẩm";
                    }
                    if (dgv_Products.Columns.Contains("ProductName"))
                    {
                        var productNameColumn = dgv_Products.Columns["ProductName"];
                        if (productNameColumn != null) productNameColumn.HeaderText = "Tên sản phẩm";
                    }
                    if (dgv_Products.Columns.Contains("OriginalPrice"))
                    {
                        var originalPriceColumn = dgv_Products.Columns["OriginalPrice"];
                        if (originalPriceColumn != null)
                        {
                            originalPriceColumn.HeaderText = "Giá gốc";
                            originalPriceColumn.DefaultCellStyle.Format = "N0";
                        }
                    }
                    if (dgv_Products.Columns.Contains("DiscountedPrice"))
                    {
                        var discountedPriceColumn = dgv_Products.Columns["DiscountedPrice"];
                        if (discountedPriceColumn != null)
                        {
                            discountedPriceColumn.HeaderText = "Giá sau giảm";
                            discountedPriceColumn.DefaultCellStyle.Format = "N0";
                        }
                    }
                    if (dgv_Products.Columns.Contains("StockQuantity"))
                    {
                        var stockColumn = dgv_Products.Columns["StockQuantity"];
                        if (stockColumn != null) stockColumn.HeaderText = "Số lượng tồn";
                    }
                    if (dgv_Products.Columns.Contains("Unit"))
                    {
                        var unitColumn = dgv_Products.Columns["Unit"];
                        if (unitColumn != null) unitColumn.HeaderText = "Đơn vị";
                    }
                    if (dgv_Products.Columns.Contains("HasDiscount"))
                    {
                        var hasDiscountColumn = dgv_Products.Columns["HasDiscount"];
                        if (hasDiscountColumn != null) hasDiscountColumn.HeaderText = "Có giảm giá";
                    }
                    if (dgv_Products.Columns.Contains("DiscountType"))
                    {
                        var discountTypeColumn = dgv_Products.Columns["DiscountType"];
                        if (discountTypeColumn != null) discountTypeColumn.HeaderText = "Loại giảm giá";
                    }
                    if (dgv_Products.Columns.Contains("DiscountValue"))
                    {
                        var discountValueColumn = dgv_Products.Columns["DiscountValue"];
                        if (discountValueColumn != null) discountValueColumn.HeaderText = "Giá trị giảm";
                    }
                
                // Ẩn một số cột không cần thiết
                if (dgv_Products.Columns.Contains("DiscountStartDate"))
                    dgv_Products.Columns["DiscountStartDate"].Visible = false;
                if (dgv_Products.Columns.Contains("DiscountEndDate"))
                    dgv_Products.Columns["DiscountEndDate"].Visible = false;
                
                // Đổi màu cho những sản phẩm có giảm giá
                if (dgv_Products.Columns.Contains("HasDiscount") && dgv_Products.Columns.Contains("DiscountedPrice"))
                {
                    foreach (DataGridViewRow row in dgv_Products.Rows)
                    {
                        if (row.Cells["HasDiscount"].Value != null && 
                            Convert.ToBoolean(row.Cells["HasDiscount"].Value))
                        {
                            row.DefaultCellStyle.BackColor = Color.LightYellow;
                            row.Cells["DiscountedPrice"].Style.ForeColor = Color.Red;
                            row.Cells["DiscountedPrice"].Style.Font = new Font(dgv_Products.Font, FontStyle.Bold);
                        }
                    }
                }
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thiết lập DataGridView: {ex.Message}\n\nChi tiết: {ex.StackTrace}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    dt = productRepository.GetAllProducts();
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
                    dt = productRepository.GetAllProducts();
                }
                else if (int.TryParse(searchText, out int productId))
                {
                    dt = productRepository.GetProductById(productId);
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập ID hợp lệ (số nguyên)", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
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
                        LoadProducts(); // Reload the product list
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
                        LoadProducts(); // Reload the product list
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
