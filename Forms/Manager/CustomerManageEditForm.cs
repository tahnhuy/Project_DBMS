using System;
using System.Data;
using System.Windows.Forms;
using Sale_Management.DatabaseAccess;

namespace Sale_Management.Forms
{
    public partial class CustomerManageEditForm : Form
    {
        private readonly CustomerRepository customerRepository;
        private readonly bool isEditMode;
        private readonly int customerId;

        public CustomerManageEditForm(bool isEditMode = false, int customerId = 0)
        {
            InitializeComponent();
            this.isEditMode = isEditMode;
            this.customerId = customerId;
            customerRepository = new CustomerRepository();

            if (isEditMode)
            {
                this.Text = "Sửa khách hàng";
                LoadCustomerData();
            }
            else
            {
                this.Text = "Thêm khách hàng";
                txt_LoyaltyPoints.Text = "0";
            }
        }

        private void LoadCustomerData()
        {
            try
            {
                DataTable dt = customerRepository.GetCustomerById(customerId);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txt_CustomerName.Text = row["CustomerName"].ToString();
                    txt_Phone.Text = row["Phone"].ToString();
                    txt_Address.Text = row["Address"].ToString();
                    txt_LoyaltyPoints.Text = row["LoyaltyPoints"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txt_CustomerName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_CustomerName.Focus();
                return false;
            }
            if (!int.TryParse(txt_LoyaltyPoints.Text, out int points) || points < 0)
            {
                MessageBox.Show("Điểm tích lũy phải là số >= 0!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_LoyaltyPoints.Focus();
                return false;
            }
            return true;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;
            try
            {
                string name = txt_CustomerName.Text.Trim();
                string phone = string.IsNullOrWhiteSpace(txt_Phone.Text) ? null : txt_Phone.Text.Trim();
                string address = string.IsNullOrWhiteSpace(txt_Address.Text) ? null : txt_Address.Text.Trim();
                int points = int.Parse(txt_LoyaltyPoints.Text);

                bool success;
                if (isEditMode)
                {
                    success = customerRepository.UpdateCustomer(customerId, name, phone, address, points);
                }
                else
                {
                    success = customerRepository.AddCustomer(name, phone, address, points);
                }
                if (success)
                {
                    MessageBox.Show("Lưu khách hàng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txt_LoyaltyPoints_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
