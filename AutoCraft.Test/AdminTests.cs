using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using AutoCraft.Domain;
using Xunit;

namespace AutoCraft.Test
{
    public class AdminTests
    {
        [Fact]
        public async Task AddAdmin_SavesAdminToDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase("AdminTestDb")
                .Options;

            using var context = new MyDbContext(options);

            var newAdmin = new Admin
            {
                FullName = "waqas",
                EmailAddress = "waqas@workshop.com",
                AccountPassword = "SecurePassword123"
            };

            // Act
            context.Admins.Add(newAdmin);
            await context.SaveChangesAsync();

            // Assert
            var savedAdmin = await context.Admins.FirstOrDefaultAsync(a => a.EmailAddress == "waqas@workshop.com");

            Assert.NotNull(savedAdmin);
            Assert.Equal("waqas", savedAdmin.FullName);
            Assert.Equal("waqas@workshop.com", savedAdmin.EmailAddress);
            Assert.Equal("SecurePassword123", savedAdmin.AccountPassword);
        }

        [Fact]
        public async Task FindAdminByEmail_ReturnsCorrectAdmin()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase("AdminSearchDb")
                .Options;

            using var context = new MyDbContext(options);

            // Add test data
            context.Admins.AddRange(
                new Admin { FullName = "waqas", EmailAddress = "waqas1@workshop.com", AccountPassword = "password1" },
                new Admin { FullName = "waqas", EmailAddress = "waqas2@workshop.com", AccountPassword = "password2" },
                new Admin { FullName = "waqas", EmailAddress = "waqas3@workshop.com", AccountPassword = "password3" }
            );
            await context.SaveChangesAsync();

            // Act
            var foundAdmin = await context.Admins
                .FirstOrDefaultAsync(a => a.EmailAddress == "waqas2@workshop.com");

            // Assert
            Assert.NotNull(foundAdmin);
            Assert.Equal("waqas", foundAdmin.FullName);
            Assert.Equal("waqas2@workshop.com", foundAdmin.EmailAddress);
        }

        [Fact]
        public async Task GetAllAdmins_ReturnsCorrectCount()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase("AdminCountDb")
                .Options;

            using var context = new MyDbContext(options);

            // Add test data
            context.Admins.AddRange(
                new Admin { FullName = "waqas", EmailAddress = "waqas1@test.com", AccountPassword = "pass1" },
                new Admin { FullName = "waqas", EmailAddress = "waqas2@test.com", AccountPassword = "pass2" },
                new Admin { FullName = "waqas", EmailAddress = "waqas3@test.com", AccountPassword = "pass3" }
            );
            await context.SaveChangesAsync();

            // Act
            var adminCount = await context.Admins.CountAsync();
            var allAdmins = await context.Admins.ToListAsync();

            // Assert
            Assert.Equal(3, adminCount);
            Assert.Equal(3, allAdmins.Count);
            Assert.Contains(allAdmins, a => a.FullName == "waqas");
            Assert.Contains(allAdmins, a => a.FullName == "waqas");
            Assert.Contains(allAdmins, a => a.FullName == "waqas");
        }
    }
}