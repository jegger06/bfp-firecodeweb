using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.Helper;

namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;

    public class RankBL : Repository<tblRanks, RankModel>, IRank
    {
        public RankBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblRanks, RankModel>().ReverseMap();
            Mapper.CreateMap<List<tblRanks>, List<RankModel>>().ReverseMap();
            Mapper.CreateMap<List<tblRanks>, List<RankModel>>();
        }

        public RankListResult GetListResult(GridInfo gridInfo)
        {
            var ret = new List<RankModel>();

            //if (CurrentUser.Role == (int) AccessRole.PortalAdmin)
            //{

                var rank = BFPContext.tblRanks.AsQueryable();

                var searchRankModel = gridInfo.searchRankModel;
                if (!string.IsNullOrEmpty(searchRankModel.Rank_Name))
                    rank = rank.Where(a => a.Rank_Name.Contains(searchRankModel.Rank_Name));

                gridInfo.recordsTotal = rank.Select(a => a.Rank_Id).Count();

                rank = rank.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
                    .Skip(gridInfo.start)
                    .Take(gridInfo.length);


                foreach (var item in rank)
                {
                    ret.Add(new RankModel()
                    {
                        Rank_Id = item.Rank_Id,
                        Rank_Name = item.Rank_Name,
                        Rank_DBM_Authorized = item.Rank_DBM_Authorized,
                    });
                }
            //}
            return new RankListResult
            {
                RankList = ret,
                DatatableInfo = gridInfo
            };
        }


        public bool DeleteByID(int rankId)
        {
            var rank = BFPContext.tblRanks
                .FirstOrDefault(a => a.Rank_Id == rankId);

            if (rank != null)
            {
                BFPContext.tblRanks.Remove(rank);
                BFPContext.SaveChanges();
            }

            return true;
        }

        public RankModel GetRankById(int rankId)
        {
            var ret = new RankModel();

            var res = BFPContext.tblRanks.FirstOrDefault(a => a.Rank_Id == rankId);

            if (res != null)
            {
                Mapper.Map(res, ret);

            }
            return ret;
        }

        public void UpdateRank(RankModel model)
        {
            IHRISUnitOfWork unitOfWork = new HRISUnitOfWork(BFPContext);
            var rankDet = BFPContext.tblRanks.FirstOrDefault(a => a.Rank_Id == model.Rank_Id);
            if (rankDet == null) throw new Exception("Rank cannot be found!");
            var oldRank = rankDet.Rank_Name;
            model.Rank_CreatedBy = rankDet.Rank_CreatedBy;
            model.Rank_CreatedDate = rankDet.Rank_CreatedDate;
            Mapper.Map(model, rankDet);

            BFPContext.Entry(rankDet).State = EntityState.Modified;
            BFPContext.SaveChanges();

            if (oldRank.Trim() != model.Rank_Name.Trim())
            {
                var logsModel = new LogsModel
                {
                    Log_Emp_Id = CurrentUser.EmployeeId,
                    Log_Remarks = "Rank - From " + oldRank + " to " + model.Rank_Name
                };
                unitOfWork.Logs.InsertLogs(logsModel);
            }
           
        }

        public int GetRankDBMById(int rankId)
        {
            var dbmAuthorized = BFPContext.tblRanks.FirstOrDefault(a => a.Rank_Id == rankId);

            if (dbmAuthorized != null)
                return dbmAuthorized.Rank_DBM_Authorized;
            return 0;
        }
        public List<ActualVsAuthorizedModel> GetRanklList()
        {
            using (var context = new EBFPEntities())
            {
                var ranks = (from rank in context.tblRanks
                             from employee in context.tblEmployees.Where(x => rank.Rank_Id == x.Emp_Curr_Rank && x.Emp_Curr_Unit != null && x.Emp_DutyStatus == (int)DutyStatuses.Active)
                                 .DefaultIfEmpty()
                             select new ActualVsAuthorizedModel
                             {
                                 RankId = rank.Rank_Id,
                                 Rank = rank.Rank_Name,
                                 DBMAuthorized = rank.Rank_DBM_Authorized,
                                 EmpId = employee != null ? employee.Emp_Id : 0,
                                 Emp_Curr_Unit = employee != null ? employee.Emp_Curr_Unit : 0,
                                 RegionId = employee != null ? employee.tblUnits.tblCityMunicipality.tblProvinces.Region_Id : 0,
                                 ProvinceId = employee != null ? employee.tblUnits.tblCityMunicipality.tblProvinces.Province_Id : 0,
                             });

                if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
                {
                    ranks = ranks.Where(a => a.RegionId == CurrentUser.RegionID);
                }
                else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
                {
                    ranks = ranks.Where(a => a.ProvinceId == CurrentUser.ProvinceID);
                }
                else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                {
                    ranks = ranks.Where(a => a.Emp_Curr_Unit == CurrentUser.EmployeeUnitId);
                }

                var fileteredRanks = ranks.ToList();

                var filtered = fileteredRanks.GroupBy(l => l.Rank)
                 .Select(cl => new ActualVsAuthorizedModel
                 {
                     RankId = cl.First().RankId,
                     Rank = cl.First().Rank,
                     DBMAuthorized = cl.First().DBMAuthorized,
                     ActualStrength = cl.Count(a => a.EmpId != 0),
                     Variance = cl.First().DBMAuthorized - cl.Count(a => a.EmpId != 0) < 0 ? 0 : cl.First().DBMAuthorized - cl.Count(a => a.EmpId != 0)
                 }).OrderByDescending(a => a.RankId).ToList();

                return filtered;
            }

        }
    }
}
