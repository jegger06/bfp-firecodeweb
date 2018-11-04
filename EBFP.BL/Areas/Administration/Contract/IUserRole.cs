using System.Collections.Generic;
using EBFP.BL.Administration;
using EBFP.BL.Helper;

namespace EBFP.BL.Administration
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    public interface IUserRole : IRepository<tblUserRoles, UserRoleModel>
    {
        UserRoleListResult GetUserRoles(GridInfo gridInfo);
        UserRoleModel GetRoleById(int roleId);
        void DeleteRoleById(int roleId);
        int SaveUserRole(UserRoleModel role);
        bool HasDefaultAccess();
        bool CheckUserRole(int userRoleId);
    }
}
