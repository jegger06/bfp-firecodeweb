using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using EBFP.BL.Helper;
using EBFP.DataAccess;
using EBFP.Helper;
using Queries.Core.Repositories;

namespace EBFP.BL.Inventory
{
    public class FireRecordBL : Repository<tblFireRecords, FireRecordsModel>, IFireRecord
    {
        public FireRecordBL(EBFPEntities context) : base(context)
        {
        }

        public void InsertBulk(List<FireRecordsModel> model, int municipalityId)
        {
            if (model == null || model.Count == 0) return;

            foreach (var item in model)
            {
                item.Fire_Municipality_Id = municipalityId;
            }

            InsertBulk(model, a => a.Fire_Municipality_Id == municipalityId);
        }


        public IQueryable<tblFireRecords> GetFilteredFireRecords()
        {
            var incidents = BFPContext.tblFireRecords.AsQueryable();


            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
                incidents = incidents.Where(a => a.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID);
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
                incidents = incidents.Where(a => a.tblCityMunicipality.Municipality_Province_Id == CurrentUser.ProvinceID);
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                incidents = incidents.Where(a => a.Fire_Municipality_Id == CurrentUser.MunicipalityID);

            return incidents;
        }

        public List<FireIncidentStatistics> GetFireIncidentsStatistics()
        {
            var yearToDate = DateTime.Now.Year;
            var years = new List<string> { yearToDate.ToString(), (yearToDate -1).ToString(), (yearToDate -2).ToString(),
                                           (yearToDate -3).ToString(), (yearToDate -4).ToString() };

            var incidents = GetFilteredFireRecords();
          

            return incidents.Where(a => years.Contains(a.Fire_Year))
                        .GroupBy(a => new {year = a.Fire_Year})
                        .Select(g => new FireIncidentStatistics
                        {
                            Year = g.Key.year,
                            FireIncidents = g.Sum(a => (a.Fire_Incidents ?? 0)),
                            Damages = g.Sum(a => (a.Fire_Damages ?? 0)),
                            Deaths = g.Sum(a => a.Fire_Deaths ?? 0),
                            Injuries = g.Sum(a => a.Fire_Injuries ?? 0)
                        }).ToList();
        }


        public IQueryable<FireRecordsModel> GetFilteredFiveYearIncidents()
        {

            var yearToDate = DateTime.Now.Year;
            var years = new List<string> { yearToDate.ToString(), (yearToDate -1).ToString(), (yearToDate -2).ToString(),
                                           (yearToDate -3).ToString(), (yearToDate -4).ToString() };

            var incidents = GetFilteredFireRecords();

            var fireIncidents = (from a in incidents
                                 join b in BFPContext.tblCityMunicipality on a.Fire_Municipality_Id equals b.Municipality_Id

                                 join e in BFPContext.tblUnits on b.Municipality_Id equals e.Unit_Municipality_Id

                                 join c in BFPContext.tblProvinces on b.Municipality_Province_Id equals c.Province_Id
                                 join d in BFPContext.tblRegions on c.Region_Id equals d.Reg_Id
                                 where years.Contains(a.Fire_Year)
                                 select new FireRecordsModel
                                 {
                                     Fire_Incidents = a.Fire_Incidents,
                                     Fire_Injuries = a.Fire_Injuries,
                                     Fire_Deaths = a.Fire_Deaths,
                                     Fire_Damages = a.Fire_Damages,
                                     Fire_Year = a.Fire_Year,
                                     RegionId = d.Reg_Id,

                                     ProvinceId = c.Province_Id,
                                     UnitId = e.Unit_Id

                                 });

            return fireIncidents;
        }

