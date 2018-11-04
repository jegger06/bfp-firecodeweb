using System.Web.Mvc;
using WebMatrix.WebData;

namespace EBFP.Web.Areas.CIS.Controllers
{
    [Authorize]
    public class CISController : Controller
    {
        // GET: CIS/CIS
        public ActionResult Index()
        {
            WebSecurity.RequireRoles("xxx");
            return View();
        }
    }
}