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
    public partial class AccountForm : Form
    {
        public AccountForm()
        {
            InitializeComponent();
            LoadAccounts();
        }

        private void LoadAccounts()
        {
            try
            {
                DataTable accounts = AccountRepository.GetAllAccounts();
                dataGridView_Accounts.DataSource = accounts;
                
                // Set column headers
                if (dataGridView_Accounts.Columns.Count > 0)
                {
                    dataGridView_Accounts.Columns["Username"].HeaderText = "Tên đăng nhập";
                    dataGridView_Accounts.Columns["Role"].HeaderText = "Vai trò";
                    dataGridView_Accounts.Columns["CreatedDate"].HeaderText = "Ngày tạo";
                    
                    // Hide password column if it exists
                    if (dataGridView_Accounts.Columns.Contains("Password"))
                    {
                        dataGridView_Accounts.Columns["Password"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btn_Edit_Click(object sender, EventArgs e)
        {
            if (dataGridView_Accounts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần chỉnh sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string username = dataGridView_Accounts.SelectedRows[0].Cells["Username"].Value.ToString();
            string currentRole = dataGridView_Accounts.SelectedRows[0].Cells["Role"].Value.ToString();

            // Show edit dialog
            using (var editForm = new AccountManageEditForm(username, currentRole))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadAccounts(); // Refresh the list
                }
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (dataGridView_Accounts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string username = dataGridView_Accounts.SelectedRows[0].Cells["Username"].Value.ToString();
            
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa tài khoản '{username}'?", 
                "Xác nhận xóa", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    DataTable deleteResult = AccountRepository.DeleteAccount(username);
                    
                    if (deleteResult.Rows.Count > 0)
                    {
                        string resultStatus = deleteResult.Rows[0]["Result"].ToString();
                        string message = deleteResult.Rows[0]["Message"].ToString();

                        if (resultStatus == "SUCCESS")
                        {
                            MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadAccounts();
                        }
                        else
                        {
                            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            LoadAccounts();
        }

        private void txt_Search_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_Search.Text))
                {
                    LoadAccounts();
                    return;
                }

                DataTable searchResult = AccountRepository.SearchAccounts(txt_Search.Text.Trim());
                dataGridView_Accounts.DataSource = searchResult;
                
                // Set column headers
                if (dataGridView_Accounts.Columns.Count > 0)
                {
                    dataGridView_Accounts.Columns["Username"].HeaderText = "Tên đăng nhập";
                    dataGridView_Accounts.Columns["Role"].HeaderText = "Vai trò";
                    dataGridView_Accounts.Columns["CreatedDate"].HeaderText = "Ngày tạo";
                    
                    // Hide password column if it exists
                    if (dataGridView_Accounts.Columns.Contains("Password"))
                    {
                        dataGridView_Accounts.Columns["Password"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void dataGridView_Accounts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btn_Edit_Click(sender, e);
            }
        }
    }
}
