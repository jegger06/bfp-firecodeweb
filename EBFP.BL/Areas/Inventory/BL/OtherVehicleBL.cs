using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public class OtherVehicleBL : Repository<tblOtherVehicles, OtherVehicleModel>, IOtherVehicle
    {
        public OtherVehicleBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public OtherVehicleModel GetOtherVehicleById(int vehicleId)
        {
            var model = new OtherVehicleModel();

            var entity = BFPContext.tblOtherVehicles.
                FirstOrDefault(a => a.Vehicle_Id == vehicleId);

            if (entity != null)
            {
                Mapper.Map(entity, model);
                model.Municipality_Name = entity.tblUnits.tblCityMunicipality.Municipality_Name;
                model.Municipality_Reg_Id = entity.tblUnits.tblCityMunicipality.tblProvinces.Region_Id;
                model.Municipality_Province_Id = entity.tblUnits.tblCityMunicipality.Municipality_Province_Id;
                model.Unit_StationName = entity.tblUnits.Unit_StationName;
                model.Municipality_Id = entity.tblUnits.Unit_Municipality_Id;
            }

            return model;
        }

        public OtherVehicleModel GetOtherVehicleByUnitId(int unitId)
        {
            var details = new OtherVehicleModel();
            var ret = BFPContext.tblUnits.FirstOrDefault(a => a.Unit_Id == unitId);

            if (ret != null)
            {
                details.Municipality_Reg_Id = ret.tblCityMunicipality?.tblProvinces?.Region_Id ?? 0;
                details.Municipality_Province_Id = ret.tblCityMunicipality?.Municipality_Province_Id ?? 0;
                details.Municipality_Name = ret.tblCityMunicipality?.Municipality_Name;
                details.Municipality_Id = ret.tblCityMunicipality?.Municipality_Id ?? 0;
                details.Vehicle_UnitId = ret.Unit_Id;
            }

            return details;
        }

        public OtherVehicleListResult GetListResult(GridInfo gridInfo)
        {
            var vehicles = BFPContext.tblOtherVehicles.AsQueryable();
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
            {
                vehicles = vehicles.Where(
                    a => a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID);
            }
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
            {
                vehicles =
                    vehicles.Where(
                        a => a.tblUnits.tblCityMunicipality.Municipality_Province_Id == CurrentUser.ProvinceID);
            }
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
            {
                vehicles = vehicles.Where(a => a.Vehicle_UnitId == CurrentUser.EmployeeUnitId);
            }


            var otherVehicles = vehicles
                .Where(a => a.tblUnits.Unit_Municipality_Id == gridInfo.searchOtherVehicleModel.Municipality_Id)
                .Select(a => new OtherVehicleModel
                {
                    Vehicle_UnitId = a.Vehicle_UnitId,
                    Vehicle_Id = a.Vehicle_Id,
                    Vehicle_Id_Code = a.Vehicle_Id_Code,
                    Unit_StationName = a.tblUnits.Unit_StationName,
                    Vehicle_Type = a.Vehicle_Type,
                    Vehicle_Status = a.Vehicle_Status,
                    Vehicle_Owner = a.Vehicle_Owner,
                    Vehicle_AcquisitionDate = a.Vehicle_AcquisitionDate,
                    Vehicle_PlateNumber = a.Vehicle_PlateNumber
                });

            var searchOtherVehicleModel = gridInfo.searchOtherVehicleModel;
            if (!string.IsNullOrEmpty(searchOtherVehicleModel.Vehicle_Id_Code))
                otherVehicles =
                    otherVehicles.Where(a => a.Vehicle_Id_Code.Contains(searchOtherVehicleModel.Vehicle_Id_Code));
            if (!string.IsNullOrEmpty(searchOtherVehicleModel.Vehicle_PlateNumber))
                otherVehicles =
                    otherVehicles.Where(
                        a => a.Vehicle_PlateNumber.Contains(searchOtherVehicleModel.Vehicle_PlateNumber));
            if (searchOtherVehicleModel.Vehicle_Type > 0)
                otherVehicles = otherVehicles.Where(a => a.Vehicle_Type == searchOtherVehicleModel.Vehicle_Type);
            if (searchOtherVehicleModel.Vehicle_Status > 0)
                otherVehicles = otherVehicles.Where(a => a.Vehicle_Status == searchOtherVehicleModel.Vehicle_Status);
            if (searchOtherVehicleModel.Vehicle_Owner > 0)
                otherVehicles = otherVehicles.Where(a => a.Vehicle_Owner == searchOtherVehicleModel.Vehicle_Owner);
            if (searchOtherVehicleModel.Vehicle_UnitId > 0)
                otherVehicles = otherVehicles.Where(a => a.Vehicle_UnitId == searchOtherVehicleModel.Vehicle_UnitId);

            gridInfo.recordsTotal = otherVehicles.Select(a => a.Vehicle_Id).Count();

            otherVehicles = otherVehicles.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
                .Skip(gridInfo.start)
                .Take(gridInfo.length);
            return new OtherVehicleListResult
            {
                OtherVehicleList = otherVehicles.ToList(),
                DatatableInfo = gridInfo
            };
        }

        public void UpdateOtherVehicle(OtherVehicleModel model)
        {
            var vehicleDet = BFPContext.tblOtherVehicles.FirstOrDefault(a => a.Vehicle_Id == model.Vehicle_Id);
            if (vehicleDet == null) throw new Exception("Other vehicle cannot be found!");

            model.Vehicle_CreatedDate = vehicleDet.Vehicle_CreatedDate;
            model.Vehicle_CreatedBy = vehicleDet.Vehicle_CreatedBy;
            model.Vehicle_ModifiedDate = DateTime.Now;
            Mapper.Map(model, vehicleDet);

            BFPContext.Entry(vehicleDet).State = EntityState.Modified;
            BFPContext.SaveChanges();
        }

        public bool DeleteByID(int vehicle_Id)
        {
            var vehicle = BFPContext.tblOtherVehicles
                .FirstOrDefault(a => a.Vehicle_Id == vehicle_Id);

            if (vehicle != null)
            {
                BFPContext.tblOtherVehicles.Remove(vehicle);
                BFPContext.SaveChanges();
            }

            return true;
        }

        public OtherVehicleModel UploadPicture(OtherVehicleModel model)
        {
            var municipalityFolder = $@"{HttpContext.Current.Request.PhysicalApplicationPath}Content\MISC\Images\{ model.Municipality_Name}";

            var newVehicleFoler = $@"{municipalityFolder}\{model.Vehicle_UnitId}\Other Vehicle\";
            FileHelper.CheckDirectory(newVehicleFoler);

            OtherVehicleModel vehicle = new OtherVehicleModel();
            if (model.Vehicle_Id > 0)
            {
                vehicle = GetOtherVehicleById(model.Vehicle_Id);
                //If Change Station, Move files
                if (vehicle != null && (model.Vehicle_UnitId != vehicle.Vehicle_UnitId))
                {
                    var oldVehicleFolder = $@"{municipalityFolder}\{vehicle.Vehicle_UnitId}\Other Vehicle\";
                    FileHelper.CheckDirectory(oldVehicleFolder);

                    FileHelper.MoveFile(vehicle.Vehicle_RightView, oldVehicleFolder, newVehicleFoler);
                    FileHelper.MoveFile(vehicle.Vehicle_LeftView, oldVehicleFolder, newVehicleFoler);
                    FileHelper.MoveFile(vehicle.Vehicle_FrontView, oldVehicleFolder, newVehicleFoler);
                    FileHelper.MoveFile(vehicle.Vehicle_RearView, oldVehicleFolder, newVehicleFoler);
                }
            }

            if (model.RightView?.ContentLength > 0)
                model.Vehicle_RightView = FileHelper.SaveFile(newVehicleFoler, vehicle?.Vehicle_RightView, model.RightView);

            if (model.LeftView?.ContentLength > 0)
                model.Vehicle_LeftView = FileHelper.SaveFile(newVehicleFoler, vehicle?.Vehicle_LeftView, model.LeftView);

            if (model.RearView?.ContentLength > 0)
                model.Vehicle_RearView = FileHelper.SaveFile(newVehicleFoler, vehicle?.Vehicle_RearView, model.RearView);

            if (model.FrontView?.ContentLength > 0)
                model.Vehicle_FrontView = FileHelper.SaveFile(newVehicleFoler, vehicle?.Vehicle_FrontView, model.FrontView);

            return model;
        }
        
        public List<VehicleCountChartModel> GetVehicleCountDetails(string sMunicipalityId = "")
        {
            var listVehicles = new List<VehicleCountChartModel>();
            var unitOfWork = new InventoryUnitOfWork();
            var truckCount = unitOfWork.Truck.GetTruckSummaryCount(sMunicipalityId);

            listVehicles.Add(new VehicleCountChartModel
            {
                Type = (int)VehicleDashboardType.FIRE_TRUCKS,
                TypeName = VehicleDashboardType.FIRE_TRUCKS.ToDescription(),
                BFPOwnedCount = truckCount.TotalBFPOwned,
                LGUOwnedCount = truckCount.TotalLGUOwned
            });

            var notIncluded = new List<int> { (int)Truck_Status.BeyondEconomicRepair, (int)Truck_Status.ServiceableButForReplacement };
            var vehicles = GetDashboardVehicleList(sMunicipalityId).Where(a => !notIncluded.Contains(a.Vehicle_Status ?? 0)).ToList();
            var vehicleTypes = Enum.GetValues(typeof(VehicleDashboardType)).Cast<VehicleDashboardType>().Cast<int>().ToList();
            var res = vehicleTypes.Where(type => type != (int)VehicleDashboardType.FIRE_TRUCKS).Select(type => new VehicleCountChartModel
            {
                Type = type,
                TypeName = ((VehicleDashboardType)type).ToDescription(),
                BFPOwnedCount = GetVehicleCount(type, (int)VehicleOwner.BFP, vehicles),
                LGUOwnedCount = GetVehicleCount(type, (int)VehicleOwner.LGU, vehicles),
            }).ToList();

            listVehicles.AddRange(res);
            return listVehicles;
        }
        
        private int GetVehicleCount(int types, int owner, List<OtherVehicleModel> vehicles)
        {
            var query = vehicles.Where(a => a.Vehicle_Type == types && a.Vehicle_Owner == owner);
            return query.Count();
        }
                
        public VehicleCountSummaryModel GetVehicleSummaryCount()
        {
            var unitOfWork = new InventoryUnitOfWork();
            var truckCount = unitOfWork.Truck.GetTruckSummaryCount();

            var details = GetVehicleCountDetails()
                            .Select(a => new { count = a.BFPOwnedCount + a.LGUOwnedCount, a.Type });

            var vehicleCount = new VehicleCountSummaryModel
            {
                TotalVehicle = details.Sum(a => a.count),
                TotalFireTrucks = truckCount.TotalTruck,
                TotalAmbulances = details.Where(a => a.Type == (int)VehicleDashboardType.AMBULANCE).Sum(a => a.count),
                TotalRescueVehicle = details.Where(a => a.Type == (int)VehicleDashboardType.RESCUE_VEHICLE).Sum(a => a.count),
                TotalFireBoats = details.Where(a => a.Type == (int)VehicleDashboardType.RUBBER_BOAT).Sum(a => a.count)
            };

            return vehicleCount;
        }

        private List<OtherVehicleModel> GetDashboardVehicleList(string sMunicipalityId = "")
        {
            var notIncluded = new List<int> { (int)Truck_Status.BeyondEconomicRepair, (int)Truck_Status.ServiceableButForReplacement };
            var vehicles = GetFilteredVehicles(sMunicipalityId);
            return vehicles.Join(BFPContext.tblUnits,
                                        vehicle => vehicle.Vehicle_UnitId, unit => unit.Unit_Id,
                                        (vehicle, unit) => new OtherVehicleModel
                                        {
                                            Vehicle_Id = vehicle.Vehicle_UnitId,
                                            Vehicle_Type = vehicle.Vehicle_Type,
                                            Municipality_Reg_Id = unit.tblCityMunicipality.tblProvinces.Region_Id,
                                            Municipality_Province_Id = unit.tblCityMunicipality.Municipality_Province_Id,
                                            Municipality_Id = unit.Unit_Municipality_Id,
                                            Vehicle_UnitId = unit.Unit_Id,
                                            Vehicle_Status = vehicle.Vehicle_Status,
                                            Vehicle_Owner = vehicle.Vehicle_Owner,
                                            Vehicle_AcquisitionDate = vehicle.Vehicle_AcquisitionDate
                                        })
                               .Where(a => !notIncluded.Contains(a.Vehicle_Status ?? 0))
                               .ToList();
        }

        private IQueryable<tblOtherVehicles> GetFilteredVehicles(string sMunicipalityId = "")
        {
            var vehicle = BFPContext.tblOtherVehicles.AsQueryable();

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
                vehicle = vehicle.Where(a => a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID);
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
                vehicle = vehicle.Where(a => a.tblUnits.tblCityMunicipality.Municipality_Province_Id == CurrentUser.ProvinceID);
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                vehicle = vehicle.Where(a => a.Vehicle_UnitId == CurrentUser.EmployeeUnitId);

            if (!string.IsNullOrWhiteSpace(sMunicipalityId))
            {
                var municipalityId = Convert.ToInt32(sMunicipalityId.Decrypt());
                vehicle = vehicle.Where(a => a.tblUnits.Unit_Municipality_Id == municipalityId);
            }

            return vehicle;
        }

        public OtherVehicleListResult GetVehicleStationListResult(GridInfo gridInfo)
        {
            var vehicles = BFPContext.tblOtherVehicles.AsQueryable();
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
            {
                vehicles = vehicles.Where(
                    a => a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID);
            }
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
            {
                vehicles =
                    vehicles.Where(
                        a => a.tblUnits.tblCityMunicipality.Municipality_Province_Id == CurrentUser.ProvinceID);
            }
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
            {
                vehicles = vehicles.Where(a => a.Vehicle_UnitId == CurrentUser.EmployeeUnitId);
            }

            var searchTerms = gridInfo.searchStationDetailsModel;

            if (searchTerms != null)
            {
                if (searchTerms.SubStation_Id > 0)
                    vehicles = vehicles.Where(a => a.Vehicle_SubStationId == searchTerms.SubStation_Id);
                else if (searchTerms.Station_Id > 0)
                    vehicles = vehicles.Where(a => a.Vehicle_UnitId == searchTerms.Station_Id && a.Vehicle_SubStationId == null);

            }

            var vehicleRes = vehicles
                .Select(a => new OtherVehicleModel
                {
                    Vehicle_UnitId = a.Vehicle_UnitId,
                    Vehicle_Id = a.Vehicle_Id,
                    Vehicle_Id_Code = a.Vehicle_Id_Code,
                    Vehicle_Type = a.Vehicle_Type,
                    Vehicle_Status = a.Vehicle_Status,
                    Vehicle_Owner = a.Vehicle_Owner,
                    Vehicle_PlateNumber = a.Vehicle_PlateNumber
                });


            gridInfo.recordsTotal = vehicles.Select(a => a.Vehicle_Id).Count();
            var res = vehicleRes.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
                .Skip(gridInfo.start)
                .Take(gridInfo.length);

            return new OtherVehicleListResult
            {
                OtherVehicleList = res.ToList(),
                DatatableInfo = gridInfo
            };
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblOtherVehicles, OtherVehicleModel>().ReverseMap();
            Mapper.CreateMap<List<tblOtherVehicles>, List<OtherVehicleModel>>().ReverseMap();
            Mapper.CreateMap<List<tblOtherVehicles>, List<OtherVehicleModel>>();
        }
    }
}