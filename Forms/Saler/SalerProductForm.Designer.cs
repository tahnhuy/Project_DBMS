namespace Sale_Management.Forms
{
    partial class SalerProductForm
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
            this.dgv_Products = new System.Windows.Forms.DataGridView();
            this.btn_Refresh = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_nameSearch = new System.Windows.Forms.TextBox();
            this.txt_idSearch = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Products)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_Products
            // 
            this.dgv_Products.AllowUserToAddRows = false;
            this.dgv_Products.AllowUserToDeleteRows = false;
            this.dgv_Products.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Products.Location = new System.Drawing.Point(24, 154);
            this.dgv_Products.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.dgv_Products.Name = "dgv_Products";
            this.dgv_Products.ReadOnly = true;
            this.dgv_Products.RowHeadersWidth = 82;
            this.dgv_Products.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_Products.Size = new System.Drawing.Size(1529, 549);
            this.dgv_Products.TabIndex = 0;
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.Location = new System.Drawing.Point(24, 734);
            this.btn_Refresh.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(179, 68);
            this.btn_Refresh.TabIndex = 1;
            this.btn_Refresh.Text = "Làm mới";
            this.btn_Refresh.UseVisualStyleBackColor = true;
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(24, 29);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(343, 44);
            this.label1.TabIndex = 3;
            this.label1.Text = "Quản lý sản phẩm";
            // 
            // txt_nameSearch
            // 
            this.txt_nameSearch.Location = new System.Drawing.Point(200, 96);
            this.txt_nameSearch.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txt_nameSearch.Name = "txt_nameSearch";
            this.txt_nameSearch.Size = new System.Drawing.Size(396, 31);
            this.txt_nameSearch.TabIndex = 4;
            this.txt_nameSearch.TextChanged += new System.EventHandler(this.txt_nameSearch_TextChanged);
            // 
            // txt_idSearch
            // 
            this.txt_idSearch.Location = new System.Drawing.Point(800, 96);
            this.txt_idSearch.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txt_idSearch.Name = "txt_idSearch";
            this.txt_idSearch.Size = new System.Drawing.Size(196, 31);
            this.txt_idSearch.TabIndex = 5;
            this.txt_idSearch.TextChanged += new System.EventHandler(this.txt_idSearch_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 102);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "Tìm theo tên:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(640, 102);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 25);
            this.label3.TabIndex = 7;
            this.label3.Text = "Tìm theo ID:";
            // 
            // SalerProductForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1600, 962);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_idSearch);
            this.Controls.Add(this.txt_nameSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Refresh);
            this.Controls.Add(this.dgv_Products);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "SalerProductForm";
            this.Text = "Quản lý sản phẩm";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Products)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_Products;
        private System.Windows.Forms.Button btn_Refresh;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_nameSearch;
        private System.Windows.Forms.TextBox txt_idSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
