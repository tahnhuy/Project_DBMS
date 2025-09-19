namespace Sale_Management.Forms
{
    partial class ProductEditForm
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
            this.lbl_ProductName = new System.Windows.Forms.Label();
            this.txt_ProductName = new System.Windows.Forms.TextBox();
            this.lbl_Price = new System.Windows.Forms.Label();
            this.txt_Price = new System.Windows.Forms.TextBox();
            this.lbl_StockQuantity = new System.Windows.Forms.Label();
            this.txt_StockQuantity = new System.Windows.Forms.TextBox();
            this.lbl_Unit = new System.Windows.Forms.Label();
            this.txt_Unit = new System.Windows.Forms.TextBox();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_ProductName
            // 
            this.lbl_ProductName.AutoSize = true;
            this.lbl_ProductName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lbl_ProductName.Location = new System.Drawing.Point(30, 30);
            this.lbl_ProductName.Name = "lbl_ProductName";
            this.lbl_ProductName.Size = new System.Drawing.Size(194, 31);
            this.lbl_ProductName.TabIndex = 0;
            this.lbl_ProductName.Text = "Tên sản phẩm:";
            // 
            // txt_ProductName
            // 
            this.txt_ProductName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txt_ProductName.Location = new System.Drawing.Point(242, 27);
            this.txt_ProductName.Name = "txt_ProductName";
            this.txt_ProductName.Size = new System.Drawing.Size(300, 38);
            this.txt_ProductName.TabIndex = 1;
            // 
            // lbl_Price
            // 
            this.lbl_Price.AutoSize = true;
            this.lbl_Price.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lbl_Price.Location = new System.Drawing.Point(30, 80);
            this.lbl_Price.Name = "lbl_Price";
            this.lbl_Price.Size = new System.Drawing.Size(64, 31);
            this.lbl_Price.TabIndex = 2;
            this.lbl_Price.Text = "Giá:";
            // 
            // txt_Price
            // 
            this.txt_Price.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txt_Price.Location = new System.Drawing.Point(242, 77);
            this.txt_Price.Name = "txt_Price";
            this.txt_Price.Size = new System.Drawing.Size(300, 38);
            this.txt_Price.TabIndex = 3;
            this.txt_Price.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_Price_KeyPress);
            // 
            // lbl_StockQuantity
            // 
            this.lbl_StockQuantity.AutoSize = true;
            this.lbl_StockQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lbl_StockQuantity.Location = new System.Drawing.Point(30, 130);
            this.lbl_StockQuantity.Name = "lbl_StockQuantity";
            this.lbl_StockQuantity.Size = new System.Drawing.Size(173, 31);
            this.lbl_StockQuantity.TabIndex = 4;
            this.lbl_StockQuantity.Text = "Số lượng tồn:";
            // 
            // txt_StockQuantity
            // 
            this.txt_StockQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txt_StockQuantity.Location = new System.Drawing.Point(242, 130);
            this.txt_StockQuantity.Name = "txt_StockQuantity";
            this.txt_StockQuantity.Size = new System.Drawing.Size(300, 38);
            this.txt_StockQuantity.TabIndex = 5;
            this.txt_StockQuantity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_StockQuantity_KeyPress);
            // 
            // lbl_Unit
            // 
            this.lbl_Unit.AutoSize = true;
            this.lbl_Unit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lbl_Unit.Location = new System.Drawing.Point(30, 180);
            this.lbl_Unit.Name = "lbl_Unit";
            this.lbl_Unit.Size = new System.Drawing.Size(99, 31);
            this.lbl_Unit.TabIndex = 6;
            this.lbl_Unit.Text = "Đơn vị:";
            // 
            // txt_Unit
            // 
            this.txt_Unit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txt_Unit.Location = new System.Drawing.Point(242, 180);
            this.txt_Unit.Name = "txt_Unit";
            this.txt_Unit.Size = new System.Drawing.Size(300, 38);
            this.txt_Unit.TabIndex = 7;
            // 
            // btn_Save
            // 
            this.btn_Save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btn_Save.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btn_Save.ForeColor = System.Drawing.Color.White;
            this.btn_Save.Location = new System.Drawing.Point(242, 240);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(120, 50);
            this.btn_Save.TabIndex = 8;
            this.btn_Save.Text = "Lưu";
            this.btn_Save.UseVisualStyleBackColor = false;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btn_Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btn_Cancel.ForeColor = System.Drawing.Color.Black;
            this.btn_Cancel.Location = new System.Drawing.Point(422, 240);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(120, 50);
            this.btn_Cancel.TabIndex = 9;
            this.btn_Cancel.Text = "Hủy";
            this.btn_Cancel.UseVisualStyleBackColor = false;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // ProductEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 363);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.txt_Unit);
            this.Controls.Add(this.lbl_Unit);
            this.Controls.Add(this.txt_StockQuantity);
            this.Controls.Add(this.lbl_StockQuantity);
            this.Controls.Add(this.txt_Price);
            this.Controls.Add(this.lbl_Price);
            this.Controls.Add(this.txt_ProductName);
            this.Controls.Add(this.lbl_ProductName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProductEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thêm/Sửa sản phẩm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_ProductName;
        private System.Windows.Forms.TextBox txt_ProductName;
        private System.Windows.Forms.Label lbl_Price;
        private System.Windows.Forms.TextBox txt_Price;
        private System.Windows.Forms.Label lbl_StockQuantity;
        private System.Windows.Forms.TextBox txt_StockQuantity;
        private System.Windows.Forms.Label lbl_Unit;
        private System.Windows.Forms.TextBox txt_Unit;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button btn_Cancel;
    }
}
