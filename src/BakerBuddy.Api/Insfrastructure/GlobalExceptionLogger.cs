using Microsoft.AspNetCore.Mvc.Filters;

namespace BakerBuddy.Api.Infrastructure
{
    public class GlobalExceptionLogger : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionLogger> _logger;

        public GlobalExceptionLogger(ILogger<GlobalExceptionLogger> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception.Message);
        }
    }
}
