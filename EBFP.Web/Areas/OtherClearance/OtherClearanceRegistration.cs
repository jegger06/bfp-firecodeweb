using System.Web.Mvc;

namespace EBFP.Web.Areas.OtherClearance
{
    public class OtherClearanceRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "OtherClearance";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                "OtherClearance_default",
                "OtherClearance/{action}/{id}",
                new {controller = "OtherClearance", action = "OtherClearance", id = UrlParameter.Optional}
                );            
        }
    }
}