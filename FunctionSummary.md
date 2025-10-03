# Function and Procedure Summary (from final_func.sql, final_proc.sql)

This catalog lists all database objects defined in the final SQL files, where they live, which repository calls them, which forms use them, and their purpose. Objects are grouped by role. At the end, a usage summary shows how many are used vs. unused.

---

## Manager Role (MiniMart_Manager)

### Stored Procedures (mutations)

1) DeleteProduct(@ProductID)
- Nằm trong file nào: `final_proc.sql`
- Gọi trong repo nào: `ProductRepository.DeleteProduct(int productId)`
- Dùng trong Form nào: `Forms/Manager/ProductForm`
- Công dụng là gì: Xóa mềm hoặc xóa sản phẩm theo `ProductID` với kiểm tra an toàn.

2) RestoreProduct(@ProductID)
- Nằm trong file nào: `final_proc.sql`
- Gọi trong repo nào: `ProductRepository.RestoreProduct(int productId)`
- Dùng trong Form nào: `Forms/Manager/ProductRestoreForm`
- Công dụng là gì: Khôi phục sản phẩm đã xóa mềm (`IsDeleted = 1`).

3) AddProduct(@ProductName, @Price, @StockQuantity, @Unit)
- Nằm trong file nào: `final_proc.sql`
- Gọi trong repo nào: `ProductRepository.AddProduct(...)`
- Dùng trong Form nào: `Forms/Manager/ProductForm`, `Forms/Manager/ProductEditForm`
- Công dụng là gì: Thêm sản phẩm mới sau khi validate dữ liệu.

4) UpdateProduct(@ProductID, @ProductName, @Price, @StockQuantity, @Unit)
- Nằm trong file nào: `final_proc.sql`
- Gọi trong repo nào: `ProductRepository.UpdateProduct(...)`
- Dùng trong Form nào: `Forms/Manager/ProductEditForm`, `Forms/Manager/ProductForm`
- Công dụng là gì: Cập nhật thông tin sản phẩm với kiểm tra dữ liệu.

5) AddCustomer(@CustomerName, @Phone, @Address, @LoyaltyPoints)
- Nằm trong file nào: `final_proc.sql`
- Gọi trong repo nào: `CustomerRepository.AddCustomer(...)`
- Dùng trong Form nào: `Forms/Manager/CustomerManageForm`
- Công dụng là gì: Thêm khách hàng mới, trả về `CustomerID` khi thành công.

6) UpdateCustomer(@CustomerID, @CustomerName, @Phone, @Address, @LoyaltyPoints)
- Nằm trong file nào: `final_proc.sql`
- Gọi trong repo nào: `CustomerRepository.UpdateCustomer(...)`
- Dùng trong Form nào: `Forms/Manager/CustomerManageEditForm`
- Công dụng là gì: Cập nhật thông tin khách hàng sau khi kiểm tra ràng buộc.

7) DeleteCustomer(@CustomerID)
- Nằm trong file nào: `final_proc.sql`
- Gọi trong repo nào: `CustomerRepository.DeleteCustomer(int customerId)`
- Dùng trong Form nào: `Forms/Manager/CustomerManageForm`
- Công dụng là gì: Xóa khách hàng nếu không có ràng buộc bán hàng.

8) AddDiscount(@ProductID, @DiscountType, @DiscountValue, @StartDate, @EndDate, @IsActive, @CreatedBy)
- Nằm trong file nào: `final_proc.sql`
- Gọi trong repo nào: `DiscountRepository.AddDiscount(...)`
- Dùng trong Form nào: `Forms/Manager/AdminDiscountEditForm`
- Công dụng là gì: Tạo chương trình giảm giá cho sản phẩm.

9) UpdateDiscount(@DiscountID, @DiscountType, @DiscountValue, @StartDate, @EndDate, @IsActive)
- Nằm trong file nào: `final_proc.sql`
- Gọi trong repo nào: `DiscountRepository.UpdateDiscount(...)`
- Dùng trong Form nào: `Forms/Manager/AdminDiscountEditForm`
- Công dụng là gì: Cập nhật chương trình giảm giá.

10) DeleteDiscount(@DiscountID)
- Nằm trong file nào: `final_proc.sql`
- Gọi trong repo nào: `DiscountRepository.DeleteDiscount(int discountId)`
- Dùng trong Form nào: `Forms/Manager/AdminDiscountForm`
- Công dụng là gì: Xóa chương trình giảm giá.

