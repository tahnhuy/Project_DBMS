namespace Sale_Management.Forms
{
    partial class AccountManageForm
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
            this.btn_Backup = new System.Windows.Forms.Button();
            this.btn_Statistics = new System.Windows.Forms.Button();
            this.btn_Search = new System.Windows.Forms.Button();
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
            this.groupBox_AccountList.Controls.Add(this.btn_Backup);
            this.groupBox_AccountList.Controls.Add(this.btn_Statistics);
            this.groupBox_AccountList.Controls.Add(this.btn_Search);
            this.groupBox_AccountList.Controls.Add(this.txt_Search);
            this.groupBox_AccountList.Controls.Add(this.label_Search);
            this.groupBox_AccountList.Controls.Add(this.btn_Delete);
            this.groupBox_AccountList.Controls.Add(this.btn_Edit);
            this.groupBox_AccountList.Controls.Add(this.btn_Refresh);
            this.groupBox_AccountList.Controls.Add(this.dataGridView_Accounts);
            this.groupBox_AccountList.Location = new System.Drawing.Point(16, 15);
            this.groupBox_AccountList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox_AccountList.Name = "groupBox_AccountList";
            this.groupBox_AccountList.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox_AccountList.Size = new System.Drawing.Size(1200, 600);
            this.groupBox_AccountList.TabIndex = 0;
            this.groupBox_AccountList.TabStop = false;
            this.groupBox_AccountList.Text = "Quản lý tài khoản";
            // 
            // btn_Backup
            // 
            this.btn_Backup.Location = new System.Drawing.Point(800, 550);
            this.btn_Backup.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_Backup.Name = "btn_Backup";
            this.btn_Backup.Size = new System.Drawing.Size(120, 40);
            this.btn_Backup.TabIndex = 8;
            this.btn_Backup.Text = "Backup";
            this.btn_Backup.UseVisualStyleBackColor = true;
            this.btn_Backup.Click += new System.EventHandler(this.btn_Backup_Click);
            // 
            // btn_Statistics
            // 
            this.btn_Statistics.Location = new System.Drawing.Point(650, 550);
            this.btn_Statistics.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_Statistics.Name = "btn_Statistics";
            this.btn_Statistics.Size = new System.Drawing.Size(120, 40);
            this.btn_Statistics.TabIndex = 7;
            this.btn_Statistics.Text = "Thống kê";
            this.btn_Statistics.UseVisualStyleBackColor = true;
            this.btn_Statistics.Click += new System.EventHandler(this.btn_Statistics_Click);
            // 
            // btn_Search
            // 
            this.btn_Search.Location = new System.Drawing.Point(500, 550);
            this.btn_Search.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(120, 40);
            this.btn_Search.TabIndex = 6;
            this.btn_Search.Text = "Tìm kiếm";
            this.btn_Search.UseVisualStyleBackColor = true;
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // txt_Search
            // 
            this.txt_Search.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txt_Search.Location = new System.Drawing.Point(200, 555);
            this.txt_Search.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_Search.Name = "txt_Search";
            this.txt_Search.Size = new System.Drawing.Size(280, 38);
            this.txt_Search.TabIndex = 5;
            this.txt_Search.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_Search_KeyPress);
            // 
            // label_Search
            // 
            this.label_Search.AutoSize = true;
            this.label_Search.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label_Search.Location = new System.Drawing.Point(27, 558);
            this.label_Search.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Search.Name = "label_Search";
            this.label_Search.Size = new System.Drawing.Size(150, 31);
            this.label_Search.TabIndex = 4;
            this.label_Search.Text = "Tìm kiếm:";
            // 
            // btn_Delete
            // 
            this.btn_Delete.Location = new System.Drawing.Point(350, 500);
            this.btn_Delete.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(120, 40);
            this.btn_Delete.TabIndex = 3;
            this.btn_Delete.Text = "Xóa tài khoản";
            this.btn_Delete.UseVisualStyleBackColor = true;
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // btn_Edit
            // 
            this.btn_Edit.Location = new System.Drawing.Point(200, 500);
            this.btn_Edit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_Edit.Name = "btn_Edit";
            this.btn_Edit.Size = new System.Drawing.Size(120, 40);
            this.btn_Edit.TabIndex = 2;
            this.btn_Edit.Text = "Chỉnh sửa";
            this.btn_Edit.UseVisualStyleBackColor = true;
            this.btn_Edit.Click += new System.EventHandler(this.btn_Edit_Click);
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.Location = new System.Drawing.Point(50, 500);
            this.btn_Refresh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(120, 40);
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
            this.dataGridView_Accounts.Location = new System.Drawing.Point(27, 38);
            this.dataGridView_Accounts.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView_Accounts.Name = "dataGridView_Accounts";
            this.dataGridView_Accounts.ReadOnly = true;
            this.dataGridView_Accounts.RowHeadersWidth = 62;
            this.dataGridView_Accounts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_Accounts.Size = new System.Drawing.Size(1140, 450);
            this.dataGridView_Accounts.TabIndex = 0;
            // 
            // AccountManageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1250, 650);
            this.Controls.Add(this.groupBox_AccountList);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "AccountManageForm";
            this.Text = "Quản lý tài khoản";
            this.groupBox_AccountList.ResumeLayout(false);
            this.groupBox_AccountList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Accounts)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_AccountList;
        private System.Windows.Forms.Button btn_Backup;
        private System.Windows.Forms.Button btn_Statistics;
        private System.Windows.Forms.Button btn_Search;
        private System.Windows.Forms.TextBox txt_Search;
        private System.Windows.Forms.Label label_Search;
        private System.Windows.Forms.Button btn_Delete;
        private System.Windows.Forms.Button btn_Edit;
        private System.Windows.Forms.Button btn_Refresh;
        private System.Windows.Forms.DataGridView dataGridView_Accounts;
    }
}
