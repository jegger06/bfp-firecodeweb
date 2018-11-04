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
using EBFP.BL.InspectionOrder;
using EBFP.BL.InspectionOrder;

namespace EBFP.Web.Areas.InspectionOrder.Controllers
{
    [Authorize]
    public class InspectionOrderController : Controller
    {
        public InspectionOrderController()
        {
            unitOfWork = new InspectionOrderUnitOfWork();
        }

        public InspectionOrderController(InspectionOrderUnitOfWork inspOrder)
        {
            unitOfWork = inspOrder;
        }

        private IInspectionOrderUnitOfWork unitOfWork { get; }

        public ActionResult InspectionOrder(string PageStatus)
        {
            ViewBag.PageStatus = PageStatus;
            return View();
        }
        
        
        [OutputCache(Duration = 10, VaryByParam = "sEst_id")]
        public ActionResult InspectionOrderDetails(string sEst_id, string PageStatus)
        {
            var retInsp = new InspectionOrderModel();
            return View(retInsp);
        }


       [HttpPost]
        public ActionResult InspectionOrderDetails(InspectionOrderModel model)
        {
            try
            {
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
            }

            return View(model);
        }
        
    }
}