WebApplicationExtensions.InitialiseLogger();

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    builder.AddSwagger();
    builder.AddServices();
    builder.AddLogging();

    WebApplication app = builder.Build();
    app.UseSerilogRequestLogging();
    app.AddDocumentation();
    app.UseHttpsRedirection();
    app.AddEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}