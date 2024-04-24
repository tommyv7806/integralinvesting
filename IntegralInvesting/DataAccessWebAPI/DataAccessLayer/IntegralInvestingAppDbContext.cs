using DataAccessWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessWebAPI.DataAccessLayer
{
    public class IntegralInvestingAppDbContext : DbContext
    {
        public IntegralInvestingAppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<UserFund> UserFunds { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<PortfolioStock> PortfolioStocks { get; set; }
        public DbSet<PortfolioAsset> PortfolioAssets { get; set; }
    }
}
