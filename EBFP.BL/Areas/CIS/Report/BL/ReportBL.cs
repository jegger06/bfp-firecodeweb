using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EBFP.BL.Helper;
using EBFP.DataAccess;
using EBFP.BL.HumanResources;
using EBFP.Helper;

namespace EBFP.BL.CIS
{
    public class ReportBL : EntityFrameworkBase, IReport
    {
        public ReportBL(EBFPEntities _context)
        {
            context_ = _context;
        }

        public List<DirectoratesReportModel> GetDirectoratesByGroup(int groupId, DateTime endDate)
        {
            var date = endDate.Date;
            var directoratesByGroup = context.tblPhysicalInventory
                .Where(
                    a =>
                        a.PI_IG_Id == groupId && DbFunctions.TruncateTime(a.PI_DateAcquired) <= date &&
                        a.PI_IsDeleted == false)
                .Select(a => new DirectoratesReportModel
                {
                    Dir_Description = a.tblDirectorates.Dir_Name,
                    Dir_Id = a.PI_Dir_Id,
                    IG_Code = a.tblInventoryGroups.IG_Code,
                    IG_Name = a.tblInventoryGroups.IG_Description
                }).Distinct().OrderBy(a => a.Dir_Description)
                .ToList();

            return directoratesByGroup;
        }

        public List<PhysicalInventoryReportModel> GetArticlesByDirectorates(int dirId, DateTime endDate)
        {
            var physicalInventoryReport = context.tblPhysicalInventory
                .Where(
                    a =>
                        a.PI_Dir_Id == dirId && DbFunctions.TruncateTime(a.PI_DateAcquired) <= endDate &&
                        a.PI_IsDeleted == false)
                .Select(a => new PhysicalInventoryReportModel
                {
                    PI_Art_Name = a.tblArticles.Art_Name,
                    PI_Description = a.PI_Description,
                    PI_PropertyNumber = a.PI_PropertyNumber,
                    PI_UnitOfMeasure = a.PI_UnitOfMeasure,
                    PI_DateAcquired = a.PI_DateAcquired,
                    PI_UnitValue = a.PI_UnitValue,
                    PI_Office = a.PI_Office,
                    PI_End_User =
                        a.PI_Emp_Id <= 0 || a.PI_Emp_Id == null
                            ? a.PI_Remarks
                            : a.tblEmployees.tblRanks.Rank_Name + " " + a.tblEmployees.Emp_FirstName + " " +
                              a.tblEmployees.Emp_LastName +
                              (!string.IsNullOrEmpty(a.PI_Remarks) ? "/ " + a.PI_Remarks : ""),
                    PI_ARENumber = a.PI_ARENumber,
                    PI_ICSNUmber = a.PI_ICSNUmber
                }).OrderBy(a => a.PI_Art_Name)
                .ToList();

            return physicalInventoryReport;
        }

        public List<UnserviceableReportModel> GetUnserviceable(int startmonth, int endMonth,int year)
        {
            var unserviceableList = context.tblUnserviceablePhysicalInventory
                .Where(
                    a =>( DbFunctions.TruncateTime(a.UPI_CreatedDate).Value.Month >= startmonth && DbFunctions.TruncateTime(a.UPI_CreatedDate).Value.Month <= endMonth)
                           && DbFunctions.TruncateTime(a.UPI_CreatedDate).Value.Year == year)
                .Select(a => new UnserviceableReportModel
                {
                    UPI_Id = a.UPI_Id,
                    UPI_WMR = a.UPI_WMR,
                    UPI_ReportingOffice = a.UPI_ReportingOffice
                }).OrderBy(a => a.UPI_WMR)
                .ToList();

            return unserviceableList;
        }

        public List<UnserviceableItemList> GetUnserviceableItem(int unserviceableId)
        {
            var unserviceableItemList = context.tblPhysicalInventory
                .Where(a => a.PI_UPI_Id == unserviceableId)
                .Select(a => new UnserviceableItemList
                {
                    PI_Description = a.PI_Description,
                    PI_PropertyNumber = a.PI_PropertyNumber,
                    PI_DateAcquired = a.PI_DateAcquired,
                    PI_UnitValue = a.PI_UnitValue
                }).OrderBy(a => a.PI_Description)
                .ToList();

            return unserviceableItemList;
        }

