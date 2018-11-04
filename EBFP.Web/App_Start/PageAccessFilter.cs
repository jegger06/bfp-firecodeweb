using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EBFP.Web
{
    public class PageAccessFilter :  ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            foreach (dynamic route in RouteTable.Routes)
            {
                try
                {
                    var area = route.DataTokens["area"];
                    var controllerName = route.Defaults["controller"];
                    var actionName = route.Defaults["action"];
                    string message = String.Format("{0} controller:{1} action:{2}  area:{3}", "", Convert.ToString(controllerName), Convert.ToString(actionName), Convert.ToString(area));
                    Debug.WriteLine(message, "Action Filter Log");
                }
                catch (Exception ex)
                {

                }
            }
            Log("OnActionExecuting", filterContext.RouteData);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Log("OnActionExecuted", filterContext.RouteData);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Log("OnResultExecuting", filterContext.RouteData);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Log("OnResultExecuted", filterContext.RouteData);
        }


        private void Log(string methodName, RouteData routeData)
        {
            var area = routeData.DataTokens["area"];
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            var message = String.Format("{0} controller:{1} action:{2}  area:{3}", methodName, controllerName, actionName , area);
            Debug.WriteLine(message, "Action Filter Log");
        }
    }
}