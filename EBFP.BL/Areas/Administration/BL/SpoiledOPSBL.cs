using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using EBFP.DataAccess;
using Queries.Core.Repositories;
using EBFP.BL.Helper;

namespace EBFP.BL.Administration
{
    public class SpoiledOPSBL : Repository<tblSpoiledOPS, SpoiledOPSModel>, ISpoiledOPS
    {
        public SpoiledOPSBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }
        public void CreateMapping()
        {
            Mapper.CreateMap<tblSpoiledOPS, SpoiledOPSModel>().ReverseMap();
            Mapper.CreateMap<List<tblSpoiledOPS>, List<SpoiledOPSModel>>().ReverseMap();
            Mapper.CreateMap<List<tblSpoiledOPS>, List<SpoiledOPSModel>>();
        }

        public SpoiledOPSListResult GetSpoiledOPSList(GridInfo gridInfo, int unitId)
        {

            var retDeposits = new List<SpoiledOPSModel>();

            var SearchTerms = gridInfo.searchSpoiledOPS;      
            var spoiledOPS = BFPContext.tblSpoiledOPS.Where(a=> a.SOPS_Unit_Id == unitId).AsQueryable();

            if (!string.IsNullOrEmpty(SearchTerms.SpoiledOPSNumber))
                spoiledOPS = spoiledOPS.Where(a => a.SOPS_Number.Contains(SearchTerms.SpoiledOPSNumber));


            gridInfo.recordsTotal = spoiledOPS.Select(a => a.SOPS_Id).Count();
            var spoiledOPSListResult = spoiledOPS.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
             .Skip(gridInfo.start)
             .Take(gridInfo.length)
             .ToList();

            foreach (var spoiled in spoiledOPSListResult)
            {
                retDeposits.Add(new SpoiledOPSModel
                {
                    SOPS_Number = spoiled.SOPS_Number,
                    SOPS_CreatedDate = spoiled.SOPS_CreatedDate,
                    SOPS_Id = spoiled.SOPS_Id,
                    Ref_SOPS_Id = spoiled.Ref_SOPS_Id
                });
            }

            return new SpoiledOPSListResult
            {
                SpoiledOPSList = retDeposits.OrderBy(a => a.SOPS_CreatedDate).ToList(),
                DatatableInfo = gridInfo
            };
        }
    }
}