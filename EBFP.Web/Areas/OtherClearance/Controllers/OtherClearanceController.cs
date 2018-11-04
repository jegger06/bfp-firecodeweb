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
namespace EBFP.Web.Areas.OtherClearance.Controllers
{
    [Authorize]
    public class OtherClearanceController : Controller
    {

        public OtherClearanceController()
        {
           
        }
        
        public ActionResult OtherClearance(string PageStatus)
        {
            ViewBag.PageStatus = PageStatus;
            return View();
        }

        public ActionResult ReleasedOtherClearance(string PageStatus)
        {
            ViewBag.PageStatus = PageStatus;
            return View();
        }

        
        [OutputCache(Duration = 10, VaryByParam = "sEst_id")]
        public ActionResult OtherClearanceDetails(string sEst_id, string PageStatus)
        {
            return View();
        }

        
        
    }
}