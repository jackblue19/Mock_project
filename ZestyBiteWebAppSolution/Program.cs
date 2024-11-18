using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Repositories.Implementations;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Implementations;
using ZestyBiteWebAppSolution.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Thêm dịch vụ Session
builder.Services.AddDistributedMemoryCache(); // Lưu trữ session trong bộ nhớ
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian session tồn tại
    options.Cookie.HttpOnly = true; // Bảo mật cookie
    options.Cookie.IsEssential = true; // Đảm bảo cookie hoạt động
});

// Thêm Razor Pages và MVC
builder.Services.AddControllers();
builder.Services.AddRazorPages();

// Cấu hình Swagger
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "My API",
        Version = "v1",
        Description = "An API to demonstrate Swagger integration",
    });
});

// Cấu hình chuỗi kết nối MySQL

var connectionString = "server=localhost;user=root;password=hung300403.;database=zestybite";
var serverVersion = ServerVersion.AutoDetect(connectionString);

// Register the DbContext with the DI container.
builder.Services.AddDbContext<ZestybiteContext>(
    dbContextOptions => dbContextOptions
        .UseMySql(connectionString, serverVersion)
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

// Xây dựng ứng dụng
var app = builder.Build();

// Middleware xử lý lỗi
if (app.Environment.IsDevelopment()) {
    app.UseSwagger(); // Kích hoạt Swagger UI
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "swagger"; // Đặt đường dẫn cho Swagger UI
    });
    app.UseDeveloperExceptionPage();
} else {
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Thêm bảo mật HSTS
}

// Middleware bảo mật và xử lý tệp tĩnh
app.UseHttpsRedirection();
app.UseStaticFiles();

// Middleware xử lý Routing
app.UseRouting();

// Thêm Session sau Routing nhưng trước Authorization
app.UseSession();

// Middleware xử lý Authorization
app.UseAuthorization();

// Cấu hình Route cho Areas
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Cấu hình Route mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Chạy ứng dụng
app.Run();