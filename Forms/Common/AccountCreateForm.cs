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
        public AccountCreateForm()
        {
            InitializeComponent();
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

                // Get creator username (you might want to get this from login session)
                string creatorUsername = "admin"; // Default creator, should be from login session

                // Create account
                DataTable result = AccountRepository.AddAccount(
                    creatorUsername,
                    txt_Username.Text.Trim(),
                    txt_Password.Text,
                    cmb_Role.SelectedItem.ToString()
                );

                if (result.Rows.Count > 0)
                {
                    string resultStatus = result.Rows[0]["Result"].ToString();
                    string message = result.Rows[0]["Message"].ToString();

                    if (resultStatus == "SUCCESS")
                    {
                        MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
            cmb_Role.SelectedIndex = -1;
            txt_Username.Focus();
        }
    }
}