11) ChangePassword(@Username, @OldPassword, @NewPassword)
- Nằm trong file nào: `final_proc.sql`
- Gọi trong repo nào: `AccountRepository.ChangePassword(...)` và `SecurityHelper.ChangePassword(...)`
- Dùng trong Form nào: `Forms/Common/AccountInfoForm`
- Công dụng là gì: Đổi mật khẩu tài khoản với kiểm tra hợp lệ.

12) UpdateAccount(@Username, @Password, @Role, @CustomerID, @EmployeeID)
- Nằm trong file nào: `final_proc.sql`
- Gọi trong repo nào: `AccountRepository.UpdateAccount(...)`
- Dùng trong Form nào: `Forms/Manager/AccountManageEditForm`
- Công dụng là gì: Cập nhật tài khoản; tự tạo `Employee` nếu cần dựa theo role.

13) DeleteAccount(@Username)
- Nằm trong file nào: `final_proc.sql`
- Gọi trong repo nào: `AccountRepository.DeleteAccount(string username)`
- Dùng trong Form nào: `Forms/Manager/AccountForm`
- Công dụng là gì: Xóa tài khoản nếu không vướng lịch sử giao dịch.

14) CreateSale(@CustomerID, @TotalAmount, @PaymentMethod, @CreatedBy)
- Nằm trong file nào: `final_proc.sql`
- Gọi trong repo nào: `SaleRepository.CreateSaleWithDetails(...)`
- Dùng trong Form nào: `Forms/Saler/SalerInvoiceForm`
- Công dụng là gì: Tạo hóa đơn bán hàng (header).

15) AddSaleDetail(@SaleID, @ProductID, @Quantity, @SalePrice)
- Nằm trong file nào: `final_proc.sql`
- Gọi trong repo nào: `SaleRepository.CreateSaleWithDetails(...)`, `SaleRepository.AddSaleDetail(...)`
- Dùng trong Form nào: `Forms/Saler/SalerInvoiceForm`
- Công dụng là gì: Thêm dòng chi tiết cho hóa đơn.

16) UpdateSale(@SaleID, @CustomerID, @TotalAmount, @PaymentMethod)
- Nằm trong file nào: `final_proc.sql`
- Gọi trong repo nào: `SaleRepository.CreateSaleWithDetails(...)`, `SaleRepository.UpdateSale(...)`
- Dùng trong Form nào: `Forms/Saler/SalerInvoiceForm`
- Công dụng là gì: Cập nhật thông tin hóa đơn sau khi thêm chi tiết.

17) CreateSQLAccount(@Username, @Password, @Role)
- Nằm trong file nào: (định nghĩa ở file khác; vẫn ghi nhận sử dụng)
- Gọi trong repo nào: `AccountRepository.CreateSQLAccount(...)`
- Dùng trong Form nào: `Forms/Manager/AccountCreateForm`
- Công dụng là gì: Tạo SQL Login/User và gán role phù hợp.

18) DeleteSQLAccount(@Username)
- Nằm trong file nào: (định nghĩa ở file khác; vẫn ghi nhận sử dụng)
- Gọi trong repo nào: `AccountRepository.DeleteSQLAccount(string username)`
- Dùng trong Form nào: `Forms/Manager/AccountForm`
- Công dụng là gì: Xóa SQL Login/User tương ứng.

### Table-Valued Functions (reads)

1) fnProducts_All()
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `ProductRepository.GetAllProducts()`
- Dùng trong Form nào: `ProductForm`, `SalerProductForm`, `AdminDiscountForm`, `AdminDiscountEditForm`, `SalerInvoiceForm`
- Công dụng là gì: Danh sách sản phẩm cơ bản.

2) fnProducts_ByID(@ProductID)
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `ProductRepository.GetProductById(int productId)`
- Dùng trong Form nào: `ProductForm`, `ProductEditForm`, `SalerInvoiceForm`
- Công dụng là gì: Lấy sản phẩm theo ID, kèm giá sau giảm.

3) GetProductByName(@Name)
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `ProductRepository.GetProductByName(string name)`
- Dùng trong Form nào: `ProductForm`, `SalerProductForm`
- Công dụng là gì: Tìm sản phẩm theo tên (Vietnamese_CI_AI).

