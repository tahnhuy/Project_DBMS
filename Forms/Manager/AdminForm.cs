using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sale_Management
{
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();

            ShowFormInPanel(new Forms.ProductForm());
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

    }
}
