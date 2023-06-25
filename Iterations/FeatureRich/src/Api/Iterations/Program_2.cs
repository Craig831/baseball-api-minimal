// Program.cs adding MapGroup to Program_1.cs

//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.OpenApi;
//using Api.Data;
//using Api.Models;

//var builder = WebApplication.CreateBuilder(args);

//// add dbcontext
//builder.Services.AddDbContext<GameDb>(opt => opt.UseInMemoryDatabase("GamesDatabase"));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//// add swagger
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//var gameEndpoints = app.MapGroup("/games");
//var teamEndpoints = app.MapGroup("/teams");


//gameEndpoints.MapGet("/", async (GameDb db) =>
//    await db.Games.ToListAsync())
//    .WithName("GetGames")
//    .WithOpenApi();

//gameEndpoints.MapGet("/final", async (GameDb db) =>
//    await db.Games.Where(g => g.IsFinal).ToListAsync())
//    .WithName("GetFinalsGames")
//    .WithOpenApi();

//gameEndpoints.MapGet("/{id}", async (int id, GameDb db) =>
//    await db.Games.FindAsync(id)
//        is Game game
//            ? Results.Ok(game)
//            : Results.NotFound())
//    .WithName("GetGameById")
//    .WithOpenApi();

//gameEndpoints.MapPost("/", async (Game game, GameDb db) =>
//{
//    db.Games.Add(game);
//    await db.SaveChangesAsync();

//    return Results.Created($"/games/{game.Id}", game);
//})
//    .WithName("AddGame")
//    .WithOpenApi();

//gameEndpoints.MapPut("/{id}", async (int id, Game updatedGame, GameDb db) =>
//{
//    Game existingGame = await db.Games.FindAsync(id);

//    if (existingGame is null) return Results.NotFound();

//    existingGame.GameDateTime = updatedGame.GameDateTime;
//    existingGame.HomeTeamId = updatedGame.HomeTeamId;
//    existingGame.AwayTeamId = updatedGame.AwayTeamId;
//    existingGame.HomeTeamScore = updatedGame.HomeTeamScore;
//    existingGame.AwayTeamScore = updatedGame.AwayTeamScore;
//    existingGame.IsFinal = updatedGame.IsFinal;

//    await db.SaveChangesAsync();

//    return Results.NoContent();
//})
//    .WithName("UpdateGame")
//    .WithOpenApi();

//gameEndpoints.MapDelete("/{id}", async (int id, GameDb db) =>
//{
//    if (await db.Games.FindAsync(id) is Game game)
//    {
//        db.Games.Remove(game);
//        await db.SaveChangesAsync();
//        return Results.Ok(game);
//    }
//    return Results.NotFound();
//})
//    .WithName("DeleteGame")
//    .WithOpenApi();


//teamEndpoints.MapGet("/", async (GameDb db) =>
//    await db.Teams.ToListAsync())
//    .WithName("GetTeams")
//    .WithOpenApi();

//teamEndpoints.MapGet("/{id}", async (int id, GameDb db) =>
//    await db.Teams.FindAsync(id)
//        is Team team
//            ? Results.Ok(team)
//            : Results.NotFound())
//    .WithName("GetTeamById")
//    .WithOpenApi();

//teamEndpoints.MapPost("/", async (Team team, GameDb db) =>
//{
//    db.Teams.Add(team);
//    await db.SaveChangesAsync();

//    return Results.Created($"/teams/{team.Id}", team);
//})
//    .WithName("AddTeam")
//    .WithOpenApi();

//teamEndpoints.MapPut("/{id}", async (int id, Team updatedTeam, GameDb db) =>
//{
//    Team existingTeam = await db.Teams.FindAsync(id);

//    if (existingTeam is null) return Results.NotFound();

//    existingTeam.TeamName = updatedTeam.TeamName;
//    existingTeam.Abbreviation = updatedTeam.Abbreviation;

//    await db.SaveChangesAsync();

//    return Results.NoContent();
//})
//    .WithName("UpdateTeam")
//    .WithOpenApi();

//teamEndpoints.MapDelete("/{id}", async (int id, GameDb db) =>
//{
//    if (await db.Teams.FindAsync(id) is Team team)
//    {
//        db.Teams.Remove(team);
//        await db.SaveChangesAsync();
//        return Results.Ok(team);
//    }
//    return Results.NotFound();
//})
//    .WithName("DeleteTeam")
//    .WithOpenApi();

//app.Run();
