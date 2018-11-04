
using EBFP.BL.CIS;

namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Helper;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface IMunicipality : IRepository<tblCityMunicipality, MunicipalityModel>
    {
        List<SelectionUnitModel> GetAllForSelection();
        MunicipalityModel GetMunicipalityById(int municipalityId);
        MunicipalityListResult GetListResult(GridInfo gridInfo);
        void UpdateMunicipality(MunicipalityModel model);
        bool DeleteByID(int municipalityId);
        List<MunicipalityModel> GetMunicipalityPerProvince(int provinceId);
        List<FireFightingModel> GetFireFightingCountDetails();
        int GetPopulation();
        bool CheckMunicipality(int municipalityId);
    }
}