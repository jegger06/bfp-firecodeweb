using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using EBFP.DataAccess;
using Queries.Core.Repositories;
using EBFP.BL.Helper;
using System;

namespace EBFP.BL.Administration
{
    public class SpoiledORBL : Repository<tblSpoiledOR, SpoiledORModel>, ISpoiledOR
    {
        public SpoiledORBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }
        public void CreateMapping()
        {
            Mapper.CreateMap<tblSpoiledOR, SpoiledORModel>().ReverseMap();
            Mapper.CreateMap<List<tblSpoiledOR>, List<SpoiledORModel>>().ReverseMap();
            Mapper.CreateMap<List<tblSpoiledOR>, List<SpoiledORModel>>();
        }

        public SpoiledORListResult GetSpoiledORList(GridInfo gridInfo, int unitId)
        {

            var retDeposits = new List<SpoiledORModel>();

            var SearchTerms = gridInfo.searchSpoiledOR;
            var spoiledOR = BFPContext.tblSpoiledOR.Where(a => a.SOR_Unit_Id == unitId).AsQueryable();

            if (!string.IsNullOrEmpty(SearchTerms.SpoiledORNumber))
            {
                var sorNumber = Convert.ToInt64(SearchTerms.SpoiledORNumber);
                spoiledOR = spoiledOR.Where(a => a.SOR_Number == sorNumber);
            }
             

            gridInfo.recordsTotal = spoiledOR.Select(a => a.SOR_Id).Count();
            var spoiledORListResult = spoiledOR.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
             .Skip(gridInfo.start)
             .Take(gridInfo.length)
             .ToList();

            foreach (var spoiled in spoiledORListResult)
            {
                retDeposits.Add(new SpoiledORModel
                {
                    SOR_Number = spoiled.SOR_Number,
                    SOR_CreatedDate = spoiled.SOR_CreatedDate,
                    SOR_Id = spoiled.SOR_Id,
                    Ref_SOR_Id = spoiled.Ref_SOR_Id
                });
            }

            return new SpoiledORListResult
            {
                SpoiledORList = retDeposits.OrderBy(a => a.SOR_CreatedDate).ToList(),
                DatatableInfo = gridInfo
            };
        }
    }
}