using Budget.Domain;
using Budget.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using static System.Console;
using System;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;

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

            var choice = ReadLine().Trim();

            while (choice != "q")
            {
                switch (choice)
                {
                    case "1":
                        NewUser();
                        return;
                    case "q":
                        return;
                    default:
                        WriteLine("Invalid option");
                        return;
                }
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
                "1. Set up a new user."
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