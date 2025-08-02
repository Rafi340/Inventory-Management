using Inventory.Domain.Entities;
using Inventory.Infrastructure.Identity;
using Inventory.Infrastructure.Seeds;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,
        ApplicationRole, Guid,
        ApplicationUserClaim, ApplicationUserRole,
        ApplicationUserLogin, ApplicationRoleClaim,
        ApplicationUserToken>
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;
        public ApplicationDbContext(string connectionString, string migrationAssembly)
        {
            _migrationAssembly = migrationAssembly;
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString, (x) => x.MigrationsAssembly(_migrationAssembly));
            }
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.UnitPrice)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Sale>()
                .HasMany(x => x.SalesItems)
                .WithOne(x => x.Sales)
                .HasForeignKey(x => x.SalesId);
           


            modelBuilder.Entity<SalesType>().HasData(TypeSeed.SaleTypeSeed());
            modelBuilder.Entity<AccountType>().HasData(TypeSeed.AcountTypeSeed());
            modelBuilder.Entity<Account>().HasData(AccountSeed.AccountSeeds());
            modelBuilder.Entity<ApplicationRole>().HasData(RoleSeed.GetRoles());
            modelBuilder.Entity<Category>().HasData(CategorySeed.CategorySeeds());
            modelBuilder.Entity<Unit>().HasData(UnitSeed.UnitSeeds());
        }
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItems> SalesItems { get; set; }
        public DbSet<SalesType> SalesType { get; set; }
        public DbSet<AccountType> AccountType { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<BalanceTransfer> BalanceTransfer { get; set; }
    }
}
