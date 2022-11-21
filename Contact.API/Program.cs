var builder = WebApplication.CreateBuilder(args);

var app = builder
    .AddServices()
    .Build();

app.ConfigureApp()
   .AddEndpoints()
   .Run();
