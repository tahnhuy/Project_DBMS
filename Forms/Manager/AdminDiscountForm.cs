using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sale_Management.DatabaseAccess;

namespace Sale_Management.Forms
{
    public partial class AdminDiscountForm : Form
    {
        private DiscountRepository discountRepository;
        private ProductRepository productRepository;

        public AdminDiscountForm()
        {
            InitializeComponent();
            discountRepository = new DiscountRepository();
            productRepository = new ProductRepository();
            InitializeControls();
            LoadDiscounts();
        }

        private void InitializeControls()
        {
            // Thiết lập form
            this.Text = "Quản lý Chương trình Giảm giá";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Tạo các control
            CreateControls();
        }

        private void CreateControls()
        {
            // DataGridView để hiển thị danh sách giảm giá
            DataGridView dgvDiscounts = new DataGridView();
            dgvDiscounts.Name = "dgvDiscounts";
            dgvDiscounts.Dock = DockStyle.Fill;
            dgvDiscounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDiscounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDiscounts.ReadOnly = true;
            dgvDiscounts.AllowUserToAddRows = false;
            dgvDiscounts.AllowUserToDeleteRows = false;

            // Panel cho các nút chức năng
            Panel panelButtons = new Panel();
            panelButtons.Name = "panelButtons";
            panelButtons.Height = 60;
            panelButtons.Dock = DockStyle.Top;

            // Nút thêm giảm giá
            Button btnAdd = new Button();
            btnAdd.Name = "btnAdd";
            btnAdd.Text = "Thêm Giảm giá";
            btnAdd.Size = new Size(120, 35);
            btnAdd.Location = new Point(10, 12);
            btnAdd.Click += BtnAdd_Click;

            // Nút sửa giảm giá
            Button btnEdit = new Button();
            btnEdit.Name = "btnEdit";
            btnEdit.Text = "Sửa Giảm giá";
            btnEdit.Size = new Size(120, 35);
            btnEdit.Location = new Point(140, 12);
            btnEdit.Click += BtnEdit_Click;

            // Nút xóa giảm giá
            Button btnDelete = new Button();
            btnDelete.Name = "btnDelete";
            btnDelete.Text = "Xóa Giảm giá";
            btnDelete.Size = new Size(120, 35);
            btnDelete.Location = new Point(270, 12);
            btnDelete.Click += BtnDelete_Click;

            // Nút làm mới
            Button btnRefresh = new Button();
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Text = "Làm mới";
            btnRefresh.Size = new Size(120, 35);
            btnRefresh.Location = new Point(400, 12);
            btnRefresh.Click += BtnRefresh_Click;

            // ComboBox lọc theo trạng thái
            ComboBox cmbStatusFilter = new ComboBox();
            cmbStatusFilter.Name = "cmbStatusFilter";
            cmbStatusFilter.Size = new Size(150, 25);
            cmbStatusFilter.Location = new Point(550, 17);
            cmbStatusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatusFilter.Items.AddRange(new string[] { "Tất cả", "Đang hoạt động", "Không hoạt động", "Đã hết hạn" });
            cmbStatusFilter.SelectedIndex = 0;
            cmbStatusFilter.SelectedIndexChanged += CmbStatusFilter_SelectedIndexChanged;

            // ComboBox lọc theo sản phẩm
            ComboBox cmbProductFilter = new ComboBox();
            cmbProductFilter.Name = "cmbProductFilter";
            cmbProductFilter.Size = new Size(200, 25);
            cmbProductFilter.Location = new Point(720, 17);
            cmbProductFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProductFilter.SelectedIndexChanged += CmbProductFilter_SelectedIndexChanged;
            LoadProductsForFilter(cmbProductFilter);

            // Label cho bộ lọc
            Label lblStatusFilter = new Label();
            lblStatusFilter.Text = "Trạng thái:";
            lblStatusFilter.Location = new Point(480, 20);
            lblStatusFilter.Size = new Size(70, 20);

            Label lblProductFilter = new Label();
            lblProductFilter.Text = "Sản phẩm:";
            lblProductFilter.Location = new Point(650, 20);
            lblProductFilter.Size = new Size(70, 20);

            // Thêm controls vào panel
            panelButtons.Controls.AddRange(new Control[] { 
                btnAdd, btnEdit, btnDelete, btnRefresh, 
                lblStatusFilter, cmbStatusFilter, 
                lblProductFilter, cmbProductFilter 
            });

            // Thêm controls vào form
            this.Controls.Add(dgvDiscounts);
            this.Controls.Add(panelButtons);
        }

