using System.Collections.Generic;
using EBFP.BL.Administration;
using EBFP.BL.Helper;

namespace EBFP.BL.Administration
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    public interface IRoleAccess : IRepository<tblRoleAccess, RoleAccessModel>
    {
        void InsertBulk(List<RoleAccessModel> model, int Role_Id);
        bool HasAccess(PageArea pageArea);
    }
}
