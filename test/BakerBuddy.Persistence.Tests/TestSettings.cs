using BakerBuddy.Persistence.Recipes;
using Microsoft.Extensions.Configuration;

namespace BakerBuddy.Persistence.Tests
{
    public class TestSettings: IDbSettings
    {
        public static TestSettings Instance { get; }

        public string DbConnectionString { get; set; } = default!;

        static TestSettings()
        {
            var configuration = new ConfigurationBuilder()
                               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                               .Build();

            var configurationSection = configuration.GetSection("TestSettings");

            Instance = configurationSection.Get<TestSettings>();
        }
    }
}
