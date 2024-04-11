using DataAccessWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessWebAPI.DataAccessLayer
{
    public class IntegralInvestingAppDbContext : DbContext
    {
        public IntegralInvestingAppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<BankAccount> BankAccounts { get; set; }
    }
}
