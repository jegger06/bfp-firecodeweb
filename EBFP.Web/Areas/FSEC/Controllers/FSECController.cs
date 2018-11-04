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
using EBFP.BL.FSEC;
using EBFP.BL.FSEC;

namespace EBFP.Web.Areas.FSEC.Controllers
{
    [Authorize]
    public class FSECController : Controller
    {

        public FSECController()
        {
           
        }
        
        public ActionResult FSEC(string PageStatus)
        {
            ViewBag.PageStatus = PageStatus;
            return View();
        }

        public ActionResult ReleasedFSEC(string PageStatus)
        {
            ViewBag.PageStatus = PageStatus;
            return View();
        }

        
        [OutputCache(Duration = 10, VaryByParam = "sEst_id")]
        public ActionResult FSECDetails(string sEst_id, string PageStatus)
        {
            var retInsp = new FSECModel();
            return View(retInsp);
        }


       [HttpPost]
        public ActionResult FSECDetails(FSECModel model)
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