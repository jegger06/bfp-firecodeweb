using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.DataAccess;
using Queries.Core.Repositories;
using EBFP.Helper;

namespace EBFP.BL.Inventory
{
    public class TruckBL : Repository<tblTrucks, TruckModel>, ITruck
    {
        public TruckBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblTrucks, TruckModel>().ReverseMap();
            Mapper.CreateMap<List<tblTrucks>, List<TruckModel>>().ReverseMap();
            Mapper.CreateMap<List<tblTrucks>, List<TruckModel>>();
        }

        public TruckModel GetTruckById(int truckId)
        {
            var ret = new TruckModel();

            var res = BFPContext.tblTrucks
                .FirstOrDefault(a => a.Truck_Id == truckId);

            if (res != null)
            {
                Mapper.Map(res, ret);
                ret.Municipality_Reg_Id = res.tblUnits.tblCityMunicipality.tblProvinces.Region_Id;
                ret.Municipality_Name = res.tblUnits.tblCityMunicipality.Municipality_Name;
                ret.Municipality_Province_Id = res.tblUnits.tblCityMunicipality.Municipality_Province_Id;
                ret.Municipality_Id = res.tblUnits.Unit_Municipality_Id;
            }


            return ret;
        }
        
        public TruckModel GetTruckByUnitId(int unitId)
        {
            var truckDetails = new TruckModel();
            var ret = BFPContext.tblUnits.FirstOrDefault(a => a.Unit_Id == unitId);

            if (ret != null)
            {
                truckDetails.Municipality_Reg_Id = ret.tblCityMunicipality?.tblProvinces?.Region_Id ?? 0;
                truckDetails.Municipality_Province_Id = ret.tblCityMunicipality?.Municipality_Province_Id ?? 0;
                truckDetails.Municipality_Name = ret.tblCityMunicipality?.Municipality_Name;
                truckDetails.Municipality_Id = ret.tblCityMunicipality?.Municipality_Id ?? 0;
                truckDetails.Truck_UnitId = ret.Unit_Id;
            }

            return truckDetails;
        }

        public IQueryable<tblTrucks> GetFilteredFireTrucks(string sMunicipalityId = "")
        {
            var firetrucks = BFPContext.tblTrucks.AsQueryable();

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
                firetrucks = BFPContext.tblTrucks.Where(a => a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID);
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
                firetrucks = BFPContext.tblTrucks.Where(a => a.tblUnits.tblCityMunicipality.Municipality_Province_Id == CurrentUser.ProvinceID);
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                firetrucks = BFPContext.tblTrucks.Where(a => a.Truck_UnitId == CurrentUser.EmployeeUnitId);

            if (!string.IsNullOrWhiteSpace(sMunicipalityId))
            {
                var municipalityId = Convert.ToInt32(sMunicipalityId.Decrypt());
                firetrucks = firetrucks.Where(a => a.tblUnits.Unit_Municipality_Id == municipalityId);
            }
            
            return firetrucks;
        }

        public List<tblTrucks> GetActualNRFireTrucks()
        {
            return GetFilteredFireTrucks().ToList();
        }

