using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Infrastructure.Data.Entities
{
    public class Account
    {
        public string ShebaNumber { get; set; }
        public string FullName { get; set; }
        public decimal Balance { get; set; }
        
    }

    // Custom config for Accounts table
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");
            builder.HasKey(x => x.ShebaNumber);
        }
    }
}

