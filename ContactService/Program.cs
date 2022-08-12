WebApplicationExtensions.InitialiseLogger();

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    builder.AddSwagger();
    builder.AddServices();
    builder.AddCorsPolicies();
    builder.AddLogging();
    builder.AddValidation<Program>();

    WebApplication app = builder.Build();
    app.UseSerilogRequestLogging();
    app.AddDocumentation();
    app.UseHttpsRedirection();
    app.RegisterMiddleware();
    app.UseCors(WebApplicationExtensions.AllowAllCorsPolicy);
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