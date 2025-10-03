namespace Sale_Management.Forms.Manager
{
    partial class AccountManageEditForm
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
            this.txt_Username = new System.Windows.Forms.TextBox();
            this.txt_Password = new System.Windows.Forms.TextBox();
            this.txt_ConfirmPassword = new System.Windows.Forms.TextBox();
            this.cmb_Role = new System.Windows.Forms.ComboBox();
            this.btn_Update = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_Clear = new System.Windows.Forms.Button();
            this.chk_ShowPassword = new System.Windows.Forms.CheckBox();
            this.lbl_Username = new System.Windows.Forms.Label();
            this.lbl_Password = new System.Windows.Forms.Label();
            this.lbl_ConfirmPassword = new System.Windows.Forms.Label();
            this.lbl_Role = new System.Windows.Forms.Label();
            this.lbl_FullName = new System.Windows.Forms.Label();
            this.txt_FullName = new System.Windows.Forms.TextBox();
            this.lbl_Phone = new System.Windows.Forms.Label();
            this.txt_Phone = new System.Windows.Forms.TextBox();
            this.lbl_Address = new System.Windows.Forms.Label();
            this.txt_Address = new System.Windows.Forms.TextBox();
            this.lbl_Position = new System.Windows.Forms.Label();
            this.txt_Position = new System.Windows.Forms.TextBox();
            this.lbl_Customer = new System.Windows.Forms.Label();
            this.cmb_Customer = new System.Windows.Forms.ComboBox();
            this.lbl_Employee = new System.Windows.Forms.Label();
            this.cmb_Employee = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txt_Username
            // 
            this.txt_Username.Location = new System.Drawing.Point(300, 58);
            this.txt_Username.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txt_Username.Name = "txt_Username";
            this.txt_Username.Size = new System.Drawing.Size(396, 31);
            this.txt_Username.TabIndex = 0;
            // 
            // txt_Password
            // 
            this.txt_Password.Location = new System.Drawing.Point(300, 346);
            this.txt_Password.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txt_Password.Name = "txt_Password";
            this.txt_Password.PasswordChar = '*';
            this.txt_Password.Size = new System.Drawing.Size(396, 31);
            this.txt_Password.TabIndex = 4;
            this.txt_Password.TextChanged += new System.EventHandler(this.txt_Password_TextChanged);
            // 
            // txt_ConfirmPassword
            // 
            this.txt_ConfirmPassword.Location = new System.Drawing.Point(300, 404);
            this.txt_ConfirmPassword.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txt_ConfirmPassword.Name = "txt_ConfirmPassword";
            this.txt_ConfirmPassword.PasswordChar = '*';
            this.txt_ConfirmPassword.Size = new System.Drawing.Size(396, 31);
            this.txt_ConfirmPassword.TabIndex = 5;
            // 
            // cmb_Role
            // 
            this.cmb_Role.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Role.FormattingEnabled = true;
            this.cmb_Role.Items.AddRange(new object[] {
            "manager",
            "saler",
            "customer"});
            this.cmb_Role.Location = new System.Drawing.Point(300, 462);
            this.cmb_Role.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.cmb_Role.Name = "cmb_Role";
            this.cmb_Role.Size = new System.Drawing.Size(396, 33);
            this.cmb_Role.TabIndex = 6;
            this.cmb_Role.SelectedIndexChanged += new System.EventHandler(this.cmb_Role_SelectedIndexChanged);
            // 
            // btn_Update
            // 
            this.btn_Update.Location = new System.Drawing.Point(100, 673);
            this.btn_Update.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btn_Update.Name = "btn_Update";
            this.btn_Update.Size = new System.Drawing.Size(150, 44);
            this.btn_Update.TabIndex = 7;
            this.btn_Update.Text = "Cập nhật";
            this.btn_Update.UseVisualStyleBackColor = true;
            this.btn_Update.Click += new System.EventHandler(this.btn_Update_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(300, 673);
            this.btn_Cancel.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(150, 44);
            this.btn_Cancel.TabIndex = 8;
            this.btn_Cancel.Text = "Hủy";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_Clear
            // 
            this.btn_Clear.Location = new System.Drawing.Point(500, 673);
            this.btn_Clear.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(150, 44);
            this.btn_Clear.TabIndex = 9;
            this.btn_Clear.Text = "Xóa";
            this.btn_Clear.UseVisualStyleBackColor = true;
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // chk_ShowPassword
            // 
            this.chk_ShowPassword.AutoSize = true;
            this.chk_ShowPassword.Location = new System.Drawing.Point(300, 538);
            this.chk_ShowPassword.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.chk_ShowPassword.Name = "chk_ShowPassword";
            this.chk_ShowPassword.Size = new System.Drawing.Size(211, 29);
            this.chk_ShowPassword.TabIndex = 10;
            this.chk_ShowPassword.Text = "Hiển thị mật khẩu";
            this.chk_ShowPassword.UseVisualStyleBackColor = true;
            this.chk_ShowPassword.CheckedChanged += new System.EventHandler(this.chk_ShowPassword_CheckedChanged);
            // 
            // lbl_Username
            // 
            this.lbl_Username.AutoSize = true;
            this.lbl_Username.Location = new System.Drawing.Point(100, 63);
            this.lbl_Username.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_Username.Name = "lbl_Username";
            this.lbl_Username.Size = new System.Drawing.Size(163, 25);
            this.lbl_Username.TabIndex = 11;
            this.lbl_Username.Text = "Tên đăng nhập:";
            // 
            // lbl_Password
            // 
            this.lbl_Password.AutoSize = true;
            this.lbl_Password.Location = new System.Drawing.Point(100, 352);
            this.lbl_Password.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_Password.Name = "lbl_Password";
            this.lbl_Password.Size = new System.Drawing.Size(147, 25);
            this.lbl_Password.TabIndex = 12;
            this.lbl_Password.Text = "Mật khẩu mới:";
            // 
            // lbl_ConfirmPassword
            // 
            this.lbl_ConfirmPassword.AutoSize = true;
            this.lbl_ConfirmPassword.Location = new System.Drawing.Point(100, 410);
            this.lbl_ConfirmPassword.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_ConfirmPassword.Name = "lbl_ConfirmPassword";
            this.lbl_ConfirmPassword.Size = new System.Drawing.Size(203, 25);
            this.lbl_ConfirmPassword.TabIndex = 13;
            this.lbl_ConfirmPassword.Text = "Xác nhận mật khẩu:";
            // 
            // lbl_Role
            // 
            this.lbl_Role.AutoSize = true;
            this.lbl_Role.Location = new System.Drawing.Point(100, 467);
            this.lbl_Role.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_Role.Name = "lbl_Role";
            this.lbl_Role.Size = new System.Drawing.Size(80, 25);
            this.lbl_Role.TabIndex = 14;
            this.lbl_Role.Text = "Vai trò:";
            // 
            // lbl_FullName
            // 
            this.lbl_FullName.AutoSize = true;
            this.lbl_FullName.Location = new System.Drawing.Point(100, 121);
            this.lbl_FullName.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_FullName.Name = "lbl_FullName";
            this.lbl_FullName.Size = new System.Drawing.Size(81, 25);
            this.lbl_FullName.TabIndex = 15;
            this.lbl_FullName.Text = "Họ tên:";
            // 
            // txt_FullName
            // 
            this.txt_FullName.Location = new System.Drawing.Point(300, 115);
            this.txt_FullName.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txt_FullName.Name = "txt_FullName";
            this.txt_FullName.ReadOnly = true;
            this.txt_FullName.Size = new System.Drawing.Size(396, 31);
            this.txt_FullName.TabIndex = 1;
            // 
            // lbl_Phone
            // 
            this.lbl_Phone.AutoSize = true;
            this.lbl_Phone.Location = new System.Drawing.Point(100, 179);
            this.lbl_Phone.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_Phone.Name = "lbl_Phone";
            this.lbl_Phone.Size = new System.Drawing.Size(144, 25);
            this.lbl_Phone.TabIndex = 16;
            this.lbl_Phone.Text = "Số điện thoại:";
            // 
            // txt_Phone
            // 
            this.txt_Phone.Location = new System.Drawing.Point(300, 173);
            this.txt_Phone.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txt_Phone.Name = "txt_Phone";
            this.txt_Phone.ReadOnly = true;
            this.txt_Phone.Size = new System.Drawing.Size(396, 31);
            this.txt_Phone.TabIndex = 2;
            // 
            // lbl_Address
            // 
            this.lbl_Address.AutoSize = true;
            this.lbl_Address.Location = new System.Drawing.Point(100, 237);
            this.lbl_Address.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_Address.Name = "lbl_Address";
            this.lbl_Address.Size = new System.Drawing.Size(84, 25);
            this.lbl_Address.TabIndex = 17;
            this.lbl_Address.Text = "Địa chỉ:";
            // 
            // txt_Address
            // 
            this.txt_Address.Location = new System.Drawing.Point(300, 231);
            this.txt_Address.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txt_Address.Name = "txt_Address";
            this.txt_Address.ReadOnly = true;
            this.txt_Address.Size = new System.Drawing.Size(396, 31);
            this.txt_Address.TabIndex = 3;
            // 
            // lbl_Position
            // 
            this.lbl_Position.AutoSize = true;
            this.lbl_Position.Location = new System.Drawing.Point(100, 294);
            this.lbl_Position.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_Position.Name = "lbl_Position";
            this.lbl_Position.Size = new System.Drawing.Size(97, 25);
            this.lbl_Position.TabIndex = 18;
            this.lbl_Position.Text = "Chức vụ:";
            // 
            // txt_Position
            // 
            this.txt_Position.Location = new System.Drawing.Point(300, 288);
            this.txt_Position.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txt_Position.Name = "txt_Position";
            this.txt_Position.ReadOnly = true;
            this.txt_Position.Size = new System.Drawing.Size(396, 31);
            this.txt_Position.TabIndex = 4;
            // 
            // lbl_Customer
            // 
            this.lbl_Customer.AutoSize = true;
            this.lbl_Customer.Location = new System.Drawing.Point(100, 596);
            this.lbl_Customer.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_Customer.Name = "lbl_Customer";
            this.lbl_Customer.Size = new System.Drawing.Size(133, 25);
            this.lbl_Customer.TabIndex = 19;
            this.lbl_Customer.Text = "Khách hàng:";
            this.lbl_Customer.Visible = false;
            // 
            // cmb_Customer
            // 
            this.cmb_Customer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Customer.FormattingEnabled = true;
            this.cmb_Customer.Location = new System.Drawing.Point(300, 590);
            this.cmb_Customer.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.cmb_Customer.Name = "cmb_Customer";
            this.cmb_Customer.Size = new System.Drawing.Size(396, 33);
            this.cmb_Customer.TabIndex = 20;
            this.cmb_Customer.Visible = false;
            // 
            // lbl_Employee
            // 
            this.lbl_Employee.AutoSize = true;
            this.lbl_Employee.Location = new System.Drawing.Point(100, 596);
            this.lbl_Employee.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbl_Employee.Name = "lbl_Employee";
            this.lbl_Employee.Size = new System.Drawing.Size(115, 25);
            this.lbl_Employee.TabIndex = 21;
            this.lbl_Employee.Text = "Nhân viên:";
            this.lbl_Employee.Visible = false;
            // 
            // cmb_Employee
            // 
            this.cmb_Employee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Employee.FormattingEnabled = true;
            this.cmb_Employee.Location = new System.Drawing.Point(300, 590);
            this.cmb_Employee.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.cmb_Employee.Name = "cmb_Employee";
            this.cmb_Employee.Size = new System.Drawing.Size(396, 33);
            this.cmb_Employee.TabIndex = 22;
            this.cmb_Employee.Visible = false;
            // 
            // AccountManageEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 769);
            this.Controls.Add(this.cmb_Employee);
            this.Controls.Add(this.lbl_Employee);
            this.Controls.Add(this.cmb_Customer);
            this.Controls.Add(this.lbl_Customer);
            this.Controls.Add(this.txt_Position);
            this.Controls.Add(this.lbl_Position);
            this.Controls.Add(this.txt_Address);
            this.Controls.Add(this.lbl_Address);
            this.Controls.Add(this.txt_Phone);
            this.Controls.Add(this.lbl_Phone);
            this.Controls.Add(this.txt_FullName);
            this.Controls.Add(this.lbl_FullName);
            this.Controls.Add(this.lbl_Role);
            this.Controls.Add(this.lbl_ConfirmPassword);
            this.Controls.Add(this.lbl_Password);
            this.Controls.Add(this.lbl_Username);
            this.Controls.Add(this.chk_ShowPassword);
            this.Controls.Add(this.btn_Clear);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Update);
            this.Controls.Add(this.cmb_Role);
            this.Controls.Add(this.txt_ConfirmPassword);
            this.Controls.Add(this.txt_Password);
            this.Controls.Add(this.txt_Username);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "AccountManageEditForm";
            this.Text = "Chỉnh sửa tài khoản";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_Username;
        private System.Windows.Forms.TextBox txt_Password;
        private System.Windows.Forms.TextBox txt_ConfirmPassword;
        private System.Windows.Forms.ComboBox cmb_Role;
        private System.Windows.Forms.Button btn_Update;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button btn_Clear;
        private System.Windows.Forms.CheckBox chk_ShowPassword;
        private System.Windows.Forms.Label lbl_Username;
        private System.Windows.Forms.Label lbl_Password;
        private System.Windows.Forms.Label lbl_ConfirmPassword;
        private System.Windows.Forms.Label lbl_Role;
        private System.Windows.Forms.Label lbl_FullName;
        private System.Windows.Forms.TextBox txt_FullName;
        private System.Windows.Forms.Label lbl_Phone;
        private System.Windows.Forms.TextBox txt_Phone;
        private System.Windows.Forms.Label lbl_Address;
        private System.Windows.Forms.TextBox txt_Address;
        private System.Windows.Forms.Label lbl_Position;
        private System.Windows.Forms.TextBox txt_Position;
        private System.Windows.Forms.Label lbl_Customer;
        private System.Windows.Forms.ComboBox cmb_Customer;
        private System.Windows.Forms.Label lbl_Employee;
        private System.Windows.Forms.ComboBox cmb_Employee;
    }
}