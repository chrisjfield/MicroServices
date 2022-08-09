namespace EventService.Infrastructure
{
    public static class ServiceDefinitions
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<EventDb>(options => options.UseInMemoryDatabase("applicationDb"));
            builder.Services.AddScoped<EventDataService>();
        }
    }
}
