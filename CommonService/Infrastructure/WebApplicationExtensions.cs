﻿using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CommonService
{
    public static class WebApplicationExtensions
    {
        public static void InitialiseLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            Log.Information("Starting up");
        }

        public static void AddSwagger(this WebApplicationBuilder builder)
        {
            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen((options) => options.EnableAnnotations());
        }

        public static void AddLogging(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((ctx, lc) => lc
            .WriteTo.Console()
            .ReadFrom.Configuration(ctx.Configuration));
        }

        public static void AddDocumentation(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}