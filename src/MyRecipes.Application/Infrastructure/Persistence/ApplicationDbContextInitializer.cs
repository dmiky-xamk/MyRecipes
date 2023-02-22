using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyRecipes.Infrastructure.Identity;

namespace MyRecipes.Infrastructure.Persistence;

public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger,
        ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public async Task InitializeAsync()
    {
        try
        {
            if (_context.Database.IsSqlite())
            {
                await _context.Database.MigrateAsync();
            }
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while initializing the database.");
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while seeding the database.");
        }
    }

    public async Task TrySeedAsync()
    {
        // Default user
        ApplicationUser adminUser = new()
        {
            Email = "admin@localhost.com"
        };

        if (_userManager.Users.All(u => u.Email != adminUser.Email))
        {
            await _userManager.CreateAsync(adminUser, "Admin123");
        }

        // Seed recipes

        await _context.SaveChangesAsync();
    }
}