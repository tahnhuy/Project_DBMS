-- =============================================
-- NEW TRIGGERS - Essential Triggers Only
-- Based on FunctionSummary.md analysis
-- Total: 2 Essential Triggers
-- =============================================

USE Minimart_SalesDB;
GO

-- ========== ESSENTIAL TRIGGERS (2 triggers) ==========

-- 1. TR_SaleDetails_UpdateStock - Update stock when sale details are added
create or alter trigger TR_SaleDetails_UpdateStock
on dbo.SaleDetails
after insert, update, delete
as
begin
    set nocount on;
    
    -- Kiểm tra khi INSERT hoặc UPDATE
    if exists(select * from inserted)
    begin
        -- Kiểm tra số lượng tồn kho đủ không
        if exists(
            select 1 
            from inserted i
            inner join dbo.Products p on i.ProductID = p.ProductID
            where p.StockQuantity < i.Quantity
        )
        begin
            raiserror(N'Số lượng tồn kho không đủ để bán!', 16, 1);
            rollback transaction;
            return;
        end
        
        -- Trừ số lượng tồn kho cho sản phẩm mới bán
        update p
        set StockQuantity = p.StockQuantity - i.Quantity
        from dbo.Products p
        inner join inserted i on p.ProductID = i.ProductID;
        
        -- Nếu là UPDATE, cần cộng lại số lượng cũ
        if exists(select * from deleted)
        begin
            update p
            set StockQuantity = p.StockQuantity + d.Quantity
            from dbo.Products p
            inner join deleted d on p.ProductID = d.ProductID;
        end
    end
    
    -- Xử lý khi DELETE (hoàn trả số lượng)
    if exists(select * from deleted) and not exists(select * from inserted)
    begin
        update p
        set StockQuantity = p.StockQuantity + d.Quantity
        from dbo.Products p
        inner join deleted d on p.ProductID = d.ProductID;
    end
end
go

-- 2. TR_Sales_UpdateLoyaltyPoints - Update customer loyalty points when sale is completed
create or alter trigger TR_Sales_UpdateLoyaltyPoints
on dbo.Sales
after insert, update
as
begin
    set nocount on;
    
    -- Chỉ cập nhật điểm cho khách hàng có CustomerID
    update c
    set LoyaltyPoints = c.LoyaltyPoints + cast(i.TotalAmount / 10000 as int)
    from dbo.Customers c
    inner join inserted i on c.CustomerID = i.CustomerID
    where i.CustomerID is not null
      and i.TotalAmount > 0;
      
    -- Trừ điểm nếu là cập nhật và có giá trị cũ
    if exists(select * from deleted)
    begin
        update c
        set LoyaltyPoints = c.LoyaltyPoints - cast(d.TotalAmount / 10000 as int)
        from dbo.Customers c
        inner join deleted d on c.CustomerID = d.CustomerID
        where d.CustomerID is not null
          and d.TotalAmount > 0
          and c.LoyaltyPoints >= cast(d.TotalAmount / 10000 as int);
    end
end
go


-- ========== TRIGGER 3: Kiểm tra ràng buộc giảm giá ==========
-- Trigger này kiểm tra tính hợp lệ của chương trình giảm giá
create or alter trigger TR_Discounts_ValidateDiscount
on dbo.Discounts
after insert, update
as
begin
    set nocount on;
    
    -- Kiểm tra giá trị giảm giá hợp lệ
    if exists(
        select 1 from inserted 
        where (DiscountType = 'percentage' and DiscountValue > 100)
           or (DiscountType = 'fixed' and DiscountValue < 0)
           or (DiscountValue <= 0)
    )
    begin
        raiserror(N'Giá trị giảm giá không hợp lệ!', 16, 1);
        rollback transaction;
        return;
    end
    
    -- Kiểm tra thời gian hợp lệ
    if exists(select 1 from inserted where EndDate <= StartDate)
    begin
        raiserror(N'Thời gian kết thúc phải sau thời gian bắt đầu!', 16, 1);
        rollback transaction;
        return;
    end
    
    -- Kiểm tra trùng lặp thời gian giảm giá cho cùng sản phẩm
    if exists(
        select 1 
        from inserted i
        inner join dbo.Discounts d on i.ProductID = d.ProductID
        where d.IsActive = 1
          and d.DiscountID != isnull(i.DiscountID, 0)
          and (
              (i.StartDate between d.StartDate and d.EndDate) or
              (i.EndDate between d.StartDate and d.EndDate) or
              (d.StartDate between i.StartDate and i.EndDate) or
              (d.EndDate between i.StartDate and i.EndDate)
          )
    )
    begin
        raiserror(N'Đã có chương trình giảm giá khác cho sản phẩm này trong khoảng thời gian này!', 16, 1);
        rollback transaction;
        return;
    end
end
go

