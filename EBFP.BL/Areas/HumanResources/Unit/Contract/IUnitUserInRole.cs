
namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Helper;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface IUnitUserInRole : IRepository<tblUnitsUserInRole, UnitUserInRoleModel>
    {
        void SyncUnitsUserInRoleLocalToServer(List<UnitUserInRoleModel> model);
    }
}