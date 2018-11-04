using EBFP.Helper;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
namespace EBFP.Web.Filters
{
    public class TokenActionFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            var DoAuthenticateAPIRequest = filterContext.ActionDescriptor.GetCustomAttributes<AuthenticateAPIRequest>().Any()
               || filterContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AuthenticateAPIRequest>().Any(); ;

            if (DoAuthenticateAPIRequest)
            {
                try
                {
                    if (string.Equals(HttpContext.Current.Request.HttpMethod, "GET"))//REMOVE IF AUTH IS FULLY IMPLEMENTED IN POST
                    {
                        string tokenString = HttpContext.Current.Request.Headers["Authorization"];

                        if (string.IsNullOrEmpty(tokenString))
                            throw new UnauthorizedAccessException("Authorization token not provided.");
                         
                        var expires = ExtractTokenDateTimeExpiry(tokenString);
                        if (DateTime.UtcNow > expires)
                            throw new UnauthorizedAccessException("Authorization token has expired.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ThrowHttpResponseException(new UnauthorizedAccessException(ex.Message), HttpStatusCode.Unauthorized);
                }
            }
        }

        private DateTime ExtractTokenDateTimeExpiry(string tokenString)
        {
            try
            {
                tokenString = tokenString.Replace("Bearer", "").Trim();
                tokenString = tokenString.Decrypt();
                var expires = Convert.ToDateTime(tokenString);
                return expires;
            }
            catch
            {
                throw new UnauthorizedAccessException("Authorization token is not valid.");
            }
        }
         
        private  bool ThrowHttpResponseException(Exception ex,
         HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            var message = ex.InnerException?.Message ?? ex.Message;
            var exception = GetException(statusCode, message, message);
            throw new HttpResponseException(exception);
        }

        private  HttpResponseMessage GetException(HttpStatusCode statusCode, string content, string reason)
        {
            var resp = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(content),
                ReasonPhrase = reason
            };
            return resp;
        }
        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        { 
           
        }
    }
}