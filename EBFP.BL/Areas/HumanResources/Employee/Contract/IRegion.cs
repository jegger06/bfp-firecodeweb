using EBFP.BL.Helper;
using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.HumanResources
{
    public interface IRegion : IRepository<tblRegions, RegionModel>
    {
        RegionListResult GetListResult(GridInfo gridInfo);
        RegionModel GetRegionById(int regionId);
        void UpdateRegion(RegionModel model);
    }
}