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
    public partial class AccountCreateForm : Form
    {
        private AccountRepository accountRepository;
        
        public AccountCreateForm()
        {
            InitializeComponent();
            accountRepository = new AccountRepository();
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
                cmb_Role.SelectedIndex = 0;

                // Load giá trị mặc định cho CustomerID và EmployeeID
                LoadDefaultIDs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thiết lập form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDefaultIDs()
        {
            try
            {
                // Không cần load ID mặc định nữa vì stored procedure sẽ tự động tạo
                // Chỉ cần clear các textbox
                txt_CustomerID.Clear();
                txt_EmployeeID.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải ID mặc định: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmb_Role_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmb_Role.SelectedItem != null)
                {
                    string role = cmb_Role.SelectedItem.ToString();
                    
                    // Ẩn tất cả các TextBox vì stored procedure sẽ tự động tạo Customer/Employee
                    lbl_CustomerID.Visible = false;
                    txt_CustomerID.Visible = false;
                    lbl_EmployeeID.Visible = false;
                    txt_EmployeeID.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thay đổi vai trò: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(txt_Username.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_Username.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txt_Password.Text))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_Password.Focus();
                    return;
                }

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

                if (cmb_Role.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn vai trò!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmb_Role.Focus();
                    return;
                }

                string role = cmb_Role.SelectedItem.ToString();

                // Check if username already exists
                if (accountRepository.IsUsernameExists(txt_Username.Text.Trim()))
                {
                    MessageBox.Show("Tên đăng nhập đã được sử dụng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_Username.Focus();
                    return;
                }

                // Create account - stored procedure sẽ tự động tạo Customer/Employee
                bool success = accountRepository.AddAccount(
                    txt_Username.Text.Trim(),
                    txt_Password.Text,
                    role,
                    null, // CustomerID = null (stored procedure sẽ tự động tạo)
                    null  // EmployeeID = null (stored procedure sẽ tự động tạo)
                );

                if (success)
                {
                    // Tạo SQL Account (Login và User) sau khi tạo Account thành công
                    try
                    {
                        bool sqlSuccess = accountRepository.CreateSQLAccount(
                            txt_Username.Text.Trim(),
                            txt_Password.Text,
                            role
                        );
                        
                        if (sqlSuccess)
                        {
                            MessageBox.Show("Tạo tài khoản và SQL Account thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Tạo tài khoản thành công nhưng không thể tạo SQL Account.\n\nVui lòng kiểm tra quyền và cấu hình database.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception sqlEx)
                    {
                        MessageBox.Show($"Tạo tài khoản thành công nhưng lỗi khi tạo SQL Account:\n{sqlEx.Message}\n\nVui lòng kiểm tra quyền và cấu hình database.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể tạo tài khoản. Vui lòng kiểm tra lại.\n\nChi tiết: Stored procedure trả về false", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txt_Username.Clear();
            txt_Password.Clear();
            txt_ConfirmPassword.Clear();
            cmb_Role.SelectedIndex = 0;
            
            // Reload default IDs
            LoadDefaultIDs();
            
            txt_Username.Focus();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}