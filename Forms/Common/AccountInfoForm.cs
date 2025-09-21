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
    public partial class AccountInfoForm : Form
    {
        private string currentUsername;
        private string currentRole;

        public AccountInfoForm(string username, string role)
        {
            InitializeComponent();
            currentUsername = username;
            currentRole = role;
            LoadAccountInfo();
        }

        private void LoadAccountInfo()
        {
            try
            {
                // Kiểm tra username có hợp lệ không
                if (string.IsNullOrEmpty(currentUsername))
                {
                    MessageBox.Show("Không có thông tin người dùng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy thông tin từ bảng Account
                string query = "SELECT Username, Role, CreatedDate, EmployeeID, CustomerID FROM Account WHERE Username = @Username";
                
                var parameters = new System.Data.SqlClient.SqlParameter[] {
                    new System.Data.SqlClient.SqlParameter("@Username", currentUsername ?? "")
                };
                
                var accountData = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
                
                if (accountData != null && accountData.Rows.Count > 0)
                {
                    DataRow accountRow = accountData.Rows[0];
                    
                    // Hiển thị thông tin cơ bản
                    lblUsername.Text = "Tên đăng nhập:";
                    lblUsernameValue.Text = accountRow["Username"].ToString();
                    lblRole.Text = "Vai trò:";
                    lblRoleValue.Text = accountRow["Role"].ToString();
                    lblCreatedDate.Text = "Ngày tạo:";
                    lblCreatedDateValue.Text = Convert.ToDateTime(accountRow["CreatedDate"]).ToString("dd/MM/yyyy");

                    // Lấy thông tin nhân viên nếu có EmployeeID
                    if (accountRow["EmployeeID"] != DBNull.Value)
                    {
                        int employeeId = Convert.ToInt32(accountRow["EmployeeID"]);
                        
                        string employeeQuery = "SELECT EmployeeID, EmployeeName, Phone, Address, Position, HireDate, Salary FROM Employees WHERE EmployeeID = @EmployeeID";
                        
                        var employeeParams = new System.Data.SqlClient.SqlParameter[] {
                            new System.Data.SqlClient.SqlParameter("@EmployeeID", employeeId)
                        };
                        
                        var employeeData = DatabaseConnection.ExecuteQuery(employeeQuery, CommandType.Text, employeeParams);
                        
                        if (employeeData != null && employeeData.Rows.Count > 0)
                        {
                            DataRow employeeRow = employeeData.Rows[0];
                            
                            lblEmployeeName.Text = "Tên nhân viên:";
                            lblEmployeeNameValue.Text = employeeRow["EmployeeName"].ToString();
                            lblPhone.Text = "Số điện thoại:";
                            lblPhoneValue.Text = employeeRow["Phone"].ToString();
                            lblAddress.Text = "Địa chỉ:";
                            lblAddressValue.Text = employeeRow["Address"] != DBNull.Value ? employeeRow["Address"].ToString() : "Không có";
                            lblPosition.Text = "Chức vụ:";
                            lblPositionValue.Text = employeeRow["Position"].ToString();
                            lblHireDate.Text = "Ngày vào làm:";
                            lblHireDateValue.Text = Convert.ToDateTime(employeeRow["HireDate"]).ToString("dd/MM/yyyy");
                            lblSalary.Text = "Lương:";
                            
                            if (employeeRow["Salary"] != DBNull.Value)
                            {
                                decimal salary = Convert.ToDecimal(employeeRow["Salary"]);
                                lblSalaryValue.Text = salary.ToString("N0") + " VND";
                            }
                            else
                            {
                                lblSalaryValue.Text = "Không có";
                            }
                        }
                    }
                    else
                    {
                        // Nếu không có EmployeeID, ẩn các thông tin nhân viên
                        lblEmployeeName.Text = "Tên nhân viên:";
                        lblEmployeeNameValue.Text = "Không có";
                        lblPhone.Text = "Số điện thoại:";
                        lblPhoneValue.Text = "Không có";
                        lblAddress.Text = "Địa chỉ:";
                        lblAddressValue.Text = "Không có";
                        lblPosition.Text = "Chức vụ:";
                        lblPositionValue.Text = "Không có";
                        lblHireDate.Text = "Ngày vào làm:";
                        lblHireDateValue.Text = "Không có";
                        lblSalary.Text = "Lương:";
                        lblSalaryValue.Text = "Không có";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin tài khoản: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo form đổi mật khẩu đơn giản
                Form changePasswordForm = new Form();
                changePasswordForm.Text = "Đổi mật khẩu";
                changePasswordForm.Size = new Size(350, 200);
                changePasswordForm.StartPosition = FormStartPosition.CenterParent;
                changePasswordForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                changePasswordForm.MaximizeBox = false;
                changePasswordForm.MinimizeBox = false;

                Label lblOldPassword = new Label();
                lblOldPassword.Text = "Mật khẩu cũ:";
                lblOldPassword.Location = new Point(20, 20);
                lblOldPassword.Size = new Size(100, 20);

                TextBox txtOldPassword = new TextBox();
                txtOldPassword.Location = new Point(130, 18);
                txtOldPassword.Size = new Size(180, 20);
                txtOldPassword.PasswordChar = '*';

                Label lblNewPassword = new Label();
                lblNewPassword.Text = "Mật khẩu mới:";
                lblNewPassword.Location = new Point(20, 50);
                lblNewPassword.Size = new Size(100, 20);

                TextBox txtNewPassword = new TextBox();
                txtNewPassword.Location = new Point(130, 48);
                txtNewPassword.Size = new Size(180, 20);
                txtNewPassword.PasswordChar = '*';

                Label lblConfirmPassword = new Label();
                lblConfirmPassword.Text = "Xác nhận:";
                lblConfirmPassword.Location = new Point(20, 80);
                lblConfirmPassword.Size = new Size(100, 20);

                TextBox txtConfirmPassword = new TextBox();
                txtConfirmPassword.Location = new Point(130, 78);
                txtConfirmPassword.Size = new Size(180, 20);
                txtConfirmPassword.PasswordChar = '*';

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

                changePasswordForm.Controls.AddRange(new Control[] { 
                    lblOldPassword, txtOldPassword, 
                    lblNewPassword, txtNewPassword, 
                    lblConfirmPassword, txtConfirmPassword, 
                    btnOK, btnCancel 
                });

                if (changePasswordForm.ShowDialog() == DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(txtOldPassword.Text) || 
                        string.IsNullOrEmpty(txtNewPassword.Text) || 
                        string.IsNullOrEmpty(txtConfirmPassword.Text))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (txtNewPassword.Text != txtConfirmPassword.Text)
                    {
                        MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Validate password format
                    if (!SecurityHelper.ValidatePassword(txtNewPassword.Text))
                    {
                        MessageBox.Show("Mật khẩu không hợp lệ (ít nhất 6 ký tự)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    bool success = SecurityHelper.ChangePassword(currentUsername, txtOldPassword.Text, txtNewPassword.Text);
                    if (success)
                    {
                        MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đổi mật khẩu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewActivity_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable activityData = AccountRepository.GetAccountActivity(currentUsername);
                
                if (activityData.Rows.Count == 0)
                {
                    MessageBox.Show("Không có hoạt động nào được ghi nhận.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Tạo form hiển thị hoạt động
                Form activityForm = new Form();
                activityForm.Text = "Hoạt động tài khoản - " + currentUsername;
                activityForm.Size = new Size(800, 500);
                activityForm.StartPosition = FormStartPosition.CenterParent;

                DataGridView dgvActivity = new DataGridView();
                dgvActivity.DataSource = activityData;
                dgvActivity.Dock = DockStyle.Fill;
                dgvActivity.ReadOnly = true;
                dgvActivity.AllowUserToAddRows = false;
                dgvActivity.AllowUserToDeleteRows = false;

                activityForm.Controls.Add(dgvActivity);
                activityForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xem hoạt động: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBackupAccount_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable backupData = AccountRepository.BackupAccounts();
                
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                saveDialog.FileName = $"AccountBackup_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    // Export to CSV
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(saveDialog.FileName, false, System.Text.Encoding.UTF8))
                    {
                        // Write headers
                        for (int i = 0; i < backupData.Columns.Count; i++)
                        {
                            writer.Write(backupData.Columns[i].ColumnName);
                            if (i < backupData.Columns.Count - 1)
                                writer.Write(",");
                        }
                        writer.WriteLine();

                        // Write data
                        foreach (DataRow row in backupData.Rows)
                        {
                            for (int i = 0; i < backupData.Columns.Count; i++)
                            {
                                writer.Write(row[i].ToString());
                                if (i < backupData.Columns.Count - 1)
                                    writer.Write(",");
                            }
                            writer.WriteLine();
                        }
                    }
                    MessageBox.Show("Backup tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi backup tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
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
    }
}
