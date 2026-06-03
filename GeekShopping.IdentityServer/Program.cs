using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Services;
using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.initializer;
using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Model.Context;
using GeekShopping.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Connection String
            var connection = builder.Configuration
                 .GetSection("SqlServerConnection")
                 .GetValue<string>("SqlServerConnectionString");

            builder.Services.AddDbContext<SqlServerContext>(options =>
                     options.UseSqlServer(connection));

            // Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<SqlServerContext>()
                .AddDefaultTokenProviders();

            // IdentityServer
            builder.Services.AddIdentityServer(options =>
            {
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
            .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
            .AddInMemoryClients(IdentityConfiguration.Clients)
            .AddAspNetIdentity<ApplicationUser>()
            .AddDeveloperSigningCredential();

            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            builder.Services.AddScoped<IProfileService, ProfileService>();
            builder.Services.AddControllersWithViews();

            
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                initializer.Initialize();
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}