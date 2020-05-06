using Hangfire.Dashboard;

namespace Infrastructure.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        /// <summary>
        /// It's dangerous to allow all authenticated users to see the Dashboard
        /// </summary>
        /// <param name="context">context</param>
        /// <returns></returns>
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            return httpContext.User.Identity.IsAuthenticated;
        }
    }
}
