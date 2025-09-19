namespace Sale_Management.Forms
{
    partial class CustomerManageEditForm
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
			this.lblName = new System.Windows.Forms.Label();
			this.lblPhone = new System.Windows.Forms.Label();
			this.lblAddress = new System.Windows.Forms.Label();
			this.lblPoints = new System.Windows.Forms.Label();
			this.txt_CustomerName = new System.Windows.Forms.TextBox();
			this.txt_Phone = new System.Windows.Forms.TextBox();
			this.txt_Address = new System.Windows.Forms.TextBox();
			this.txt_LoyaltyPoints = new System.Windows.Forms.TextBox();
			this.btn_Save = new System.Windows.Forms.Button();
			this.btn_Cancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblName
			// 
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(30, 30);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(78, 13);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "Họ và tên:";
			// 
			// lblPhone
			// 
			this.lblPhone.AutoSize = true;
			this.lblPhone.Location = new System.Drawing.Point(30, 70);
			this.lblPhone.Name = "lblPhone";
			this.lblPhone.Size = new System.Drawing.Size(76, 13);
			this.lblPhone.TabIndex = 1;
			this.lblPhone.Text = "Số điện thoại:";
			// 
			// lblAddress
			// 
			this.lblAddress.AutoSize = true;
			this.lblAddress.Location = new System.Drawing.Point(30, 110);
			this.lblAddress.Name = "lblAddress";
			this.lblAddress.Size = new System.Drawing.Size(47, 13);
			this.lblAddress.TabIndex = 2;
			this.lblAddress.Text = "Địa chỉ:";
			// 
			// lblPoints
			// 
			this.lblPoints.AutoSize = true;
			this.lblPoints.Location = new System.Drawing.Point(30, 220);
			this.lblPoints.Name = "lblPoints";
			this.lblPoints.Size = new System.Drawing.Size(78, 13);
			this.lblPoints.TabIndex = 3;
			this.lblPoints.Text = "Điểm tích lũy:";
			// 
			// txt_CustomerName
			// 
			this.txt_CustomerName.Location = new System.Drawing.Point(160, 27);
			this.txt_CustomerName.Name = "txt_CustomerName";
			this.txt_CustomerName.Size = new System.Drawing.Size(320, 20);
			this.txt_CustomerName.TabIndex = 4;
			// 
			// txt_Phone
			// 
			this.txt_Phone.Location = new System.Drawing.Point(160, 67);
			this.txt_Phone.Name = "txt_Phone";
			this.txt_Phone.Size = new System.Drawing.Size(320, 20);
			this.txt_Phone.TabIndex = 5;
			// 
			// txt_Address
			// 
			this.txt_Address.Location = new System.Drawing.Point(160, 107);
			this.txt_Address.Multiline = true;
			this.txt_Address.Name = "txt_Address";
			this.txt_Address.Size = new System.Drawing.Size(320, 90);
			this.txt_Address.TabIndex = 6;
			// 
			// txt_LoyaltyPoints
			// 
			this.txt_LoyaltyPoints.Location = new System.Drawing.Point(160, 217);
			this.txt_LoyaltyPoints.Name = "txt_LoyaltyPoints";
			this.txt_LoyaltyPoints.Size = new System.Drawing.Size(120, 20);
			this.txt_LoyaltyPoints.TabIndex = 7;
			this.txt_LoyaltyPoints.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_LoyaltyPoints_KeyPress);
			// 
			// btn_Save
			// 
			this.btn_Save.Location = new System.Drawing.Point(160, 260);
			this.btn_Save.Name = "btn_Save";
			this.btn_Save.Size = new System.Drawing.Size(150, 35);
			this.btn_Save.TabIndex = 8;
			this.btn_Save.Text = "Lưu";
			this.btn_Save.UseVisualStyleBackColor = true;
			this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
			// 
			// btn_Cancel
			// 
			this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btn_Cancel.Location = new System.Drawing.Point(330, 260);
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.Size = new System.Drawing.Size(150, 35);
			this.btn_Cancel.TabIndex = 9;
			this.btn_Cancel.Text = "Hủy";
			this.btn_Cancel.UseVisualStyleBackColor = true;
			this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
			// 
			// CustomerEditForm
			// 
			this.AcceptButton = this.btn_Save;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btn_Cancel;
			this.ClientSize = new System.Drawing.Size(520, 320);
			this.Controls.Add(this.btn_Cancel);
			this.Controls.Add(this.btn_Save);
			this.Controls.Add(this.txt_LoyaltyPoints);
			this.Controls.Add(this.txt_Address);
			this.Controls.Add(this.txt_Phone);
			this.Controls.Add(this.txt_CustomerName);
			this.Controls.Add(this.lblPoints);
			this.Controls.Add(this.lblAddress);
			this.Controls.Add(this.lblPhone);
			this.Controls.Add(this.lblName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CustomerEditForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Khách hàng";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

        #endregion
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblPhone;
		private System.Windows.Forms.Label lblAddress;
		private System.Windows.Forms.Label lblPoints;
		private System.Windows.Forms.TextBox txt_CustomerName;
		private System.Windows.Forms.TextBox txt_Phone;
		private System.Windows.Forms.TextBox txt_Address;
		private System.Windows.Forms.TextBox txt_LoyaltyPoints;
		private System.Windows.Forms.Button btn_Save;
		private System.Windows.Forms.Button btn_Cancel;
	}
}