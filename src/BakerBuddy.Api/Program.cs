namespace BakerBuddy.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddLogging(
                loggingBuilder =>
                {
                    loggingBuilder.AddConsole();
                }
            );

            builder.Services.AddHealthChecks();

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