        public List<SummaryReportModel> GetPhyicalInventorySummaryReport(DateTime endDate, int type)
        {
            endDate = endDate.Date;
            var summaryPARReport = new List<SummaryReportModel>();
            
            var physicalInventoryByType = context.tblPhysicalInventory.Where(
                    a => DbFunctions.TruncateTime(a.PI_DateAcquired) <= endDate && a.PI_IsDeleted == false)
                    .Select(a => new
                    {
                        a.PI_IG_Id,
                        a.tblInventoryGroups.IG_Code,
                        a.tblInventoryGroups.IG_Description,
                        a.PI_ARENumber,
                        a.PI_ICSNUmber
                    });

                physicalInventoryByType = type == (int)PhysicalInventoryReportType.PAR ?
                physicalInventoryByType.Where(a => !string.IsNullOrEmpty(a.PI_ARENumber)) :
                physicalInventoryByType.Where(a => !string.IsNullOrEmpty(a.PI_ICSNUmber));

          var physicalInventoryByTypeList =  physicalInventoryByType.Select(a => new PhysicalInventoryList
            {
                PI_IG_Id = a.PI_IG_Id,
                PI_IG_Code = a.IG_Code,
                PI_IG_Description = a.IG_Description
            }).Distinct().ToList();

            foreach (var ig in physicalInventoryByTypeList.OrderBy(a => a.PI_IG_Id))
            {          
                var summaryPARReportModel = new SummaryReportModel();

                summaryPARReportModel.IG_Code = ig.PI_IG_Code;
                summaryPARReportModel.IG_Name = ig.PI_IG_Description;
                summaryPARReportModel.TotalCost = GetPhysicalInventoryTotal(ig.PI_IG_Id, type, endDate);
                summaryPARReport.Add(summaryPARReportModel);
            }

            return summaryPARReport;
        }

        private decimal? GetPhysicalInventoryTotal(int igId, int type, DateTime endDate)
        {
            var physicalInventoryByType =
                context.tblPhysicalInventory.Where(
                    a => DbFunctions.TruncateTime(a.PI_DateAcquired) <= endDate && a.PI_IsDeleted == false && a.PI_IG_Id == igId)
                    .Select(a => new
                    {
                        a.PI_UnitValue,
                        a.PI_ARENumber,
                        a.PI_ICSNUmber
                    });

            physicalInventoryByType = type == (int)PhysicalInventoryReportType.PAR ?
                 physicalInventoryByType.Where(a => !string.IsNullOrEmpty(a.PI_ARENumber)) 
                : physicalInventoryByType.Where(a => !string.IsNullOrEmpty(a.PI_ICSNUmber));

            return physicalInventoryByType.Select(a => a.PI_UnitValue).Sum();
        }

        public List<PhysicalInventorySuppliesModel> GetPhyicalInventorySuppliesReport(DateTime endDate)
        {
            endDate = endDate.Date;

            var supplies = (from si in context.tblSuppliesInventory
                           join art in context.tblArticles on si.SI_Art_Id equals art.Art_Id
                           where si.SI_IsDeleted == false && DbFunctions.TruncateTime(si.SI_DateAcquired) <= endDate
                            select new PhysicalInventorySuppliesModel
                           {
                               SI_Art_Name = art.Art_Name,
                               SI_Description = si.SI_Description,
                               SI_StockNumber = si.SI_StockNumber,
                               SI_UnitOfMeasure = si.SI_UnitOfMeasure,
                               SI_UnitValue = si.SI_UnitValue,
                               SI_Quantity = si.SI_Quantity,
                               SI_OnHand = (si.SI_Quantity ?? 0) > 0 ?
                                   si.SI_Quantity.Value - (si.tblSuppliesInventoryOut.Count > 0 ? si.tblSuppliesInventoryOut.Sum(a => a.SIO_QuantityOut) : 0) :
                                   0,
                               SI_TotalAmount = ((si.SI_UnitValue ?? 0) * (si.SI_Quantity ?? 0))
                           }).ToList();

            return supplies;
        }

