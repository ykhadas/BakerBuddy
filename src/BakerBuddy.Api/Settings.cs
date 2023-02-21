using BakerBuddy.Persistence.Recipes;

namespace BakerBuddy.Api
{
    public class Settings: IDbSettings
    {
        public ApplicationInsightsSettings ApplicationInsights{ get; set; } = default!;
        public string DbConnectionString { get; set; } = default!;
    }
}
