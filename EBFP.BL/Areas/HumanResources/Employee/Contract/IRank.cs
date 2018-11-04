using System.Collections.Generic;
using EBFP.BL.Helper;
using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.HumanResources
{
    public interface IRank : IRepository<tblRanks, RankModel>
    {
        RankListResult GetListResult(GridInfo gridInfo);
        bool DeleteByID(int rankId);
        RankModel GetRankById(int rankId);
        void UpdateRank(RankModel model);
        int GetRankDBMById(int rankId);
        List<ActualVsAuthorizedModel> GetRanklList();
    }
}