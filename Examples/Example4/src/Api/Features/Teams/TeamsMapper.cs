namespace Api.Features.Teams
{
    public static  class TeamsMapper
    {
        public static void MapRoutes(WebApplication app)
        {
            var teamEndpoints = app.MapGroup("/teams");

            teamEndpoints.MapGet("/", TeamsHandler.GetTeams)
                .WithOpenApi(op =>
                {
                    op.Summary = "Retrieve all teams";
                    return op;
                });

            teamEndpoints.MapGet("/{id}", TeamsHandler.GetTeamById)
                .WithOpenApi(op =>
                {
                    op.Summary = "Retrieve one team by team id";
                    return op;
                });

            teamEndpoints.MapPost("/", TeamsHandler.AddTeam)
                .WithOpenApi(op =>
                {
                    op.Summary = "Create a new team";
                    return op;
                });

            teamEndpoints.MapPut("/{id}", TeamsHandler.UpdateTeam)
                .WithOpenApi(op =>
                {
                    op.Summary = "Update an existing team";
                    return op;
                });

            teamEndpoints.MapDelete("/{id}", TeamsHandler.DeleteTeam)
                .WithOpenApi(op =>
                {
                    op.Summary = "Delete an existing team";
                    return op;
                });
        }
    }
}
