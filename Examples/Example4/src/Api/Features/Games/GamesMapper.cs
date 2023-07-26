namespace Api.Features.Games
{
    public static  class GamesMapper
    {
        public static void MapRoutes(WebApplication app)
        {
            var gameEndpoints = app.MapGroup("/games");

            gameEndpoints.MapGet("/", GamesHandler.GetGames)
            .WithOpenApi(op =>
            {
                op.Summary = "Retrieve all games";
                return op;
            });

            gameEndpoints.MapGet("/final", GamesHandler.GetFinalGames)
                .WithOpenApi(op =>
                {
                    op.Summary = "Retrieve games with final scores";
                    return op;
                });

            gameEndpoints.MapGet("/{id}", GamesHandler.GetGameById)
                .WithOpenApi(op =>
                {
                    op.Summary = "Retrieve one game by game id";
                    return op;
                });

            gameEndpoints.MapPost("/", GamesHandler.CreateGame)
                .WithOpenApi(op =>
                {
                    op.Summary = "Create a new game";
                    return op;
                });

            gameEndpoints.MapPut("/{id}", GamesHandler.UpdateGame)
                .WithOpenApi(op =>
                {
                    op.Summary = "Update an existing game";
                    return op;
                });

            gameEndpoints.MapPatch("/{id}", GamesHandler.UpdateGameScore)
                .WithOpenApi(op =>
                {
                    op.Summary = "Update score and status of an existing game";
                    return op;
                });

            gameEndpoints.MapDelete("/{id}", GamesHandler.DeleteGame)
                .WithOpenApi(op =>
                {
                    op.Summary = "Delete an existing game";
                    return op;
                });
        }

    }
}
