using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Models;

namespace Api.Features.Games
{
    public static class GamesHandler
    {
        public static async Task<IResult> GetGames(GameDb db)
        {
            return TypedResults.Ok(await db.Games.ToArrayAsync());
        }

        public static async Task<IResult> GetFinalGames(GameDb db)
        {
            return TypedResults.Ok(await db.Games.Where(g => g.IsFinal).ToListAsync());
        }

        public static async Task<IResult> GetGameById(int id, GameDb db)
        {
            return await db.Games.FindAsync(id)
                is Game game
                    ? TypedResults.Ok(game)
                    : TypedResults.NotFound();
        }

        public static async Task<IResult> CreateGame(Game game, GameDb db)
        {
            db.Games.Add(game);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/games/{game.Id}", game);

        }

        public static async Task<IResult> UpdateGame(int id, Game updatedGame, GameDb db)
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

        public static async Task<IResult> UpdateGameScore(int id, ScoreUpdateDTO scoreUpdate, GameDb db)
        {
            Game existingGame = await db.Games.FindAsync(id);

            if (existingGame is null) return TypedResults.NotFound();

            existingGame.HomeTeamScore = scoreUpdate.HomeTeamScore;
            existingGame.AwayTeamScore = scoreUpdate.AwayTeamScore;
            existingGame.IsFinal = scoreUpdate.IsFinal;

            await db.SaveChangesAsync();

            return TypedResults.NoContent();

        }

        public static async Task<IResult> DeleteGame(int id, GameDb db)
        {
            if (await db.Games.FindAsync(id) is Game game)
            {
                db.Games.Remove(game);
                await db.SaveChangesAsync();
                return TypedResults.Ok(game);
            }
            return TypedResults.NotFound();

        }
    }
}
