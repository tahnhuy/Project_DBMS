using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sale_Management.DatabaseAccess;

namespace Sale_Management.Forms
{
    public partial class AdminDiscountEditForm : Form
    {
        private DiscountRepository discountRepository;
        private ProductRepository productRepository;
        private int? discountId; // null nếu thêm mới, có giá trị nếu chỉnh sửa
        private string currentUser = "admin"; // Có thể thay đổi thành user hiện tại
        private bool isEditMode = false;

        // Constructor cho thêm mới
        public AdminDiscountEditForm()
        {
            InitializeComponent();
            discountRepository = new DiscountRepository();
            productRepository = new ProductRepository();
            isEditMode = false;
            InitializeControls();
            SetupNewDiscount();
        }

        // Constructor cho chỉnh sửa
        public AdminDiscountEditForm(int discountId)
        {
            InitializeComponent();
            discountRepository = new DiscountRepository();
            productRepository = new ProductRepository();
            this.discountId = discountId;
            isEditMode = true;
            InitializeControls();
            LoadDiscountData();
        }

        private void InitializeControls()
        {
            // Thiết lập form
            this.Text = isEditMode ? "Chỉnh sửa Chương trình Giảm giá" : "Thêm Chương trình Giảm giá mới";
            this.Size = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            CreateControls();
        }

        private void CreateControls()
        {
            // Panel chính
            Panel mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Padding = new Padding(20);

            // Label và ComboBox cho Sản phẩm
            Label lblProduct = new Label();
            lblProduct.Text = "Sản phẩm:";
            lblProduct.Location = new Point(0, 20);
            lblProduct.Size = new Size(100, 23);

            ComboBox cmbProduct = new ComboBox();
            cmbProduct.Name = "cmbProduct";
            cmbProduct.Location = new Point(120, 20);
            cmbProduct.Size = new Size(300, 23);
            cmbProduct.DropDownStyle = ComboBoxStyle.DropDownList;
            LoadProducts(cmbProduct);

            // Label và ComboBox cho Loại giảm giá
            Label lblDiscountType = new Label();
            lblDiscountType.Text = "Loại giảm giá:";
            lblDiscountType.Location = new Point(0, 60);
            lblDiscountType.Size = new Size(100, 23);

            ComboBox cmbDiscountType = new ComboBox();
            cmbDiscountType.Name = "cmbDiscountType";
            cmbDiscountType.Location = new Point(120, 60);
            cmbDiscountType.Size = new Size(300, 23);
            cmbDiscountType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDiscountType.Items.AddRange(new string[] { "Phần trăm (%)", "Số tiền cố định (VNĐ)" });
            cmbDiscountType.SelectedIndexChanged += CmbDiscountType_SelectedIndexChanged;

            // Label và NumericUpDown cho Giá trị giảm
            Label lblDiscountValue = new Label();
            lblDiscountValue.Text = "Giá trị giảm:";
            lblDiscountValue.Location = new Point(0, 100);
            lblDiscountValue.Size = new Size(100, 23);

            NumericUpDown nudDiscountValue = new NumericUpDown();
            nudDiscountValue.Name = "nudDiscountValue";
            nudDiscountValue.Location = new Point(120, 100);
            nudDiscountValue.Size = new Size(200, 23);
            nudDiscountValue.DecimalPlaces = 2;
            nudDiscountValue.Minimum = 0;
            nudDiscountValue.Maximum = 999999999;

            Label lblDiscountUnit = new Label();
            lblDiscountUnit.Name = "lblDiscountUnit";
            lblDiscountUnit.Text = "";
            lblDiscountUnit.Location = new Point(330, 103);
            lblDiscountUnit.Size = new Size(90, 20);

            // Label và DateTimePicker cho Ngày bắt đầu
            Label lblStartDate = new Label();
            lblStartDate.Text = "Ngày bắt đầu:";
            lblStartDate.Location = new Point(0, 140);
            lblStartDate.Size = new Size(100, 23);

            DateTimePicker dtpStartDate = new DateTimePicker();
            dtpStartDate.Name = "dtpStartDate";
            dtpStartDate.Location = new Point(120, 140);
            dtpStartDate.Size = new Size(300, 23);
            dtpStartDate.Format = DateTimePickerFormat.Custom;
            dtpStartDate.CustomFormat = "dd/MM/yyyy HH:mm";

            // Label và DateTimePicker cho Ngày kết thúc
            Label lblEndDate = new Label();
            lblEndDate.Text = "Ngày kết thúc:";
            lblEndDate.Location = new Point(0, 180);
            lblEndDate.Size = new Size(100, 23);

            DateTimePicker dtpEndDate = new DateTimePicker();
            dtpEndDate.Name = "dtpEndDate";
            dtpEndDate.Location = new Point(120, 180);
            dtpEndDate.Size = new Size(300, 23);
            dtpEndDate.Format = DateTimePickerFormat.Custom;
            dtpEndDate.CustomFormat = "dd/MM/yyyy HH:mm";

            // CheckBox cho Trạng thái hoạt động
            CheckBox chkIsActive = new CheckBox();
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Text = "Chương trình đang hoạt động";
            chkIsActive.Location = new Point(120, 220);
            chkIsActive.Size = new Size(250, 23);
            chkIsActive.Checked = true;

            // Panel cho các nút
            Panel buttonPanel = new Panel();
            buttonPanel.Height = 60;
            buttonPanel.Dock = DockStyle.Bottom;

            // Nút Lưu
            Button btnSave = new Button();
            btnSave.Name = "btnSave";
            btnSave.Text = isEditMode ? "Cập nhật" : "Thêm mới";
            btnSave.Size = new Size(100, 35);
            btnSave.Location = new Point(220, 15);
            btnSave.Click += BtnSave_Click;

            // Nút Hủy
            Button btnCancel = new Button();
            btnCancel.Name = "btnCancel";
            btnCancel.Text = "Hủy";
            btnCancel.Size = new Size(100, 35);
            btnCancel.Location = new Point(330, 15);
            btnCancel.Click += BtnCancel_Click;

            // Thêm controls vào panel chính
            mainPanel.Controls.AddRange(new Control[] {
                lblProduct, cmbProduct,
                lblDiscountType, cmbDiscountType,
                lblDiscountValue, nudDiscountValue, lblDiscountUnit,
                lblStartDate, dtpStartDate,
                lblEndDate, dtpEndDate,
                chkIsActive
            });

            // Thêm nút vào button panel
            buttonPanel.Controls.AddRange(new Control[] { btnSave, btnCancel });

            // Thêm panels vào form
            this.Controls.Add(mainPanel);
            this.Controls.Add(buttonPanel);
        }

