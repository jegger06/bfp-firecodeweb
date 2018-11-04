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

namespace EBFP.Web.Areas.Setting.Controllers
{
    [Authorize]
    public class SettingController : Controller
    {

        public SettingController()
        {

        }

        public ActionResult Setting(string PageStatus)
        {
            ViewBag.PageStatus = PageStatus;
            return View();
        }
    }
}