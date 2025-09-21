using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Sale_Management.DatabaseAccess;

namespace Sale_Management.Forms.Manager
{
    public partial class StatisticsForm : Form
    {
        private string currentUsername;

        public StatisticsForm(string username)
        {
            InitializeComponent();
            currentUsername = username;
            SetupForm();
            LoadDashboardStats();
        }

        private void SetupForm()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = "Thống kê và Báo cáo";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.Sizable;

            // Create main TabControl
            TabControl mainTabControl = new TabControl();
            mainTabControl.Dock = DockStyle.Fill;
            mainTabControl.Name = "mainTabControl";

            // Tab 1: Dashboard Overview
            TabPage dashboardTab = new TabPage("Tổng quan");
            CreateDashboardTab(dashboardTab);
            mainTabControl.TabPages.Add(dashboardTab);

            // Tab 2: Revenue Reports
            TabPage revenueTab = new TabPage("Báo cáo doanh thu");
            CreateRevenueTab(revenueTab);
            mainTabControl.TabPages.Add(revenueTab);

            // Tab 3: Product Statistics
            TabPage productTab = new TabPage("Thống kê sản phẩm");
            CreateProductTab(productTab);
            mainTabControl.TabPages.Add(productTab);

            // Tab 4: Customer Statistics
            TabPage customerTab = new TabPage("Thống kê khách hàng");
            CreateCustomerTab(customerTab);
            mainTabControl.TabPages.Add(customerTab);

            // Tab 5: Transaction Reports
            TabPage transactionTab = new TabPage("Báo cáo giao dịch");
            CreateTransactionTab(transactionTab);
            mainTabControl.TabPages.Add(transactionTab);

            // Tab 6: Account Reports
            TabPage accountTab = new TabPage("Báo cáo tài khoản");
            CreateAccountTab(accountTab);
            mainTabControl.TabPages.Add(accountTab);

            // Tab 7: Discount Reports
            TabPage discountTab = new TabPage("Báo cáo giảm giá");
            CreateDiscountTab(discountTab);
            mainTabControl.TabPages.Add(discountTab);

            this.Controls.Add(mainTabControl);
            this.ResumeLayout(false);
        }

        private void CreateDashboardTab(TabPage tab)
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;

