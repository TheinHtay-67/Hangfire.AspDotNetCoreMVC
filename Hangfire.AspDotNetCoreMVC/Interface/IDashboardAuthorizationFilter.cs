using Hangfire.Dashboard;

namespace Hangfire.AspDotNetCoreMVC.Interface
{
    public interface IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context);
    }
}
