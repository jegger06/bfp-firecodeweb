using System.Web.Mvc;

namespace EBFP.Web.Areas.FSIC
{
    public class FSICRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "FSIC";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                "FSIC_default",
                "FSIC/{action}/{id}",
                new {controller = "FSIC", action = "FSIC", id = UrlParameter.Optional}
                );            
        }
    }
}