using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Models;

namespace Api.Features.Teams
{
    public static class TeamsHandler
    {
        public static async Task<IResult> GetTeams(GameDb db)
        {
            return TypedResults.Ok(await db.Teams.ToListAsync());
        }

        public static async Task<IResult> GetTeamById(int id, GameDb db)
        {
            return await db.Teams.FindAsync(id)
                is Team team
                    ? TypedResults.Ok(team)
                    : TypedResults.NotFound();
        }

        public static async Task<IResult> AddTeam(Team team, GameDb db)
        {
            db.Teams.Add(team);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/teams/{team.Id}", team);
        }

        public static async Task<IResult> UpdateTeam(int id, Team updatedTeam, GameDb db)
        {
            Team existingTeam = await db.Teams.FindAsync(id);

            if (existingTeam is null) return Results.NotFound();

            existingTeam.TeamName = updatedTeam.TeamName;
            existingTeam.Abbreviation = updatedTeam.Abbreviation;

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        public static async Task<IResult> DeleteTeam(int id, GameDb db)
        {
            if (await db.Teams.FindAsync(id) is Team team)
            {
                db.Teams.Remove(team);
                await db.SaveChangesAsync();
                return TypedResults.Ok(team);
            }
            return TypedResults.NotFound();
        }
    }
}
