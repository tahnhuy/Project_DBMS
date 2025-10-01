namespace Sale_Management.Forms.Manager
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
            this.dgv_Accounts = new System.Windows.Forms.DataGridView();
            this.txt_SearchUsername = new System.Windows.Forms.TextBox();
            this.txt_SearchName = new System.Windows.Forms.TextBox();
            this.cmb_SearchRole = new System.Windows.Forms.ComboBox();
            this.btn_Add = new System.Windows.Forms.Button();
            this.btn_Edit = new System.Windows.Forms.Button();
            this.btn_Delete = new System.Windows.Forms.Button();
            this.btn_Refresh = new System.Windows.Forms.Button();
            this.lbl_SearchUsername = new System.Windows.Forms.Label();
            this.lbl_SearchName = new System.Windows.Forms.Label();
            this.lbl_SearchRole = new System.Windows.Forms.Label();
            this.grp_Search = new System.Windows.Forms.GroupBox();
            this.grp_Actions = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Accounts)).BeginInit();
            this.grp_Search.SuspendLayout();
            this.grp_Actions.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv_Accounts
            // 
            this.dgv_Accounts.AllowUserToAddRows = false;
            this.dgv_Accounts.AllowUserToDeleteRows = false;
            this.dgv_Accounts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_Accounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Accounts.Location = new System.Drawing.Point(24, 231);
            this.dgv_Accounts.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.dgv_Accounts.MultiSelect = false;
            this.dgv_Accounts.Name = "dgv_Accounts";
            this.dgv_Accounts.ReadOnly = true;
            this.dgv_Accounts.RowHeadersWidth = 82;
            this.dgv_Accounts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_Accounts.Size = new System.Drawing.Size(1552, 577);
            this.dgv_Accounts.TabIndex = 0;
            // 
            // txt_SearchUsername
            // 
            this.txt_SearchUsername.Location = new System.Drawing.Point(200, 48);
            this.txt_SearchUsername.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txt_SearchUsername.Name = "txt_SearchUsername";
            this.txt_SearchUsername.Size = new System.Drawing.Size(296, 31);
            this.txt_SearchUsername.TabIndex = 1;
            this.txt_SearchUsername.TextChanged += new System.EventHandler(this.txt_SearchUsername_TextChanged);
            // 
            // txt_SearchName
            // 
            this.txt_SearchName.Location = new System.Drawing.Point(200, 106);
            this.txt_SearchName.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txt_SearchName.Name = "txt_SearchName";
            this.txt_SearchName.Size = new System.Drawing.Size(296, 31);
            this.txt_SearchName.TabIndex = 2;
            this.txt_SearchName.TextChanged += new System.EventHandler(this.txt_SearchName_TextChanged);
            // 
            // cmb_SearchRole
            // 
            this.cmb_SearchRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_SearchRole.FormattingEnabled = true;
            this.cmb_SearchRole.Items.AddRange(new object[] {
            "Tất cả",
            "manager",
            "saler",
            "customer"});
            this.cmb_SearchRole.Location = new System.Drawing.Point(200, 163);
            this.cmb_SearchRole.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.cmb_SearchRole.Name = "cmb_SearchRole";
            this.cmb_SearchRole.Size = new System.Drawing.Size(296, 33);
            this.cmb_SearchRole.TabIndex = 3;
            this.cmb_SearchRole.SelectedIndexChanged += new System.EventHandler(this.cmb_SearchRole_SelectedIndexChanged);
            // 
            // btn_Add
            // 
            this.btn_Add.Location = new System.Drawing.Point(30, 48);
            this.btn_Add.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(150, 44);
            this.btn_Add.TabIndex = 4;
            this.btn_Add.Text = "Thêm";
            this.btn_Add.UseVisualStyleBackColor = true;
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // btn_Edit
            // 
            this.btn_Edit.Location = new System.Drawing.Point(210, 48);
            this.btn_Edit.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btn_Edit.Name = "btn_Edit";
            this.btn_Edit.Size = new System.Drawing.Size(150, 44);
            this.btn_Edit.TabIndex = 5;
            this.btn_Edit.Text = "Sửa";
            this.btn_Edit.UseVisualStyleBackColor = true;
            this.btn_Edit.Click += new System.EventHandler(this.btn_Edit_Click);
            // 
            // btn_Delete
            // 
            this.btn_Delete.Location = new System.Drawing.Point(390, 48);
            this.btn_Delete.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(150, 44);
            this.btn_Delete.TabIndex = 6;
            this.btn_Delete.Text = "Xóa";
            this.btn_Delete.UseVisualStyleBackColor = true;
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.Location = new System.Drawing.Point(570, 48);
            this.btn_Refresh.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(150, 44);
            this.btn_Refresh.TabIndex = 7;
            this.btn_Refresh.Text = "Làm mới";
            this.btn_Refresh.UseVisualStyleBackColor = true;
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // lbl_SearchUsername
            // 
            this.lbl_SearchUsername.AutoSize = true;
            this.lbl_SearchUsername.Location = new System.Drawing.Point(30, 54);
            this.lbl_SearchUsername.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_SearchUsername.Name = "lbl_SearchUsername";
            this.lbl_SearchUsername.Size = new System.Drawing.Size(163, 25);
            this.lbl_SearchUsername.TabIndex = 9;
            this.lbl_SearchUsername.Text = "Tên đăng nhập:";
            // 
            // lbl_SearchName
            // 
            this.lbl_SearchName.AutoSize = true;
            this.lbl_SearchName.Location = new System.Drawing.Point(30, 112);
            this.lbl_SearchName.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_SearchName.Name = "lbl_SearchName";
            this.lbl_SearchName.Size = new System.Drawing.Size(81, 25);
            this.lbl_SearchName.TabIndex = 10;
            this.lbl_SearchName.Text = "Họ tên:";
            // 
            // lbl_SearchRole
            // 
            this.lbl_SearchRole.AutoSize = true;
            this.lbl_SearchRole.Location = new System.Drawing.Point(30, 169);
            this.lbl_SearchRole.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_SearchRole.Name = "lbl_SearchRole";
            this.lbl_SearchRole.Size = new System.Drawing.Size(80, 25);
            this.lbl_SearchRole.TabIndex = 11;
            this.lbl_SearchRole.Text = "Vai trò:";
            // 
            // grp_Search
            // 
            this.grp_Search.Controls.Add(this.lbl_SearchRole);
            this.grp_Search.Controls.Add(this.lbl_SearchName);
            this.grp_Search.Controls.Add(this.lbl_SearchUsername);
            this.grp_Search.Controls.Add(this.cmb_SearchRole);
            this.grp_Search.Controls.Add(this.txt_SearchName);
            this.grp_Search.Controls.Add(this.txt_SearchUsername);
            this.grp_Search.Location = new System.Drawing.Point(24, 23);
            this.grp_Search.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.grp_Search.Name = "grp_Search";
            this.grp_Search.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.grp_Search.Size = new System.Drawing.Size(600, 205);
            this.grp_Search.TabIndex = 12;
            this.grp_Search.TabStop = false;
            this.grp_Search.Text = "Tìm kiếm";
            // 
            // grp_Actions
            // 
            this.grp_Actions.Controls.Add(this.btn_Refresh);
            this.grp_Actions.Controls.Add(this.btn_Delete);
            this.grp_Actions.Controls.Add(this.btn_Edit);
            this.grp_Actions.Controls.Add(this.btn_Add);
            this.grp_Actions.Location = new System.Drawing.Point(636, 23);
            this.grp_Actions.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.grp_Actions.Name = "grp_Actions";
            this.grp_Actions.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.grp_Actions.Size = new System.Drawing.Size(1000, 115);
            this.grp_Actions.TabIndex = 13;
            this.grp_Actions.TabStop = false;
            this.grp_Actions.Text = "Thao tác";
            // 
            // AccountForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1600, 865);
            this.Controls.Add(this.grp_Actions);
            this.Controls.Add(this.grp_Search);
            this.Controls.Add(this.dgv_Accounts);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "AccountForm";
            this.Text = "Quản lý tài khoản";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Accounts)).EndInit();
            this.grp_Search.ResumeLayout(false);
            this.grp_Search.PerformLayout();
            this.grp_Actions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_Accounts;
        private System.Windows.Forms.TextBox txt_SearchUsername;
        private System.Windows.Forms.TextBox txt_SearchName;
        private System.Windows.Forms.ComboBox cmb_SearchRole;
        private System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.Button btn_Edit;
        private System.Windows.Forms.Button btn_Delete;
        private System.Windows.Forms.Button btn_Refresh;
        private System.Windows.Forms.Label lbl_SearchUsername;
        private System.Windows.Forms.Label lbl_SearchName;
        private System.Windows.Forms.Label lbl_SearchRole;
        private System.Windows.Forms.GroupBox grp_Search;
        private System.Windows.Forms.GroupBox grp_Actions;
    }
}