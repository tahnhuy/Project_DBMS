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
using Sale_Management.Forms;

namespace Sale_Management.Forms.Manager
{
    public partial class AccountForm : Form
    {
        private AccountRepository accountRepository;
        
        public AccountForm()
        {
            try
            {
                InitializeComponent();
                accountRepository = new AccountRepository();
                LoadAccounts();
                SetupComboBoxes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo form: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAccounts()
        {
            try
            {
                DataTable dt = accountRepository.GetAllAccounts();
                if (dt != null)
                {
                    dgv_Accounts.DataSource = dt;
                    ConfigureDataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu tài khoản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void ConfigureDataGridView()
        {
            try
            {
                if (dgv_Accounts == null) return;
                
                if (dgv_Accounts.Columns != null && dgv_Accounts.Columns.Count > 0)
                {
                    // Thiết lập header text cho các cột
                    if (dgv_Accounts.Columns.Contains("Username"))
                    {
                        var usernameColumn = dgv_Accounts.Columns["Username"];
                        if (usernameColumn != null) usernameColumn.HeaderText = "Tên đăng nhập";
                    }
                    if (dgv_Accounts.Columns.Contains("Role"))
                    {
                        var roleColumn = dgv_Accounts.Columns["Role"];
                        if (roleColumn != null) roleColumn.HeaderText = "Vai trò";
                    }
                    if (dgv_Accounts.Columns.Contains("FullName"))
                    {
                        var fullNameColumn = dgv_Accounts.Columns["FullName"];
                        if (fullNameColumn != null) fullNameColumn.HeaderText = "Họ tên";
                    }
                    if (dgv_Accounts.Columns.Contains("Phone"))
                    {
                        var phoneColumn = dgv_Accounts.Columns["Phone"];
                        if (phoneColumn != null) phoneColumn.HeaderText = "Số điện thoại";
                    }
                    if (dgv_Accounts.Columns.Contains("Address"))
                    {
                        var addressColumn = dgv_Accounts.Columns["Address"];
                        if (addressColumn != null) addressColumn.HeaderText = "Địa chỉ";
                    }
                    if (dgv_Accounts.Columns.Contains("Position"))
                    {
                        var positionColumn = dgv_Accounts.Columns["Position"];
                        if (positionColumn != null) positionColumn.HeaderText = "Chức vụ";
                    }
                    if (dgv_Accounts.Columns.Contains("CreatedDate"))
                    {
                        var createdDateColumn = dgv_Accounts.Columns["CreatedDate"];
                        if (createdDateColumn != null)
                        {
                            createdDateColumn.HeaderText = "Ngày tạo";
                            createdDateColumn.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                        }
                    }
                
                    // Ẩn một số cột không cần thiết
                    if (dgv_Accounts.Columns.Contains("CustomerID"))
                        dgv_Accounts.Columns["CustomerID"].Visible = false;
                    if (dgv_Accounts.Columns.Contains("EmployeeID"))
                        dgv_Accounts.Columns["EmployeeID"].Visible = false;
                
                    // Đổi màu cho các vai trò khác nhau
                    if (dgv_Accounts.Columns.Contains("Role"))
                    {
                        foreach (DataGridViewRow row in dgv_Accounts.Rows)
                        {
                            if (row.Cells["Role"].Value != null)
                            {
                                string role = row.Cells["Role"].Value.ToString();
                                switch (role)
                                {
                                    case "manager":
                                        row.DefaultCellStyle.BackColor = Color.LightBlue;
                                        break;
                                    case "saler":
                                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                                        break;
                                    case "customer":
                                        row.DefaultCellStyle.BackColor = Color.LightYellow;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thiết lập DataGridView: {ex.Message}\n\nChi tiết: {ex.StackTrace}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupComboBoxes()
        {
            try
            {
                // Thiết lập ComboBox cho tìm kiếm theo vai trò
                cmb_SearchRole.Items.Clear();
                cmb_SearchRole.Items.Add("Tất cả");
                cmb_SearchRole.Items.Add("manager");
                cmb_SearchRole.Items.Add("saler");
                cmb_SearchRole.Items.Add("customer");
                cmb_SearchRole.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thiết lập ComboBox: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        // Tìm kiếm tài khoản theo username
        private void SearchAccountsByUsername(string searchText)
        {
            try
            {
                DataTable dt;
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    dt = accountRepository.GetAllAccounts();
                }
                else
                {
                    dt = accountRepository.SearchAccountsByUsername(searchText);
                }
                dgv_Accounts.DataSource = dt;
                ConfigureDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm tài khoản: " + ex.Message);
            }
        }
        
        private void txt_SearchUsername_TextChanged(object sender, EventArgs e)
        {
            SearchAccountsByUsername(txt_SearchUsername.Text.Trim());

            if (txt_SearchUsername.Focused)
            {
                txt_SearchName.Text = string.Empty;
            }
        }

        // Tìm kiếm tài khoản theo tên
        private void SearchAccountsByName(string searchText)
        {
            try
            {
                DataTable dt;
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    dt = accountRepository.GetAllAccounts();
                }
                else
                {
                    dt = accountRepository.SearchAccountsByName(searchText);
                }
                dgv_Accounts.DataSource = dt;
                ConfigureDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm tài khoản: " + ex.Message);
            }
        }
        
        private void txt_SearchName_TextChanged(object sender, EventArgs e)
        {
            SearchAccountsByName(txt_SearchName.Text.Trim());

            if (txt_SearchName.Focused)
            {
                txt_SearchUsername.Text = string.Empty;
            }
        }

        // Tìm kiếm theo vai trò
        private void cmb_SearchRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt;
                if (cmb_SearchRole.SelectedIndex == 0) // Tất cả
                {
                    dt = accountRepository.GetAllAccounts();
                }
                else
                {
                    string role = cmb_SearchRole.SelectedItem.ToString();
                    dt = accountRepository.GetAccountsByRole(role);
                }
                dgv_Accounts.DataSource = dt;
                ConfigureDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm theo vai trò: " + ex.Message);
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            AccountCreateForm createForm = new AccountCreateForm();
            if (createForm.ShowDialog() == DialogResult.OK)
            {
                LoadAccounts(); // Tải lại danh sách tài khoản
            }
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            if (dgv_Accounts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dgv_Accounts.SelectedRows[0];
            string username = selectedRow.Cells["Username"].Value.ToString();

            AccountManageEditForm editForm = new AccountManageEditForm(username);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                LoadAccounts(); // Tải lại danh sách tài khoản
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (dgv_Accounts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dgv_Accounts.SelectedRows[0];
            string username = selectedRow.Cells["Username"].Value.ToString();
            string fullName = selectedRow.Cells["FullName"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa tài khoản '{username}' ({fullName}) không?\n\nLưu ý: Không thể xóa tài khoản đã có giao dịch.",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    bool success = accountRepository.DeleteAccount(username);
                    if (success)
                    {
                        MessageBox.Show("Xóa tài khoản thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAccounts(); // Reload the account list
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa tài khoản. Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            txt_SearchUsername.Text = string.Empty;
            txt_SearchName.Text = string.Empty;
            cmb_SearchRole.SelectedIndex = 0;
        }
    }
}