        #region SCBA
        public List<SCBANationWideModel> GetSCBANationwide()
        {
            var scbaNationWideList = new List<SCBANationWideModel>();
            var regions = context.tblRegions.Select(a => new
            {
                a.Reg_Id,
                a.Reg_Title
            }).ToList();
            var trucks = FilteredTrucks();
            var scbaMunicipality = FilteredMunicipalities();

            foreach (var reg in regions.OrderBy(a => a.Reg_Id))
            {
                var scbaNationWideModel = new SCBANationWideModel();
                var trucksByRegion = trucks.Where(a => a.Truck_Owner == (int)Truck_Owner.BFP && a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == reg.Reg_Id);
                var scba = scbaMunicipality.Where(a => a.RegId == reg.Reg_Id);

                scbaNationWideModel.RegId = reg.Reg_Id;
                scbaNationWideModel.RegTitle = reg.Reg_Title;
                scbaNationWideModel.Serviceable = trucksByRegion.Count(a => a.Truck_Status == (int)Truck_Status.ServiceAble);
                scbaNationWideModel.Unserviceable = trucksByRegion.Count(a => a.Truck_Status == (int)Truck_Status.UnserviceAble);
                scbaNationWideModel.UnderRepair = trucksByRegion.Count(a => a.Truck_Status == (int)Truck_Status.UnderRepair);

                scbaNationWideModel.SCBAServiceable = scba.Sum(a => a.SCBAServiceable);
                scbaNationWideModel.SCBAServiceableButForRepl = scba.Sum(a => a.SCBAServiceableButForRepl);
                scbaNationWideList.Add(scbaNationWideModel);
            }

            return scbaNationWideList;
        }

        public List<SCBAReportModel> GetSCBAByRegion(int region)
        {
            var scbaReportList = new List<SCBAReportModel>();
            var provinces = context.tblProvinces.Where(a => a.Region_Id == region).ToList();
            var scbaMunicipality = FilteredMunicipalities();
            var trucks = FilteredTrucks();
            foreach (var prov in provinces.OrderBy(a => a.Province_Id))
            {
                var scbaReportModel = new SCBAReportModel();
                var trucksByProvince = trucks.Where(a => a.Truck_Owner == (int)Truck_Owner.BFP &&
                 a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == prov.Province_Id);
                var scba = scbaMunicipality.Where(a => a.ProvinceId == prov.Province_Id);

                scbaReportModel.RegionName = prov.tblRegions.Reg_Title;
                scbaReportModel.Province = prov.Province_Name;
                scbaReportModel.Serviceable = trucksByProvince.Count(a => a.Truck_Status == (int)Truck_Status.ServiceAble);
                scbaReportModel.Unserviceable = trucksByProvince.Count(a => a.Truck_Status == (int)Truck_Status.UnserviceAble);
                scbaReportModel.UnderRepair = trucksByProvince.Count(a => a.Truck_Status == (int)Truck_Status.UnderRepair);

                scbaReportModel.SCBAServiceable = scba.Sum(a => a.SCBAServiceable);
                scbaReportModel.SCBAServiceableButForRepl = scba.Sum(a => a.SCBAServiceableButForRepl);
                scbaReportList.Add(scbaReportModel);
            }
            return scbaReportList;
        }

        public List<SCBAReportModel> GetSCBAByProvince(int provinceId)
        {
            var scbaReportList = new List<SCBAReportModel>();
            var municipality = context.tblCityMunicipality.Where(a => a.Municipality_Province_Id == provinceId).ToList();
            var scbaMunicipality = FilteredMunicipalities();
            var trucks = FilteredTrucks();
            foreach (var mun in municipality.OrderBy(a => a.Municipality_Id))
            {
                var scbaReportModel = new SCBAReportModel();

                var trucksByProvince = trucks.Where(a => a.Truck_Owner == (int)Truck_Owner.BFP &&
                a.tblUnits.tblCityMunicipality.Municipality_Id == mun.Municipality_Id);
                var scba = scbaMunicipality.Where(a => a.MunicipalityId == mun.Municipality_Id);

                scbaReportModel.MunicipalityName = mun.Municipality_Name;
                scbaReportModel.Province = mun.tblProvinces.Province_Name;
                scbaReportModel.Serviceable = trucksByProvince.Count(a => a.Truck_Status == (int)Truck_Status.ServiceAble);
                scbaReportModel.Unserviceable = trucksByProvince.Count(a => a.Truck_Status == (int)Truck_Status.UnserviceAble);
                scbaReportModel.UnderRepair =
                    trucksByProvince.Count(a => a.Truck_Status == (int)Truck_Status.UnderRepair);

                scbaReportModel.SCBAServiceable = scba.Sum(a => a.SCBAServiceable);
                scbaReportModel.SCBAServiceableButForRepl = scba.Sum(a => a.SCBAServiceableButForRepl);
                scbaReportList.Add(scbaReportModel);
            }

            return scbaReportList;
        }

