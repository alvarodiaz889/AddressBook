using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AddressBook.Data;
using AddressBook.Models;
using Microsoft.Extensions.Options;
using AddressBook.Tools;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<AppUser, AppRole>()
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<AdminCredentialsOptions>(
    builder.Configuration.GetSection(AdminCredentialsOptions.SECTION));

builder.Services.AddControllersWithViews();
var mvcBuilder = builder.Services.AddRazorPages();

if (builder.Environment.IsDevelopment())
{
    mvcBuilder.AddRazorRuntimeCompilation();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using var scope = (app as IApplicationBuilder).ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();

var op = scope.ServiceProvider.GetRequiredService<IOptions<AdminCredentialsOptions>>().Value;
var um = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
var rm = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
AppUserTools.CreateInitialIdentity(um, rm, op);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
