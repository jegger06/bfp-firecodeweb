using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EBFP.BL.Helper;
using EBFP.BL.HumanResources;
using EBFP.BL.Inventory;
using EBFP.Helper;

namespace EBFP.Web.Areas.Inventory.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    public class InventoryController : Controller
    {
        private readonly IInventoryUnitOfWork _oUnitOfWork = new InventoryUnitOfWork();
        private readonly IHRISUnitOfWork _unitOfWork = new HRISUnitOfWork();

        public ActionResult Index(string sMunicipalityId)
        {
            int municipalityId;
            if (!string.IsNullOrEmpty(sMunicipalityId))
            {
                municipalityId = Convert.ToInt32(sMunicipalityId.Decrypt());
            }
            else
            {
                var unitId = CurrentUser.EmployeeUnitId;
                var municipality = _unitOfWork.Unit.GetUnitById(unitId);
                municipalityId = municipality.Unit_Municipality_Id;

                return RedirectToAction("Index", new { sMunicipalityId = municipalityId.ToString().Encrypt() });
            }


            var retUnit = new MunicipalityModel();
            if (municipalityId > 0)
            {
                retUnit = _unitOfWork.Municipality.GetMunicipalityById(municipalityId);
                if (retUnit == null)
                    return HttpNotFound();
            }
            return View(retUnit);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(MunicipalityModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Municipality_Id > 0)
                        _oUnitOfWork.Municipality.UpdateInventoryMunicipality(model);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
            }
            return View(model);
        }

        public ActionResult MunicipalityList()
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
                var ret = _unitOfWork.Municipality.GetListResult(gridInfo);

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

        #region Station
        //Station
        public ActionResult StationDetails(string sId)
        {
            if (string.IsNullOrWhiteSpace(sId))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var retSub = new StationModel();
            var stationId = Convert.ToInt32(sId.Decrypt());
            if (stationId > 0)
                retSub = _oUnitOfWork.Station.GetStationById(stationId);

            if (retSub == null)
                return HttpNotFound();

            return View(retSub);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StationDetails(StationModel model)
        {
            if (ModelState.IsValid)
                try
                {
                    if (model.Unit_FireMarshall_Emp_Id == null || model.Unit_FireMarshall_Emp_Id <= 0)
                        throw new Exception("Fire marshall name is required.");

                    model = _oUnitOfWork.Station.SaveStationDetails(model);
                    var sId = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["sMunicipalityId"];
                    if (sId.Decrypt() == "0")
                    {
                        return RedirectToAction("Index", "Inventory");
                    }
                    return RedirectToAction("Index", "Inventory", new { sMunicipalityId = sId });
                }
                catch (Exception ex)
                {
                    ViewBag.ExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                    ViewBag.PageStatus = PageStatus.Error.ToString();
                }
            return View(model);
        }

        [HttpPost]
        public JsonResult GetStation(GridInfo gridInfo)
        {
            try
            {
                var sId = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["sMunicipalityId"];
                if (!string.IsNullOrEmpty(sId))
                {
                    gridInfo.searchStationModel.Municipality_Id = Convert.ToInt32(sId.Decrypt());
                }
                else
                {
                    var unitId = CurrentUser.EmployeeUnitId;
                    var municipalityId = _unitOfWork.Unit.GetUnitById(unitId);

                    gridInfo.searchStationModel.Municipality_Id = municipalityId.Unit_Municipality_Id;
                }
               

                var ret = _oUnitOfWork.Station.GetListResult(gridInfo);

                var retJson = ret.StationList.Select(a => new
                {
                    a.Unit_StationName,
                    a.Unit_Code,
                    a.Unit_CategoryName,
                    a.Unit_BuildingStatus_Text,
                    a.Unit_BuildingOwner_Text,
                    a.Unit_LotOwner_Text,
                    a.Unit_LotStatus_Text,
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
                ViewBag.ExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
                return null;
            }
        }
        #endregion


        #region Sub Station
        //Sub Station
        public ActionResult SubStationDetails(string sId, string sMunicipalityId)
        {
            if (string.IsNullOrWhiteSpace(sId))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var retSub = new SubStationModel();
            var subStationId = Convert.ToInt32(sId.Decrypt());
            retSub.Sub_WithBuilding = false;
            if (subStationId > 0)
            {
                retSub = _oUnitOfWork.SubStation.GetSubStationById(subStationId);
            }
            else
            {
                var unitId = CurrentUser.EmployeeUnitId;
                if (!string.IsNullOrWhiteSpace(sMunicipalityId))
                {
                    var municipalityId = Convert.ToInt32(sMunicipalityId.Decrypt());
                    unitId = _oUnitOfWork.Municipality.GetUnitIdByMunicipality(municipalityId);
                }
                retSub = _oUnitOfWork.SubStation.GetSubStationByUnitId(unitId);
            }

            if (retSub == null)
                return HttpNotFound();

            return View(retSub);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubStationDetails(SubStationModel model)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.Sub_Station_Code) && !string.IsNullOrWhiteSpace(model.Sub_Station_Name) 
                    && model.Sub_FireMarshall_EmpId <= 0)
                    throw new Exception("Fire marshall name is required.");

                if (model.Sub_Unit_Id <= 0)
                    throw new Exception("Unit (Station) is required.");

                if (ModelState.IsValid)
                {
                    model = _oUnitOfWork.SubStation.SaveSubStationDetails(model);
                    
                    var sId = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["sMunicipalityId"];
                    if (sId.Decrypt() == "0")
                    {
                        return RedirectToAction("Index", "Inventory");
                    }
                    return RedirectToAction("Index", "Inventory", new {sMunicipalityId = sId});
                }
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult DeleteSubStation(string sId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sId))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var subStaitonId = Convert.ToInt32(sId.Decrypt());
                _oUnitOfWork.SubStation.DeleteByID(subStaitonId);

                ViewBag.PageStatus = PageStatus.Success.ToString();
            }
            catch (Exception ex)
            {
                var exceptionMessage = ex.InnerException?.Message ?? ex.Message;
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, exceptionMessage);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public JsonResult GetSubStation(GridInfo gridInfo)
        {
            try
            {
                var sId = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["sMunicipalityId"];
                if (!string.IsNullOrEmpty(sId))
                {
                    gridInfo.searchSubStationModel.Municipality_Id = Convert.ToInt32(sId.Decrypt());
                }
                else
                {
                    var unitId = CurrentUser.EmployeeUnitId;
                    var municipalityId = _unitOfWork.Unit.GetUnitById(unitId);

                    gridInfo.searchSubStationModel.Municipality_Id = municipalityId.Unit_Municipality_Id;
                }
               

                var ret = _oUnitOfWork.SubStation.GetListResult(gridInfo);

                var retJson = ret.SubStationList.Select(a => new
                {
                    a.Sub_Unit_Name,
                    a.Sub_Station_Code,
                    a.Sub_Station_Name,
                    a.Sub_BuildingStatus_Text,
                    a.Sub_BuildingOwner_Text,
                    a.Sub_LotOwner_Text,
                    a.Sub_LotStatus_Text,
                    a.sSub_Id
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

        public ActionResult SubStationEditor(int unitId = 0, int subStationId = 0)
        {
            var editor = new SubStationEmployeeModel();
            editor.Unit_Id = unitId;
            editor.SubStation_Id = subStationId;
            return PartialView("~/Areas/Inventory/Views/Inventory/Include/SubStation/Editor.cshtml", editor);
        }
        #endregion


        #region Truck
        //Truck
        [HttpPost]
        public JsonResult GetTruck(GridInfo gridInfo)
        {
            try
            {
                var sId = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["sMunicipalityId"];
                if (!string.IsNullOrEmpty(sId))
                {
                    gridInfo.searchTruckModel.Municipality_Id = Convert.ToInt32(sId.Decrypt());
                }
                else
                {
                    var unitId = CurrentUser.EmployeeUnitId;
                    var municipalityId = _unitOfWork.Unit.GetUnitById(unitId);

                    gridInfo.searchTruckModel.Municipality_Id = municipalityId.Unit_Municipality_Id;
                }
               

                var ret = _oUnitOfWork.Truck.GetListResult(gridInfo);

                var retJson = ret.TruckList.Select(a => new
                {
                    a.Truck_Id_Code,
                    a.Unit_StationName,
                    a.Truck_TypeName,
                    a.Truck_StatusName,
                    a.Truck_OwnerName,
                    a.Truck_PlateNumber,
                    a.sTruck_Id
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

        public async Task<ActionResult> TruckDetails(string sId, string sMunicipalityId)
        {
            if (string.IsNullOrWhiteSpace(sId))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var unitId = CurrentUser.EmployeeUnitId;
            if (unitId <= 0)
                return Redirect("~/Account/Login");

            var retUnit = new TruckModel();
            var truckId = Convert.ToInt32(sId.Decrypt());
            if (truckId > 0)
            {
                retUnit = _oUnitOfWork.Truck.GetTruckById(truckId);
                if (retUnit == null)
                    return HttpNotFound();
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(sMunicipalityId))
                {
                    var municipalityId = Convert.ToInt32(sMunicipalityId.Decrypt());
                    unitId = _oUnitOfWork.Municipality.GetUnitIdByMunicipality(municipalityId);
                }
                retUnit = _oUnitOfWork.Truck.GetTruckByUnitId(unitId);
            }
            return View(retUnit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TruckDetails(TruckModel model)
        {
            if (ModelState.IsValid)
                try
                {
                    var empId = CurrentUser.EmployeeId;
                    if (empId <= 0)
                        return Redirect("~/Account/Login");

                    //model.FrontView = Request.Files["IFView"];
                    //model.LeftView = Request.Files["ILView"];
                    //model.RightView = Request.Files["IRView"];
                    //model.RearView = Request.Files["IRearView"];

                    model = _oUnitOfWork.Truck.UploadPicture(model);

                    if (model.Truck_Id > 0)
                    {
                        if (model.Truck_SubStationId == 0)
                            model.Truck_SubStationId = null;
                        model.Truck_ModifiedBy = empId;
                        _oUnitOfWork.Truck.UpdateTruck(model);
                    }
                    else
                    {
                        model.Truck_CreatedBy = empId;
                        model.Truck_DateCreated = DateTime.Now;
                        if (model.Truck_SubStationId == 0)
                            model.Truck_SubStationId = null;
                        _oUnitOfWork.Truck.Add(model);
                    }
                    _oUnitOfWork.Complete();


                    var sId = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["sMunicipalityId"];
                    if (sId.Decrypt() == "0")
                    {
                        return RedirectToAction("Index", "Inventory");
                    }
                    return RedirectToAction("Index", "Inventory", new { sMunicipalityId = sId });
                }
                catch (Exception ex)
                {
                    ViewBag.ExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                    ViewBag.PageStatus = PageStatus.Error.ToString();
                    //_oUnitOfWork.Truck.DeleteFiles(model);
                }
            return View(model);
        }

        [HttpGet]
        public ActionResult DeleteTruck(string sId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sId))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var truckId = Convert.ToInt32(sId.Decrypt());
                _oUnitOfWork.Truck.DeleteByID(truckId);
                ViewBag.PageStatus = PageStatus.Success.ToString();
            }
            catch (Exception ex)
            {
                var ExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ExceptionMessage);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        //STATION
        [HttpPost]
        public JsonResult GetTruckStation(GridInfo gridInfo)
        {
            try
            {
                var ret = _oUnitOfWork.Truck.GetTruckStationListResult(gridInfo);

                var retJson = ret.TruckList.Select(a => new
                {
                    a.Truck_Id_Code,
                    a.Truck_TypeName,
                    a.Truck_StatusName,
                    a.Truck_OwnerName,
                    a.Truck_PlateNumber,
                    a.sTruck_Id
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

        #endregion


        #region Other Vehicle
        //Other Vehicle
        [HttpPost]
        public JsonResult GetOtherVehicle(GridInfo gridInfo)
        {
            try
            {
                var sId = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["sMunicipalityId"];
                if (!string.IsNullOrEmpty(sId))
                {
                    gridInfo.searchOtherVehicleModel.Municipality_Id = Convert.ToInt32(sId.Decrypt());
                }
                else
                {
                    var unitId = CurrentUser.EmployeeUnitId;
                    var municipalityId = _unitOfWork.Unit.GetUnitById(unitId);

                    gridInfo.searchOtherVehicleModel.Municipality_Id = municipalityId.Unit_Municipality_Id;
                }
             
                var ret = _oUnitOfWork.OtherVehicle.GetListResult(gridInfo);

                var retJson = ret.OtherVehicleList.Select(a => new
                {
                    a.Vehicle_Id_Code,
                    a.Unit_StationName,
                    a.Vehicle_TypeName,
                    a.Vehicle_StatusName,
                    a.Vehicle_OwnerName,
                    a.Vehicle_PlateNumber,
                    a.sVehicle_Id
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
        
        public async Task<ActionResult> OtherVehicleDetails(string sId, string sMunicipalityId)
        {
            if (string.IsNullOrWhiteSpace(sId))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var vehicleId = Convert.ToInt32(sId.Decrypt());
            var retUnit = new OtherVehicleModel();

            var unitId = CurrentUser.EmployeeUnitId;
            if (unitId <= 0)
                return Redirect("~/Account/Login");


            if (vehicleId > 0)
            {
                retUnit = _oUnitOfWork.OtherVehicle.GetOtherVehicleById(vehicleId);
                if (retUnit == null)
                    return HttpNotFound();
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(sMunicipalityId))
                {
                    var municipalityId = Convert.ToInt32(sMunicipalityId.Decrypt());
                    unitId = _oUnitOfWork.Municipality.GetUnitIdByMunicipality(municipalityId);
                }
                retUnit = _oUnitOfWork.OtherVehicle.GetOtherVehicleByUnitId(unitId);
            }
            return View(retUnit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> OtherVehicleDetails(OtherVehicleModel model)
        {
            if (ModelState.IsValid)
                try
                {
                    var empId = CurrentUser.EmployeeId;
                    if (empId <= 0)
                        return Redirect("~/Account/Login");

                    //model.FrontView = Request.Files["IOVFView"];
                    //model.LeftView = Request.Files["IOVLView"];
                    //model.RightView = Request.Files["IOVRView"];
                    //model.RearView = Request.Files["IOVRearView"];

                    model = _oUnitOfWork.OtherVehicle.UploadPicture(model);


                    if (model.Vehicle_Id > 0)
                    {
                        if (model.Vehicle_SubStationId == 0)
                            model.Vehicle_SubStationId = null;
                        model.Vehicle_ModifiedBy = empId;
                        _oUnitOfWork.OtherVehicle.UpdateOtherVehicle(model);
                    }
                    else
                    {
                        model.Vehicle_CreatedBy = empId;
                        model.Vehicle_CreatedDate = DateTime.Now;
                        if (model.Vehicle_SubStationId == 0)
                            model.Vehicle_SubStationId = null;
                        _oUnitOfWork.OtherVehicle.Add(model);
                    }
                    _oUnitOfWork.Complete();

                    var sId = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["sMunicipalityId"];
                    if (sId.Decrypt() == "0")
                    {
                        return RedirectToAction("Index", "Inventory");
                    }
                    return RedirectToAction("Index", "Inventory", new { sMunicipalityId = sId });
                }
                catch (Exception ex)
                {
                    ViewBag.ExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                    ViewBag.PageStatus = PageStatus.Error.ToString();
                }
            return View(model);
        }
        
        [HttpGet]
        public ActionResult DeleteVehicle(string sId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sId))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var vehicleId = Convert.ToInt32(sId.Decrypt());
                _oUnitOfWork.OtherVehicle.DeleteByID(vehicleId);
                ViewBag.PageStatus = PageStatus.Success.ToString();
            }
            catch (Exception ex)
            {
                var ExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ExceptionMessage);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        //Other Vehicle Station
        [HttpPost]
        public JsonResult GetStationOtherVehicle(GridInfo gridInfo)
        {
            try
            {               
                var ret = _oUnitOfWork.OtherVehicle.GetVehicleStationListResult(gridInfo);

                var retJson = ret.OtherVehicleList.Select(a => new
                {
                    a.Vehicle_Id_Code,
                    a.Vehicle_TypeName,
                    a.Vehicle_StatusName,
                    a.Vehicle_OwnerName,
                    a.Vehicle_PlateNumber,
                    a.sVehicle_Id
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
        #endregion

        #region Personnel
        [HttpPost]
        public JsonResult GetPersonneList(GridInfo gridInfo)
        {
            try
            {
                var ret = _oUnitOfWork.Personnel.GetPersonneListResult(gridInfo);
                var jsonResult = Json(new
                {
                    ret.DatatableInfo.recordsTotal,
                    recordsFiltered = ret.DatatableInfo.recordsTotal,
                    data = ret.PersonnelListModel
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
        #endregion

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadProvinceByDistrict(string regionId)
        {
            var provinceList = new List<SelectListItem>();
            if (!string.IsNullOrWhiteSpace(regionId))
            {
                var province = _unitOfWork.Province.GetProvincePerRegion(Convert.ToInt32(regionId));
                provinceList = province.Select(m => new SelectListItem
                {
                    Text = m.Province_Name,
                    Value = m.Province_Id.ToString()
                }).ToList();
            }
            return Json(provinceList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadSubStationByStation(string stationId)
        {
            var provinceList = new List<SelectListItem>();
            if (!string.IsNullOrWhiteSpace(stationId))
            {
                var province = _oUnitOfWork.SubStation.GetSubStationByStation(Convert.ToInt32(stationId));
                provinceList = province.Select(m => new SelectListItem
                {
                    Text = m.Sub_Station_Name,
                    Value = m.Sub_Id.ToString()
                }).ToList();
            }
            return Json(provinceList, JsonRequestBehavior.AllowGet);
        }
        
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadFireMarshallDetails(int empId)
        {
            var res = _oUnitOfWork.SubStation.GetFireMarshall(empId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public List<SelectListItem> GetProvincePerRegion(int regId)
        {
            return Selections.GetProvince(regId);
        }
        
        public ActionResult FireRecordEditor()
        {
            var model = new FireRecordsModel();
            model.CreatedFromAjax = true;
            return PartialView("~/Areas/Inventory/Views/Inventory/Include/FireRecords/Editor.cshtml", model);
        }

        public ActionResult PopulationEditor()
        {
            var model = new PopulationModel();
            model.CreatedFromAjax = true;
            return PartialView("~/Areas/Inventory/Views/Inventory/Include/Population/Editor.cshtml", model);
        }

    }
}