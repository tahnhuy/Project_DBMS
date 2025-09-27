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
    public partial class AccountManageEditForm : Form
    {
        private string _username;
        private string _currentRole;

        public AccountManageEditForm(string username, string currentRole)
        {
            InitializeComponent();
            _username = username;
            _currentRole = currentRole;
            LoadAccountData();
        }

        private void LoadAccountData()
        {
            try
            {
                textBox1.Text = _username;
                textBox1.ReadOnly = true;
                textBox1.BackColor = SystemColors.Control;

                comboBox1.SelectedItem = _currentRole;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox2.Text) && comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu mới hoặc chọn vai trò mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    if (textBox2.Text != textBox3.Text)
                    {
                        MessageBox.Show("Mật khẩu xác nhận không khớp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBox3.Focus();
                        return;
                    }
                }

                string newPassword = string.IsNullOrWhiteSpace(textBox2.Text) ? null : textBox2.Text;
                string newRole = comboBox1.SelectedItem?.ToString();

                MessageBox.Show("Chức năng chỉnh sửa tài khoản đã được tắt.", 
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            textBox3.Clear();
            textBox2.Focus();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = checkBox1.Checked ? '\0' : '*';
            textBox3.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }
    }
}