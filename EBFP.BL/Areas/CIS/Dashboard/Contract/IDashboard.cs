using System.Collections.Generic;
using EBFP.BL.Establishment;
using EBFP.BL.Inventory;

namespace EBFP.BL.CIS
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    public interface IDashboard
    {
        List<DashboardPersonnelModel> GetDashPersonnelPerRegion();
        List<FiveYearInspectionStatisticModel> GetInspectionOrderOnEstablishmemts();
        //List<FiveYearStatisticModel> GetInspectedEstablishment();
        List<FireFeesCollectionModel> GetFireCodeFees();
        //List<FirePreventionDashboardModel> GetFirePreventionActivities();
        FCREstablishmentModel GetEstablishmentCounter(int municipalityId);
        DashboardPersonnelModel GetDashPersonnelPerMunicipality(int municipalityId);

    }
}
