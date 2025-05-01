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
                var adminUser = new User { UserName = "admin", Email = "admin@example.com", Role = "admin" };
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

            // Типы топлива (FuelTypes)
            var fuelTypes = new List<FuelType>
            {
                new FuelType { Name = "Бензин" },
                new FuelType { Name = "Дизель" },
                new FuelType { Name = "Электричество" },
                new FuelType { Name = "Гибрид" }
            };

            foreach (var fuelType in fuelTypes)
            {
                if (!await context.FuelTypes.AnyAsync(ft => ft.Name == fuelType.Name))
                {
                    await context.FuelTypes.AddAsync(fuelType);
                }
            }

            // Типы привода (DriveTypes)
            var driveTypes = new List<CarDriveType>
            {
                new CarDriveType { Name = "Передний привод" },
                new CarDriveType { Name = "Задний привод" },
                new CarDriveType { Name = "Полный привод" }
            };

            foreach (var driveType in driveTypes)
            {
                if (!await context.DriveTypes.AnyAsync(dt => dt.Name == driveType.Name))
                {
                    await context.DriveTypes.AddAsync(driveType);
                }
            }

            // Типы кузова (BodyTypes)
            var bodyTypes = new List<BodyType>
            {
                new BodyType { Name = "Седан" },
                new BodyType { Name = "Хэтчбек" },
                new BodyType { Name = "Универсал" },
                new BodyType { Name = "Внедорожник" },
                new BodyType { Name = "Купе" },
                new BodyType { Name = "Кабриолет" }
            };

            foreach (var bodyType in bodyTypes)
            {
                if (!await context.BodyTypes.AnyAsync(bt => bt.Name == bodyType.Name))
                {
                    await context.BodyTypes.AddAsync(bodyType);
                }
            }

            // Категории автомобилей (CarCategories)
            var carCategories = new List<CarCategory>
            {
                new CarCategory { Name = "Эконом", Description = "Бюджетные автомобили" },
                new CarCategory { Name = "Стандарт", Description = "Автомобили среднего класса" },
                new CarCategory { Name = "Комфорт", Description = "Комфортабельные автомобили" },
                new CarCategory { Name = "Премиум", Description = "Автомобили премиум-класса" },
                new CarCategory { Name = "Бизнес", Description = "Автомобили для бизнеса" }
            };

            foreach (var category in carCategories)
            {
                if (!await context.CarCategories.AnyAsync(cc => cc.Name == category.Name))
                {
                    await context.CarCategories.AddAsync(category);
                }
            }

            // Характеристики автомобилей (CarFeatures)
            var carFeatures = new List<CarFeature>
            {
                new CarFeature { Name = "Кондиционер" },
                new CarFeature { Name = "Климат-контроль" },
                new CarFeature { Name = "Подогрев сидений" },
                new CarFeature { Name = "Кожаный салон" },
                new CarFeature { Name = "Круиз-контроль" },
                new CarFeature { Name = "Парктроник" },
                new CarFeature { Name = "Камера заднего вида" },
                new CarFeature { Name = "Мультимедиа система" },
                new CarFeature { Name = "Bluetooth" },
                new CarFeature { Name = "Apple CarPlay/Android Auto" },
                new CarFeature { Name = "Ксеноновые фары" },
                new CarFeature { Name = "Светодиодные фары" },
                new CarFeature { Name = "Люк" },
                new CarFeature { Name = "Панорамная крыша" }
            };

            foreach (var feature in carFeatures)
            {
                if (!await context.CarFeatures.AnyAsync(cf => cf.Name == feature.Name))
                {
                    await context.CarFeatures.AddAsync(feature);
                }
            }

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Ошибка при сохранении начальных данных: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                throw; // Пробрасываем исключение дальше
            }

        }
    }
}
