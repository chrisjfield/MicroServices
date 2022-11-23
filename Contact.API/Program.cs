var builder = WebApplication.CreateBuilder(args);

var app = builder
    .AddServices()
    .Build();

var versionSet = app.ApiVersionSet();

app.ConfigureApp()
   .AddEndpoints(versionSet)
   .Run();
