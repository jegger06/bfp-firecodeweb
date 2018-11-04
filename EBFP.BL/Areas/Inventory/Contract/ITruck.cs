
namespace EBFP.BL.Inventory
{
    using EBFP.DataAccess;
    using Helper;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface ITruck : IRepository<tblTrucks, TruckModel>
    {
        TruckModel GetTruckById(int truckId);
        TruckModel GetTruckByUnitId(int unitId);
        TruckListResult GetListResult(GridInfo gridInfo);
        void UpdateTruck(TruckModel model);
        bool DeleteByID(int truckId);
        TruckModel UploadPicture(TruckModel model);
        List<TruckCountChartModel> GetTruckCountDetails(string sMunicipalityId = "");
        List<TruckAgeCountChartModel> GetTruckAgeCountDetails(string sMunicipalityId = "");
        TruckCountSummaryModel GetTruckSummaryCount(string sMunicipalityId = "");
        List<TruckAgeCountChartModel> GetTruckAgeGroupCount(string sMunicipalityId = "");
        List<tblTrucks> GetActualNRFireTrucks();
        TruckListResult GetTruckStationListResult(GridInfo gridInfo);
    }
}