            // Title
            Label titleLabel = new Label();
            titleLabel.Text = "THỐNG KÊ TỔNG QUAN";
            titleLabel.Font = new Font("Arial", 16, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Size = new Size(300, 30);
            titleLabel.ForeColor = Color.DarkBlue;

            // Refresh button
            Button btnRefresh = new Button();
            btnRefresh.Text = "Làm mới";
            btnRefresh.Location = new Point(1000, 20);
            btnRefresh.Size = new Size(100, 30);
            btnRefresh.Click += (s, e) => LoadDashboardStats();

            // Dashboard DataGridView
            DataGridView dgvDashboard = new DataGridView();
            dgvDashboard.Name = "dgvDashboard";
            dgvDashboard.Location = new Point(20, 60);
            dgvDashboard.Size = new Size(1080, 300);
            dgvDashboard.ReadOnly = true;
            dgvDashboard.AllowUserToAddRows = false;
            dgvDashboard.AllowUserToDeleteRows = false;
            dgvDashboard.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Revenue Summary Panel
            Panel revenuePanel = new Panel();
            revenuePanel.Location = new Point(20, 380);
            revenuePanel.Size = new Size(1080, 200);
            revenuePanel.BorderStyle = BorderStyle.FixedSingle;

            Label revenueTitle = new Label();
            revenueTitle.Text = "TÓM TẮT DOANH THU";
            revenueTitle.Font = new Font("Arial", 12, FontStyle.Bold);
            revenueTitle.Location = new Point(10, 10);
            revenueTitle.Size = new Size(200, 20);

            Label lblTodayRevenue = new Label();
            lblTodayRevenue.Text = "Doanh thu hôm nay:";
            lblTodayRevenue.Location = new Point(20, 40);
            lblTodayRevenue.Size = new Size(150, 20);

            Label lblTodayRevenueValue = new Label();
            lblTodayRevenueValue.Name = "lblTodayRevenueValue";
            lblTodayRevenueValue.Text = "0 ₫";
            lblTodayRevenueValue.Font = new Font("Arial", 10, FontStyle.Bold);
            lblTodayRevenueValue.ForeColor = Color.Green;
            lblTodayRevenueValue.Location = new Point(180, 40);
            lblTodayRevenueValue.Size = new Size(200, 20);

            Label lblMonthlyRevenue = new Label();
            lblMonthlyRevenue.Text = "Doanh thu tháng này:";
            lblMonthlyRevenue.Location = new Point(20, 70);
            lblMonthlyRevenue.Size = new Size(150, 20);

            Label lblMonthlyRevenueValue = new Label();
            lblMonthlyRevenueValue.Name = "lblMonthlyRevenueValue";
            lblMonthlyRevenueValue.Text = "0 ₫";
            lblMonthlyRevenueValue.Font = new Font("Arial", 10, FontStyle.Bold);
            lblMonthlyRevenueValue.ForeColor = Color.Green;
            lblMonthlyRevenueValue.Location = new Point(180, 70);
            lblMonthlyRevenueValue.Size = new Size(200, 20);

            Button btnCalculateToday = new Button();
            btnCalculateToday.Text = "Tính doanh thu hôm nay";
            btnCalculateToday.Location = new Point(20, 100);
            btnCalculateToday.Size = new Size(150, 30);
            btnCalculateToday.Click += (s, e) => CalculateTodayRevenue();

            Button btnCalculateMonthly = new Button();
            btnCalculateMonthly.Text = "Tính doanh thu tháng";
            btnCalculateMonthly.Location = new Point(180, 100);
            btnCalculateMonthly.Size = new Size(150, 30);
            btnCalculateMonthly.Click += (s, e) => CalculateMonthlyRevenue();

            revenuePanel.Controls.AddRange(new Control[] { 
                revenueTitle, lblTodayRevenue, lblTodayRevenueValue, 
                lblMonthlyRevenue, lblMonthlyRevenueValue, 
                btnCalculateToday, btnCalculateMonthly 
            });

            panel.Controls.AddRange(new Control[] { 
                titleLabel, btnRefresh, dgvDashboard, revenuePanel 
            });

            tab.Controls.Add(panel);
        }

        private void CreateRevenueTab(TabPage tab)
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;

            Label titleLabel = new Label();
            titleLabel.Text = "BÁO CÁO DOANH THU THEO SẢN PHẨM";
            titleLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Size = new Size(400, 30);

            // Date range selection
            Label lblStartDate = new Label();
            lblStartDate.Text = "Từ ngày:";
            lblStartDate.Location = new Point(20, 60);
            lblStartDate.Size = new Size(80, 20);

            DateTimePicker dtpStartDate = new DateTimePicker();
            dtpStartDate.Name = "dtpStartDate";
            dtpStartDate.Location = new Point(100, 58);
            dtpStartDate.Size = new Size(150, 20);
            dtpStartDate.Value = DateTime.Now.AddDays(-30);

            Label lblEndDate = new Label();
            lblEndDate.Text = "Đến ngày:";
            lblEndDate.Location = new Point(270, 60);
            lblEndDate.Size = new Size(80, 20);

            DateTimePicker dtpEndDate = new DateTimePicker();
            dtpEndDate.Name = "dtpEndDate";
            dtpEndDate.Location = new Point(350, 58);
            dtpEndDate.Size = new Size(150, 20);
            dtpEndDate.Value = DateTime.Now;

            Button btnLoadRevenue = new Button();
            btnLoadRevenue.Text = "Tải báo cáo";
            btnLoadRevenue.Location = new Point(520, 56);
            btnLoadRevenue.Size = new Size(100, 25);
            btnLoadRevenue.Click += (s, e) => LoadProductRevenueReport();

            // Revenue DataGridView
            DataGridView dgvRevenue = new DataGridView();
            dgvRevenue.Name = "dgvRevenue";
            dgvRevenue.Location = new Point(20, 100);
            dgvRevenue.Size = new Size(1080, 500);
            dgvRevenue.ReadOnly = true;
            dgvRevenue.AllowUserToAddRows = false;
            dgvRevenue.AllowUserToDeleteRows = false;
            dgvRevenue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            panel.Controls.AddRange(new Control[] { 
                titleLabel, lblStartDate, dtpStartDate, lblEndDate, dtpEndDate, 
                btnLoadRevenue, dgvRevenue 
            });

