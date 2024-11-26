global using Microsoft.EntityFrameworkCore;
using ZetstyBite.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using ZetstyBite.Repositories.Interfaces;
using ZetstyBite.Repositories.Implementations;
using ZetstyBite.Services.Implementations;
using ZetstyBite.Services.Interfaces;
using ZetstyBite.Models.DTOs;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // builder.Services.AddControllersWithViews(); // => support basic MVC
        builder.Services.AddControllers();
        // builder.Services.AddAuthentication().AddJwtBearer();

        var connectionString = builder.Configuration.GetConnectionString("ZestyBiteDb");
        builder.Services.AddDbContext<ZestyBiteDbContext>(options =>
        {
            _ = options.UseMySQL(connectionString);
        });

        //  register
        builder.Services.AddScoped<IAccountService , AccountService>();
        builder.Services.AddScoped<IAccountRepository , AccountRepository>();
        builder.Services.AddScoped<IRoleService, RoleService>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();

        builder.Services.AddEndpointsApiExplorer();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if ( !app.Environment.IsDevelopment() )
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }


        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.MapControllers();
        app.UseRouting();

        // app.UseAuthorization();

        app.MapControllerRoute(
            name: "default" ,
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}

/*
    => 	Program.cs GỌI Controller.cs,
	Controller.cs GỌI Service.cs,
	Service.cs GỌI Model.cs
        services call repos and repos call models
    =>	.............................
 */