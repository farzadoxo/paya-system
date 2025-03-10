using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Entities;

namespace Infrastructure.Data.Contexts
{
    public class PayaDbContext : DbContext
    {
        public PayaDbContext(DbContextOptions option):base(option)
        {

        }

        // Tables
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        // Applying config on database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
        }

        // Connect
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-9PPD55B; Initial Catalog=Bank; Integrated Security=true; TrustServerCertificate=True");
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added);

            foreach (var entry in entries)
            {
                // دسترسی به اطلاعات رکورد جدید
                var entity = entry.Entity;
                
                // ذخیره اطلاعات قبل از ذخیره‌سازی
                var originalValues = entry.OriginalValues;
                
                // ذخیره اطلاعات بعد از ذخیره‌سازی
                await base.SaveChangesAsync(cancellationToken);
                var currentValues = entry.CurrentValues;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}