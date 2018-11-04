using System.Web.Mvc;

namespace EBFP.Web.Areas.Administration
{
    public class AdministrationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Administration";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Administration_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
             "Role_default",
             "Admin/Role/{action}/{id}",
             new { controller = "Role", action = "Index", id = UrlParameter.Optional }
                );

            //context.MapRoute(
            //"Administration_default",
            //"Deposits/{action}/{id}",
            //new { controller = "Deposits", action = "Deposits", id = UrlParameter.Optional }
            //);

            context.MapRoute(
           "Administration_Deposits_default",
           "Administration/Deposits/{action}/{id}",
           new { controller = "Deposits", action = "Deposits", id = UrlParameter.Optional }
           );


            context.MapRoute(
           "Administration_OPS_default",
           "Administration/OPS/{action}/{id}",
           new { controller = "OPS", action = "SpoiledOPS", id = UrlParameter.Optional }
           );

            context.MapRoute(
         "Administration_OR_default",
         "Administration/OR/{action}/{id}",
         new { controller = "OR", action = "OR", id = UrlParameter.Optional }
         );

                context.MapRoute(
          "Administration_SpoiledOR_default",
          "Administration/SpoiledOR/{action}/{id}",
          new { controller = "OR", action = "SpoiledOR", id = UrlParameter.Optional }
          );


            context.MapRoute(
            "Administration_BPLOPayments_default",
            "Administration/BPLOPayments/{action}/{id}",
            new { controller = "BPLOPayments", action = "BPLOPayments", id = UrlParameter.Optional }
            );

            context.MapRoute(
            "Administration_Violation_default",
            "Administration/Violation/{action}/{id}",
            new { controller = "Violation", action = "Violation", id = UrlParameter.Optional }
            );


            context.MapRoute(
            "Administration_Reports_default",
            "Administration/Reports/{action}/{id}",
            new { controller = "Reports", action = "Reports", id = UrlParameter.Optional }
            );

        }
    }
}