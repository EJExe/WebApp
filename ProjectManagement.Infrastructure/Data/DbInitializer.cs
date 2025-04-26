using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Infrastructure.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
                await roleManager.CreateAsync(new IdentityRole("Client"));
            }

            if (!userManager.Users.Any())
            {
                var adminUser = new User{ UserName = "admin", Email = "admin@example.com", Role = "admin" };
                var userCreationResult = await userManager.CreateAsync(adminUser, "Admin@123");
                if (userCreationResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "admin");
                }

                var normalUser = new User { UserName = "user", Email = "user@example.com", Role = "Client" };
                var normalUserCreationResult = await userManager.CreateAsync(normalUser, "User@123");
                if (normalUserCreationResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(normalUser, "Client");
                }
            }

        }
    }
}
