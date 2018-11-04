using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using EBFP.BL.Administration;
using EBFP.BL.Helper;

namespace EBFP.BL.Administration
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Linq;
    using EBFP.Helper;

    public class RoleAccessBL : Repository<tblRoleAccess, RoleAccessModel>, IRoleAccess
    {
        public RoleAccessBL(EBFPEntities context) : base(context)
        {
        }

        public void InsertBulk(List<RoleAccessModel> model, int Role_Id)
        {
            if (model == null) return;

            foreach (var item in model)
            {
                item.RA_Role_ID = Role_Id;
            }

            InsertBulk(model, a => a.RA_Role_ID == Role_Id);
        }

        public bool HasAccess(PageArea pageArea)
        {
            var EmployeeID = CurrentUser.EmployeeId;
            var userRole = BFPContext.tblUserInRole
                            .FirstOrDefault(a => a.UIR_EmployeeID == EmployeeID);


            if (userRole == null)
            {
                var access = BFPContext.tblUserRoles.FirstOrDefault(a => a.Role_DefaultAccess);

                //if (access != null)
                //{
                //    var xx = access.tblRoleAccess.FirstOrDefault(a => a.RA_PageSecurityID == (int) pageArea);
                //    if (xx != null)
                //      return  true;
                //    else
                //        return false;
                //}
                return access?.tblRoleAccess.FirstOrDefault(a => a.RA_PageSecurityID == (int)pageArea) != null;
            }
            else
            {
                var access = userRole?.tblUserRoles.tblRoleAccess;
                return access?.FirstOrDefault(a => a.RA_PageSecurityID == (int)pageArea) != null;
            }
           
            
        }
    }
}
