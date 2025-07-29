using ButterflyCinema.Context;
using ButterflyCinema.Models;
using Microsoft.EntityFrameworkCore;
using ButterflyCinema.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ButterflycinemaContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("CinemaDb"));
});

// Đăng ký EmailService
builder.Services.AddTransient<EmailService>();

// Thêm dịch vụ MemoryCache (cần cho Session mặc định)
builder.Services.AddDistributedMemoryCache();

// Thêm dịch vụ Session và cấu hình
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian session hết hạn sau 30 phút không hoạt động
    options.Cookie.HttpOnly = true; // Cookie chỉ truy cập qua HTTP, không qua client-side script
    options.Cookie.IsEssential = true; // Đặt là thiết yếu nếu bạn cần GDPR compliance
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

app.UseSession(); // Sử dụng Session trước khi xác thực người dùng

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
