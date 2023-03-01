using MyRecipes.Api;
using MyRecipes.Application;
using MyRecipes.Infrastructure;
using MyRecipes.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// TODO: Replace IConfiguration with IOptions.
// TODO: Learn to write more tests.

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
builder.Services.AddInfrasctuctureServices(builder.Configuration);
builder.Services.AddAPIServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
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
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
