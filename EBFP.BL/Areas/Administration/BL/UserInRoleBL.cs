using System.Collections.Generic;
using System.Linq.Dynamic;
using AutoMapper;
using EBFP.BL.Administration;
using EBFP.BL.Helper;
using EBFP.BL.HumanResources;
using EBFP.Helper;

namespace EBFP.BL.Administration
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Linq;

    public class UsernInRoleBL : Repository<tblUserInRole, UserInRoleModel>, IUserInRole
    {
        public UsernInRoleBL(EBFPEntities context) : base(context)
        {
            Mapper.CreateMap<vwUsersInRoleModel, vwUsersInRole>().ReverseMap();
            Mapper.CreateMap<List<vwUsersInRoleModel>, List<vwUsersInRole>>().ReverseMap();
        }

        public UserInRoleListResult GetUserInRoles(GridInfo gridInfo)
        {
            var searchTerms = gridInfo.searchUserInRoleModel; 
            var userRoles = BFPContext.vwUsersInRole.Where(a => a.Emp_DutyStatus == (int)DutyStatuses.Active);

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
            {
                userRoles = userRoles.Where(
                    a => a.Reg_Id == CurrentUser.RegionID);
            }

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
            {
                userRoles =
                    userRoles.Where(
                        a => a.Province_Id == CurrentUser.ProvinceID);
            }

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
            {
                userRoles = userRoles.Where(a => a.Emp_Curr_Unit == CurrentUser.EmployeeUnitId);
            }

            if (!string.IsNullOrEmpty(searchTerms.EmployeeName))
                userRoles = userRoles.Where(a => 
                                            a.Emp_FirstName.Contains(searchTerms.EmployeeName) || 
                                            a.Emp_LastName.Contains(searchTerms.EmployeeName));
            if (!string.IsNullOrEmpty(searchTerms.Emp_Number))
                userRoles = userRoles.Where(a =>
                                            a.Emp_Number.Contains(searchTerms.Emp_Number));

            if (searchTerms.RoleId > 0)
                userRoles = userRoles.Where(a => a.UIR_RoleID == searchTerms.RoleId);

            gridInfo.recordsTotal = userRoles.Select(a=> a.Emp_Id).Count();

            var result = userRoles.OrderBy(string.Format("{0} {1}", gridInfo.sortColumnName, gridInfo.sortOrder))
                .Skip(gridInfo.start)
                .Take(gridInfo.length)
                .Select(Mapper.Map<vwUsersInRole, vwUsersInRoleModel>)
                .ToList();
             
            return new UserInRoleListResult
            {
                OvwUsersInRoleModel = result,
                DatatableInfo = gridInfo
            };
        }

        public void DeleteUserInRoleById(int UIR_ID)
        {
            var details = BFPContext.tblUserInRole.FirstOrDefault(a => a.UIR_ID == UIR_ID);

            if (details != null)
            {
                BFPContext.tblUserInRole.Remove(details);
                BFPContext.SaveChanges();
            }

        }

        public bool SaveUserInRole(UserInRoleModel model)
        {
            IHRISUnitOfWork unitOfWork = new HRISUnitOfWork();

            var entity = new tblUserInRole();
            var uirId = model.UIR_ID;

            if (model.UIR_ID > 0)
            {
                entity = BFPContext.tblUserInRole.FirstOrDefault(a => a.UIR_ID == model.UIR_ID);
            }

            if (entity != null)
            {
                var oldUserInRoleId = entity.UIR_RoleID;

                entity = Mapper.Map(model, entity);

                if (model.UIR_ID == 0)
                    BFPContext.tblUserInRole.Add(entity);

                BFPContext.SaveChanges();
                
                var oldUserInRole = uirId > 0 ? GetUserRoleName(oldUserInRoleId) : "";
                var newUserInRole = GetUserRoleName(entity.UIR_RoleID);

                var logsModel = new LogsModel
                {
                    Log_Emp_Id = entity.UIR_EmployeeID,
                    Log_Remarks = string.IsNullOrEmpty(oldUserInRole) ? "User In Role - Newly created with role " + newUserInRole : "User in Role - From " + oldUserInRole + " to " + newUserInRole
                };
                unitOfWork.Logs.InsertLogs(logsModel);

                return true;
            }

            return false;
        }

        private string GetUserRoleName(int roleId)
        {
            var roleName = BFPContext.tblUserRoles.FirstOrDefault(a => a.Role_ID == roleId);

            return roleName == null ? "" : roleName.Role_Name;
        }

        public bool IsEmployeeExisting(int id, int empId)
        {
            var exist = BFPContext.tblUserInRole.FirstOrDefault(a => a.UIR_EmployeeID == empId && a.UIR_ID != id);
            return (exist != null);
        }

        public List<RoleModel> GetUserRoleList()
        {
            var ret = new List<RoleModel>();
            var list = BFPContext.tblUserRoles.OrderBy(a => a.Role_Name);

            foreach (var item in list)
            {
                var model = new RoleModel();
                model.RoleName = item.Role_Name;
                model.RoleId = item.Role_ID;

                ret.Add(model);
            }
            return ret.ToList();
        }
    }
}
