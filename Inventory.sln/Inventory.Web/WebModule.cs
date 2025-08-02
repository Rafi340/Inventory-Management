using Autofac;
using Inventory.Application;
using Inventory.Application.Features.Products.Commands;
using Inventory.Application.Services;
using Inventory.Domain.Repositories;
using Inventory.Domain.Services;
using Inventory.Domain.Utilities;
using Inventory.Infrastructure;
using Inventory.Infrastructure.Repositories;
using Inventory.Infrastructure.Utilities;
namespace Inventory.Web
{
    public class WebModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;
        public WebModule(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDbContext>().AsSelf()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssembly", _migrationAssembly)
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationUnitOfWork>().As<IApplicationUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ProductRepository>().As<IProductRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ProductService>().As<IProductService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<CategoryService>().As<ICategoryService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitRepository>().As<IUnitRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<UnitService>().As<IUnitService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<SalesTypeRepository>().As<ISalesTypeRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AccountTypeRepository>().As<IAccountTypeRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AccountRepository>().As<IAccountRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<SaleRepository>().As<ISaleRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<SaleItemsRepository>().As<ISaleItemsRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AwsUtility>().As<IAwsUtility>()
                .InstancePerLifetimeScope();

            builder.RegisterType<FileHelperUtility>().As<IFileHelperUtility>()
                .InstancePerLifetimeScope();
            builder.RegisterType<BalanceTransferRepository>().As<IBalanceTransferRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ProductAddCommand>().AsSelf();
            base.Load(builder);
        }
    }
}
