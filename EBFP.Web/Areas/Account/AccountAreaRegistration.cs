using System.Web.Mvc;

namespace EBFP.Web.Areas.Account
{
    public class AccountAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Account";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        { 
            context.MapRoute(
               "Account_default",
               "Account/{action}/{id}",
               new { controller = "Account", action = "Index", id = UrlParameter.Optional }
           );
        }
    }
}