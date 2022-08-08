namespace ContactService.Infrastructure
{
    public static class ServiceDefinitions
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ContactDb>(options => options.UseInMemoryDatabase("applicationDb"));
            builder.Services.AddScoped<ContactDataService>();
        }
    }
}
