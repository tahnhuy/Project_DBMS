using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sale_Management.Forms
{
    public partial class SalerForm : Form
    {
        public SalerForm()
        {
            InitializeComponent();
            
            // Hiển thị form mặc định khi khởi động
            ShowFormInPanel(new SalerProductForm());
        }

        private void ShowFormInPanel(Form form)
        {
            panel_Container.Controls.Clear();

            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            panel_Container.Controls.Add(form);
            form.Show();
        }

        private void msi_Product_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new SalerProductForm());
        }

        private void msi_Customer_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new SalerCustomerViewForm());
        }


        private void msi_CreateInvoice_Click(object sender, EventArgs e)
        {
            ShowFormInPanel(new SalerInvoiceForm());
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                this.Hide();
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
            }
        }
    }
}

