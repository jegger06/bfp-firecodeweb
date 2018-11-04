
using EBFP.BL.HumanResources;

namespace EBFP.BL.Inventory
{
    using EBFP.DataAccess;
    using Helper;
    using Queries.Core.Repositories;

    public interface IMunicipality : IRepository<tblCityMunicipality, MunicipalityModel>
    {
        void UpdateInventoryMunicipality(MunicipalityModel model);
        int GetUnitIdByMunicipality(int municipalityId);
        MunicipalitySearchModel GetIdsByMunicipality(int municipalityId);
        PPEDashboardModel GetPPEDashboardCount(string sMunicipalityId = "");
    }
}