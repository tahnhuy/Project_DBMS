namespace Sale_Management.Forms
{
    partial class CustomerManageForm
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
            this.dgv_Customers = new System.Windows.Forms.DataGridView();
            this.lbl_nameSearch = new System.Windows.Forms.Label();
            this.lbl_idSearch = new System.Windows.Forms.Label();
            this.txt_nameSearch = new System.Windows.Forms.TextBox();
            this.txt_idSearch = new System.Windows.Forms.TextBox();
            this.btn_Add = new System.Windows.Forms.Button();
            this.btn_Edit = new System.Windows.Forms.Button();
            this.btn_Delete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Customers)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_Customers
            // 
            this.dgv_Customers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Customers.Location = new System.Drawing.Point(12, 235);
            this.dgv_Customers.Name = "dgv_Customers";
            this.dgv_Customers.RowHeadersWidth = 82;
            this.dgv_Customers.RowTemplate.Height = 33;
            this.dgv_Customers.Size = new System.Drawing.Size(1297, 547);
            this.dgv_Customers.TabIndex = 0;
            // 
            // lbl_nameSearch
            // 
            this.lbl_nameSearch.AutoSize = true;
            this.lbl_nameSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lbl_nameSearch.Location = new System.Drawing.Point(47, 47);
            this.lbl_nameSearch.Name = "lbl_nameSearch";
            this.lbl_nameSearch.Size = new System.Drawing.Size(279, 31);
            this.lbl_nameSearch.TabIndex = 1;
            this.lbl_nameSearch.Text = "Tìm khách hàng(Tên):";
            this.lbl_nameSearch.Click += new System.EventHandler(this.label1_Click);
            // 
            // lbl_idSearch
            // 
            this.lbl_idSearch.AutoSize = true;
            this.lbl_idSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lbl_idSearch.Location = new System.Drawing.Point(47, 118);
            this.lbl_idSearch.Name = "lbl_idSearch";
            this.lbl_idSearch.Size = new System.Drawing.Size(260, 31);
            this.lbl_idSearch.TabIndex = 2;
            this.lbl_idSearch.Text = "Tìm khách hàng(ID):";
            // 
            // txt_nameSearch
            // 
            this.txt_nameSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txt_nameSearch.Location = new System.Drawing.Point(332, 47);
            this.txt_nameSearch.Name = "txt_nameSearch";
            this.txt_nameSearch.Size = new System.Drawing.Size(272, 44);
            this.txt_nameSearch.TabIndex = 3;
            this.txt_nameSearch.TextChanged += new System.EventHandler(this.txt_nameSearch_TextChanged);
            // 
            // txt_idSearch
            // 
            this.txt_idSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txt_idSearch.Location = new System.Drawing.Point(332, 112);
            this.txt_idSearch.Name = "txt_idSearch";
            this.txt_idSearch.Size = new System.Drawing.Size(272, 44);
            this.txt_idSearch.TabIndex = 4;
            this.txt_idSearch.TextChanged += new System.EventHandler(this.txt_idSearch_TextChanged);
            // 
            // btn_Add
            // 
            this.btn_Add.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btn_Add.Location = new System.Drawing.Point(654, 61);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(210, 73);
            this.btn_Add.TabIndex = 5;
            this.btn_Add.Text = "Thêm";
            this.btn_Add.UseVisualStyleBackColor = true;
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // btn_Edit
            // 
            this.btn_Edit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btn_Edit.Location = new System.Drawing.Point(1094, 61);
            this.btn_Edit.Name = "btn_Edit";
            this.btn_Edit.Size = new System.Drawing.Size(215, 73);
            this.btn_Edit.TabIndex = 6;
            this.btn_Edit.Text = "Sửa";
            this.btn_Edit.UseVisualStyleBackColor = true;
            this.btn_Edit.Click += new System.EventHandler(this.btn_Edit_Click);
            // 
            // btn_Delete
            // 
            this.btn_Delete.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btn_Delete.Location = new System.Drawing.Point(884, 61);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(192, 73);
            this.btn_Delete.TabIndex = 7;
            this.btn_Delete.Text = "Xóa";
            this.btn_Delete.UseVisualStyleBackColor = true;
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // CustomerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1321, 794);
            this.Controls.Add(this.btn_Delete);
            this.Controls.Add(this.btn_Edit);
            this.Controls.Add(this.btn_Add);
            this.Controls.Add(this.txt_idSearch);
            this.Controls.Add(this.txt_nameSearch);
            this.Controls.Add(this.lbl_idSearch);
            this.Controls.Add(this.lbl_nameSearch);
            this.Controls.Add(this.dgv_Customers);
            this.Name = "CustomerForm";
            this.Text = "CustomerForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Customers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_Customers;
        private System.Windows.Forms.Label lbl_nameSearch;
        private System.Windows.Forms.Label lbl_idSearch;
        private System.Windows.Forms.TextBox txt_nameSearch;
		private System.Windows.Forms.TextBox txt_idSearch;
		private System.Windows.Forms.Button btn_Add;
		private System.Windows.Forms.Button btn_Edit;
		private System.Windows.Forms.Button btn_Delete;
    }
}