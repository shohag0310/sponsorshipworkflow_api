using SponsorshipWorkflow.Domain.Entities;
using SponsorshipWorkflow.Domain.Enums;
using SponsorshipWorkflow.Infrastructure.Services;

namespace SponsorshipWorkflow.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (!context.Users.Any())
        {
            var users = new List<User>
            {
                new()
                {
                    Name = "Requestor User",
                    Email = "requestor@test.com",
                    PasswordHash = PasswordService.Hash("Password123!"),
                    Role = UserRole.Requestor
                },

                new()
                {
                    Name = "Manager User",
                    Email = "manager@test.com",
                    PasswordHash = PasswordService.Hash("Password123!"),
                    Role = UserRole.Manager
                },

                new()
                {
                    Name = "Finance Admin",
                    Email = "finance@test.com",
                    PasswordHash = PasswordService.Hash("Password123!"),
                    Role = UserRole.FinanceAdmin
                },

                new()
                {
                    Name = "System Admin",
                    Email = "admin@test.com",
                    PasswordHash = PasswordService.Hash("Password123!"),
                    Role = UserRole.SystemAdmin
                }
            };

            await context.Users.AddRangeAsync(users);
        }

        if (!context.SponsorshipTypes.Any())
        {
            await context.SponsorshipTypes.AddRangeAsync(
                new SponsorshipType { Name = "Conference" },
                new SponsorshipType { Name = "Community Event" },
                new SponsorshipType { Name = "Marketing Event" }
            );
        }

        await context.SaveChangesAsync();
    }
}