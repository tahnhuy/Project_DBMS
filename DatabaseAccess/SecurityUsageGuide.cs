using System;
using System.Data;
using System.Windows.Forms;

namespace Sale_Management.DatabaseAccess
{
    /// <summary>
    /// Hướng dẫn sử dụng hệ thống phân quyền trong ứng dụng C#
    /// </summary>
    public static class SecurityUsageGuide
    {
        /// <summary>
        /// Ví dụ sử dụng trong LoginForm
        /// </summary>
        public static void ExampleLoginForm()
        {
            /*
            // Trong LoginForm.cs - Button Login Click Event
            private void btnLogin_Click(object sender, EventArgs e)
            {
                try
                {
                    string username = txtUsername.Text.Trim();
                    string password = txtPassword.Text.Trim();
                    
                    // Kiểm tra đăng nhập và lấy role
                    string userRole = SecurityHelper.CheckUserRole(username, password);
                    
                    if (userRole == "manager")
                    {
                        // Mở form Manager với đầy đủ quyền
                        ManagerForm managerForm = new ManagerForm(username);
                        managerForm.Show();
                        this.Hide();
                    }
                    else if (userRole == "saler")
                    {
                        // Mở form Saler với quyền hạn chế
                        SalerForm salerForm = new SalerForm(username);
                        salerForm.Show();
                        this.Hide();
                    }
                    else if (userRole == "customer")
                    {
                        // Mở form Customer với quyền tối thiểu
                        CustomerForm customerForm = new CustomerForm(username);
                        customerForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi đăng nhập", 
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi đăng nhập: " + ex.Message, "Lỗi", 
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            */
        }

        /// <summary>
        /// Ví dụ sử dụng trong ProductForm
        /// </summary>
        public static void ExampleProductForm()
        {
            /*
            // Trong ProductForm.cs - Load Event
            private void ProductForm_Load(object sender, EventArgs e)
            {
                string currentUser = GetCurrentUser(); // Lấy user hiện tại
                
                // Kiểm tra quyền và ẩn/hiện controls
                if (!SecurityHelper.CanDeleteProducts(currentUser))
                {
                    btnDeleteProduct.Enabled = false;
                    btnDeleteProduct.Visible = false;
                }
                
                if (!SecurityHelper.CanManageProducts(currentUser))
                {
                    btnAddProduct.Enabled = false;
                    btnEditProduct.Enabled = false;
                }
                
                // Load dữ liệu
                LoadProducts();
            }
            
            // Trong ProductForm.cs - Button Delete Click Event
            private void btnDeleteProduct_Click(object sender, EventArgs e)
            {
                string currentUser = GetCurrentUser();
                
                // Kiểm tra quyền trước khi thực hiện
                if (!SecurityHelper.CanDeleteProducts(currentUser))
                {
                    MessageBox.Show("Bạn không có quyền xóa sản phẩm!", "Không có quyền", 
                                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // Thực hiện xóa
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận", 
                                   MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Gọi stored procedure xóa
                    ProductRepository.DeleteProduct(selectedProductId);
                    LoadProducts(); // Reload danh sách
                }
            }
            */
        }

        /// <summary>
        /// Ví dụ sử dụng trong CustomerForm
        /// </summary>
        public static void ExampleCustomerForm()
        {
            /*
            // Trong CustomerForm.cs - Load Event
            private void CustomerForm_Load(object sender, EventArgs e)
            {
                string currentUser = GetCurrentUser();
                
                // Kiểm tra quyền
                if (!SecurityHelper.CanDeleteCustomers(currentUser))
                {
                    btnDeleteCustomer.Enabled = false;
                    btnDeleteCustomer.Visible = false;
                }
                
                if (!SecurityHelper.CanManageCustomers(currentUser))
                {
                    btnAddCustomer.Enabled = false;
                    btnEditCustomer.Enabled = false;
                }
                
                LoadCustomers();
            }
            */
        }

        /// <summary>
        /// Ví dụ sử dụng trong DiscountForm
        /// </summary>
        public static void ExampleDiscountForm()
        {
            /*
            // Trong DiscountForm.cs - Load Event
            private void DiscountForm_Load(object sender, EventArgs e)
            {
                string currentUser = GetCurrentUser();
                
                // Chỉ Manager mới có quyền quản lý giảm giá
                if (!SecurityHelper.CanManageDiscounts(currentUser))
                {
                    btnAddDiscount.Enabled = false;
                    btnEditDiscount.Enabled = false;
                    btnDeleteDiscount.Enabled = false;
                    
                    MessageBox.Show("Chỉ quản lý mới có quyền quản lý giảm giá!", "Không có quyền", 
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                LoadDiscounts();
            }
            */
        }

        /// <summary>
        /// Ví dụ sử dụng trong ReportForm
        /// </summary>
        public static void ExampleReportForm()
        {
            /*
            // Trong ReportForm.cs - Load Event
            private void ReportForm_Load(object sender, EventArgs e)
            {
                string currentUser = GetCurrentUser();
                
                // Kiểm tra quyền xem báo cáo
                if (!SecurityHelper.CanViewRevenueReports(currentUser))
                {
                    btnRevenueReport.Enabled = false;
                    btnRevenueReport.Visible = false;
                }
                
                if (!SecurityHelper.CanViewDetailedReports(currentUser))
                {
                    btnDetailedReport.Enabled = false;
                    btnDetailedReport.Visible = false;
                }
                
                LoadReports();
            }
            */
        }