4) fnCustomers_All()
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `CustomerRepository.GetAllCustomers()`
- Dùng trong Form nào: `CustomerManageForm`, `SalerInvoiceForm`
- Công dụng là gì: Danh sách khách hàng.

5) fnCustomers_ByName(@CustomerName)
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `CustomerRepository.GetCustomerByName(string customerName)`
- Dùng trong Form nào: `CustomerManageForm`
- Công dụng là gì: Tìm khách hàng theo tên (Vietnamese_CI_AI).

6) fnCustomers_ByID(@CustomerID)
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `CustomerRepository.GetCustomerById(int id)`
- Dùng trong Form nào: `CustomerManageEditForm`
- Công dụng là gì: Lấy chi tiết khách hàng theo ID.

7) fnSales_ByID(@SaleID)
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `SaleRepository.GetSaleById(int id)`
- Dùng trong Form nào: `SalerInvoiceHistoryForm`
- Công dụng là gì: Lấy thông tin hóa đơn theo ID.

8) fnSaleDetails_BySaleID(@SaleID)
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `SaleRepository.GetSaleDetails(int saleId)`
- Dùng trong Form nào: `SalerInvoiceHistoryForm`
- Công dụng là gì: Lấy chi tiết các dòng của hóa đơn.

9) fnDiscounts_Active()
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `DiscountRepository.GetActiveDiscounts()`
- Dùng trong Form nào: `AdminDiscountForm`
- Công dụng là gì: Danh sách chương trình giảm giá đang hiệu lực.

10) fnDiscounts_ByProduct(@ProductID)
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `DiscountRepository.GetDiscountsByProduct(int productId)`
- Dùng trong Form nào: `AdminDiscountEditForm`
- Công dụng là gì: Liệt kê giảm giá theo sản phẩm.

11) fnAccounts_All()
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `AccountRepository.GetAllAccounts()`
- Dùng trong Form nào: `AccountForm`
- Công dụng là gì: Danh sách tài khoản kèm thông tin nhân viên liên kết.

12) fnAccounts_ByUsername(@Username)
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `AccountRepository.GetAccountByUsername(string username)`
- Dùng trong Form nào: `AccountManageEditForm`, `AccountForm`
- Công dụng là gì: Tra cứu tài khoản theo username.

13) fnAccounts_ByRole(@Role)
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `AccountRepository.GetAccountsByRole(string role)`
- Dùng trong Form nào: `AccountForm`
- Công dụng là gì: Lọc danh sách tài khoản theo vai trò.

14) fnAuth_User(@Username, @Password)
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `AccountRepository.CheckLogin(string u, string p)`
- Dùng trong Form nào: `LoginForm`
- Công dụng là gì: Xác thực người dùng và trả về role.

15) fnReport_TopSellingProducts(@StartDate, @EndDate, @TopN)
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `ReportRepository.GetTopSellingProducts(...)`
- Dùng trong Form nào: `StatisticsForm`
- Công dụng là gì: Báo cáo Top N sản phẩm bán chạy theo thời gian.

16) fnReport_CustomerRanking(@StartDate, @EndDate)
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `ReportRepository.GetCustomerRanking(...)`
- Dùng trong Form nào: `StatisticsForm`
- Công dụng là gì: Xếp hạng khách hàng theo tổng chi tiêu.

17) fnReport_DailyProductSalesTrend(@ProductID, @StartDate, @EndDate)
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `ReportRepository.GetProductSalesTrend(...)`
- Dùng trong Form nào: `StatisticsForm`
- Công dụng là gì: Xu hướng bán hàng theo ngày (có lũy kế và trung bình trượt 7 ngày).

### Scalar Functions

1) ValidatePassword(@Password)
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `SecurityHelper.ValidatePassword(...)`
- Dùng trong Form nào: `Forms/Common/AccountInfoForm` (khi đổi mật khẩu)
- Công dụng là gì: Kiểm tra độ dài mật khẩu hợp lệ (>= 6).

2) GetDailyRevenue(@Date)
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `ReportRepository.GetDailyRevenue(DateTime date)`
- Dùng trong Form nào: `StatisticsForm`
- Công dụng là gì: Tổng doanh thu trong một ngày.

3) GetMonthlyRevenue(@Year, @Month)
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `ReportRepository.GetMonthlyRevenue(int year, int month)`
- Dùng trong Form nào: `StatisticsForm`
- Công dụng là gì: Tổng doanh thu trong một tháng.

