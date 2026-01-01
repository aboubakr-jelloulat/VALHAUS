using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using Valhaus.Data.Data;
using Valhaus.Data.DbInitializer;
using Valhaus.Data.Repository.IRepository;
using Valhaus.Data.Repository.Repositories;
using Valhaus.Utils;
using VALHAUS.Areas.Customer.Controllers;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 104857600;
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("constr")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders() ;

// Configure Strip
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));



builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

// Google Authentication
builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});


// DbInitializer
builder.Services.AddScoped<IDbInitializer, DbInitializer>();


builder.Services.AddRazorPages();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IEmailSender, EmailSender>();

// session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// configure Strip Api Keys

//StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:Secretkey").Get<string>();
StripeConfiguration.ApiKey = builder.Configuration["Stripe:Secretkey"];


app.UseRouting();

app.UseAuthentication(); // Required for Identity authentication
app.UseAuthorization();

app.UseSession();

SedDb(); // IDbInitializer

app.MapRazorPages(); // Required for Identity Razor Pages (Login/Register)

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();


void    SedDb()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}
/*
    We create a scope at startup to safely resolve scoped services like DbContext,
    then run a DbInitializer to apply pending migrations and seed required roles before the application starts handling requests.
 
 */