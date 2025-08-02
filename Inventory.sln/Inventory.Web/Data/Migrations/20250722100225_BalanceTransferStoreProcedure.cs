using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class BalanceTransferStoreProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                       CREATE OR    ALTER    PROCEDURE [dbo].[GetBalanceTransfer] 
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
                	SET @countsql = 'select @TotalDisplay = count(*) from BalanceTransfer BT
                	INNER JOIN  AccountType SAT on SAT.Id = BT.SenderAccountTypeId
                	INNER JOIN Account SA on SA.Id = BT.SenderAccountId
                	INNER JOIN AccountType RAT on RAT.Id = BT.ReceiverAccountTypeId
                	INNER JOIN Account RA on RA.Id = BT.ReceiverAccountId
                	where 1 = 1 ';

                	SET @countsql = @countsql + ' OR SAT.Type LIKE ''%'' + @xSearchText + ''%''' 
                	SET @countsql = @countsql + ' OR SA.AccountNumber LIKE ''%'' + @xSearchText + ''%'''

                	SET @countsql = @countsql + ' OR RAT.Type LIKE ''%'' + @xSearchText + ''%''' 
                	SET @countsql = @countsql + ' OR RA.AccountNumber LIKE ''%'' + @xSearchText + ''%'''



                	SELECT @countparamlist = '@xSearchText nvarchar(max),
                		@TotalDisplay int output' ;

                	exec sp_executesql @countsql , @countparamlist ,
                		@SearchText,
                		@TotalDisplay = @TotalDisplay output;

                	-- Collecting Data
                	SET @sql = 'Select BT.Id,
                	SAT.Type AS SenderAcountType,
                	SA.AccountNumber as SenderAccountNumber,
                	RAT.Type As ReceiverAccountType,
                	RA.AccountNumber as ReceiverAccountNumber,
                	BT.TransferAmount,
                	BT.Note,
                	BT.CreatedAt TransferTime
                	from BalanceTransfer BT

                	INNER JOIN  AccountType SAT on SAT.Id = BT.SenderAccountTypeId
                	INNER JOIN Account SA on SA.Id = BT.SenderAccountId
                	INNER JOIN AccountType RAT on RAT.Id = BT.ReceiverAccountTypeId
                	INNER JOIN Account RA on RA.Id = BT.ReceiverAccountId
                	where 1 = 1 ';

                	SET @sql = @sql + ' OR SAT.Type LIKE ''%'' + @xSearchText + ''%'''

                	SET @sql = @sql + ' OR SA.AccountNumber LIKE ''%'' + @xSearchText + ''%'''

                	SET @sql = @sql + ' OR RAT.Type LIKE ''%'' + @xSearchText + ''%''' 

                	SET @sql = @sql + ' OR RA.AccountNumber LIKE ''%'' + @xSearchText + ''%'''

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
            var sql = "DROP PROCEDURE [dbo].[GetBalanceTransfer]";
            migrationBuilder.Sql(sql);
        }
    }
}
