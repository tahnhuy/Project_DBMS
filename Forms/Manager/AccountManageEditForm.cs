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

namespace Sale_Management.Forms.Manager
{
    public partial class AccountManageEditForm : Form
    {
        private string _username;
        private AccountRepository accountRepository;
        private DataTable accountData;

        public AccountManageEditForm(string username)
        {
            InitializeComponent();
            _username = username;
            accountRepository = new AccountRepository();
            LoadAccountData();
            SetupForm();
        }

        private void SetupForm()
        {
            try
            {
                // Thiết lập ComboBox vai trò
                cmb_Role.Items.Clear();
                cmb_Role.Items.Add("manager");
                cmb_Role.Items.Add("saler");
                cmb_Role.Items.Add("customer");

                // Load dữ liệu cho ComboBox khách hàng và nhân viên
                LoadCustomersAndEmployees();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thiết lập form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCustomersAndEmployees()
        {
            try
            {
                // Load customers
                DataTable customers = accountRepository.GetCustomersForAccount();
                cmb_Customer.DataSource = customers;
                cmb_Customer.DisplayMember = "CustomerName";
                cmb_Customer.ValueMember = "CustomerID";
                cmb_Customer.SelectedIndex = -1;

                // Load employees
                DataTable employees = accountRepository.GetEmployeesForAccount();
                cmb_Employee.DataSource = employees;
                cmb_Employee.DisplayMember = "EmployeeName";
                cmb_Employee.ValueMember = "EmployeeID";
                cmb_Employee.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAccountData()
        {
            try
            {
                accountData = accountRepository.GetAccountByUsername(_username);
                if (accountData != null && accountData.Rows.Count > 0)
                {
                    DataRow row = accountData.Rows[0];
                    
                    txt_Username.Text = row["Username"].ToString();
                    txt_Username.ReadOnly = true;
                    txt_Username.BackColor = SystemColors.Control;
                    
                    txt_FullName.Text = row["FullName"].ToString();
                    txt_Phone.Text = row["Phone"].ToString();
                    txt_Address.Text = row["Address"].ToString();
                    txt_Position.Text = row["Position"].ToString();
                    
                    cmb_Role.SelectedItem = row["Role"].ToString();
                    
                    // Set CustomerID or EmployeeID based on role
                    if (row["CustomerID"] != DBNull.Value)
                    {
                        cmb_Customer.SelectedValue = Convert.ToInt32(row["CustomerID"]);
                    }
                    if (row["EmployeeID"] != DBNull.Value)
                    {
                        cmb_Employee.SelectedValue = Convert.ToInt32(row["EmployeeID"]);
                    }
                    
                    // Show/hide appropriate controls based on role
                    UpdateRoleControls();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmb_Role_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateRoleControls();
        }

        private void UpdateRoleControls()
        {
            try
            {
                if (cmb_Role.SelectedItem != null)
                {
                    string role = cmb_Role.SelectedItem.ToString();
                    
                    // Hiển thị/ẩn các ComboBox tương ứng
                    if (role == "customer")
                    {
                        lbl_Customer.Visible = true;
                        cmb_Customer.Visible = true;
                        lbl_Employee.Visible = false;
                        cmb_Employee.Visible = false;
                    }
                    else if (role == "manager" || role == "saler")
                    {
                        lbl_Customer.Visible = false;
                        cmb_Customer.Visible = false;
                        lbl_Employee.Visible = true;
                        cmb_Employee.Visible = true;
                    }
                    else
                    {
                        lbl_Customer.Visible = false;
                        cmb_Customer.Visible = false;
                        lbl_Employee.Visible = false;
                        cmb_Employee.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thay đổi vai trò: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_Password.Text) && cmb_Role.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu mới hoặc chọn vai trò mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!string.IsNullOrWhiteSpace(txt_Password.Text))
                {
                    if (txt_Password.Text.Length < 6)
                    {
                        MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txt_Password.Focus();
                        return;
                    }

                    if (txt_Password.Text != txt_ConfirmPassword.Text)
                    {
                        MessageBox.Show("Mật khẩu xác nhận không khớp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txt_ConfirmPassword.Focus();
                        return;
                    }
                }

                string newPassword = string.IsNullOrWhiteSpace(txt_Password.Text) ? null : txt_Password.Text;
                string newRole = cmb_Role.SelectedItem?.ToString();
                int? customerId = null;
                int? employeeId = null;

                // Validate based on role
                if (newRole == "customer")
                {
                    if (cmb_Customer.SelectedValue == null)
                    {
                        MessageBox.Show("Vui lòng chọn khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmb_Customer.Focus();
                        return;
                    }
                    customerId = Convert.ToInt32(cmb_Customer.SelectedValue);
                }
                else if (newRole == "manager" || newRole == "saler")
                {
                    if (cmb_Employee.SelectedValue == null)
                    {
                        MessageBox.Show("Vui lòng chọn nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmb_Employee.Focus();
                        return;
                    }
                    employeeId = Convert.ToInt32(cmb_Employee.SelectedValue);
                }

                // Update account
                bool success = accountRepository.UpdateAccount(
                    _username,
                    newPassword,
                    newRole,
                    customerId,
                    employeeId
                );

                if (success)
                {
                    MessageBox.Show("Cập nhật tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật tài khoản. Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            txt_Password.Clear();
            txt_ConfirmPassword.Clear();
            txt_Password.Focus();
        }

        private void chk_ShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txt_Password.PasswordChar = chk_ShowPassword.Checked ? '\0' : '*';
            txt_ConfirmPassword.PasswordChar = chk_ShowPassword.Checked ? '\0' : '*';
        }

        private void txt_Password_TextChanged(object sender, EventArgs e)
        {

        }
    }
}