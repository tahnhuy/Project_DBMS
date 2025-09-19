using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sale_Management.DatabaseAccess;

namespace Sale_Management.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

		private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
		{
			txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void btnLogin_Click(object sender, EventArgs e)
		{
			string username = txtUsername.Text.Trim();
			string password = txtPassword.Text;

			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
			{
				MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			try
			{
				SqlParameter pUser = new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username };
				SqlParameter pPass = new SqlParameter("@Password", SqlDbType.NVarChar, 255) { Value = password };
				DataTable dt = DatabaseConnection.ExecuteQuery("CheckLogin", CommandType.StoredProcedure, pUser, pPass);

				if (dt.Rows.Count == 0)
				{
					MessageBox.Show("Không thể kết nối hoặc không có phản hồi từ máy chủ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				DataRow row = dt.Rows[0];
				string result = row["Result"].ToString();
				string message = row.Table.Columns.Contains("Message") ? row["Message"].ToString() : string.Empty;

				if (result == "ERROR")
				{
					MessageBox.Show(string.IsNullOrEmpty(message) ? "Tên đăng nhập hoặc mật khẩu không đúng" : message, "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				string role = row.Table.Columns.Contains("Role") ? row["Role"].ToString() : string.Empty;
				if (string.IsNullOrEmpty(role))
				{
					MessageBox.Show("Không xác định được vai trò người dùng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

                // Route theo role
				this.Hide();
				Form mainForm = null;
				if (role.Equals("manager", StringComparison.OrdinalIgnoreCase))
				{
					mainForm = new AdminForm();
				}
				else if (role.Equals("saler", StringComparison.OrdinalIgnoreCase))
				{
					mainForm = new SalerForm();
				}
                else if (role.Equals("customer", StringComparison.OrdinalIgnoreCase))
                {
                    mainForm = new CustomerForm(username);
                }
				else
				{
					mainForm = new ProductForm();
				}

				mainForm.FormClosed += (s, args) => this.Close();
				mainForm.Show();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi đăng nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
    }
}
