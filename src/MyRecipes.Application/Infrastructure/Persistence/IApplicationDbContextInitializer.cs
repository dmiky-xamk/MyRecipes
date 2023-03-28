namespace MyRecipes.Infrastructure.Persistence;

public interface IApplicationDbContextInitializer
{
    Task InitializeAsync();
    Task SeedAsync();
}