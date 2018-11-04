using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EBFP.BL.CIS;
using EBFP.BL.Helper;
using EBFP.BL.HumanResources;
using EBFP.BL.Inventory;
using EBFP.Helper;

namespace EBFP.Web.Areas.CIS.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ICISUnitOfWork _oCISUnitOfWork = new CISUnitOfWork();
        private readonly IInventoryUnitOfWork _oUnitOfWork = new InventoryUnitOfWork();
        private readonly IHRISUnitOfWork oUnitOfWork = new HRISUnitOfWork();

        public ActionResult Dashboard()
        {
            return View();
        }

        #region Fire Fighting Capability

        public JsonResult GetFireFightingCountDetails()
        {
            var res = oUnitOfWork.Municipality.GetFireFightingCountDetails();
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPopulationCount()
        {
            var populations = oUnitOfWork.Municipality.GetPopulation();
            return Json(populations, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region FireIncidents
        public JsonResult GetFireIncidentsStatistics()
        {
            var res = _oUnitOfWork.FireRecord.GetFireIncidentsStatistics();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFireIncidentRespondedTo()
        {
            var res = _oUnitOfWork.FireRecord.GetFireIncidentRespondedTo();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFireIncidentInjured()
        {
            var res = _oUnitOfWork.FireRecord.GetFireIncidentInjured();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFireIncidentDeaths()
        {
            var res = _oUnitOfWork.FireRecord.GetFireIncidentDeaths();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFireIncidentDamages()
        {
            var res = _oUnitOfWork.FireRecord.GetFireIncidentDamages();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Inspection Orders
        public JsonResult GetFSICEstablishments()
        {
            var res = _oCISUnitOfWork.Dashboard.GetInspectionOrderOnEstablishmemts();
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Fire Prevention
        public JsonResult GetFirePreventionActivities()
        {
            //var res = _oCISUnitOfWork.Dashboard.GetFirePreventionActivities();
        
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Personnel Strength
        public JsonResult GetPersonnelStrenght()
        {
            var res = oUnitOfWork.Employee.GetPersonnelStrenght();
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPersonnelStrenthTotals()
        {
            try
            {
                var officerRanks = Functions.OfficerRanks();
                var nonOfficerRanks = Functions.NonOfficerRanks();

                var rankList = oUnitOfWork.Rank.GetRanklList().ToList();

                var officer = rankList.Where(a => officerRanks.Contains(a.RankId)).ToList();

                var nonOfficer = rankList.Where(a => nonOfficerRanks.Contains(a.RankId)).ToList();

                var nonUniformed = rankList.Where(a => a.RankId == (int)Rank.NUP).ToList();

                var ret = new PersonnelStrengthModel
                {
                    TotalDBMOfficer = officer.Sum(a => a.DBMAuthorized),
                    TotalActualOfficer = officer.Sum(a => a.ActualStrength),
                    TotalDBMNonOfficer = nonOfficer.Sum(a => a.DBMAuthorized),
                    TotalActualNonOfficer = nonOfficer.Sum(a => a.ActualStrength),
                    TotalDBMNonUnifomedPersonnel = nonUniformed.Sum(a => a.DBMAuthorized),
                    TotalActualNonUnifomedPersonnel = nonUniformed.Sum(a => a.ActualStrength)
                };
                return Json(ret, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
                return null;
            }
        }
        public JsonResult GetPersonnelPerRegion()
        {
            var personnelStrengthList = new List<PersonnelStrengthModel>();
            var res = _oCISUnitOfWork.Dashboard.GetDashPersonnelPerRegion();
            var firetrucks = _oUnitOfWork.Truck.GetActualNRFireTrucks();

            if (res.Count > 0)
            {
                foreach (var item in res)
                {
                    var personnelStrength = new PersonnelStrengthModel();
            
                    personnelStrength.OfficerRanks = item.Officer;
                    personnelStrength.NonOfficerRanks = item.NonOfficer;
                    personnelStrength.NonUniformedPersonnel = item.NUP;
                    personnelStrength.TotalPersonnel = personnelStrength.OfficerRanks +
                                                       personnelStrength.NonOfficerRanks +
                                                       personnelStrength.NonUniformedPersonnel;

                    if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
                    {
                        personnelStrength.Province = item.Province;
                        personnelStrength.ActualFireTrucks = firetrucks.Count(a =>
                         a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == item.ProvinceId);
                    }
                    else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
                    {
                        personnelStrength.Unit = item.Unit;
                        personnelStrength.ActualFireTrucks = firetrucks.Count(a =>
                         a.Truck_UnitId == item.UnitId);
                    }
                    else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                    {
                        personnelStrength.Unit = item.Unit;
                        personnelStrength.ActualFireTrucks = firetrucks.Count(a =>
                         a.Truck_UnitId == item.UnitId);
                    }
                    else if (!(PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation) ||
                               PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince) ||
                               PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion)))
                    {
                        personnelStrength.Region = item.Region;
                        personnelStrength.ActualFireTrucks = firetrucks.Count(a =>
                            a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == item.RegId);
                    }
                    //else
                    //{
                    //    personnelStrength.Region = item.Region;
                    //    personnelStrength.ActualFireTrucks = 0;
                    //}
                      

                    personnelStrength.IdealPersonnel = personnelStrength.ActualFireTrucks * 14;
                    personnelStrength.AdditionalManpower = personnelStrength.IdealPersonnel -
                                                           personnelStrength.TotalPersonnel;
                    personnelStrength.AdminPersonnel = item.GeneralAdmin;
                    personnelStrength.OperationPersonnel = item.Operations;
                    personnelStrength.NUPAdminPersonnel = item.NUPGeneralAdmin;
                    personnelStrength.NUPOperationPersonnel = item.NUPOperations;
                    personnelStrengthList.Add(personnelStrength);
                }
            }
            return Json(personnelStrengthList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDashPersonnelPerMunicipality(int municipalityId)
        {
            var res = _oCISUnitOfWork.Dashboard.GetDashPersonnelPerMunicipality(municipalityId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRankDetails()
        {
            try
            {
                var nonOfficerRanks = Functions.NonOfficerRanks();
         
                  var  res = oUnitOfWork.Rank.GetRanklList().Where(a => nonOfficerRanks.Contains(a.RankId)).ToList();

                    if (res.Count() <= 0)
                    {
                        foreach (var item in nonOfficerRanks)
                        {
                            var model = new ActualVsAuthorizedModel();
                            model.Rank = ((Rank)item).ToDescription();
                            model.DBMAuthorized = 0;
                            model.ActualStrength = 0;
                            model.Variance = 0;

                            res.Add(model);
                        }
                    }
                
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
                return null;
            }
        }


        public JsonResult GetOfficerRankDetails()
        {
            try
            {
                var officerRanks = Functions.OfficerRanks();

                var res = oUnitOfWork.Rank.GetRanklList().Where(a => officerRanks.Contains(a.RankId)).ToList();

                if (res.Count() <= 0)
                {
                    foreach (var item in officerRanks)
                    {
                        var model = new ActualVsAuthorizedModel();
                        model.Rank = ((Rank)item).ToDescription();
                        model.DBMAuthorized = 0;
                        model.ActualStrength = 0;
                        model.Variance = 0;

                        res.Add(model);
                    }
                }
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
                return null;
            }
        }
        #endregion

        #region Establishment
        public JsonResult GetInspectedEstablishment()
        {
            //var res = _oCISUnitOfWork.Dashboard.GetInspectedEstablishment();
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFireCodeFees()
        {
            var res = _oCISUnitOfWork.Dashboard.GetFireCodeFees();
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEstablishmentCounter(int municipalityId)
        {
            var res = _oCISUnitOfWork.Dashboard.GetEstablishmentCounter(municipalityId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Truck

        public JsonResult GetTruckCountDetails(string sMunicipalityId = "")
        {
            var res = _oUnitOfWork.Truck.GetTruckCountDetails(sMunicipalityId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTruckAgeCountDetails()
        {
            var res = _oUnitOfWork.Truck.GetTruckAgeCountDetails();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTruckSummaryCount()
        {
            var res = _oUnitOfWork.Truck.GetTruckSummaryCount();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTruckAgeGroupCount(string sMunicipalityId = "")
        {
            var res = _oUnitOfWork.Truck.GetTruckAgeGroupCount(sMunicipalityId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Other Vehicle

        public JsonResult GetVehicleCountDetails(string sMunicipalityId = "")
        {
            var res = _oUnitOfWork.OtherVehicle.GetVehicleCountDetails(sMunicipalityId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVehicleSummaryCount()
        {
            var res = _oUnitOfWork.OtherVehicle.GetVehicleSummaryCount();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PPE
        public JsonResult GetPPEDashboardCount(string sMunicipalityId = "")
        {
            var res = _oUnitOfWork.Municipality.GetPPEDashboardCount(sMunicipalityId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}