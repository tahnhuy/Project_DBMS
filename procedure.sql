

-- Lấy thông tin sản phẩm
create or alter procedure GetAllProducts 
as
begin
	select 
		[ProductID], [ProductName], [Price], [StockQuantity], [Unit]
	from [dbo].[Products]
	order by [ProductID]
end
go


use Minimart_SalesDB 
go

-- Chọn sản phẩm theo tên
create or alter function GetProductByName(@Name nvarchar(100))
returns table
as
	return (
		select * from Products 
		where ProductName collate Vietnamese_CI_AI like N'%' + @Name + '%');

go


-- Chọn sản phẩm theo ID 
create or alter function GetProductByID(@id int)
returns table 
as
return
(
	select * from Products 
	where ProductID = @id
);
go


-- Thêm sản phẩm mới
create or alter procedure AddProduct
    @ProductName nvarchar(100),
    @Price decimal(18, 2),
    @StockQuantity int,
    @Unit nvarchar(50)
as
begin
    begin try
        insert into Products (ProductName, Price, StockQuantity, Unit)
        values (@ProductName, @Price, @StockQuantity, @Unit)
        
        select 'SUCCESS' as Result, 'Thêm sản phẩm thành công' as Message
    end try
    begin catch
        select 'ERROR' as Result, ERROR_MESSAGE() as Message
    end catch
end
go

-- Cập nhật sản phẩm
create or alter procedure UpdateProduct
    @ProductID int,
    @ProductName nvarchar(100),
    @Price decimal(18, 2),
    @StockQuantity int,
    @Unit nvarchar(50)
as
begin
    begin try
        update Products 
        set ProductName = @ProductName,
            Price = @Price,
            StockQuantity = @StockQuantity,
            Unit = @Unit
        where ProductID = @ProductID
        
        if @@ROWCOUNT = 0
            select 'ERROR' as Result, 'Không tìm thấy sản phẩm để cập nhật' as Message
        else
            select 'SUCCESS' as Result, 'Cập nhật sản phẩm thành công' as Message
    end try
    begin catch
        select 'ERROR' as Result, ERROR_MESSAGE() as Message
    end catch
end
go

-- Xóa sản phẩm
create or alter procedure DeleteProduct
    @ProductID int
as
begin
    begin try
        -- Kiểm tra xem sản phẩm có trong SaleDetails không
        if exists (select 1 from SaleDetails where ProductID = @ProductID)
        begin
            select 'ERROR' as Result, 'Không thể xóa sản phẩm vì đã có trong hóa đơn' as Message
            return
        end
        
        delete from Products where ProductID = @ProductID
        
        if @@ROWCOUNT = 0
            select 'ERROR' as Result, 'Không tìm thấy sản phẩm để xóa' as Message
        else
            select 'SUCCESS' as Result, 'Xóa sản phẩm thành công' as Message
    end try
    begin catch
        select 'ERROR' as Result, ERROR_MESSAGE() as Message
    end catch
end
go

select * from GetProductByName('Gao');
select * from GetProductByID(1);

select * from Products