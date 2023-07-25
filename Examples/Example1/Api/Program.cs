using Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// inline lambda
app.MapGet("/that", () => "Hello THAT Conference. We love bacon and inline lambda expressions!");

// lambda variable
var lambdaVariable = () => "Hello from a lambda variable!";

app.MapGet("/lambda", lambdaVariable);

// local function
string localFunction() => "Hello from a local function!";

app.MapGet("/local", localFunction);

// instance method
var instanceHandler = new InstanceHandler();

app.MapGet("/instance", instanceHandler.Hello);

// static method
app.MapGet("/static", StaticHandler.Hello);

// defined outside of program.cs
Endpoints.Map(app);

app.Run();
