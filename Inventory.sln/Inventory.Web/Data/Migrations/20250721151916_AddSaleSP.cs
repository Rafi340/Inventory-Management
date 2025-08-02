using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSaleSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                    CREATE OR ALTER   PROCEDURE [dbo].[GetSales] 
                	@PageIndex int,
                	@PageSize int , 
                	@OrderBy nvarchar(50),
                	@InvoiceNo NVARCHAR(MAX) = '%',
                	@CustomerId nvarchar(128) = NULL,
                	@FromDate DATETIME = NULL,
                	@ToDate DATETIME = NULL,
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
                	Select @Total = count(*) from Sales;

                	-- Collecting Total Display
                	SET @countsql = 'select @TotalDisplay = count(*) from Sales S
                	INNER JOIN AccountType AT on AT.Id = S.AcountTypeId
                	INNER JOIN Account A on A.Id = S.AccountId
                	INNER JOIN SalesType ST on S.SalesTypeId = ST.Id
                	INNER JOIN Customer C on C.Id = S.CustomerId

                	where 1 = 1 ';

                	SET @countsql = @countsql + ' AND s.InvoiceNo LIKE ''%'' + @xInvoiceNo + ''%''' 



                	IF @CustomerId IS NOT NULL
                	SET @countsql = @countsql + ' AND S.CustomerId = @xCustomerId'

                	IF @FromDate IS NOT NULL
                	SET @countsql = @countsql + ' AND S.SaleDate >= @xFromDate'

                	IF @ToDate IS NOT NULL
                	SET @countsql = @countsql + ' AND S.SaleDate <= @xToDate'

                	SELECT @countparamlist = '@xInvoiceNo nvarchar(max),
                		@xCustomerId nvarchar(128),
                		@xFromDate DATETIME,
                		@xToDate DATETIME,
                		@TotalDisplay int output' ;

                	exec sp_executesql @countsql , @countparamlist ,
                		@InvoiceNo,
                		@CustomerId,
                		@FromDate,
                		@ToDate,
                		@TotalDisplay = @TotalDisplay output;

                	-- Collecting Data
                	SET @sql = '
                	SELECT S.Id
                		,S.InvoiceNo
                      , S.SaleDate
                      , C.Name AS CustomerName
                      , ST.Name AS SalesTypeName
                      , S.Vat 
                      , S.NetAmount
                      ,S.Discount
                      ,S.TotalAmount
                      ,S.PaidAmount
                      ,S.DueAmount
                      ,AT.Type AS AccountTypeName
                      , A.AccountNumber
                      ,S.Note
                      ,S.TermsConditions
                      ,S.CreatedAt
                      ,S.UpdatedAt
                      , 
                	  CASE
                        WHEN S.STATUS = 1 THEN ''<span class="badge bg-success bg-glow">Paid</span>''
                        WHEN S.STATUS = 0 THEN ''<span class="badge bg-danger bg-glow">Due</span>''
                        WHEN S.STATUS = 2 THEN ''<span class="badge bg-warning bg-glow text-dark">Partial Paid</span>''
                        ELSE ''<span class="badge bg-secondary bg-glow">Unknown</span>''
                		END AS StatusHtml,
                		S.Status
                	  FROM Sales S

                	INNER JOIN AccountType AT on AT.Id = S.AcountTypeId
                	INNER JOIN Account A on A.Id = S.AccountId
                	INNER JOIN SalesType ST on S.SalesTypeId = ST.Id
                	INNER JOIN Customer C on C.Id = S.CustomerId
                	where 1 = 1 ';

                	SET @sql = @sql + ' AND S.InvoiceNo LIKE ''%'' + @xInvoiceNo + ''%''' 

                	IF @CustomerId IS NOT NULL
                	SET @sql = @sql + ' AND S.CustomerId = @xCustomerId'

                	IF @FromDate IS NOT NULL
                	SET @sql = @sql + ' AND S.SaleDate >= @xFromDate'

                	IF @ToDate IS NOT NULL
                	SET @sql = @sql + ' AND S.SaleDate <= @xToDate'



                	SET @sql = @sql + ' Order by '+@OrderBy+' OFFSET @PageSize * (@PageIndex - 1) 
                	ROWS FETCH NEXT @PageSize ROWS ONLY';

                	SELECT @paramlist = '@xInvoiceNo nvarchar(max),
                		@xCustomerId nvarchar(128),
                		@xFromDate DATETIME,
                		@xToDate DATETIME,
                		@PageIndex int,
                		@PageSize int' ;

                	exec sp_executesql @sql , @paramlist ,
                		@InvoiceNo,
                		@CustomerId,
                		@FromDate,
                		@ToDate,
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
            var sql = "DROP PROCEDURE [dbo].[GetSales]";
            migrationBuilder.Sql(sql);
        }
    }
}
