using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sale_Management.Forms;
using Sale_Management.DatabaseAccess;

namespace Sale_Management
{
    public partial class AdminForm : Form
    {
        private string currentUsername;
        private string currentRole;

        public AdminForm(string username = null, string role = null)
        {
            try
            {
                InitializeComponent();
                currentUsername = username ?? "";
                currentRole = role ?? "";

                // Không tự động load form nào, để user chọn từ menu
                // Hiển thị thông báo chào mừng
                Label welcomeLabel = new Label();
                welcomeLabel.Text = $"Chào mừng {currentUsername}!\nVui lòng chọn chức năng từ menu.";
                welcomeLabel.Font = new Font("Arial", 14, FontStyle.Bold);
                welcomeLabel.TextAlign = ContentAlignment.MiddleCenter;
                welcomeLabel.Dock = DockStyle.Fill;
                panel_Container.Controls.Add(welcomeLabel);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo AdminForm: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowFormInPanel(Form form)
        {
            try
            {
                if (form != null)
                {
                    panel_Container.Controls.Clear();
                    form.TopLevel = false;
                    form.FormBorderStyle = FormBorderStyle.None;
                    form.Dock = DockStyle.Fill;
                    panel_Container.Controls.Add(form);
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hiển thị form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void msi_Product_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new Forms.ProductForm());
        }

        private void kháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new Forms.CustomerManageForm());
        }



        private void msi_createAcc_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new Forms.AccountCreateForm());
        }

