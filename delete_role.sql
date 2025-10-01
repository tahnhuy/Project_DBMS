/* ============================================================
   PHẦN 1: DROP role customer & RESET quyền 2 role còn lại
   DB: Minimart_SalesDB
   - Sửa lỗi: KHÔNG dùng QUOTENAME cho tên quyền (DELETE/SELECT/…)
   - Vẫn REVOKE GRANT OPTION FOR trước khi REVOKE quyền chính
   ============================================================ */
USE Minimart_SalesDB;
GO
SET NOCOUNT ON;

BEGIN TRY
  BEGIN TRAN;

  /* 1) Gỡ thành viên & DROP ROLE MiniMart_Customer (nếu có) */
  IF EXISTS (SELECT 1 FROM sys.database_principals 
             WHERE name = N'MiniMart_Customer' AND type = 'R')
  BEGIN
    DECLARE @sqlDropMembers NVARCHAR(MAX) = N'';
    SELECT @sqlDropMembers = @sqlDropMembers
      + N'ALTER ROLE [MiniMart_Customer] DROP MEMBER '
      + QUOTENAME(USER_NAME(drm.member_principal_id)) + N';' + CHAR(10)
    FROM sys.database_role_members drm
    JOIN sys.database_principals r
      ON r.principal_id = drm.role_principal_id
    WHERE r.name = N'MiniMart_Customer';

    IF (@sqlDropMembers <> N'') EXEC sys.sp_executesql @sqlDropMembers;

    DROP ROLE MiniMart_Customer;
  END

  /* 2) Thu gom mọi quyền hiện có của Manager/Saler để REVOKE */
  ;WITH roles AS (
    SELECT principal_id, name
    FROM sys.database_principals
    WHERE name IN (N'MiniMart_Manager', N'MiniMart_Saler')
  )
  SELECT
      dp.class,               -- 0 = DB, 1 = Object/Column, 3 = Schema
      dp.class_desc,
      dp.permission_name,     -- ví dụ: SELECT, DELETE, EXECUTE, VIEW DEFINITION, ...
      dp.state_desc,          -- GRANT, GRANT_WITH_GRANT_OPTION, DENY
      dp.major_id, dp.minor_id,
      r.name AS role_name,
      obj = CASE 
              WHEN dp.class = 1 THEN QUOTENAME(SCHEMA_NAME(o.schema_id)) + N'.' + QUOTENAME(o.name)
              WHEN dp.class = 3 THEN N'SCHEMA::' + QUOTENAME(sch.name)
              WHEN dp.class = 0 THEN N'DATABASE'
              ELSE NULL
            END,
      col = CASE WHEN dp.class = 1 THEN c.name ELSE NULL END
  INTO #perm_to_revoke
  FROM sys.database_permissions dp
  JOIN roles r ON r.principal_id = dp.grantee_principal_id
  LEFT JOIN sys.objects  o   ON dp.class = 1 AND dp.major_id = o.object_id
  LEFT JOIN sys.schemas  sch ON dp.class = 3 AND dp.major_id = sch.schema_id
  LEFT JOIN sys.columns  c   ON dp.class = 1 AND dp.major_id = c.object_id AND dp.minor_id = c.column_id;

  DECLARE @role SYSNAME, @class INT, @perm NVARCHAR(128), @state_desc NVARCHAR(60);
  DECLARE @obj NVARCHAR(512), @col SYSNAME, @cmd NVARCHAR(MAX);

  DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
    SELECT role_name, class, permission_name, state_desc, obj, col
    FROM #perm_to_revoke
    ORDER BY role_name, class, obj, col, permission_name;

  OPEN cur;
  FETCH NEXT FROM cur INTO @role, @class, @perm, @state_desc, @obj, @col;

  WHILE @@FETCH_STATUS = 0
  BEGIN
    /* 2.1) Nếu là GRANT WITH GRANT OPTION → REVOKE GRANT OPTION trước */
    IF (@state_desc = N'GRANT_WITH_GRANT_OPTION')
    BEGIN
      IF (@class = 0) -- DATABASE
        SET @cmd = N'REVOKE GRANT OPTION FOR ' + @perm + N' FROM ' + QUOTENAME(@role) + N';';
      ELSE IF (@class = 3) -- SCHEMA
        SET @cmd = N'REVOKE GRANT OPTION FOR ' + @perm + N' ON ' + @obj + N' FROM ' + QUOTENAME(@role) + N';';
      ELSE IF (@class = 1) -- OBJECT/COLUMN
        SET @cmd = N'REVOKE GRANT OPTION FOR ' + @perm
                 + CASE WHEN @col IS NOT NULL THEN N' (' + QUOTENAME(@col) + N')' ELSE N'' END
                 + N' ON OBJECT::' + @obj + N' FROM ' + QUOTENAME(@role) + N';';

      EXEC sys.sp_executesql @cmd;
    END

    /* 2.2) REVOKE quyền chính (xóa cả GRANT lẫn DENY) */
    IF (@class = 0) -- DATABASE
      SET @cmd = N'REVOKE ' + @perm + N' FROM ' + QUOTENAME(@role) + N';';
    ELSE IF (@class = 3) -- SCHEMA
      SET @cmd = N'REVOKE ' + @perm + N' ON ' + @obj + N' FROM ' + QUOTENAME(@role) + N';';
    ELSE IF (@class = 1) -- OBJECT/COLUMN
      SET @cmd = N'REVOKE ' + @perm
               + CASE WHEN @col IS NOT NULL THEN N' (' + QUOTENAME(@col) + N')' ELSE N'' END
               + N' ON OBJECT::' + @obj + N' FROM ' + QUOTENAME(@role) + N';';

    EXEC sys.sp_executesql @cmd;

    FETCH NEXT FROM cur INTO @role, @class, @perm, @state_desc, @obj, @col;
  END

  CLOSE cur; DEALLOCATE cur;
  DROP TABLE IF EXISTS #perm_to_revoke;

  COMMIT TRAN;
  PRINT N'✅ ĐÃ RESET SẠCH quyền của MiniMart_Manager & MiniMart_Saler và drop MiniMart_Customer (nếu có).';
END TRY
BEGIN CATCH
  IF XACT_STATE() <> 0 ROLLBACK TRAN;
  PRINT N'❌ Lỗi khi reset quyền/role.';
  PRINT ERROR_MESSAGE();
END CATCH
GO
