using Microsoft.EntityFrameworkCore;
using MCBA_Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MCBA_Web.Data
{
    public class McbaContext : IdentityDbContext<IdentityUser>
    {
        public McbaContext(DbContextOptions<McbaContext> options) : base(options)
        { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        // Fluent-API.
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Customer>().HasCheckConstraint("CH_CustomerID", "len(CustomerID) = 4").
                HasCheckConstraint("CH_TFN", "len(TFN) = 11").
                HasCheckConstraint("CH_Postcode", "len(Postcode) = 4").
                HasCheckConstraint("CH_Mobile", "len(Mobile) = 12");
            builder.Entity<Login>().HasCheckConstraint("CH_Login_LoginID", "len(LoginID) = 8").
                HasCheckConstraint("CH_Login_PasswordHash", "len(PasswordHash) = 64");
            builder.Entity<Account>().HasCheckConstraint("CH_Account_Balance", "Balance >= 0");
            builder.Entity<Transaction>().HasCheckConstraint("CH_Transaction_Amount", "Amount > 0");

            // Configure ambiguous Account.Transactions navigation property relationship.
            builder.Entity<Transaction>().
                HasOne(x => x.Account).WithMany(x => x.Transactions).HasForeignKey(x => x.AccountNumber);
        }
    }
}
