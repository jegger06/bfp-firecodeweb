using System.Web.Mvc;
using EBFP.BL.HumanResources;

namespace EBFP.Web.Areas.HRIS.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public DashboardController()
        {
            oUnitOfWork = new HRISUnitOfWork();
        }

        IHRISUnitOfWork oUnitOfWork { get; }
        // GET: HRIS/Dashboard
        public ActionResult Dashboard()
        {
            return View();
        }

        public JsonResult GetDashboardDetails()
        {
            var res = oUnitOfWork.Dashboard.GetDashboardDetails();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDutyStatusChartDetails()
        {
            var res = oUnitOfWork.Dashboard.GetDutyStatusChartDetails();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHRISDashboardCounter()
        {
            var res = oUnitOfWork.Dashboard.GetHRISDashboardCounter();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

    }
}