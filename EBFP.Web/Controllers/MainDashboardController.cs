using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EBFP.Web.Controllers
{
    public class MainDashboardController : Controller
    {
        // GET: MainDashboard
        [Authorize]
        public ActionResult Index()
        {
            //return View();
            return Redirect("/Dashboard/Dashboard");
        }
    }
}