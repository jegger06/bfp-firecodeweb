using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using EBFP.BL.Helper;
using EBFP.BL.HumanResources;
using EBFP.Helper;
using Newtonsoft.Json;
using WebMatrix.WebData;
using System.Web;
using System.Web.UI.WebControls;
using EBFP.DataAccess;
using EBFP.Web.Areas.Account.Controllers;
using EBFP.Web.Areas.HelpPage;

// ReSharper disable MergeConditionalExpression

namespace EBFP.Web.Areas.HRIS.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        // default constructor, establishes default service

        public EmployeeController()
        {
            unitOfWork = new HRISUnitOfWork();
        }
        // overloaded 'injectable' constructor
        // ** Constructor Dependency Injection (DI).

        public EmployeeController(HRISUnitOfWork employee)
        {
            unitOfWork = employee;
        }

        private IHRISUnitOfWork unitOfWork { get; }

        public ActionResult EmployeeRoster(string result)
        {
            var employeeModel = new EmployeeModel();
            employeeModel.ProvinceID = CurrentUser.ProvinceID;
            employeeModel.MunicipalityID = CurrentUser.MunicipalityID;
            employeeModel.Emp_Curr_Unit = CurrentUser.EmployeeUnitId;
            employeeModel.UploadResult = result;
            return View(employeeModel);
        }

        [HttpGet]
        public JsonResult SelectionAutoComplete(string search)
        {
            try
            {
                var ranks = unitOfWork.Rank.GetList();
                dynamic ret;
                if (PageSecurity.HasAccess(PageArea.HRIS_EmployeeRoster_RestricttoRegion))
                {
                     ret = unitOfWork.Employee
                     .GetList(
                         a =>
                             (a.Emp_FirstName.Contains(search) || a.Emp_LastName.Contains(search) ||
                             a.Emp_Number.Contains(search)) && a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID && a.Emp_IsDeleted != true).Select(a => new
                             {
                                 text = (a.Emp_Curr_Rank != null && a.Emp_Curr_Rank > 0 ? ranks.FirstOrDefault(b => b.Rank_Id == a.Emp_Curr_Rank).Rank_Name : "") + " " + a.Emp_FirstName + " " + a.Emp_MiddleName + " " + a.Emp_LastName,
                                 id = a.Emp_Id
                             }).OrderBy(a => a.text);

                }

               else if (PageSecurity.HasAccess(PageArea.HRIS_EmployeeRoster_RestricttoProvince))
                {
                 
                    ret = unitOfWork.Employee
                    .GetList(
                        a =>
                            (a.Emp_FirstName.Contains(search) || a.Emp_LastName.Contains(search) ||
                            a.Emp_Number.Contains(search)) && a.tblUnits.tblCityMunicipality.Municipality_Province_Id == CurrentUser.ProvinceID && a.Emp_IsDeleted != true).Select(a => new
                            {
                                text = (a.Emp_Curr_Rank != null && a.Emp_Curr_Rank > 0 ? ranks.FirstOrDefault(b => b.Rank_Id == a.Emp_Curr_Rank).Rank_Name : "") + " " + a.Emp_FirstName + " " + a.Emp_MiddleName + " " + a.Emp_LastName,
                                id = a.Emp_Id
                            }).OrderBy(a => a.text);
                }

               else if (PageSecurity.HasAccess(PageArea.HRIS_EmployeeRoster_RestricttoStation))
               {
                     ret = unitOfWork.Employee
                   .GetList(
                       a =>
                           (a.Emp_FirstName.Contains(search) || a.Emp_LastName.Contains(search) ||
                           a.Emp_Number.Contains(search) ) && a.Emp_Curr_Unit == CurrentUser.EmployeeUnitId && a.Emp_IsDeleted != true).Select(a => new
                           {
                               text = (a.Emp_Curr_Rank != null && a.Emp_Curr_Rank > 0 ? ranks.FirstOrDefault(b => b.Rank_Id == a.Emp_Curr_Rank).Rank_Name : "") + " " + a.Emp_FirstName + " " + a.Emp_MiddleName + " " + a.Emp_LastName,
                               id = a.Emp_Id
                           }).OrderBy(a => a.text);
                }

               else
               {
                    ret = unitOfWork.Employee
                       .GetList(
                           a =>
                               a.Emp_FirstName.Contains(search) || a.Emp_LastName.Contains(search) ||
                               a.Emp_Number.Contains(search) && a.Emp_IsDeleted != true).Select(a => new
                               {
                                   text = (a.Emp_Curr_Rank != null && a.Emp_Curr_Rank > 0 ? ranks.FirstOrDefault(b => b.Rank_Id == a.Emp_Curr_Rank).Rank_Name : "") + " " + a.Emp_FirstName + " " + a.Emp_MiddleName + " " + a.Emp_LastName,
                                   id = a.Emp_Id
                               }).OrderBy(a => a.text);
               }


                var jsonResult = Json(new
                {
                    data = ret
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

        [HttpGet]
        public JsonResult SelectionAutoCompleteRetired(string search)
        {
            try
            {
                var ranks = unitOfWork.Rank.GetList();
                dynamic ret;
                if (PageSecurity.HasAccess(PageArea.HRIS_EmployeeRoster_RestricttoRegion))
                {
                    ret = unitOfWork.Employee
                        .GetList(a => (a.Emp_FirstName.Contains(search) || a.Emp_LastName.Contains(search) ||
                               a.Emp_Number.Contains(search)) &&
                                      a.Emp_DutyStatus == (int)DutyStatuses.Retired && a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID && a.Emp_IsDeleted != true).Select(a => new
                                      {
                                          text = (a.Emp_Curr_Rank != null && a.Emp_Curr_Rank > 0 ? ranks.FirstOrDefault(b => b.Rank_Id == a.Emp_Curr_Rank).Rank_Name : "") + " " + a.Emp_FirstName + " " + a.Emp_MiddleName + " " + a.Emp_LastName,
                                          id = a.Emp_Id
                                      }).OrderBy(a => a.text);

                }

                else if (PageSecurity.HasAccess(PageArea.HRIS_EmployeeRoster_RestricttoProvince))
                {
                    ret = unitOfWork.Employee
                        .GetList(a => (a.Emp_FirstName.Contains(search) || a.Emp_LastName.Contains(search) ||
                               a.Emp_Number.Contains(search)) &&
                                      a.Emp_DutyStatus == (int)DutyStatuses.Retired && a.tblUnits.tblCityMunicipality.Municipality_Province_Id == CurrentUser.ProvinceID && a.Emp_IsDeleted != true).Select(a => new
                                      {
                                          text = (a.Emp_Curr_Rank != null && a.Emp_Curr_Rank > 0 ? ranks.FirstOrDefault(b => b.Rank_Id == a.Emp_Curr_Rank).Rank_Name : "") + " " + a.Emp_FirstName + " " + a.Emp_MiddleName + " " + a.Emp_LastName,
                                          id = a.Emp_Id
                                      }).OrderBy(a => a.text);
                }

                else if (PageSecurity.HasAccess(PageArea.HRIS_EmployeeRoster_RestricttoStation))
                {

                    ret = unitOfWork.Employee
                        .GetList(a => (a.Emp_FirstName.Contains(search) || a.Emp_LastName.Contains(search) ||
                               a.Emp_Number.Contains(search)) &&
                                      a.Emp_DutyStatus == (int) DutyStatuses.Retired &&
                                      a.Emp_Curr_Unit == CurrentUser.EmployeeUnitId && a.Emp_IsDeleted != true).Select(a => new
                                      {
                                          text = (a.Emp_Curr_Rank != null && a.Emp_Curr_Rank > 0 ? ranks.FirstOrDefault(b => b.Rank_Id == a.Emp_Curr_Rank).Rank_Name : "") + " " + a.Emp_FirstName + " " + a.Emp_MiddleName + " " + a.Emp_LastName,
                                          id = a.Emp_Id
                                      }).OrderBy(a => a.text);
                }

                else
                {
                    ret = unitOfWork.Employee
                        .GetList(a => (a.Emp_FirstName.Contains(search) || a.Emp_LastName.Contains(search) ||
                               a.Emp_Number.Contains(search)) &&
                                      a.Emp_DutyStatus == (int) DutyStatuses.Retired && a.Emp_IsDeleted != true).Select(a => new
                                      {
                                          text = (a.Emp_Curr_Rank != null && a.Emp_Curr_Rank > 0 ? ranks.FirstOrDefault(b => b.Rank_Id == a.Emp_Curr_Rank).Rank_Name : "") + " " + a.Emp_FirstName + " " + a.Emp_MiddleName + " " + a.Emp_LastName,
                                          id = a.Emp_Id
                                      }).OrderBy(a => a.text);
                }


                var jsonResult = Json(new
                {
                    data = ret
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
        public JsonResult GetEmployees(GridInfo gridInfo)
        {
            try
            { 
                var ret = unitOfWork.Employee.GetEmployees(gridInfo);
                var jsonResult = Json(new
                {
                    ret.DatatableInfo.recordsTotal,
                    recordsFiltered = ret.DatatableInfo.recordsTotal,
                    data = ret.EmployeeListModel
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

        [OutputCache(Duration = 10, VaryByParam = "sEmp_Id")]
        public ActionResult EmployeeDetails(string sEmp_Id, string AccessType, string PageStatus)
        {
            ViewBag.PageStatus = PageStatus;
            if (string.IsNullOrWhiteSpace(sEmp_Id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var employeeID = Convert.ToInt32(sEmp_Id.Decrypt());
            var retEmployee = new EmployeeModel();
            if (employeeID > 0)
            {
                retEmployee = unitOfWork.Employee.EmployeeDetails(employeeID);
                if (retEmployee == null)
                    return HttpNotFound();
            }

            return View(retEmployee);
        }

        private void ChangePassword(EmployeeModel tblEmployees)
        {
            var token = WebSecurity.GeneratePasswordResetToken(tblEmployees.Emp_Username);
            var isChangepasswordSuccess = WebSecurity.ResetPassword(token, tblEmployees.user.NewPassword);
            if (!isChangepasswordSuccess)
                throw new Exception(
                    "An error occurred while the wizard was attempting to set the password for this user account.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeDetails(EmployeeModel tblEmployees)
        {
            try
            {
                var isValid = false;
                if (string.IsNullOrWhiteSpace(tblEmployees.sEmp_Id))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var accessType = string.Empty;
                if (!string.IsNullOrWhiteSpace(Request.QueryString["AccessType"]))
                    accessType = Request.QueryString["AccessType"].Decrypt();
                else if(!string.IsNullOrWhiteSpace(tblEmployees.AccessType))
                    accessType = tblEmployees.AccessType.Decrypt();

                if (accessType == "View")
                {
                    isValid = true;
                }

                if (ModelState.IsValid || isValid)
                {
                    MemoryStream target = new MemoryStream();
                    tblEmployees.EmployeeImage?.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    var isImageByteEmpty = data.All(B => B == default(Byte));
                    if (!isImageByteEmpty)
                    {
                        tblEmployees.Emp_Photo = data;
                    }

                    var isEditProfile = accessType == "View";
                    if (tblEmployees.Emp_Id == 0 && string.IsNullOrWhiteSpace(tblEmployees.user.NewPassword))
                        throw new Exception("Password is required");

                    tblEmployees.Emp_Id = Convert.ToInt32(tblEmployees.sEmp_Id.Decrypt());
                    unitOfWork.Employee.SaveEmployeeDetails(tblEmployees, isEditProfile);

                    if (tblEmployees.Emp_Id == CurrentUser.EmployeeId)
                    {
                        var controller = DependencyResolver.Current.GetService<AccountController>();
                        controller.ControllerContext = new ControllerContext(Request.RequestContext, controller);
                        controller.SetCustomAuthenticationCookie(CurrentUser.Username, false);
                    }

                    if (!unitOfWork.User.IsMember(tblEmployees.Emp_Id) &&
                        !string.IsNullOrWhiteSpace(tblEmployees.user.NewPassword))
                    {
                        WebSecurity.CreateAccount(tblEmployees.Emp_Username, tblEmployees.user.NewPassword);
                        unitOfWork.User.UpdateDecryptedPassword(tblEmployees.Emp_Id, tblEmployees.user.NewPassword);
                    }
                    else if (unitOfWork.User.IsMember(tblEmployees.Emp_Id) &&
                             !string.IsNullOrWhiteSpace(tblEmployees.user.NewPassword))
                    {
                        ChangePassword(tblEmployees);
                        unitOfWork.User.UpdateDecryptedPassword(tblEmployees.Emp_Id, tblEmployees.user.NewPassword);
                    }
                     
                    return RedirectToAction("EmployeeDetails", new
                    {
                        sEmp_Id = tblEmployees.Emp_Id.ToString().Encrypt(),
                        AccessType = accessType.Encrypt(),
                        PageStatus = PageStatus.Success.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
            }
            return View(tblEmployees);
        }

        public async Task<ActionResult> MedicalDetails(string sEmp_Id)
        {
            if (string.IsNullOrWhiteSpace(sEmp_Id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var employeeID = Convert.ToInt32(sEmp_Id.Decrypt());
            var retEmployee = new EmployeeMedicalModel();
            if (employeeID > 0)
            {
                retEmployee = await unitOfWork.Medical.EmployeeMedicalDetails(employeeID);
                if (retEmployee == null)
                    return HttpNotFound();
            }

            return View(retEmployee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MedicalDetails(EmployeeMedicalModel tblMedical)
        {
            try
            {
                unitOfWork.Medical.SaveMedicalDetails(tblMedical);

                if (ModelState.IsValid)
                    ViewBag.PageStatus = PageStatus.Success.ToString();
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
            }
            return View(tblMedical);
        }


        [HttpPost]
        public JsonResult GetPhysicalExamList(GridInfo gridInfo)
        {
            try
            {
                var ret = unitOfWork.PhysicalExam.GetPhysicalExams(gridInfo);
                var jsonResult = Json(new
                {
                    ret.DatatableInfo.recordsTotal,
                    recordsFiltered = ret.DatatableInfo.recordsTotal,
                    data = ret.PhysicalExamListModel
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

        public async Task<ActionResult> PhysicalExamDetails(string sPE_Id, string sEmp_Id)
        {
            if (string.IsNullOrWhiteSpace(sPE_Id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var retEmployee = new PhysicalExamModel();
            var physicalExamId = Convert.ToInt32(sPE_Id.Decrypt());
            retEmployee.PatientInformation.Emp_Id = Convert.ToInt32(sEmp_Id.Decrypt());
            retEmployee.PatientInformation.PE_Id = physicalExamId;

            if (physicalExamId > 0)
            {
                retEmployee = await unitOfWork.PhysicalExam.PhysicalExamDetails(physicalExamId);
                if (retEmployee == null)
                    return HttpNotFound();
            }

            return View(retEmployee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PhysicalExamDetails(PhysicalExamModel tblPE)
        {
            try
            {
                unitOfWork.PhysicalExam.SavePhysicalExamDetails(tblPE);

                var json = JsonConvert.SerializeObject(tblPE);

                if (string.IsNullOrWhiteSpace(tblPE.PatientInformation.sPE_Id))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                if (ModelState.IsValid)
                {
                    tblPE.PatientInformation.PE_Id = Convert.ToInt32(tblPE.PatientInformation.sPE_Id.Decrypt());
                    ViewBag.PageStatus = PageStatus.Success.ToString();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
            }
            return View(tblPE);
        }

        public ActionResult UploadAlpha(EmployeeModel files)
        {
            try
            {
                var newUsers = new List<string>();
                unitOfWork.Employee.UpdateByAlphaList(files.Alphalist,ref newUsers);

                Parallel.ForEach(newUsers, (userName) =>
                {
                    WebSecurity.CreateAccount(userName, "Admin1@3");//DEFAULT PASSWORD : "Admin1@3"
                    unitOfWork.User.UpdateDecryptedPasswordByUsername(userName, "Admin1@3");
                });

                return RedirectToAction("EmployeeRoster", new { result = "Success" });
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
                return RedirectToAction("EmployeeRoster", new { result = ViewBag.ExceptionMessage });
            }
        }

        [HttpGet]
        public ActionResult Delete(string sEmp_Id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sEmp_Id))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var Emp_Id = Convert.ToInt32(sEmp_Id.Decrypt());
                unitOfWork.Employee.DeleteByID(Emp_Id);
                ViewBag.PageStatus = PageStatus.Success.ToString();
            }
            catch (Exception ex)
            {
                var ExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ExceptionMessage);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public ActionResult DeletePhysicalExam(string sPE_Id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sPE_Id))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var PE_Id = Convert.ToInt32(sPE_Id.Decrypt());
                unitOfWork.PhysicalExam.DeleteByPEID(PE_Id);
                ViewBag.PageStatus = PageStatus.Success.ToString();
            }
            catch (Exception ex)
            {
                var ExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ExceptionMessage);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult LeaveCreditManager()
        {
            return View(new List<UnitModel>());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }

        public string PrintPDS(string sEmp_Id)
        {
            var employeeID = Convert.ToInt32(sEmp_Id.Decrypt());

            if (employeeID > 0)
                unitOfWork.PDSExport.PrintPDS(employeeID);

            return "Success";
        }

        [HttpPost]
        public string DownloadAlphaTemplate(List<string> list)
        {
            return unitOfWork.HRISReport.DownloadAlphaTemplate(list);
        }

        [HttpPost]
        public string ExportAlpha(EmployeeSearchModel model, List<string> list)
        {
            var filename = unitOfWork.HRISReport.DownloadAlphaTemplate(list);
            var employees = unitOfWork.Employee.GetAlphaList(model, filename); 
            return unitOfWork.HRISReport.ExportAlpha(employees, filename);
        }

        [HttpPost]
        public string SetValidatedEmp(string type,int empId)
        {
            unitOfWork.Employee.UpdateValidatedEmp(type, empId, CurrentUser.EmployeeId);

            if (type != "Validated")
                return  "";

            return unitOfWork.Employee.GetEmployeeNameById(CurrentUser.EmployeeId);
        }

        [HttpPost]
        public string GetValidatedBy(int empId)
        {
            return empId <= 0 ? "" : unitOfWork.Employee.GetEmployeeNameById(empId);
        }

        #region Leave

        public async Task<ActionResult> LeaveDetails(string sEmp_Id)
        {
            if (string.IsNullOrWhiteSpace(sEmp_Id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var employeeID = Convert.ToInt32(sEmp_Id.Decrypt());
            if (employeeID == 0) return HttpNotFound();

            ViewBag.EmployeeID = employeeID;
            ViewBag.sEmployeeID = sEmp_Id;

            var leaveCredits = await unitOfWork.Leave.GetLeaveCredits(employeeID);

            return View(leaveCredits);
        }

        [HttpPost]
        public JsonResult GetLeaveList(GridInfo gridInfo)
        {
            try
            {
                var ret = unitOfWork.Leave.GetEmployeeLeave(gridInfo);
                var jsonResult = Json(new
                {
                    ret.DatatableInfo.recordsTotal,
                    recordsFiltered = ret.DatatableInfo.recordsTotal,
                    data = ret.LeaveListModel
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

        public async Task<ActionResult> LeaveRecord(string sELR_Id, string sEmp_Id)
        {
            if (string.IsNullOrWhiteSpace(sELR_Id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var employeeId = Convert.ToInt32(sEmp_Id.Decrypt());
            var leaveId = Convert.ToInt32(sELR_Id.Decrypt());
            var retLeave = new LeaveModel();
            if (leaveId > 0)
            {
                retLeave = unitOfWork.Leave.LeaveRecord(leaveId);
                if (retLeave == null)
                    return HttpNotFound();
            }
            else
            {
                retLeave.ELR_StartDate = DateTime.Now.Date;
                retLeave.ELR_EndDate = DateTime.Now.Date;

                if (!string.IsNullOrEmpty(sEmp_Id) && employeeId > 0)
                    retLeave.ELR_Emp_Id = Convert.ToInt32(sEmp_Id.Decrypt());
            }

            var leaveCredits = await unitOfWork.Leave.GetLeaveCredits(employeeId);
            ViewBag.sEmployeeID = sEmp_Id;
            ViewBag.Vacation = leaveCredits.RemainingVacationLeave;
            ViewBag.Sick = leaveCredits.RemainingSickLeave;
            ViewBag.Total = leaveCredits.TotalRemainingLeave;

            return View(retLeave);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LeaveRecord(LeaveModel leave)
        {
            try
            {
                if (leave.ELR_StartDate.Year != leave.ELR_EndDate.Year)
                    throw new Exception(
                        "You can only apply leave with the same year.  Please book another leave for next year.");

                leave.ELR_InputDate = DateTime.Now;
                leave.ELR_Input_Emp_Id = CurrentUser.EmployeeId;

                leave = unitOfWork.Leave.SaveLeaveRecord(leave);

                if (ModelState.IsValid)
                    ViewBag.PageStatus = PageStatus.Success.ToString();

                return RedirectToAction("LeaveDetails", new
                {
                    sEmp_Id = leave.ELR_Emp_Id.ToString().Encrypt()
                });
            }
            catch (Exception ex)
            {
                ViewBag.ExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.PageStatus = PageStatus.Error.ToString();
            }

            return View(leave);
        }

        [HttpGet]
        public ActionResult DeleteLeaveRecord(string sELR_Id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sELR_Id))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                var PE_Id = Convert.ToInt32(sELR_Id.Decrypt());
                unitOfWork.Leave.DeleteLeaveRecord(PE_Id);
                ViewBag.PageStatus = PageStatus.Success.ToString();
            }
            catch (Exception ex)
            {
                var ExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ExceptionMessage);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        #endregion

        #region EditorTempates

        public ActionResult EmployeeChildEditor()
        {
            return PartialView("~/Areas/HRIS/Views/Employee/Include/EmployeeDetails/FamilyBackgound/Editor.cshtml",
                new EmployeeChildModel());
        }

        public ActionResult EducBackgrounditor()
        {
            var model = new EducBackgroundModel {CreatedFromAjax = true};
            return PartialView(
                "~/Areas/HRIS/Views/Employee/Include/EmployeeDetails/EducationalBackground/Editor.cshtml", model);
        }

        public ActionResult CivilServiceEligibilityEditor()
        {
            return PartialView(
                "~/Areas/HRIS/Views/Employee/Include/EmployeeDetails/CivilServiceEligibility/Editor.cshtml",
                new CivilServiceEligibilityModel());
        }

        public ActionResult ServiceRecordEditor()
        {
            return PartialView("~/Areas/HRIS/Views/Employee/Include/EmployeeDetails/ServiceRecord/Editor.cshtml",
                new ServiceAppointmentModel());
        }

        public ActionResult WorkExperienceEditor()
        {
            return PartialView("~/Areas/HRIS/Views/Employee/Include/EmployeeDetails/WorkExperience/Editor.cshtml",
                new WorkExperienceModel());
        }

        public ActionResult VoluntaryWorkEditor()
        {
            return PartialView("~/Areas/HRIS/Views/Employee/Include/EmployeeDetails/VoluntaryWork/Editor.cshtml",
                new VoluntaryWorkModel());
        }

        public ActionResult TrainingProgramsEditor()
        {
            return PartialView("~/Areas/HRIS/Views/Employee/Include/EmployeeDetails/TrainingPrograms/Editor.cshtml",
                new TrainingProgramModel());
        }

        public ActionResult OtherInformationEditor()
        {
            return PartialView("~/Areas/HRIS/Views/Employee/Include/EmployeeDetails/OtherInformation/Editor.cshtml",
                new OtherInformationModel());
        }

        public ActionResult SpecialSkillsHobbyEditor()
        {
            return PartialView(
                "~/Areas/HRIS/Views/Employee/Include/EmployeeDetails/OtherInformation/SpecialSkillsHobbies/Editor.cshtml",
                new SpecialSkillsHobbyModel());
        }

        public ActionResult NonAcademicDistinctionEditor()
        {
            return PartialView(
                "~/Areas/HRIS/Views/Employee/Include/EmployeeDetails/OtherInformation/NonAcademicDistinction/Editor.cshtml",
                new NonAcademicDistinctionModel());
        }

        public ActionResult MemInAssociationOrgEditor()
        {
            return PartialView(
                "~/Areas/HRIS/Views/Employee/Include/EmployeeDetails/OtherInformation/MemInAssociationOrg/Editor.cshtml",
                new MembershipInAssociationOrganizationModel());
        }

        public ActionResult ReferenceEditor()
        {
            return PartialView(
                "~/Areas/HRIS/Views/Employee/Include/EmployeeDetails/OtherInformation/References/Editor.cshtml",
                new ReferenceModel());
        }

        public ActionResult CurrentMedicationEditor()
        {
            var model = new CurrentMedicationModel {CreatedFromAjax = true};
            return PartialView("~/Areas/HRIS/Views/Employee/Include/MedicalDetails/CurrentMedication/Editor.cshtml",
                model);
        }

        public ActionResult HealthCareProviderEditor()
        {
            var model = new HealthCareProviderModel {CreatedFromAjax = true};
            return PartialView(
                "~/Areas/HRIS/Views/Employee/Include/MedicalDetails/PrevHealthCareProvider/Editor.cshtml", model);
        }

        public ActionResult AllergicReactionEditor()
        {
            var model = new AllergicReactionModel {CreatedFromAjax = true};
            return PartialView("~/Areas/HRIS/Views/Employee/Include/MedicalDetails/AllergicReaction/Editor.cshtml",
                model);
        }

        public ActionResult HealthRecordEditor()
        {
            var model = new HealthRecordModel {CreatedFromAjax = true};
            return PartialView("~/Areas/HRIS/Views/Employee/Include/MedicalDetails/HealthRecord/Editor.cshtml", model);
        }

        public ActionResult PastSurgicalHistoryEditor()
        {
            var model = new PastSurgicalHistoryModel {CreatedFromAjax = true};
            return PartialView("~/Areas/HRIS/Views/Employee/Include/MedicalDetails/PastSurgicalHistory/Editor.cshtml",
                model);
        }

        public ActionResult DiagnosisEditor()
        {
            var model = new PhysicalExamDiagnosisModel {CreatedFromAjax = true};
            return PartialView("~/Areas/HRIS/Views/Employee/Include/MedicalDetails/Diagnosis/Editor.cshtml", model);
        }

        public ActionResult SpecifyDesignationEditor()
        {
            return PartialView(
                "~/Areas/HRIS/Views/Employee/Include/EmployeeDetails/PersonalInformation/SpecifyDesignation/Editor.cshtml",
                new SpecifyDesignationModel());
        }
        #endregion
    }
}