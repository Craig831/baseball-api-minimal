using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Serilog;

using Api.Data;
using Api.Models;

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

    Log.Information("Creating endpoints");

    app.MapGet("/games", async (GameDb db) =>
        await db.Games.ToListAsync())
        .WithOpenApi(op =>
        {
            op.Summary = "Retrieve all games";
            return op;
        });

    app.MapGet("/games/final", async (GameDb db) =>
        await db.Games.Where(g => g.IsFinal).ToListAsync())
        .WithOpenApi(op =>
        {
            op.Summary = "Retrieve games with final scores";
            return op;
        });

    app.MapGet("/games/{id}", async (int id, GameDb db) =>
        await db.Games.FindAsync(id)
            is Game game
                ? Results.Ok(game)
                : Results.NotFound())
        .WithOpenApi(op =>
        {
            op.Summary = "Retrieve one game by game id";
            return op;
        });

    app.MapPost("/games", async (Game game, GameDb db) =>
    {
        db.Games.Add(game);
        await db.SaveChangesAsync();

        return Results.Created($"/games/{game.Id}", game);
    })
    .WithOpenApi(op =>
    {
        op.Summary = "Create a new game";
        return op;
    });

    app.MapPut("/games/{id}", async (int id, Game updatedGame, GameDb db) =>
    {
        Game existingGame = await db.Games.FindAsync(id);

        if (existingGame is null) return Results.NotFound();

        existingGame.GameDateTime = updatedGame.GameDateTime;
        existingGame.HomeTeamId = updatedGame.HomeTeamId;
        existingGame.AwayTeamId = updatedGame.AwayTeamId;
        existingGame.HomeTeamScore = updatedGame.HomeTeamScore;
        existingGame.AwayTeamScore = updatedGame.AwayTeamScore;
        existingGame.IsFinal = updatedGame.IsFinal;

        await db.SaveChangesAsync();

        return Results.NoContent();
    })
    .WithOpenApi(op =>
    {
        op.Summary = "Update an existing game";
        return op;
    });

    app.MapPatch("/games/{id}", async (int id, ScoreUpdateDTO scoreUpdate, GameDb db) =>
    {
        Game existingGame = await db.Games.FindAsync(id);

        if (existingGame is null) return Results.NotFound();

        existingGame.HomeTeamScore = scoreUpdate.HomeTeamScore;
        existingGame.AwayTeamScore = scoreUpdate.AwayTeamScore;
        existingGame.IsFinal = scoreUpdate.IsFinal;

        await db.SaveChangesAsync();

        return Results.NoContent();
    }).AddEndpointFilter(async (invocationContext, next) =>
    {
        var update = invocationContext.GetArgument<ScoreUpdateDTO>(1);

        if (update.IsFinal && update.AwayTeamScore.Equals(update.HomeTeamScore))
        {
            Log.Information("Home and away scores cannot be the same if the game is a final.");
            return Results.Problem("Home and away scores cannot be the same if the game is a final.");
        }
        return await next(invocationContext);
    })
    .WithOpenApi(op =>
    {
        op.Summary = "Update score and status of an existing game";
        return op;
    });

    app.MapDelete("/games/{id}", async (int id, GameDb db) =>
    {
        if (await db.Games.FindAsync(id) is Game game)
        {
            db.Games.Remove(game);
            await db.SaveChangesAsync();
            return Results.Ok(game);
        }
        return Results.NotFound();
    })
    .WithOpenApi(op =>
    {
        op.Summary = "Delete an existing game";
        return op;
    });


    app.MapGet("/teams", async (GameDb db) =>
        await db.Teams.ToListAsync())
        .WithOpenApi(op =>
        {
            op.Summary = "Retrieve all teams";
            return op;
        });

    app.MapGet("/teams/{id}", async (int id, GameDb db) =>
        await db.Teams.FindAsync(id)
                is Team team
                    ? Results.Ok(team)
                    : Results.NotFound())
        .WithOpenApi(op =>
        {
            op.Summary = "Retrieve one team by team id";
            return op;
        });

    app.MapPost("/teams", async (Team team, GameDb db) =>
    {
        db.Teams.Add(team);
        await db.SaveChangesAsync();

        return Results.Created($"/teams/{team.Id}", team);
    })
    .WithOpenApi(op =>
    {
        op.Summary = "Create a new team";
        return op;
    });

    app.MapPut("/teams/{id}", async (int id, Team updatedTeam, GameDb db) =>
    {
        Team existingTeam = await db.Teams.FindAsync(id);

        if (existingTeam is null) return Results.NotFound();

        existingTeam.TeamName = updatedTeam.TeamName;
        existingTeam.Abbreviation = updatedTeam.Abbreviation;

        await db.SaveChangesAsync();

        return Results.NoContent();
    })
    .WithOpenApi(op =>
    {
        op.Summary = "Update an existing team";
        return op;
    });

    app.MapDelete("/teams/{id}", async (int id, GameDb db) =>
    {
        if (await db.Teams.FindAsync(id) is Team team)
        {
            db.Teams.Remove(team);
            await db.SaveChangesAsync();
            return Results.Ok(team);
        }
        return Results.NotFound();
    })
    .WithOpenApi(op =>
    {
        op.Summary = "Delete an existing team";
        return op;
    });

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
