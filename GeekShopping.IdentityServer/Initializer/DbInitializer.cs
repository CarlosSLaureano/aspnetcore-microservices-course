using Duende.IdentityModel;
using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Model.Context;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GeekShopping.IdentityServer.initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly SqlServerContext _contex;
        private readonly UserManager<ApplicationUser> _user;
        private readonly RoleManager<IdentityRole> _role;

        public DbInitializer(SqlServerContext contex, 
            UserManager<ApplicationUser> user, 
            RoleManager<IdentityRole> role)
        {
            _contex = contex;
            _user = user;
            _role = role;
        }

        public void Initialize()
        {
            if (_role.FindByNameAsync(IdentityConfiguration.Admin).Result != null) return;
            _role.CreateAsync(new IdentityRole(
                IdentityConfiguration.Admin)).GetAwaiter().GetResult();
            _role.CreateAsync(new IdentityRole(
                IdentityConfiguration.Client)).GetAwaiter().GetResult();

            ApplicationUser admin = new ApplicationUser()
            {
                UserName = "carlos-admin",
                Email = "carlos-admin@souza.com.br",
                EmailConfirmed = true,
                PhoneNumber = "+55 (21) 123455678",
                FirstName = "Carlos",
                LastName = "Admin"
            };

            _user.CreateAsync(admin, "Souza123$").GetAwaiter().GetResult();
            _user.AddToRoleAsync(admin,
                IdentityConfiguration.Admin).GetAwaiter().GetResult();
            var adminClaims = _user.AddClaimsAsync(admin, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                new Claim(JwtClaimTypes.GivenName, admin.FirstName),
                new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin)
            }).Result;

            ApplicationUser client = new ApplicationUser()
            {
                UserName = "carlos-client",
                Email = "carlos-client@souza.com.br",
                EmailConfirmed = true,
                PhoneNumber = "+55 (21) 123455678",
                FirstName = "Carlos",
                LastName = "Client"
            };

            _user.CreateAsync(client, "CSouza123$").GetAwaiter().GetResult();
            _user.AddToRoleAsync(client,
                IdentityConfiguration.Client).GetAwaiter().GetResult();
            var clientClaims = _user.AddClaimsAsync(client, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{client.FirstName} {client.LastName}"),
                new Claim(JwtClaimTypes.GivenName, client.FirstName),
                new Claim(JwtClaimTypes.FamilyName, client.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Client)
            }).Result;
        }
    }
}
