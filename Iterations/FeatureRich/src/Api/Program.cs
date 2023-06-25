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

    // add this? 
    //.WithName("DeleteTeam")
    //.WithOpenApi();

    Log.Information("Creating route maps");
    var gameEndpoints = app.MapGroup("/games");
    gameEndpoints.MapGet("/", GetGames)
        .WithOpenApi(op =>
        {
            op.Summary = "Retrieve all games";
            return op;
        });
    gameEndpoints.MapGet("/final", GetFinalGames)
        .WithOpenApi(op =>
        {
            op.Summary = "Retrieve games with final scores";
            return op;
        });
    gameEndpoints.MapGet("/{id}", GetGameById)
        .WithOpenApi(op =>
        {
            op.Summary = "Retrieve one game by game id";
            return op;
        });
    gameEndpoints.MapPost("/", CreateGame)
        .WithOpenApi(op =>
        {
            op.Summary = "Create a new game";
            return op;
        });
    gameEndpoints.MapPut("/{id}", UpdateGame)
        .WithOpenApi(op =>
        {
            op.Summary = "Update an existing game";
            return op;
        });
    gameEndpoints.MapPatch("/{id}", UpdateGameScore)
        .WithOpenApi(op =>
        {
            op.Summary = "Update score and status of an existing game";
            return op;
        });
    gameEndpoints.MapDelete("/{id}", DeleteGame)
        .WithOpenApi(op =>
        {
            op.Summary = "Delete an existing game";
            return op;
        });

    var teamEndpoints = app.MapGroup("/teams");
    teamEndpoints.MapGet("/", GetTeams)
        .WithOpenApi(op =>
        {
            op.Summary = "Retrieve all teams";
            return op;
        });
    teamEndpoints.MapGet("/{id}", GetTeamById)
        .WithOpenApi(op =>
        {
            op.Summary = "Retrieve one team by team id";
            return op;
        });
    teamEndpoints.MapPost("/", AddTeam)
        .WithOpenApi(op =>
        {
            op.Summary = "Create a new team";
            return op;
        });
    teamEndpoints.MapPut("/{id}", UpdateTeam)
        .WithOpenApi(op =>
        {
            op.Summary = "Update an existing team";
            return op;
        });
    teamEndpoints.MapDelete("/{id}", DeleteTeam)
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

static async Task<IResult> GetGames(GameDb db)
{
    return TypedResults.Ok(await db.Games.ToArrayAsync());
}

static async Task<IResult> GetFinalGames(GameDb db)
{
    return TypedResults.Ok(await db.Games.Where(g => g.IsFinal).ToListAsync());
}

static async Task<IResult> GetGameById(int id, GameDb db)
{
    return await db.Games.FindAsync(id)
        is Game game
            ? TypedResults.Ok(game)
            : TypedResults.NotFound();
}

static async Task<IResult> CreateGame(Game game, GameDb db)
{
    db.Games.Add(game);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/games/{game.Id}", game);

}

static async Task<IResult> UpdateGame(int id, Game updatedGame, GameDb db)
{
    Game existingGame = await db.Games.FindAsync(id);

    if (existingGame is null) return TypedResults.NotFound();

    existingGame.GameDateTime = updatedGame.GameDateTime;
    existingGame.HomeTeamId = updatedGame.HomeTeamId;
    existingGame.AwayTeamId = updatedGame.AwayTeamId;
    existingGame.HomeTeamScore = updatedGame.HomeTeamScore;
    existingGame.AwayTeamScore = updatedGame.AwayTeamScore;
    existingGame.IsFinal = updatedGame.IsFinal;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();

}

static async Task<IResult> UpdateGameScore(int id, ScoreUpdate scoreUpdate, GameDb db)
{
    Game existingGame = await db.Games.FindAsync(id);

    if (existingGame is null) return TypedResults.NotFound();

    existingGame.HomeTeamScore = scoreUpdate.HomeTeamScore;
    existingGame.AwayTeamScore = scoreUpdate.AwayTeamScore;
    existingGame.IsFinal = scoreUpdate.IsFinal;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();

}

static async Task<IResult> DeleteGame(int id, GameDb db)
{
    if (await db.Games.FindAsync(id) is Game game)
    {
        db.Games.Remove(game);
        await db.SaveChangesAsync();
        return TypedResults.Ok(game);
    }
    return TypedResults.NotFound();

}

static async Task<IResult> GetTeams(GameDb db)
{
    return TypedResults.Ok(await db.Teams.ToListAsync());
}

static async Task<IResult> GetTeamById(int id, GameDb db)
{
    return await db.Teams.FindAsync(id)
        is Team team
            ? TypedResults.Ok(team)
            : TypedResults.NotFound();
}

static async Task<IResult> AddTeam(Team team, GameDb db)
{
    db.Teams.Add(team);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/teams/{team.Id}", team);
}

static async Task<IResult> UpdateTeam(int id, Team updatedTeam, GameDb db)
{
    Team existingTeam = await db.Teams.FindAsync(id);

    if (existingTeam is null) return Results.NotFound();

    existingTeam.TeamName = updatedTeam.TeamName;
    existingTeam.Abbreviation = updatedTeam.Abbreviation;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeleteTeam(int id, GameDb db)
{
    if (await db.Teams.FindAsync(id) is Team team)
    {
        db.Teams.Remove(team);
        await db.SaveChangesAsync();
        return TypedResults.Ok(team);
    }
    return TypedResults.NotFound();
}
