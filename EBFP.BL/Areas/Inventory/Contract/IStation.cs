
namespace EBFP.BL.Inventory
{
    using EBFP.DataAccess;
    using Helper;
    using Queries.Core.Repositories;

    public interface IStation : IRepository<tblUnits, StationModel>
    {
        StationListResult GetListResult(GridInfo gridInfo);
        StationModel GetStationById(int stationId);
        StationModel SaveStationDetails(StationModel model);
    }
}