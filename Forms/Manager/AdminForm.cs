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

        private void msi_Discount_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new Forms.AdminDiscountForm());
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

    }
}
