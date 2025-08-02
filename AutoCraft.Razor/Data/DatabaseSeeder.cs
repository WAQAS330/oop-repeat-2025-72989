using AutoCraft.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AutoCraft.Razor.Data;

public static class AutoCraftRoles
{
    public const string Admin = "Admin";
    public const string Mechanic = "Mechanic";
    public const string Customer = "Customer";
}

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<MyDbContext>();

            // Check if any data exists
            var hasAnyData = await context.Admins.AnyAsync() ||
                             await context.Mechanics.AnyAsync() ||
                             await context.Customers.AnyAsync();

            if (!hasAnyData)
            {
                Console.WriteLine("No existing data found. Starting database seeding...");
                await AddDefaultAdminAndMechanics(context);
                await AddDefaultCustomers(context);
                Console.WriteLine("Database seeding completed successfully.");
            }
            else
            {
                Console.WriteLine("Database already contains data. Skipping seeding.");
            }
        }
    }

    private static async Task AddDefaultAdminAndMechanics(MyDbContext context)
    {
        Console.WriteLine("Creating default admin and mechanics...");

        // Add Admin
        if (!await context.Admins.AnyAsync(a => a.EmailAddress == "admin@carservice.com"))
        {
            Console.WriteLine("Creating admin user...");
            await context.Admins.AddAsync(new Admin
            {
                FullName = "Admin",
                EmailAddress = "admin@carservice.com",
                AccountPassword = "Dorset001^"
            });
        }

        // Add Mechanics
        if (!await context.Mechanics.AnyAsync(m => m.EmailAddress == "mechanic1@carservice.com"))
        {
            Console.WriteLine("Creating mechanic 1...");
            await context.Mechanics.AddAsync(new Mechanic
            {
                FullName = "Mechanic 1",
                EmailAddress = "mechanic1@carservice.com",
                AccountPassword = "Dorset001^"
            });
        }

        if (!await context.Mechanics.AnyAsync(m => m.EmailAddress == "mechanic2@carservice.com"))
        {
            Console.WriteLine("Creating mechanic 2...");
            await context.Mechanics.AddAsync(new Mechanic
            {
                FullName = "Mechanic 2",
                EmailAddress = "mechanic2@carservice.com",
                AccountPassword = "Dorset001^"
            });
        }

        await context.SaveChangesAsync();
        Console.WriteLine("Admin and mechanics created successfully.");
    }

    private static async Task AddDefaultCustomers(MyDbContext context)
    {
        Console.WriteLine("Creating default customers...");

        // Add Customer 1
        if (!await context.Customers.AnyAsync(c => c.EmailAddress == "customer1@carservice.com"))
        {
            Console.WriteLine("Creating customer 1...");
            await context.Customers.AddAsync(new Customer
            {
                FullName = "Customer1",
                EmailAddress = "customer1@carservice.com",
                AccountPassword = "Dorset001^"
            });
        }

        // Add Customer 2
        if (!await context.Customers.AnyAsync(c => c.EmailAddress == "customer2@carservice.com"))
        {
            Console.WriteLine("Creating customer 2...");
            await context.Customers.AddAsync(new Customer
            {
                FullName = "Customer2",
                EmailAddress = "customer2@carservice.com",
                AccountPassword = "Dorset001^"
            });
        }

        await context.SaveChangesAsync();

        // Add car for Customer 1
        var customer1 = await context.Customers.FirstOrDefaultAsync(c => c.EmailAddress == "customer1@carservice.com");
        if (customer1 != null && !await context.Cars.AnyAsync(c => c.LicensePlateNumber == "ABC123"))
        {
            Console.WriteLine("Adding car for customer 1...");
            await context.Cars.AddAsync(new Car
            {
                LicensePlateNumber = "ABC123",
                VehicleOwnerId = customer1.CustomerIdentifier
            });
            await context.SaveChangesAsync();
        }

        Console.WriteLine("Customers and car created successfully.");
    }
} 