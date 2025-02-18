using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SamaritanAPI.Authentication;

namespace SamaritanAPI.Services
{
    public class SeedingServices
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public SeedingServices(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public async Task SeedingRolesAsync()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                var _services = serviceScope.ServiceProvider;
                var roleManager = _services.GetRequiredService<RoleManager<IdentityRole>>();
                if(!await roleManager.RoleExistsAsync(AppRoles.Administrator))
                    await roleManager.CreateAsync(new IdentityRole(AppRoles.Administrator));
                if(!await roleManager.RoleExistsAsync(AppRoles.SubLeader))
                    await roleManager.CreateAsync(new IdentityRole(AppRoles.SubLeader));
                if(!await roleManager.RoleExistsAsync(AppRoles.ServantDialler))
                    await roleManager.CreateAsync(new IdentityRole(AppRoles.ServantDialler));
            }
        }
    }
}