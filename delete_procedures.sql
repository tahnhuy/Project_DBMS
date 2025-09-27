-- =============================================
-- Stored Procedures for Delete Operations
-- =============================================

-- 1. Delete Product
-- =============================================
CREATE OR ALTER PROCEDURE DeleteProduct
    @ProductID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check if product exists
        IF NOT EXISTS (SELECT 1 FROM Products WHERE ProductID = @ProductID)
        BEGIN
            SELECT 'ERROR' AS Result, 'Sản phẩm không tồn tại.' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Check if product has been used in sales
        IF EXISTS (SELECT 1 FROM SaleDetails WHERE ProductID = @ProductID)
        BEGIN
            SELECT 'ERROR' AS Result, 'Không thể xóa sản phẩm đã có trong hóa đơn bán hàng.' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Check if product has active discounts
        IF EXISTS (SELECT 1 FROM Discounts WHERE ProductID = @ProductID AND EndDate >= GETDATE())
        BEGIN
            SELECT 'ERROR' AS Result, 'Không thể xóa sản phẩm đang có chương trình giảm giá hoạt động.' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Delete product
        DELETE FROM Products WHERE ProductID = @ProductID;
        
        IF @@ROWCOUNT > 0
        BEGIN
            SELECT 'SUCCESS' AS Result, 'Xóa sản phẩm thành công.' AS Message;
        END
        ELSE
        BEGIN
            SELECT 'ERROR' AS Result, 'Không thể xóa sản phẩm.' AS Message;
        END
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, 'Lỗi khi xóa sản phẩm: ' + ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- 2. Delete Customer
-- =============================================
CREATE OR ALTER PROCEDURE DeleteCustomer
    @CustomerID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check if customer exists
        IF NOT EXISTS (SELECT 1 FROM Customers WHERE CustomerID = @CustomerID)
        BEGIN
            SELECT 'ERROR' AS Result, 'Khách hàng không tồn tại.' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Check if customer has sales history
        IF EXISTS (SELECT 1 FROM Sales WHERE CustomerID = @CustomerID)
        BEGIN
            SELECT 'ERROR' AS Result, 'Không thể xóa khách hàng đã có lịch sử mua hàng.' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Check if customer has account
        IF EXISTS (SELECT 1 FROM Account WHERE CustomerID = @CustomerID)
        BEGIN
            SELECT 'ERROR' AS Result, 'Không thể xóa khách hàng đã có tài khoản đăng nhập.' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Delete customer
        DELETE FROM Customers WHERE CustomerID = @CustomerID;
        
        IF @@ROWCOUNT > 0
        BEGIN
            SELECT 'SUCCESS' AS Result, 'Xóa khách hàng thành công.' AS Message;
        END
        ELSE
        BEGIN
            SELECT 'ERROR' AS Result, 'Không thể xóa khách hàng.' AS Message;
        END
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, 'Lỗi khi xóa khách hàng: ' + ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- 3. Delete Discount
-- =============================================
CREATE OR ALTER PROCEDURE DeleteDiscount
    @DiscountID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check if discount exists
        IF NOT EXISTS (SELECT 1 FROM Discounts WHERE DiscountID = @DiscountID)
        BEGIN
            SELECT 'ERROR' AS Result, 'Chương trình giảm giá không tồn tại.' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Check if discount is currently active
        IF EXISTS (SELECT 1 FROM Discounts WHERE DiscountID = @DiscountID AND StartDate <= GETDATE() AND EndDate >= GETDATE())
        BEGIN
            SELECT 'ERROR' AS Result, 'Không thể xóa chương trình giảm giá đang hoạt động.' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Delete discount
        DELETE FROM Discounts WHERE DiscountID = @DiscountID;
        
        IF @@ROWCOUNT > 0
        BEGIN
            SELECT 'SUCCESS' AS Result, 'Xóa chương trình giảm giá thành công.' AS Message;
        END
        ELSE
        BEGIN
            SELECT 'ERROR' AS Result, 'Không thể xóa chương trình giảm giá.' AS Message;
        END
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, 'Lỗi khi xóa chương trình giảm giá: ' + ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- 4. Delete Sale (Optional - for admin purposes)
-- =============================================
CREATE OR ALTER PROCEDURE DeleteSale
    @SaleID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check if sale exists
        IF NOT EXISTS (SELECT 1 FROM Sales WHERE SaleID = @SaleID)
        BEGIN
            SELECT 'ERROR' AS Result, 'Hóa đơn không tồn tại.' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Check if sale is recent (within 24 hours)
        IF EXISTS (SELECT 1 FROM Sales WHERE SaleID = @SaleID AND SaleDate < DATEADD(HOUR, -24, GETDATE()))
        BEGIN
            SELECT 'ERROR' AS Result, 'Chỉ có thể xóa hóa đơn trong vòng 24 giờ.' AS Message;
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Delete sale details first
        DELETE FROM SaleDetails WHERE SaleID = @SaleID;
        
        -- Delete sale
        DELETE FROM Sales WHERE SaleID = @SaleID;
        
        IF @@ROWCOUNT > 0
        BEGIN
            SELECT 'SUCCESS' AS Result, 'Xóa hóa đơn thành công.' AS Message;
        END
        ELSE
        BEGIN
            SELECT 'ERROR' AS Result, 'Không thể xóa hóa đơn.' AS Message;
        END
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SELECT 'ERROR' AS Result, 'Lỗi khi xóa hóa đơn: ' + ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

PRINT 'Đã tạo thành công các stored procedures xóa:'
PRINT '- DeleteProduct'
PRINT '- DeleteCustomer' 
PRINT '- DeleteDiscount'
PRINT '- DeleteSale'