        public List<SCBAReportModel> GetSCBAByStation(int municipality)
        {
            var scbaReportList = new List<SCBAReportModel>();
            var station = context.tblUnits.Where(a => a.Unit_Municipality_Id == municipality).ToList();
            var subStation = context.tblUnitSubStation.Where(a => a.tblUnits.Unit_Municipality_Id == municipality).ToList();
            var trucks = FilteredTrucks();
            //Station
            foreach (var item in station.OrderBy(a => a.Unit_Id))
            {
                var scbaReportModel = new SCBAReportModel();
                var trucksByProvince = trucks.Where(a => a.Truck_Owner == (int)Truck_Owner.BFP &&
                a.Truck_UnitId == item.Unit_Id);

                scbaReportModel.MunicipalityName = item.tblCityMunicipality.Municipality_Name;
                scbaReportModel.StationName = item.Unit_StationName + " FIRE STATION";
                scbaReportModel.Serviceable = trucksByProvince.Count(a => a.Truck_Status == (int)Truck_Status.ServiceAble);
                scbaReportModel.Unserviceable = trucksByProvince.Count(a => a.Truck_Status == (int)Truck_Status.UnserviceAble);
                scbaReportModel.UnderRepair = trucksByProvince.Count(a => a.Truck_Status == (int)Truck_Status.UnderRepair);

                scbaReportList.Add(scbaReportModel);
            }

            //Substation
            foreach (var sub in subStation.OrderBy(a => a.Sub_Id))
            {
                var scbaReportModel = new SCBAReportModel();
                var trucksByProvince = trucks.Where(a => a.Truck_Owner == (int)Truck_Owner.BFP &&
                a.tblUnitSubStation.Sub_Id == sub.Sub_Id);

                scbaReportModel.MunicipalityName = sub.tblUnits.tblCityMunicipality.Municipality_Name;
                scbaReportModel.StationName = sub.tblUnits.Unit_StationName + " FIRE SUBSTATION";
                scbaReportModel.Serviceable = trucksByProvince.Count(a => a.Truck_Status == (int)Truck_Status.ServiceAble);
                scbaReportModel.Unserviceable = trucksByProvince.Count(a => a.Truck_Status == (int)Truck_Status.UnserviceAble);
                scbaReportModel.UnderRepair = trucksByProvince.Count(a => a.Truck_Status == (int)Truck_Status.UnderRepair);

                scbaReportList.Add(scbaReportModel);
            }
            return scbaReportList;
        }
        #endregion

        #region PPE 
        public List<PPENationWideModel> GetPPENationwide()
        { 
            var ppeNationWideList = new List<PPENationWideModel>();
            var regions = context.tblRegions.Select(a => new
            {
                a.Reg_Id,
                a.Reg_Title
            }).ToList();

            var ppeMunicipality = FilteredMunicipalities();
            var empList = FilteredEmp();

            foreach (var reg in regions.OrderBy(a => a.Reg_Id))
            {
                var ppeNationWideModel = new PPENationWideModel();
                var employee = empList.Where(a => a.RegId == reg.Reg_Id);
                var ppe = ppeMunicipality.Where(a => a.RegId == reg.Reg_Id);

                ppeNationWideModel.RegId = reg.Reg_Id;
                ppeNationWideModel.RegTitle = reg.Reg_Title;
                ppeNationWideModel.OperationPersonnel = employee.Count(a => a.Emp_Curr_JobFunc != (int) JobFunction.GeneralAdmin && a.Emp_Curr_JobFunc != 0);
                ppeNationWideModel.FireCoatServiceable = ppe.Sum(a => a.FireCoatServiceableButForRepl);
                ppeNationWideModel.FireCoatServiceableButForRepl = ppe.Sum(a => a.FireCoatServiceableButForRepl);
                ppeNationWideModel.TrouserServiceable = ppe.Sum(a => a.TrouserServiceable);
                ppeNationWideModel.TrouserServiceableButForRepl = ppe.Sum(a => a.TrouserServiceableButForRepl);
                ppeNationWideModel.BootsServiceable = ppe.Sum(a => a.BootsServiceable);
                ppeNationWideModel.BootsServiceableButForRepl = ppe.Sum(a => a.BootsServiceableButForRepl);
                ppeNationWideModel.GlovesServiceable = ppe.Sum(a => a.GlovesServiceable);
                ppeNationWideModel.GlovesServiceableButForRepl = ppe.Sum(a => a.GlovesServiceableButForRepl);
                ppeNationWideModel.HelmetServiceable = ppe.Sum(a => a.HelmetServiceable);
                ppeNationWideModel.HelmetServiceableButForRepl = ppe.Sum(a => a.HelmetServiceableButForRepl);
                ppeNationWideList.Add(ppeNationWideModel);
            }

            return ppeNationWideList;
        }

