using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuxNest.Application.Common.Interfaces;
using LuxNest.Application.Common.Utility;
using LuxNest.Domain.Entities;

namespace LuxNest.Infrastructure.Data
{
    public class DbInitializer:IDbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        public DbInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }
        public void Initialize()
        {
            // 1. Apply migrations
            if (_db.Database.GetPendingMigrations().Any())
            {
                _db.Database.Migrate();
            }

            // 2. Create roles if they don't exist
            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
            }

            // 3. Create admin user if it doesn't exist
            if (_userManager.FindByEmailAsync("admin@gmail.com").GetAwaiter().GetResult() == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    Name = "Monica Dra",
                    PhoneNumber = "1122334455",
                    EmailConfirmed = true
                };

                var result = _userManager
                    .CreateAsync(adminUser, "Admin123*")
                    .GetAwaiter()
                    .GetResult();

                if (result.Succeeded)
                {
                    _userManager
                        .AddToRoleAsync(adminUser, SD.Role_Admin)
                        .GetAwaiter()
                        .GetResult();
                }
            }
        }

    }
}
