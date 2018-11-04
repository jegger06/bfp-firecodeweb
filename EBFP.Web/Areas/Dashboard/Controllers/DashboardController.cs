using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EBFP.BL.Administration;
using EBFP.BL.Helper;
using EBFP.BL.HumanResources;
using EBFP.Helper;
using WebMatrix.WebData;
using EBFP.Web.Areas.Account.Controllers;

namespace EBFP.Web.Areas.Dashboard.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public DashboardController()
        {
            
        }
        public ActionResult Dashboard()
        {
            ViewBag.EmployeeId = CurrentUser.EmployeeId;
            ViewBag.Impersonate = CurrentUser.Impersonate;
            return View();
        }

        [HttpGet]
        public JsonResult SelectionAutoComplete(string search)
        {
            try
            {
                var ounitOfWork = new HRISUnitOfWork();
                var ret = ounitOfWork.Unit
                     .GetList(
                         a =>
                             a.Unit_StationName.Contains(search) || a.Unit_Code.Contains(search)).Select(a => new
                                {
                                    text = "[" +a.Unit_Code + "] - " + a.Unit_StationName +  ", " + a.Province_Name,
                                    id = a.Unit_Id
                                }).OrderBy(a => a.text);

                var jsonResult = Json(new
                {
                    data = ret
                }, JsonRequestBehavior.AllowGet);

                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
                return null;
            }
        }

        [HttpPost]
        public ActionResult ImpersonateUnit(string unitId)
        {
            try
            {
                if (!string.IsNullOrEmpty(unitId))
                {
                    var controller = DependencyResolver.Current.GetService<AccountController>();
                    controller.ControllerContext = new ControllerContext(Request.RequestContext, controller);
                    controller.SetCustomAuthenticationCookie(CurrentUser.Username, false, Convert.ToInt32(unitId), true);
                }
            }
            catch (Exception ex)
            {
                var jsonResultError = Json(new
                {
                    message = ex.InnerException?.Message ?? ex.Message
                }, JsonRequestBehavior.AllowGet);
                return jsonResultError;
            }

            var jsonResult = Json(new { message = "Success" }, JsonRequestBehavior.AllowGet);
            return jsonResult;

        }

        [HttpPost]
        public ActionResult CancelImpersonateUnit()
        {
            try
            {
                var controller = DependencyResolver.Current.GetService<AccountController>();
                controller.ControllerContext = new ControllerContext(Request.RequestContext, controller);
                controller.SetCustomAuthenticationCookie(CurrentUser.Username, false, null, true);

            }
            catch (Exception ex)
            {
                var jsonResultError = Json(new
                {
                    message = ex.InnerException?.Message ?? ex.Message
                }, JsonRequestBehavior.AllowGet);
                return jsonResultError;
            }

            var jsonResult = Json(new { message = "Success" }, JsonRequestBehavior.AllowGet);
            return jsonResult;

        }
    }
}