        public List<PPEReportModel> GetPPEByRegion(int region)
        {
            var ppeReportList = new List<PPEReportModel>();
            var provinces = context.tblProvinces.Where(a => a.Region_Id == region).ToList();
            var scbaMunicipality = FilteredMunicipalities();
            var emp = FilteredEmp();
            foreach (var prov in provinces.OrderBy(a => a.Province_Id))
            {
                var ppeNationWideModel = new PPEReportModel();
                var employee =
                    emp.Where(a => a.ProvinceId == prov.Province_Id);
                var scba = scbaMunicipality.Where(a => a.ProvinceId == prov.Province_Id);

                ppeNationWideModel.RegionName = prov.tblRegions.Reg_Title;
                ppeNationWideModel.Province = prov.Province_Name;
                ppeNationWideModel.OperationPersonnel =
                    employee.Count(
                        a => a.Emp_Curr_JobFunc != (int) JobFunction.GeneralAdmin && a.Emp_Curr_JobFunc != 0);
                ppeNationWideModel.FireCoatServiceable = scba.Sum(a => a.FireCoatServiceable);
                ppeNationWideModel.FireCoatServiceableButForRepl = scba.Sum(a => a.FireCoatServiceableButForRepl);
                ppeNationWideModel.TrouserServiceable = scba.Sum(a => a.TrouserServiceable);
                ppeNationWideModel.TrouserServiceableButForRepl = scba.Sum(a => a.TrouserServiceableButForRepl);
                ppeNationWideModel.BootsServiceable = scba.Sum(a => a.BootsServiceable);
                ppeNationWideModel.BootsServiceableButForRepl = scba.Sum(a => a.BootsServiceableButForRepl);
                ppeNationWideModel.GlovesServiceable = scba.Sum(a => a.GlovesServiceable);
                ppeNationWideModel.GlovesServiceableButForRepl = scba.Sum(a => a.GlovesServiceableButForRepl);
                ppeNationWideModel.HelmetServiceable = scba.Sum(a => a.HelmetServiceable);
                ppeNationWideModel.HelmetServiceableButForRepl = scba.Sum(a => a.HelmetServiceableButForRepl);
                ppeReportList.Add(ppeNationWideModel);
            }
            return ppeReportList;
        }

        public List<PPEReportModel> GetPPEByProvince(int provinceId)
        {
            var ppeReportList = new List<PPEReportModel>();
            var municipality = context.tblCityMunicipality.Where(a => a.Municipality_Province_Id == provinceId).ToList();
            var scbaMunicipality = FilteredMunicipalities();
            var emp = FilteredEmp();
            foreach (var mun in municipality.OrderBy(a => a.Municipality_Id))
            {
                var ppeModel = new PPEReportModel();
                var employee = emp.Where(a => a.MunicipalityId == mun.Municipality_Id);
                var scba = scbaMunicipality.Where(a => a.MunicipalityId == mun.Municipality_Id);

                ppeModel.MunicipalityName = mun.Municipality_Name;
                ppeModel.Province = mun.tblProvinces.Province_Name;
                ppeModel.OperationPersonnel = employee.Count(a => a.Emp_Curr_JobFunc != (int)JobFunction.GeneralAdmin && a.Emp_Curr_JobFunc != 0);
                ppeModel.FireCoatServiceable = scba.Sum(a => a.FireCoatServiceable) ;
                ppeModel.FireCoatServiceableButForRepl = scba.Sum(a => a.FireCoatServiceableButForRepl) ;
                ppeModel.TrouserServiceable = scba.Sum(a => a.TrouserServiceable) ;
                ppeModel.TrouserServiceableButForRepl = scba.Sum(a => a.TrouserServiceableButForRepl) ;
                ppeModel.BootsServiceable = scba.Sum(a => a.BootsServiceable) ;
                ppeModel.BootsServiceableButForRepl = scba.Sum(a => a.BootsServiceableButForRepl) ;
                ppeModel.GlovesServiceable = scba.Sum(a => a.GlovesServiceable) ;
                ppeModel.GlovesServiceableButForRepl = scba.Sum(a => a.GlovesServiceableButForRepl) ;
                ppeModel.HelmetServiceable = scba.Sum(a => a.HelmetServiceable) ;
                ppeModel.HelmetServiceableButForRepl = scba.Sum(a => a.HelmetServiceableButForRepl) ;
                ppeReportList.Add(ppeModel);
            }

            return ppeReportList;
        }
        #endregion

