using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sale_Management.DatabaseAccess;

namespace Sale_Management.Forms.Manager
{
    public class ProductRestoreForm : Form
    {
        private DataGridView dgvDeletedProducts;
        private Button btnRestore;
        private Button btnRefresh;
        private Panel panelBottom;
        private ProductRepository productRepository;

        public ProductRestoreForm()
        {
            InitializeComponent();
            productRepository = new ProductRepository();
            LoadDeletedProducts();
        }

        private void InitializeComponent()
        {
            this.dgvDeletedProducts = new System.Windows.Forms.DataGridView();
            this.btnRestore = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.panelBottom = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeletedProducts)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvDeletedProducts
            // 
            this.dgvDeletedProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDeletedProducts.ColumnHeadersHeight = 46;
            this.dgvDeletedProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDeletedProducts.Location = new System.Drawing.Point(0, 0);
            this.dgvDeletedProducts.MultiSelect = false;
            this.dgvDeletedProducts.Name = "dgvDeletedProducts";
            this.dgvDeletedProducts.RowHeadersWidth = 82;
            this.dgvDeletedProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDeletedProducts.Size = new System.Drawing.Size(1471, 627);
            this.dgvDeletedProducts.TabIndex = 0;
            // 
            // btnRestore
            // 
            this.btnRestore.Location = new System.Drawing.Point(20, 15);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(150, 40);
            this.btnRestore.TabIndex = 1;
            this.btnRestore.Text = "Khôi phục";
            this.btnRestore.Click += new System.EventHandler(this.BtnRestore_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(190, 15);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(120, 40);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Tải lại";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // panelBottom
            // 
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Height = 70;
            this.panelBottom.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelBottom.Controls.Add(this.btnRestore);
            this.panelBottom.Controls.Add(this.btnRefresh);
            // 
            // ProductRestoreForm
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1471, 697);
            this.Controls.Add(this.dgvDeletedProducts);
            this.Controls.Add(this.panelBottom);
            this.Name = "ProductRestoreForm";
            this.Text = "Khôi phục sản phẩm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeletedProducts)).EndInit();
            this.ResumeLayout(false);

        }

        private void LoadDeletedProducts()
        {
            try
            {
                DataTable data = productRepository.GetDeletedProducts();
                dgvDeletedProducts.DataSource = data;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sản phẩm đã xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDeletedProducts();
        }

        private void BtnRestore_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDeletedProducts.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn một sản phẩm để khôi phục.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int productId = Convert.ToInt32(dgvDeletedProducts.CurrentRow.Cells["ProductID"].Value);

                var confirm = MessageBox.Show("Khôi phục sản phẩm đã chọn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                bool success = productRepository.RestoreProduct(productId);
                if (success)
                {
                    MessageBox.Show("Khôi phục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDeletedProducts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi khôi phục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}


