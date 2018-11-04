using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EBFP.BL.CIS;
using EBFP.BL.Helper;
using EBFP.BL.Inventory;
using EBFP.DataAccess;
using EBFP.Helper;
using Queries.Core.Repositories;

namespace EBFP.BL.HumanResources
{
    public class MunicipalityBL : Repository<tblCityMunicipality, MunicipalityModel>, IMunicipality
    {
        public MunicipalityBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public MunicipalityListResult GetListResult(GridInfo gridInfo)
        {
            var municipality = BFPContext.tblCityMunicipality
                .Select(unit => new MunicipalityModel
                {
                    Municipality_Reg_Id = unit.tblProvinces.tblRegions.Reg_Id,
                    Reg_Description = unit.tblProvinces.tblRegions.Reg_Description,
                    Province_Name = unit.tblProvinces.Province_Name,
                    Municipality_Name = unit.Municipality_Name,
                    Municipality_Id = unit.Municipality_Id,
                    Municipality_NSCB = unit.Municipality_NSCB,
                    Municipality_Province_Id = unit.Municipality_Province_Id
                });

            if (!PageSecurity.HasAccess(PageArea.HRIS_Municipality_CanViewAll))
            {
                if (PageSecurity.HasAccess(PageArea.HRIS_Municipality_RestricttoRegion))
                    municipality = municipality.Where(a => a.Municipality_Reg_Id == CurrentUser.RegionID);

                if (PageSecurity.HasAccess(PageArea.HRIS_Municipality_RestricttoProvince))
                    municipality = municipality.Where(a => a.Municipality_Province_Id == CurrentUser.ProvinceID);
            }

            var searchMunicipalityModel = gridInfo.searchMunicipalityModel;

            if (!string.IsNullOrEmpty(searchMunicipalityModel.MunicipalityId))
                municipality =
                    municipality.Where(a => a.Municipality_Name.Contains(searchMunicipalityModel.MunicipalityId));

            if (!string.IsNullOrEmpty(searchMunicipalityModel.Municipality_NSCB))
                municipality =
                    municipality.Where(a => a.Municipality_NSCB.Contains(searchMunicipalityModel.Municipality_NSCB));

            if (searchMunicipalityModel.RegionId > 0)
                municipality = municipality.Where(a => a.Municipality_Reg_Id == searchMunicipalityModel.RegionId);

            if (searchMunicipalityModel.ProvinceId > 0)
                municipality =
                    municipality.Where(a => a.Municipality_Province_Id == searchMunicipalityModel.ProvinceId);

            gridInfo.recordsTotal = municipality.Select(a => a.Municipality_Id).Count();
            municipality = municipality.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
                .Skip(gridInfo.start)
                .Take(gridInfo.length);

            return new MunicipalityListResult
            {
                MunicipalityList = municipality.ToList(),
                DatatableInfo = gridInfo
            };
        }

        public List<SelectionUnitModel> GetAllForSelection()
        {
            var ret = new List<SelectionUnitModel>();

            var regions = BFPContext.tblRegions
                .OrderBy(a => a.Reg_Id)
                .ToList();

            var units = BFPContext.tblUnits
                .Select(a => new
                {
                    a.tblCityMunicipality.tblProvinces.Region_Id,
                    a.tblCityMunicipality.tblProvinces.Province_Name,
                    a.Unit_StationName,
                    a.Unit_Id,
                    a.Unit_Code
                }).ToList();

            foreach (var region in regions)
            {
                var unitModel = new SelectionUnitModel
                {
                    Reg_Id = region.Reg_Id,
                    Reg_Description = region.Reg_Description
                };
                foreach (var unit in units.Where(a => a.Region_Id == region.Reg_Id))
                    unitModel.Units.Add(new UnitModel
                    {
                        Province_Name = unit.Province_Name,
                        Unit_StationName = unit.Unit_StationName,
                        Unit_Id = unit.Unit_Id,
                        Unit_Code = unit.Unit_Code
                    });
                ret.Add(unitModel);
            }

            return ret;
        }