            tab.Controls.Add(panel);
        }

        private void CreateProductTab(TabPage tab)
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;

            Label titleLabel = new Label();
            titleLabel.Text = "THỐNG KÊ SẢN PHẨM";
            titleLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Size = new Size(300, 30);

            Button btnLoadProductStats = new Button();
            btnLoadProductStats.Text = "Tải thống kê sản phẩm";
            btnLoadProductStats.Location = new Point(20, 60);
            btnLoadProductStats.Size = new Size(200, 30);
            btnLoadProductStats.Click += (s, e) => LoadProductStats();

            Button btnLoadTopProducts = new Button();
            btnLoadTopProducts.Text = "Top sản phẩm bán chạy";
            btnLoadTopProducts.Location = new Point(240, 60);
            btnLoadTopProducts.Size = new Size(200, 30);
            btnLoadTopProducts.Click += (s, e) => LoadTopSellingProducts();

            Button btnLoadLowStock = new Button();
            btnLoadLowStock.Text = "Sản phẩm sắp hết hàng";
            btnLoadLowStock.Location = new Point(460, 60);
            btnLoadLowStock.Size = new Size(200, 30);
            btnLoadLowStock.Click += (s, e) => LoadLowStockProducts();

            // Product DataGridView
            DataGridView dgvProducts = new DataGridView();
            dgvProducts.Name = "dgvProducts";
            dgvProducts.Location = new Point(20, 100);
            dgvProducts.Size = new Size(1080, 500);
            dgvProducts.ReadOnly = true;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            panel.Controls.AddRange(new Control[] { 
                titleLabel, btnLoadProductStats, btnLoadTopProducts, btnLoadLowStock, dgvProducts 
            });

            tab.Controls.Add(panel);
        }

        private void CreateCustomerTab(TabPage tab)
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;

            Label titleLabel = new Label();
            titleLabel.Text = "THỐNG KÊ KHÁCH HÀNG";
            titleLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Size = new Size(300, 30);

            Button btnLoadCustomerStats = new Button();
            btnLoadCustomerStats.Text = "Tải thống kê khách hàng";
            btnLoadCustomerStats.Location = new Point(20, 60);
            btnLoadCustomerStats.Size = new Size(200, 30);
            btnLoadCustomerStats.Click += (s, e) => LoadCustomerStats();

            // Customer DataGridView
            DataGridView dgvCustomers = new DataGridView();
            dgvCustomers.Name = "dgvCustomers";
            dgvCustomers.Location = new Point(20, 100);
            dgvCustomers.Size = new Size(1080, 500);
            dgvCustomers.ReadOnly = true;
            dgvCustomers.AllowUserToAddRows = false;
            dgvCustomers.AllowUserToDeleteRows = false;
            dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            panel.Controls.AddRange(new Control[] { 
                titleLabel, btnLoadCustomerStats, dgvCustomers 
            });

            tab.Controls.Add(panel);
        }

        private void CreateTransactionTab(TabPage tab)
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;

            Label titleLabel = new Label();
            titleLabel.Text = "BÁO CÁO GIAO DỊCH";
            titleLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Size = new Size(300, 30);

            Button btnLoadTransactions = new Button();
            btnLoadTransactions.Text = "Tải báo cáo giao dịch";
            btnLoadTransactions.Location = new Point(20, 60);
            btnLoadTransactions.Size = new Size(200, 30);
            btnLoadTransactions.Click += (s, e) => LoadTransactionSummary();

            // Transaction DataGridView
            DataGridView dgvTransactions = new DataGridView();
            dgvTransactions.Name = "dgvTransactions";
            dgvTransactions.Location = new Point(20, 100);
            dgvTransactions.Size = new Size(1080, 500);
            dgvTransactions.ReadOnly = true;
            dgvTransactions.AllowUserToAddRows = false;
            dgvTransactions.AllowUserToDeleteRows = false;
            dgvTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            panel.Controls.AddRange(new Control[] { 
                titleLabel, btnLoadTransactions, dgvTransactions 
            });

            tab.Controls.Add(panel);
        }

        private void CreateAccountTab(TabPage tab)
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;

            Label titleLabel = new Label();
            titleLabel.Text = "BÁO CÁO TÀI KHOẢN";
            titleLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Size = new Size(300, 30);

            Button btnLoadAccounts = new Button();
            btnLoadAccounts.Text = "Tải báo cáo tài khoản";
            btnLoadAccounts.Location = new Point(20, 60);
            btnLoadAccounts.Size = new Size(200, 30);
            btnLoadAccounts.Click += (s, e) => LoadAccountSummary();

            // Account DataGridView
            DataGridView dgvAccounts = new DataGridView();
            dgvAccounts.Name = "dgvAccounts";
            dgvAccounts.Location = new Point(20, 100);
            dgvAccounts.Size = new Size(1080, 500);
            dgvAccounts.ReadOnly = true;
            dgvAccounts.AllowUserToAddRows = false;
            dgvAccounts.AllowUserToDeleteRows = false;
            dgvAccounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            panel.Controls.AddRange(new Control[] { 
                titleLabel, btnLoadAccounts, dgvAccounts 
            });

            tab.Controls.Add(panel);
        }

        private void CreateDiscountTab(TabPage tab)
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;

            Label titleLabel = new Label();
            titleLabel.Text = "BÁO CÁO GIẢM GIÁ";
            titleLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Size = new Size(300, 30);

            Button btnLoadDiscounts = new Button();
            btnLoadDiscounts.Text = "Tải báo cáo giảm giá";
            btnLoadDiscounts.Location = new Point(20, 60);
            btnLoadDiscounts.Size = new Size(200, 30);
            btnLoadDiscounts.Click += (s, e) => LoadDiscountReports();

            // Discount DataGridView
            DataGridView dgvDiscounts = new DataGridView();
            dgvDiscounts.Name = "dgvDiscounts";
            dgvDiscounts.Location = new Point(20, 100);
            dgvDiscounts.Size = new Size(1080, 500);
            dgvDiscounts.ReadOnly = true;
            dgvDiscounts.AllowUserToAddRows = false;
            dgvDiscounts.AllowUserToDeleteRows = false;
            dgvDiscounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            panel.Controls.AddRange(new Control[] { 
                titleLabel, btnLoadDiscounts, dgvDiscounts 
            });

            tab.Controls.Add(panel);
        }

        // Data loading methods
        private void LoadDashboardStats()
        {
            try
            {
                DataTable dashboardData = ReportRepository.GetDashboardStats();
                TabControl mainTab = this.Controls.Find("mainTabControl", true)[0] as TabControl;
                TabPage dashboardTab = mainTab.TabPages[0];
                DataGridView dgvDashboard = dashboardTab.Controls.Find("dgvDashboard", true)[0] as DataGridView;
                dgvDashboard.DataSource = dashboardData;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thống kê tổng quan: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculateTodayRevenue()
        {
            try
            {
                decimal todayRevenue = ReportRepository.GetDailyRevenue(DateTime.Now);
                string formattedRevenue = ReportRepository.FormatVietnamMoney(todayRevenue);
                
                TabControl mainTab = this.Controls.Find("mainTabControl", true)[0] as TabControl;
                TabPage dashboardTab = mainTab.TabPages[0];
                Label lblTodayRevenueValue = dashboardTab.Controls.Find("lblTodayRevenueValue", true)[0] as Label;
                lblTodayRevenueValue.Text = formattedRevenue;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tính doanh thu hôm nay: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculateMonthlyRevenue()
        {
            try
            {
                decimal monthlyRevenue = ReportRepository.GetMonthlyRevenue(DateTime.Now.Year, DateTime.Now.Month);
                string formattedRevenue = ReportRepository.FormatVietnamMoney(monthlyRevenue);
                
                TabControl mainTab = this.Controls.Find("mainTabControl", true)[0] as TabControl;
                TabPage dashboardTab = mainTab.TabPages[0];
                Label lblMonthlyRevenueValue = dashboardTab.Controls.Find("lblMonthlyRevenueValue", true)[0] as Label;
                lblMonthlyRevenueValue.Text = formattedRevenue;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tính doanh thu tháng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProductRevenueReport()
        {
            try
            {
                TabControl mainTab = this.Controls.Find("mainTabControl", true)[0] as TabControl;
                TabPage revenueTab = mainTab.TabPages[1];
                
                DateTimePicker dtpStartDate = revenueTab.Controls.Find("dtpStartDate", true)[0] as DateTimePicker;
                DateTimePicker dtpEndDate = revenueTab.Controls.Find("dtpEndDate", true)[0] as DateTimePicker;
                DataGridView dgvRevenue = revenueTab.Controls.Find("dgvRevenue", true)[0] as DataGridView;

                DataTable revenueData = ReportRepository.GetProductRevenueReport(dtpStartDate.Value, dtpEndDate.Value);
                dgvRevenue.DataSource = revenueData;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải báo cáo doanh thu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProductStats()
        {
            try
            {
                TabControl mainTab = this.Controls.Find("mainTabControl", true)[0] as TabControl;
                TabPage productTab = mainTab.TabPages[2];
                DataGridView dgvProducts = productTab.Controls.Find("dgvProducts", true)[0] as DataGridView;

                ProductRepository productRepo = new ProductRepository();
                DataTable productData = productRepo.GetAllProducts();
                dgvProducts.DataSource = productData;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thống kê sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTopSellingProducts()
        {
            try
            {
                TabControl mainTab = this.Controls.Find("mainTabControl", true)[0] as TabControl;
                TabPage productTab = mainTab.TabPages[2];
                DataGridView dgvProducts = productTab.Controls.Find("dgvProducts", true)[0] as DataGridView;

                ProductRepository productRepo = new ProductRepository();
                DataTable topProductsData = productRepo.GetTopSellingProducts(10);
                dgvProducts.DataSource = topProductsData;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải top sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLowStockProducts()
        {
            try
            {
                TabControl mainTab = this.Controls.Find("mainTabControl", true)[0] as TabControl;
                TabPage productTab = mainTab.TabPages[2];
                DataGridView dgvProducts = productTab.Controls.Find("dgvProducts", true)[0] as DataGridView;

                ProductRepository productRepo = new ProductRepository();
                DataTable lowStockData = productRepo.GetLowStockProducts();
                dgvProducts.DataSource = lowStockData;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải sản phẩm sắp hết hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCustomerStats()
        {
            try
            {
                TabControl mainTab = this.Controls.Find("mainTabControl", true)[0] as TabControl;
                TabPage customerTab = mainTab.TabPages[3];
                DataGridView dgvCustomers = customerTab.Controls.Find("dgvCustomers", true)[0] as DataGridView;

                CustomerRepository customerRepo = new CustomerRepository();
                DataTable customerData = customerRepo.GetAllCustomers();
                dgvCustomers.DataSource = customerData;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thống kê khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTransactionSummary()
        {
            try
            {
                TabControl mainTab = this.Controls.Find("mainTabControl", true)[0] as TabControl;
                TabPage transactionTab = mainTab.TabPages[4];
                DataGridView dgvTransactions = transactionTab.Controls.Find("dgvTransactions", true)[0] as DataGridView;

                DataTable transactionData = ReportRepository.GetTransactionSummary();
                dgvTransactions.DataSource = transactionData;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải báo cáo giao dịch: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAccountSummary()
        {
            try
            {
                TabControl mainTab = this.Controls.Find("mainTabControl", true)[0] as TabControl;
                TabPage accountTab = mainTab.TabPages[5];
                DataGridView dgvAccounts = accountTab.Controls.Find("dgvAccounts", true)[0] as DataGridView;

                DataTable accountData = ReportRepository.GetAccountSummary();
                dgvAccounts.DataSource = accountData;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải báo cáo tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDiscountReports()
        {
            try
            {
                TabControl mainTab = this.Controls.Find("mainTabControl", true)[0] as TabControl;
                TabPage discountTab = mainTab.TabPages[6];
                DataGridView dgvDiscounts = discountTab.Controls.Find("dgvDiscounts", true)[0] as DataGridView;

                DataTable discountData = ReportRepository.GetActiveDiscountsDetail();
                dgvDiscounts.DataSource = discountData;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải báo cáo giảm giá: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
