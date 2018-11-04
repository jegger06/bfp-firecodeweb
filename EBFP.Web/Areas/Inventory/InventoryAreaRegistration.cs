using System.Web.Mvc;

namespace EBFP.Web.Areas.Inventory
{
    public class InventoryAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Inventory";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Inventory_default",
                "Inventory/{action}/{id}",
                new {controller = "Inventory", action = "Index", id = UrlParameter.Optional});

        }
    }
}