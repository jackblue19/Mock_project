using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Helpers;
using ZestyBiteWebAppSolution.Mappings;
using ZestyBiteWebAppSolution.Middlewares;
using ZestyBiteWebAppSolution.Repositories;
using ZestyBiteWebAppSolution.Repositories.Implementations;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Implementations;
using ZestyBiteWebAppSolution.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configure Session
builder.Services.AddDistributedMemoryCache(); // Store session in memory
builder.Services.AddSession(options => {
    options.Cookie.Name = ".ZestyBite.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = false;
});

// Thêm dịch vụ Authorization và tạo các policies phân quyền
builder.Services.AddAuthorization(options => {
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
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "My API",
        Version = "v1",
        Description = "An API to demonstrate Swagger integration",
    });
});
builder.Services.AddAuthorization();

//VNPay
builder.Services.AddSingleton<IVnPayService, VnPayService>();

//  Email sender
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Configure MySQL Connection
var connectionString = builder.Configuration.GetConnectionString("ZestyBiteDb");
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
builder.Services.AddScoped<IItemService, ItemService>();

builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<ITableService, TableService>();

builder.Services.AddScoped<ITableDetailRepository, TableDetailRepository>();
builder.Services.AddScoped<ITableDetailService, TableDetailService>();

builder.Services.AddScoped<ISupplyRepository, SupplyRepository>();
builder.Services.AddScoped<ISupplyService, SupplyService>();

builder.Services.AddScoped<ISupplyItemService, SupplyItemService>();
builder.Services.AddScoped<ISupplyItemRepository, SupplyItemRepository>();


builder.Services.AddScoped<IVerifyService, VerifySerivce>();
builder.Services.AddScoped<IBillRepository, BillRepository>();
builder.Services.AddScoped<IBillRepository, BillRepository>();
builder.Services.AddScoped<IVerifyService, VerifySerivce>();



builder.Services.AddEndpointsApiExplorer();

// Add AutoMapper services
builder.Services.AddAutoMapper(typeof(MappingProfile));


// Configure CORS
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Build the app
var app = builder.Build();

// Middleware to handle errors
if (app.Environment.IsDevelopment()) {
    app.UseSwagger(); // Enable Swagger UI in development
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "swagger"; // Route prefix for Swagger UI
    });
    app.UseDeveloperExceptionPage();
} else {
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

// Middleware for Authorization
app.UseAuthentication();
app.UseAuthorization();

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

