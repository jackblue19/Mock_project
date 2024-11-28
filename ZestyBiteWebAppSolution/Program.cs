using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ZestyBiteWebAppSolution.Data;
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
    options.Cookie.Name = ".Restaurant.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = false; // => maybe comment vì hình như nó chặn http chỉ cho https => from TRUE to FALSE
    options.Cookie.IsEssential = true;
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
builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.WriteIndented = true;
    });
builder.Services.AddRazorPages();
// Configure Swagger
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "My API",
        Version = "v1",
        Description = "An API to demonstrate Swagger integration",
    });
});

// Configure MySQL Connection
var connectionString = "Server=localhost;Port=3306;Database=zestybite;Uid=root;Pwd=hung300403.";
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
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
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

app.MapControllers();

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