4) GetDiscountedPrice(@ProductID, @OriginalPrice)
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `DiscountRepository.GetDiscountedPrice(int productId, decimal originalPrice)`
- Dùng trong Form nào: `SalerProductForm`, `SalerInvoiceForm`
- Công dụng là gì: Tính giá sau giảm dựa trên chương trình giảm giá hiện hành.

5) IsStockAvailable(@ProductID, @RequiredQuantity)
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `ProductRepository.IsStockAvailable(...)`, `SaleRepository` (nội bộ giao dịch)
- Dùng trong Form nào: `SalerInvoiceForm`
- Công dụng là gì: Kiểm tra tồn kho có đủ cho số lượng yêu cầu.

6) GetSalesGrowthRate_Monthly(@CP_Year, @CP_Month, @PP_Year, @PP_Month)
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `ReportRepository.GetSalesGrowthRateMonthly(...)`
- Dùng trong Form nào: `StatisticsForm`
- Công dụng là gì: Tỷ lệ tăng trưởng doanh thu theo tháng; trả về 999.99 nếu tăng trưởng vô hạn.

### Views (tham chiếu sử dụng)

1) LowStockProducts
- Nằm trong file nào: (định nghĩa ở file khác; vẫn ghi nhận sử dụng)
- Gọi trong repo nào: `ProductRepository.GetLowStockProducts()`, `ReportRepository.GetLowStockProducts()`
- Dùng trong Form nào: `StatisticsForm`
- Công dụng là gì: Danh sách sản phẩm sắp hết hàng và trạng thái kho.

2) SalesSummary
- Nằm trong file nào: (định nghĩa ở file khác; vẫn ghi nhận sử dụng)
- Gọi trong repo nào: `SaleRepository.GetSalesSummary()`
- Dùng trong Form nào: `SalerInvoiceHistoryForm`
- Công dụng là gì: Tóm tắt hóa đơn (header + thông tin khách hàng).

---

## Saler Role (MiniMart_Saler)

### Stored Procedures (mutations liên quan tới bán hàng)

1) CreateSale / AddSaleDetail / UpdateSale
- Nằm trong file nào: `final_proc.sql`
- Gọi trong repo nào: `SaleRepository.CreateSaleWithDetails(...)`, `SaleRepository.AddSaleDetail(...)`, `SaleRepository.UpdateSale(...)`
- Dùng trong Form nào: `Forms/Saler/SalerInvoiceForm`
- Công dụng là gì: Tạo hóa đơn và thêm chi tiết; đảm bảo kiểm tra tồn kho và cập nhật tổng tiền.

### Table-Valued Functions (đọc dữ liệu)

1) fnProducts_All, GetProductByName
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `ProductRepository`
- Dùng trong Form nào: `SalerProductForm`, `SalerInvoiceForm`
- Công dụng là gì: Hiển thị/tìm kiếm sản phẩm khi lập hóa đơn.

2) fnCustomers_All, fnCustomers_ByName
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `CustomerRepository`
- Dùng trong Form nào: `SalerInvoiceForm`
- Công dụng là gì: Chọn khách hàng khi lập hóa đơn.

3) fnSales_ByID, fnSaleDetails_BySaleID
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `SaleRepository`
- Dùng trong Form nào: `SalerInvoiceHistoryForm`
- Công dụng là gì: Xem lại hóa đơn và chi tiết.

4) fnDiscounts_Active, GetDiscountedPrice, IsStockAvailable
- Nằm trong file nào: `final_func.sql`
- Gọi trong repo nào: `DiscountRepository`, `ProductRepository`, `SaleRepository`
- Dùng trong Form nào: `SalerProductForm`, `SalerInvoiceForm`
- Công dụng là gì: Tính giá sau giảm và kiểm tra tồn kho khi bán hàng.

---

## Usage Summary (đối tượng trong final_func.sql, final_proc.sql)

- Stored Procedures: 16 dùng (0 chưa dùng)
- Scalar Functions: 6 dùng (0 chưa dùng)
- Table-Valued Functions: 17 dùng (0 chưa dùng)
- Views (tham chiếu): 2 dùng (0 chưa dùng)

Tổng số đối tượng đang dùng: 41. Tổng số đối tượng chưa dùng: 0.
