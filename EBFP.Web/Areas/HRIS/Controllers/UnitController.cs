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
    public class UnitController : Controller
    {
        public UnitController()
        {
            unitOfWork = new HRISUnitOfWork();
        }

        public UnitController(HRISUnitOfWork employee)
        {
            unitOfWork = employee;
        }

        private IHRISUnitOfWork unitOfWork { get; }

        public ActionResult Units()
        {        
            var unitModel = new UnitModel();
            unitModel.Unit_ProvDistrict = CurrentUser.ProvinceID;
            unitModel.Unit_Municipality_Id = CurrentUser.MunicipalityID;
            return View(unitModel);
        }

        [HttpPost]
        public JsonResult GetUnits(GridInfo gridInfo)
        {
            try
            {
                var ret = unitOfWork.Unit.GetListResult(gridInfo);

                var retJson = ret.UnitList.Select(a => new
                {
                    a.Reg_Description,
                    a.Unit_Code,
                    a.CityMunicipality_Name,
                    a.Unit_StationName,
                    a.Province_Name,
                    a.Unit_CategoryName,
                    a.sUnit_Id
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
                ViewBag.ExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
                return null;
            }
        }

        public async Task<ActionResult> UnitDetails(string sId, string PageStatus)
        {
            ViewBag.PageStatus = PageStatus;
            if (string.IsNullOrWhiteSpace(sId))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var unitId = Convert.ToInt32(sId.Decrypt());
            var retUnit = new UnitModel();
            if (unitId > 0)
            {
                retUnit = unitOfWork.Unit.GetUnitById(unitId);
                if (retUnit == null)
                    return HttpNotFound();
            }
            return View(retUnit);
        }

        public List<SelectListItem> GetProvincePerRegion(int reg_Id)
        {
            return Selections.GetProvince(reg_Id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UnitDetails(UnitModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MemoryStream target = new MemoryStream();
                    model.ChiefFSESImage?.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    var isImageByteEmpty = data.All(B => B == default(Byte));
                    if (!isImageByteEmpty)
                    {
                        model.Unit_ChiefFSES_Signature = data;
                    }
                    
                    MemoryStream target2 = new MemoryStream();
                    model.FireMarshallImage?.InputStream.CopyTo(target2);
                    byte[] data2 = target2.ToArray();
                    var isImageByteEmpty2 = data2.All(B => B == default(Byte));
                    if (!isImageByteEmpty2)
                    {
                        model.Unit_FireMarshall_Signature = data2;
                    }


                    if (model.Unit_Id > 0)
                    {
                        model.UnitUserInRoleModel?.ForEach(a => a.Unit_UIR_Unit_Id = model.Unit_Id);
                        
                        unitOfWork.Unit.UpdateUnit(model);
                    }
                    else
                    {
                        unitOfWork.Unit.Add(model);
                    }

                    unitOfWork.Complete();
                    return RedirectToAction("Units", "Unit");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
            }
            return View(model);
        }

        public ActionResult UserInRoleEditor(int unitId)
        {
            var UserInRoleEditor = new UnitUserInRoleModel();
            UserInRoleEditor.Unit_UIR_Unit_Id = unitId;
            return PartialView("~/Areas/HRIS/Views/Unit/Include/UnitDetails/UnitsUserInRole/Editor.cshtml",
                UserInRoleEditor);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadProvinceByDistrict(string regionId)
        {
            var ret = new List<SelectListItem>();
            var Region_Id = !string.IsNullOrWhiteSpace(regionId) ? Convert.ToInt32(regionId) : 0;
            var province = unitOfWork.Province.GetList(a => a.Region_Id == Region_Id);

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince) ||
                 PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
            {
                province = province.Where(a => a.Province_Id == CurrentUser.ProvinceID).ToList();
            }

            ret = province.Select(m => new SelectListItem()
            {
                Text = m.Province_Name,
                Value = m.Province_Id.ToString(),
            }).ToList();

            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadUnitByMunicipality(string municipalityId)
        {
            var provinceList = new List<SelectListItem>();
            if (!string.IsNullOrWhiteSpace(municipalityId))
            {
                var province = unitOfWork.Unit.GetUnitByMunicipality(Convert.ToInt32(municipalityId));
                provinceList = province.Select(m => new SelectListItem()
                {
                    Text = m.Unit_StationName,
                    Value = m.Unit_Id.ToString(),
                }).ToList();
            }
            return Json(provinceList, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public string Delete(string sId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sId))
                    throw new Exception("Cannot find item.");
               
                var unit_Id = Convert.ToInt32(sId.Decrypt());
                var inUsed = unitOfWork.Unit.CheckUnit(unit_Id);

                if (inUsed)
                    throw new Exception("This unit is currently in used.");

                unitOfWork.Unit.DeleteByID(unit_Id);
                ViewBag.PageStatus = PageStatus.Success.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return "Success";
        }
    }
}