        #region Equipment
        public List<EquipmentNationWideModel> GetEquipmentNationwide()
        {
            var equipmentNationWideList = new List<EquipmentNationWideModel>();
            var regions = context.tblRegions.Select(a => new
            {
                a.Reg_Id,
                a.Reg_Title
            }).ToList();

            var trucks = FilteredTrucks();
            var equipmentMunicipality = FilteredMunicipalities();

            foreach (var reg in regions.OrderBy(a => a.Reg_Id))
            {
                var equipmentNationWideModel = new EquipmentNationWideModel();
                var trucksByRegion = trucks.Where(a => a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == reg.Reg_Id);
                var equipment = equipmentMunicipality.Where(a => a.RegId == reg.Reg_Id);

                equipmentNationWideModel.RegId = reg.Reg_Id;
                equipmentNationWideModel.RegTitle = reg.Reg_Title;
                equipmentNationWideModel.Serviceable = trucksByRegion.Count(a => a.Truck_Status == (int)Truck_Status.ServiceAble);
                equipmentNationWideModel.Unserviceable = trucksByRegion.Count(a => a.Truck_Status == (int)Truck_Status.UnserviceAble);
                equipmentNationWideModel.UnderRepair = trucksByRegion.Count(a => a.Truck_Status == (int)Truck_Status.UnderRepair);

                equipmentNationWideModel.FireHose15Serviceable = equipment.Sum(a => a.FireHose15Serviceable) ;
                equipmentNationWideModel.FireHose15ServiceableButForRepl = equipment.Sum(a => a.FireHose15ServiceableButForRepl) ;
                equipmentNationWideModel.FireHose25Serviceable = equipment.Sum(a => a.FireHose25Serviceable) ;
                equipmentNationWideModel.FireHose25ServiceableButForRepl = equipment.Sum(a => a.FireHose25ServiceableButForRepl) ;
                equipmentNationWideModel.FireNozzle15Serviceable = equipment.Sum(a => a.FireNozzle15Serviceable) ;
                equipmentNationWideModel.FireNozzle15ServiceableButForRepl = equipment.Sum(a => a.FireNozzle15ServiceableButForRepl) ;
                equipmentNationWideModel.FireNozzle25Serviceable = equipment.Sum(a => a.FireNozzle25Serviceable) ;
                equipmentNationWideModel.FireNozzle25ServiceableButForRepl = equipment.Sum(a => a.FireNozzle25ServiceableButForRepl) ;
                equipmentNationWideList.Add(equipmentNationWideModel);
            }

            return equipmentNationWideList;
        }

