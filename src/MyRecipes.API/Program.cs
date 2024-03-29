using MyRecipes.API.Extensions;
using MyRecipes.API.Mapping;
using MyRecipes.Application;
using MyRecipes.Application.Infrastructure;
using MyRecipes.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// TODO: Replace IConfiguration with IOptions.

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .WithOrigins("http://localhost:3000", "https://my-recipes-etwl.onrender.com");
    });
});

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddAPIServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<IApplicationDbContextInitializer>();
    await initializer.InitializeAsync();
    await initializer.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt => 
    {
        // Persist the authorization through restarts.
        // https://github.com/TryCatchLearn/Restore/blob/0da0ea7fcab7d6d4d1ba79b5d37c1fdab0cd8927/API/Program.cs
        opt.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
    });

    app.ConfigureExceptionHandler(includeDetails: true);
} 
else
{
    app.ConfigureExceptionHandler(includeDetails: false);
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }