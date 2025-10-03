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
        private ReportRepository reportRepository;

        public StatisticsForm(string username)
        {
            InitializeComponent();
            currentUsername = username;
            reportRepository = new ReportRepository();
            SetupForm();
            LoadAllTabs();
        }

        private void SafeInitializeNumericUpDown(NumericUpDown nud, decimal min, decimal max, decimal defaultValue)
        {
            try
            {
                nud.Minimum = min;
                nud.Maximum = max;
                nud.Value = Math.Max(min, Math.Min(max, defaultValue));
            }
            catch (Exception)
            {
                // If there's an issue, set to a safe default value
                nud.Value = min;
            }
        }

        private void SetupForm()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = "Thống kê và Báo cáo";
            this.Size = new Size(1000, 700);
            this.MinimumSize = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.Sizable;

            // Create main TabControl
            TabControl mainTabControl = new TabControl();
            mainTabControl.Dock = DockStyle.Fill;
            mainTabControl.Name = "mainTabControl";

            // Tab 1: Doanh thu ngày
            TabPage dailyRevenueTab = new TabPage("Doanh thu ngày");
            dailyRevenueTab.AutoScroll = true;
            CreateDailyRevenueTab(dailyRevenueTab);
            mainTabControl.TabPages.Add(dailyRevenueTab);

            // Tab 2: Doanh thu tháng & Tăng trưởng
            TabPage monthlyRevenueTab = new TabPage("Doanh thu tháng & Tăng trưởng");
            monthlyRevenueTab.AutoScroll = true;
            CreateMonthlyRevenueTab(monthlyRevenueTab);
            mainTabControl.TabPages.Add(monthlyRevenueTab);

            // Tab 3: Tồn kho thấp
            TabPage lowStockTab = new TabPage("Tồn kho thấp");
            lowStockTab.AutoScroll = true;
            CreateLowStockTab(lowStockTab);
            mainTabControl.TabPages.Add(lowStockTab);

            // Tab 4: Top sản phẩm bán chạy
            TabPage topProductsTab = new TabPage("Top Sản phẩm Bán chạy");
            topProductsTab.AutoScroll = true;
            CreateTopSellingProductsTab(topProductsTab);
            mainTabControl.TabPages.Add(topProductsTab);

            // Tab 5: Xếp hạng Khách hàng
            TabPage customerRankingTab = new TabPage("Xếp hạng Khách hàng");
            customerRankingTab.AutoScroll = true;
            CreateCustomerRankingTab(customerRankingTab);
            mainTabControl.TabPages.Add(customerRankingTab);

            // Tab 6: Xu hướng Bán hàng Sản phẩm
            TabPage productTrendTab = new TabPage("Xu hướng Bán hàng Sản phẩm");
            productTrendTab.AutoScroll = true;
            CreateProductSalesTrendTab(productTrendTab);
            mainTabControl.TabPages.Add(productTrendTab);

            this.Controls.Add(mainTabControl);
            this.ResumeLayout(false);
        }

        private void CreateDailyRevenueTab(TabPage tab)
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.AutoScroll = true;

            // Title
            Label titleLabel = new Label();
            titleLabel.Text = "DOANH THU THEO NGÀY";
            titleLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Size = new Size(300, 30);

            // Date selection
            Label lblDate = new Label();
            lblDate.Text = "Chọn ngày:";
            lblDate.Location = new Point(20, 60);
            lblDate.Size = new Size(80, 20);

            DateTimePicker dtpDate = new DateTimePicker();
            dtpDate.Name = "dtpDate";
            dtpDate.Location = new Point(100, 58);
            dtpDate.Size = new Size(150, 20);
            dtpDate.Value = DateTime.Today;

            Button btnCalculate = new Button();
            btnCalculate.Text = "Tính doanh thu";
            btnCalculate.Location = new Point(270, 56);
            btnCalculate.Size = new Size(120, 25);
            btnCalculate.Click += (s, e) => CalculateDailyRevenue();

            // Revenue display
            Label lblRevenue = new Label();
            lblRevenue.Text = "Doanh thu:";
            lblRevenue.Location = new Point(20, 100);
            lblRevenue.Size = new Size(100, 20);
            lblRevenue.Font = new Font("Arial", 12, FontStyle.Bold);

            Label lblRevenueValue = new Label();
            lblRevenueValue.Name = "lblDailyRevenueValue";
            lblRevenueValue.Text = "0 ₫";
            lblRevenueValue.Font = new Font("Arial", 14, FontStyle.Bold);
            lblRevenueValue.ForeColor = Color.Green;
            lblRevenueValue.Location = new Point(130, 100);
            lblRevenueValue.Size = new Size(200, 25);

            panel.Controls.AddRange(new Control[] { 
                titleLabel, lblDate, dtpDate, btnCalculate, lblRevenue, lblRevenueValue 
            });

            tab.Controls.Add(panel);
        }

        private void CreateMonthlyRevenueTab(TabPage tab)
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.AutoScroll = true;

            // Title
            Label titleLabel = new Label();
            titleLabel.Text = "DOANH THU THEO THÁNG & TĂNG TRƯỞNG";
            titleLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Size = new Size(400, 30);

            // --- Kỳ hiện tại (CP) ---
            Label lblCP = new Label();
            lblCP.Text = "Kỳ hiện tại:";
            lblCP.Location = new Point(20, 60);
            lblCP.Size = new Size(80, 20);

            Label lblYearCP = new Label();
            lblYearCP.Text = "Năm:";
            lblYearCP.Location = new Point(100, 60);
            lblYearCP.Size = new Size(40, 20);

            NumericUpDown nudYearCP = new NumericUpDown();
            nudYearCP.Name = "nudYearCP";
            nudYearCP.Location = new Point(150, 58);
            nudYearCP.Size = new Size(80, 20);
            SafeInitializeNumericUpDown(nudYearCP, 2020, 2030, DateTime.Now.Year);

            Label lblMonthCP = new Label();
            lblMonthCP.Text = "Tháng:";
            lblMonthCP.Location = new Point(250, 60);
            lblMonthCP.Size = new Size(50, 20);

            NumericUpDown nudMonthCP = new NumericUpDown();
            nudMonthCP.Name = "nudMonthCP";
            nudMonthCP.Location = new Point(310, 58);
            nudMonthCP.Size = new Size(60, 20);
            SafeInitializeNumericUpDown(nudMonthCP, 1, 12, DateTime.Now.Month);

            // --- Kỳ gốc (PP) ---
            Label lblPP = new Label();
            lblPP.Text = "Kỳ gốc (so sánh):";
            lblPP.Location = new Point(20, 90);
            lblPP.Size = new Size(100, 20);

            Label lblYearPP = new Label();
            lblYearPP.Text = "Năm:";
            lblYearPP.Location = new Point(100, 90);
            lblYearPP.Size = new Size(40, 20);

            NumericUpDown nudYearPP = new NumericUpDown();
            nudYearPP.Name = "nudYearPP";
            nudYearPP.Location = new Point(150, 88);
            nudYearPP.Size = new Size(80, 20);
            SafeInitializeNumericUpDown(nudYearPP, 2020, 2030, DateTime.Now.Year);

            Label lblMonthPP = new Label();
            lblMonthPP.Text = "Tháng:";
            lblMonthPP.Location = new Point(250, 90);
            lblMonthPP.Size = new Size(50, 20);

            NumericUpDown nudMonthPP = new NumericUpDown();
            nudMonthPP.Name = "nudMonthPP";
            nudMonthPP.Location = new Point(310, 88);
            nudMonthPP.Size = new Size(60, 20);
            SafeInitializeNumericUpDown(nudMonthPP, 1, 12, DateTime.Now.Month - 1 > 0 ? DateTime.Now.Month - 1 : 12);

            Button btnCalculate = new Button();
            btnCalculate.Text = "Tính toán";
            btnCalculate.Location = new Point(390, 70);
            btnCalculate.Size = new Size(120, 25);
            btnCalculate.Click += (s, e) => CalculateMonthlyRevenue();

            // --- Hiển thị Doanh thu Kỳ hiện tại ---
            Label lblRevenueCP = new Label();
            lblRevenueCP.Text = "Doanh thu kỳ hiện tại:";
            lblRevenueCP.Location = new Point(20, 130);
            lblRevenueCP.Size = new Size(150, 20);
            lblRevenueCP.Font = new Font("Arial", 12, FontStyle.Bold);

            Label lblRevenueValueCP = new Label();
            lblRevenueValueCP.Name = "lblMonthlyRevenueValueCP";
            lblRevenueValueCP.Text = "0 ₫";
            lblRevenueValueCP.Font = new Font("Arial", 14, FontStyle.Bold);
            lblRevenueValueCP.ForeColor = Color.Green;
            lblRevenueValueCP.Location = new Point(180, 130);
            lblRevenueValueCP.Size = new Size(200, 25);
            
            // --- Hiển thị Tăng trưởng ---
            Label lblGrowthRate = new Label();
            lblGrowthRate.Text = "Tỷ lệ tăng trưởng:";
            lblGrowthRate.Location = new Point(20, 170);
            lblGrowthRate.Size = new Size(150, 20);
            lblGrowthRate.Font = new Font("Arial", 12, FontStyle.Bold);

            Label lblGrowthRateValue = new Label();
            lblGrowthRateValue.Name = "lblGrowthRateValue";
            lblGrowthRateValue.Text = "0.00%";
            lblGrowthRateValue.Font = new Font("Arial", 14, FontStyle.Bold);
            lblGrowthRateValue.ForeColor = Color.Blue;
            lblGrowthRateValue.Location = new Point(180, 170);
            lblGrowthRateValue.Size = new Size(200, 25);

            panel.Controls.AddRange(new Control[] { 
                titleLabel, lblCP, lblYearCP, nudYearCP, lblMonthCP, nudMonthCP,
                lblPP, lblYearPP, nudYearPP, lblMonthPP, nudMonthPP, btnCalculate,
                lblRevenueCP, lblRevenueValueCP, lblGrowthRate, lblGrowthRateValue
            });

            tab.Controls.Add(panel);
        }

        private void CreateLowStockTab(TabPage tab)
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.AutoScroll = true;

            // Title
            Label titleLabel = new Label();
            titleLabel.Text = "SẢN PHẨM SẮP HẾT HÀNG";
            titleLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Size = new Size(300, 30);

            Button btnRefresh = new Button();
            btnRefresh.Text = "Làm mới";
            btnRefresh.Location = new Point(20, 60);
            btnRefresh.Size = new Size(100, 30);
            btnRefresh.Click += (s, e) => LoadLowStockProducts();

            // Low stock DataGridView
            DataGridView dgvLowStock = new DataGridView();
            dgvLowStock.Name = "dgvLowStock";
            dgvLowStock.Location = new Point(20, 100);
            dgvLowStock.Size = new Size(900, 500);
            dgvLowStock.ReadOnly = true;
            dgvLowStock.AllowUserToAddRows = false;
            dgvLowStock.AllowUserToDeleteRows = false;
            dgvLowStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLowStock.ScrollBars = ScrollBars.Both;
            dgvLowStock.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            panel.Controls.AddRange(new Control[] { 
                titleLabel, btnRefresh, dgvLowStock 
            });

            tab.Controls.Add(panel);
        }


        private void SafeSetColumnHeader(DataGridView dgv, string columnName, string headerText)
        {
            try
            {
                if (dgv.Columns[columnName] != null)
                {
                    dgv.Columns[columnName].HeaderText = headerText;
                }
            }
            catch
            {
                // Column doesn't exist, ignore
            }
        }

        private void SafeSetColumnFormat(DataGridView dgv, string columnName, string format)
        {
            try
            {
                if (dgv.Columns[columnName] != null)
                {
                    dgv.Columns[columnName].DefaultCellStyle.Format = format;
                }
            }
            catch
            {
                // Column doesn't exist, ignore
            }
        }

        private void LoadAllTabs()
        {
            try
            {
                LoadLowStockProducts();
                // Tải dữ liệu mặc định cho các tab mới
                LoadTopSellingProducts();
                LoadCustomerRanking();
                LoadProductSalesTrend();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu thống kê: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculateDailyRevenue()
        {
            try
            {
                TabControl mainTabControl = this.Controls["mainTabControl"] as TabControl;
                TabPage dailyTab = mainTabControl.TabPages[0];
                DateTimePicker dtpDate = dailyTab.Controls.Find("dtpDate", true)[0] as DateTimePicker;
                Label lblRevenueValue = dailyTab.Controls.Find("lblDailyRevenueValue", true)[0] as Label;

                decimal revenue = reportRepository.GetDailyRevenue(dtpDate.Value.Date);
                lblRevenueValue.Text = revenue.ToString("N0") + " ₫";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tính doanh thu ngày: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculateMonthlyRevenue()
        {
            try
            {
                TabControl mainTabControl = this.Controls["mainTabControl"] as TabControl;
                TabPage monthlyTab = mainTabControl.TabPages[1];
                
                // Kỳ hiện tại (CP)
                NumericUpDown nudYearCP = monthlyTab.Controls.Find("nudYearCP", true)[0] as NumericUpDown;
                NumericUpDown nudMonthCP = monthlyTab.Controls.Find("nudMonthCP", true)[0] as NumericUpDown;
                Label lblRevenueValueCP = monthlyTab.Controls.Find("lblMonthlyRevenueValueCP", true)[0] as Label;
                
                // Kỳ gốc (PP)
                NumericUpDown nudYearPP = monthlyTab.Controls.Find("nudYearPP", true)[0] as NumericUpDown;
                NumericUpDown nudMonthPP = monthlyTab.Controls.Find("nudMonthPP", true)[0] as NumericUpDown;
                Label lblGrowthRateValue = monthlyTab.Controls.Find("lblGrowthRateValue", true)[0] as Label;

                // Ensure values are within valid range
                int yearCP = (int)nudYearCP.Value;
                int monthCP = (int)nudMonthCP.Value;
                int yearPP = (int)nudYearPP.Value;
                int monthPP = (int)nudMonthPP.Value;

                // 1. Tính Doanh thu kỳ hiện tại
                decimal revenueCP = reportRepository.GetMonthlyRevenue(yearCP, monthCP);
                lblRevenueValueCP.Text = revenueCP.ToString("N0") + " ₫";
                
                // 2. Tính Tỷ lệ tăng trưởng
                decimal growthRate = reportRepository.GetSalesGrowthRateMonthly(yearCP, monthCP, yearPP, monthPP);
                
                if (growthRate >= 999.99m) // Xử lý trường hợp tăng trưởng vô hạn
                {
                    lblGrowthRateValue.Text = "Vô hạn (Từ 0)";
                    lblGrowthRateValue.ForeColor = Color.Red;
                }
                else
                {
                    // Hiển thị dưới dạng phần trăm (P)
                    lblGrowthRateValue.Text = growthRate.ToString("P2");
                    lblGrowthRateValue.ForeColor = growthRate >= 0 ? Color.Blue : Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tính doanh thu tháng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLowStockProducts()
        {
            try
            {
                TabControl mainTabControl = this.Controls["mainTabControl"] as TabControl;
                TabPage lowStockTab = mainTabControl.TabPages[2];
                DataGridView dgvLowStock = lowStockTab.Controls.Find("dgvLowStock", true)[0] as DataGridView;

                DataTable data = reportRepository.GetLowStockProducts();
                dgvLowStock.DataSource = data;

                if (dgvLowStock.Columns.Count > 0)
                {
                    // Use safe methods to set column headers
                    SafeSetColumnHeader(dgvLowStock, "ProductID", "Mã sản phẩm");
                    SafeSetColumnHeader(dgvLowStock, "ProductName", "Tên sản phẩm");
                    SafeSetColumnHeader(dgvLowStock, "Price", "Giá");
                    SafeSetColumnHeader(dgvLowStock, "StockQuantity", "Số lượng tồn");
                    SafeSetColumnHeader(dgvLowStock, "Unit", "Đơn vị");
                    SafeSetColumnHeader(dgvLowStock, "StockStatus", "Trạng thái");
                    SafeSetColumnHeader(dgvLowStock, "TotalSold", "Đã bán (30 ngày)");

                    // Format price column
                    SafeSetColumnFormat(dgvLowStock, "Price", "N0");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sản phẩm sắp hết hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // NEW METHOD: Tạo UI cho Top Sản phẩm Bán chạy
        private void CreateTopSellingProductsTab(TabPage tab)
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.AutoScroll = true;

            // Title
            Label titleLabel = new Label();
            titleLabel.Text = "TOP SẢN PHẨM BÁN CHẠY NHẤT";
            titleLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Size = new Size(400, 30);

            // Date selection
            Label lblStart = new Label();
            lblStart.Text = "Từ ngày:";
            lblStart.Location = new Point(20, 60);
            lblStart.Size = new Size(60, 20);

            DateTimePicker dtpStart = new DateTimePicker();
            dtpStart.Name = "dtpTopStart";
            dtpStart.Location = new Point(80, 58);
            dtpStart.Size = new Size(150, 20);
            dtpStart.Value = DateTime.Today.AddMonths(-1);

            Label lblEnd = new Label();
            lblEnd.Text = "Đến ngày:";
            lblEnd.Location = new Point(250, 60);
            lblEnd.Size = new Size(70, 20);

            DateTimePicker dtpEnd = new DateTimePicker();
            dtpEnd.Name = "dtpTopEnd";
            dtpEnd.Location = new Point(320, 58);
            dtpEnd.Size = new Size(150, 20);
            dtpEnd.Value = DateTime.Today;
            
            // Top N selection
            Label lblTopN = new Label();
            lblTopN.Text = "Top N:";
            lblTopN.Location = new Point(490, 60);
            lblTopN.Size = new Size(50, 20);
            
            NumericUpDown nudTopN = new NumericUpDown();
            nudTopN.Name = "nudTopN";
            nudTopN.Location = new Point(540, 58);
            nudTopN.Size = new Size(60, 20);
            SafeInitializeNumericUpDown(nudTopN, 1, 100, 10);

            Button btnRefresh = new Button();
            btnRefresh.Text = "Xem báo cáo";
            btnRefresh.Location = new Point(620, 56);
            btnRefresh.Size = new Size(120, 25);
            btnRefresh.Click += (s, e) => LoadTopSellingProducts();

            // DataGridView
            DataGridView dgvTopProducts = new DataGridView();
            dgvTopProducts.Name = "dgvTopProducts";
            dgvTopProducts.Location = new Point(20, 100);
            dgvTopProducts.Size = new Size(900, 500);
            dgvTopProducts.ReadOnly = true;
            dgvTopProducts.AllowUserToAddRows = false;
            dgvTopProducts.AllowUserToDeleteRows = false;
            dgvTopProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvTopProducts.ScrollBars = ScrollBars.Both;
            dgvTopProducts.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            panel.Controls.AddRange(new Control[] { 
                titleLabel, lblStart, dtpStart, lblEnd, dtpEnd, lblTopN, nudTopN, btnRefresh, dgvTopProducts 
            });

            tab.Controls.Add(panel);
        }

        // NEW METHOD: Tạo UI cho Xếp hạng Khách hàng
        private void CreateCustomerRankingTab(TabPage tab)
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.AutoScroll = true;

            // Title
            Label titleLabel = new Label();
            titleLabel.Text = "XẾP HẠNG KHÁCH HÀNG THEO TỔNG CHI TIÊU";
            titleLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Size = new Size(450, 30);

            // Date selection
            Label lblStart = new Label();
            lblStart.Text = "Từ ngày:";
            lblStart.Location = new Point(20, 60);
            lblStart.Size = new Size(60, 20);

            DateTimePicker dtpStart = new DateTimePicker();
            dtpStart.Name = "dtpRankStart";
            dtpStart.Location = new Point(80, 58);
            dtpStart.Size = new Size(150, 20);
            dtpStart.Value = DateTime.Today.AddMonths(-3);

            Label lblEnd = new Label();
            lblEnd.Text = "Đến ngày:";
            lblEnd.Location = new Point(250, 60);
            lblEnd.Size = new Size(70, 20);

            DateTimePicker dtpEnd = new DateTimePicker();
            dtpEnd.Name = "dtpRankEnd";
            dtpEnd.Location = new Point(320, 58);
            dtpEnd.Size = new Size(150, 20);
            dtpEnd.Value = DateTime.Today;

            Button btnRefresh = new Button();
            btnRefresh.Text = "Xem xếp hạng";
            btnRefresh.Location = new Point(490, 56);
            btnRefresh.Size = new Size(120, 25);
            btnRefresh.Click += (s, e) => LoadCustomerRanking();

            // DataGridView
            DataGridView dgvCustomerRanking = new DataGridView();
            dgvCustomerRanking.Name = "dgvCustomerRanking";
            dgvCustomerRanking.Location = new Point(20, 100);
            dgvCustomerRanking.Size = new Size(900, 500);
            dgvCustomerRanking.ReadOnly = true;
            dgvCustomerRanking.AllowUserToAddRows = false;
            dgvCustomerRanking.AllowUserToDeleteRows = false;
            dgvCustomerRanking.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvCustomerRanking.ScrollBars = ScrollBars.Both;
            dgvCustomerRanking.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            panel.Controls.AddRange(new Control[] { 
                titleLabel, lblStart, dtpStart, lblEnd, dtpEnd, btnRefresh, dgvCustomerRanking 
            });

            tab.Controls.Add(panel);
        }

        private void CreateProductSalesTrendTab(TabPage tab)
        {
            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.AutoScroll = true;

            Label titleLabel = new Label();
            titleLabel.Text = "XU HƯỚNG BÁN HÀNG SẢN PHẨM";
            titleLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Size = new Size(450, 30);

            Label lblProductID = new Label();
            lblProductID.Text = "Mã Sản phẩm:";
            lblProductID.Location = new Point(20, 60);
            lblProductID.Size = new Size(100, 20);

            NumericUpDown nudProductID = new NumericUpDown();
            nudProductID.Name = "nudTrendProductID";
            nudProductID.Location = new Point(120, 58);
            nudProductID.Size = new Size(80, 20);
            SafeInitializeNumericUpDown(nudProductID, 1, 99999, 1);

            Label lblStart = new Label();
            lblStart.Text = "Từ ngày:";
            lblStart.Location = new Point(220, 60);
            lblStart.Size = new Size(60, 20);

            DateTimePicker dtpTrendStart = new DateTimePicker();
            dtpTrendStart.Name = "dtpTrendStart";
            dtpTrendStart.Location = new Point(280, 58);
            dtpTrendStart.Size = new Size(150, 20);
            dtpTrendStart.Value = DateTime.Today.AddMonths(-1);

            Label lblEnd = new Label();
            lblEnd.Text = "Đến ngày:";
            lblEnd.Location = new Point(450, 60);
            lblEnd.Size = new Size(70, 20);

            DateTimePicker dtpTrendEnd = new DateTimePicker();
            dtpTrendEnd.Name = "dtpTrendEnd";
            dtpTrendEnd.Location = new Point(520, 58);
            dtpTrendEnd.Size = new Size(150, 20);
            dtpTrendEnd.Value = DateTime.Today;

            Button btnRefresh = new Button();
            btnRefresh.Text = "Xem xu hướng";
            btnRefresh.Location = new Point(690, 56);
            btnRefresh.Size = new Size(120, 25);
            btnRefresh.Click += (s, e) => LoadProductSalesTrend();

            DataGridView dgvSalesTrend = new DataGridView();
            dgvSalesTrend.Name = "dgvSalesTrend";
            dgvSalesTrend.Location = new Point(20, 100);
            dgvSalesTrend.Size = new Size(900, 500);
            dgvSalesTrend.ReadOnly = true;
            dgvSalesTrend.AllowUserToAddRows = false;
            dgvSalesTrend.AllowUserToDeleteRows = false;
            dgvSalesTrend.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvSalesTrend.ScrollBars = ScrollBars.Both;
            dgvSalesTrend.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            panel.Controls.AddRange(new Control[] {
                titleLabel, lblProductID, nudProductID, lblStart, dtpTrendStart, lblEnd, dtpTrendEnd, btnRefresh, dgvSalesTrend
            });

            tab.Controls.Add(panel);
        }

        private void LoadProductSalesTrend()
        {
            try
            {
                TabControl mainTabControl = this.Controls["mainTabControl"] as TabControl;
                TabPage tab = mainTabControl.TabPages[5];

                NumericUpDown nudProductID = tab.Controls.Find("nudTrendProductID", true)[0] as NumericUpDown;
                DateTimePicker dtpTrendStart = tab.Controls.Find("dtpTrendStart", true)[0] as DateTimePicker;
                DateTimePicker dtpTrendEnd = tab.Controls.Find("dtpTrendEnd", true)[0] as DateTimePicker;
                DataGridView dgvSalesTrend = tab.Controls.Find("dgvSalesTrend", true)[0] as DataGridView;

                DataTable data = reportRepository.GetProductSalesTrend((int)nudProductID.Value, dtpTrendStart.Value.Date, dtpTrendEnd.Value.Date);
                dgvSalesTrend.DataSource = data;

                if (dgvSalesTrend.Columns.Count > 0)
                {
                    SafeSetColumnHeader(dgvSalesTrend, "SaleDay", "Ngày bán");
                    SafeSetColumnHeader(dgvSalesTrend, "DailyRevenue", "Doanh thu ngày");
                    SafeSetColumnHeader(dgvSalesTrend, "CumulativeRevenue", "Doanh thu Lũy kế");
                    SafeSetColumnHeader(dgvSalesTrend, "RollingAverage7Day", "TB Trượt (7 ngày)");
                    SafeSetColumnFormat(dgvSalesTrend, "DailyRevenue", "N0");
                    SafeSetColumnFormat(dgvSalesTrend, "CumulativeRevenue", "N0");
                    SafeSetColumnFormat(dgvSalesTrend, "RollingAverage7Day", "N0");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải báo cáo xu hướng sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // NEW METHOD: Tải dữ liệu Top Sản phẩm
        private void LoadTopSellingProducts()
        {
            try
            {
                TabControl mainTabControl = this.Controls["mainTabControl"] as TabControl;
                TabPage tab = mainTabControl.TabPages[3]; // Tab 4
                
                DateTimePicker dtpStart = tab.Controls.Find("dtpTopStart", true)[0] as DateTimePicker;
                DateTimePicker dtpEnd = tab.Controls.Find("dtpTopEnd", true)[0] as DateTimePicker;
                NumericUpDown nudTopN = tab.Controls.Find("nudTopN", true)[0] as NumericUpDown;
                DataGridView dgvTopProducts = tab.Controls.Find("dgvTopProducts", true)[0] as DataGridView;

                // Lấy dữ liệu
                DataTable data = reportRepository.GetTopSellingProducts(dtpStart.Value.Date, dtpEnd.Value.Date, (int)nudTopN.Value);
                dgvTopProducts.DataSource = data;

                if (dgvTopProducts.Columns.Count > 0)
                {
                    // Định dạng cột
                    SafeSetColumnHeader(dgvTopProducts, "ProductID", "Mã SP");
                    SafeSetColumnHeader(dgvTopProducts, "ProductName", "Tên sản phẩm");
                    SafeSetColumnHeader(dgvTopProducts, "Unit", "Đơn vị");
                    SafeSetColumnHeader(dgvTopProducts, "TotalQuantitySold", "Tổng SL Bán");
                    SafeSetColumnHeader(dgvTopProducts, "TotalRevenue", "Tổng Doanh thu");
                    SafeSetColumnFormat(dgvTopProducts, "TotalRevenue", "N0");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải báo cáo Top Sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // NEW METHOD: Tải dữ liệu Xếp hạng Khách hàng
        private void LoadCustomerRanking()
        {
            try
            {
                TabControl mainTabControl = this.Controls["mainTabControl"] as TabControl;
                TabPage tab = mainTabControl.TabPages[4]; // Tab 5

                DateTimePicker dtpStart = tab.Controls.Find("dtpRankStart", true)[0] as DateTimePicker;
                DateTimePicker dtpEnd = tab.Controls.Find("dtpRankEnd", true)[0] as DateTimePicker;
                DataGridView dgvCustomerRanking = tab.Controls.Find("dgvCustomerRanking", true)[0] as DataGridView;

                // Lấy dữ liệu
                DataTable data = reportRepository.GetCustomerRanking(dtpStart.Value.Date, dtpEnd.Value.Date);
                dgvCustomerRanking.DataSource = data;

                if (dgvCustomerRanking.Columns.Count > 0)
                {
                    // Định dạng cột
                    SafeSetColumnHeader(dgvCustomerRanking, "Ranking", "Hạng");
                    SafeSetColumnHeader(dgvCustomerRanking, "CustomerID", "Mã KH");
                    SafeSetColumnHeader(dgvCustomerRanking, "CustomerName", "Tên Khách hàng");
                    SafeSetColumnHeader(dgvCustomerRanking, "Phone", "SĐT");
                    SafeSetColumnHeader(dgvCustomerRanking, "LoyaltyPoints", "Điểm tích lũy");
                    SafeSetColumnHeader(dgvCustomerRanking, "TotalSpending", "Tổng chi tiêu");
                    SafeSetColumnFormat(dgvCustomerRanking, "TotalSpending", "N0");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải bảng xếp hạng khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}