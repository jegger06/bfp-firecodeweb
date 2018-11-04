using System.Web.Mvc;

namespace EBFP.Web.Areas.HRIS
{
    public class HRISAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "HRIS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                "HRIS_Dashboard_default",
                "HRIS/Dashboard/{action}/{id}",
                new {controller = "Dashboard", action = "Dashboard", id = UrlParameter.Optional}
                );

            context.MapRoute(
                "HRIS_Employee_default",
                "HRIS/Employee/{action}/{id}",
                new {controller = "Employee", action = "EmployeeRoster", id = UrlParameter.Optional}
                );

            context.MapRoute(
                "HRIS_default_Del",
                "HRIS/Employee/Delete",
                new {controller = "Employee", action = "Delete"}
                );


            context.MapRoute(
                "HRIS_Unit_default",
                "HRIS/Unit/{action}/{id}",
                new {controller = "Unit", action = "Units", id = UrlParameter.Optional}
                );

            context.MapRoute(
                "HRIS_SLL_default",
                "HRIS/SLL/{action}/{id}",
                new {controller = "SLL", action = "SLListing", id = UrlParameter.Optional}
                );

            context.MapRoute(
                "HRIS_Municipality_default",
                "HRIS/Municipality/{action}/{id}",
                new {controller = "Municipality", action = "Municipality", id = UrlParameter.Optional}
                );
            context.MapRoute(
                "HRIS_Reports_default",
                "HRIS/Reports/{action}/{id}",
                new {controller = "Reports", action = "Reports", id = UrlParameter.Optional}
                );
            context.MapRoute(
                "HRIS_Rank_default",
                "HRIS/Rank/{action}/{id}",
                new {controller = "Rank", action = "Rank", id = UrlParameter.Optional}
                );

            context.MapRoute(
                "HRIS_Directorates_default",
                "HRIS/Directorates/{action}/{id}",
                new { controller = "Directorates", action = "Index", id = UrlParameter.Optional }
            );
            
            context.MapRoute(
              "HRIS_Appointment_default",
              "HRIS/Appointment/{action}/{id}",
              new { controller = "Appointment", action = "Appointment", id = UrlParameter.Optional }
              );

            context.MapRoute(
            "HRIS_Region_default",
            "HRIS/Region/{action}/{id}",
            new { controller = "Region", action = "Region", id = UrlParameter.Optional }
            );
        }
    }
}