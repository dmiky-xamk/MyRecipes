using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyRecipes.Infrastructure.Identity;
using System.Reflection;
using MyRecipes.Application.Infrastructure.Persistence;

namespace MyRecipes.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly string _connectionString;
    
    public ApplicationDbContext(DbContextOptions options, IDbConnectionFactory dbConnectionFactory) 
        : base(options)
    {
        _connectionString = dbConnectionFactory.ConnectionString;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString, builder =>
        {
            builder.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
        });
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}