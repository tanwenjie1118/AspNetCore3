using Autofac;
using Hal.Infrastructure.Singleton;
using Hangfire.Dashboard;


namespace Hal.Infrastructure.Filters
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
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }
    }
}
