using System.Web.Mvc;

namespace EBFP.Web.Areas.Dashboard
{
    public class DashboardRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Dashboard";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                "Dashboard_default",
                "Dashboard/{action}/{id}",
                new {controller = "Dashboard", action = "Dashboard", id = UrlParameter.Optional}
                );            
        }
    }
}