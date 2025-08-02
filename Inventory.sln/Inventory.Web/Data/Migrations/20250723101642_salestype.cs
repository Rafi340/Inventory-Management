using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class salestype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                                   CREATE OR ALTER     PROCEDURE [dbo].[GetUsers] 
                	@PageIndex int,
                	@PageSize int , 
                	@OrderBy nvarchar(50),
                	@SearchText nvarchar(max) = '%',
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
                	Select @Total = count(*) from BalanceTransfer;

                	-- Collecting Total Display
                	SET @countsql = 'select @TotalDisplay = count(*) from AspNetUsers AU
                	INNER JOIN AspNetUserRoles UR ON AU.Id = UR.UserId
                	INNER JOIN AspNetRoles R ON R.Id = UR.RoleId

                	where 1 = 1 ';

                	SET @countsql = @countsql + ' AND AU.NormalizedEmail LIKE ''%'' + @xSearchText + ''%''' 




                	SELECT @countparamlist = '@xSearchText nvarchar(max),
                		@TotalDisplay int output' ;

                	exec sp_executesql @countsql , @countparamlist ,
                		@SearchText,
                		@TotalDisplay = @TotalDisplay output;

                	-- Collecting Data
                	SET @sql = 'Select 
                	AU.Id,
                	AU.UserName,
                	AU.FirstName,
                	AU.LastName,
                	AU.RegistrationDate,
                	R.Name AS RoleName
                	from AspNetUsers AU

                	INNER JOIN AspNetUserRoles UR ON AU.Id = UR.UserId
                	INNER JOIN AspNetRoles R ON R.Id = UR.RoleId
                	where 1 = 1 ';

                	SET @sql = @sql + ' AND AU.NormalizedEmail LIKE ''%'' + @xSearchText + ''%''' 


                	SET @sql = @sql + ' Order by '+@OrderBy+' OFFSET @PageSize * (@PageIndex - 1) 
                	ROWS FETCH NEXT @PageSize ROWS ONLY';

                	SELECT @paramlist = '
                		@xSearchText nvarchar(max),
                		@PageIndex int,
                		@PageSize int' ;

                	exec sp_executesql @sql , @paramlist ,
                		@SearchText,
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
            var sql = "DROP PROCEDURE [dbo].[GetUsers]";
            migrationBuilder.Sql(sql);
        }
    }
}
