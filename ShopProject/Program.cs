using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
//using Microsoft.Data.Sqlite;
//using SQLitePCL;
using ShopProject.Data;
using ShopProject.Helper;
using ShopProject.Repository;
using ShopProject.Repository.Interfaces;
using ShopProject.Repository.Interfaces.RepositoryInterfaces;
using ShopProject.Services;

var builder = WebApplication.CreateBuilder(args);



var serv = builder.Services;

//var connectionString = builder.Configuration.GetConnectionString("SqliteConnection");
//var c = new SqliteConnection($"DataSource={ApplicationDbContext.DbPath}");
//serv.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(c));


serv.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
serv.AddTransient<ApplicationDbContext>();


//builder.Services.AddDatabaseDeveloperPageExceptionFilter();


serv.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
serv.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICollectionRepository, CollectionRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();




serv.AddScoped<IPropertyRepository, PropertyRepository>();
serv.AddScoped<IPropertyValueRepository, PropertyValueRepository>();



serv.AddSingleton<MegaService>();


//------------------------------
serv.AddDefaultIdentity<User>(config =>
{
    config.Password.RequireDigit = false;
    config.Password.RequireLowercase = false;
    config.Password.RequireNonAlphanumeric = false;
    config.Password.RequireUppercase = false;
    config.Password.RequiredLength = 4;


}).AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


#region  Recomended Identity Options

//serv.Configure<IdentityOptions>(options =>
//{
//    // Password settings.
//    options.Password.RequireDigit = true;
//    options.Password.RequireLowercase = true;
//    options.Password.RequireNonAlphanumeric = true;
//    options.Password.RequireUppercase = true;
//    options.Password.RequiredLength = 6;
//    options.Password.RequiredUniqueChars = 1;

//    // Lockout settings.
//    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
//    options.Lockout.MaxFailedAccessAttempts = 5;
//    options.Lockout.AllowedForNewUsers = true;

//    // User settings.
//    options.User.AllowedUserNameCharacters =
//    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
//    options.User.RequireUniqueEmail = false;
//});
#endregion


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();




serv.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Home/Index";
    config.AccessDeniedPath = "/Home/AccessDenied";
});

serv.AddAuthentication().AddFacebook(config =>
{
    config.AppId = builder.Configuration.GetSection("FacebookAuthentication")["AppId"];
    config.AppSecret = builder.Configuration.GetSection("FacebookAuthentication")["AppSecret"];

});

serv.AddAuthorization(options =>
{
    options.AddPolicy(builder.Configuration["Role:user"], builderPolicy =>
    {
        builderPolicy.RequireClaim(ClaimTypes.Role, builder.Configuration["Role:user"]);
    });

    options.AddPolicy(builder.Configuration["Role:admin"], builderPolicy =>
    {
        builderPolicy.RequireAssertion(x => x.User.HasClaim(ClaimTypes.Role, builder.Configuration["Role:user"]) ||
                                       x.User.HasClaim(ClaimTypes.Role, builder.Configuration["Role:admin"]));
    });


});




var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//app.Services.CreateScope().ServiceProvider.GetService<ApplicationDbContext>().Database.EnsureCreated();
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
