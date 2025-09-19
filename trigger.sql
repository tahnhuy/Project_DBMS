if db_id('Minimart_SalesDB') is null
begin
    create database Minimart_SalesDB;
end
go

use Minimart_SalesDB
go

-- Drop all object/table triggers in current database
declare @sql nvarchar(max) = N'';
select @sql = @sql + N'DROP TRIGGER [' + s.name + N'].[' + t.name + N'];' + CHAR(10)
from sys.triggers t
inner join sys.objects o on t.object_id = o.object_id
inner join sys.schemas s on o.schema_id = s.schema_id
where t.parent_class_desc = 'OBJECT_OR_COLUMN';

if len(@sql) > 0 exec sp_executesql @sql;
go

-- No triggers required by the application at the moment.
-- Add new triggers here if/when needed.
go

