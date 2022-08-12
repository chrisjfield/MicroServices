WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddSwagger();
builder.AddServices();
builder.AddCorsPolicies();
builder.AddValidation<Program>();

WebApplication app = builder.Build();
app.Logger.LogInformation("The app started");

app.AddDocumentation();
app.UseHttpsRedirection();
app.RegisterMiddleware();
app.UseCors(WebApplicationExtensions.AllowAllCorsPolicy);
app.AddEndpoints();
app.UseHttpLogging();

app.Run();