
namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Helper;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface IUnit : IRepository<tblUnits, UnitModel>
    {
        List<SelectionUnitModel> GetAllForSelection();
        UnitModel GetUnitById(int unit_Id);
        UnitListResult GetListResult(GridInfo gridInfo);
        void UpdateUnit(UnitModel model);
        bool DeleteByID(int employeeID);
        //List<UnitModel> GetMunicipalityPerProvince(int provinceId);
        List<UnitModel> GetUnitByMunicipality(int municipalityId);
        List<UnitModel> GetUnitByProvince(int provinceId);
        bool CheckUnit(int unitId);
    }
}