using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using HotelManagementSystem;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

// 配置EF Core使用MySQL
builder.Services.AddDbContext<HotelManagementContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddDbContext<UserContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddIdentity<HotelStuff, IdentityRole>(options =>
    options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<UserContext>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("经理或管理员", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(claim =>
                (claim.Type == "stuff_role" && (claim.Value == "管理员" || claim.Value == "经理")))));
});

builder.Services.AddDistributedMemoryCache(); // 使用内存缓存存储会话
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2); // 设置会话超时时间
    options.Cookie.HttpOnly = true; // 增加安全性
    options.Cookie.IsEssential = true; // 标记为必要Cookie
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
