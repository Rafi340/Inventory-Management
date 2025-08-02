using Autofac;
using Inventory.Application;
using Inventory.Application.Features.Customers.Queries;
using Inventory.Application.Features.Products.Commands;
using Inventory.Application.Services;
using Inventory.Domain.Repositories;
using Inventory.Domain.Services;
using Inventory.Domain.Utilities;
using Inventory.Infrastructure;
using Inventory.Infrastructure.Repositories;
using Inventory.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Worker
{
    public class WorkerModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;

        public WorkerModule(string connectionString, string migrationAssembly)
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
            builder.RegisterType<ImageResizerUtility>().As<IImageResizerUtility>();
            builder.RegisterType<BalanceTransferRepository>().As<IBalanceTransferRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<AwsUtility>().As<IAwsUtility>()
                .InstancePerLifetimeScope();
            builder.RegisterType<EmailUtility>().As<IEmailUtility>()
                .InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}