PRINT N'✅ Đã tạo thành công 2 triggers cần thiết!'
PRINT N'📋 Tổng cộng:'
PRINT N'   - TR_SaleDetails_UpdateStock - Cập nhật tồn kho khi bán hàng'
PRINT N'   - TR_Sales_UpdateLoyaltyPoints - Cập nhật điểm tích lũy khách hàng'
PRINT N''
PRINT N'🔧 Chức năng của triggers:'
PRINT N'   1. TR_SaleDetails_UpdateStock:'
PRINT N'      - Tự động giảm số lượng tồn kho khi thêm chi tiết hóa đơn'
PRINT N'      - Ghi log giao dịch cập nhật tồn kho'
PRINT N'   2. TR_Sales_UpdateLoyaltyPoints:'
PRINT N'      - Tự động cộng điểm tích lũy cho khách hàng (10,000 VND = 1 điểm)'
PRINT N'      - Ghi log giao dịch tích điểm'
GO

/* ============================================================
   Trigger: dbo.trg_Accounts_AutoRole
   Mục đích:
     - Khi thêm/sửa tài khoản trong dbo.Accounts, tự động đồng bộ role
       DB: Minimart_SalesDB
   Ghi chú:
     - Cần quyền để CREATE USER / ALTER ROLE. Dùng EXECUTE AS OWNER.
     - Cột tên người dùng: Username
     - Cột vai trò: Role (giá trị mong đợi: 'manager' hoặc 'saler')
   ============================================================ */
USE Minimart_SalesDB;
GO

/*CREATE OR ALTER TRIGGER dbo.trg_Accounts_AutoRole
ON dbo.Account
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Gom danh sách user bị ảnh hưởng
    IF OBJECT_ID('tempdb..#Affected') IS NOT NULL DROP TABLE #Affected;

    SELECT DISTINCT
           [Username] = i.[Username],
           RoleName   = LOWER(i.[Role])
    INTO #Affected
    FROM inserted i
    WHERE i.[Username] IS NOT NULL;

    DECLARE @u SYSNAME, @role NVARCHAR(50), @sql NVARCHAR(MAX);

    DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
        SELECT [Username], RoleName FROM #Affected;

    OPEN cur;
    FETCH NEXT FROM cur INTO @u, @role;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        BEGIN TRY
            /* 1) Tạo DB USER nếu chưa có (chỉ khi LOGIN cùng tên đã tồn tại) */
            IF NOT EXISTS (
                SELECT 1 FROM sys.database_principals
                WHERE name = @u AND type IN ('S','U','G')
            )
            BEGIN
                IF EXISTS (SELECT 1 FROM sys.server_principals WHERE name = @u)
                BEGIN
                    SET @sql = N'CREATE USER ' + QUOTENAME(@u)
                             + N' FOR LOGIN ' + QUOTENAME(@u) + N';';
                    EXEC (@sql);
                END
                ELSE
                BEGIN
                    PRINT N'[AutoRole] Bỏ qua CREATE USER vì chưa có LOGIN: ' + @u;
                END
            END

            /* 2) Bỏ khỏi các role cũ (nếu đang là thành viên) để tránh “2 vai” */
            IF EXISTS (
                SELECT 1
                FROM sys.database_role_members drm
                JOIN sys.database_principals r ON r.principal_id = drm.role_principal_id
                JOIN sys.database_principals m ON m.principal_id = drm.member_principal_id
                WHERE r.name = N'MiniMart_Manager' AND m.name = @u
            )
            BEGIN
                SET @sql = N'ALTER ROLE [MiniMart_Manager] DROP MEMBER ' + QUOTENAME(@u) + N';';
                EXEC (@sql);
            END

            IF EXISTS (
                SELECT 1
                FROM sys.database_role_members drm
                JOIN sys.database_principals r ON r.principal_id = drm.role_principal_id
                JOIN sys.database_principals m ON m.principal_id = drm.member_principal_id
                WHERE r.name = N'MiniMart_Saler' AND m.name = @u
            )
            BEGIN
                SET @sql = N'ALTER ROLE [MiniMart_Saler] DROP MEMBER ' + QUOTENAME(@u) + N';';
                EXEC (@sql);
            END

            /* 3) Thêm vào role mới theo cột [Role] */
            IF (@role = N'manager')
            BEGIN
                SET @sql = N'ALTER ROLE [MiniMart_Manager] ADD MEMBER ' + QUOTENAME(@u) + N';';
                EXEC (@sql);
            END
            ELSE IF (@role = N'saler')
            BEGIN
                SET @sql = N'ALTER ROLE [MiniMart_Saler] ADD MEMBER ' + QUOTENAME(@u) + N';';
                EXEC (@sql);
            END
            ELSE
            BEGIN
                PRINT N'[AutoRole] Giá trị Role không hỗ trợ, bỏ qua user: '
                      + @u + N' (Role=' + COALESCE(@role,N'NULL') + N')';
            END
        END TRY
        BEGIN CATCH
            PRINT N'[AutoRole][Error] User=' + COALESCE(@u,N'(null)') + N' → ' + ERROR_MESSAGE();
        END CATCH;

        FETCH NEXT FROM cur INTO @u, @role;
    END

    CLOSE cur; DEALLOCATE cur;
    DROP TABLE IF EXISTS #Affected;
END
GO
*/

