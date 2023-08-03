using Microsoft.AspNetCore.Authentication.Facebook;
using Fitness1919.Data;
using Fitness1919.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Fitness1919.Services.Data.Interfaces;
using Fitness1919.Services.Data;
using Microsoft.AspNetCore.Mvc;
using static Fitness1919.Common.GeneralApplicationConstants;
using Fitness1919.Web.Infrastructure.ModelBinders;
using Fitness1919.Web.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<Fitness1919DbContext>(options =>
{
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Fitness1919.Data"));
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;

})
     .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<Fitness1919DbContext>();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services
                .AddControllersWithViews()
                .AddMvcOptions(options =>
                {
                    options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
                    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
                });
builder.Services.AddRecaptchaService();

builder.Services.ConfigureApplicationCookie(cfg =>
{
    cfg.LoginPath = "/User/Login";
});

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.SeedAdministrator(AdminEmail);
}

app.UseEndpoints(config =>
{
    config.MapControllerRoute(
        name: "ProtectingUrlRoute",
        pattern: "/{controller}/{action}/{id}/{information}",
        defaults: new { Controller = "Brand", Action = "Index" });
    config.MapDefaultControllerRoute();
    config.MapRazorPages();
});

app.Run();