        public MunicipalityModel GetMunicipalityById(int municipalityId)
        {
            var ret = new MunicipalityModel();

            var res = BFPContext.tblCityMunicipality.Select(x => new
            {
                x.tblProvinces.tblRegions.Reg_Description,
                x.tblProvinces.Region_Id,
                x.tblProvinces.Province_Name,
                x.Municipality_Id,
                x.Municipality_WithBuilding,
                municipality = x,
                Reg_Id =
                BFPContext.tblRegions.Where(a => a.Reg_Id == x.tblProvinces.Region_Id)
                    .Select(a => a.Reg_Id)
                    .FirstOrDefault()
            }).FirstOrDefault(a => a.Municipality_Id == municipalityId);

            if (res?.municipality != null)
            {
                Mapper.Map(res.municipality, ret);

                ret.FireRecordsList =
                res.municipality.tblFireRecords.OrderBy(a => a.Fire_Year).AsQueryable()
                     .Project().To<FireRecordsModel>().ToList();

                ret.PopulationList =
                  res.municipality.tblPopulations.OrderBy(a => a.Population_Year).AsQueryable()
                       .Project().To<PopulationModel>().ToList();

                ret.Total_Stations = res.municipality.tblUnits.Count +
                                     res.municipality.tblUnits.Select(a => new {truckCount = a.tblUnitSubStation.Count})
                                         .Sum(a => a.truckCount);
                ret.Total_Trucks =
                    res.municipality.tblUnits.Select(a => new {countTrucks = a.tblTrucks?.Count})
                        .Sum(a => a.countTrucks);
                ret.Total_OtherVehicles =
                    res.municipality.tblUnits.Select(a => new {countVehicle = a.tblOtherVehicles?.Count})
                        .Sum(a => a.countVehicle);


                ret.Reg_Description = res.Reg_Description;
                ret.Province_Name = res.Province_Name;
                ret.Municipality_Reg_Id = res.Reg_Id;
                ret.Municipality_WithBuilding = res.Municipality_WithBuilding ?? false;
            }
            return ret;
        }

        public void UpdateMunicipality(MunicipalityModel model)
        {
            var municipalityDet =
                BFPContext.tblCityMunicipality.FirstOrDefault(a => a.Municipality_Id == model.Municipality_Id);
            if (municipalityDet == null) throw new Exception("Municipality cannot be found!");

            municipalityDet.Municipality_NSCB = model.Municipality_NSCB;
            municipalityDet.Municipality_Province_Id = model.Municipality_Province_Id;
            municipalityDet.Municipality_Name = model.Municipality_Name;
            municipalityDet.Municipality_Type = model.Municipality_Type;
            municipalityDet.Municipality_WithBuilding = model.Municipality_WithBuilding;

            BFPContext.Entry(municipalityDet).State = EntityState.Modified;
            BFPContext.SaveChanges();
        }

        public bool DeleteByID(int municipalityId)
        {
            var municipality = BFPContext.tblCityMunicipality
                .FirstOrDefault(a => a.Municipality_Id == municipalityId);

            if (municipality != null)
            {
                BFPContext.tblCityMunicipality.Remove(municipality);
                BFPContext.SaveChanges();
            }

            return true;
        }

        public List<MunicipalityModel> GetMunicipalityPerProvince(int provinceId)
        {
            var list = new List<MunicipalityModel>();
            var ret = BFPContext.tblCityMunicipality.Where(a => a.Municipality_Province_Id == provinceId)
                .OrderBy(a => a.Municipality_Name).ToList();

            foreach (var item in ret)
            {
                var model = new MunicipalityModel
                {
                    Municipality_Id = item.Municipality_Id,
                    Municipality_Name = item.Municipality_Name
                };
                list.Add(model);
            }


            return list;
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblCityMunicipality, MunicipalityModel>().ReverseMap();
            Mapper.CreateMap<List<tblCityMunicipality>, List<MunicipalityModel>>().ReverseMap();
            Mapper.CreateMap<List<tblCityMunicipality>, List<MunicipalityModel>>();

            Mapper.CreateMap<tblFireRecords, FireRecordsModel>().ReverseMap();
            Mapper.CreateMap<tblPopulations, PopulationModel>().ReverseMap();
        }
        
