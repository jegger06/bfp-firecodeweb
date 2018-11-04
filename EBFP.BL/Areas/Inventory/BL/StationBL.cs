using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.Inventory
{
    public class StationBL : Repository<tblUnits, StationModel>, IStation
    {
        public StationBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }
        public void CreateMapping()
        {
            Mapper.CreateMap<tblUnits, StationModel>();
            Mapper.CreateMap<tblUnits, StationModel>().ReverseMap();
        }

        public StationListResult GetListResult(GridInfo gridInfo)
        {
            var station = BFPContext.tblUnits
                .Where(a => a.Unit_Municipality_Id == gridInfo.searchStationModel.Municipality_Id)
                .Select(a => new StationModel
                {
                    Unit_Id = a.Unit_Id,
                    Unit_Code = a.Unit_Code,
                    Unit_BuildingStatus = a.Unit_BuildingStatus,
                    Unit_BuildingOwner = a.Unit_BuildingOwner,
                    Unit_LotOwner = a.Unit_LotOwner,
                    Unit_LotStatus = a.Unit_LotStatus,
                    Unit_StationName = a.Unit_StationName,
                    Unit_Category = a.Unit_Category,
                    Unit_Municipality_Id = a.Unit_Municipality_Id
                });

            var search = gridInfo.searchStationModel ?? new StationSearchModel();
            if (!string.IsNullOrEmpty(search.Unit_Code))
                station = station.Where(a => a.Unit_Code.Contains(search.Unit_Code));
            if (!string.IsNullOrEmpty(search.Unit_StationName))
                station = station.Where(a => a.Unit_StationName.Contains(search.Unit_StationName));
            if (search.Unit_Id > 0)
                station = station.Where(a => a.Unit_Id == search.Unit_Id);
            if (search.Unit_BuildingStatus > 0)
                station = station.Where(a => a.Unit_BuildingStatus == search.Unit_BuildingStatus);
            if (search.Unit_BuildingOwner > 0)
                station = station.Where(a => a.Unit_BuildingOwner == search.Unit_BuildingOwner);
            if (search.Unit_LotStatus > 0)
                station = station.Where(a => a.Unit_LotStatus == search.Unit_LotStatus);
            if (search.Unit_LotOwner > 0)
                station = station.Where(a => a.Unit_LotOwner == search.Unit_LotOwner);
            if (search.Unit_Category > 0)
                station = station.Where(a => a.Unit_Category == search.Unit_Category);

            //total
            gridInfo.recordsTotal = station.Select(a => a.Unit_Id).Count();

            station = station.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
                .Skip(gridInfo.start)
                .Take(gridInfo.length);

            return new StationListResult
            {
                StationList = station.ToList(),
                DatatableInfo = gridInfo
            };
        }

        public StationModel GetStationById(int stationId)
        {
            var stationDetails = new StationModel();
            var ret = BFPContext.tblUnits.FirstOrDefault(a => a.Unit_Id == stationId);

            if(ret != null)
            {
                Mapper.Map(ret, stationDetails);
                stationDetails.RegionId = ret.tblCityMunicipality?.tblProvinces?.Region_Id ?? 0;
                stationDetails.ProvinceId = ret.tblCityMunicipality?.Municipality_Province_Id ?? 0;
                stationDetails.Unit_Municipality_Id = ret.Unit_Municipality_Id;
                stationDetails.MunicipalityName = ret.tblCityMunicipality?.Municipality_Name;
                stationDetails.NSCB = ret.tblCityMunicipality?.Municipality_NSCB;
            }

            return stationDetails;
        }
        

        public StationModel SaveStationDetails(StationModel model)
        {
            try
            {
                var station = new tblUnits();
                if (model.Unit_Id > 0)
                {
                    station = BFPContext.tblUnits
                        .FirstOrDefault(a => a.Unit_Id == model.Unit_Id);

                    Mapper.Map(model, station);
                    BFPContext.SaveChanges();
                }

                return model;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
       
        }

    }
}