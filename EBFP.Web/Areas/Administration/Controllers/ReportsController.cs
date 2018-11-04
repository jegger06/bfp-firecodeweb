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

namespace EBFP.Web.Areas.Administration.Controllers
{
    public class ReportsController : Controller
    {
        public ReportsController()
        {
            
        }

        public ActionResult Reports()
        {
            return View();
        }
    }
}