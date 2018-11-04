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

namespace EBFP.Web.Areas.FSIC.Controllers
{
    [Authorize]
    public class FSICController : Controller
    {

        public FSICController()
        {
           
        }
        
        public ActionResult FSIC(string PageStatus)
        {
            ViewBag.PageStatus = PageStatus;
            return View();
        }

        public ActionResult ReleasedFSIC(string PageStatus)
        {
            ViewBag.PageStatus = PageStatus;
            return View();
        }

        [OutputCache(Duration = 10, VaryByParam = "sEst_id")]
        public ActionResult FSICDetails(string sEst_id, string PageStatus)
        {
            return View();
        }


       [HttpPost]
        public ActionResult FSICDetails()
        {
            try
            {
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
            }

            return View();
        }
        
    }
}