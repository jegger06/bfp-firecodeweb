namespace EBFP.BL.Inventory
{
    using EBFP.DataAccess;
    using Helper;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface IOtherVehicle : IRepository<tblOtherVehicles, OtherVehicleModel>
    {
        OtherVehicleModel GetOtherVehicleById(int vehicleId);
        OtherVehicleModel GetOtherVehicleByUnitId(int unitId);
        OtherVehicleListResult GetListResult(GridInfo gridInfo);
        void UpdateOtherVehicle(OtherVehicleModel model);
        bool DeleteByID(int truckId);
        OtherVehicleModel UploadPicture(OtherVehicleModel model);
        List<VehicleCountChartModel> GetVehicleCountDetails(string sMunicipalityId = "");
        VehicleCountSummaryModel GetVehicleSummaryCount();
        OtherVehicleListResult GetVehicleStationListResult(GridInfo gridInfo);
    }
}