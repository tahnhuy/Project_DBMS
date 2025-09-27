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

            // Tab 2: Doanh thu tháng
            TabPage monthlyRevenueTab = new TabPage("Doanh thu tháng");
            monthlyRevenueTab.AutoScroll = true;
            CreateMonthlyRevenueTab(monthlyRevenueTab);
            mainTabControl.TabPages.Add(monthlyRevenueTab);

            // Tab 3: Tồn kho thấp
            TabPage lowStockTab = new TabPage("Tồn kho thấp");
            lowStockTab.AutoScroll = true;
            CreateLowStockTab(lowStockTab);
            mainTabControl.TabPages.Add(lowStockTab);


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
            titleLabel.Text = "DOANH THU THEO THÁNG";
            titleLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Size = new Size(300, 30);

            // Month/Year selection
            Label lblYear = new Label();
            lblYear.Text = "Năm:";
            lblYear.Location = new Point(20, 60);
            lblYear.Size = new Size(40, 20);

            NumericUpDown nudYear = new NumericUpDown();
            nudYear.Name = "nudYear";
            nudYear.Location = new Point(70, 58);
            nudYear.Size = new Size(80, 20);
            SafeInitializeNumericUpDown(nudYear, 2020, 2030, DateTime.Now.Year);

            Label lblMonth = new Label();
            lblMonth.Text = "Tháng:";
            lblMonth.Location = new Point(170, 60);
            lblMonth.Size = new Size(50, 20);

            NumericUpDown nudMonth = new NumericUpDown();
            nudMonth.Name = "nudMonth";
            nudMonth.Location = new Point(230, 58);
            nudMonth.Size = new Size(60, 20);
            SafeInitializeNumericUpDown(nudMonth, 1, 12, DateTime.Now.Month);

            Button btnCalculate = new Button();
            btnCalculate.Text = "Tính doanh thu";
            btnCalculate.Location = new Point(310, 56);
            btnCalculate.Size = new Size(120, 25);
            btnCalculate.Click += (s, e) => CalculateMonthlyRevenue();

            // Revenue display
            Label lblRevenue = new Label();
            lblRevenue.Text = "Doanh thu:";
            lblRevenue.Location = new Point(20, 100);
            lblRevenue.Size = new Size(100, 20);
            lblRevenue.Font = new Font("Arial", 12, FontStyle.Bold);

            Label lblRevenueValue = new Label();
            lblRevenueValue.Name = "lblMonthlyRevenueValue";
            lblRevenueValue.Text = "0 ₫";
            lblRevenueValue.Font = new Font("Arial", 14, FontStyle.Bold);
            lblRevenueValue.ForeColor = Color.Green;
            lblRevenueValue.Location = new Point(130, 100);
            lblRevenueValue.Size = new Size(200, 25);

            panel.Controls.AddRange(new Control[] { 
                titleLabel, lblYear, nudYear, lblMonth, nudMonth, btnCalculate, lblRevenue, lblRevenueValue 
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
            dgvLowStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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
                NumericUpDown nudYear = monthlyTab.Controls.Find("nudYear", true)[0] as NumericUpDown;
                NumericUpDown nudMonth = monthlyTab.Controls.Find("nudMonth", true)[0] as NumericUpDown;
                Label lblRevenueValue = monthlyTab.Controls.Find("lblMonthlyRevenueValue", true)[0] as Label;

                // Ensure values are within valid range
                int year = Math.Max(2020, Math.Min(2030, (int)nudYear.Value));
                int month = Math.Max(1, Math.Min(12, (int)nudMonth.Value));

                decimal revenue = reportRepository.GetMonthlyRevenue(year, month);
                lblRevenueValue.Text = revenue.ToString("N0") + " ₫";
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

    }
}