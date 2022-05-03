using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using RestWebAppl.Models;
using Newtonsoft.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);

var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
var IdentityConnectionString = builder.Configuration.GetConnectionString("IdentityConnectionString");
// Add services to the container.
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
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddScoped<IRepository,EFRepository>();
builder.Services.AddScoped<IReviewRepository,EFReviewRepository>();
builder.Services.AddTransient<IOrderRepository, EFOrderRepository>();
builder.Services.AddScoped<Cart>(sp=>SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();
builder.Services.AddMvc(options=>options.EnableEndpointRouting=false).AddNewtonsoftJson(options=>options.SerializerSettings.ContractResolver=new DefaultContractResolver());
builder.Services.AddMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStatusCodePages();
app.UseStaticFiles();
app.UseSession();
app.UseAuthentication();
app.UseMvc(routes => {
    routes.MapRoute(
        name: null,
        template: "{category}/Страница{productPage:int}",
        defaults: new { Controller = "Home", action = "Shop" });
    routes.MapRoute(
        name: null,
        template: "",
        defaults: new { Controller = "Home", action = "Index", productPage = 1 });
    routes.MapRoute(
        name: null,
        template: "Страница{productPage:int}",
        defaults: new { Controller = "Home", action = "Index"});
    routes.MapRoute(
        name: null,
        template: "{controller}/{action}/{category}",
        defaults: new { Controller = "Home",action = "Shop",productPage=1 });
    routes.MapRoute(
        name: null,
        template: "{action}/{itemId}",
        defaults: new { Controller = "Order", action = "Item"});
    routes.MapRoute(
        name: null,
        template: "{controller}/{action}",
        defaults: new { Controller = "Home" });

});
//SeedData.FillDb(app);
//IdentitySeedData.EnsurePopulated(app);
app.Run();