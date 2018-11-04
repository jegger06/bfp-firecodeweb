using System.Web.Mvc;

namespace EBFP.Web.Areas.Setting
{
    public class SettingRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Setting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                "Setting_default",
                "Setting/{action}/{id}",
                new {controller = "Setting", action = "Setting", id = UrlParameter.Optional}
                );            
        }
    }
}