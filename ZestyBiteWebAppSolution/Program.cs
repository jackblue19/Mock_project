using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using ZestyBiteWebAppSolution.Data;
using ZestyBiteWebAppSolution.Models.Entities;
using ZestyBiteWebAppSolution.Repositories;
using ZestyBiteWebAppSolution.Repositories.Implementations;
using ZestyBiteWebAppSolution.Repositories.Interfaces;
using ZestyBiteWebAppSolution.Services.Implementations;
using ZestyBiteWebAppSolution.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Configure MySQL Connection
var connectionString = "Server=localhost;Port=3306;Database=zestybite;Uid=root;Pwd=123456";
var serverVersion = ServerVersion.AutoDetect(connectionString);

builder.Services.AddDbContext<ZestyBiteContext>(dbContextOptions =>
    dbContextOptions
        .UseMySql(connectionString, serverVersion)
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);

// Configure Session
builder.Services.AddDistributedMemoryCache(); // Store session in memory
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout duration
    options.Cookie.HttpOnly = true; // Secure cookie
    options.Cookie.IsEssential = true; // Essential cookie
});

// Add Razor Pages and MVC
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//  4 dòng code dưới để có thể return về 1 Entity mà không cần chuyển sang DTO
//  -> tuy nhiên rủi ro kha cao
//  => nên kết hợp cùng [JsonIgnore] có thể tham khảo ở Entity Role (class) <4 loc>
/*builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });*/


// Configure Swagger
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "My API",
        Version = "v1",
        Description = "An API to demonstrate Swagger integration",
    });
});


// Register Repositories and Services in DI
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IRoleService , RoleService>();
builder.Services.AddScoped<IRoleRepository , RoleRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
//builder.Services.AddScoped<IItemService, ItemService>();


//adding services before run
builder.Services.AddEndpointsApiExplorer();

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
//app.MapControllers();

// Middleware for Authorization
app.UseAuthorization();

// Configure Routing for Areas
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

// Configure Default Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map Razor Pages
app.MapRazorPages();

// Run the app
app.Run();