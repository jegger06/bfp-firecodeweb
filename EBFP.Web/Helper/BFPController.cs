using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Validation;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace EBFP.Web.Helper
{
    public class BFPController : Controller
    {
        public void Exception(Exception ex)
        {
            var msg = ex.InnerException != null
                ? (ex.InnerException.InnerException != null
                    ? ex.InnerException.InnerException.Message
                    : ex.InnerException.Message)
                : ex.Message;

            HttpStatusCode StatusCode = HttpStatusCode.BadRequest;
            var w32ex = ex as Win32Exception;
            if (w32ex == null)
                w32ex = ex.InnerException as Win32Exception;

            if (w32ex != null)
                StatusCode = (HttpStatusCode) w32ex.ErrorCode;

          ThrowHttpException(msg, StatusCode);
        }

        public void Exception(DbEntityValidationException ex)
        {
            var msg = "";
            foreach (var eve in ex.EntityValidationErrors)
            foreach (var ve in eve.ValidationErrors)
                msg += ve.ErrorMessage.Replace("FI_", "").Replace("_", " ").Replace("The ", "") + " ";

            ThrowHttpException(msg, HttpStatusCode.BadRequest);
        }

        private void ThrowHttpException(string errMsg, HttpStatusCode StatusCode)
        {
            ViewBag.ExceptionMessage = errMsg;
            ViewBag.PageStatus = PageStatus.Error.ToString();
             
            //HttpContext.Response.Status = errMsg;
            HttpContext.Response.StatusCode = (int) StatusCode; 
        }
    }
}