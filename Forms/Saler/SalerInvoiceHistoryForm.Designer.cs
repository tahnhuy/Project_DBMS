namespace Sale_Management.Forms
{
    partial class SalerInvoiceHistoryForm
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
            this.dgv_InvoiceHistory = new System.Windows.Forms.DataGridView();
            this.btn_Refresh = new System.Windows.Forms.Button();
            this.btn_Close = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_InvoiceHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_InvoiceHistory
            // 
            this.dgv_InvoiceHistory.AllowUserToAddRows = false;
            this.dgv_InvoiceHistory.AllowUserToDeleteRows = false;
            this.dgv_InvoiceHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_InvoiceHistory.Location = new System.Drawing.Point(12, 50);
            this.dgv_InvoiceHistory.Name = "dgv_InvoiceHistory";
            this.dgv_InvoiceHistory.ReadOnly = true;
            this.dgv_InvoiceHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_InvoiceHistory.Size = new System.Drawing.Size(960, 400);
            this.dgv_InvoiceHistory.TabIndex = 0;
            this.dgv_InvoiceHistory.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_InvoiceHistory_CellDoubleClick);
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.Location = new System.Drawing.Point(12, 460);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(100, 30);
            this.btn_Refresh.TabIndex = 1;
            this.btn_Refresh.Text = "Làm mới";
            this.btn_Refresh.UseVisualStyleBackColor = true;
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // btn_Close
            // 
            this.btn_Close.Location = new System.Drawing.Point(872, 460);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(100, 30);
            this.btn_Close.TabIndex = 2;
            this.btn_Close.Text = "Đóng";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "Lịch sử hóa đơn bán hàng";
            // 
            // SalerInvoiceHistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 502);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.btn_Refresh);
            this.Controls.Add(this.dgv_InvoiceHistory);
            this.Name = "SalerInvoiceHistoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lịch sử hóa đơn";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_InvoiceHistory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_InvoiceHistory;
        private System.Windows.Forms.Button btn_Refresh;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.Label label1;
    }
}
