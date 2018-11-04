using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using EBFP.BL.Helper;
using EBFP.BL.HumanResources;
using EBFP.Helper;

namespace EBFP.Web.Areas.HRIS.Controllers
{
    [Authorize]
    public class MunicipalityController : Controller
    {
        public MunicipalityController()
        {
            unitOfWork = new HRISUnitOfWork();
        }

        public MunicipalityController(HRISUnitOfWork employee)
        {
            unitOfWork = employee;
        }

        private IHRISUnitOfWork unitOfWork { get; }

        public ActionResult Municipality()
        {
            var municipalityModel = new MunicipalityModel();
            municipalityModel.ProvinceId = CurrentUser.ProvinceID;
            municipalityModel.Municipality_Id = CurrentUser.MunicipalityID;
            return View(municipalityModel);
        }

        [HttpPost]
        public JsonResult GetMunicipalities(GridInfo gridInfo)
        {
            try
            {
                var ret = unitOfWork.Municipality.GetListResult(gridInfo);
                var retJson = ret.MunicipalityList.Select(a => new
                {
                    a.Municipality_NSCB,
                    a.Reg_Description,
                    a.Province_Name,
                    a.Municipality_Name,
                    a.sMunicipality_Id
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

        public async Task<ActionResult> MunicipalityDetails(string sId, string PageStatus)
        {
            ViewBag.PageStatus = PageStatus;
            if (string.IsNullOrWhiteSpace(sId))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var municipalityId = Convert.ToInt32(sId.Decrypt());
            var retMunicipality = new MunicipalityModel();
            retMunicipality.Municipality_WithBuilding = false;
            if (municipalityId > 0)
            {
                retMunicipality = unitOfWork.Municipality.GetMunicipalityById(municipalityId);
                if (retMunicipality == null)
                    return HttpNotFound();
            }
            return View(retMunicipality);
        }

        public List<SelectListItem> GetProvincePerRegion(int reg_Id)
        {
            return Selections.GetProvince(reg_Id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MunicipalityDetails(MunicipalityModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Municipality_Id > 0)
                    {
                        unitOfWork.Municipality.UpdateMunicipality(model);
                    }
                    else
                    {
                        unitOfWork.Municipality.Add(model);
                    }

                    unitOfWork.Complete();
                    return RedirectToAction("Municipality", "Municipality");
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
            var provinceList = new List<SelectListItem>();
            if (!string.IsNullOrWhiteSpace(regionId))
            {
                var province = unitOfWork.Province.GetProvincePerRegion(Convert.ToInt32(regionId));
                provinceList = province.Select(m => new SelectListItem()
                {
                    Text = m.Province_Name,
                    Value = m.Province_Id.ToString(),
                }).ToList();
            }
            return Json(provinceList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadMunicipalityByProvince(string provinceId)
        {
            var ret = new List<SelectListItem>();
            var Province_Id = !string.IsNullOrWhiteSpace(provinceId) ? Convert.ToInt32(provinceId) : 0;

            var municipality = unitOfWork.Municipality.GetList(a => a.Municipality_Province_Id == Province_Id)
                .OrderBy(a => a.Municipality_Name).ToList();

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
            {
                municipality = municipality.Where(a => a.Municipality_Id == CurrentUser.MunicipalityID).ToList();
            }

            ret = municipality.Select(m => new SelectListItem()
            {
                Text = m.Municipality_Name,
                Value = m.Municipality_Id.ToString(),
            }).ToList();

            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string Delete(string sId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sId))
                    throw new Exception("Cannot find item.");
            
                var municipality_Id = Convert.ToInt32(sId.Decrypt());
                var inUsed = unitOfWork.Municipality.CheckMunicipality(municipality_Id);
                if (inUsed)
                    throw new Exception("This municipality is currently used.");

                unitOfWork.Municipality.DeleteByID(municipality_Id);
                ViewBag.PageStatus = PageStatus.Success.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException?.Message ?? ex.Message;
            }

            return "Success";
        }
    }
}