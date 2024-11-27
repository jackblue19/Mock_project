using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Repositories.Implementations;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Implementations;
using ZestyBiteWebAppSolution.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
// using Microsoft.AspNetCore.Authentication;
using ZestyBiteWebAppSolution.Middlewares;
using Microsoft.Identity;
using ZestyBiteWebAppSolution.Repositories;

/*dotnet add package Microsoft.IdentityModel.Tokens
Install-Package Microsoft.AspNetCore.Session

Install-Package Microsoft.AspNetCore.Authentication.Cookies

Install-Package Microsoft.AspNetCore.Authorization

Install-Package Microsoft.AspNetCore.Mvc

Install-Package Microsoft.Extensions.DependencyInjection

Install-Package Microsoft.AspNetCore.Http

Install-Package Microsoft.AspNetCore.Http.Abstractions
*/

var builder = WebApplication.CreateBuilder(args);

// Configure Session
builder.Services.AddDistributedMemoryCache(); // Store session in memory
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Restaurant.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = false; // => maybe comment vì hình như nó chặn http chỉ cho https => from TRUE to FALSE
    options.Cookie.IsEssential = true;
});

/*
Cấu hình Redirect khi chưa đăng nhập (Login Redirect):
ASP.NET Core sẽ tự động chuyển hướng người dùng đến trang đăng nhập mặc định nếu họ không được xác thực (tức là không có session, không có token hợp lệ, v.v.). Nếu bạn muốn tùy chỉnh trang đăng nhập hoặc thông báo lỗi, bạn có thể cấu hình như sau:

Ví dụ cấu hình Redirect:
csharp
Copy code
public void ConfigureServices(IServiceCollection services)
{
    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login"; // Đường dẫn đến trang đăng nhập
                options.AccessDeniedPath = "/Account/AccessDenied"; // Đường dẫn khi người dùng không có quyền truy cập
            });
}
*/

// Thêm dịch vụ Authorization và tạo các policies phân quyền
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("manager"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("order taker", "staff"));
    options.AddPolicy("ManagerPolicy", policy => policy.RequireRole("manager"));
    options.AddPolicy("OrderTakerPolicy", policy => policy.RequireRole("order taker"));
    options.AddPolicy("StaffPolicy", policy => policy.RequireRole("staff"));
    options.AddPolicy("KitchenPolicy", policy => policy.RequireRole("kitchen"));
    options.AddPolicy("CustomerPolicy", policy => policy.RequireRole("customer"));
    options.AddPolicy("FoodRunnerPolicy", policy => policy.RequireRole("food runner"));
    options.AddPolicy("ServicerPolicy", policy => policy.RequireRole("servicer"));
});

// Add Razor Pages and MVC
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "An API to demonstrate Swagger integration",
    });
});

// Configure MySQL Connection
var connectionString = "Server=Jack-Blue;Port=3306;Database=zestybite;Uid=root;Pwd=123456789";
var serverVersion = ServerVersion.AutoDetect(connectionString);

builder.Services.AddDbContext<ZestyBiteContext>(dbContextOptions =>
    dbContextOptions
        .UseMySql(connectionString, serverVersion)
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);

// Register Repositories and Services in DI
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();

builder.Services.AddEndpointsApiExplorer();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


// Build the app
var app = builder.Build();

// Middleware to handle errors
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger UI in development
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "swagger"; // Route prefix for Swagger UI
    });
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Enable HSTS for production
}

// Middleware for static files and HTTPS redirection
app.UseHttpsRedirection();
app.UseStaticFiles();

// Enable CORS
app.UseCors("AllowAll");

// Middleware for Routing
app.UseRouting();

// Add Session Middleware (after routing and before authorization)
app.UseSession();
app.UseMiddleware<AuthenticationMiddleware>();

//app.MapControllers();

// Middleware for Authorization
app.UseAuthentication();
app.UseAuthorization();

/*      applied for route("/private") in AuthZN
app.MapWhen(context => context.Request.Path.StartsWithSegments("/private"), builder =>
{
    builder.UseAuthentication();
    builder.UseAuthorization();
});
*/

// Configure Routing for Areas => nên sử dụng username thay thế =Đ
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

// Configure Default Routing -> author theo role -> using folder Areas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map Razor Pages
app.MapRazorPages();

// Run the app
app.Run();