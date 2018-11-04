using System.Collections.Generic;
using EBFP.BL.Administration;
using EBFP.BL.Helper;

namespace EBFP.BL.Administration
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    public interface IUserInRole : IRepository<tblUserInRole, UserInRoleModel>
    {
        UserInRoleListResult GetUserInRoles(GridInfo gridInfo);
        void DeleteUserInRoleById(int UIR_ID);
        List<RoleModel> GetUserRoleList();
        bool SaveUserInRole(UserInRoleModel model);
        bool IsEmployeeExisting(int id, int empId);
    }
}
