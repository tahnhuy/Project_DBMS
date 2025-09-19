if db_id('Minimart_SalesDB') is null
begin
    create database Minimart_SalesDB;
end
go

use Minimart_SalesDB
go

-- Drop all functions (scalar, inline TVF, multi-statement TVF)
declare @sql nvarchar(max) = N'';
select @sql = @sql + N'DROP FUNCTION [' + s.name + N'].[' + o.name + N'];' + CHAR(10)
from sys.objects o
inner join sys.schemas s on s.schema_id = o.schema_id
where o.type in ('FN','IF','TF');

if len(@sql) > 0 exec sp_executesql @sql;
go

-- Recreate functions actually used by the application

-- Chọn sản phẩm theo tên
create or alter function dbo.GetProductByName(@Name nvarchar(100))
returns table
as
    return (
        select * from dbo.Products 
        where ProductName collate Vietnamese_CI_AI like N'%' + @Name + '%'
    );
go

-- Chọn sản phẩm theo ID 
create or alter function dbo.GetProductByID(@id int)
returns table 
as
return
(
    select * from dbo.Products 
    where ProductID = @id
);
go

