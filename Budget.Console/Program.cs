using Budget.Domain;
using Budget.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;

using static System.Console;
using Budget.Domain.Helpers;
using Budget.Domain.Repos;
using Budget.Domain.Interfaces;
using Budget.Domain.Models.FormObjects;
using Budget.Domain.SearchTools;

namespace Budget.Console
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; private set; }

        static void Main(string[] args)
        {
            Configure();

            WriteLine("What do you need to do?");
            PrintOptions();

            var input = ReadLine();
            var choice = input.Trim();

            while (choice != "q")
            {
                switch (choice)
                {
                    case "1":
                        NewUser();
                        return;
                    case "2":
                        PayMonthly();
                        return;
                    case "3":
                        TaskReward();
                        return;
                    case "q":
                        return;
                    default:
                        WriteLine("Invalid option");
                        return;
                }
            }
        }

        private static void PayMonthly()
        {
            var services = CreateServices();
            var userRepo = ActivatorUtilities.CreateInstance<UserRepo>(services);
            var entryRepo = ActivatorUtilities.CreateInstance<EntryRepo>(services);
            var strongParams = new string[] { "Payee", "Category", "Notes", "Price", "EntryDate", "UserId" };

            var users = Task.Run(async() => await userRepo.GetList()).Result;
            foreach (var user in users)
            {
                var entry = new Entry
                {
                    UserId = user.Id,
                    Price = -100m,
                    Payee = user.UserName,
                    Category = "Allowance",
                    EntryDate = DateTime.Now,
                    Notes = $"{DateTime.Now.ToString("MMMM")} Allowance"
                };
                entryRepo.Create(entry, strongParams).Wait();
                WriteLine($"Allowance paid to {user.UserName}");
            }
        }

        private static void TaskReward()
        {
            var services = CreateServices();
            var userRepo = ActivatorUtilities.CreateInstance<UserRepo>(services);
            var entryRepo = ActivatorUtilities.CreateInstance<EntryRepo>(services);
            var taskRepo = ActivatorUtilities.CreateInstance<TaskRepo>(services);
            var strongParams = new string[] { "Payee", "Category", "Notes", "Price", "EntryDate", "UserId" };

            var users = Task.Run(async() => await userRepo.GetList()).Result;
            foreach (var user in users)
            {
                var task = Task.Run(async () => {
                    var tasks = await taskRepo.GetList(t => t.UserId == user.Id);
                    return tasks.FirstOrDefault();
                }).Result;
                if (task == null)
                {
                    continue;
                }
                var completedCount = task.ToDaysCompleted().Count;

                // Reset task and save before continuing
                task.Days = 0;
                taskRepo.Update(task.Id, task).Wait();

                if (completedCount < 4)
                {
                    continue;
                }

                // Reward percentage of $10 based on number of completed days
                var payment = Math.Round(10m * (completedCount / 7m), 2);

                var entry = new Entry
                {
                    UserId = user.Id,
                    Price = -payment,
                    Payee = user.UserName,
                    Category = "Task",
                    EntryDate = DateTime.Now,
                    Notes = "Weekly Task Completed"
                };
                entryRepo.Create(entry, strongParams).Wait();
                WriteLine($"{payment} paid to {user.UserName} for weekly task completion");
            }

        }

        private static void NewUser()
        {
            var services = CreateServices();
            WriteLine("Email:");
            var email = ReadLine().Trim();
            WriteLine("Password:");
            var password = ReadLine().Trim();
            var user = new ApplicationUser { UserName = email, Email = email };
            var userManager = ActivatorUtilities.CreateInstance<UserManager<ApplicationUser>>(services);
            var result = Task.Run(async () => await userManager.CreateAsync(user, password)).Result;
            if (result.Succeeded)
            {
                WriteLine("Success! User created");
                return;
            }
            throw new Exception($"User creation failed because: {result.Errors.First().Description}");
        }

        private static void PrintOptions()
        {
            var options = new string[]
            {
                "1. Set up a new user.",
                "2. Pay monthly allowance to all users.",
                "3. Pay weekly task reward to all users."
            };
            foreach (var option in options)
            {
                WriteLine(option);
            }
        }

        private static IServiceProvider CreateServices()
        {
            var serviceCollection = new ServiceCollection();
            // Add services to collection here
            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            serviceCollection.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            serviceCollection.AddTransient(typeof(RepoHelper<>));
            serviceCollection.AddScoped<UserRepo>();
            serviceCollection.AddScoped<ISearchTool<Entry, EntrySearchFormObject>, EntrySearch>();
            serviceCollection.AddScoped<ISearchableRepo<Entry, EntrySearchFormObject>, EntryRepo>();
            serviceCollection.AddScoped<IRepo<AllowanceTask>, TaskRepo>();

            return serviceCollection.BuildServiceProvider();
        }

        private static void Configure()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }
    }
}