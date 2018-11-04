using System;
using System.Collections.Generic;
using System.Linq;
using EBFP.BL.Establishment;
using EBFP.BL.Helper;
using EBFP.BL.InspectionOrder;
using EBFP.DataAccess;
using EBFP.Helper;
using EBFP.BL.HumanResources;

namespace EBFP.BL.CIS
{
    public class DashboardBL : EntityFrameworkBase, IDashboard
    {
        public DashboardBL(EBFPEntities _context)
        {
            context_ = _context;
        }

        public List<int> GetLastFiveYears()
        {
            var yearToDate = DateTime.Now.Year;
            return new List<int> { (yearToDate - 4), (yearToDate - 3), (yearToDate - 2), (yearToDate - 1), yearToDate };
        }

        public List<DashboardPersonnelModel> GetDashPersonnelPerRegion()
        {
            var personnelList = new List<DashboardPersonnelModel>();  

            var emp = from a in context.tblEmployees
                where
                    a.Emp_Curr_Rank != null && a.Emp_Curr_Unit != null && a.Emp_DutyStatus == (int) DutyStatuses.Active
                    && a.Emp_IsDeleted == false
                select new EmployeeModel
                {
                    RegionID = a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id,
                    ProvinceID = a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id,
                    Emp_Curr_Unit = a.Emp_Curr_Unit,
                    Emp_Curr_Rank = a.Emp_Curr_Rank,
                    Emp_Curr_JobFunc = a.Emp_Curr_JobFunc
                };
            
            var officerRanks = Functions.OfficerRanks();
            var nonOfficerRanks = Functions.NonOfficerRanks();
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
            {
                var provinces = context.tblProvinces.Where(a => a.Region_Id == CurrentUser.RegionID).ToList();

                foreach (var prov in provinces)
                {
                    var personnelModel = new DashboardPersonnelModel();
                    var byProvince = emp.Where(a => a.ProvinceID == prov.Province_Id);

                    personnelModel.ProvinceId = prov.Province_Id;
                    personnelModel.Province = prov.Province_Name;
                    personnelModel.NUP = byProvince.Count(a => a.Emp_Curr_Rank == (int)Rank.NUP);
                    personnelModel.Officer = byProvince.Count(a => officerRanks.Contains(a.Emp_Curr_Rank.Value));
                    personnelModel.NonOfficer = byProvince.Count(a => nonOfficerRanks.Contains(a.Emp_Curr_Rank.Value));
                    personnelModel.GeneralAdmin = byProvince.Count(a => a.Emp_Curr_JobFunc == (int)JobFunction.GeneralAdmin);
                    personnelModel.Operations =
                        byProvince.Count(
                            a => a.Emp_Curr_JobFunc != (int)JobFunction.GeneralAdmin && a.Emp_Curr_JobFunc != null);
                    personnelModel.NUPGeneralAdmin =
                        byProvince.Count(
                            a => a.Emp_Curr_JobFunc == (int)JobFunction.GeneralAdmin && a.Emp_Curr_Rank == (int)Rank.NUP);
                    personnelModel.NUPOperations =
                        byProvince.Count(
                            a =>
                                a.Emp_Curr_JobFunc != (int)JobFunction.GeneralAdmin && a.Emp_Curr_JobFunc != null &&
                                a.Emp_Curr_Rank == (int)Rank.NUP);

                    personnelList.Add(personnelModel);
                }
            }
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
            {
                var units = context.tblUnits.Where(a => a.Unit_ProvDistrict == CurrentUser.ProvinceID).ToList();

                foreach (var unit in units)
                {
                    var personnelModel = new DashboardPersonnelModel();
                    var byUnit = emp.Where(a => a.Emp_Curr_Unit == unit.Unit_Id);

                    personnelModel.UnitId = unit.Unit_Id;
                    personnelModel.Unit = unit.Unit_StationName;

                    personnelModel.NUP = byUnit.Count(a => a.Emp_Curr_Rank == (int)Rank.NUP);
                    personnelModel.Officer = byUnit.Count(a => officerRanks.Contains(a.Emp_Curr_Rank.Value));
                    personnelModel.NonOfficer = byUnit.Count(a => nonOfficerRanks.Contains(a.Emp_Curr_Rank.Value));
                    personnelModel.GeneralAdmin = byUnit.Count(a => a.Emp_Curr_JobFunc == (int)JobFunction.GeneralAdmin);
                    personnelModel.Operations =
                        byUnit.Count(
                            a => a.Emp_Curr_JobFunc != (int)JobFunction.GeneralAdmin && a.Emp_Curr_JobFunc != null);
                    personnelModel.NUPGeneralAdmin =
                        byUnit.Count(
                            a => a.Emp_Curr_JobFunc == (int)JobFunction.GeneralAdmin && a.Emp_Curr_Rank == (int)Rank.NUP);
                    personnelModel.NUPOperations =
                        byUnit.Count(
                            a =>
                                a.Emp_Curr_JobFunc != (int)JobFunction.GeneralAdmin && a.Emp_Curr_JobFunc != null &&
                                a.Emp_Curr_Rank == (int)Rank.NUP);

                    personnelList.Add(personnelModel);
                }

            }
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
            {
                var unit = context.tblUnits.FirstOrDefault(a => a.Unit_Id == CurrentUser.EmployeeUnitId);

                var personnelModel = new DashboardPersonnelModel();
                var byUnit = emp.Where(a => a.Emp_Curr_Unit == unit.Unit_Id);

                personnelModel.UnitId = unit.Unit_Id;
                personnelModel.Unit = unit.Unit_StationName;

                personnelModel.NUP = byUnit.Count(a => a.Emp_Curr_Rank == (int) Rank.NUP);
                personnelModel.Officer = byUnit.Count(a => officerRanks.Contains(a.Emp_Curr_Rank.Value));
                personnelModel.NonOfficer = byUnit.Count(a => nonOfficerRanks.Contains(a.Emp_Curr_Rank.Value));
                personnelModel.GeneralAdmin = byUnit.Count(a => a.Emp_Curr_JobFunc == (int) JobFunction.GeneralAdmin);
                personnelModel.Operations =
                    byUnit.Count(
                        a => a.Emp_Curr_JobFunc != (int) JobFunction.GeneralAdmin && a.Emp_Curr_JobFunc != null);
                personnelModel.NUPGeneralAdmin =
                    byUnit.Count(
                        a => a.Emp_Curr_JobFunc == (int) JobFunction.GeneralAdmin && a.Emp_Curr_Rank == (int) Rank.NUP);
                personnelModel.NUPOperations =
                    byUnit.Count(
                        a =>
                            a.Emp_Curr_JobFunc != (int) JobFunction.GeneralAdmin && a.Emp_Curr_JobFunc != null &&
                            a.Emp_Curr_Rank == (int) Rank.NUP);

                personnelList.Add(personnelModel);
            }
            else if (!(PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation) ||
                       PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince) ||
                       PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion)))
            {
                var regions = context.tblRegions.ToList();
                foreach (var reg in regions.OrderBy(a => a.Reg_Id))
                {
                    var personnelModel = new DashboardPersonnelModel();
                    var byRegion = emp.Where(a => a.RegionID == reg.Reg_Id);

                    personnelModel.RegId = reg.Reg_Id;
                    personnelModel.Region = reg.Reg_Title;
                    personnelModel.NUP = byRegion.Count(a => a.Emp_Curr_Rank == (int) Rank.NUP);
                    personnelModel.Officer = byRegion.Count(a => officerRanks.Contains(a.Emp_Curr_Rank.Value));
                    personnelModel.NonOfficer = byRegion.Count(a => nonOfficerRanks.Contains(a.Emp_Curr_Rank.Value));
                    personnelModel.GeneralAdmin =
                        byRegion.Count(a => a.Emp_Curr_JobFunc == (int) JobFunction.GeneralAdmin);
                    personnelModel.Operations =
                        byRegion.Count(
                            a => a.Emp_Curr_JobFunc != (int) JobFunction.GeneralAdmin && a.Emp_Curr_JobFunc != null);
                    personnelModel.NUPGeneralAdmin =
                        byRegion.Count(
                            a =>
                                a.Emp_Curr_JobFunc == (int) JobFunction.GeneralAdmin &&
                                a.Emp_Curr_Rank == (int) Rank.NUP);
                    personnelModel.NUPOperations =
                        byRegion.Count(
                            a =>
                                a.Emp_Curr_JobFunc != (int) JobFunction.GeneralAdmin && a.Emp_Curr_JobFunc != null &&
                                a.Emp_Curr_Rank == (int) Rank.NUP);

                    personnelList.Add(personnelModel);
                }
            }
            return personnelList;
        }

        //public List<FiveYearStatisticModel> GetInspectedEstablishment()
        //{
        //    var years = GetLastFiveYears();
        //    var establishment = GetFilteredEstablishment();

        //    var inspectedEst = (from a in establishment
        //        join b in context.tblInspectionOrders on a.Ref_Est_Id equals b.IO_Est_Id
        //        join c in context.tblUnits on b.IO_Unit_Id equals c.Unit_Id
        //        join d in context.tblCityMunicipality on c.Unit_Municipality_Id equals d.Municipality_Id
        //        join e in context.tblProvinces on d.Municipality_Province_Id equals e.Province_Id
        //        join f in context.tblRegions on e.Region_Id equals f.Reg_Id
        //        where b.IO_Remarks == 1 && b.IO_InspectionDate != null && years.Contains(b.IO_InspectionDate.Value.Year)
        //        select new
        //        {
        //            a.Est_Id,
        //            b.IO_InspectionDate.Value.Year,
        //            f.Reg_Id,
        //            e.Province_Id,
        //            c.Unit_Id
        //        })
        //        .GroupBy(a => new {a.Year, a.Reg_Id,a.Province_Id,a.Unit_Id })
        //        .Select(g => new
        //        {
        //            g.Key.Reg_Id,
        //            g.Key.Year,
        //            g.Key.Province_Id,
        //            g.Key.Unit_Id,
        //            Count = g.Count()
        //        });

        //    var inspectedEstList = new List<FiveYearStatisticModel>();
        //    if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
        //    {
        //        var provinces = context.tblProvinces.Where(a => a.Region_Id == CurrentUser.RegionID).ToList();

        //        foreach (var prov in provinces)
        //        {
        //            var byRegion = inspectedEst.Where(a => a.Province_Id == prov.Province_Id).ToList();

        //            var model = new FiveYearStatisticModel();
        //            model.ProvinceName = prov.Province_Name;
        //            model.ProvinceId = prov.Province_Id;

        //            model.Year1 = byRegion.Count(a => a.Year == years[0]);
        //            model.Year2 = byRegion.Count(a => a.Year == years[1]);
        //            model.Year3 = byRegion.Count(a => a.Year == years[2]);
        //            model.Year4 = byRegion.Count(a => a.Year == years[3]);
        //            model.Year5 = byRegion.Count(a => a.Year == years[4]);
        //            model.Average =
        //                Math.Round((model.Year1 + model.Year2 + model.Year3 + model.Year4 + model.Year5) / (decimal)5, 0);
        //            inspectedEstList.Add(model);
        //        }
        //    }
        //    else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
        //    {
        //        var units = context.tblUnits.Where(a => a.Unit_ProvDistrict == CurrentUser.ProvinceID).ToList();

        //        foreach (var unit in units)
        //        {
        //            var byProvince = inspectedEst.Where(a => a.Unit_Id == unit.Unit_Id).ToList();

        //            var model = new FiveYearStatisticModel();
        //            model.UnitName = unit.Unit_StationName;
        //            model.UnitId = unit.Unit_Id;

        //            model.Year1 = byProvince.Count(a => a.Year == years[0]);
        //            model.Year2 = byProvince.Count(a => a.Year == years[1]);
        //            model.Year3 = byProvince.Count(a => a.Year == years[2]);
        //            model.Year4 = byProvince.Count(a => a.Year == years[3]);
        //            model.Year5 = byProvince.Count(a => a.Year == years[4]);
        //            model.Average =
        //                Math.Round((model.Year1 + model.Year2 + model.Year3 + model.Year4 + model.Year5) / (decimal)5, 0);
        //            inspectedEstList.Add(model);
        //        }

        //    }
        //    else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
        //    {
        //        var unit = context.tblUnits.FirstOrDefault(a => a.Unit_Id == CurrentUser.EmployeeUnitId);

        //        var byProvince = inspectedEst.Where(a => a.Unit_Id == unit.Unit_Id).ToList();

        //        var model = new FiveYearStatisticModel();
        //        model.UnitName = unit.Unit_StationName;
        //        model.UnitId = unit.Unit_Id;

        //        model.Year1 = byProvince.Count(a => a.Year == years[0]);
        //        model.Year2 = byProvince.Count(a => a.Year == years[1]);
        //        model.Year3 = byProvince.Count(a => a.Year == years[2]);
        //        model.Year4 = byProvince.Count(a => a.Year == years[3]);
        //        model.Year5 = byProvince.Count(a => a.Year == years[4]);
        //        model.Average =
        //            Math.Round((model.Year1 + model.Year2 + model.Year3 + model.Year4 + model.Year5)/(decimal) 5, 0);
        //        inspectedEstList.Add(model);
        //    }
        //    else if (!(PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation) ||
        //               PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince) ||
        //               PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion)))
        //    {
        //        var excludedRegion = new List<int> {19, 20}; //NTFI & NHQ
        //        var regions = context.tblRegions.Where(a => !excludedRegion.Contains(a.Reg_Id));

        //        foreach (var reg in regions)
        //        {
        //            var byRegion = inspectedEst.Where(a => a.Reg_Id == reg.Reg_Id).ToList();

        //            var model = new FiveYearStatisticModel();
        //            model.RegionName = reg.Reg_Title;
        //            model.RegionId = reg.Reg_Id;

        //            model.Year1 = byRegion.Count(a => a.Year == years[0]);
        //            model.Year2 = byRegion.Count(a => a.Year == years[1]);
        //            model.Year3 = byRegion.Count(a => a.Year == years[2]);
        //            model.Year4 = byRegion.Count(a => a.Year == years[3]);
        //            model.Year5 = byRegion.Count(a => a.Year == years[4]);
        //            model.Average =
        //                Math.Round((model.Year1 + model.Year2 + model.Year3 + model.Year4 + model.Year5)/(decimal) 5, 0);
        //            inspectedEstList.Add(model);
        //        }
        //    }
        //    return inspectedEstList;
        //}
        public List<FireFeesCollectionModel> GetFireCodeFees()
        {

            var inspectedEstList = new List<FireFeesCollectionModel>();
            var years = GetLastFiveYears();
            var fireCodeFees = context.vwFeesByYearAndRegion;

            var inspectedEst = (from a in fireCodeFees
                                select new
                                {
                                    a.TotalFee,
                                    a.Collected_Date,
                                    a.RegionId,
                                    a.ProvinceId,
                                    a.UnitId
                                })
               .GroupBy(a => new { a.Collected_Date, a.RegionId, a.ProvinceId, a.UnitId })
               .Select(g => new
               {
                   g.Key.RegionId,
                   g.Key.Collected_Date,
                   g.Key.UnitId,
                   g.Key.ProvinceId,
                   Count = g.Sum(a => a.TotalFee ?? 0)
               });

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
            {
                var provinces = context.tblProvinces.Where(a => a.Region_Id == CurrentUser.RegionID).ToList();

                foreach (var prov in provinces)
                {
                    var byProvince = inspectedEst.Where(a => a.ProvinceId == prov.Province_Id).ToList();

                    var model = new FireFeesCollectionModel();
                    model.ProvinceName = prov.Province_Name;
                    model.ProvinceId = prov.Province_Id;

                    model.Year1 = byProvince.Where(a => a.Collected_Date == years[0]).Sum(a => a.Count);
                    model.Year2 = byProvince.Where(a => a.Collected_Date == years[1]).Sum(a => a.Count);
                    model.Year3 = byProvince.Where(a => a.Collected_Date == years[2]).Sum(a => a.Count);
                    model.Year4 = byProvince.Where(a => a.Collected_Date == years[3]).Sum(a => a.Count);
                    model.Year5 = byProvince.Where(a => a.Collected_Date == years[4]).Sum(a => a.Count);
                    model.Average = (model.Year1 + model.Year2 + model.Year3 + model.Year4 + model.Year5) / 5;
                    inspectedEstList.Add(model);
                }
            }
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
            {
                var units = context.tblUnits.Where(a => a.Unit_ProvDistrict == CurrentUser.ProvinceID).ToList();

                foreach (var unit in units)
                {
                    var byUnit = inspectedEst.Where(a => a.UnitId == unit.Unit_Id).ToList();

                    var model = new FireFeesCollectionModel();
                    model.UnitName = unit.Unit_StationName;
                    model.UnitId = unit.Unit_Id;

                    model.Year1 = byUnit.Where(a => a.Collected_Date == years[0]).Sum(a => a.Count);
                    model.Year2 = byUnit.Where(a => a.Collected_Date == years[1]).Sum(a => a.Count);
                    model.Year3 = byUnit.Where(a => a.Collected_Date == years[2]).Sum(a => a.Count);
                    model.Year4 = byUnit.Where(a => a.Collected_Date == years[3]).Sum(a => a.Count);
                    model.Year5 = byUnit.Where(a => a.Collected_Date == years[4]).Sum(a => a.Count);
                    model.Average = (model.Year1 + model.Year2 + model.Year3 + model.Year4 + model.Year5) / 5;
                    inspectedEstList.Add(model);
                }

            }
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
            {
                var unit = context.tblUnits.FirstOrDefault(a => a.Unit_Id == CurrentUser.EmployeeUnitId);

                var byUnit = inspectedEst.Where(a => a.UnitId == unit.Unit_Id).ToList();

                var model = new FireFeesCollectionModel();
                model.UnitName = unit?.Unit_StationName;
                model.UnitId = unit.Unit_Id;
                model.Year1 = byUnit.Where(a => a.Collected_Date == years[0]).Sum(a => a.Count);
                model.Year2 = byUnit.Where(a => a.Collected_Date == years[1]).Sum(a => a.Count);
                model.Year3 = byUnit.Where(a => a.Collected_Date == years[2]).Sum(a => a.Count);
                model.Year4 = byUnit.Where(a => a.Collected_Date == years[3]).Sum(a => a.Count);
                model.Year5 = byUnit.Where(a => a.Collected_Date == years[4]).Sum(a => a.Count);
                model.Average = (model.Year1 + model.Year2 + model.Year3 + model.Year4 + model.Year5) / 5;
                inspectedEstList.Add(model);
            }
            else if (!(PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation) ||
                       PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince) ||
                       PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion)))
            {
                var excludedRegion = new List<int> {19, 20}; //NTFI & NHQ
                var regions = context.tblRegions.Where(a => !excludedRegion.Contains(a.Reg_Id));

                foreach (var reg in regions)
                {
                    var byRegion = inspectedEst.Where(a => a.RegionId == reg.Reg_Id).ToList();

                    var model = new FireFeesCollectionModel();
                    model.RegionName = reg.Reg_Title;
                    model.RegionId = reg.Reg_Id;

                    model.Year1 = byRegion.Where(a => a.Collected_Date == years[0]).Sum(a => a.Count);
                    model.Year2 = byRegion.Where(a => a.Collected_Date == years[1]).Sum(a => a.Count);
                    model.Year3 = byRegion.Where(a => a.Collected_Date == years[2]).Sum(a => a.Count);
                    model.Year4 = byRegion.Where(a => a.Collected_Date == years[3]).Sum(a => a.Count);
                    model.Year5 = byRegion.Where(a => a.Collected_Date == years[4]).Sum(a => a.Count);
                    //model.Average = Math.Round((model.Year1 + model.Year2 + model.Year3 + model.Year4 + model.Year5)/5,
                    //    0);
                    model.Average = (model.Year1 + model.Year2 + model.Year3 + model.Year4 + model.Year5) / 5;
                    inspectedEstList.Add(model);
                }

                inspectedEstList[0].TotalYear1 = inspectedEstList.Sum(a => a.Year1);
                inspectedEstList[0].TotalYear2 = inspectedEstList.Sum(a => a.Year2);
                inspectedEstList[0].TotalYear3 = inspectedEstList.Sum(a => a.Year3);
                inspectedEstList[0].TotalYear4 = inspectedEstList.Sum(a => a.Year4);
                inspectedEstList[0].TotalYear5 = inspectedEstList.Sum(a => a.Year5);
                inspectedEstList[0].TotalAverage = inspectedEstList.Sum(a => a.Average);

            }

            return inspectedEstList;
        }

        //public IQueryable<EstablishmentModel> GetFilteredEstablishment()
        //{
        //    //var establishment = context.tblEstablishments.AsQueryable();

        //    var establishment = (
        //       from est in context.tblEstablishments
        //       select new EstablishmentModel
        //       {
        //           Est_Unit_Id = est.Est_Unit_Id,
        //           RegionID = est.tblUnits.tblCityMunicipality.tblProvinces.Region_Id,
        //           ProvinceID = est.tblUnits.tblCityMunicipality.Municipality_Province_Id,
        //           Ref_Est_Id = est.Ref_Est_Id,
        //           Est_Id = est.Est_Id,
        //           Est_EstablishmentStatus = est.Est_EstablishmentStatus,
        //           Est_CreatedDate = est.Est_CreatedDate
        //       }).AsQueryable();

        //    if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
        //        establishment =
        //            establishment.Where(
        //                a => a.RegionID == CurrentUser.RegionID);
        //    else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
        //        establishment =
        //            establishment.Where(
        //                a => a.ProvinceID == CurrentUser.ProvinceID);
        //    else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
        //        establishment = establishment.Where(a => a.Est_Unit_Id == CurrentUser.EmployeeUnitId);

        //    return establishment;
        //}

        public List<FiveYearInspectionStatisticModel> GetInspectionOrderOnEstablishmemts()
        {
            var filteredInspectionOrders = GetFilteredFiveYearInspectionOrder();

            var inspectionOrders = filteredInspectionOrders
                .GroupBy(a => new { a.Year, a.OccupancyType })
                .Select(g => new { g.Key.Year, g.Key.OccupancyType, Count = g.Count() }).ToList();

            var listInspectionOrders = new List<FiveYearInspectionStatisticModel>();

            var years = GetLastFiveYears();

            foreach (var year in years)
            {
                var byYear = inspectionOrders.Where(a => a.Year == year).ToList();

                var model = new FiveYearInspectionStatisticModel();
                model.Year = year;

                model.Type1 = byYear.Where(a => a.OccupancyType == (int)OccupancyType.Assembly).Sum(a => a.Count);
                model.Type2 = byYear.Where(a => a.OccupancyType == (int)OccupancyType.Educational).Sum(a => a.Count);
                model.Type3 = byYear.Where(a => a.OccupancyType == (int)OccupancyType.Health_Care).Sum(a => a.Count);
                model.Type4 = byYear.Where(a => a.OccupancyType == (int)OccupancyType.Correction_and_Detention_Center).Sum(a => a.Count);
                model.Type5 = byYear.Where(a => a.OccupancyType == (int)OccupancyType.Mercantile).Sum(a => a.Count);

                model.Type6 = byYear.Where(a => a.OccupancyType == (int)OccupancyType.Industrial).Sum(a => a.Count);
                model.Type7 = byYear.Where(a => a.OccupancyType == (int)OccupancyType.Business).Sum(a => a.Count);
                model.Type8 = byYear.Where(a => a.OccupancyType == (int)OccupancyType.Storage).Sum(a => a.Count);
                model.Type9 = byYear.Where(a => a.OccupancyType == (int)OccupancyType.Mixed).Sum(a => a.Count);
                model.Type10 = byYear.Where(a => a.OccupancyType == (int)OccupancyType.Miscellaneous).Sum(a => a.Count);
                model.Type11 = byYear.Where(a => a.OccupancyType == (int)OccupancyType.Residential).Sum(a => a.Count);

                model.Total = model.Type1 + model.Type2 + model.Type3 + model.Type4 + model.Type5 +
                              model.Type6 + model.Type7 + model.Type8 + model.Type9 + model.Type10 + model.Type11;

                listInspectionOrders.Add(model);
            }

            return listInspectionOrders;
        }

        public IQueryable<InspectionOrderDashboardModel> GetFilteredFiveYearInspectionOrder()
        {
            var years = GetLastFiveYears();
            var filteredInspections = (
                from insp in context.tblInspectionOrders
                where insp.IO_InspectionDate.HasValue
                select new InspectionOrderModel()
                {
                    IO_Est_Id = insp.IO_Est_Id,
                    IO_Unit_Id = insp.IO_Unit_Id,
                    RegionId = insp.tblUnits.tblCityMunicipality.tblProvinces.Region_Id,
                    ProvinceID = insp.tblUnits.tblCityMunicipality.Municipality_Province_Id,
                    IO_DashInspectionDate = insp.IO_InspectionDate
                }).AsQueryable();

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
                filteredInspections =
                    filteredInspections.Where(
                        a => a.RegionId == CurrentUser.RegionID);
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
                filteredInspections =
                    filteredInspections.Where(
                        a => a.ProvinceID == CurrentUser.ProvinceID);
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                filteredInspections = filteredInspections.Where(a => a.IO_Unit_Id == CurrentUser.EmployeeUnitId);

            var inspectionOrders = (from a in filteredInspections
                join b in context.tblEstablishments on a.IO_Est_Id equals b.Ref_Est_Id
                where years.Contains(a.IO_DashInspectionDate.Value.Year)
                select new InspectionOrderDashboardModel
                {
                    Year = a.IO_DashInspectionDate.Value.Year,
                    OccupancyType = b.Est_OccupancyType
                });

            return inspectionOrders;
        }

        //public List<FirePreventionDashboardModel> GetFirePreventionActivities()
        //{
        //    var years = GetLastFiveYears();

        //    var filteredFSIC = GetFilteredFSIC();
        //    var filteredNTC = GetFilteredNTC();
        //    var filteredNTCV = GetFilteredNTCV();
        //    var filteredEstablishment = GetFilteredEstablishment();
        //    var filteredFSEC = GetFilteredFSEC();

        //    var fsic = filteredFSIC
        //                           .Where(a => a.FSIC_ApplicationType != 3 && a.FSIC_Issue_Date.HasValue)
        //                          .GroupBy(a => new { a.FSIC_Issue_Date.Value.Year, a.FSIC_ApplicationType })
        //                          .Select(g => new { g.Key.Year, g.Key.FSIC_ApplicationType, Count = g.Count() })
        //                          .ToList();

        //    var fsec = filteredFSEC
        //                         .Where(a => a.FSEC_Released_Date.HasValue)
        //                         .GroupBy(a => new { a.FSEC_Released_Date.Value.Year })
        //                         .Select(g => new { g.Key.Year, Count = g.Count() })
        //                         .ToList();

        //    var ntc = filteredNTC
        //                         .GroupBy(a => new { a.NTC_IssueDate.Year })
        //                         .Select(g => new { g.Key.Year, Count = g.Count() })
        //                         .ToList();


        //    var ntcv = filteredNTCV
        //                         .GroupBy(a => new { a.NTCV_IssueDate.Year })
        //                         .Select(g => new { g.Key.Year, Count = g.Count() })
        //                         .ToList();


        //    var establishment = filteredEstablishment
        //                         .Where(a => a.Est_EstablishmentStatus == (int)EstablishmentStatus.Issued_Abatement_Order || a.Est_EstablishmentStatus == (int)EstablishmentStatus.Issued_Closure_Notice)
        //                         .GroupBy(a => new { a.Est_CreatedDate.Year, a.Est_EstablishmentStatus })
        //                         .Select(g => new { g.Key.Est_EstablishmentStatus, g.Key.Year, Count = g.Count() })
        //                         .ToList();

        //    var fireCodeFees = context.vwFeesByYearAndRegion;
        //    var listPrevention = new List<FirePreventionDashboardModel>();

        //    foreach (var year in years.OrderBy(y => y))
        //    {
        //        var byYears = fsic.Where(a => a.Year == year).ToList();
        //        var model = new FirePreventionDashboardModel();
        //        model.Year = year;
        //        model.FSIC_Occupancy = byYears.Where(a => a.FSIC_ApplicationType == (int)FSIC_Type.Occupancy).Sum(a => a.Count);
        //        model.FSIC_Business_Permit = byYears.Where(a => a.FSIC_ApplicationType == (int)FSIC_Type.Business ||
        //                                                     a.FSIC_ApplicationType == (int)FSIC_Type.PermitToOperate).Sum(a => a.Count);
        //        model.FSIC_Total = model.FSIC_Occupancy + model.FSIC_Business_Permit;

        //        model.NTC = ntc.Where(a => a.Year == year).Sum(a => a.Count);
        //        model.NTCV = ntcv.Where(a => a.Year == year).Sum(a => a.Count);
        //        model.Abatement = establishment.Where(a => a.Year == year && a.Est_EstablishmentStatus == (int)EstablishmentStatus.Issued_Abatement_Order).Sum(a => a.Count);
        //        model.Closure = establishment.Where(a => a.Year == year && a.Est_EstablishmentStatus == (int)EstablishmentStatus.Issued_Closure_Notice).Sum(a => a.Count);
        //        model.BuildingPlans = fsec.Where(a => a.Year == year).Sum(a => a.Count);
        //        model.Amount_FireCodeFees_Assessed = fireCodeFees.Where(a => a.Collected_Date == year).Sum(a => a.TotalFee) ?? 0;

        //        listPrevention.Add(model);
        //    }
        //    listPrevention[0].Total_Amount_FireCodeFees_Assessed = listPrevention.Sum(a => a.Amount_FireCodeFees_Assessed);
        //    return listPrevention;
        //}

        public List<tblFSICApplication> GetFilteredFSIC()
        {
            var fsicApplication = from a in context.tblFSICApplication
                                  join b in context.tblUnits on a.FSIC_Unit_Id equals b.Unit_Id
                                  join c in context.tblCityMunicipality on b.Unit_Municipality_Id equals c.Municipality_Id
                                  join d in context.tblProvinces on c.Municipality_Province_Id equals d.Province_Id
                                  join e in context.tblRegions on d.Region_Id equals e.Reg_Id
                                  join f in context.tblEstablishments on a.FSIC_Est_Id equals f.Ref_Est_Id
                                  where f.Est_EstablishmentStatus == (int)EstablishmentStatus.Issued_FSIC
                                  select new
                                  {
                                      fsic = a, province = d, region = e
                                  };


            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
                fsicApplication = fsicApplication.Where(a => a.region.Reg_Id == CurrentUser.RegionID);
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
                fsicApplication = fsicApplication.Where(a => a.province.Province_Id == CurrentUser.ProvinceID);
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                fsicApplication = fsicApplication.Where(a => a.fsic.FSIC_Unit_Id == CurrentUser.EmployeeUnitId);

            return fsicApplication.Select(a => a.fsic).ToList();

        }

        public List<tblNoticeToComply> GetFilteredNTC()
        {
            var noticeToComply = from a in context.tblNoticeToComply
                                 join b in context.tblUnits on a.NTC_Unit_Id equals b.Unit_Id
                                 join c in context.tblCityMunicipality on b.Unit_Municipality_Id equals c.Municipality_Id
                                 join d in context.tblProvinces on c.Municipality_Province_Id equals d.Province_Id
                                 join e in context.tblRegions on d.Region_Id equals e.Reg_Id
                                 join f in context.tblEstablishments on a.NTC_Est_Id equals f.Ref_Est_Id
                                 where f.Est_EstablishmentStatus == (int)EstablishmentStatus.Issued_NTC
                                 select new { ntc = a, province = d, region = e };


            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
                noticeToComply = noticeToComply.Where(a => a.region.Reg_Id == CurrentUser.RegionID);
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
                noticeToComply = noticeToComply.Where(a => a.province.Province_Id == CurrentUser.ProvinceID);
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                noticeToComply = noticeToComply.Where(a => a.ntc.NTC_Unit_Id == CurrentUser.EmployeeUnitId);

            return noticeToComply.Select(a => a.ntc).ToList();

        }

        public List<tblNoticeToCorrectViolations> GetFilteredNTCV()
        {
            var noticeToComply = from a in context.tblNoticeToCorrectViolations
                                 join b in context.tblUnits on a.NTCV_Unit_Id equals b.Unit_Id
                                 join c in context.tblCityMunicipality on b.Unit_Municipality_Id equals c.Municipality_Id
                                 join d in context.tblProvinces on c.Municipality_Province_Id equals d.Province_Id
                                 join e in context.tblRegions on d.Region_Id equals e.Reg_Id
                                 join f in context.tblEstablishments on a.NTCV_Est_Id equals f.Ref_Est_Id
                                 where f.Est_EstablishmentStatus == (int)EstablishmentStatus.Issued_NTCV
                                 select new { ntcv = a, province = d, region = e };


            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
                noticeToComply = noticeToComply.Where(a => a.region.Reg_Id == CurrentUser.RegionID);
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
                noticeToComply = noticeToComply.Where(a => a.province.Province_Id == CurrentUser.ProvinceID);
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                noticeToComply = noticeToComply.Where(a => a.ntcv.NTCV_Unit_Id == CurrentUser.EmployeeUnitId);

            return noticeToComply.Select(a => a.ntcv).ToList();

        }

        public List<tblFSECApplication> GetFilteredFSEC()
        {
            var fsecApplication = from a in context.tblFSECApplication
                                  join b in context.tblUnits on a.FSEC_Unit_Id equals b.Unit_Id
                                  join c in context.tblCityMunicipality on b.Unit_Municipality_Id equals c.Municipality_Id
                                  join d in context.tblProvinces on c.Municipality_Province_Id equals d.Province_Id
                                  join e in context.tblRegions on d.Region_Id equals e.Reg_Id
                                  join f in context.tblEstablishments on a.FSEC_Est_Id equals f.Ref_Est_Id
                                  where f.Est_EstablishmentStatus == (int)EstablishmentStatus.Issued_FSEC
                                  select new { fsec = a, province = d, region = e };


            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
                fsecApplication = fsecApplication.Where(a => a.region.Reg_Id == CurrentUser.RegionID);
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
                fsecApplication = fsecApplication.Where(a => a.province.Province_Id == CurrentUser.ProvinceID);
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                fsecApplication = fsecApplication.Where(a => a.fsec.FSEC_Unit_Id == CurrentUser.EmployeeUnitId);

            return fsecApplication.Select(a => a.fsec).ToList();

        }
        
        public FCREstablishmentModel GetEstablishmentCounter(int municipalityId)
        {
            var Compliant = Functions.CompliantStatus();
            var NonCompliant = Functions.NonCompliantStatus();
            var Closure = Functions.ClosureStatus();
            var establishments = context.tblEstablishments.AsQueryable();

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
                establishments =
                    establishments.Where(
                        a => a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID);

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
                establishments =
                    establishments.Where(
                        a => a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == CurrentUser.ProvinceID);

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                establishments =
                    establishments.Where(
                        a => a.Est_Unit_Id == CurrentUser.EmployeeUnitId);

            if (PageSecurity.HasAccess(PageArea.FPSS_Establishment_RestricttoStation))
                establishments = establishments.Where(a => a.Est_Unit_Id == CurrentUser.EmployeeUnitId);

            establishments = establishments.Where(a => a.tblUnits.Unit_Municipality_Id == municipalityId);

            var counter = new FCREstablishmentModel
            {
                Compliant = establishments.Count(a => Compliant.Contains(a.Est_EstablishmentStatus)),
                NonCompliant = establishments.Count(a => NonCompliant.Contains(a.Est_EstablishmentStatus)),
                Closure = establishments.Count(a => Closure.Contains(a.Est_EstablishmentStatus))
            };
            counter.Total = counter.Compliant + counter.NonCompliant + counter.Closure;

            return counter;

        }

        public DashboardPersonnelModel GetDashPersonnelPerMunicipality(int municipalityId)
        {
            var emp = from a in context.tblEmployees
                      where
                          a.Emp_Curr_Rank != null && a.Emp_Curr_Unit != null && a.Emp_DutyStatus == (int)DutyStatuses.Active
                          && a.Emp_IsDeleted == false
                      select new EmployeeModel
                      {
                          RegionID = a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id,
                          ProvinceID = a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id,
                          Emp_Curr_Unit = a.Emp_Curr_Unit,
                          Emp_Curr_Rank = a.Emp_Curr_Rank,
                          Emp_Curr_JobFunc = a.Emp_Curr_JobFunc,
                          MunicipalityID = a.tblUnits.Unit_Municipality_Id
                      };

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
                emp = emp.Where(a => a.RegionID == CurrentUser.RegionID);
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
                emp = emp.Where(a => a.ProvinceID == CurrentUser.ProvinceID);
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                emp = emp.Where(a => a.Emp_Curr_Unit == CurrentUser.EmployeeUnitId);

            emp = emp.Where(a => a.MunicipalityID == municipalityId);

            var officerRanks = Functions.OfficerRanks();
            var nonOfficerRanks = Functions.NonOfficerRanks();

            var personnelModel = new DashboardPersonnelModel();

            personnelModel.NUP = emp.Count(a => a.Emp_Curr_Rank == (int) Rank.NUP);
            personnelModel.Officer = emp.Count(a => officerRanks.Contains(a.Emp_Curr_Rank.Value));
            personnelModel.NonOfficer = emp.Count(a => nonOfficerRanks.Contains(a.Emp_Curr_Rank.Value));
            personnelModel.GeneralAdmin = emp.Count(a => a.Emp_Curr_JobFunc == (int) JobFunction.GeneralAdmin);
            personnelModel.Operations =
                emp.Count(a => a.Emp_Curr_JobFunc != (int) JobFunction.GeneralAdmin && a.Emp_Curr_JobFunc != null);

            return personnelModel;
        }
    }
}