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
            //await context.Database.MigrateAsync();

            // Проверяем наличие ролей
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            // Проверяем наличие пользователей
            if (!userManager.Users.Any())
            {
                var adminUser = new User
                {
                    UserName = "admin",
                    Email = "admin@example.com"
                    
                };
                var userCreationResult = await userManager.CreateAsync(adminUser, "Admin@123");
                if (userCreationResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "admin");
                }

                var normalUser = new User { UserName = "user", Email = "user@example.com" };
                var normalUserCreationResult = await userManager.CreateAsync(normalUser, "User@123");
                if (normalUserCreationResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(normalUser, "user");
                }
            }

            //// Добавляем тестовые данные проектов и задач
            //if (!context.Projects.Any())
            //{
            //    var project = new Project
            //    {
            //        Title = "МедАналог",
            //        Description = "веб-приложение, которое помогает пользователям находить аналоги лекарств по действующему веществу, составу, цене или отзывам. Сервис будет полезен людям, желающим найти более доступные или подходящие по индивидуальным требованиям аналоги медикаментов. В приложение будут встроены удобный поиск, фильтры, отзывы, а также информация о доступности в аптеках",
            //        DueDate = DateTime.Now.AddDays(30),
            //        Tasks = new List<Domain.Entities.AppTask>
            //        {
            //            new Domain.Entities.AppTask { Title = "Разработка структуры базы данных", Description = "Создать базу данных для хранения информации о медикаментах, аналогах, действующих веществах, ценах, производителях и отзывах." },
            //            new Domain.Entities.AppTask { Title = "Интеграция API с базами лекарств", Description = "Подключить приложение к открытым API или локальным базам данных с актуальной информацией о наличии и ценах на лекарства." },
            //            new Domain.Entities.AppTask { Title = "Создание интерфейса поиска и фильтров", Description = "Разработать удобную поисковую строку и фильтры для отображения результатов поиска по категориям, цене, составу и противопоказаниям." },
            //            new Domain.Entities.AppTask { Title = "Разработка алгоритма подбора аналогов", Description = "Реализовать алгоритм, который анализирует действующее вещество и предлагает доступные аналоги с учетом указанных пользователем критериев." },
            //            new Domain.Entities.AppTask { Title = "Дизайн пользовательского интерфейса (UI/UX)", Description = "Создать интуитивно понятный и современный интерфейс для комфортного использования приложения пользователями любого уровня подготовки." },
            //            new Domain.Entities.AppTask { Title = "Система отзывов и рейтинга лекарственных средств", Description = "Внедрить функционал, позволяющий пользователям оставлять отзывы, выставлять оценки и делиться опытом использования препаратов." },
            //            new Domain.Entities.AppTask { Title = "Функция сравнения лекарств", Description = "Добавить возможность сравнивать лекарства по цене, качеству, стране производства и другим параметрам." },
            //            new Domain.Entities.AppTask { Title = "Модуль рекомендаций для пользователей", Description = "Создать модуль рекомендаций, который предложит пользователю аналоги на основе истории поиска и его предпочтений." },
            //            new Domain.Entities.AppTask { Title = "Мобильная адаптация и разработка PWA", Description = "Сделать приложение адаптивным для использования на мобильных устройствах, а также обеспечить возможность работы как PWA (прогрессивное веб-приложение)." },
            //            new Domain.Entities.AppTask { Title = "Тестирование и отладка функционала", Description = "Провести тщательное тестирование всех функций приложения, устранить ошибки и оптимизировать производительность перед запуском." },
            //        }
            //    };

            //    var project2 = new Project
            //    {
            //        Title = "Example Project 2",
            //        Description = "This is an example project.",
            //        DueDate = DateTime.Now.AddDays(30),
            //        Tasks = new List<Domain.Entities.AppTask>
            //        {
            //            new Domain.Entities.AppTask { Title = "Task 1", Description = "First task." },
            //            new Domain.Entities.AppTask { Title = "Task 2", Description = "Second task." },
            //        }
            //    };
            //    var project3 = new Project
            //    {
            //        Title = "Example Project 2",
            //        Description = "This is an example project.",
            //        DueDate = DateTime.Now.AddDays(30),
            //        Tasks = new List<Domain.Entities.AppTask>()
            //    };
            //    context.Projects.Add(project);
            //    context.Projects.Add(project2);
            //    context.Projects.Add(project3);
            //    await context.SaveChangesAsync();
            //}
        }
    }
}
