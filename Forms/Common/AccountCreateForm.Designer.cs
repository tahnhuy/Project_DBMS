namespace Sale_Management.Forms
{
    partial class AccountCreateForm
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
            this.groupBox_CreateAccount = new System.Windows.Forms.GroupBox();
            this.btn_Clear = new System.Windows.Forms.Button();
            this.btn_Create = new System.Windows.Forms.Button();
            this.cmb_Role = new System.Windows.Forms.ComboBox();
            this.txt_ConfirmPassword = new System.Windows.Forms.TextBox();
            this.txt_Password = new System.Windows.Forms.TextBox();
            this.txt_Username = new System.Windows.Forms.TextBox();
            this.label_Role = new System.Windows.Forms.Label();
            this.label_ConfirmPassword = new System.Windows.Forms.Label();
            this.label_Password = new System.Windows.Forms.Label();
            this.label_Username = new System.Windows.Forms.Label();
            this.groupBox_CreateAccount.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_CreateAccount
            // 
            this.groupBox_CreateAccount.Controls.Add(this.btn_Clear);
            this.groupBox_CreateAccount.Controls.Add(this.btn_Create);
            this.groupBox_CreateAccount.Controls.Add(this.cmb_Role);
            this.groupBox_CreateAccount.Controls.Add(this.txt_ConfirmPassword);
            this.groupBox_CreateAccount.Controls.Add(this.txt_Password);
            this.groupBox_CreateAccount.Controls.Add(this.txt_Username);
            this.groupBox_CreateAccount.Controls.Add(this.label_Role);
            this.groupBox_CreateAccount.Controls.Add(this.label_ConfirmPassword);
            this.groupBox_CreateAccount.Controls.Add(this.label_Password);
            this.groupBox_CreateAccount.Controls.Add(this.label_Username);
            this.groupBox_CreateAccount.Location = new System.Drawing.Point(401, 148);
            this.groupBox_CreateAccount.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_CreateAccount.Name = "groupBox_CreateAccount";
            this.groupBox_CreateAccount.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_CreateAccount.Size = new System.Drawing.Size(533, 400);
            this.groupBox_CreateAccount.TabIndex = 0;
            this.groupBox_CreateAccount.TabStop = false;
            this.groupBox_CreateAccount.Text = "Tạo tài khoản mới";
            // 
            // btn_Clear
            // 
            this.btn_Clear.Location = new System.Drawing.Point(304, 336);
            this.btn_Clear.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(161, 55);
            this.btn_Clear.TabIndex = 9;
            this.btn_Clear.Text = "Xóa";
            this.btn_Clear.UseVisualStyleBackColor = true;
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // btn_Create
            // 
            this.btn_Create.Location = new System.Drawing.Point(61, 336);
            this.btn_Create.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Create.Name = "btn_Create";
            this.btn_Create.Size = new System.Drawing.Size(206, 55);
            this.btn_Create.TabIndex = 8;
            this.btn_Create.Text = "Tạo tài khoản";
            this.btn_Create.UseVisualStyleBackColor = true;
            this.btn_Create.Click += new System.EventHandler(this.btn_Create_Click);
            // 
            // cmb_Role
            // 
            this.cmb_Role.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Role.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cmb_Role.FormattingEnabled = true;
            this.cmb_Role.Items.AddRange(new object[] {
            "manager",
            "saler",
            "customer"});
            this.cmb_Role.Location = new System.Drawing.Point(231, 247);
            this.cmb_Role.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_Role.Name = "cmb_Role";
            this.cmb_Role.Size = new System.Drawing.Size(265, 39);
            this.cmb_Role.TabIndex = 7;
            // 
            // txt_ConfirmPassword
            // 
            this.txt_ConfirmPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txt_ConfirmPassword.Location = new System.Drawing.Point(231, 185);
            this.txt_ConfirmPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txt_ConfirmPassword.Name = "txt_ConfirmPassword";
            this.txt_ConfirmPassword.PasswordChar = '*';
            this.txt_ConfirmPassword.Size = new System.Drawing.Size(265, 38);
            this.txt_ConfirmPassword.TabIndex = 6;
            // 
            // txt_Password
            // 
            this.txt_Password.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txt_Password.Location = new System.Drawing.Point(231, 124);
            this.txt_Password.Margin = new System.Windows.Forms.Padding(4);
            this.txt_Password.Name = "txt_Password";
            this.txt_Password.PasswordChar = '*';
            this.txt_Password.Size = new System.Drawing.Size(265, 38);
            this.txt_Password.TabIndex = 5;
            // 
            // txt_Username
            // 
            this.txt_Username.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txt_Username.Location = new System.Drawing.Point(231, 65);
            this.txt_Username.Margin = new System.Windows.Forms.Padding(4);
            this.txt_Username.Name = "txt_Username";
            this.txt_Username.Size = new System.Drawing.Size(265, 38);
            this.txt_Username.TabIndex = 4;
            // 
            // label_Role
            // 
            this.label_Role.AutoSize = true;
            this.label_Role.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label_Role.Location = new System.Drawing.Point(100, 250);
            this.label_Role.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Role.Name = "label_Role";
            this.label_Role.Size = new System.Drawing.Size(100, 31);
            this.label_Role.TabIndex = 3;
            this.label_Role.Text = "Vai trò:";
            // 
            // label_ConfirmPassword
            // 
            this.label_ConfirmPassword.AutoSize = true;
            this.label_ConfirmPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label_ConfirmPassword.Location = new System.Drawing.Point(17, 185);
            this.label_ConfirmPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_ConfirmPassword.Name = "label_ConfirmPassword";
            this.label_ConfirmPassword.Size = new System.Drawing.Size(183, 31);
            this.label_ConfirmPassword.TabIndex = 2;
            this.label_ConfirmPassword.Text = "Xác nhận MK:";
            // 
            // label_Password
            // 
            this.label_Password.AutoSize = true;
            this.label_Password.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label_Password.Location = new System.Drawing.Point(67, 124);
            this.label_Password.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Password.Name = "label_Password";
            this.label_Password.Size = new System.Drawing.Size(133, 31);
            this.label_Password.TabIndex = 1;
            this.label_Password.Text = "Mật khẩu:";
            // 
            // label_Username
            // 
            this.label_Username.AutoSize = true;
            this.label_Username.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label_Username.Location = new System.Drawing.Point(-3, 65);
            this.label_Username.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Username.Name = "label_Username";
            this.label_Username.Size = new System.Drawing.Size(203, 31);
            this.label_Username.TabIndex = 0;
            this.label_Username.Text = "Tên đăng nhập:";
            // 
            // AccountCreateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1381, 706);
            this.Controls.Add(this.groupBox_CreateAccount);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AccountCreateForm";
            this.Text = "Tạo tài khoản";
            this.groupBox_CreateAccount.ResumeLayout(false);
            this.groupBox_CreateAccount.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_CreateAccount;
        private System.Windows.Forms.Button btn_Clear;
        private System.Windows.Forms.Button btn_Create;
        private System.Windows.Forms.ComboBox cmb_Role;
        private System.Windows.Forms.TextBox txt_ConfirmPassword;
        private System.Windows.Forms.TextBox txt_Password;
        private System.Windows.Forms.TextBox txt_Username;
        private System.Windows.Forms.Label label_Role;
        private System.Windows.Forms.Label label_ConfirmPassword;
        private System.Windows.Forms.Label label_Password;
        private System.Windows.Forms.Label label_Username;
    }
}