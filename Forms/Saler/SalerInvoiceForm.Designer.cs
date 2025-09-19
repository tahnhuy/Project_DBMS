namespace Sale_Management.Forms
{
    partial class SalerInvoiceForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmb_Customer = new System.Windows.Forms.ComboBox();
            this.cmb_Product = new System.Windows.Forms.ComboBox();
            this.txt_Quantity = new System.Windows.Forms.TextBox();
            this.btn_AddProduct = new System.Windows.Forms.Button();
            this.dgv_InvoiceItems = new System.Windows.Forms.DataGridView();
            this.lbl_Total = new System.Windows.Forms.Label();
            this.btn_CreateInvoice = new System.Windows.Forms.Button();
            this.btn_Clear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_InvoiceItems)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tạo hóa đơn";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Khách hàng:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Sản phẩm:";
            // 
            // cmb_Customer
            // 
            this.cmb_Customer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Customer.FormattingEnabled = true;
            this.cmb_Customer.Location = new System.Drawing.Point(100, 47);
            this.cmb_Customer.Name = "cmb_Customer";
            this.cmb_Customer.Size = new System.Drawing.Size(200, 21);
            this.cmb_Customer.TabIndex = 3;
            // 
            // cmb_Product
            // 
            this.cmb_Product.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Product.FormattingEnabled = true;
            this.cmb_Product.Location = new System.Drawing.Point(100, 77);
            this.cmb_Product.Name = "cmb_Product";
            this.cmb_Product.Size = new System.Drawing.Size(200, 21);
            this.cmb_Product.TabIndex = 4;
            // 
            // txt_Quantity
            // 
            this.txt_Quantity.Location = new System.Drawing.Point(320, 77);
            this.txt_Quantity.Name = "txt_Quantity";
            this.txt_Quantity.Size = new System.Drawing.Size(80, 20);
            this.txt_Quantity.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(320, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Số lượng:";
            // 
            // btn_AddProduct
            // 
            this.btn_AddProduct.Location = new System.Drawing.Point(420, 75);
            this.btn_AddProduct.Name = "btn_AddProduct";
            this.btn_AddProduct.Size = new System.Drawing.Size(75, 23);
            this.btn_AddProduct.TabIndex = 6;
            this.btn_AddProduct.Text = "Thêm";
            this.btn_AddProduct.UseVisualStyleBackColor = true;
            this.btn_AddProduct.Click += new System.EventHandler(this.btn_AddProduct_Click);
            // 
            // dgv_InvoiceItems
            // 
            this.dgv_InvoiceItems.AllowUserToAddRows = false;
            this.dgv_InvoiceItems.AllowUserToDeleteRows = false;
            this.dgv_InvoiceItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_InvoiceItems.Location = new System.Drawing.Point(12, 120);
            this.dgv_InvoiceItems.Name = "dgv_InvoiceItems";
            this.dgv_InvoiceItems.ReadOnly = true;
            this.dgv_InvoiceItems.Size = new System.Drawing.Size(776, 300);
            this.dgv_InvoiceItems.TabIndex = 7;
            // 
            // lbl_Total
            // 
            this.lbl_Total.AutoSize = true;
            this.lbl_Total.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_Total.Location = new System.Drawing.Point(12, 430);
            this.lbl_Total.Name = "lbl_Total";
            this.lbl_Total.Size = new System.Drawing.Size(120, 20);
            this.lbl_Total.TabIndex = 8;
            this.lbl_Total.Text = "Tổng cộng: 0 VNĐ";
            // 
            // btn_CreateInvoice
            // 
            this.btn_CreateInvoice.Location = new System.Drawing.Point(600, 430);
            this.btn_CreateInvoice.Name = "btn_CreateInvoice";
            this.btn_CreateInvoice.Size = new System.Drawing.Size(100, 30);
            this.btn_CreateInvoice.TabIndex = 9;
            this.btn_CreateInvoice.Text = "Tạo hóa đơn";
            this.btn_CreateInvoice.UseVisualStyleBackColor = true;
            this.btn_CreateInvoice.Click += new System.EventHandler(this.btn_CreateInvoice_Click);
            // 
            // btn_Clear
            // 
            this.btn_Clear.Location = new System.Drawing.Point(500, 430);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(75, 30);
            this.btn_Clear.TabIndex = 10;
            this.btn_Clear.Text = "Xóa";
            this.btn_Clear.UseVisualStyleBackColor = true;
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // SalerInvoiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.btn_Clear);
            this.Controls.Add(this.btn_CreateInvoice);
            this.Controls.Add(this.lbl_Total);
            this.Controls.Add(this.dgv_InvoiceItems);
            this.Controls.Add(this.btn_AddProduct);
            this.Controls.Add(this.txt_Quantity);
            this.Controls.Add(this.cmb_Product);
            this.Controls.Add(this.cmb_Customer);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SalerInvoiceForm";
            this.Text = "Tạo hóa đơn";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_InvoiceItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmb_Customer;
        private System.Windows.Forms.ComboBox cmb_Product;
        private System.Windows.Forms.TextBox txt_Quantity;
        private System.Windows.Forms.Button btn_AddProduct;
        private System.Windows.Forms.DataGridView dgv_InvoiceItems;
        private System.Windows.Forms.Label lbl_Total;
        private System.Windows.Forms.Button btn_CreateInvoice;
        private System.Windows.Forms.Button btn_Clear;
    }
}
