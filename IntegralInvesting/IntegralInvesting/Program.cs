using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IntegralInvesting.Data;
using IntegralInvesting.Areas.Identity.Data;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("IntegralInvestingDataContextConnection") ?? throw new InvalidOperationException("Connection string 'IntegralInvestingDataContextConnection' not found.");

builder.Services.AddDbContext<IntegralInvestingDataContext>(options => options.UseSqlServer(connectionString));

// RequireConfirmedAccount set to false means the user does not need to do a verification email upon registration
builder.Services.AddDefaultIdentity<IntegralInvestingUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<IntegralInvestingDataContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
