using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EBFP.BL.Helper;
using EBFP.BL.HumanResources;
using System.Linq;

namespace EBFP.Web.Areas.HRIS.Controllers
{
    [Authorize]
    public class SLLController : Controller
    {
        public SLLController()
        {
            unitOfWork = new HRISUnitOfWork();
        }

        public SLLController(HRISUnitOfWork employee)
        {
            unitOfWork = employee;
        }

        private IHRISUnitOfWork unitOfWork { get; }

        public ActionResult SLListing()
        {
            return View(new List<UnitModel>());
        }

        [HttpPost]
        public JsonResult GetSeniorLinearListing(GridInfo gridInfo)
        {
            try
            {
                var ret = unitOfWork.SLL.GetListResult(gridInfo);

                var retJson = ret.SeniorityLinealList.Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                    a.RegionName,
                    a.ProvinceName,
                    a.UnitName,
                    a.PresentRankName,
                    a.QualifiedRank
                });

                var jsonResult = Json(new
                {
                    ret.DatatableInfo.recordsTotal,
                    recordsFiltered = ret.DatatableInfo.recordsTotal,
                    data = retJson
                }, JsonRequestBehavior.AllowGet);

                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
                return null;
            }
        }
    }
}