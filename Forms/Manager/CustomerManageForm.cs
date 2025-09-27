using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Sale_Management.DatabaseAccess;

namespace Sale_Management.Forms
{
    public partial class CustomerManageForm : Form
    {
        private CustomerRepository customerRepository;
        public CustomerManageForm()
        {
            InitializeComponent();
            customerRepository = new CustomerRepository();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            try
            {
                DataTable dt = DatabaseConnection.ExecuteQuery("GetAllCustomers", CommandType.StoredProcedure, null);
                dgv_Customers.DataSource = dt;
                ConfigureDataGridView();
            } catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu khách hàng: " + ex.Message);
            }
        }
        private void ConfigureDataGridView()
        {
            if (dgv_Customers.Columns.Count > 0)
            {
                dgv_Customers.Columns["CustomerID"].HeaderText = "Mã khách hàng";
                dgv_Customers.Columns["CustomerName"].HeaderText = "Tên khách hàng";
                dgv_Customers.Columns["Phone"].HeaderText = "Số điện thoại";
                dgv_Customers.Columns["Address"].HeaderText = "Địa chỉ";
                dgv_Customers.Columns["LoyaltyPoints"].HeaderText = "Điểm tích lũy";

            }
        }

        // Tim kiem khach hang theo ten 
        private void SearchCustomersByName(string searchText)
        {
            try
            {
                DataTable dt;
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    dt = DatabaseConnection.ExecuteQuery("GetAllCustomers", CommandType.StoredProcedure, null);
                }
                else
                {
                    dt = customerRepository.GetCustomerByName(searchText);
                }
                dgv_Customers.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm khách hàng: " + ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txt_nameSearch_TextChanged(object sender, EventArgs e)
        {
            SearchCustomersByName(txt_nameSearch.Text.Trim());

            if (txt_nameSearch.Focused)
            {
                txt_idSearch.Text = string.Empty;
            }

        }

        // Tim kiem khach hang theo ID
        private void SearchCustomerByID(string id)
        {
            try
            {
                DataTable dt;
                if (string.IsNullOrWhiteSpace(id))
                {
                    dt = DatabaseConnection.ExecuteQuery("GetAllCustomers", CommandType.StoredProcedure, null);
                }
                else
                {
                    dt = customerRepository.GetCustomerById(int.Parse(id));
                }
                dgv_Customers.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm khách hàng: " + ex.Message);
            }
        }

        private void txt_idSearch_TextChanged(object sender, EventArgs e)
        {
            SearchCustomerByID(txt_idSearch.Text);

            if (txt_idSearch.Focused)
            {
                txt_nameSearch.Text = string.Empty;
            }
        }

		private int? GetSelectedCustomerId()
		{
			if (dgv_Customers.CurrentRow == null)
				return null;
			if (dgv_Customers.CurrentRow.DataBoundItem is DataRowView drv)
			{
				return Convert.ToInt32(drv["CustomerID"]);
			}
			var cell = dgv_Customers.CurrentRow.Cells["CustomerID"];
			if (cell != null && int.TryParse(cell.Value?.ToString(), out int id))
				return id;
			return null;
		}

		private void btn_Add_Click(object sender, EventArgs e)
		{
			using (var dlg = new CustomerManageEditForm(false, 0))
			{
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					LoadCustomers();
				}
			}
		}

		private void btn_Edit_Click(object sender, EventArgs e)
		{
			int? id = GetSelectedCustomerId();
			if (id == null)
			{
				MessageBox.Show("Vui lòng chọn khách hàng để sửa.");
				return;
			}
			using (var dlg = new CustomerManageEditForm(true, id.Value))
			{
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					LoadCustomers();
				}
			}
		}

		private void btn_Delete_Click(object sender, EventArgs e)
		{
			int? id = GetSelectedCustomerId();
			if (id == null)
			{
				MessageBox.Show("Vui lòng chọn khách hàng để xóa.");
				return;
			}
			if (MessageBox.Show("Bạn có chắc muốn xóa khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				try
				{
					bool success = customerRepository.DeleteCustomer(id.Value);
					if (success)
					{
						MessageBox.Show("Xóa khách hàng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
						LoadCustomers(); // Reload the customer list
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Lỗi khi xóa khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
    }
}