        private void LoadProductsForFilter(ComboBox cmb)
        {
            try
            {
                DataTable products = productRepository.GetAllProducts();
                cmb.Items.Clear();
                cmb.Items.Add("Tất cả sản phẩm");
                
                foreach (DataRow row in products.Rows)
                {
                    cmb.Items.Add($"{row["ProductID"]} - {row["ProductName"]}");
                }
                cmb.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sản phẩm: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDiscounts()
        {
            try
            {
                DataTable dt = discountRepository.GetActiveDiscounts();
                Control[] dgvControls = this.Controls.Find("dgvDiscounts", true);
                DataGridView dgv = dgvControls.Length > 0 ? dgvControls[0] as DataGridView : null;
                if (dgv != null)
                {
                    dgv.DataSource = dt;
                    ConfigureDataGridView(dgv);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu chương trình giảm giá: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureDataGridView(DataGridView dgv)
        {
            if (dgv.Columns.Count > 0)
            {
                // Cấu hình header text
                if (dgv.Columns.Contains("DiscountID"))
                    dgv.Columns["DiscountID"].HeaderText = "ID";
                if (dgv.Columns.Contains("ProductID"))
                    dgv.Columns["ProductID"].HeaderText = "Mã SP";
                if (dgv.Columns.Contains("ProductName"))
                    dgv.Columns["ProductName"].HeaderText = "Tên sản phẩm";
                if (dgv.Columns.Contains("DiscountType"))
                    dgv.Columns["DiscountType"].HeaderText = "Loại giảm giá";
                if (dgv.Columns.Contains("DiscountValue"))
                    dgv.Columns["DiscountValue"].HeaderText = "Giá trị giảm";
                if (dgv.Columns.Contains("StartDate"))
                    dgv.Columns["StartDate"].HeaderText = "Ngày bắt đầu";
                if (dgv.Columns.Contains("EndDate"))
                    dgv.Columns["EndDate"].HeaderText = "Ngày kết thúc";
                if (dgv.Columns.Contains("IsActive"))
                    dgv.Columns["IsActive"].HeaderText = "Trạng thái";
                if (dgv.Columns.Contains("CreatedDate"))
                    dgv.Columns["CreatedDate"].HeaderText = "Ngày tạo";
                if (dgv.Columns.Contains("CreatedBy"))
                    dgv.Columns["CreatedBy"].HeaderText = "Người tạo";

                // Ẩn cột ID
                if (dgv.Columns.Contains("DiscountID"))
                    dgv.Columns["DiscountID"].Visible = false;

                // Định dạng cột ngày
                if (dgv.Columns.Contains("StartDate"))
                    dgv.Columns["StartDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
                if (dgv.Columns.Contains("EndDate"))
                    dgv.Columns["EndDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
                if (dgv.Columns.Contains("CreatedDate"))
                    dgv.Columns["CreatedDate"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                AdminDiscountEditForm editForm = new AdminDiscountEditForm();
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    ApplyCurrentFilters();
                    MessageBox.Show("Thêm chương trình giảm giá thành công!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở form thêm giảm giá: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                Control[] dgvControls = this.Controls.Find("dgvDiscounts", true);
                DataGridView dgv = dgvControls.Length > 0 ? dgvControls[0] as DataGridView : null;
                if (dgv == null || dgv.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn chương trình giảm giá cần sửa!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataRowView selectedRow = dgv.SelectedRows[0].DataBoundItem as DataRowView;
                int discountId = Convert.ToInt32(selectedRow["DiscountID"]);

                AdminDiscountEditForm editForm = new AdminDiscountEditForm(discountId);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    ApplyCurrentFilters();
                    MessageBox.Show("Cập nhật chương trình giảm giá thành công!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở form sửa giảm giá: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Control[] dgvControls = this.Controls.Find("dgvDiscounts", true);
                DataGridView dgv = dgvControls.Length > 0 ? dgvControls[0] as DataGridView : null;
                if (dgv == null || dgv.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn chương trình giảm giá cần xóa!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataRowView selectedRow = dgv.SelectedRows[0].DataBoundItem as DataRowView;
                int discountId = Convert.ToInt32(selectedRow["DiscountID"]);
                string productName = selectedRow["ProductName"].ToString();

                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa chương trình giảm giá cho '{productName}'?", 
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        bool success = discountRepository.DeleteDiscount(discountId);
                        if (success)
                        {
                            MessageBox.Show("Xóa chương trình giảm giá thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDiscounts(); // Reload the discount list
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa chương trình giảm giá: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa chương trình giảm giá: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            // Reset bộ lọc về mặc định
            Control[] statusControls = this.Controls.Find("cmbStatusFilter", true);
            ComboBox cmbStatusFilter = statusControls.Length > 0 ? statusControls[0] as ComboBox : null;
            Control[] productControls = this.Controls.Find("cmbProductFilter", true);
            ComboBox cmbProductFilter = productControls.Length > 0 ? productControls[0] as ComboBox : null;
            
            if (cmbStatusFilter != null) cmbStatusFilter.SelectedIndex = 0;
            if (cmbProductFilter != null) cmbProductFilter.SelectedIndex = 0;
            
            LoadDiscounts();
        }

        private void CmbStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyCurrentFilters();
        }

        private void CmbProductFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyCurrentFilters();
        }

        private void ApplyCurrentFilters()
        {
            try
            {
                DataTable allDiscounts = discountRepository.GetActiveDiscounts();
                DataView dv = allDiscounts.DefaultView;

                // Lấy giá trị bộ lọc trạng thái
                Control[] statusControls = this.Controls.Find("cmbStatusFilter", true);
                ComboBox cmbStatusFilter = statusControls.Length > 0 ? statusControls[0] as ComboBox : null;
                string statusFilter = cmbStatusFilter?.SelectedItem?.ToString() ?? "";

                // Lấy giá trị bộ lọc sản phẩm
                Control[] productControls = this.Controls.Find("cmbProductFilter", true);
                ComboBox cmbProductFilter = productControls.Length > 0 ? productControls[0] as ComboBox : null;
                string productFilter = cmbProductFilter?.SelectedItem?.ToString() ?? "";

                string filter = "";

                // Bộ lọc trạng thái
                if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "Tất cả")
                {
                    DateTime now = DateTime.Now;
                    switch (statusFilter)
                    {
                        case "Đang hoạt động":
                            filter = string.Format("IsActive = True AND StartDate <= '{0}' AND EndDate >= '{0}'", 
                                now.ToString("yyyy-MM-dd HH:mm:ss"));
                            break;
                        case "Không hoạt động":
                            filter = "IsActive = False";
                            break;
                        case "Đã hết hạn":
                            filter = string.Format("EndDate < '{0}'", 
                                now.ToString("yyyy-MM-dd HH:mm:ss"));
                            break;
                    }
                }

                // Bộ lọc sản phẩm
                if (!string.IsNullOrEmpty(productFilter) && productFilter != "Tất cả sản phẩm")
                {
                    // Kiểm tra xem string có chứa dấu '-' không
                    if (productFilter.Contains("-"))
                    {
                        string[] parts = productFilter.Split('-');
                        if (parts.Length > 0)
                        {
                            string productIdStr = parts[0].Trim();
                            // Kiểm tra xem ProductID có phải là số không
                            if (int.TryParse(productIdStr, out int productId))
                            {
                                string productCondition = $"ProductID = {productId}";
                                
                                if (!string.IsNullOrEmpty(filter))
                                    filter += " AND ";
                                filter += productCondition;
                            }
                        }
                    }
                }

                dv.RowFilter = filter;

                Control[] dgvControls = this.Controls.Find("dgvDiscounts", true);
                DataGridView dgv = dgvControls.Length > 0 ? dgvControls[0] as DataGridView : null;
                if (dgv != null)
                {
                    dgv.DataSource = dv.ToTable();
                    ConfigureDataGridView(dgv);
                }
            }
            catch (Exception ex)
            {
                // Log chi tiết lỗi để debug
                string errorDetails = $"Lỗi chi tiết: {ex.Message}\n" +
                                    $"Stack trace: {ex.StackTrace}\n" +
                                    $"Inner exception: {ex.InnerException?.Message ?? "None"}";
                
                MessageBox.Show($"Lỗi khi áp dụng bộ lọc: {ex.Message}\n\n{errorDetails}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
