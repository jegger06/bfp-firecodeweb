using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EBFP.Helper;
using WebMatrix.WebData;

public class SessionExpireFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (HttpContext.Current.Session == null || CurrentUser.EmployeeUnitId == 0 || CurrentUser.EmployeeId == 0)
        {
            FormsAuthentication.SignOut();
            WebSecurity.Logout();
            var redirectPath = filterContext?.HttpContext?.Request?.Url?.AbsoluteUri;

            if (filterContext != null)
                filterContext.Result = new RedirectResult(redirectPath);

            return;
        }

        base.OnActionExecuting(filterContext);
    }
}

