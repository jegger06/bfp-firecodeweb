using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using EBFP.Helper;
using WebMatrix.WebData;

namespace EBFP.Web
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            WebSecurity.InitializeDatabaseConnection("HRIS_Connection", "tblEmployees", "Emp_Id", "Emp_Username", true);

            //var token = WebSecurity.GeneratePasswordResetToken("portaladmin");
            //WebSecurity.ResetPassword(token, "Admin1@3");
            // var employee = new EmployeeModel();
            // employee.Emp_BirthDate = DateTime.Now;
            // employee.Emp_BirthPlace = "Manila";
            // employee.Emp_Citizenship = "Filipino";
            // employee.Emp_CivilStatus = 1;
            // employee.Emp_FirstName = "Administrator";
            // employee.Emp_Gender = "F";
            // employee.Emp_LastName = "Administrator";
            // employee.Emp_MiddleName = "A";
            // employee.Emp_Password = "Admin1@3";
            // employee.Emp_Service_StartDate = DateTime.Now;
            // employee.Emp_Username = "portaladmin";
            //var success = EmployeeBL.CreateUserAndAccount(employee);
            // if (success)
            // { 
            //     WebSecurity.CreateAccount(employee.Emp_Username, employee.Emp_Password);
            // }

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles); 
        }


        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            try
            {
                var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    var encodedTicket = authCookie.Value;
                    if (!string.IsNullOrEmpty(encodedTicket))
                    {
                        var ticket = FormsAuthentication.Decrypt(encodedTicket);
                        var serializer = new JavaScriptSerializer();
                        var principalModel = serializer.Deserialize<CustomPrincipalModel>(ticket.UserData);

                        // setup proper roles

                        var currentRoles = new List<string>();

                        var principal = new CustomPrincipal(ticket.Name, currentRoles.ToArray());
                        principal.UserId = principalModel.UserId;
                        principal.FirstName = principalModel.FirstName;
                        principal.LastName = principalModel.LastName;
                        principal.Username = principalModel.Username;
                        principal.EmployeeUnitId = principalModel.EmployeeUnitId;
                        principal.RegionID = principalModel.RegionID;
                        principal.ProvinceID = principalModel.ProvinceID;
                        principal.MunicipalityID = principalModel.MunicipalityID;
                        principal.RankName = principalModel.RankName;
                        principal.RoleName = principalModel.RoleName;
                        principal.Impersonate = principalModel.Impersonate;

                        // assign to current thread

                        Thread.CurrentPrincipal = principal;
                    }
                }
            }
            catch
            {
                WebSecurity.Logout();
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Exception exception = Server.GetLastError();
            //Response.Clear();

            //HttpException httpException = exception as HttpException;

            //if (httpException != null)
            //{
            //    //string action;

            //    //switch (httpException.GetHttpCode())
            //    //{
            //    //    case 404:
            //    //        // page not found
            //    //        action = "HttpError404";
            //    //        break;
            //    //    case 500:
            //    //        // server error
            //    //        action = "HttpError500";
            //    //        break;
            //    //    default:
            //    //        action = "General";
            //    //        break;
            //    //}

            //    // clear error on server
            //    Server.ClearError(); 
            //}
        }
    }
}