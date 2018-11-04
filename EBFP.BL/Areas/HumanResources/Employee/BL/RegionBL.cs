namespace EBFP.BL.HumanResources
{
    using AutoMapper;
    using EBFP.DataAccess;
    using Helper;
    using Queries.Core.Repositories;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data.Entity;
    using System.Linq.Dynamic;
    using System;

    public class RegionBL : Repository<tblRegions, RegionModel>, IRegion
    {
        public RegionBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblRegions, RegionModel>().ReverseMap();
            Mapper.CreateMap<List<tblRegions>, List<RegionModel>>().ReverseMap();
            Mapper.CreateMap<List<tblRegions>, List<RegionModel>>();
        }

        public RegionListResult GetListResult(GridInfo gridInfo)
        {
            var ret = new List<RegionModel>();

            var region = BFPContext.tblRegions.AsQueryable();

            gridInfo.recordsTotal = region.Select(a => a.Reg_Id).Count();

            region = region.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
                .Skip(gridInfo.start)
                .Take(gridInfo.length);


            foreach (var item in region)
            {
                ret.Add(new RegionModel()
                {
                    Reg_Id = item.Reg_Id,
                    Reg_Title = item.Reg_Title
                });
            }

            return new RegionListResult
            {
                RegionList = ret,
                DatatableInfo = gridInfo
            };
        }

        public RegionModel GetRegionById(int regionId)
        {
            var ret = new RegionModel();

            var res = BFPContext.tblRegions.FirstOrDefault(a => a.Reg_Id == regionId);

            if (res != null)
            {
                Mapper.Map(res, ret);

            }
            return ret;
        }

        public void UpdateRegion(RegionModel model)
        {
            var regionDet = BFPContext.tblRegions.FirstOrDefault(a => a.Reg_Id == model.Reg_Id);
            if (regionDet == null) throw new Exception("Unit cannot be found!");

            if (model.Reg_Logo != null)
                regionDet.Reg_Logo = model.Reg_Logo;

            BFPContext.Entry(regionDet).State = EntityState.Modified;
            BFPContext.SaveChanges();
        }
    } 
}
