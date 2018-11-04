namespace EBFP.BL.Inventory
{
    using EBFP.DataAccess;
    using Helper;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface ISubStation : IRepository<tblUnitSubStation, SubStationModel>
    {
        SubStationListResult GetListResult(GridInfo gridInfo);
        List<SubStationModel> GetSubStationByStation(int unitId);
        SubStationModel GetSubStationById(int subStationId);
        SubStationModel GetSubStationByUnitId(int unitId);
        SelectionFireMarshallModel GetFireMarshall(int EmpId);
        SubStationModel SaveSubStationDetails(SubStationModel model);
        bool DeleteByID(int subStationId);
    }
}