        public TruckListResult GetListResult(GridInfo gridInfo)
        {
            var trucks =BFPContext.tblTrucks
                .Where(a => a.tblUnits.Unit_Municipality_Id == gridInfo.searchTruckModel.Municipality_Id)
                .Select(a => new TruckModel
                {
                    Truck_UnitId = a.Truck_UnitId,
                    Truck_Id = a.Truck_Id,
                    Truck_Id_Code = a.Truck_Id_Code,
                    Unit_StationName = a.tblUnits.Unit_StationName,
                    Truck_Type = a.Truck_Type,
                    Truck_Capacity = a.Truck_Capacity,
                    Truck_Status = a.Truck_Status,
                    Truck_Owner = a.Truck_Owner,
                    Truck_AcquisitionDate = a.Truck_AcquisitionDate,
                    Truck_ManufactureDate = a.Truck_ManufactureDate,
                    Truck_PlateNumber = a.Truck_PlateNumber
                });

            var searchTruckModel = gridInfo.searchTruckModel;
            if (!string.IsNullOrEmpty(searchTruckModel.Truck_Id_Code))
                trucks = trucks.Where(a => a.Truck_Id_Code.Contains(searchTruckModel.Truck_Id_Code));
            if (!string.IsNullOrEmpty(searchTruckModel.Truck_PlateNumber))
                trucks = trucks.Where(a => a.Truck_PlateNumber.Contains(searchTruckModel.Truck_PlateNumber));
            if (searchTruckModel.Truck_Type > 0)
                trucks = trucks.Where(a => a.Truck_Type == searchTruckModel.Truck_Type);
            if (searchTruckModel.Truck_Capacity > 0)
                trucks = trucks.Where(a => a.Truck_Capacity == searchTruckModel.Truck_Capacity);
            if (searchTruckModel.Truck_Status > 0)
                trucks = trucks.Where(a => a.Truck_Status == searchTruckModel.Truck_Status);
            if (searchTruckModel.Truck_Owner > 0)
                trucks = trucks.Where(a => a.Truck_Owner == searchTruckModel.Truck_Owner);
            if (searchTruckModel.Truck_UnitId > 0)
                trucks = trucks.Where(a => a.Truck_UnitId == searchTruckModel.Truck_UnitId);

            gridInfo.recordsTotal = trucks.Select(a => a.Truck_Id).Count();

            trucks = trucks.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
                .Skip(gridInfo.start)
                .Take(gridInfo.length);

            return new TruckListResult
            {
                TruckList = trucks.ToList(),
                DatatableInfo = gridInfo
            };
        }

        public void UpdateTruck(TruckModel model)
        {
            var truckDet = BFPContext.tblTrucks.FirstOrDefault(a => a.Truck_Id == model.Truck_Id);
            if (truckDet == null) throw new Exception("Truck cannot be found!");

            model.Truck_DateCreated = truckDet.Truck_DateCreated;
            model.Truck_CreatedBy = truckDet.Truck_CreatedBy;
            model.Truck_ModifiedDate = DateTime.Now;

            Mapper.Map(model, truckDet);


            BFPContext.Entry(truckDet).State = EntityState.Modified;
            BFPContext.SaveChanges();
        }

        public bool DeleteByID(int truckId)
        {
            var truck = BFPContext.tblTrucks
                .FirstOrDefault(a => a.Truck_Id == truckId);

            if (truck != null)
            {
                BFPContext.tblTrucks.Remove(truck);
                BFPContext.SaveChanges();
            }

            return true;
        }

        public TruckModel UploadPicture(TruckModel model)
        {
            var municipalityFolder = $@"{HttpContext.Current.Request.PhysicalApplicationPath}Content\MISC\Images\{ model.Municipality_Name}"; 

            var newTruckFoler = $@"{municipalityFolder}\{model.Truck_UnitId}\Truck\";
            FileHelper.CheckDirectory(newTruckFoler);

            TruckModel truck = new TruckModel();
            if (model.Truck_Id > 0)
            {
                truck = GetTruckById(model.Truck_Id);
                //If Change Station, Move files
                if (truck != null && (model.Truck_UnitId != truck.Truck_UnitId))
                {
                    var oldTruckFoler = $@"{municipalityFolder}\{truck.Truck_UnitId}\Truck\";
                    FileHelper.CheckDirectory(oldTruckFoler);

                    FileHelper.MoveFile(truck.Truck_RightView, oldTruckFoler, newTruckFoler);
                    FileHelper.MoveFile(truck.Truck_LeftView, oldTruckFoler, newTruckFoler);
                    FileHelper.MoveFile(truck.Truck_FrontView, oldTruckFoler, newTruckFoler);
                    FileHelper.MoveFile(truck.Truck_RearView, oldTruckFoler, newTruckFoler);
                }
            }

            if (model.RightView?.ContentLength > 0)
                model.Truck_RightView = FileHelper.SaveFile(newTruckFoler, truck?.Truck_RightView, model.RightView);

            if (model.LeftView?.ContentLength > 0)
                model.Truck_LeftView = FileHelper.SaveFile(newTruckFoler, truck?.Truck_LeftView, model.LeftView);

            if (model.RearView?.ContentLength > 0)
                model.Truck_RearView = FileHelper.SaveFile(newTruckFoler, truck?.Truck_RearView, model.RearView);

            if (model.FrontView?.ContentLength > 0)
                model.Truck_FrontView = FileHelper.SaveFile(newTruckFoler, truck?.Truck_FrontView, model.FrontView);

            return model;
        }
        
