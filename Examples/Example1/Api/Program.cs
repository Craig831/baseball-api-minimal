var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/that", () => "Hello THAT Conference. We love bacon!");

app.Run();
