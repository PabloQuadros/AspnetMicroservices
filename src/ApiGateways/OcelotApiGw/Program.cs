using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging")).AddConsole().AddDebug();
builder.Services.AddOcelot();
app.MapGet("/", () => "Hello World!");
await app.UseOcelot();
app.Run();