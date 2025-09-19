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
    public partial class SalerCustomerViewForm : Form
    {
        public SalerCustomerViewForm()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            try
            {
                CustomerRepository customerRepo = new CustomerRepository();
                DataTable dt = customerRepo.GetAllCustomers();
                dgv_Customers.DataSource = dt;
                
                // Định dạng cột
                if (dgv_Customers.Columns.Count > 0)
                {
                    dgv_Customers.Columns[0].HeaderText = "Mã khách hàng";
                    dgv_Customers.Columns[1].HeaderText = "Tên khách hàng";
                    dgv_Customers.Columns[2].HeaderText = "Email";
                    dgv_Customers.Columns[3].HeaderText = "Số điện thoại";
                    dgv_Customers.Columns[4].HeaderText = "Địa chỉ";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            try
            {
                string searchTerm = txt_Search.Text.Trim();
                if (string.IsNullOrEmpty(searchTerm))
                {
                    LoadCustomers();
                    return;
                }

                CustomerRepository customerRepo = new CustomerRepository();
                DataTable dt = customerRepo.SearchCustomers(searchTerm);
                dgv_Customers.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
