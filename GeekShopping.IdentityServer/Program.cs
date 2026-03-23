using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Model.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace GeekShopping.IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connection = builder.Configuration
                 .GetSection("SqlServerConnection")
                 .GetValue<string>("SqlServerConnectionString");

            builder.Services.AddDbContext<SqlServerContext>(options =>
                     options.UseSqlServer(connection));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<SqlServerContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddIdentityServer(options =>
               {
                   options.Events.RaiseInformationEvents = true;
                   options.Events.RaiseInformationEvents = true;
                   options.Events.RaiseFailureEvents = true;
                   options.Events.RaiseSuccessEvents = true;
                   options.EmitStaticAudienceClaim = true;
               }).AddInMemoryIdentityResources(
                   IdentityConfiguration.IdentityResources)
                 .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
                 .AddInMemoryClients(IdentityConfiguration.Clients)
                 .AddAspNetIdentity<ApplicationUser>()
                 .AddDeveloperSigningCredential();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
