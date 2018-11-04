using System.Web.Mvc;

namespace EBFP.Web.Areas.FSEC
{
    public class FSECRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "FSEC";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                "FSEC_default",
                "FSEC/{action}/{id}",
                new {controller = "FSEC", action = "FSEC", id = UrlParameter.Optional}
                );            
        }
    }
}