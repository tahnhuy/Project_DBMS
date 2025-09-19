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
    public partial class CustomerForm : Form
    {
        private readonly string _username;
        private readonly DatabaseAccess.CustomerRepository _customerRepo = new DatabaseAccess.CustomerRepository();

        public CustomerForm(string username)
        {
            InitializeComponent();
            _username = username;
        }

        private void CustomerForm_Load(object sender, EventArgs e)
        {
            LoadProfile();
            LoadSalesHistory();
        }

        private void LoadProfile()
        {
            var dt = _customerRepo.GetCustomerByUsername(_username);
            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                txtName.Text = row["CustomerName"]?.ToString();
                txtPhone.Text = row["Phone"]?.ToString();
                txtAddress.Text = row["Address"]?.ToString();
                lblPoints.Text = row["LoyaltyPoints"]?.ToString();
            }
            else
            {
                txtName.Text = string.Empty;
                txtPhone.Text = string.Empty;
                txtAddress.Text = string.Empty;
                lblPoints.Text = "0";
            }
        }

        private void LoadSalesHistory()
        {
            var dt = _customerRepo.GetSalesByCustomerUsername(_username);
            dgvSales.DataSource = dt;
            if (dgvSales.Columns.Contains("SaleID")) dgvSales.Columns["SaleID"].HeaderText = "Mã HĐ";
            if (dgvSales.Columns.Contains("SaleDate")) dgvSales.Columns["SaleDate"].HeaderText = "Ngày bán";
            if (dgvSales.Columns.Contains("TotalAmount")) dgvSales.Columns["TotalAmount"].HeaderText = "Tổng tiền";
            if (dgvSales.Columns.Contains("PaymentMethod")) dgvSales.Columns["PaymentMethod"].HeaderText = "Thanh toán";
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            using (var f = new CustomerEditForm(_username))
            {
                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    LoadProfile();
                }
            }
        }
    }
}
