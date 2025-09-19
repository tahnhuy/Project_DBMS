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
    public partial class CustomerEditForm : Form
    {
        private readonly string _username;
        private readonly DatabaseAccess.CustomerRepository _customerRepo = new DatabaseAccess.CustomerRepository();

        public CustomerEditForm(string username)
        {
            InitializeComponent();
            _username = username;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadCurrent();
        }

        private void LoadCurrent()
        {
            var dt = _customerRepo.GetCustomerByUsername(_username);
            if (dt.Rows.Count > 0)
            {
                var r = dt.Rows[0];
                txtName.Text = r["CustomerName"]?.ToString();
                txtPhone.Text = r["Phone"]?.ToString();
                txtAddress.Text = r["Address"]?.ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _customerRepo.UpdateCustomerByUsername(_username, txtName.Text.Trim(), txtPhone.Text.Trim(), txtAddress.Text.Trim());
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
