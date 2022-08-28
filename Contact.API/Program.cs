var builder = WebApplication.CreateBuilder(args);
builder.AddServices();
builder.OpenTelemetryInstrumenter();

var app = builder.Build();
app.UseHttpsRedirection();
app.AddDocumentation();
app.RegisterMiddleware();
app.UseHttpLogging();
app.UseRateLimiting();
app.AddEndpoints();

app.Run();