        public List<TruckCountChartModel> GetTruckCountDetails(string sMunicipalityId = "")
        {
            var trucks = GetDashboardTruckList(sMunicipalityId);
            var notIncluded = new List<int> { (int)Truck_Status.BeyondEconomicRepair, (int)Truck_Status.ServiceableButForReplacement };
            var truckStatus = Enum.GetValues(typeof(Truck_Status)).Cast<Truck_Status>().Cast<int>().ToList();
            return truckStatus.Where(status => !notIncluded.Contains(status)).Select(status => new TruckCountChartModel
            {
                StatusId = status,
                StatusName = ((Truck_Status)status).ToDescription(),
                BFPOwnedCount = GetTruckCount(status, (int)Truck_Owner.BFP, trucks),
                LGUOwnedCount = GetTruckCount(status, (int)Truck_Owner.LGU, trucks)
            }).ToList();
        }

        public TruckCountSummaryModel GetTruckSummaryCount(string sMunicipalityId = "")
        {
            var details = GetTruckCountDetails(sMunicipalityId)
                            .Select(a => new { count = a.BFPOwnedCount + a.LGUOwnedCount, a.StatusId, a.BFPOwnedCount, a.LGUOwnedCount });

            var trucksCounts = new TruckCountSummaryModel
            {
                TotalTruck = details.Sum(a => a.count),
                TotalServiceable = details.Where(a => a.StatusId == (int)Truck_Status.ServiceAble).Sum(a => a.count),
                TotalUnserviceable = details.Where(a => a.StatusId == (int)Truck_Status.UnserviceAble).Sum(a => a.count),
                TotalUnderRepair = details.Where(a => a.StatusId == (int)Truck_Status.UnderRepair).Sum(a => a.count),
                TotalBFPOwned = details.Sum(a => a.BFPOwnedCount),
                TotalLGUOwned = details.Sum(a => a.LGUOwnedCount),
            };

            return trucksCounts;
        }

        private int GetTruckCount(int status, int owner, List<TruckModel> trucks)
        {
            var query = trucks.Where(a => a.Truck_Status == status && a.Truck_Owner == owner);

            return query.Count();
        }
        
        public List<TruckAgeCountChartModel> GetTruckAgeCountDetails(string sMunicipalityId = "")
        {
            var notIncluded = new List<int> { (int)Truck_Status.BeyondEconomicRepair, (int)Truck_Status.ServiceableButForReplacement };
            var trucks = GetDashboardTruckList(sMunicipalityId).Where(a => !notIncluded.Contains(a.Truck_Status ?? 0)).ToList();
            var truckAge = Enum.GetValues(typeof(Truck_Age)).Cast<Truck_Age>().Cast<int>().ToList();
            return truckAge.Select(age => new TruckAgeCountChartModel
            {
                AgeId = age,
                Age = ((Truck_Age)age).ToDescription(),
                BFPOwnedCount = GetTruckAgeCount(((Truck_Age)age).ToDescription(), (int)Truck_Owner.BFP, trucks),
                LGUOwnedCount = GetTruckAgeCount(((Truck_Age)age).ToDescription(), (int)Truck_Owner.LGU, trucks)
            }).ToList();
        }

        private int GetTruckAgeCount(string age, int owner, List<TruckModel> trucks)
        {
            int fromAge = 0;
            int toAge = 0;

            if (age.Contains("Above"))
            {
                fromAge = 51;
            }
            else if (!string.IsNullOrWhiteSpace(age))
            {
                var range = age.Split('-');
                fromAge = Convert.ToInt32(range[0]);
                if(range.Count() > 1 && fromAge != 50)
                    toAge = Convert.ToInt32(range[1]);
            }
            
            var query = trucks.Where(a => a.Truck_Owner == owner);
            if (fromAge == 25)
                query = query.Where(a => a.Truck_AcquisitionAge == fromAge);
            else if (fromAge == 51)
                query = query.Where(a => a.Truck_AcquisitionAge >= fromAge);
            else
                query = query.Where(a => a.Truck_AcquisitionAge >= fromAge && a.Truck_AcquisitionAge <= toAge);

            return query.Count();
        }