        public List<EquipmentReportModel> GetEquipmentByRegion(int region)
        {
            var equipmentReportList = new List<EquipmentReportModel>();
            var provinces = context.tblProvinces.Where(a => a.Region_Id == region).ToList();
            var equipmentMunicipality = FilteredMunicipalities();
            var trucks = FilteredTrucks();
            foreach (var prov in provinces.OrderBy(a => a.Province_Id))
            {
                var equipmentReportModel = new EquipmentReportModel();
                var trucksByProvince = trucks.Where(a => a.Truck_Owner == (int)Truck_Owner.BFP &&
                 a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == prov.Province_Id);
                var equipment = equipmentMunicipality.Where(a => a.ProvinceId == prov.Province_Id);

                equipmentReportModel.RegionName = prov.tblRegions.Reg_Title;
                equipmentReportModel.Province = prov.Province_Name;
                equipmentReportModel.Serviceable = trucksByProvince.Count(a => a.Truck_Status == (int)Truck_Status.ServiceAble);
                equipmentReportModel.Unserviceable = trucksByProvince.Count(a => a.Truck_Status == (int)Truck_Status.UnserviceAble);
                equipmentReportModel.UnderRepair = trucksByProvince.Count(a => a.Truck_Status == (int)Truck_Status.UnderRepair);

                equipmentReportModel.FireHose15Serviceable = equipment.Sum(a => a.FireHose15Serviceable) ;
                equipmentReportModel.FireHose15ServiceableButForRepl = equipment.Sum(a => a.FireHose15ServiceableButForRepl) ;
                equipmentReportModel.FireHose25Serviceable = equipment.Sum(a => a.FireHose25Serviceable) ;
                equipmentReportModel.FireHose25ServiceableButForRepl = equipment.Sum(a => a.FireHose25ServiceableButForRepl) ;
                equipmentReportModel.FireNozzle15Serviceable = equipment.Sum(a => a.FireNozzle15Serviceable) ;
                equipmentReportModel.FireNozzle15ServiceableButForRepl = equipment.Sum(a => a.FireNozzle15ServiceableButForRepl) ;
                equipmentReportModel.FireNozzle25Serviceable = equipment.Sum(a => a.FireNozzle25Serviceable) ;
                equipmentReportModel.FireNozzle25ServiceableButForRepl = equipment.Sum(a => a.FireNozzle25ServiceableButForRepl) ;
                equipmentReportList.Add(equipmentReportModel);
            }
            return equipmentReportList;
        }

        public List<EquipmentReportModel> GetEquipmentByProvince(int provinceId)
        {
            var equipmentReportList = new List<EquipmentReportModel>();
            var municipality = context.tblCityMunicipality.Where(a => a.Municipality_Province_Id == provinceId).ToList();
            var equipmentMunicipality = FilteredMunicipalities();
            var trucks = FilteredTrucks();
            foreach (var mun in municipality.OrderBy(a => a.Municipality_Id))
            {
                var equipmentReportModel = new EquipmentReportModel();

                var trucksByProvince = trucks.Where(a => a.Truck_Owner == (int)Truck_Owner.BFP &&
                a.tblUnits.tblCityMunicipality.Municipality_Id == mun.Municipality_Id);
                var equipment = equipmentMunicipality.Where(a => a.MunicipalityId == mun.Municipality_Id);

                equipmentReportModel.MunicipalityName = mun.Municipality_Name;
                equipmentReportModel.Province = mun.tblProvinces.Province_Name;
                equipmentReportModel.Serviceable = trucksByProvince.Count(a => a.Truck_Status == (int)Truck_Status.ServiceAble);
                equipmentReportModel.Unserviceable = trucksByProvince.Count(a => a.Truck_Status == (int)Truck_Status.UnserviceAble);
                equipmentReportModel.UnderRepair =
                    trucksByProvince.Count(a => a.Truck_Status == (int)Truck_Status.UnderRepair);

                equipmentReportModel.FireHose15Serviceable = equipment.Sum(a => a.FireHose15Serviceable) ;
                equipmentReportModel.FireHose15ServiceableButForRepl = equipment.Sum(a => a.FireHose15ServiceableButForRepl) ;
                equipmentReportModel.FireHose25Serviceable = equipment.Sum(a => a.FireHose25Serviceable) ;
                equipmentReportModel.FireHose25ServiceableButForRepl = equipment.Sum(a => a.FireHose25ServiceableButForRepl) ;
                equipmentReportModel.FireNozzle15Serviceable = equipment.Sum(a => a.FireNozzle15Serviceable) ;
                equipmentReportModel.FireNozzle15ServiceableButForRepl = equipment.Sum(a => a.FireNozzle15ServiceableButForRepl) ;
                equipmentReportModel.FireNozzle25Serviceable = equipment.Sum(a => a.FireNozzle25Serviceable) ;
                equipmentReportModel.FireNozzle25ServiceableButForRepl = equipment.Sum(a => a.FireNozzle25ServiceableButForRepl) ;
                equipmentReportList.Add(equipmentReportModel);
            }

            return equipmentReportList;
        }
        #endregion
        
