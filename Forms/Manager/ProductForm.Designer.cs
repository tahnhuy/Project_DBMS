namespace Sale_Management.Forms
{
    partial class ProductForm
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
            this.txt_nameSearch = new System.Windows.Forms.TextBox();
            this.lbl_nameSearch = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_idSearch = new System.Windows.Forms.TextBox();
            this.btn_AddProduct = new System.Windows.Forms.Button();
            this.btn_DeleteProduct = new System.Windows.Forms.Button();
            this.btn_EditProduct = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Products)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_Products
            // 
            this.dgv_Products.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Products.Location = new System.Drawing.Point(21, 206);
            this.dgv_Products.Name = "dgv_Products";
            this.dgv_Products.RowHeadersWidth = 82;
            this.dgv_Products.RowTemplate.Height = 33;
            this.dgv_Products.Size = new System.Drawing.Size(1313, 605);
            this.dgv_Products.TabIndex = 0;
            // 
            // txt_nameSearch
            // 
            this.txt_nameSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_nameSearch.Location = new System.Drawing.Point(300, 49);
            this.txt_nameSearch.Name = "txt_nameSearch";
            this.txt_nameSearch.Size = new System.Drawing.Size(300, 44);
            this.txt_nameSearch.TabIndex = 1;
            this.txt_nameSearch.TextChanged += new System.EventHandler(this.txt_SearchProduct_TextChanged);
            // 
            // lbl_nameSearch
            // 
            this.lbl_nameSearch.AutoSize = true;
            this.lbl_nameSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lbl_nameSearch.Location = new System.Drawing.Point(25, 55);
            this.lbl_nameSearch.Name = "lbl_nameSearch";
            this.lbl_nameSearch.Size = new System.Drawing.Size(257, 31);
            this.lbl_nameSearch.TabIndex = 2;
            this.lbl_nameSearch.Text = "Tìm sản phẩm(Tên):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label1.Location = new System.Drawing.Point(25, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(238, 31);
            this.label1.TabIndex = 3;
            this.label1.Text = "Tìm sản phẩm(ID):";
            // 
            // txt_idSearch
            // 
            this.txt_idSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_idSearch.Location = new System.Drawing.Point(300, 120);
            this.txt_idSearch.Name = "txt_idSearch";
            this.txt_idSearch.Size = new System.Drawing.Size(300, 44);
            this.txt_idSearch.TabIndex = 4;
            this.txt_idSearch.TextChanged += new System.EventHandler(this.txt_idSearch_TextChanged);
            // 
            // btn_AddProduct
            // 
            this.btn_AddProduct.Location = new System.Drawing.Point(684, 67);
            this.btn_AddProduct.Name = "btn_AddProduct";
            this.btn_AddProduct.Size = new System.Drawing.Size(174, 71);
            this.btn_AddProduct.TabIndex = 8;
            this.btn_AddProduct.Text = "Thêm";
            this.btn_AddProduct.UseVisualStyleBackColor = true;
            this.btn_AddProduct.Click += new System.EventHandler(this.btn_AddProduct_Click);
            // 
            // btn_DeleteProduct
            // 
            this.btn_DeleteProduct.Location = new System.Drawing.Point(907, 69);
            this.btn_DeleteProduct.Name = "btn_DeleteProduct";
            this.btn_DeleteProduct.Size = new System.Drawing.Size(174, 71);
            this.btn_DeleteProduct.TabIndex = 9;
            this.btn_DeleteProduct.Text = "Xóa";
            this.btn_DeleteProduct.UseVisualStyleBackColor = true;
            this.btn_DeleteProduct.Click += new System.EventHandler(this.btn_DeleteProduct_Click);
            // 
            // btn_EditProduct
            // 
            this.btn_EditProduct.Location = new System.Drawing.Point(1131, 69);
            this.btn_EditProduct.Name = "btn_EditProduct";
            this.btn_EditProduct.Size = new System.Drawing.Size(174, 71);
            this.btn_EditProduct.TabIndex = 10;
            this.btn_EditProduct.Text = "Sửa";
            this.btn_EditProduct.UseVisualStyleBackColor = true;
            this.btn_EditProduct.UseWaitCursor = true;
            this.btn_EditProduct.Click += new System.EventHandler(this.btn_EditProduct_Click);
            // 
            // ProductForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1346, 823);
            this.Controls.Add(this.btn_EditProduct);
            this.Controls.Add(this.btn_DeleteProduct);
            this.Controls.Add(this.btn_AddProduct);
            this.Controls.Add(this.txt_idSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_nameSearch);
            this.Controls.Add(this.txt_nameSearch);
            this.Controls.Add(this.dgv_Products);
            this.Name = "ProductForm";
            this.Text = "Quản lý sản phẩm";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Products)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_Products;
        private System.Windows.Forms.TextBox txt_nameSearch;
        private System.Windows.Forms.Label lbl_nameSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_idSearch;
        private System.Windows.Forms.Button btn_AddProduct;
        private System.Windows.Forms.Button btn_DeleteProduct;
        private System.Windows.Forms.Button btn_EditProduct;
    }
}