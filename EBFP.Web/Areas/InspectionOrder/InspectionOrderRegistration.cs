using System.Web.Mvc;

namespace EBFP.Web.Areas.InspectionOrder
{
    public class InspectionOrderRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "InspectionOrder";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                "InspectionOrder_default",
                "InspectionOrder/{action}/{id}",
                new {controller = "InspectionOrder", action = "InspectionOrder", id = UrlParameter.Optional}
                );            
        }
    }
}