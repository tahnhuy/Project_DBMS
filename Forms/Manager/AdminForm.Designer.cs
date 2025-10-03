namespace Sale_Management
{
    partial class AdminForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.msi_Product = new System.Windows.Forms.ToolStripMenuItem();
            this.msi_Customer = new System.Windows.Forms.ToolStripMenuItem();
            this.msi_Discount = new System.Windows.Forms.ToolStripMenuItem();
            this.msi_AccManage = new System.Windows.Forms.ToolStripMenuItem();
            this.msi_RestoreProduct = new System.Windows.Forms.ToolStripMenuItem();
            this.msi_createAcc = new System.Windows.Forms.ToolStripMenuItem();
            this.msi_Statistics = new System.Windows.Forms.ToolStripMenuItem();
            this.tàiKhoảnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panel_Container = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msi_Product,
            this.msi_Customer,
            this.msi_Discount,
            this.msi_RestoreProduct,
            this.msi_AccManage,
            this.msi_Statistics,
            this.tàiKhoảnToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1341, 40);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // msi_Product
            // 
            this.msi_Product.Name = "msi_Product";
            this.msi_Product.Size = new System.Drawing.Size(141, 36);
            this.msi_Product.Text = "Sản phẩm";
            this.msi_Product.Click += new System.EventHandler(this.msi_Product_Click);
            // 
            // msi_Customer
            // 
            this.msi_Customer.Name = "msi_Customer";
            this.msi_Customer.Size = new System.Drawing.Size(160, 36);
            this.msi_Customer.Text = "Khách hàng";
            this.msi_Customer.Click += new System.EventHandler(this.kháchHàngToolStripMenuItem_Click);
            // 
            // msi_Discount
            // 
            this.msi_Discount.Name = "msi_Discount";
            this.msi_Discount.Size = new System.Drawing.Size(128, 36);
            this.msi_Discount.Text = "Giảm giá";
            this.msi_Discount.Click += new System.EventHandler(this.msi_Discount_Click);
            // 
            // msi_AccManage
            // 
            this.msi_AccManage.Name = "msi_AccManage";
            this.msi_AccManage.Size = new System.Drawing.Size(217, 36);
            this.msi_AccManage.Text = "Quản lí tài khoản";
            this.msi_AccManage.Click += new System.EventHandler(this.msi_AccManage_Click);
            // 
            // msi_RestoreProduct
            // 
            this.msi_RestoreProduct.Name = "msi_RestoreProduct";
            this.msi_RestoreProduct.Size = new System.Drawing.Size(242, 36);
            this.msi_RestoreProduct.Text = "Khôi phục sản phẩm";
            this.msi_RestoreProduct.Click += new System.EventHandler(this.msi_RestoreProduct_Click);
            // 
            // msi_createAcc
            // 
            this.msi_createAcc.Name = "msi_createAcc";
            this.msi_createAcc.Size = new System.Drawing.Size(179, 36);
            this.msi_createAcc.Text = "Tạo tài khoản";
            // 
            // msi_Statistics
            // 
            this.msi_Statistics.Name = "msi_Statistics";
            this.msi_Statistics.Size = new System.Drawing.Size(120, 36);
            this.msi_Statistics.Text = "Thống kê";
            this.msi_Statistics.Click += new System.EventHandler(this.msi_Statistics_Click);
            // 
            // tàiKhoảnToolStripMenuItem
            // 
            this.tàiKhoảnToolStripMenuItem.Name = "tàiKhoảnToolStripMenuItem";
            this.tàiKhoảnToolStripMenuItem.Size = new System.Drawing.Size(135, 36);
            this.tàiKhoảnToolStripMenuItem.Text = "Tài khoản";
            this.tàiKhoảnToolStripMenuItem.Click += new System.EventHandler(this.tàiKhoảnToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // panel_Container
            // 
            this.panel_Container.Location = new System.Drawing.Point(0, 43);
            this.panel_Container.Name = "panel_Container";
            this.panel_Container.Size = new System.Drawing.Size(1341, 775);
            this.panel_Container.TabIndex = 2;
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1341, 815);
            this.Controls.Add(this.panel_Container);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "AdminForm";
            this.Text = "Quản lí bản hàng Minimart";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem msi_Product;
        private System.Windows.Forms.ToolStripMenuItem msi_Customer;
        private System.Windows.Forms.ToolStripMenuItem msi_Discount;
        private System.Windows.Forms.ToolStripMenuItem msi_createAcc;
        private System.Windows.Forms.ToolStripMenuItem msi_Statistics;
        private System.Windows.Forms.ToolStripMenuItem tàiKhoảnToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Panel panel_Container;
        private System.Windows.Forms.ToolStripMenuItem msi_AccManage;
        private System.Windows.Forms.ToolStripMenuItem msi_RestoreProduct;
    }
}