        public List<FireIncidentFiveYearStat> MapFireIncidentDashboardDetails(List<FireIncidentStatistics> fireIncidents)
        {
            var listIncidents = new List<FireIncidentFiveYearStat>();
            var yearToDate = DateTime.Now.Year;
            var years = new List<string> { yearToDate.ToString(), (yearToDate -1).ToString(), (yearToDate -2).ToString(),
                                           (yearToDate -3).ToString(), (yearToDate -4).ToString() };

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
            {
                var provinces = BFPContext.tblProvinces.Where(a => a.Region_Id == CurrentUser.RegionID);
                foreach (var prov in provinces)
                {
                    var byRegion = fireIncidents.Where(a => a.ProvinceId == prov.Province_Id).ToList();

                    var model = new FireIncidentFiveYearStat();
                    model.ProvinceName = prov.Province_Name;
                    model.ProvinceId = prov.Province_Id;

                    model.Year1 = byRegion.Where(a => a.Year == years[4]).Sum(a => a.Count);
                    model.Year2 = byRegion.Where(a => a.Year == years[3]).Sum(a => a.Count);
                    model.Year3 = byRegion.Where(a => a.Year == years[2]).Sum(a => a.Count);
                    model.Year4 = byRegion.Where(a => a.Year == years[1]).Sum(a => a.Count);
                    model.Year5 = byRegion.Where(a => a.Year == years[0]).Sum(a => a.Count);
                    model.Average = Math.Round(((model.Year1 + model.Year2 + model.Year3 + model.Year4 + model.Year5) / (decimal)5), 0);
                    listIncidents.Add(model);
                }
            }
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
            {
                var units = BFPContext.tblUnits.Where(a => a.Unit_ProvDistrict == CurrentUser.ProvinceID).ToList();

                foreach (var unit in units)
                {
                    var byProvince = fireIncidents.Where(a => a.UnitId == unit.Unit_Id).ToList();

                    var model = new FireIncidentFiveYearStat();
                    model.UnitName = unit.Unit_StationName;
                    model.UnitId = unit.Unit_Id;

                    model.Year1 = byProvince.Where(a => a.Year == years[4]).Sum(a => a.Count);
                    model.Year2 = byProvince.Where(a => a.Year == years[3]).Sum(a => a.Count);
                    model.Year3 = byProvince.Where(a => a.Year == years[2]).Sum(a => a.Count);
                    model.Year4 = byProvince.Where(a => a.Year == years[1]).Sum(a => a.Count);
                    model.Year5 = byProvince.Where(a => a.Year == years[0]).Sum(a => a.Count);
                    model.Average = Math.Round(((model.Year1 + model.Year2 + model.Year3 + model.Year4 + model.Year5) / (decimal)5), 0);
                    listIncidents.Add(model);
                }

            }
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
            {
                var unit = BFPContext.tblUnits.FirstOrDefault(a => a.Unit_Id == CurrentUser.EmployeeUnitId);

                var byProvince = fireIncidents.Where(a => a.UnitId == unit.Unit_Id).ToList();

                var model = new FireIncidentFiveYearStat();
                model.UnitName = unit.Unit_StationName;
                model.UnitId = unit.Unit_Id;

                model.Year1 = byProvince.Where(a => a.Year == years[4]).Sum(a => a.Count);
                model.Year2 = byProvince.Where(a => a.Year == years[3]).Sum(a => a.Count);
                model.Year3 = byProvince.Where(a => a.Year == years[2]).Sum(a => a.Count);
                model.Year4 = byProvince.Where(a => a.Year == years[1]).Sum(a => a.Count);
                model.Year5 = byProvince.Where(a => a.Year == years[0]).Sum(a => a.Count);
                model.Average =
                    Math.Round(((model.Year1 + model.Year2 + model.Year3 + model.Year4 + model.Year5)/(decimal) 5), 0);
                listIncidents.Add(model);
            }
            else if (!(PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation) ||
                       PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince) ||
                       PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion)))
            {

                var excludedRegion = new List<int> {19, 20}; //NTFI & NHQ
                var regions = BFPContext.tblRegions.Where(a => !excludedRegion.Contains(a.Reg_Id));
                foreach (var reg in regions)
                {
                    var byRegion = fireIncidents.Where(a => a.RegionId == reg.Reg_Id).ToList();

                    var model = new FireIncidentFiveYearStat();
                    model.RegionName = reg.Reg_Title;
                    model.RegionId = reg.Reg_Id;

                    model.Year1 = byRegion.Where(a => a.Year == years[4]).Sum(a => a.Count);
                    model.Year2 = byRegion.Where(a => a.Year == years[3]).Sum(a => a.Count);
                    model.Year3 = byRegion.Where(a => a.Year == years[2]).Sum(a => a.Count);
                    model.Year4 = byRegion.Where(a => a.Year == years[1]).Sum(a => a.Count);
                    model.Year5 = byRegion.Where(a => a.Year == years[0]).Sum(a => a.Count);
                    model.Average =
                        Math.Round(((model.Year1 + model.Year2 + model.Year3 + model.Year4 + model.Year5)/(decimal) 5),
                            0);
                    listIncidents.Add(model);
                }
            }

            return listIncidents;
        }

        public List<FireIncidentFiveYearStat> GetFireIncidentRespondedTo()
        {
            var incidents = GetFilteredFiveYearIncidents();

            var fireIncidents = incidents
                .GroupBy(a => new {a.Fire_Year, a.RegionId, a.ProvinceId, a.UnitId })
                .Select(g => new FireIncidentStatistics
                {
                    RegionId = g.Key.RegionId,
                    Year = g.Key.Fire_Year,
                    ProvinceId = g.Key.ProvinceId,
                    UnitId = g.Key.UnitId,
                    Count = g.Sum(a => (a.Fire_Incidents ?? 0))
                }).ToList();

            return MapFireIncidentDashboardDetails(fireIncidents);
        }

        public List<FireIncidentFiveYearStat> GetFireIncidentInjured()
        {
            var incidents = GetFilteredFiveYearIncidents();

            var fireIncidents = incidents
                .GroupBy(a => new { a.Fire_Year, a.RegionId, a.ProvinceId, a.UnitId })
                .Select(g => new FireIncidentStatistics
                {
                    RegionId = g.Key.RegionId,
                    Year = g.Key.Fire_Year,
                    ProvinceId = g.Key.ProvinceId,
                    UnitId = g.Key.UnitId,
                    Count = g.Sum(a => (a.Fire_Injuries ?? 0))
                }).ToList();

            return MapFireIncidentDashboardDetails(fireIncidents);
        }

        public List<FireIncidentFiveYearStat> GetFireIncidentDeaths()
        {
            var incidents = GetFilteredFiveYearIncidents();

            var fireIncidents = incidents
                .GroupBy(a => new { a.Fire_Year, a.RegionId, a.ProvinceId, a.UnitId })
                .Select(g => new FireIncidentStatistics
                {
                    RegionId = g.Key.RegionId,
                    Year = g.Key.Fire_Year,
                    ProvinceId = g.Key.ProvinceId,
                    UnitId = g.Key.UnitId,
                    Count = g.Sum(a => (a.Fire_Deaths ?? 0))
                }).ToList();

            return MapFireIncidentDashboardDetails(fireIncidents);
        }

        public List<FireIncidentFiveYearStat> GetFireIncidentDamages()
        {
            var incidents = GetFilteredFiveYearIncidents();

            var fireIncidents = incidents
                .GroupBy(a => new { a.Fire_Year, a.RegionId,a.ProvinceId ,a.UnitId})
                .Select(g => new FireIncidentStatistics
                {
                    RegionId = g.Key.RegionId,
                    Year = g.Key.Fire_Year,
                    ProvinceId = g.Key.ProvinceId,
                    UnitId = g.Key.UnitId,
                    Count = g.Sum(a => (a.Fire_Damages ?? 0))
                }).ToList();

            return MapFireIncidentDashboardDetails(fireIncidents);
        }

    }
}