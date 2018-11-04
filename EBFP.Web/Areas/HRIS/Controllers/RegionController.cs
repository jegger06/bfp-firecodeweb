using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using EBFP.BL.Helper;
using EBFP.BL.HumanResources;
using EBFP.Helper;
using System.IO;

namespace EBFP.Web.Areas.HRIS.Controllers
{
    [Authorize]
    public class RegionController : Controller
    {
        public RegionController()
        {
            unitOfWork = new HRISUnitOfWork();
        }

        public RegionController(HRISUnitOfWork employee)
        {
            unitOfWork = employee;
        }

        private IHRISUnitOfWork unitOfWork { get; }

        public ActionResult Region()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetRegions(GridInfo gridInfo)
        {
            try
            {
                var ret = unitOfWork.Region.GetListResult(gridInfo);
                var jsonResult = Json(new
                {
                    ret.DatatableInfo.recordsTotal,
                    recordsFiltered = ret.DatatableInfo.recordsTotal,
                    data = ret.RegionList.OrderBy(a=> a.Reg_Id)
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

        public async Task<ActionResult> RegionDetails(string sId, string PageStatus)
        {
            ViewBag.PageStatus = PageStatus;
            if (string.IsNullOrWhiteSpace(sId))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var regionId = Convert.ToInt32(sId.Decrypt());
            var retUnit = new RegionModel();
            if (regionId > 0)
            {
                retUnit = unitOfWork.Region.GetRegionById(regionId);
                if (retUnit == null)
                    return HttpNotFound();
            }
            return View(retUnit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegionDetails(RegionModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MemoryStream target = new MemoryStream();
                    model.RegLogo?.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    var isImageByteEmpty = data.All(B => B == default(Byte));
                    if (!isImageByteEmpty)
                    {
                        model.Reg_Logo = data;
                    }
                    
                    if(model.Reg_Id > 0)
                        unitOfWork.Region.UpdateRegion(model);

                    unitOfWork.Complete();
                    return RedirectToAction("Region", "Region");
                }
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