        /// <summary>
        /// Ví dụ sử dụng trong AccountForm
        /// </summary>
        public static void ExampleAccountForm()
        {
            /*
            // Trong AccountForm.cs - Load Event
            private void AccountForm_Load(object sender, EventArgs e)
            {
                string currentUser = GetCurrentUser();
                
                // Chỉ Manager mới có quyền quản lý tài khoản
                if (!SecurityHelper.CanManageAccounts(currentUser))
                {
                    btnAddAccount.Enabled = false;
                    btnEditAccount.Enabled = false;
                    btnDeleteAccount.Enabled = false;
                    
                    MessageBox.Show("Chỉ quản lý mới có quyền quản lý tài khoản!", "Không có quyền", 
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                LoadAccounts();
            }
            */
        }

        /// <summary>
        /// Ví dụ sử dụng function trong code
        /// </summary>
        public static void ExampleUsingFunctions()
        {
            /*
            // Sử dụng Scalar Functions (returns single value)
            try
            {
                // Tính giá sau giảm
                decimal originalPrice = 100000;
                int productId = 1;
                decimal discountedPrice = DiscountRepository.GetDiscountedPrice(productId, originalPrice);
                
                // Kiểm tra tồn kho
                bool isAvailable = ProductRepository.IsStockAvailable(productId, 5);
                
                // Tính điểm tích lũy
                decimal purchaseAmount = 500000;
                int loyaltyPoints = ReportRepository.CalculateLoyaltyPoints(purchaseAmount);
                
                // Format tiền Việt Nam
                string formattedMoney = ReportRepository.FormatVietnamMoney(1000000);
                
                // Validate số điện thoại Việt Nam
                bool isValidPhone = ReportRepository.IsValidVietnamesePhone("0123456789");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            
            // Sử dụng Table-valued Functions (returns table)
            try
            {
                // Tìm sản phẩm theo tên
                DataTable products = ProductRepository.GetProductByName("Coca");
                
                // Tìm sản phẩm theo ID
                DataTable product = ProductRepository.GetProductByID(1);
                
                // Lấy top sản phẩm bán chạy
                DataTable topProducts = ProductRepository.GetTopSellingProducts(10);
                
                // Lấy lịch sử mua hàng của khách hàng
                DataTable purchaseHistory = CustomerRepository.GetCustomerPurchaseHistory("customer001");
                
                // Tìm kiếm khách hàng
                DataTable customers = CustomerRepository.SearchCustomers("Nguyễn");
                
                // Lấy thống kê dashboard
                DataTable dashboardStats = ReportRepository.GetDashboardStats();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            */
        }

        /// <summary>
        /// Ví dụ sử dụng Views
        /// </summary>
        public static void ExampleUsingViews()
        {
            /*
            // Sử dụng Views (chỉ SELECT)
            try
            {
                // Xem sản phẩm có giảm giá
                DataTable productsWithDiscounts = DatabaseConnection.ExecuteQuery(
                    "SELECT * FROM ProductsWithDiscounts", CommandType.Text, null);
                
                // Xem tóm tắt bán hàng
                DataTable salesSummary = DatabaseConnection.ExecuteQuery(
                    "SELECT * FROM SalesSummary", CommandType.Text, null);
                
                // Xem thống kê sản phẩm
                DataTable productStats = DatabaseConnection.ExecuteQuery(
                    "SELECT * FROM ProductSalesStats", CommandType.Text, null);
                
                // Xem tóm tắt khách hàng
                DataTable customerSummary = DatabaseConnection.ExecuteQuery(
                    "SELECT * FROM CustomerPurchaseSummary", CommandType.Text, null);
                
                // Xem báo cáo hàng ngày
                DataTable dailyReport = DatabaseConnection.ExecuteQuery(
                    "SELECT * FROM DailySalesReport", CommandType.Text, null);
                
                // Xem sản phẩm sắp hết hàng
                DataTable lowStockProducts = DatabaseConnection.ExecuteQuery(
                    "SELECT * FROM LowStockProducts", CommandType.Text, null);
                
                // Xem chi tiết giảm giá đang hoạt động
                DataTable activeDiscounts = DatabaseConnection.ExecuteQuery(
                    "SELECT * FROM ActiveDiscountsDetail", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            */
        }

        /// <summary>
        /// Ví dụ xử lý lỗi phân quyền
        /// </summary>
        public static void ExampleErrorHandling()
        {
            /*
            try
            {
                // Thực hiện hành động cần quyền
                string currentUser = GetCurrentUser();
                
                if (!SecurityHelper.CanManageProducts(currentUser))
                {
                    throw new UnauthorizedAccessException("Bạn không có quyền quản lý sản phẩm!");
                }
                
                // Thực hiện hành động
                ProductRepository.AddProduct(productData);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, "Không có quyền", 
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", 
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            */
        }

        /// <summary>
        /// Lấy user hiện tại (cần implement trong ứng dụng)
        /// </summary>
        /// <returns>Tên user hiện tại</returns>
        private static string GetCurrentUser()
        {
            // Cần implement logic lấy user hiện tại
            // Ví dụ: return CurrentUser.Username;
            return "admin"; // Placeholder
        }
    }
}
