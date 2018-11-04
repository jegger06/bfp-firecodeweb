using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EBFP.BL.Administration;
using EBFP.BL.Helper;
using EBFP.BL.HumanResources;
using EBFP.Helper;
using WebMatrix.WebData;

namespace EBFP.Web.Areas.Administration.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        IAdministrationUnitOfWork unitOfWork { get;}
        // default constructor, establishes default service

        public RoleController()
        {
            unitOfWork = new AdministrationUnitOfWork();
        }
        // GET: Admin/Role
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Access()
        {
            return View();
        }

        public ActionResult UserInRoles()
        {
            return View();
        }

        public ActionResult UserRoles()
        {
            return View();
        }

        public ActionResult UserRoleDetails(string sRole_Id, string pageStatus)
        {
            var details = new UserRoleModel();
            try
            {
                if(pageStatus == PageStatus.Success.ToString())
                    ViewBag.PageStatus = PageStatus.Success.ToString();

                if (string.IsNullOrWhiteSpace(sRole_Id))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var RoleId = Convert.ToInt32(sRole_Id.Decrypt());
                if (RoleId > 0)
                    details = unitOfWork.UserRole.GetRoleById(RoleId);
            }
            catch (Exception ex)
            {
                var ExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ExceptionMessage);
            }
            
            
            return View(details);
        }

        [HttpPost]
        public JsonResult GetUserRoles(GridInfo gridInfo)
        {
            try
            {
                var ret = unitOfWork.UserRole.GetUserRoles(gridInfo);

                var retJson = ret.UserRoleListModel.Select(a => new
                {
                    a.Role_Name,
                    a.Role_Description,
                    a.NumberOfAccess,
                    a.sRole_ID
                });

                var jsonResult = Json(new
                {
                    recordsTotal = ret.DatatableInfo.recordsTotal,
                    recordsFiltered = ret.DatatableInfo.recordsTotal,
                    data = retJson
                }, JsonRequestBehavior.AllowGet);

                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
                return null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserRoleDetails(UserRoleModel roleModel)
        {
            try
            {
                if (roleModel.Role_DefaultAccess && roleModel.Role_AllAccess)
                    throw new Exception("You cannot set both Default Access and Set All Access at the same time. Choose only one.");

                if (string.IsNullOrWhiteSpace(roleModel.sRole_ID))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var list = roleModel.RoleAccessList.Select(a => a.RA_PageSecurityID).ToList();
                if (list.Count != list.Distinct().Count())
                    throw new Exception("Role Access has duplicate!");

                if (ModelState.IsValid)
                {
                    if (roleModel.Role_ID == 0 && string.IsNullOrWhiteSpace(roleModel.Role_Name))
                        throw new Exception("Role Name is required");

                    roleModel.Role_ID = Convert.ToInt32(roleModel.sRole_ID.Decrypt());
                    int roleId = unitOfWork.UserRole.SaveUserRole(roleModel);

                    if (roleId > 0)
                    {
                        ViewBag.PageStatus = PageStatus.Success.ToString();
                        return RedirectToAction("UserRoleDetails", new
                        {
                            sRole_Id = roleId.ToString().Encrypt(),
                            PageStatus = PageStatus.Success.ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
            }
            return View(roleModel);
        }

        [HttpPost]
        public string DeleteRole(string sRole_Id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sRole_Id))
                    throw new Exception("Cannot find item.");

                var roleId = Convert.ToInt32(sRole_Id.Decrypt());
                var inUsed = unitOfWork.UserRole.CheckUserRole(roleId);

                if (inUsed)
                    throw new Exception("This role is currently in used.");

                unitOfWork.UserRole.DeleteRoleById(roleId);
                ViewBag.PageStatus = PageStatus.Success.ToString();

                //return RedirectToAction("UserRoles");
            }
            catch (Exception ex)
            {
                return ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }

            return "Success";
        }

        
        [HttpPost]
        public JsonResult GetUserInRoles(GridInfo gridInfo)
        {
            try
            {
                var ret = unitOfWork.UserInRole.GetUserInRoles(gridInfo);
                var jsonResult = Json(new
                {
                    recordsTotal = ret.DatatableInfo.recordsTotal,
                    recordsFiltered = ret.DatatableInfo.recordsTotal,
                    data = ret.OvwUsersInRoleModel
                }, JsonRequestBehavior.AllowGet);

                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
                return null;
            }
        }

        [HttpPost]
        public JsonResult SaveUserInRoles(UserInRoleModel roleModel)
        {
            try
            {
                if (roleModel.UIR_RoleID == 0)
                    throw new Exception("Please fill required field!");
                if (ModelState.IsValid)
                {
                    if (unitOfWork.UserInRole.IsEmployeeExisting(roleModel.UIR_ID, roleModel.UIR_EmployeeID))
                        throw new Exception("Selected employee has existing role!");

                    unitOfWork.UserInRole.SaveUserInRole(roleModel);
                }
            }
            catch (Exception ex)
            {
                var jsonResultError = Json(new { message = ex.InnerException?.Message ?? ex.Message
                }, JsonRequestBehavior.AllowGet);
                return jsonResultError;
            }

            var jsonResult = Json(new  {  message = "success"  }, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        
        [HttpGet]
        public ActionResult DeleteUserInRole(string sUIR_ID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sUIR_ID))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var userInRoleId = Convert.ToInt32(sUIR_ID.Decrypt());
                unitOfWork.UserInRole.DeleteUserInRoleById(userInRoleId);
                ViewBag.PageStatus = PageStatus.Success.ToString();
            }
            catch (Exception ex)
            {
                var ExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ExceptionMessage);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult RoleAccessEditor()
        {
            return PartialView("~/Areas/Administration/Views/Role/Include/RoleAccess/Editor.cshtml", new RoleAccessModel());
        }
        
    }
}