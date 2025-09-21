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
    public partial class AccountInfoForm : Form
    {
        private string currentUsername;
        private string currentRole;

        public AccountInfoForm(string username, string role)
        {
            InitializeComponent();
            currentUsername = username;
            currentRole = role;
            LoadAccountInfo();
        }

        private void LoadAccountInfo()
        {
            try
            {
                // Kiểm tra username có hợp lệ không
                if (string.IsNullOrEmpty(currentUsername))
                {
                    MessageBox.Show("Không có thông tin người dùng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy thông tin từ bảng Account
                string query = "SELECT Username, Role, CreatedDate, EmployeeID, CustomerID FROM Account WHERE Username = @Username";
                
                var parameters = new System.Data.SqlClient.SqlParameter[] {
                    new System.Data.SqlClient.SqlParameter("@Username", currentUsername ?? "")
                };
                
                var accountData = DatabaseConnection.ExecuteQuery(query, CommandType.Text, parameters);
                
                if (accountData != null && accountData.Rows.Count > 0)
                {
                    DataRow accountRow = accountData.Rows[0];
                    
                    // Hiển thị thông tin cơ bản
                    lblUsername.Text = "Tên đăng nhập:";
                    lblUsernameValue.Text = accountRow["Username"].ToString();
                    lblRole.Text = "Vai trò:";
                    lblRoleValue.Text = accountRow["Role"].ToString();
                    lblCreatedDate.Text = "Ngày tạo:";
                    lblCreatedDateValue.Text = Convert.ToDateTime(accountRow["CreatedDate"]).ToString("dd/MM/yyyy");

                    // Lấy thông tin nhân viên nếu có EmployeeID
                    if (accountRow["EmployeeID"] != DBNull.Value)
                    {
                        int employeeId = Convert.ToInt32(accountRow["EmployeeID"]);
                        
                        string employeeQuery = "SELECT EmployeeID, EmployeeName, Phone, Address, Position, HireDate, Salary FROM Employees WHERE EmployeeID = @EmployeeID";
                        
                        var employeeParams = new System.Data.SqlClient.SqlParameter[] {
                            new System.Data.SqlClient.SqlParameter("@EmployeeID", employeeId)
                        };
                        
                        var employeeData = DatabaseConnection.ExecuteQuery(employeeQuery, CommandType.Text, employeeParams);
                        
                        if (employeeData != null && employeeData.Rows.Count > 0)
                        {
                            DataRow employeeRow = employeeData.Rows[0];
                            
                            lblEmployeeName.Text = "Tên nhân viên:";
                            lblEmployeeNameValue.Text = employeeRow["EmployeeName"].ToString();
                            lblPhone.Text = "Số điện thoại:";
                            lblPhoneValue.Text = employeeRow["Phone"].ToString();
                            lblAddress.Text = "Địa chỉ:";
                            lblAddressValue.Text = employeeRow["Address"] != DBNull.Value ? employeeRow["Address"].ToString() : "Không có";
                            lblPosition.Text = "Chức vụ:";
                            lblPositionValue.Text = employeeRow["Position"].ToString();
                            lblHireDate.Text = "Ngày vào làm:";
                            lblHireDateValue.Text = Convert.ToDateTime(employeeRow["HireDate"]).ToString("dd/MM/yyyy");
                            lblSalary.Text = "Lương:";
                            
                            if (employeeRow["Salary"] != DBNull.Value)
                            {
                                decimal salary = Convert.ToDecimal(employeeRow["Salary"]);
                                lblSalaryValue.Text = salary.ToString("N0") + " VND";
                            }
                            else
                            {
                                lblSalaryValue.Text = "Không có";
                            }
                        }
                    }
                    else
                    {
                        // Nếu không có EmployeeID, ẩn các thông tin nhân viên
                        lblEmployeeName.Text = "Tên nhân viên:";
                        lblEmployeeNameValue.Text = "Không có";
                        lblPhone.Text = "Số điện thoại:";
                        lblPhoneValue.Text = "Không có";
                        lblAddress.Text = "Địa chỉ:";
                        lblAddressValue.Text = "Không có";
                        lblPosition.Text = "Chức vụ:";
                        lblPositionValue.Text = "Không có";
                        lblHireDate.Text = "Ngày vào làm:";
                        lblHireDateValue.Text = "Không có";
                        lblSalary.Text = "Lương:";
                        lblSalaryValue.Text = "Không có";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin tài khoản: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
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