CREATE OR ALTER TRIGGER trg_CreateSQLAccount
ON dbo.Account
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @username NVARCHAR(50), @password NVARCHAR(255), @role NVARCHAR(20);
    DECLARE @sqlString NVARCHAR(MAX);
    
    -- Lấy thông tin từ record mới được thêm
    SELECT @username = i.Username,
           @password = i.Password,
           @role = i.Role
    FROM inserted i;

    BEGIN TRY
        -- Tạo Login (kiểm tra không tồn tại)
        IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = @username)
        BEGIN
            SET @sqlString = 'CREATE LOGIN [' + @username + '] WITH PASSWORD = ''' + @password + 
                            ''', DEFAULT_DATABASE = [Minimart_SalesDB], CHECK_EXPIRATION = OFF, CHECK_POLICY = OFF';
            EXEC (@sqlString);
        END

        -- Tạo User (kiểm tra không tồn tại)
        IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = @username)
        BEGIN
            SET @sqlString = 'CREATE USER [' + @username + '] FOR LOGIN [' + @username + ']';
            EXEC (@sqlString);
        END

        -- Gán vào Role tương ứng theo role trong bảng Account
        IF (@role = N'manager')
        BEGIN
            -- Kiểm tra role tồn tại trước khi gán
            IF EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Manager' AND type = 'R')
            BEGIN
                SET @sqlString = 'ALTER ROLE [MiniMart_Manager] ADD MEMBER [' + @username + ']';
                EXEC (@sqlString);
            END
        END
        ELSE IF (@role = N'saler')
        BEGIN
            -- Kiểm tra role tồn tại trước khi gán
            IF EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Saler' AND type = 'R')
            BEGIN
                SET @sqlString = 'ALTER ROLE [MiniMart_Saler] ADD MEMBER [' + @username + ']';
                EXEC (@sqlString);
            END
        END
        ELSE IF (@role = N'customer')
        BEGIN
            -- Kiểm tra role tồn tại trước khi gán
            IF EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Customer' AND type = 'R')
            BEGIN
                SET @sqlString = 'ALTER ROLE [MiniMart_Customer] ADD MEMBER [' + @username + ']';
                EXEC (@sqlString);
            END
        END
    END TRY
    BEGIN CATCH
        -- Ghi log lỗi nếu cần (có thể bỏ qua lỗi nếu Login/User đã tồn tại)
        PRINT 'Warning: Could not create SQL account for user: ' + @username;
        PRINT ERROR_MESSAGE();
    END CATCH
END;
GO

-- ========================================
-- 7. TẠO FUNCTION KIỂM TRA QUYỀN
-- ========================================

CREATE OR ALTER TRIGGER trg_CreateSQLAccount
ON dbo.Account
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @username NVARCHAR(50), @password NVARCHAR(255), @role NVARCHAR(20);
    DECLARE @sqlString NVARCHAR(MAX);
    
    -- Lấy thông tin từ record mới được thêm
    SELECT @username = i.Username,
           @password = i.Password,
           @role = i.Role
    FROM inserted i;

    BEGIN TRY
        -- Tạo Login (kiểm tra không tồn tại)
        IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = @username)
        BEGIN
            SET @sqlString = 'CREATE LOGIN [' + @username + '] WITH PASSWORD = ''' + @password + 
                            ''', DEFAULT_DATABASE = [Minimart_SalesDB], CHECK_EXPIRATION = OFF, CHECK_POLICY = OFF';
            EXEC (@sqlString);
        END

        -- Tạo User (kiểm tra không tồn tại)
        IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = @username)
        BEGIN
            SET @sqlString = 'CREATE USER [' + @username + '] FOR LOGIN [' + @username + ']';
            EXEC (@sqlString);
        END

        -- Gán vào Role tương ứng theo role trong bảng Account
        IF (@role = N'manager')
        BEGIN
            -- Kiểm tra role tồn tại trước khi gán
            IF EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Manager' AND type = 'R')
            BEGIN
                SET @sqlString = 'ALTER ROLE [MiniMart_Manager] ADD MEMBER [' + @username + ']';
                EXEC (@sqlString);
            END
        END
        ELSE IF (@role = N'saler')
        BEGIN
            -- Kiểm tra role tồn tại trước khi gán
            IF EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Saler' AND type = 'R')
            BEGIN
                SET @sqlString = 'ALTER ROLE [MiniMart_Saler] ADD MEMBER [' + @username + ']';
                EXEC (@sqlString);
            END
        END
        ELSE IF (@role = N'customer')
        BEGIN
            -- Kiểm tra role tồn tại trước khi gán
            IF EXISTS (SELECT * FROM sys.database_principals WHERE name = 'MiniMart_Customer' AND type = 'R')
            BEGIN
                SET @sqlString = 'ALTER ROLE [MiniMart_Customer] ADD MEMBER [' + @username + ']';
                EXEC (@sqlString);
            END
        END
    END TRY
    BEGIN CATCH
        -- Ghi log lỗi nếu cần (có thể bỏ qua lỗi nếu Login/User đã tồn tại)
        PRINT 'Warning: Could not create SQL account for user: ' + @username;
        PRINT ERROR_MESSAGE();
    END CATCH
END;
GO
