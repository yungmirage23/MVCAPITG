using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using RestWebAppl.Models;
using Newtonsoft.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);

var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
var IdentityConnectionString = builder.Configuration.GetConnectionString("IdentityConnectionString");

// Add services to the container.

// Added DbContexts | AppIdentityDbContext= UsersDB, ApplicationDbContext= Orders and Items DB
builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(IdentityConnectionString));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
{
    opts.Password.RequiredLength = 8;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireDigit = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(ConnectionString));
builder.Services.AddScoped<IRepository, EFRepository>();
builder.Services.AddScoped<IReviewRepository, EFReviewRepository>();
builder.Services.AddTransient<IOrderRepository, EFOrderRepository>();
// Added Shoping Cart services using Sessions
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// Used NewtonsoftJson lib to serialize and deserialize objects 
builder.Services.AddMvc(options=>options.EnableEndpointRouting=false).AddNewtonsoftJson(options=>options.SerializerSettings.ContractResolver=new DefaultContractResolver());


var app = builder.Build();
app.UseStatusCodePages();
app.UseStaticFiles();
app.UseSession();
app.UseAuthentication();
app.UseMvc(routes => {
    routes.MapRoute(
        name: null,
        template: "{category}/Страница{productPage:int}",
        defaults: new { Controller = "Home", action = "Index" });
    routes.MapRoute(
        name: null,
        template: "",
        defaults: new { Controller = "Home", action = "Index", productPage = 1 });
    routes.MapRoute(
        name: null,
        template: "Страница{productPage:int}",
        defaults: new { Controller = "Home", action = "Index",category=string.Empty});
    routes.MapRoute(
        name: null,
        template: "{action}/{itemId}",
        defaults: new { Controller = "Order", action = "Item"});
    routes.MapRoute(
        name: null,
        template: "{controller}/{action}/{id?}");
});
app.Run();