using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MyShop.Core.Interfaces;
using MyShop.Core.Services;
using MyShop.DataLayer.Context;
using MyShop.DataLayer.Interfaces;
using MyShop.DataLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;



var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<MyShop.DataLayer.Context.MyShopDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("MyShopContext") ?? throw new InvalidOperationException("Connection string 'MyShopContext' not found.")));
//builder.Services.AddDbContext<MyShop.Data.MyShopDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("MyShopContext") ?? throw new InvalidOperationException("Connection string 'MyShopContext' not found.")));

//builder.Services.AddDbContext<MyShopContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("MyShopContext") ?? throw new InvalidOperationException("Connection string 'MyShopContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddDbContext<MyShop.DataLayer.Context.MyShopDbContext>(options =>
{
    options.UseSqlServer("Data Source=.;Initial Catalog=MyShopDb;Integrated Security=True;Encrypt=False");
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

}).AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/LogOut";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);

});

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


app.UseAuthentication();
app.UseAuthorization();


    
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    
});

app.Run();
