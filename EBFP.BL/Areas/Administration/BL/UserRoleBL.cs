using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EBFP.BL.Helper;
using EBFP.BL.HumanResources;
using EBFP.DataAccess;
using EBFP.Helper;
using Queries.Core.Repositories;

namespace EBFP.BL.Administration
{
    public class UserRoleBL : Repository<tblUserRoles, UserRoleModel>, IUserRole
    {
        public UserRoleBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public UserRoleListResult GetUserRoles(GridInfo gridInfo)
        {
            var searchTerms = gridInfo.searchUserRoleModel;
            var userRoles = BFPContext.tblUserRoles.AsQueryable();


            if (!string.IsNullOrEmpty(searchTerms.Role_Name))
                userRoles = userRoles.Where(a => a.Role_Name.Contains(searchTerms.Role_Name));
            if (!string.IsNullOrEmpty(searchTerms.Role_Description))
                userRoles = userRoles.Where(a => a.Role_Description.Contains(searchTerms.Role_Description));


            gridInfo.recordsTotal = userRoles.Count();
            var employeesResult = userRoles.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
                .Skip(gridInfo.start)
                .Take(gridInfo.length)
                .ToList();

            var retRoles = employeesResult.Select(item => new UserRoleModel
            {
                Role_Name = item.Role_Name,
                Role_Description = item.Role_Description,
                NumberOfAccess = item.tblRoleAccess.Count,
                Role_ID = item.Role_ID
            }).ToList();

            return new UserRoleListResult
            {
                UserRoleListModel = retRoles,
                DatatableInfo = gridInfo
            };
        }


        public UserRoleModel GetRoleById(int roleId)
        {
            var model = new UserRoleModel();
            var details = BFPContext.tblUserRoles.FirstOrDefault(a => a.Role_ID == roleId);

            Mapper.Map(details, model);
            if (details != null && details.tblRoleAccess.Count > 0)
            {
                var accessList = details.tblRoleAccess.AsQueryable().Project().To<RoleAccessModel>().ToList();
                model.RoleAccessList = accessList;
            }

            return model;
        }

        public void DeleteRoleById(int roleId)
        {
            var details = BFPContext.tblUserRoles.FirstOrDefault(a => a.Role_ID == roleId);

            if (details != null)
            {
                var toDelete = BFPContext.tblRoleAccess.Where(a => a.RA_Role_ID == roleId).ToList();
                if (toDelete.Count > 0)
                {
                    BFPContext.tblRoleAccess.RemoveRange(toDelete);
                    BFPContext.SaveChanges();
                }

                BFPContext.tblUserRoles.Remove(details);
                BFPContext.SaveChanges();
            }
        }

        public int SaveUserRole(UserRoleModel role)
        {
            IAdministrationUnitOfWork UnitOfWork = new AdministrationUnitOfWork(BFPContext);
            var id = 0;
            var entity = new tblUserRoles();
            if (role.Role_ID > 0)
            {
                entity = BFPContext.tblUserRoles.FirstOrDefault(a => a.Role_ID == role.Role_ID);
                id = entity.Role_ID;
            }

            if (entity != null)
            {
                entity = Mapper.Map(role, entity);
                entity.Role_ModifiedBy = CurrentUser.EmployeeId;
                entity.Role_ModifiedDate = DateTime.Now;
                entity.Role_ID = id;

                if (role.Role_ID == 0)
                {
                    entity.Role_CreatedBy = CurrentUser.EmployeeId;
                    entity.Role_CreatedDate = DateTime.Now;
                    BFPContext.tblUserRoles.Add(entity);
                }

                BFPContext.SaveChanges();

                if (role.Role_AllAccess)
                    role.RoleAccessList = SetAllAccess(entity.Role_ID);

                UnitOfWork.RoleAccess.InsertBulk(role.RoleAccessList, entity.Role_ID);
                UnitOfWork.Complete();

                return entity.Role_ID;
            }

            return 0;
        }

        public bool HasDefaultAccess()
        {
            return BFPContext.tblUserRoles.FirstOrDefault(a => a.Role_DefaultAccess) != null;
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblUserRoles, UserRoleModel>();
            Mapper.CreateMap<tblUserRoles, UserRoleModel>().ReverseMap();

            Mapper.CreateMap<tblRoleAccess, RoleAccessModel>();
            Mapper.CreateMap<tblRoleAccess, RoleAccessModel>().ReverseMap();
        }

        public List<RoleAccessModel> SetAllAccess(int roleId)
        {
            var list = new List<RoleAccessModel>();
            foreach (var item in PageArea.HRIS.ToSelectList())
            {
                if (item.Text.Contains("Restrict")) continue;

                var model = new RoleAccessModel
                {
                    RA_Role_ID = roleId,
                    RA_PageSecurityID = Convert.ToInt32(item.Value)
                };
                list.Add(model);
            }

            return list;
        }

        //public bool SendConfirmDeviceEmail(string fullName, string userEmail, string confirmationKey)
        //{
        //    try
        //    {
        //        var oEmail = new MailHelper
        //        {
        //            EmailTo = userEmail,
        //            Subject = "Device authorization needed - Provider Relationship Management (PRiM)"
        //        };

        //        var mailBody = File.ReadAllText(Common.Email_Template_Path + @"Default.html");

        //        var sb = new StringBuilder();
        //        sb.Append("Your account was used to sign in on new device recently: " + DateTime.UtcNow + " UTC<br/>");
        //        sb.Append("We could not verify if your account has been used on this device before.<br/>");
        //        sb.Append(
        //            "To ensure your account's security, you need to add this device to your trusted devices before you can complete the sign in.<br/>");
        //        sb.Append("Please follow the link to add this device to your trusted devices: <a href='" + BaseUrl +
        //                  "/Login/ConfirmDevice?confirmationKey=" + confirmationKey +
        //                  "'>Add to trusted devices</a><br/>");
        //        sb.Append(
        //            "If you did not try to sign in on new device or you are unsure, we recommend that you change your account password to keep your data safe.<br/>");

        //        mailBody = mailBody.Replace("{{Name}}", fullName)
        //            .Replace("{{userEmail}}", userEmail)
        //            .Replace("{{emailContent}}", sb.ToString());

        //        oEmail.Body = mailBody;
        //        oEmail.SendEmail();
        //    }
        //    catch (HttpResponseException)
        //    {
        //        throw;
        //    }
        //    catch (Exception exception)
        //    {
        //        ExceptionHelper.ThrowHttpResponseException(exception);
        //        throw;
        //    }

        //    return true;
        //}
        public bool CheckUserRole(int userRoleId)
        {
            var userInRole = BFPContext.tblUserInRole.FirstOrDefault(a => a.UIR_RoleID == userRoleId);
            if (userInRole != null)
                return true;
            else
                return false;
        }
    }
}