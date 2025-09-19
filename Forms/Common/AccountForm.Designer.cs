namespace Sale_Management.Forms
{
    partial class AccountForm
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
            this.groupBox_AccountList = new System.Windows.Forms.GroupBox();
            this.txt_Search = new System.Windows.Forms.TextBox();
            this.label_Search = new System.Windows.Forms.Label();
            this.btn_Delete = new System.Windows.Forms.Button();
            this.btn_Edit = new System.Windows.Forms.Button();
            this.btn_Refresh = new System.Windows.Forms.Button();
            this.dataGridView_Accounts = new System.Windows.Forms.DataGridView();
            this.groupBox_AccountList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Accounts)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox_AccountList
            // 
            this.groupBox_AccountList.Controls.Add(this.txt_Search);
            this.groupBox_AccountList.Controls.Add(this.label_Search);
            this.groupBox_AccountList.Controls.Add(this.btn_Delete);
            this.groupBox_AccountList.Controls.Add(this.btn_Edit);
            this.groupBox_AccountList.Controls.Add(this.btn_Refresh);
            this.groupBox_AccountList.Controls.Add(this.dataGridView_Accounts);
            this.groupBox_AccountList.Location = new System.Drawing.Point(121, 29);
            this.groupBox_AccountList.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_AccountList.Name = "groupBox_AccountList";
            this.groupBox_AccountList.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_AccountList.Size = new System.Drawing.Size(1200, 709);
            this.groupBox_AccountList.TabIndex = 0;
            this.groupBox_AccountList.TabStop = false;
            this.groupBox_AccountList.Text = "Quản lý tài khoản";
            // 
            // txt_Search
            // 
            this.txt_Search.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txt_Search.Location = new System.Drawing.Point(201, 625);
            this.txt_Search.Margin = new System.Windows.Forms.Padding(4);
            this.txt_Search.Name = "txt_Search";
            this.txt_Search.Size = new System.Drawing.Size(280, 38);
            this.txt_Search.TabIndex = 5;
            this.txt_Search.TextChanged += new System.EventHandler(this.txt_Search_TextChanged);
            // 
            // label_Search
            // 
            this.label_Search.AutoSize = true;
            this.label_Search.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label_Search.Location = new System.Drawing.Point(40, 628);
            this.label_Search.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Search.Name = "label_Search";
            this.label_Search.Size = new System.Drawing.Size(131, 31);
            this.label_Search.TabIndex = 4;
            this.label_Search.Text = "Tìm kiếm:";
            // 
            // btn_Delete
            // 
            this.btn_Delete.Location = new System.Drawing.Point(269, 512);
            this.btn_Delete.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(190, 80);
            this.btn_Delete.TabIndex = 3;
            this.btn_Delete.Text = "Xóa tài khoản";
            this.btn_Delete.UseVisualStyleBackColor = true;
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // btn_Edit
            // 
            this.btn_Edit.Location = new System.Drawing.Point(46, 512);
            this.btn_Edit.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Edit.Name = "btn_Edit";
            this.btn_Edit.Size = new System.Drawing.Size(173, 80);
            this.btn_Edit.TabIndex = 2;
            this.btn_Edit.Text = "Chỉnh sửa";
            this.btn_Edit.UseVisualStyleBackColor = true;
            this.btn_Edit.Click += new System.EventHandler(this.btn_Edit_Click);
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.Location = new System.Drawing.Point(519, 512);
            this.btn_Refresh.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(167, 80);
            this.btn_Refresh.TabIndex = 1;
            this.btn_Refresh.Text = "Làm mới";
            this.btn_Refresh.UseVisualStyleBackColor = true;
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // dataGridView_Accounts
            // 
            this.dataGridView_Accounts.AllowUserToAddRows = false;
            this.dataGridView_Accounts.AllowUserToDeleteRows = false;
            this.dataGridView_Accounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Accounts.Location = new System.Drawing.Point(22, 42);
            this.dataGridView_Accounts.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView_Accounts.Name = "dataGridView_Accounts";
            this.dataGridView_Accounts.ReadOnly = true;
            this.dataGridView_Accounts.RowHeadersWidth = 62;
            this.dataGridView_Accounts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_Accounts.Size = new System.Drawing.Size(1140, 450);
            this.dataGridView_Accounts.TabIndex = 0;
            this.dataGridView_Accounts.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_Accounts_CellDoubleClick);
            // 
            // AccountForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1442, 811);
            this.Controls.Add(this.groupBox_AccountList);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AccountForm";
            this.Text = "Quản lý tài khoản";
            this.groupBox_AccountList.ResumeLayout(false);
            this.groupBox_AccountList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Accounts)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_AccountList;
        private System.Windows.Forms.TextBox txt_Search;
        private System.Windows.Forms.Label label_Search;
        private System.Windows.Forms.Button btn_Delete;
        private System.Windows.Forms.Button btn_Edit;
        private System.Windows.Forms.Button btn_Refresh;
        private System.Windows.Forms.DataGridView dataGridView_Accounts;
    }
}