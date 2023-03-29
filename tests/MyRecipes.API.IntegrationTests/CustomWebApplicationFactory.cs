using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipes.Infrastructure.Persistence;
using Npgsql;
using System.Data.Common;
using Testcontainers.PostgreSql;

namespace MyRecipes.API.IntegrationTests;

/// <summary>
/// Creates an in-memory version of the API to be tested.
/// </summary>
public sealed class CustomWebApplicationFactory
    : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer =
        new PostgreSqlBuilder()
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");

        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            var integrationConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            configurationBuilder.AddConfiguration(integrationConfig);
        });

        builder.ConfigureTestServices(services =>
        {
            // Remove 'ApplicationDbContext' so we can replace it with the 'TestContainers' docker implementation.
            var dbContextDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (dbContextDescriptor is not null)
                services.Remove(dbContextDescriptor);

            // Remove 'IApplicationDbContextInitializer' so it can be replaced with the one meant for test db seeding.
            var dbContextInitializerDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IApplicationDbContextInitializer));
            if (dbContextInitializerDescriptor is not null)
                services.Remove(dbContextInitializerDescriptor);

            services.AddScoped<IApplicationDbContextInitializer, TestApplicationDbContextInitializer>();
            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseNpgsql(_postgreSqlContainer.GetConnectionString());
            });
        });
    }

    public async Task ResetDatabaseAsync()
    {
        using (DbConnection connection = new NpgsqlConnection(_postgreSqlContainer.GetConnectionString()))
        {
            using (DbCommand command = new NpgsqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"DO $$ DECLARE
                    r RECORD;
                BEGIN
                    FOR r IN (SELECT tablename FROM pg_tables WHERE schemaname = current_schema()) LOOP
                        EXECUTE 'TRUNCATE TABLE ' || quote_ident(r.tablename) || ' CASCADE;';
                    END LOOP;
                END $$;";

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync().AsTask();
    }
}