        private List<MunicipalityReportModel> FilteredMunicipalities()
        {
            var municipality = context.tblCityMunicipality.AsQueryable();

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
                municipality = municipality.Where(a => a.tblProvinces.Region_Id == CurrentUser.RegionID);
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
                municipality = municipality.Where(a => a.tblProvinces.Province_Id == CurrentUser.ProvinceID);
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                municipality = municipality.Where(a => a.Municipality_Id == CurrentUser.MunicipalityID);

            var filteredMunicipality = municipality.Select(a => new MunicipalityReportModel()
            {
                ProvinceId = a.tblProvinces.Province_Id,
                RegId = a.tblProvinces.Region_Id,
                MunicipalityId = a.Municipality_Id,
                SCBAServiceable = a.Municipality_SCBA_Serviceable ?? 0,
                SCBAServiceableButForRepl = a.Municipality_SCBA_ServiceableForReplacement ?? 0,

                FireHose15Serviceable = a.Municipality_FireHose15_Serviceable ?? 0,
                FireHose15ServiceableButForRepl = a.Municipality_FireHose15_ServiceableForReplacement ?? 0,
                FireHose25Serviceable = a.Municipality_FireHose25_Serviceable ?? 0,
                FireHose25ServiceableButForRepl = a.Municipality_FireHose25_ServiceableForReplacement ?? 0,
                FireNozzle15Serviceable = a.Municipality_FireNozzle15_Serviceable ?? 0,
                FireNozzle15ServiceableButForRepl = a.Municipality_FireNozzle15_ServiceableForReplacement ?? 0,
                FireNozzle25Serviceable = a.Municipality_FireNozzle25_Serviceable ?? 0,
                FireNozzle25ServiceableButForRepl = a.Municipality_FireNozzle25_ServiceableForReplacement ?? 0,

                FireCoatServiceable = a.Municipality_FireCoat_Serviceable ?? 0,
                FireCoatServiceableButForRepl =  a.Municipality_FireCoat_ServiceableForReplacement ?? 0,
                TrouserServiceable=  a.Municipality_Trouser_Serviceable ?? 0,
                TrouserServiceableButForRepl = a.Municipality_Trouser_ServiceableForReplacement ?? 0,
                BootsServiceable= a.Municipality_Boots_Serviceable ?? 0,
                BootsServiceableButForRepl=  a.Municipality_Boots_ServiceableForReplacement ?? 0,
                GlovesServiceable= a.Municipality_Gloves_Serviceable ?? 0,
                GlovesServiceableButForRepl= a.Municipality_Gloves_ServiceableForReplacement ?? 0,
                HelmetServiceable=  a.Municipality_Helmet_Serviceable ?? 0,
                HelmetServiceableButForRepl= a.Municipality_Helmet_ServiceableForReplacement ?? 0
            }).ToList();

            return filteredMunicipality;
        } 

        private List<EmployeeCISReportModel> FilteredEmp()
        {

            var emp = context.tblEmployees.Where(a => a.Emp_Curr_Rank != null && a.Emp_Curr_Unit != null &&
                                                      a.Emp_DutyStatus == (int) DutyStatuses.Active);

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
                emp = emp.Where(a => a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID);
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
                emp = emp.Where(a => a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == CurrentUser.ProvinceID);
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                emp = emp.Where(a => a.Emp_Curr_Unit == CurrentUser.EmployeeUnitId);

            var empList = emp.Select(a => new EmployeeCISReportModel
            {
              RegId = a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id,
              ProvinceId = a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id,
              MunicipalityId = a.tblUnits.tblCityMunicipality.Municipality_Id,
              Emp_Curr_JobFunc =  a.Emp_Curr_JobFunc ?? 0
            }).ToList();

            return empList;
        }

        public List<tblTrucks> FilteredTrucks()
        {
            var truckList = new List<tblTrucks>();
            var truck = context.tblTrucks.AsQueryable();

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
                truck = truck.Where(a => a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID);
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
                truck = truck.Where(a => a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == CurrentUser.ProvinceID);
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                truck = truck.Where(a => a.Truck_UnitId == CurrentUser.EmployeeUnitId);

            truckList = truck.ToList();
            return truckList;
        }        
    }
}