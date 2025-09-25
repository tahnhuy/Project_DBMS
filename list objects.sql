use Minimart_SalesDB
go

-- Liệt kê Procedure, Function, View, Trigger (DML & DDL database-level) trong DB hiện tại
;WITH objs AS (
    SELECT 
        schema_name = OBJECT_SCHEMA_NAME(o.object_id),
        object_name = o.name,
        object_type = CASE o.type
            WHEN 'P'  THEN 'PROCEDURE'
            WHEN 'FN' THEN 'FUNCTION_SCALAR'
            WHEN 'TF' THEN 'FUNCTION_TABLE_MULTI'
            WHEN 'IF' THEN 'FUNCTION_TABLE_INLINE'
            WHEN 'V'  THEN 'VIEW'
            WHEN 'TR' THEN 'TRIGGER_DML'  -- trigger gắn với bảng/view
            ELSE o.type_desc
        END,
        create_date = o.create_date,
        modify_date = o.modify_date
    FROM sys.objects o
    WHERE o.type IN ('P','FN','TF','IF','V','TR')
      AND o.is_ms_shipped = 0
)
SELECT *
FROM objs
UNION ALL
-- DDL trigger mức database (không có schema)
SELECT
    schema_name = NULL,
    object_name = t.name,
    object_type = 'TRIGGER_DDL_DATABASE',
    t.create_date,
    t.modify_date
FROM sys.triggers AS t
WHERE t.parent_class = 0   -- 0 = database-level
ORDER BY object_type, schema_name, object_name;
