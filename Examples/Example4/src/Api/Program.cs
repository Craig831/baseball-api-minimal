using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Serilog;

using Api.Data;
using Api.Models;
using Api.Features.Games;
using Api.Features.Teams;

// create logger for application startup. it will be replaced by the application
// logger configured with UseSerilog()
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting application");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // add dbcontext
    builder.Services.AddDbContext<GameDb>(opt => opt.UseInMemoryDatabase("GamesDatabase"));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    // add swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Version = "v1",
            Title = "Schedule API",
            Description = "Simple API solution written in .NET 7 to demonstrate minimal APIs"
        });
    });

    // add serilog
    builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

    var app = builder.Build();

    // seed some teams and games into the in-memory database
    Log.Information("Initializing in-memory database");
    DbInitializer.Initialize(app);

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // add automatic http request logging
    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    Log.Information("Creating route maps");

    GamesMapper.MapRoutes(app);
    TeamsMapper.MapRoutes(app);

    Log.Information("Successfully started...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal("An unhandled exception occured during application startup", ex);
}
finally
{
    Log.CloseAndFlush();
}
