using BakerBuddy.Persistence.Recipes;

namespace BakerBuddy.Api
{
    public class Settings: IDbSettings
    {
        public ApplicationInsightsSettings ApplicationInsights{ get; set; }
        public string DbConnectionString { get; set; }
    }
}