        private List<TruckModel> GetDashboardTruckList(string sMunicipalityId = "")
        {
            var notIncluded = new List<int> { (int)Truck_Status.BeyondEconomicRepair, (int)Truck_Status.ServiceableButForReplacement };

            var trucks = GetFilteredFireTrucks(sMunicipalityId);

            return trucks.Join(BFPContext.tblUnits,
                                        truck => truck.Truck_UnitId, unit => unit.Unit_Id,
                                        (truck, unit) => new TruckModel
                                        {
                                            Truck_Id = truck.Truck_Id,
                                            Municipality_Reg_Id = unit.tblCityMunicipality.tblProvinces.Region_Id,
                                            Municipality_Province_Id = unit.tblCityMunicipality.Municipality_Province_Id,
                                            Municipality_Id = unit.Unit_Municipality_Id,
                                            Truck_UnitId = unit.Unit_Id,
                                            Truck_Status = truck.Truck_Status,
                                            Truck_Owner = truck.Truck_Owner,
                                            Truck_AcquisitionDate = truck.Truck_AcquisitionDate
                                        })
                               .Where(a => !notIncluded.Contains(a.Truck_Status ?? 0))
                               .ToList();
        }

        public List<TruckAgeCountChartModel> GetTruckAgeGroupCount(string sMunicipalityId)
        {
            var count = GetTruckAgeCountDetails(sMunicipalityId);
            
            var ZeroToTwentyFour = new List<int> { 1, 2, 3, 4, 5 };
            var TwentyFiveAbove = new List<int> { 6, 7, 8, 9, 10, 11, 12 };

            var list = new List<TruckAgeCountChartModel>();
            for (int x = 1; x <= 2; x++)
            {
                var filterByAge = x == 1 ? ZeroToTwentyFour : TwentyFiveAbove;
                var model = new TruckAgeCountChartModel
                {
                    BFPOwnedCount = count.Where(a => filterByAge.Contains(a.AgeId)).Sum(a => a.BFPOwnedCount),
                    LGUOwnedCount = count.Where(a => filterByAge.Contains(a.AgeId)).Sum(a => a.LGUOwnedCount)
                };
                model.SubTotal = model.BFPOwnedCount + model.LGUOwnedCount;
                list.Add(model);
            };

            var total = new TruckAgeCountChartModel
            {
                BFPOwnedCount = list.Sum(a => a.BFPOwnedCount),
                LGUOwnedCount = list.Sum(a => a.LGUOwnedCount)
            };
            total.SubTotal = total.BFPOwnedCount + total.LGUOwnedCount;
            list.Add(total);

            foreach(var item in list)
            {
                item.Share = total.SubTotal > 0 ? (int)Math.Round(((item.SubTotal * 100) / (double)total.SubTotal)) : 0;
            }

            return list;
        }

        public TruckListResult GetTruckStationListResult(GridInfo gridInfo)
        {
            var trucks = BFPContext.tblTrucks.AsQueryable();
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
            {
                trucks = trucks.Where(
                    a => a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID);
            }
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
            {
                trucks =
                    trucks.Where(
                        a => a.tblUnits.tblCityMunicipality.Municipality_Province_Id == CurrentUser.ProvinceID);
            }
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
            {
                trucks = trucks.Where(a => a.Truck_UnitId == CurrentUser.EmployeeUnitId);
            }

            var searchTerms = gridInfo.searchStationDetailsModel;

            if(searchTerms != null)
            {
                if (searchTerms.SubStation_Id > 0)
                    trucks = trucks.Where(a => a.Truck_SubStationId == searchTerms.SubStation_Id);
                else if (searchTerms.Station_Id > 0)
                    trucks = trucks.Where(a => a.Truck_UnitId == searchTerms.Station_Id && a.Truck_SubStationId == null);               
            }

            var truckRes = trucks             
                .Select(a => new TruckModel
                {
                    Truck_UnitId = a.Truck_UnitId,
                    Truck_Id = a.Truck_Id,
                    Truck_Id_Code = a.Truck_Id_Code,
                    Truck_Type = a.Truck_Type,
                    Truck_Status = a.Truck_Status,
                    Truck_Owner = a.Truck_Owner,
                    Truck_PlateNumber = a.Truck_PlateNumber
                });
            

            gridInfo.recordsTotal = trucks.Select(a => a.Truck_Id).Count();
            var res = truckRes.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
                .Skip(gridInfo.start)
                .Take(gridInfo.length);

            return new TruckListResult
            {
                TruckList = res.ToList(),
                DatatableInfo = gridInfo
            };
        }
    }
}