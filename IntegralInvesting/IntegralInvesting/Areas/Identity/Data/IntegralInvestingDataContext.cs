using IntegralInvesting.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IntegralInvesting.Models;

namespace IntegralInvesting.Data;

public class IntegralInvestingDataContext : IdentityDbContext<IntegralInvestingUser>
{
    public IntegralInvestingDataContext(DbContextOptions<IntegralInvestingDataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

public DbSet<IntegralInvesting.Models.BankAccountViewModel> BankAccountViewModel { get; set; } = default!;
}