        private void msi_AccManage_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new Forms.AccountForm());
        }

        private void msi_AccountManagement_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra quyền quản lý tài khoản
                if (!SecurityHelper.CanManageAccounts(currentUsername))
                {
                    MessageBox.Show("Bạn không có quyền quản lý tài khoản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo form quản lý tài khoản đơn giản
                Form accountManagementForm = new Form();
                accountManagementForm.Text = "Quản lý tài khoản";
                accountManagementForm.Size = new Size(1000, 600);
                accountManagementForm.StartPosition = FormStartPosition.CenterParent;

                // Tạo DataGridView để hiển thị danh sách tài khoản
                DataGridView dgvAccounts = new DataGridView();
                dgvAccounts.Dock = DockStyle.Fill;
                dgvAccounts.ReadOnly = true;
                dgvAccounts.AllowUserToAddRows = false;
                dgvAccounts.AllowUserToDeleteRows = false;
                dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                // Tạo panel chứa các button
                Panel buttonPanel = new Panel();
                buttonPanel.Dock = DockStyle.Top;
                buttonPanel.Height = 50;

                Button btnAddAccount = new Button();
                btnAddAccount.Text = "Thêm tài khoản";
                btnAddAccount.Location = new Point(10, 10);
                btnAddAccount.Size = new Size(100, 30);
                btnAddAccount.Click += (s, ev) => AddAccount();

                Button btnEditAccount = new Button();
                btnEditAccount.Text = "Sửa tài khoản";
                btnEditAccount.Location = new Point(120, 10);
                btnEditAccount.Size = new Size(100, 30);
                btnEditAccount.Click += (s, ev) => EditAccount(dgvAccounts);

                Button btnDeleteAccount = new Button();
                btnDeleteAccount.Text = "Xóa tài khoản";
                btnDeleteAccount.Location = new Point(230, 10);
                btnDeleteAccount.Size = new Size(100, 30);
                btnDeleteAccount.Click += (s, ev) => DeleteAccount(dgvAccounts);

                Button btnRefresh = new Button();
                btnRefresh.Text = "Làm mới";
                btnRefresh.Location = new Point(340, 10);
                btnRefresh.Size = new Size(80, 30);
                btnRefresh.Click += (s, ev) => LoadAccounts(dgvAccounts);

                Button btnViewStats = new Button();
                btnViewStats.Text = "Thống kê";
                btnViewStats.Location = new Point(430, 10);
                btnViewStats.Size = new Size(80, 30);
                btnViewStats.Click += (s, ev) => ViewAccountStats();

                buttonPanel.Controls.AddRange(new Control[] { btnAddAccount, btnEditAccount, btnDeleteAccount, btnRefresh, btnViewStats });

                accountManagementForm.Controls.Add(dgvAccounts);
                accountManagementForm.Controls.Add(buttonPanel);

                // Load dữ liệu
                LoadAccounts(dgvAccounts);

                ShowFormInPanel(accountManagementForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở quản lý tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAccounts(DataGridView dgv)
        {
            try
            {
                DataTable accountsData = AccountRepository.GetAccountsWithDetails();
                dgv.DataSource = accountsData;
                
                if (dgv.Columns.Count > 0)
                {
                    dgv.Columns["Username"].HeaderText = "Tên đăng nhập";
                    dgv.Columns["Role"].HeaderText = "Vai trò";
                    dgv.Columns["CreatedDate"].HeaderText = "Ngày tạo";
                    dgv.Columns["CustomerName"].HeaderText = "Tên khách hàng";
                    dgv.Columns["Phone"].HeaderText = "Số điện thoại";
                    dgv.Columns["LoyaltyPoints"].HeaderText = "Điểm tích lũy";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddAccount()
        {
            try
            {
                // Tạo form thêm tài khoản đơn giản
                Form addAccountForm = new Form();
                addAccountForm.Text = "Thêm tài khoản";
                addAccountForm.Size = new Size(350, 250);
                addAccountForm.StartPosition = FormStartPosition.CenterParent;
                addAccountForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                addAccountForm.MaximizeBox = false;
                addAccountForm.MinimizeBox = false;

                Label lblUsername = new Label();
                lblUsername.Text = "Tên đăng nhập:";
                lblUsername.Location = new Point(20, 20);
                lblUsername.Size = new Size(100, 20);

                TextBox txtUsername = new TextBox();
                txtUsername.Location = new Point(130, 18);
                txtUsername.Size = new Size(180, 20);

                Label lblPassword = new Label();
                lblPassword.Text = "Mật khẩu:";
                lblPassword.Location = new Point(20, 50);
                lblPassword.Size = new Size(100, 20);

                TextBox txtPassword = new TextBox();
                txtPassword.Location = new Point(130, 48);
                txtPassword.Size = new Size(180, 20);
                txtPassword.PasswordChar = '*';

                Label lblRole = new Label();
                lblRole.Text = "Vai trò:";
                lblRole.Location = new Point(20, 80);
                lblRole.Size = new Size(100, 20);

                ComboBox cmbRole = new ComboBox();
                cmbRole.Location = new Point(130, 78);
                cmbRole.Size = new Size(180, 20);
                cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbRole.Items.AddRange(new object[] { "employee", "customer" });
                cmbRole.SelectedIndex = 0;

                Button btnOK = new Button();
                btnOK.Text = "OK";
                btnOK.Location = new Point(130, 120);
                btnOK.Size = new Size(80, 30);
                btnOK.DialogResult = DialogResult.OK;

                Button btnCancel = new Button();
                btnCancel.Text = "Hủy";
                btnCancel.Location = new Point(220, 120);
                btnCancel.Size = new Size(80, 30);
                btnCancel.DialogResult = DialogResult.Cancel;

                addAccountForm.Controls.AddRange(new Control[] { 
                    lblUsername, txtUsername, 
                    lblPassword, txtPassword, 
                    lblRole, cmbRole, 
                    btnOK, btnCancel 
                });

                if (addAccountForm.ShowDialog() == DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(txtUsername.Text) || 
                        string.IsNullOrEmpty(txtPassword.Text))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string username = txtUsername.Text;
                    string password = txtPassword.Text;
                    string role = cmbRole.SelectedItem.ToString();

                    // Validate inputs
                    if (!SecurityHelper.ValidateUsername(username))
                    {
                        MessageBox.Show("Tên đăng nhập không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!SecurityHelper.ValidatePassword(password))
                    {
                        MessageBox.Show("Mật khẩu không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    DataTable result = AccountRepository.AddAccount(currentUsername, username, password, role);
                    if (result.Rows.Count > 0 && result.Rows[0]["Result"].ToString() == "SUCCESS")
                    {
                        MessageBox.Show("Thêm tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(result.Rows[0]["Message"].ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditAccount(DataGridView dgv)
        {
            try
            {
                if (dgv.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn tài khoản cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string username = dgv.SelectedRows[0].Cells["Username"].Value.ToString();
                
                if (username == currentUsername)
                {
                    MessageBox.Show("Không thể sửa tài khoản của chính mình!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo form sửa tài khoản đơn giản
                Form editAccountForm = new Form();
                editAccountForm.Text = "Sửa tài khoản - " + username;
                editAccountForm.Size = new Size(350, 200);
                editAccountForm.StartPosition = FormStartPosition.CenterParent;
                editAccountForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                editAccountForm.MaximizeBox = false;
                editAccountForm.MinimizeBox = false;

                Label lblNewPassword = new Label();
                lblNewPassword.Text = "Mật khẩu mới:";
                lblNewPassword.Location = new Point(20, 20);
                lblNewPassword.Size = new Size(120, 20);

                TextBox txtNewPassword = new TextBox();
                txtNewPassword.Location = new Point(150, 18);
                txtNewPassword.Size = new Size(160, 20);
                txtNewPassword.PasswordChar = '*';

                Label lblNewRole = new Label();
                lblNewRole.Text = "Vai trò mới:";
                lblNewRole.Location = new Point(20, 50);
                lblNewRole.Size = new Size(120, 20);

                ComboBox cmbNewRole = new ComboBox();
                cmbNewRole.Location = new Point(150, 48);
                cmbNewRole.Size = new Size(160, 20);
                cmbNewRole.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbNewRole.Items.AddRange(new object[] { "", "admin", "manager", "employee", "customer" });
                cmbNewRole.SelectedIndex = 0;

                Button btnOK = new Button();
                btnOK.Text = "OK";
                btnOK.Location = new Point(150, 100);
                btnOK.Size = new Size(80, 30);
                btnOK.DialogResult = DialogResult.OK;

                Button btnCancel = new Button();
                btnCancel.Text = "Hủy";
                btnCancel.Location = new Point(240, 100);
                btnCancel.Size = new Size(80, 30);
                btnCancel.DialogResult = DialogResult.Cancel;

                editAccountForm.Controls.AddRange(new Control[] { 
                    lblNewPassword, txtNewPassword, 
                    lblNewRole, cmbNewRole, 
                    btnOK, btnCancel 
                });

                if (editAccountForm.ShowDialog() == DialogResult.OK)
                {
                    string newPassword = string.IsNullOrEmpty(txtNewPassword.Text) ? null : txtNewPassword.Text;
                    string newRole = cmbNewRole.SelectedIndex == 0 ? null : cmbNewRole.SelectedItem.ToString();

                    DataTable result = AccountRepository.UpdateAccount(username, newPassword, newRole);

                    if (result.Rows.Count > 0 && result.Rows[0]["Result"].ToString() == "SUCCESS")
                    {
                        MessageBox.Show("Sửa tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(result.Rows[0]["Message"].ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteAccount(DataGridView dgv)
        {
            try
            {
                if (dgv.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn tài khoản cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string username = dgv.SelectedRows[0].Cells["Username"].Value.ToString();
                
                if (username == currentUsername)
                {
                    MessageBox.Show("Không thể xóa tài khoản của chính mình!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show($"Bạn có chắc chắn muốn xóa tài khoản '{username}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataTable result = AccountRepository.DeleteAccount(username);
                    if (result.Rows.Count > 0 && result.Rows[0]["Result"].ToString() == "SUCCESS")
                    {
                        MessageBox.Show("Xóa tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(result.Rows[0]["Message"].ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ViewAccountStats()
        {
            try
            {
                DataTable stats = AccountRepository.GetAccountStatistics();
                if (stats.Rows.Count > 0)
                {
                    var row = stats.Rows[0];
                    string message = $"Thống kê tài khoản:\n\n" +
                                   $"Tổng số tài khoản: {row["TotalAccounts"]}\n" +
                                   $"Admin: {row["AdminCount"]}\n" +
                                   $"Manager: {row["ManagerCount"]}\n" +
                                   $"Employee: {row["EmployeeCount"]}\n" +
                                   $"Customer: {row["CustomerCount"]}";
                    
                    MessageBox.Show(message, "Thống kê tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xem thống kê: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void msi_Discount_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new Forms.AdminDiscountForm());
        }

        private void msi_Reports_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra quyền xem báo cáo
                if (!SecurityHelper.CanViewRevenueReports(currentUsername))
                {
                    MessageBox.Show("Bạn không có quyền xem báo cáo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo form báo cáo đơn giản
                Form reportsForm = new Form();
                reportsForm.Text = "Báo cáo và thống kê";
                reportsForm.Size = new Size(1200, 700);
                reportsForm.StartPosition = FormStartPosition.CenterParent;

                // Tạo TabControl để phân chia các loại báo cáo
                TabControl tabControl = new TabControl();
                tabControl.Dock = DockStyle.Fill;

                // Tab 1: Thống kê tổng quan
                TabPage dashboardTab = new TabPage("Thống kê tổng quan");
                DataGridView dgvDashboard = new DataGridView();
                dgvDashboard.Dock = DockStyle.Fill;
                dgvDashboard.ReadOnly = true;
                dgvDashboard.AllowUserToAddRows = false;
                dgvDashboard.AllowUserToDeleteRows = false;

                try
                {
                    DataTable dashboardData = ReportRepository.GetDashboardStats();
                    dgvDashboard.DataSource = dashboardData;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải thống kê tổng quan: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                dashboardTab.Controls.Add(dgvDashboard);
                tabControl.TabPages.Add(dashboardTab);

                // Tab 2: Báo cáo doanh thu sản phẩm
                TabPage productRevenueTab = new TabPage("Doanh thu sản phẩm");
                Panel productRevenuePanel = new Panel();
                productRevenuePanel.Dock = DockStyle.Fill;

                DateTimePicker dtpStartDate = new DateTimePicker();
                dtpStartDate.Location = new Point(10, 10);
                dtpStartDate.Size = new Size(150, 20);
                dtpStartDate.Value = DateTime.Now.AddDays(-30);

                DateTimePicker dtpEndDate = new DateTimePicker();
                dtpEndDate.Location = new Point(170, 10);
                dtpEndDate.Size = new Size(150, 20);
                dtpEndDate.Value = DateTime.Now;

                Button btnLoadProductRevenue = new Button();
                btnLoadProductRevenue.Text = "Tải báo cáo";
                btnLoadProductRevenue.Location = new Point(330, 8);
                btnLoadProductRevenue.Size = new Size(100, 25);
                btnLoadProductRevenue.Click += (s, ev) => LoadProductRevenueReport(dtpStartDate.Value, dtpEndDate.Value, productRevenuePanel);

                Label lblStartDate = new Label();
                lblStartDate.Text = "Từ ngày:";
                lblStartDate.Location = new Point(10, 35);
                lblStartDate.Size = new Size(60, 15);

                Label lblEndDate = new Label();
                lblEndDate.Text = "Đến ngày:";
                lblEndDate.Location = new Point(170, 35);
                lblEndDate.Size = new Size(60, 15);

                DataGridView dgvProductRevenue = new DataGridView();
                dgvProductRevenue.Location = new Point(10, 60);
                dgvProductRevenue.Size = new Size(1160, 580);
                dgvProductRevenue.ReadOnly = true;
                dgvProductRevenue.AllowUserToAddRows = false;
                dgvProductRevenue.AllowUserToDeleteRows = false;

                productRevenuePanel.Controls.AddRange(new Control[] { dtpStartDate, dtpEndDate, btnLoadProductRevenue, lblStartDate, lblEndDate, dgvProductRevenue });
                productRevenueTab.Controls.Add(productRevenuePanel);
                tabControl.TabPages.Add(productRevenueTab);

                // Tab 3: Tóm tắt giao dịch
                TabPage transactionTab = new TabPage("Tóm tắt giao dịch");
                DataGridView dgvTransactions = new DataGridView();
                dgvTransactions.Dock = DockStyle.Fill;
                dgvTransactions.ReadOnly = true;
                dgvTransactions.AllowUserToAddRows = false;
                dgvTransactions.AllowUserToDeleteRows = false;

                try
                {
                    DataTable transactionData = ReportRepository.GetTransactionSummary();
                    dgvTransactions.DataSource = transactionData;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải tóm tắt giao dịch: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                transactionTab.Controls.Add(dgvTransactions);
                tabControl.TabPages.Add(transactionTab);

                // Tab 4: Tóm tắt tài khoản
                TabPage accountTab = new TabPage("Tóm tắt tài khoản");
                DataGridView dgvAccounts = new DataGridView();
                dgvAccounts.Dock = DockStyle.Fill;
                dgvAccounts.ReadOnly = true;
                dgvAccounts.AllowUserToAddRows = false;
                dgvAccounts.AllowUserToDeleteRows = false;

                try
                {
                    DataTable accountData = ReportRepository.GetAccountSummary();
                    dgvAccounts.DataSource = accountData;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải tóm tắt tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                accountTab.Controls.Add(dgvAccounts);
                tabControl.TabPages.Add(accountTab);

                reportsForm.Controls.Add(tabControl);
                ShowFormInPanel(reportsForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProductRevenueReport(DateTime startDate, DateTime endDate, Panel parentPanel)
        {
            try
            {
                DataTable productRevenueData = ReportRepository.GetProductRevenueReport(startDate, endDate);
                
                // Tìm DataGridView trong panel
                DataGridView dgv = null;
                foreach (Control control in parentPanel.Controls)
                {
                    if (control is DataGridView)
                    {
                        dgv = control as DataGridView;
                        break;
                    }
                }

                if (dgv != null)
                {
                    dgv.DataSource = productRevenueData;
                    
                    if (dgv.Columns.Count > 0)
                    {
                        dgv.Columns["ProductID"].HeaderText = "Mã sản phẩm";
                        dgv.Columns["ProductName"].HeaderText = "Tên sản phẩm";
                        dgv.Columns["Unit"].HeaderText = "Đơn vị";
                        dgv.Columns["TotalQuantitySold"].HeaderText = "Tổng số lượng bán";
                        dgv.Columns["TotalRevenue"].HeaderText = "Tổng doanh thu";
                        dgv.Columns["AveragePrice"].HeaderText = "Giá trung bình";
                        dgv.Columns["NumberOfOrders"].HeaderText = "Số đơn hàng";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải báo cáo doanh thu sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new Forms.AccountInfoForm(currentUsername, currentRole));
        }

        private void btn_Logout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                // Đóng tất cả form con trước
                var formsToClose = new List<Form>();
                foreach (Form form in Application.OpenForms)
                {
                    if (form != this && !(form is LoginForm))
                    {
                        formsToClose.Add(form);
                    }
                }
                
                // Đóng tất cả form con
                foreach (Form form in formsToClose)
                {
                    form.Close();
                }
                
                // Đóng form chính - sẽ trigger FormClosed event và quay lại LoginForm
                this.Close();
            }
        }

        private void msi_Statistics_Click(object sender, EventArgs e)
        {
            try
            {
                // Debug: Hiển thị role hiện tại
                MessageBox.Show($"Role hiện tại: '{currentRole}'", "Debug Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Kiểm tra quyền admin hoặc manager
                string roleLower = currentRole?.ToLower() ?? "";
                if (roleLower != "admin" && roleLower != "manager")
                {
                    MessageBox.Show("Chỉ có Admin và Manager mới có thể truy cập chức năng thống kê!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Mở form thống kê
                Forms.Manager.StatisticsForm statisticsForm = new Forms.Manager.StatisticsForm(currentUsername);
                ShowFormInPanel(statisticsForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở form thống kê: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
