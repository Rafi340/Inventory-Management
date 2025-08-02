using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProductStoreProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                                CREATE OR ALTER  PROCEDURE [dbo].[GetProducts] 
                	@PageIndex int,
                	@PageSize int , 
                	@OrderBy nvarchar(50),
                	@UnitId int = NULL,
                	@CategoryId int = NULL,
                	@Name nvarchar(max) = '%',
                	@SKU nvarchar(max) = '%',
                	@Total int output,
                	@TotalDisplay int output
                AS
                BEGIN

                	SET NOCOUNT ON;

                	Declare @sql nvarchar(2000);
                	Declare @countsql nvarchar(2000);
                	Declare @paramList nvarchar(MAX); 
                	Declare @countparamList nvarchar(MAX);

                	-- Collecting Total
                	Select @Total = count(*) from Product;

                	-- Collecting Total Display
                	SET @countsql = 'select @TotalDisplay = count(*) from Product p
                	INNER JOIN Unit U on U.Id = p.UnitId
                	INNER JOIN Category C on C.Id = p.CategoryId

                	where 1 = 1 ';

                	SET @countsql = @countsql + ' AND p.Name LIKE ''%'' + @xName + ''%''' 

                	SET @countsql = @countsql + ' AND p.SKU LIKE ''%'' + @xSKU + ''%''' 

                	IF @UnitId IS NOT NULL
                	SET @countsql = @countsql + ' AND p.UnitId = @xUnitId'

                	IF @CategoryId IS NOT NULL
                	SET @countsql = @countsql + ' AND p.CategoryId = @xCategoryId' 

                	SELECT @countparamlist = '@xName nvarchar(max),
                		@xSKU nvarchar(max),
                		@xUnitId int,
                		@xCategoryId int,
                		@TotalDisplay int output' ;

                	exec sp_executesql @countsql , @countparamlist ,
                		@Name,
                		@SKU,
                		@UnitId,
                		@CategoryId,
                		@TotalDisplay = @TotalDisplay output;

                	-- Collecting Data
                	SET @sql = 'select p.Id,p.Name,p.Description,p.SKU,u.Name as UnitName, c.Name as CategoryName, p.UnitPrice, p.Quantity, (p.UnitPrice*p.Quantity) as TotalPrice from Product p

                	INNER JOIN Unit U on U.Id = p.UnitId
                	INNER JOIN Category C on C.Id = p.CategoryId
                	where 1 = 1 ';

                	SET @sql = @sql + ' AND p.Name LIKE ''%'' + @xName + ''%''' 

                	SET @sql = @sql + ' AND p.SKU LIKE ''%'' + @xSKU + ''%''' 

                	IF @UnitId IS NOT NULL
                	SET @sql = @sql + ' AND p.UnitId = @xUnitId'

                	IF @CategoryId IS NOT NULL
                	SET @sql = @sql + ' AND p.CategoryId = @xCategoryId' 

                	SET @sql = @sql + ' Order by '+@OrderBy+' OFFSET @PageSize * (@PageIndex - 1) 
                	ROWS FETCH NEXT @PageSize ROWS ONLY';

                	SELECT @paramlist = '@xName nvarchar(max),
                		@xSKU nvarchar(max),
                		@xUnitId int,
                		@xCategoryId int,
                		@PageIndex int,
                		@PageSize int' ;

                	exec sp_executesql @sql , @paramlist ,
                		@Name,
                		@SKU,
                		@UnitId,
                		@CategoryId,
                		@PageIndex,
                		@PageSize;

                	print @sql;
                	print @countsql;

                END
                """;
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[GetProducts]";
            migrationBuilder.Sql(sql);
        }
    }
}