        public List<FireFightingModel> GetFireFightingCountDetails()
        {
            var fireFightingList = new List<FireFightingModel>();

         var municipalities = (
                        from mun in BFPContext.tblCityMunicipality
                        join unit in BFPContext.tblUnits on mun.Municipality_Id equals unit.Unit_Municipality_Id
                        select new MunicipalityModel
                        {
                            Municipality_Id = mun.Municipality_Id,
                            Municipality_Type = mun.Municipality_Type,
                            Municipality_WithBuilding = mun.Municipality_WithBuilding ?? false,
                            Municipality_Reg_Id = mun.tblProvinces.Region_Id,
                            ProvinceId = mun.tblProvinces.Province_Id,
                            UnitId = unit.Unit_Id
                        });

            var subStations = (
                from a in BFPContext.tblUnitSubStation
                where a.Sub_WithBuilding == true
                select new SubStationModel
                {
                    RegionId = a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id,
                    ProvinceId = a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id,
                    Sub_Unit_Id = a.Sub_Unit_Id
                });

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
            {
                var provinces = BFPContext.tblProvinces.Where(a => a.Region_Id == CurrentUser.RegionID);
                foreach (var prov in provinces)
                {
                    var municipalitiesProv = municipalities.Where(a => a.ProvinceId == prov.Province_Id).ToList();
                    var fireFightingModel = new FireFightingModel();
                    var municipalitiesWithFireTrucks = GetMunicipalitiesWithFireTrucks(municipalitiesProv);

                    var subStationByProv = subStations.Where(a => a.ProvinceId == prov.Province_Id).ToList();

                    fireFightingModel.ProvinceId = prov.Province_Id;
                    fireFightingModel.Province = prov.Province_Name;

                    fireFightingModel.TotalCitiesWithBuildingAndFireTrucks = municipalitiesProv.Count(a => a.Municipality_Type == (int)MunicipalityType.City &&
                                                                                                    municipalitiesWithFireTrucks.Contains(a.Municipality_Id) &&
                                                                                                    a.Municipality_WithBuilding == true);

                    fireFightingModel.TotalMunicipalitiesWithBuildingAndFireTrucks = municipalitiesProv.Count(a => a.Municipality_Type == (int)MunicipalityType.Municipality &&
                                                                                                    municipalitiesWithFireTrucks.Contains(a.Municipality_Id) &&
                                                                                                    a.Municipality_WithBuilding == true);

                    fireFightingModel.WithBuildingAndFireTrucksSubtotal = (fireFightingModel.TotalCitiesWithBuildingAndFireTrucks + fireFightingModel.TotalMunicipalitiesWithBuildingAndFireTrucks);


                    fireFightingModel.MunicipalitiesWithFireTrucksOnly = municipalitiesProv.Count(a => a.Municipality_WithBuilding != true &&
                                                                                    municipalitiesWithFireTrucks.Contains(a.Municipality_Id));

                    fireFightingModel.MunicipalitiesWithoutFireTrucks = municipalitiesProv.Count(a => !municipalitiesWithFireTrucks.Contains(a.Municipality_Id));

                    fireFightingModel.MunicipalitiesWithFireStationBLDGOnly = municipalitiesProv.Count(a => a.Municipality_WithBuilding == true &&
                                                                                                    !municipalitiesWithFireTrucks.Contains(a.Municipality_Id));

                    fireFightingModel.MunicipalitiesWithoutFireTrucksAndStation = municipalitiesProv.Count(a => !municipalitiesWithFireTrucks.Contains(a.Municipality_Id) &&
                                                                                                                a.Municipality_WithBuilding != true);

                    fireFightingModel.MunicipalitiesWithoutFireStationBLDG = municipalitiesProv.Count(a => a.Municipality_WithBuilding != true);

                    fireFightingModel.TotalCities = municipalitiesProv.Count(a => a.Municipality_Type == (int)MunicipalityType.City);

                    fireFightingModel.TotalMunicipalities = municipalitiesProv.Count(a => a.Municipality_Type == (int)MunicipalityType.Municipality);

                    fireFightingModel.TotalCitiesAndMunicipalities = (fireFightingModel.TotalCities + fireFightingModel.TotalMunicipalities);
                    
                    fireFightingModel.NumberOfFireSubStationBuilding = subStationByProv.Count();

                    fireFightingList.Add(fireFightingModel);
                }

            }
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
            {

                var units = BFPContext.tblUnits.Where(a => a.Unit_ProvDistrict == CurrentUser.ProvinceID);
                foreach (var unit in units)
                {
                    var municipalitiesProv = municipalities.Where(a => a.UnitId == unit.Unit_Id).ToList();
                    var fireFightingModel = new FireFightingModel();
                    var municipalitiesWithFireTrucks = GetMunicipalitiesWithFireTrucks(municipalitiesProv);

                    var subStationByStation = subStations.Where(a => a.Sub_Unit_Id == unit.Unit_Id).ToList();

                    fireFightingModel.UnitId = unit.Unit_Id;
                    fireFightingModel.Unit = unit.Unit_StationName;

                    fireFightingModel.TotalCitiesWithBuildingAndFireTrucks = municipalitiesProv.Count(a => a.Municipality_Type == (int)MunicipalityType.City &&
                                                                                                    municipalitiesWithFireTrucks.Contains(a.Municipality_Id) &&
                                                                                                    a.Municipality_WithBuilding == true);

                    fireFightingModel.TotalMunicipalitiesWithBuildingAndFireTrucks = municipalitiesProv.Count(a => a.Municipality_Type == (int)MunicipalityType.Municipality &&
                                                                                                    municipalitiesWithFireTrucks.Contains(a.Municipality_Id) &&
                                                                                                    a.Municipality_WithBuilding == true);

                    fireFightingModel.WithBuildingAndFireTrucksSubtotal = (fireFightingModel.TotalCitiesWithBuildingAndFireTrucks + fireFightingModel.TotalMunicipalitiesWithBuildingAndFireTrucks);


                    fireFightingModel.MunicipalitiesWithFireTrucksOnly = municipalitiesProv.Count(a => a.Municipality_WithBuilding != true &&
                                                                                    municipalitiesWithFireTrucks.Contains(a.Municipality_Id));

                    fireFightingModel.MunicipalitiesWithoutFireTrucks = municipalitiesProv.Count(a => !municipalitiesWithFireTrucks.Contains(a.Municipality_Id));

                    fireFightingModel.MunicipalitiesWithFireStationBLDGOnly = municipalitiesProv.Count(a => a.Municipality_WithBuilding == true &&
                                                                                                    !municipalitiesWithFireTrucks.Contains(a.Municipality_Id));

                    fireFightingModel.MunicipalitiesWithoutFireTrucksAndStation = municipalitiesProv.Count(a => !municipalitiesWithFireTrucks.Contains(a.Municipality_Id) &&
                                                                                                                a.Municipality_WithBuilding != true);

                    fireFightingModel.MunicipalitiesWithoutFireStationBLDG = municipalitiesProv.Count(a => a.Municipality_WithBuilding != true);

                    fireFightingModel.TotalCities = municipalitiesProv.Count(a => a.Municipality_Type == (int)MunicipalityType.City);

                    fireFightingModel.TotalMunicipalities = municipalitiesProv.Count(a => a.Municipality_Type == (int)MunicipalityType.Municipality);

                    fireFightingModel.TotalCitiesAndMunicipalities = (fireFightingModel.TotalCities + fireFightingModel.TotalMunicipalities);

                    fireFightingModel.NumberOfFireSubStationBuilding = subStationByStation.Count();

                    fireFightingList.Add(fireFightingModel);
                }
            }
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
            {
                var unit = BFPContext.tblUnits.FirstOrDefault(a => a.Unit_Id == CurrentUser.EmployeeUnitId);

                var municipalitiesProv = municipalities.Where(a => a.UnitId == unit.Unit_Id).ToList();
                var fireFightingModel = new FireFightingModel();
                var municipalitiesWithFireTrucks = GetMunicipalitiesWithFireTrucks(municipalitiesProv);

                var subStationByStation = subStations.Where(a => a.Sub_Unit_Id == unit.Unit_Id).ToList();

                fireFightingModel.UnitId = unit.Unit_Id;
                fireFightingModel.Unit = unit.Unit_StationName;

                fireFightingModel.TotalCitiesWithBuildingAndFireTrucks =
                    municipalitiesProv.Count(a => a.Municipality_Type == (int) MunicipalityType.City &&
                                                  municipalitiesWithFireTrucks.Contains(a.Municipality_Id) &&
                                                  a.Municipality_WithBuilding == true);

                fireFightingModel.TotalMunicipalitiesWithBuildingAndFireTrucks =
                    municipalitiesProv.Count(a => a.Municipality_Type == (int) MunicipalityType.Municipality &&
                                                  municipalitiesWithFireTrucks.Contains(a.Municipality_Id) &&
                                                  a.Municipality_WithBuilding == true);

                fireFightingModel.WithBuildingAndFireTrucksSubtotal =
                    (fireFightingModel.TotalCitiesWithBuildingAndFireTrucks +
                     fireFightingModel.TotalMunicipalitiesWithBuildingAndFireTrucks);


                fireFightingModel.MunicipalitiesWithFireTrucksOnly =
                    municipalitiesProv.Count(a => a.Municipality_WithBuilding != true &&
                                                  municipalitiesWithFireTrucks.Contains(a.Municipality_Id));

                fireFightingModel.MunicipalitiesWithoutFireTrucks =
                    municipalitiesProv.Count(a => !municipalitiesWithFireTrucks.Contains(a.Municipality_Id));

                fireFightingModel.MunicipalitiesWithFireStationBLDGOnly =
                    municipalitiesProv.Count(a => a.Municipality_WithBuilding == true &&
                                                  !municipalitiesWithFireTrucks.Contains(a.Municipality_Id));

                fireFightingModel.MunicipalitiesWithoutFireTrucksAndStation =
                    municipalitiesProv.Count(a => !municipalitiesWithFireTrucks.Contains(a.Municipality_Id) &&
                                                  a.Municipality_WithBuilding != true);

                fireFightingModel.MunicipalitiesWithoutFireStationBLDG =
                    municipalitiesProv.Count(a => a.Municipality_WithBuilding != true);

                fireFightingModel.TotalCities =
                    municipalitiesProv.Count(a => a.Municipality_Type == (int) MunicipalityType.City);

                fireFightingModel.TotalMunicipalities =
                    municipalitiesProv.Count(a => a.Municipality_Type == (int) MunicipalityType.Municipality);

                fireFightingModel.TotalCitiesAndMunicipalities = (fireFightingModel.TotalCities +
                                                                  fireFightingModel.TotalMunicipalities);

                fireFightingModel.NumberOfFireSubStationBuilding = subStationByStation.Count();

                fireFightingList.Add(fireFightingModel);
            }
            else
            {
                if (!(PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation) ||
                      PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince) ||
                      PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion)))
                {
                    var excludedRegion = new List<int> {19, 20}; //NTFI & NHQ
                    var regions = BFPContext.tblRegions.Where(a => !excludedRegion.Contains(a.Reg_Id));
                    foreach (var reg in regions)
                    {
                        var municipalitiesByRegion =
                            municipalities.Where(a => a.Municipality_Reg_Id == reg.Reg_Id).ToList();
                        var fireFightingModel = new FireFightingModel();
                        var municipalitiesWithFireTrucks = GetMunicipalitiesWithFireTrucks(municipalitiesByRegion);

                        var subStationByRegion = subStations.Where(a => a.RegionId == reg.Reg_Id).ToList();

                        fireFightingModel.RegionId = reg.Reg_Id;
                        fireFightingModel.Region = reg.Reg_Title;


                        fireFightingModel.TotalCitiesWithBuildingAndFireTrucks =
                            municipalitiesByRegion.Count(a => a.Municipality_Type == (int) MunicipalityType.City &&
                                                              municipalitiesWithFireTrucks.Contains(a.Municipality_Id) &&
                                                              a.Municipality_WithBuilding == true);

                        fireFightingModel.TotalMunicipalitiesWithBuildingAndFireTrucks =
                            municipalitiesByRegion.Count(
                                a => a.Municipality_Type == (int) MunicipalityType.Municipality &&
                                     municipalitiesWithFireTrucks.Contains(a.Municipality_Id) &&
                                     a.Municipality_WithBuilding == true);

                        fireFightingModel.WithBuildingAndFireTrucksSubtotal =
                            (fireFightingModel.TotalCitiesWithBuildingAndFireTrucks +
                             fireFightingModel.TotalMunicipalitiesWithBuildingAndFireTrucks);


                        fireFightingModel.MunicipalitiesWithFireTrucksOnly =
                            municipalitiesByRegion.Count(a => a.Municipality_WithBuilding != true &&
                                                              municipalitiesWithFireTrucks.Contains(a.Municipality_Id));

                        fireFightingModel.MunicipalitiesWithoutFireTrucks =
                            municipalitiesByRegion.Count(a => !municipalitiesWithFireTrucks.Contains(a.Municipality_Id));

                        fireFightingModel.MunicipalitiesWithFireStationBLDGOnly =
                            municipalitiesByRegion.Count(a => a.Municipality_WithBuilding == true &&
                                                              !municipalitiesWithFireTrucks.Contains(a.Municipality_Id));

                        fireFightingModel.MunicipalitiesWithoutFireTrucksAndStation =
                            municipalitiesByRegion.Count(
                                a => !municipalitiesWithFireTrucks.Contains(a.Municipality_Id) &&
                                     a.Municipality_WithBuilding != true);

                        fireFightingModel.MunicipalitiesWithoutFireStationBLDG =
                            municipalitiesByRegion.Count(a => a.Municipality_WithBuilding != true);

                        fireFightingModel.TotalCities =
                            municipalitiesByRegion.Count(a => a.Municipality_Type == (int) MunicipalityType.City);

                        fireFightingModel.TotalMunicipalities =
                            municipalitiesByRegion.Count(a => a.Municipality_Type == (int) MunicipalityType.Municipality);

                        fireFightingModel.TotalCitiesAndMunicipalities = (fireFightingModel.TotalCities +
                                                                          fireFightingModel.TotalMunicipalities);

                        fireFightingModel.NumberOfFireSubStationBuilding = subStationByRegion.Count();

                        fireFightingList.Add(fireFightingModel);
                    }
                }
            }

            return fireFightingList;
        }

        private List<int> GetMunicipalitiesWithFireTrucks(IEnumerable<MunicipalityModel> tblCityMunicipality)
        {
            return (from mun in tblCityMunicipality
                join unit in BFPContext.tblUnits on mun.Municipality_Id equals unit.Unit_Municipality_Id
                join truck in BFPContext.tblTrucks on unit.Unit_Id equals truck.Truck_UnitId
                    select mun.Municipality_Id).Distinct().ToList();

        }

        public int GetPopulation()
        {
            var populations = new List<tblPopulations>();

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
                populations = BFPContext.tblPopulations.Where(a => a.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID).ToList();
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
                populations = BFPContext.tblPopulations.Where(a => a.tblCityMunicipality.tblProvinces.Province_Id == CurrentUser.ProvinceID).ToList();
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                populations = (from population in BFPContext.tblPopulations
                        join unit in BFPContext.tblUnits on population.Population_Municipality_Id equals unit.Unit_Municipality_Id
                               where unit.Unit_Id == CurrentUser.EmployeeUnitId
                        select population).ToList();
            else
                populations = BFPContext.tblPopulations.ToList();

            return populations.Sum(a => a.Population_Count ?? 0); 
        }

        public bool CheckMunicipality(int municipalityId)
        {
            var unit = BFPContext.tblUnits.FirstOrDefault(a => a.Unit_Municipality_Id == municipalityId);
            if (unit != null)
                return true;
            else
                return false;
        }
    }
}