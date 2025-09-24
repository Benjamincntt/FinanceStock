using FinanceStock;
using FinanceStock.Models;
using FinanceStock.Services;
using Hangfire;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Thêm dịch vụ
builder.Services.AddControllers();
builder.Services.AddDbContext<StockDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpClient<StockDataService>();
builder.Services.AddHttpClient<GrokService>();
builder.Services.AddScoped<StockAnalyzer>();
builder.Services.AddScoped<StockFilterService>();
builder.Services.AddScoped<StockFilterJob>();

// Cấu hình Hangfire với SQL Server
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

var app = builder.Build();

// Cấu hình middleware
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseHangfireDashboard(); // Giao diện quản lý job tại /hangfire

// Lên lịch job hàng ngày
RecurringJob.AddOrUpdate<StockFilterJob>(
    job => job.RunDaily(),
    "0 15 * * *"); // Chạy lúc 3 PM hàng ngày (giờ VN, phù hợp HOSE)

app.Run();