        private void LoadProducts(ComboBox cmb)
        {
            try
            {
                DataTable products = productRepository.GetAllProducts();
                cmb.Items.Clear();
                
                foreach (DataRow row in products.Rows)
                {
                    cmb.Items.Add($"{row["ProductID"]} - {row["ProductName"]}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sản phẩm: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbDiscountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            Label lblUnit = this.Controls.Find("lblDiscountUnit", true)[0] as Label;
            NumericUpDown nudDiscountValue = this.Controls.Find("nudDiscountValue", true)[0] as NumericUpDown;

            if (cmb.SelectedIndex == 0) // Phần trăm
            {
                lblUnit.Text = "%";
                nudDiscountValue.Maximum = 100;
                nudDiscountValue.DecimalPlaces = 1;
            }
            else // Số tiền cố định
            {
                lblUnit.Text = "VNĐ";
                nudDiscountValue.Maximum = 999999999;
                nudDiscountValue.DecimalPlaces = 0;
            }
        }

        private void SetupNewDiscount()
        {
            // Thiết lập giá trị mặc định cho discount mới
            DateTimePicker dtpStart = this.Controls.Find("dtpStartDate", true)[0] as DateTimePicker;
            DateTimePicker dtpEnd = this.Controls.Find("dtpEndDate", true)[0] as DateTimePicker;
            ComboBox cmbDiscountType = this.Controls.Find("cmbDiscountType", true)[0] as ComboBox;

            dtpStart.Value = DateTime.Now;
            dtpEnd.Value = DateTime.Now.AddMonths(1);
            cmbDiscountType.SelectedIndex = 0; // Mặc định chọn phần trăm
        }

        private void LoadDiscountData()
        {
            try
            {
                DataTable dt = DatabaseConnection.ExecuteQuery(
                    "SELECT * FROM Discounts WHERE DiscountID = @DiscountID",
                    CommandType.Text,
                    new System.Data.SqlClient.SqlParameter("@DiscountID", discountId.Value));

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    // Load dữ liệu vào các control
                    int productId = Convert.ToInt32(row["ProductID"]);
                    ComboBox cmbProduct = this.Controls.Find("cmbProduct", true)[0] as ComboBox;
                    
                    // Tìm và chọn sản phẩm tương ứng
                    for (int i = 0; i < cmbProduct.Items.Count; i++)
                    {
                        string item = cmbProduct.Items[i].ToString();
                        if (item.StartsWith(productId.ToString() + " -"))
                        {
                            cmbProduct.SelectedIndex = i;
                            break;
                        }
                    }

                    // Loại giảm giá
                    string discountType = row["DiscountType"].ToString();
                    ComboBox cmbDiscountType = this.Controls.Find("cmbDiscountType", true)[0] as ComboBox;
                    cmbDiscountType.SelectedIndex = discountType == "percentage" ? 0 : 1;

                    // Giá trị giảm
                    (this.Controls.Find("nudDiscountValue", true)[0] as NumericUpDown).Value = Convert.ToDecimal(row["DiscountValue"]);

                    // Ngày bắt đầu và kết thúc
                    (this.Controls.Find("dtpStartDate", true)[0] as DateTimePicker).Value = Convert.ToDateTime(row["StartDate"]);
                    (this.Controls.Find("dtpEndDate", true)[0] as DateTimePicker).Value = Convert.ToDateTime(row["EndDate"]);

                    // Trạng thái
                    (this.Controls.Find("chkIsActive", true)[0] as CheckBox).Checked = Convert.ToBoolean(row["IsActive"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu chương trình giảm giá: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput())
                    return;

                // Lấy dữ liệu từ các control
                ComboBox cmbProduct = this.Controls.Find("cmbProduct", true)[0] as ComboBox;
                string selectedProduct = cmbProduct.SelectedItem.ToString();
                int productId = Convert.ToInt32(selectedProduct.Split('-')[0].Trim());
                
                ComboBox cmbDiscountType = this.Controls.Find("cmbDiscountType", true)[0] as ComboBox;
                string discountType = cmbDiscountType.SelectedIndex == 0 ? "percentage" : "fixed";
                
                decimal discountValue = (this.Controls.Find("nudDiscountValue", true)[0] as NumericUpDown).Value;
                DateTime startDate = (this.Controls.Find("dtpStartDate", true)[0] as DateTimePicker).Value;
                DateTime endDate = (this.Controls.Find("dtpEndDate", true)[0] as DateTimePicker).Value;
                bool isActive = (this.Controls.Find("chkIsActive", true)[0] as CheckBox).Checked;

                bool success;

                if (isEditMode)
                {
                    // Cập nhật discount
                    success = discountRepository.UpdateDiscount(discountId.Value, productId, discountType,
                        discountValue, startDate, endDate, isActive);
                }
                else
                {
                    // Thêm discount mới
                    success = discountRepository.AddDiscount(productId, discountType,
                        discountValue, startDate, endDate, isActive, currentUser);
                }

                if (success)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể lưu chương trình giảm giá!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu chương trình giảm giá: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            // Validate sản phẩm
            ComboBox cmbProduct = this.Controls.Find("cmbProduct", true)[0] as ComboBox;
            if (cmbProduct.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validate loại giảm giá
            ComboBox cmbDiscountType = this.Controls.Find("cmbDiscountType", true)[0] as ComboBox;
            if (cmbDiscountType.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn loại giảm giá!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validate giá trị giảm
            decimal discountValue = (this.Controls.Find("nudDiscountValue", true)[0] as NumericUpDown).Value;
            if (discountValue <= 0)
            {
                MessageBox.Show("Giá trị giảm phải lớn hơn 0!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validate phần trăm
            if (cmbDiscountType.SelectedIndex == 0 && discountValue > 100)
            {
                MessageBox.Show("Phần trăm giảm không được vượt quá 100%!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validate ngày
            DateTime startDate = (this.Controls.Find("dtpStartDate", true)[0] as DateTimePicker).Value;
            DateTime endDate = (this.Controls.Find("dtpEndDate", true)[0] as DateTimePicker).Value;
            if (startDate >= endDate)
            {
                MessageBox.Show("Ngày kết thúc phải sau ngày bắt đầu!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
