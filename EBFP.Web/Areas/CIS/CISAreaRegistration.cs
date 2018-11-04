using System.Web.Mvc;

namespace EBFP.Web.Areas.CIS
{
    public class CISAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CIS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                "CIS_PhysicalInventory_default",
                "PhysicalInventory/{action}/{id}",
                new {controller = "PhysicalInventory", action = "Index", id = UrlParameter.Optional}
                );

            context.MapRoute(
                "CIS_InventoryGroups_default",
                "InventoryGroups/{action}/{id}",
                new {controller = "InventoryGroups", action = "Index", id = UrlParameter.Optional}
                );

            context.MapRoute(
                "CIS_InventoryArticles_default",
                "InventoryArticles/{action}/{id}",
                new {controller = "InventoryArticles", action = "Index", id = UrlParameter.Optional}
                );

            context.MapRoute(
                "CIS_SuppliesInventory_default",
                "SuppliesInventory/{action}/{id}",
                new {controller = "SuppliesInventory", action = "Index", id = UrlParameter.Optional}
                );
            context.MapRoute(
                "CIS_Unserviceable_default",
                "Unserviceable/{action}/{id}",
                new {controller = "Unserviceable", action = "Index", id = UrlParameter.Optional}
                );
            context.MapRoute(
               "CIS_Reports_default",
               "InventoryReports/{action}/{id}",
               new { controller = "InventoryReports", action = "InventoryReports", id = UrlParameter.Optional }
               );

            context.MapRoute(
              "CIS_Dashboard_default",
              "CIS/Dashboard/{action}/{id}",
              new { controller = "Dashboard", action = "Dashboard", id = UrlParameter.Optional }
              );
        }
    }
}