namespace Api.Endpoints
{
    public static class Endpoints
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/external", () => "Hello from an externally defined endpoint!");

            app.MapGet("/external/{id}", (int id) => $"Hello from an externally defined endpoint with an id of {id}!");
        }
    }
}
