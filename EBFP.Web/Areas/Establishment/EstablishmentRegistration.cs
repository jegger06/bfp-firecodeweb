using System.Web.Mvc;

namespace EBFP.Web.Areas.Establishment
{
    public class EstablishmentRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Establishment";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                "Establishment_default",
                "Establishment/{action}/{id}",
                new {controller = "Establishment", action = "Establishment", id = UrlParameter.Optional}
                );            
        }
    }
}