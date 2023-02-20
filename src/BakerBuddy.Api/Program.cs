using BakerBuddy.Api.Infrastructure;
using BakerBuddy.Persistence.Recipes;
using BakerBuddy.Ports;
using BakerBuddy.Recipes;

namespace BakerBuddy.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var settings = new Settings();

            builder.Configuration.Bind(settings);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddLogging(
                loggingBuilder =>
                {
                    loggingBuilder
                       .AddApplicationInsights(
                            configureTelemetryConfiguration: (config) => config.ConnectionString = settings.ApplicationInsights.ConnectionString,
                            configureApplicationInsightsLoggerOptions: (options) => { }
                        )
                       .AddConsole();
                }
            );

            builder.Services.AddHealthChecks();

            builder.Services.AddTransient<IRecipesDataStore, RecipesDataStore>();
            builder.Services.AddTransient<ICreateRecipeUseCase, CreateRecipeUseCase>();

            builder.Services.AddSingleton<GlobalExceptionLogger>();
            builder.Services.AddSingleton((IDbSettings)settings);

            var app = builder.Build();

            app.MapHealthChecks("/api/health");

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}