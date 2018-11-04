using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using AutoMapper;
using EBFP.BL.CIS;
using EBFP.BL.Helper;
using EBFP.DataAccess;
using EBFP.Helper;
using Queries.Core.Repositories;
using System.Reflection;
using System.Linq.Expressions;
using GemBox.Spreadsheet;

namespace EBFP.BL.HumanResources
{
    public class EmployeeBL : Repository<tblEmployees, EmployeeModel>, IEmployee
    {
        public EmployeeBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public List<EmployeeModel> GetEmployeeByUnitId(int unitId)
        { 
            var retEmp = new List<EmployeeModel>();
            var employeeList =
                BFPContext.tblEmployees.Where(a => a.Emp_Curr_Unit == unitId && a.Emp_Username != "portaladmin" && a.Emp_IsDeleted == false 
                && !string.IsNullOrEmpty(a.Emp_Number) )
                .Select(a=> new 
                {
                    a.Emp_Id,
                    a.Emp_Number,
                    a.Emp_FirstName,
                    a.Emp_MiddleName,
                    a.Emp_LastName,
                    a.Emp_SuffixName,
                    a.Emp_Curr_Unit,
                    a.Emp_Curr_Rank,
                    a.Emp_Username,
                    a.Emp_DutyStatus
                }).ToList();

            foreach (var item in employeeList)
            {
                var model = new EmployeeModel();
                model.Emp_Id = item.Emp_Id;
                model.Emp_Number = item.Emp_Number;
                model.Emp_FirstName = item.Emp_FirstName;
                model.Emp_MiddleName = item.Emp_MiddleName;
                model.Emp_LastName = item.Emp_LastName;
                model.Emp_SuffixName = item.Emp_SuffixName;
                model.Emp_Curr_Unit = item.Emp_Curr_Unit;
                model.Emp_Curr_Rank = item.Emp_Curr_Rank;
                model.Emp_Username = !string.IsNullOrEmpty(item.Emp_Username)  ? item.Emp_Username.Encrypt() : ("").Encrypt();
                model.Emp_DutyStatus = item.Emp_DutyStatus;
                retEmp.Add(model);
            }
            //retEmp = employeeList.Project().To<EmployeeModel>().ToList();
            return retEmp;
        }

        public List<EmployeeListModel> GetEmployeeList()
        {
            var retEmp = new List<EmployeeListModel>();
            var employeeList = BFPContext.tblEmployees.OrderBy(a => a.Emp_FirstName).ThenBy(a => a.Emp_LastName);

            foreach (var item in employeeList)
            {
                var employeeModel = new EmployeeListModel();
                employeeModel.Emp_Curr_Unit_Fullname = item.Emp_FirstName + " " + item.Emp_MiddleName + " " +
                                                       item.Emp_LastName + " " + item.Emp_SuffixName;
                employeeModel.Emp_Curr_Unit_Fullname = employeeModel.Emp_Curr_Unit_Fullname.Trim().FirstCharToUpper();
                employeeModel.Emp_Id = item.Emp_Id.ToString();

                retEmp.Add(employeeModel);
            }
            return retEmp.OrderBy(a => a.Emp_Curr_Unit_Fullname).ToList();
        }

        public List<EmployeeListModel> GetRetiredEmployeeList()
        {
            var retEmp = new List<EmployeeListModel>();
            var employeeList =
                BFPContext.tblEmployees.Where(a => a.Emp_DutyStatus == (int) DutyStatuses.Retired)
                    .OrderBy(a => a.Emp_FirstName)
                    .ThenBy(a => a.Emp_LastName);

            foreach (var item in employeeList)
            {
                var employeeModel = new EmployeeListModel();
                employeeModel.Emp_Curr_Unit_Fullname = item.Emp_FirstName + " " + item.Emp_MiddleName + " " +
                                                       item.Emp_LastName;
                employeeModel.Emp_Curr_Unit_Fullname = employeeModel.Emp_Curr_Unit_Fullname.FirstCharToUpper();
                employeeModel.Emp_Id = item.Emp_Id.ToString();

                retEmp.Add(employeeModel);
            }
            return retEmp.OrderBy(a => a.Emp_Curr_Unit_Fullname).ToList();
        }

        public EmployeeModel GetEmployeeByUserName(string userName)
        {
            var retEmp = new EmployeeModel();
            var employee = BFPContext.tblEmployees.FirstOrDefault(a => a.Emp_Username == userName && a.Emp_IsDeleted != true);

            if (employee == null) return null;

            retEmp = Mapper.Map<tblEmployees, EmployeeModel>(employee);

            //retEmp.RoleID = employee.Emp_AccessRole ?? 0;
            retEmp.RegionID = employee.tblUnits?.tblCityMunicipality?.tblProvinces?.Region_Id ?? 0;
            retEmp.ProvinceID = employee.tblUnits?.tblCityMunicipality?.Municipality_Province_Id ?? 0;
            retEmp.MunicipalityID = employee.tblUnits?.Unit_Municipality_Id ?? 0;
            retEmp.Emp_Curr_Unit = employee.Emp_Curr_Unit ?? 0;
            retEmp.Emp_Rank_Txt = employee.tblRanks?.Rank_Name;


            var userInRole = BFPContext.tblUserInRole.FirstOrDefault(a => a.UIR_EmployeeID == employee.Emp_Id);
            if (userInRole != null)
            {
                var roles =
                    BFPContext.tblUserRoles.Where(
                        a => a.Role_ID == userInRole.UIR_RoleID || a.Role_DefaultAccess).ToList();
                var defaultRole = roles.FirstOrDefault(a => a.Role_DefaultAccess);
                var empRole = roles.FirstOrDefault(a => a.Role_ID == userInRole.UIR_RoleID);

                if (empRole != null)
                {
                    retEmp.RoleID = empRole.Role_ID;
                    retEmp.RoleName = empRole.Role_Name;
                }
                else if (defaultRole != null)
                {
                    retEmp.RoleID = defaultRole.Role_ID;
                    retEmp.RoleName = defaultRole.Role_Name;
                }
            }
            else //Default Access
            {
                var defaultRole = BFPContext.tblUserRoles.FirstOrDefault(a => a.Role_DefaultAccess);
                if (defaultRole != null)
                {
                    retEmp.RoleID = defaultRole.Role_ID;
                    retEmp.RoleName = defaultRole.Role_Name;
                }
            }
            return retEmp;
        }

        public List<EmployeeModel> GetEmployeeByCurrUnit(int curr_Unit)
        {
            var retEmp = new List<EmployeeModel>();
            var employeeList = BFPContext.tblEmployees.Where(a => a.Emp_Curr_Unit == curr_Unit).Select(a => new
            {
                a.Emp_Id,
                a.Emp_FirstName,
                a.Emp_LastName,
                a.Emp_MiddleName,
                a.Emp_Curr_Unit,
                RankName = a.tblRanks != null ? a.tblRanks.Rank_Name : "",
                a.Emp_Curr_Rank
                //a.Emp_Curr_PosDesignationTitle
            }).ToList();

            foreach (var employee in employeeList)
            {
                retEmp.Add(new EmployeeModel
                {
                    Emp_FirstName = employee.Emp_FirstName,
                    Emp_LastName = employee.Emp_LastName,
                    Emp_Id = employee.Emp_Id,
                    Emp_MiddleName = employee.Emp_MiddleName,
                    Emp_Curr_Unit = employee.Emp_Curr_Unit,
                    Emp_Rank_Txt = employee.RankName,
                    Emp_Curr_Rank = employee.Emp_Curr_Rank,
                    //Emp_Curr_PosDesignationTitle = employee.Emp_Curr_PosDesignationTitle
                    Emp_Curr_PosDesignationTitle = GetPositionTitle(employee.Emp_Id)
                });
            }
            return
                retEmp.OrderBy(a => a.Emp_FirstName)
                    .ThenBy(a => a.Emp_LastName)
                    .OrderByDescending(a => a.Emp_Curr_Rank)
                    .ToList();
        }

        public bool DeleteByID(int employeeID)
        {
            var employee = BFPContext.tblEmployees
                .FirstOrDefault(a => a.Emp_Id == employeeID);

            employee.Emp_IsDeleted = true;
            employee.Emp_DutyStatus = 0;
            BFPContext.SaveChanges();

            return true;
        }

        public EmployeeListResult GetEmployees(GridInfo gridInfo)
        {
            var retEmps = new List<EmployeeListModel>();
            BFPContext.Database.Log = s => Debug.WriteLine(s);
            //TODO :add where clause if neccesary
            var SearchTerms = gridInfo.searchModel;
            var employees = BFPContext.tblEmployees.Where(a => a.Emp_IsDeleted == false);


            if (!PageSecurity.HasAccess(PageArea.HRIS_EmployeeRoster_CanViewAll))
            {
                if (PageSecurity.HasAccess(PageArea.HRIS_EmployeeRoster_RestricttoRegion))
                {
                    employees = employees.Where(
                        a => a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID);
                }

                if (PageSecurity.HasAccess(PageArea.HRIS_EmployeeRoster_RestricttoProvince))
                {
                    employees =
                        employees.Where(
                            a => a.tblUnits.tblCityMunicipality.Municipality_Province_Id == CurrentUser.ProvinceID);
                }

                if (PageSecurity.HasAccess(PageArea.HRIS_EmployeeRoster_RestricttoStation))
                {
                    employees = employees.Where(a => a.Emp_Curr_Unit == CurrentUser.EmployeeUnitId);
                }
            }

            if (!string.IsNullOrEmpty(SearchTerms.LastName))
                employees = employees.Where(a => a.Emp_LastName.Contains(SearchTerms.LastName));
            if (!string.IsNullOrEmpty(SearchTerms.AccountNumber))
                employees = employees.Where(a => a.Emp_Number.Contains(SearchTerms.AccountNumber));
            if (!string.IsNullOrEmpty(SearchTerms.BadgeNumber))
                employees = employees.Where(a => a.Emp_BadgeNumber.Contains(SearchTerms.BadgeNumber));
            if (!string.IsNullOrEmpty(SearchTerms.ItemNumber))
                employees = employees.Where(a => a.Emp_ItemNumber.Contains(SearchTerms.ItemNumber));
            if (!string.IsNullOrEmpty(SearchTerms.Gender))
                employees = employees.Where(a => a.Emp_Gender.Contains(SearchTerms.Gender));
            if (SearchTerms.StartServiceDate.HasValue)
                employees = employees.Where(a => a.Emp_Service_StartDate == SearchTerms.StartServiceDate);
            if (SearchTerms.LastTrainingDate.HasValue)
                employees = employees.Where(a => a.Emp_LastTrainingDate == SearchTerms.LastTrainingDate);
            if (SearchTerms.Birthdate.HasValue)
                employees = employees.Where(a => a.Emp_BirthDate == SearchTerms.Birthdate);
            if (SearchTerms.CivilStatus > 0)
                employees = employees.Where(a => a.Emp_CivilStatus == SearchTerms.CivilStatus);
            if (SearchTerms.DutyStatusId > 0)
                employees = employees.Where(a => a.Emp_DutyStatus == SearchTerms.DutyStatusId);
            if (SearchTerms.DesignationId > 0)
                employees = employees.Where(a => a.Emp_Curr_JobFunc == SearchTerms.DesignationId);
            if (SearchTerms.RankId > 0) employees = employees.Where(a => a.Emp_Curr_Rank == SearchTerms.RankId);
            if (SearchTerms.CourseId > 0)
                employees = employees.Where(a => a.Emp_EducCourse == SearchTerms.CourseId);
            if (SearchTerms.EligibilityId > 0)
                employees = employees.Where(a => a.Emp_Curr_Eligibility == SearchTerms.EligibilityId);
            if (SearchTerms.AppointmentStatusId > 0)
                employees = employees.Where(a => a.Emp_Curr_ApptStatus == SearchTerms.AppointmentStatusId);
            if (SearchTerms.HighestMandatoryTraining > 0)
                employees = employees.Where(a => a.Emp_HighestMandatoryTraining == SearchTerms.HighestMandatoryTraining);
            if (SearchTerms.RegionId > 0)
                employees =
                    employees.Where(
                        a => a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == SearchTerms.RegionId);
            if (SearchTerms.ProvinceId > 0)
                employees =
                    employees.Where(
                        a => a.tblUnits.tblCityMunicipality.Municipality_Province_Id == SearchTerms.ProvinceId);
            if (SearchTerms.MunicipalityId > 0)
                employees = employees.Where(a => a.tblUnits.Unit_Municipality_Id == SearchTerms.MunicipalityId);
            if (SearchTerms.UnitId > 0) employees = employees.Where(a => a.Emp_Curr_Unit == SearchTerms.UnitId);

            if (SearchTerms.isChecked > 0)
            {
                if(SearchTerms.isChecked == 1)
                    employees = employees.Where(a => a.Emp_IsChecked == true);
                else
                    employees = employees.Where(a => a.Emp_IsChecked != true);
            }
            //Last Training, End Service 
            gridInfo.recordsTotal = employees.Select(a => a.Emp_Id).Count();
            var employeesResult = employees.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
                .Skip(gridInfo.start)
                .Take(gridInfo.length)
                .ToList();

            foreach (var employee in employeesResult)
            {
                retEmps.Add(new EmployeeListModel
                {
                    Emp_FirstName = employee.Emp_FirstName,
                    Emp_LastName = employee.Emp_LastName,
                    Emp_Id = employee.Emp_Id.ToNullSafeString().Encrypt(),
                    Emp_Number = employee.Emp_Number,
                    Emp_Curr_Rank = employee.Emp_Curr_Rank.ToNullSafeString().ToRankFullName(),
                    Emp_Curr_Eligibility = employee.Emp_Curr_Eligibility.ToNullSafeString().ToEligibilityFullName(),
                    Emp_Curr_Unit_Region = employee.Emp_Curr_Unit.ToNullSafeString().ToRegionFullName(),
                    Emp_Curr_Unit_Fullname = employee.Emp_Curr_Unit.ToNullSafeString().ToUnitFullName()
                });
            }

            return new EmployeeListResult
            {
                EmployeeListModel = retEmps,
                DatatableInfo = gridInfo
            };
        }

        public EmployeeModel EmployeeDetails(int Emp_Id)
        {
            IHRISUnitOfWork UnitOfWork = new HRISUnitOfWork(BFPContext);
            var retEmp = new EmployeeModel();
            var employee = GetEmployeeByID(Emp_Id);
            
            retEmp = Mapper.Map<tblEmployees, EmployeeModel>(employee);

            var membership = BFPContext.webpages_Membership.FirstOrDefault(a => a.UserId == Emp_Id);
            if (membership != null)
                retEmp.user.OldPassword = membership.PasswordDecrypted;

            retEmp.Emp_Rank_Txt = employee.Emp_Curr_Rank == null || employee.Emp_Curr_Rank <= 0
                ? ""
                : employee.tblRanks.Rank_Name;

            retEmp.EmployeeChildren = UnitOfWork.EmployeeChildren.GetList(a => a.EC_Emp_Id == Emp_Id,
                q => q.OrderByDescending(d => d.EC_BirthDate));

            retEmp.EducationalBackgrounds = UnitOfWork.EducationalBackground.GetList(a => a.EEB_Emp_Id == Emp_Id,
                q => q.OrderBy(d => d.EEB_EducType).ThenBy(a => a.EEB_StartDate));

            retEmp.CivilServiceEligibilities = UnitOfWork.CivilServiceEligibility.GetList(a => a.ECSE_Emp_Id == Emp_Id,
                q => q.OrderBy(d => d.ECSE_ExamDate));

            retEmp.WorkExperiences = UnitOfWork.WorkExperience.GetList(a => a.EWE_Emp_Id == Emp_Id,
                q => q.OrderByDescending(d => d.EWE_StartDate));

            retEmp.VoluntaryWorks = UnitOfWork.VoluntaryWork.GetList(a => a.EVW_Emp_Id == Emp_Id,
                q => q.OrderByDescending(d => d.EVW_EndDate));

            retEmp.TrainingPrograms = UnitOfWork.TrainingProgram.GetList(a => a.ETP_Emp_Id == Emp_Id,
                q => q.OrderByDescending(d => d.ETP_EndDate));

            retEmp.References = UnitOfWork.Reference.GetList(a => a.ER_Emp_Id == Emp_Id,
                q => q.OrderBy(d => d.ER_FullName));

            retEmp.SpecialSkillsHobbies = UnitOfWork.SpecialSkillsHobby.GetList(a => a.ESSH_Emp_Id == Emp_Id,
                q => q.OrderBy(d => d.ESSH_Title));

            retEmp.NonAcademicDistinctions = UnitOfWork.NonAcademicDistinction.GetList(a => a.ENAD_Emp_Id == Emp_Id,
                q => q.OrderBy(d => d.ENAD_Title));

            retEmp.MembershipInAssociationOrganizations =
                UnitOfWork.MembershipInAssociationOrganization.GetList(a => a.EMIAO_Emp_Id == Emp_Id,
                    q => q.OrderBy(d => d.EMIAO_Title));

            retEmp.ServiceAppointment = UnitOfWork.ServiceRecord.GetList(a => a.ESA_Emp_Id == Emp_Id,
                q => q.OrderByDescending(d => d.ESA_ApptDate));

            retEmp.SpecifyDesignation = UnitOfWork.SpecifyDesignation.GetList(a => a.SpecifyDesig_Emp_Id == Emp_Id,
               q => q.OrderBy(d => d.SpecifyDesig_Title));
            
            retEmp.OtherInformation = UnitOfWork.OtherInformation.GetByEmployee(Emp_Id);

            return retEmp;
        }

        public EmployeeModel GetEmployeeById(int Emp_Id)
        {
            IHRISUnitOfWork UnitOfWork = new HRISUnitOfWork(BFPContext);
            var retEmp = new EmployeeModel();
            var employee = GetEmployeeByID(Emp_Id);
            retEmp = Mapper.Map<tblEmployees, EmployeeModel>(employee);

            return retEmp;
        }

        public EmployeeModel SaveEmployeeDetails(EmployeeModel model, bool editProfile)
        {
            try
            {
                IHRISUnitOfWork UnitOfWork = new HRISUnitOfWork(BFPContext);
                var newEmployee = new tblEmployees();
                var dutyStatus = 0;
                int? oldUnitId = null;
                if (model.Emp_Id > 0)
                {
                    var details = BFPContext.tblEmployees
                        .FirstOrDefault(a => a.Emp_Id == model.Emp_Id);

                    if (details != null)
                    {
                        oldUnitId = details.Emp_Curr_Unit;
                    }

                    if (!string.Equals(details.Emp_Username, model.Emp_Username, StringComparison.OrdinalIgnoreCase))
                        IsValidUsername(model.Emp_Username, model.Emp_Id);


                    //TODO: Remove this after testing
                    //if (!PageSecurity.HasAccess(PageArea.HRIS_EmployeeRoster_BFPInformation_Modify) || editProfile)
                    //{
                    //    UpdatePersonalInformation(model);
                    //}
                    //else
                    //{
                    var isEmpty = model.Emp_Photo?.All(B => B == default(byte)) ?? true;
                    if (isEmpty)
                        model.Emp_Photo = details.Emp_Photo;

                    //TODO: Remove this after testing
                    //if (CurrentUser.RoleName != "REGIONAL ADMIN" && CurrentUser.RoleName != "MAIN ADMIN")
                    //    model.Emp_DutyStatus = details.Emp_DutyStatus;
                    dutyStatus = details.Emp_DutyStatus ?? 0;

                    model.Emp_IsChecked = details.Emp_IsChecked;
                    model.Emp_CheckedBy = details.Emp_CheckedBy;
                    model.Emp_CheckedDate = details.Emp_CheckedDate;
                    Mapper.Map(model, details);

                    if (model.Emp_Curr_Unit != oldUnitId && details.Emp_SubStation_Id != null)
                    {
                        details.Emp_SubStation_Id = null;
                    }

                    BFPContext.SaveChanges();

                    //Insert Logs
                    if ((CurrentUser.RoleName == "REGIONAL ADMIN" || CurrentUser.RoleName == "MAIN ADMIN") &&
                        dutyStatus != model.Emp_DutyStatus)
                    {
                        var oldDutyStatus = dutyStatus > 0 ? ((DutyStatuses) dutyStatus).ToDescription() : "None";
                        var newDutyStatus = model.Emp_DutyStatus > 0 && model.Emp_DutyStatus != null
                            ? ((DutyStatuses) model.Emp_DutyStatus).ToDescription()
                            : "None";
                        var logsModel = new LogsModel
                        {
                            Log_Emp_Id = model.Emp_Id,
                            Log_Remarks = "Duty Status From " + oldDutyStatus + " to " + newDutyStatus
                        };
                        UnitOfWork.Logs.InsertLogs(logsModel);
                    }
                    if (model.Emp_Curr_Unit != oldUnitId)
                    {
                        var unit = new UnitModel();
                        var oldUnit = "None";
                        var newUnit = "None";
                        if (oldUnitId > 0)
                        {
                            unit = UnitOfWork.Unit.GetUnitById(oldUnitId.Value);
                            if (unit != null)
                                oldUnit = string.Concat("[", unit.Unit_Code, "] ", unit.Unit_StationName, " , ",
                                    unit.Province_Name);
                        }

                        var newUnitDet = new UnitModel();
                        if (model.Emp_Curr_Unit > 0 && model.Emp_Curr_Unit != null)
                        {
                            newUnitDet = UnitOfWork.Unit.GetUnitById(model.Emp_Curr_Unit.Value);
                            if (newUnitDet != null)
                                newUnit = string.Concat("[", newUnitDet.Unit_Code, "] ", newUnitDet.Unit_StationName, " , ", newUnitDet.Province_Name);
                        }
                          
                        var logsModel = new LogsModel
                        {
                            Log_Emp_Id = model.Emp_Id,
                            Log_Remarks = "Present Assignment From " + oldUnit + " to " + newUnit
                        };
                        UnitOfWork.Logs.InsertLogs(logsModel);
                    }

                    //}
                }
                else
                {
                    IsValidUsername(model.Emp_Username);

                    var employee = Mapper.Map(model, newEmployee);

                    ////TODO: Remove this after testing
                    //  if (CurrentUser.RoleName != "REGIONAL ADMIN" && CurrentUser.RoleName != "MAIN ADMIN")
                    //       newEmployee.Emp_DutyStatus =null;

                    BFPContext.tblEmployees.Add(employee);
                    BFPContext.SaveChanges();

                    //Insert Logs
                    if (CurrentUser.RoleName == "REGIONAL ADMIN" || CurrentUser.RoleName == "MAIN ADMIN")
                    {
                        var logsModel = new LogsModel
                        {
                            Log_Emp_Id = newEmployee.Emp_Id,
                            Log_Remarks =
                                "Duty Status - Newly created employee with duty status " +
                                ((DutyStatuses) newEmployee.Emp_DutyStatus).ToDescription()
                        };
                        UnitOfWork.Logs.InsertLogs(logsModel);
                    }
                }

                model.Emp_Id = model.Emp_Id > 0 ? model.Emp_Id : newEmployee.Emp_Id;

                UnitOfWork.EmployeeChildren.InsertBulk(model.EmployeeChildren, model.Emp_Id);
                UnitOfWork.EducationalBackground.InsertBulk(model.EducationalBackgrounds, model.Emp_Id);
                UnitOfWork.CivilServiceEligibility.InsertBulk(model.CivilServiceEligibilities, model.Emp_Id);
                UnitOfWork.WorkExperience.InsertBulk(model.WorkExperiences, model.Emp_Id);
                UnitOfWork.VoluntaryWork.InsertBulk(model.VoluntaryWorks, model.Emp_Id);
                UnitOfWork.TrainingProgram.InsertBulk(model.TrainingPrograms, model.Emp_Id);
                UnitOfWork.Reference.InsertBulk(model.References, model.Emp_Id);
                UnitOfWork.SpecialSkillsHobby.InsertBulk(model.SpecialSkillsHobbies, model.Emp_Id);
                UnitOfWork.NonAcademicDistinction.InsertBulk(model.NonAcademicDistinctions, model.Emp_Id);
                UnitOfWork.MembershipInAssociationOrganization.InsertBulk(model.MembershipInAssociationOrganizations,
                    model.Emp_Id);
                model.OtherInformation.EOI_Emp_Id = model.Emp_Id;
                UnitOfWork.OtherInformation.Add(model.OtherInformation, a => a.EOI_Emp_Id == model.Emp_Id);
                UnitOfWork.SpecifyDesignation.InsertBulk(model.SpecifyDesignation, model.Emp_Id);
                //if (PageSecurity.HasAccess(PageArea.HRIS_EmployeeRoster_BFPInformation_Modify) && !editProfile)
                //{
                UnitOfWork.ServiceRecord.InsertBulk(model.ServiceAppointment, model.Emp_Id);
                //}
                UnitOfWork.Complete();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            return model;
        }

        public tblEmployees GetEmployeeByID(int Emp_Id)
        {
            var employee = BFPContext.tblEmployees.Find(Emp_Id);
            return employee;
        }

        public string GetEmployeeNameById(int empId)
        {
            var employeeName = (
                   from emp in BFPContext.tblEmployees
                   where emp.Emp_Id == empId
                   select new 
                   {
                       FullName = emp.tblRanks.Rank_Name + " " + emp.Emp_FirstName +" " + emp.Emp_MiddleName + " " + emp.Emp_LastName + " " +emp.Emp_SuffixName
                   }).FirstOrDefault();

            return employeeName.FullName;
        }
        public void UpdatePersonalInformation(EmployeeModel model)
        {
            var details = BFPContext.tblEmployees
                .FirstOrDefault(a => a.Emp_Id == model.Emp_Id);

            if (details == null) throw new Exception("Employee cannot be found!");

            if (!string.Equals(details.Emp_Username, model.Emp_Username, StringComparison.OrdinalIgnoreCase))
            {
                IsValidUsername(model.Emp_Username, model.Emp_Id);
                details.Emp_Username = model.Emp_Username;
            }


            details.Emp_Number = model.Emp_Number;
            details.Emp_LastName = model.Emp_LastName;
            details.Emp_MiddleName = model.Emp_MiddleName;
            details.Emp_FirstName = model.Emp_FirstName;
            details.Emp_SuffixName = model.Emp_SuffixName;
            details.Emp_BirthDate = model.Emp_BirthDate;
            details.Emp_BirthPlace = model.Emp_BirthPlace;
            details.Emp_Gender = model.Emp_Gender;
            details.Emp_CivilStatus = model.Emp_CivilStatus;
            details.Emp_Citizenship = model.Emp_Citizenship;
            details.Emp_Citizenship_Country = model.Emp_Citizenship_Country;
            details.Emp_Citizenship_Dual = model.Emp_Citizenship_Dual;
            details.Emp_Height = model.Emp_Height;
            details.Emp_Weight = model.Emp_Weight;
            details.Emp_BloodType = model.Emp_BloodType;
            details.Emp_GSISNumber = model.Emp_GSISNumber;
            details.Emp_PAGIBIGNumber = model.Emp_PAGIBIGNumber;
            details.Emp_PHICNumber = model.Emp_PHICNumber;
            details.Emp_SSSNumber = model.Emp_SSSNumber;
            //details.Emp_Residential_Address = model.Emp_Residential_Address;
            details.Emp_Residential_HouseNo = model.Emp_Residential_HouseNo;
            details.Emp_Residential_Street = model.Emp_Residential_Street;
            details.Emp_Residential_Village = model.Emp_Residential_Village;
            details.Emp_Residential_Barangay = model.Emp_Residential_Barangay;
            details.Emp_Residential_Municipality = model.Emp_Residential_Municipality;
            details.Emp_Residential_Province = model.Emp_Residential_Province;
            details.Emp_Residential_ZipCode = model.Emp_Residential_ZipCode;
            details.Emp_Residential_PhoneNumber = model.Emp_Residential_PhoneNumber;
            //details.Emp_Permanent_Address = model.Emp_Permanent_Address;
            details.Emp_Permanent_HouseNo = model.Emp_Permanent_HouseNo;
            details.Emp_Permanent_Street = model.Emp_Permanent_Street;
            details.Emp_Permanent_Village = model.Emp_Permanent_Village;
            details.Emp_Permanent_Barangay = model.Emp_Permanent_Barangay;
            details.Emp_Permanent_Municipality = model.Emp_Permanent_Municipality;
            details.Emp_Permanent_Province = model.Emp_Permanent_Province;
            details.Emp_Permanent_ZipCode = model.Emp_Permanent_ZipCode;
            details.Emp_Permanent_PhoneNumber = model.Emp_Permanent_PhoneNumber;
            details.Emp_EmailAddress = model.Emp_EmailAddress;
            details.Emp_MobileNumber = model.Emp_MobileNumber;
            details.Emp_AgencyEmpNumber = model.Emp_AgencyEmpNumber;
            details.Emp_TINNumber = model.Emp_TINNumber;

            var isEmpty = model.Emp_Photo?.All(B => B == default(byte)) ?? true;
            if (!isEmpty)
                details.Emp_Photo = model.Emp_Photo;

            BFPContext.Entry(details).State = EntityState.Modified;
            BFPContext.SaveChanges();
        }

        public List<EmployeeModel> EmployeeWithRankSelection(string search)
        {
            var ret =
                BFPContext.tblEmployees.Where(
                    a =>
                        a.Emp_FirstName.Contains(search) || a.Emp_LastName.Contains(search) ||
                        a.tblRanks.Rank_Name.Contains(search) || a.Emp_Number.Contains(search))
                    .Select(
                        a => new EmployeeModel
                        {
                            Emp_Rank_Txt = a.Emp_Curr_Rank != null || a.Emp_Curr_Rank > 0 ? a.tblRanks.Rank_Name : "",
                            Emp_FirstName = a.Emp_FirstName,
                            Emp_LastName = a.Emp_LastName,
                            Emp_Id = a.Emp_Id
                        }
                    ).ToList();

            return ret;
        }

        public byte[] GetImage(int empId)
        {
            var user = BFPContext.tblEmployees.FirstOrDefault(a => a.Emp_Id == empId);

            if (user != null)
            {
                return user.Emp_Photo;
            }

            return new byte[0];
        }

        public void UpdateByAlphaList(HttpPostedFileBase file, ref List<string> newUsers)
        {
            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

            if ((file == null) || (file.ContentLength <= 0) || string.IsNullOrEmpty(file.FileName))
                throw new Exception("No data on the file.");
            var path = $"{applicationPath}{@"\Content\MISC\Generated\AlphaList_"}{DateTime.Now.Ticks}{".xlsx"}";
            file.SaveAs(path);

            var dtExcelTable = FileHelper.ReadExcelFileForAlphaList(path);
            File.Delete(path);

            if (dtExcelTable.Rows.Count == 0)
                throw new Exception("No data on the file.");

            var columns = GetIncludedColumn(dtExcelTable);
            var accountNumbers = new List<string>();
            var unitCodes = new List<string>();
            var courses = new List<string>();

            foreach (DataRow row in dtExcelTable.Rows)
            {
                var acctNumber = row["Account Number"].ToString();
                if (string.IsNullOrWhiteSpace(acctNumber))
                    continue; //skip

                accountNumbers.Add(acctNumber);
                if (columns.HasEmp_Curr_Unit)
                {
                    if (!string.IsNullOrEmpty(row["PRESENT ASSIGNMENT (Unit Code)"].ToString()))
                        unitCodes.Add(row["PRESENT ASSIGNMENT (Unit Code)"].ToString());
                }
               
                if (columns.HasEmp_MACourse)
                {
                    if (!string.IsNullOrEmpty(row["GRADUATE STUDIES"].ToString()))
                        courses.Add(row["GRADUATE STUDIES"].ToString());
                }
                if (columns.HasEmp_EducCourse)
                {
                    if (!string.IsNullOrEmpty(row["BASE COURSE"].ToString()))
                        courses.Add(row["BASE COURSE"].ToString());
                }
            }

            var units = BFPContext.tblUnits.Where(a => unitCodes.Contains(a.Unit_Code)).ToList();

            var religions = new List<tblReligions>();
            if (columns.HasEmp_Religion)
                religions = BFPContext.tblReligions.ToList();

            var ranks = new List<tblRanks>();
            if (columns.HasEmp_Curr_Rank)
                ranks = BFPContext.tblRanks.ToList();

            var dutyStatus = new List<tblDutyStatus>();
            if (columns.HasEmp_DutyStatus)
                dutyStatus = BFPContext.tblDutyStatus.ToList();

            var salaryGrades = new List<tblSalaryGrades>();
            if (columns.HasEmp_Curr_SalaryGrade)
                salaryGrades = BFPContext.tblSalaryGrades.ToList();

            var jobFunctions = new List<tblJobFunctions>();
            if (columns.HasEmp_Curr_JobFunc)
                jobFunctions = BFPContext.tblJobFunctions.ToList();

            var courseList = new List<tblCourses>();
            if (columns.HasEmp_EducCourse || columns.HasEmp_MACourse)
                courseList = BFPContext.tblCourses.Where(a => courses.Contains(a.Course_Name)).ToList();

            var eligibilities = new List<tblEligibilities>();
            if (columns.HasEmp_Curr_Eligibility)
                eligibilities = BFPContext.tblEligibilities.ToList();

            var mandatoryTrainings = new List<tblMandatoryTrainings>();
            if (columns.HasEmp_HighestMandatoryTraining)
                mandatoryTrainings = BFPContext.tblMandatoryTrainings.ToList();

            var liEmployees = BFPContext.tblEmployees
                .Where(a => accountNumbers.Contains(a.Emp_Number))
                .ToList();

            var LastEmployeeID = BFPContext.tblEmployees.Select(a => a.Emp_Id).ToList().LastOrDefault();

            if (LastEmployeeID == 0)
                LastEmployeeID = 1;
            else
                LastEmployeeID = LastEmployeeID + 1;

            var newEmployees = new List<tblEmployees>();

            foreach (DataRow row in dtExcelTable.Rows)
            {
                var emp = MapEmployee(row, columns, ranks, units, dutyStatus, salaryGrades,
                    jobFunctions, courseList, eligibilities, mandatoryTrainings, religions);

                if (emp == null || string.IsNullOrWhiteSpace(row["Account Number"].ToString()))
                    continue; //skip

                var employee = liEmployees.FirstOrDefault(a => a.Emp_Number == emp.Emp_Number);

                if (employee == null)
                {
                    employee = new tblEmployees();
                    Mapper.Map(emp, employee);
                    //employee.Emp_Gender = null;
                    employee.Emp_Citizenship = null;
                    employee.Emp_CivilStatus = null;
                    employee.Emp_Username = emp.Emp_Number;

                    newUsers.Add(emp.Emp_Number);
                    LastEmployeeID++;
                    employee.Emp_Id = LastEmployeeID;
                    newEmployees.Add(employee);
                }
                else
                {
                    employee.Emp_IsDeleted = false;
                    employee.Emp_Number = emp.Emp_Number;
                    if (columns.HasEmp_FirstName)
                        employee.Emp_FirstName = emp.Emp_FirstName;
                    if (columns.HasEmp_MiddleName)
                        employee.Emp_MiddleName = emp.Emp_MiddleName;
                    if (columns.HasEmp_LastName)
                        employee.Emp_LastName = emp.Emp_LastName;
                    if (columns.HasEmp_SuffixName)
                        employee.Emp_SuffixName = emp.Emp_SuffixName;
                    if (columns.HasEmp_BP_Number)
                        employee.Emp_BP_Number = emp.Emp_BP_Number;
                    if (columns.HasEmp_Tax_Code)
                        employee.Emp_Tax_Code = emp.Emp_Tax_Code;
                    if (columns.HasEmp_Atm_Number)
                        employee.Emp_Atm_Number = emp.Emp_Atm_Number;
                    if (columns.HasEmp_DES)
                        employee.Emp_DES = emp.Emp_DES;
                    if (columns.HasEmp_BirthDate)
                        employee.Emp_BirthDate = emp.Emp_BirthDate;
                    if (columns.HasEmp_BirthPlace)
                        employee.Emp_BirthPlace = emp.Emp_BirthPlace;
                    if (columns.HasEmp_Gender)
                        employee.Emp_Gender = emp.Emp_Gender;
                    if (columns.HasEmp_Height)
                        employee.Emp_Height = emp.Emp_Height;
                    if (columns.HasEmp_Weight)
                        employee.Emp_Weight = emp.Emp_Weight;
                    if (columns.HasEmp_BloodType)
                        employee.Emp_BloodType = emp.Emp_BloodType;
                    if (columns.HasEmp_GSISNumber)
                        employee.Emp_GSISNumber = emp.Emp_GSISNumber;
                    if (columns.HasEmp_PAGIBIGNumber)
                        employee.Emp_PAGIBIGNumber = emp.Emp_PAGIBIGNumber;
                    if (columns.HasEmp_PHICNumber)
                        employee.Emp_PHICNumber = emp.Emp_PHICNumber;
                    if (columns.HasEmp_SSSNumber)
                        employee.Emp_SSSNumber = emp.Emp_SSSNumber;
                    if (columns.HasEmp_TINNumber)
                        employee.Emp_TINNumber = emp.Emp_TINNumber;
                    if (columns.HasEmp_Religion)
                        employee.Emp_Religion = emp.Emp_Religion;
                    if (columns.HasEmp_EmailAddress)
                        employee.Emp_EmailAddress = emp.Emp_EmailAddress;
                    if (columns.HasEmp_MobileNumber)
                        employee.Emp_MobileNumber = emp.Emp_MobileNumber;
                    if (columns.HasEmp_AgencyEmpNumber)
                        employee.Emp_AgencyEmpNumber = emp.Emp_AgencyEmpNumber;
                    if (columns.HasEmp_Residential_HouseNo)
                        employee.Emp_Residential_HouseNo = emp.Emp_Residential_HouseNo;
                    if (columns.HasEmp_Residential_Street)
                        employee.Emp_Residential_Street = emp.Emp_Residential_Street;
                    if (columns.HasEmp_Residential_Village)
                        employee.Emp_Residential_Village = emp.Emp_Residential_Village;
                    if (columns.HasEmp_Residential_Barangay)
                        employee.Emp_Residential_Barangay = emp.Emp_Residential_Barangay;
                    if (columns.HasEmp_Residential_Municipality)
                        employee.Emp_Residential_Municipality = emp.Emp_Residential_Municipality;
                    if (columns.HasEmp_Residential_Province)
                        employee.Emp_Residential_Province = emp.Emp_Residential_Province;
                    if (columns.HasEmp_Residential_ZipCode)
                        employee.Emp_Residential_ZipCode = emp.Emp_Residential_ZipCode;
                    if (columns.HasEmp_Residential_PhoneNumber)
                        employee.Emp_Residential_PhoneNumber = emp.Emp_Residential_PhoneNumber;
                    if (columns.HasEmp_Permanent_HouseNo)
                        employee.Emp_Permanent_HouseNo = emp.Emp_Permanent_HouseNo;
                    if (columns.HasEmp_Permanent_Street)
                        employee.Emp_Permanent_Street = emp.Emp_Permanent_Street;
                    if (columns.HasEmp_Permanent_Village)
                        employee.Emp_Permanent_Village = emp.Emp_Permanent_Village;
                    if (columns.HasEmp_Permanent_Barangay)
                        employee.Emp_Permanent_Barangay = emp.Emp_Permanent_Barangay;
                    if (columns.HasEmp_Permanent_Municipality)
                        employee.Emp_Permanent_Municipality = emp.Emp_Permanent_Municipality;
                    if (columns.HasEmp_Permanent_Province)
                        employee.Emp_Permanent_Province = emp.Emp_Permanent_Province;
                    if (columns.HasEmp_Permanent_ZipCode)
                        employee.Emp_Permanent_ZipCode = emp.Emp_Permanent_ZipCode;
                    if (columns.HasEmp_Permanent_PhoneNumber)
                        employee.Emp_Permanent_PhoneNumber = emp.Emp_Permanent_PhoneNumber;
                    //BFP Information
                    if (columns.HasEmp_Service_GovtStartDate)
                        employee.Emp_Service_GovtStartDate = emp.Emp_Service_GovtStartDate;
                    if (columns.HasEmp_Service_UniformGovtStartDate)
                        employee.Emp_Service_UniformGovtStartDate = emp.Emp_Service_UniformGovtStartDate;
                    if (columns.HasEmp_Service_StartDate)
                        employee.Emp_Service_StartDate = emp.Emp_Service_StartDate;
                    if (columns.HasEmp_LastPromotionDate_Temp)
                        employee.Emp_LastPromotionDate_Temp = emp.Emp_LastPromotionDate_Temp;
                    if (columns.HasEmp_LastPromotionDate_Permanent)
                        employee.Emp_LastPromotionDate_Permanent = emp.Emp_LastPromotionDate_Permanent;
                    if (columns.HasEmp_AssumedOfficerDate)
                        employee.Emp_AssumedOfficerDate = emp.Emp_AssumedOfficerDate;
                    if (columns.HasEmp_LastTrainingDate)
                        employee.Emp_LastTrainingDate = emp.Emp_LastTrainingDate;
                    if (columns.HasEmp_ItemNumber)
                        employee.Emp_ItemNumber = emp.Emp_ItemNumber;
                    if (columns.HasEmp_BadgeNumber)
                        employee.Emp_BadgeNumber = emp.Emp_BadgeNumber;
                    if (columns.HasEmp_Curr_Rank)
                        employee.Emp_Curr_Rank = emp.Emp_Curr_Rank;
                    if (columns.HasEmp_PresentAsgmt_DO_BO_RO)
                        employee.Emp_PresentAsgmt_DO_BO_RO = emp.Emp_PresentAsgmt_DO_BO_RO;
                    if (columns.HasEmp_Curr_ApptStatus)
                        employee.Emp_Curr_ApptStatus = emp.Emp_Curr_ApptStatus;
                    if (columns.HasEmp_AppointmentStatus_DO_BO_RO)
                        employee.Emp_AppointmentStatus_DO_BO_RO = emp.Emp_AppointmentStatus_DO_BO_RO;
                    if (columns.HasEmp_DutyStatus)
                        employee.Emp_DutyStatus = emp.Emp_DutyStatus;
                    if (columns.HasEmp_Curr_SalaryGrade)
                        employee.Emp_Curr_SalaryGrade = emp.Emp_Curr_SalaryGrade;
                    if (columns.HasEmp_Curr_JobFunc)
                        employee.Emp_Curr_JobFunc = emp.Emp_Curr_JobFunc;
                    //if (columns.HasEmp_Curr_PosDesignationTitle)
                    //    employee.Emp_Curr_PosDesignationTitle = emp.Emp_Curr_PosDesignationTitle;
                    if (columns.HasEmp_Curr_Unit)
                        employee.Emp_Curr_Unit = emp.Emp_Curr_Unit;
                    if (columns.HasEmp_Remarks)
                        employee.Emp_Remarks = emp.Emp_Remarks;
                    if (columns.HasEmp_EducCourse)
                        employee.Emp_EducCourse = emp.Emp_EducCourse;
                    if (columns.HasEmp_MACourse)
                        employee.Emp_MACourse = emp.Emp_MACourse;
                    if (columns.HasEmp_HighestEducAttainment)
                        employee.Emp_HighestEducAttainment = emp.Emp_HighestEducAttainment;
                    if (columns.HasEmp_Eligibility_Type)
                        employee.Emp_Eligibility_Type = emp.Emp_Eligibility_Type;
                    if (columns.HasEmp_Curr_Eligibility)
                        employee.Emp_Curr_Eligibility = emp.Emp_Curr_Eligibility;
                    if (columns.HasEmp_HighestMandatoryTraining)
                        employee.Emp_HighestMandatoryTraining = emp.Emp_HighestMandatoryTraining;
                }
            }

            if (newEmployees.Count > 0)
            {
                BFPContext.Configuration.AutoDetectChangesEnabled = false;
                BFPContext.tblEmployees.AddRange(newEmployees);
            }
            BFPContext.SaveChanges();
        }

        public List<EmployeeModel> GetEmployees()
        {
            var retEmp = new List<EmployeeModel>();

            var employee = BFPContext.tblEmployees.AsQueryable();

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion) ||
                PageSecurity.HasAccess(PageArea.HRIS_EmployeeRoster_RestricttoRegion))
            {
                employee =
                    employee.Where(
                        a =>
                            a.tblUnits != null &&
                            a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID);
            }
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince) ||
                PageSecurity.HasAccess(PageArea.HRIS_EmployeeRoster_RestricttoProvince))
            {
                employee =
                    employee.Where(
                        a => a.tblUnits != null &&
                             a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == CurrentUser.ProvinceID);
            }
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation) ||
                PageSecurity.HasAccess(PageArea.HRIS_EmployeeRoster_RestricttoStation))
            {
                employee = employee.Where(a => a.Emp_Curr_Unit == CurrentUser.EmployeeUnitId);
            }

            var liemployee = employee.Select(item => new
            {
                item.Emp_Curr_Rank,
                item.Emp_LastName,
                item.Emp_FirstName,
                item.Emp_MiddleName,
                item.Emp_SuffixName,
                item.Emp_Curr_Unit,
                item.Emp_DES,
                item.Emp_BirthDate,
                item.Emp_TINNumber,
                item.Emp_BP_Number,
                item.Emp_PHICNumber,
                item.Emp_PAGIBIGNumber,
                item.Emp_Tax_Code,
                item.Emp_Atm_Number,
                item.Emp_Number,
                item.Emp_DutyStatus
            }).Where(a => a.Emp_DutyStatus == (int) DutyStatuses.Active)
                .ToList();


            if (liemployee.Count <= 0) return retEmp;
            {
                foreach (var item in liemployee.OrderBy(a => a.Emp_FirstName))
                {
                    var employeeModel = new EmployeeModel
                    {
                        Emp_Curr_Rank = item.Emp_Curr_Rank,
                        Emp_LastName = item.Emp_LastName,
                        Emp_FirstName = item.Emp_FirstName,
                        Emp_MiddleName = item.Emp_MiddleName,
                        Emp_SuffixName = item.Emp_SuffixName,
                        Emp_Curr_Unit = item.Emp_Curr_Unit,
                        Emp_DES = item.Emp_DES,
                        Emp_BirthDate = item.Emp_BirthDate,
                        Emp_TINNumber = item.Emp_TINNumber,
                        Emp_BP_Number = item.Emp_BP_Number,
                        Emp_PHICNumber = item.Emp_PHICNumber,
                        Emp_PAGIBIGNumber = item.Emp_PAGIBIGNumber,
                        Emp_Tax_Code = item.Emp_Tax_Code,
                        Emp_Atm_Number = item.Emp_Atm_Number,
                        Emp_Number = item.Emp_Number
                    };

                    retEmp.Add(employeeModel);
                }
            }

            return retEmp;
        }

        public PersonnelStrengthModel GetPersonnelStrenght()
        {
            var personnelStrenghtModel = new PersonnelStrengthModel();
            var officerRanks = Functions.OfficerRanks();
            var nonOfficerRanks = Functions.NonOfficerRanks();
            
            var personnelStrenght = new List<EmployeeInventoryDashModel>();

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
            {
                personnelStrenght = (
                    from emp in BFPContext.tblEmployees
                    where emp.Emp_DutyStatus == (int) DutyStatuses.Active &&
                          emp.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID
                          && emp.Emp_IsDeleted == false
                    select new EmployeeInventoryDashModel
                    {
                        tblRanks = emp.tblRanks,
                    }).ToList();
            }
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
            {
                personnelStrenght = (
                    from emp in BFPContext.tblEmployees
                    where emp.Emp_DutyStatus == (int) DutyStatuses.Active &&
                          emp.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == CurrentUser.ProvinceID
                          && emp.Emp_IsDeleted == false
                    select new EmployeeInventoryDashModel
                    {
                        tblRanks = emp.tblRanks,
                    }).ToList();
            }
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
            {
                personnelStrenght = (
                    from emp in BFPContext.tblEmployees
                    where emp.Emp_DutyStatus == (int) DutyStatuses.Active &&
                          emp.Emp_Curr_Unit == CurrentUser.EmployeeUnitId
                          && emp.Emp_IsDeleted == false
                    select new EmployeeInventoryDashModel
                    {
                        tblRanks = emp.tblRanks,
                    }).ToList();
            }
            else
            {
                if (!(PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation) ||
                      PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince) ||
                      PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion)))
                {
                    personnelStrenght = (
                        from emp in BFPContext.tblEmployees
                        where emp.Emp_DutyStatus == (int) DutyStatuses.Active
                              && emp.Emp_IsDeleted == false
                        select new EmployeeInventoryDashModel
                        {
                            tblRanks = emp.tblRanks,
                        }).ToList();
                }
            }

            personnelStrenghtModel.OfficerRanks =
                personnelStrenght.Count(a => a.tblRanks != null && officerRanks.Contains(a.tblRanks.Rank_Id));
            personnelStrenghtModel.NonOfficerRanks =
                personnelStrenght.Count(a => a.tblRanks != null && nonOfficerRanks.Contains(a.tblRanks.Rank_Id));
            personnelStrenghtModel.NonUniformedPersonnel =
                personnelStrenght.Count(a => a.tblRanks != null && a.tblRanks.Rank_Id == (int) Rank.NUP);

            return personnelStrenghtModel;
        }

        public AlphaColumnListModel GetIncludedColumn(DataTable dataTable)
        {
            var model = new AlphaColumnListModel();
          
            if (dataTable.Columns.Contains("First name"))
                model.HasEmp_FirstName = true;
            if (dataTable.Columns.Contains("Middle name"))
                model.HasEmp_MiddleName = true;
            if (dataTable.Columns.Contains("Last name"))
                model.HasEmp_LastName = true;
            if (dataTable.Columns.Contains("Suffix name"))
                model.HasEmp_SuffixName = true;

            if (dataTable.Columns.Contains("BP Number"))
                model.HasEmp_BP_Number = true;
            if (dataTable.Columns.Contains("Tax Code"))
                model.HasEmp_Tax_Code = true;
            if (dataTable.Columns.Contains("Atm Number"))
                model.HasEmp_Atm_Number = true;
            if (dataTable.Columns.Contains("DES"))
                model.HasEmp_DES = true;

            if (dataTable.Columns.Contains("Date of birth"))
                model.HasEmp_BirthDate = true;
            if (dataTable.Columns.Contains("Place of Birth"))
                model.HasEmp_BirthPlace = true;
            if (dataTable.Columns.Contains("Sex"))
                model.HasEmp_Gender = true;
            if (dataTable.Columns.Contains("Height"))
                model.HasEmp_Height = true;
            if (dataTable.Columns.Contains("Weight"))
                model.HasEmp_Weight = true;
            if (dataTable.Columns.Contains("Blood Type"))
                model.HasEmp_BloodType = true;
            if (dataTable.Columns.Contains("GSIS ID NO"))
                model.HasEmp_GSISNumber = true;
            if (dataTable.Columns.Contains("PAG-IBIG ID NO"))
                model.HasEmp_PAGIBIGNumber = true;
            if (dataTable.Columns.Contains("PHILHEALTH NO"))
                model.HasEmp_PHICNumber = true;
            if (dataTable.Columns.Contains("SSS NO"))
                model.HasEmp_SSSNumber = true;
            if (dataTable.Columns.Contains("TIN"))
                model.HasEmp_TINNumber = true;
            if (dataTable.Columns.Contains("Religion"))
                model.HasEmp_Religion = true;
            if (dataTable.Columns.Contains("E-MAIL ADDRESS (if any)"))
                model.HasEmp_EmailAddress = true;
            if (dataTable.Columns.Contains("CELLPHONE NO"))
                model.HasEmp_MobileNumber = true;
            if (dataTable.Columns.Contains("AGENCY EMPLOYEE NO"))
                model.HasEmp_AgencyEmpNumber = true;
            if (dataTable.Columns.Contains("RESIDENTIAL House/Block/Lot No"))
                model.HasEmp_Residential_HouseNo = true;
            if (dataTable.Columns.Contains("RESIDENTIAL Street"))
                model.HasEmp_Residential_Street = true;
            if (dataTable.Columns.Contains("RESIDENTIAL Subdivision/Village"))
                model.HasEmp_Residential_Village = true;
            if (dataTable.Columns.Contains("RESIDENTIAL Barangay"))
                model.HasEmp_Residential_Barangay = true;
            if (dataTable.Columns.Contains("RESIDENTIAL City/Municipality"))
                model.HasEmp_Residential_Municipality = true;
            if (dataTable.Columns.Contains("RESIDENTIAL Province"))
                model.HasEmp_Residential_Province = true;
            if (dataTable.Columns.Contains("RESIDENTIAL Zip Code"))
                model.HasEmp_Residential_ZipCode = true;
            if (dataTable.Columns.Contains("RESIDENTIAL Telephone No"))
                model.HasEmp_Residential_PhoneNumber = true;
            if (dataTable.Columns.Contains("PERMANENT House/Block/Lot No."))
                model.HasEmp_Permanent_HouseNo = true;
            if (dataTable.Columns.Contains("PERMANENT Street"))
                model.HasEmp_Permanent_Street = true;
            if (dataTable.Columns.Contains("PERMANENT Subdivision/Village"))
                model.HasEmp_Permanent_Village = true;
            if (dataTable.Columns.Contains("PERMANENT Barangay"))
                model.HasEmp_Permanent_Barangay = true;
            if (dataTable.Columns.Contains("PERMANENT City/Municipality"))
                model.HasEmp_Permanent_Municipality = true;
            if (dataTable.Columns.Contains("PERMANENT PermanentProvince"))
                model.HasEmp_Permanent_Province = true;
            if (dataTable.Columns.Contains("PERMANENT Zip Code"))
                model.HasEmp_Permanent_ZipCode = true;
            if (dataTable.Columns.Contains("PERMANENT Telephone No"))
                model.HasEmp_Permanent_PhoneNumber = true;
            //BFP Information
            if (dataTable.Columns.Contains("DATE ENTERED GOVERNMENT SERVICE"))
                model.HasEmp_Service_GovtStartDate = true;
            if (dataTable.Columns.Contains("DATE ENTERED UNIFORMED SERVICE TO OTHER GOVERNMENT AGENCY/IES"))
                model.HasEmp_Service_UniformGovtStartDate = true;
            if (dataTable.Columns.Contains("DATE ENTERED UNIFORMED FIRE SERVICE"))
                model.HasEmp_Service_StartDate = true;
            if (dataTable.Columns.Contains("DATE OF LAST PROMOTION (TEMPORARY STATUS)"))
                model.HasEmp_LastPromotionDate_Temp = true;
            if (dataTable.Columns.Contains("DATE OF LAST PROMOTION (PERMANENT STATUS)"))
                model.HasEmp_LastPromotionDate_Permanent = true;
            if (dataTable.Columns.Contains("DATE ASSUMED OFFICER"))
                model.HasEmp_AssumedOfficerDate = true;
            if (dataTable.Columns.Contains("LAST TRAINING DATE"))
                model.HasEmp_LastTrainingDate = true;
            if (dataTable.Columns.Contains("ITEM NUMBER"))
                model.HasEmp_ItemNumber = true;
            if (dataTable.Columns.Contains("BADGE NUMBER"))
                model.HasEmp_BadgeNumber = true;
            if (dataTable.Columns.Contains("PRESENT RANK"))
                model.HasEmp_Curr_Rank = true;
            if (dataTable.Columns.Contains("PRESENT ASSIGNMENT DO/BO/RO"))
                model.HasEmp_PresentAsgmt_DO_BO_RO = true;
            if (dataTable.Columns.Contains("APPOINTMENT STATUS"))
                model.HasEmp_Curr_ApptStatus = true;
            if (dataTable.Columns.Contains("Appointment Status DO/BO/RO"))
                model.HasEmp_AppointmentStatus_DO_BO_RO = true;
            if (dataTable.Columns.Contains("DUTY STATUS"))
                model.HasEmp_DutyStatus = true;
            if (dataTable.Columns.Contains("SALARY GRADE"))
                model.HasEmp_Curr_SalaryGrade = true;
            if (dataTable.Columns.Contains("PRESENT DESIGNATION (Code)"))
                model.HasEmp_Curr_JobFunc = true;

            if (dataTable.Columns.Contains("PRESENT ASSIGNMENT (Unit Code)"))
                model.HasEmp_Curr_Unit = true;
            //if (dataTable.Columns.Contains("SPECIFY DESIGNATION"))
            //    model.HasEmp_Curr_PosDesignationTitle = true;
            if (dataTable.Columns.Contains("REMARKS"))
                model.HasEmp_Remarks = true;
            if (dataTable.Columns.Contains("BASE COURSE"))
                model.HasEmp_EducCourse = true;
            if (dataTable.Columns.Contains("GRADUATE STUDIES"))
                model.HasEmp_MACourse = true;
            if (dataTable.Columns.Contains("HIGHEST EDUCATIONAL ATTAINMENT"))
                model.HasEmp_HighestEducAttainment = true;
            if (dataTable.Columns.Contains("ELIGIBITY TYPE"))
                model.HasEmp_Eligibility_Type = true;
            if (dataTable.Columns.Contains("HIGHEST ELIGIBILITY"))
                model.HasEmp_Curr_Eligibility = true;
            if (dataTable.Columns.Contains("HIGHEST MANDATORY TRAINING"))
                model.HasEmp_HighestMandatoryTraining = true;
            return model;
        }

        public EmployeeModel MapEmployee(DataRow row, AlphaColumnListModel column, List<tblRanks> ranks,
            List<tblUnits> units
            , List<tblDutyStatus> dutyStatusList, List<tblSalaryGrades> salaGradeList,
            List<tblJobFunctions> jobFunctionList,
            List<tblCourses> courseList, List<tblEligibilities> eligibilities,
            List<tblMandatoryTrainings> mandatoryTrainings
            , List<tblReligions> religions)
        {
            var model = new EmployeeModel();
        
            model.Emp_Number = row["Account Number"].ToString();
            if (column.HasEmp_LastName)
                model.Emp_LastName = row["Last name"].ToString();
            if (column.HasEmp_FirstName)
                model.Emp_FirstName = row["First name"].ToString();
            if (column.HasEmp_MiddleName)
                model.Emp_MiddleName = row["Middle name"].ToString();
            if (column.HasEmp_SuffixName)
                model.Emp_SuffixName = row["Suffix name"].ToString();
            if (column.HasEmp_BP_Number)
                model.Emp_BP_Number = row["BP Number"].ToString();
            if (column.HasEmp_Tax_Code)
                model.Emp_Tax_Code = row["Tax Code"].ToString();
            if (column.HasEmp_Atm_Number)
                model.Emp_Atm_Number = row["ATM number"].ToString();
            //model.Emp_DutyStatus = (int) DutyStatuses.Active;
            if (column.HasEmp_Curr_Unit)
            {
                if (!string.IsNullOrWhiteSpace(row["PRESENT ASSIGNMENT (Unit Code)"].ToString()))
                {
                    var unitCode = row["PRESENT ASSIGNMENT (Unit Code)"].ToString();
                    var unit = units.FirstOrDefault(a => a.Unit_Code == unitCode);
                    model.Emp_Curr_Unit = unit?.Unit_Id;
                }
            }
            if (column.HasEmp_DES)
            {
                if (!string.IsNullOrWhiteSpace(row["DES"].ToString()))
                    model.Emp_DES = Convert.ToDateTime(row["DES"].ToString());
            }
                
            if (column.HasEmp_BirthDate)
            {
                if (!string.IsNullOrWhiteSpace(row["Date of birth"].ToString()))
                    model.Emp_BirthDate = Convert.ToDateTime(row["Date of birth"].ToString());
            }
            if (column.HasEmp_BirthPlace)
                model.Emp_BirthPlace = row["Place of Birth"].ToString();
            if (column.HasEmp_Gender)
            {
                if (!string.IsNullOrEmpty(row["Sex"].ToString()))
                    model.Emp_Gender = row["Sex"].ToString().Contains("F") ? "F" : "M";
                else
                    model.Emp_Gender = null;
            }
            if (column.HasEmp_Height)
                model.Emp_Height = row["Height"].ToString();
            if (column.HasEmp_Weight)
                model.Emp_Weight = row["Weight"].ToString();
            if (column.HasEmp_BloodType)
                model.Emp_BloodType = row["Blood Type"].ToString();
            if (column.HasEmp_GSISNumber)
                model.Emp_GSISNumber = row["GSIS ID NO"].ToString();
            if (column.HasEmp_PAGIBIGNumber)
                model.Emp_PAGIBIGNumber = row["PAG-IBIG ID NO"].ToString();
            if (column.HasEmp_PHICNumber)
                model.Emp_PHICNumber = row["PHILHEALTH NO"].ToString();
            if (column.HasEmp_SSSNumber)
                model.Emp_SSSNumber = row["SSS NO"].ToString();
            if (column.HasEmp_TINNumber)
                model.Emp_TINNumber = row["TIN"].ToString();
            if (column.HasEmp_Religion)
            {
                var reg = row["Religion"].ToString();
                var religion = religions.FirstOrDefault(a => a.Religion_Name.ToLower().Trim() == reg.ToLower().Trim());
                model.Emp_Religion = religion?.Religion_Id;
            }
            if (column.HasEmp_EmailAddress)
                model.Emp_EmailAddress = row["E-MAIL ADDRESS (if any)"].ToString();
            if (column.HasEmp_MobileNumber)
                model.Emp_MobileNumber = row["CELLPHONE NO"].ToString();
            if (column.HasEmp_AgencyEmpNumber)
                model.Emp_AgencyEmpNumber = row["AGENCY EMPLOYEE NO"].ToString();
            if (column.HasEmp_Residential_HouseNo)
                model.Emp_Residential_HouseNo = row["RESIDENTIAL House/Block/Lot No"].ToString();
            if (column.HasEmp_Residential_Street)
                model.Emp_Residential_Street = row["RESIDENTIAL Street"].ToString();
            if (column.HasEmp_Residential_Village)
                model.Emp_Residential_Village = row["RESIDENTIAL Subdivision/Village"].ToString();
            if (column.HasEmp_Residential_Barangay)
                model.Emp_Residential_Barangay = row["RESIDENTIAL Barangay"].ToString();
            if (column.HasEmp_Residential_Municipality)
                model.Emp_Residential_Municipality = row["RESIDENTIAL City/Municipality"].ToString();
            if (column.HasEmp_Residential_Province)
                model.Emp_Residential_Province = row["RESIDENTIAL Province"].ToString();
            if (column.HasEmp_Residential_ZipCode)
                model.Emp_Residential_ZipCode = row["RESIDENTIAL Zip Code"].ToString();
            if (column.HasEmp_Residential_PhoneNumber)
                model.Emp_Residential_PhoneNumber = row["RESIDENTIAL Telephone No"].ToString();
            if (column.HasEmp_Permanent_HouseNo)
                model.Emp_Permanent_HouseNo = row["PERMANENT House/Block/Lot No."].ToString();
            if (column.HasEmp_Permanent_Street)
                model.Emp_Permanent_Street = row["PERMANENT Street"].ToString();
            if (column.HasEmp_Permanent_Village)
                model.Emp_Permanent_Village = row["PERMANENT Subdivision/Village"].ToString();
            if (column.HasEmp_Permanent_Barangay)
                model.Emp_Permanent_Barangay = row["PERMANENT Barangay"].ToString();
            if (column.HasEmp_Permanent_Municipality)
                model.Emp_Permanent_Municipality = row["PERMANENT City/Municipality"].ToString();
            if (column.HasEmp_Permanent_Province)
                model.Emp_Permanent_Province = row["PERMANENT PermanentProvince"].ToString();
            if (column.HasEmp_Permanent_ZipCode)
                model.Emp_Permanent_ZipCode = row["PERMANENT Zip Code"].ToString();
            if (column.HasEmp_Permanent_PhoneNumber)
                model.Emp_Permanent_PhoneNumber = row["PERMANENT Telephone No"].ToString();
            //BFP Information
            if (column.HasEmp_Service_GovtStartDate)
            {
                if (!string.IsNullOrWhiteSpace(row["DATE ENTERED GOVERNMENT SERVICE"].ToString()))
                    model.Emp_Service_GovtStartDate =
                        Convert.ToDateTime(row["DATE ENTERED GOVERNMENT SERVICE"].ToString());
            }
            if (column.HasEmp_Service_UniformGovtStartDate)
            {
                if (
                    !string.IsNullOrWhiteSpace(
                        row["DATE ENTERED UNIFORMED SERVICE TO OTHER GOVERNMENT AGENCY/IES"].ToString()))
                    model.Emp_Service_UniformGovtStartDate =
                        Convert.ToDateTime(
                            row["DATE ENTERED UNIFORMED SERVICE TO OTHER GOVERNMENT AGENCY/IES"].ToString());
            }
            if (column.HasEmp_Service_StartDate)
            {
                if (!string.IsNullOrWhiteSpace(row["DATE ENTERED UNIFORMED FIRE SERVICE"].ToString()))
                    model.Emp_Service_StartDate =
                        Convert.ToDateTime(row["DATE ENTERED UNIFORMED FIRE SERVICE"].ToString());
            }
            if (column.HasEmp_LastPromotionDate_Temp)
            {
                if (!string.IsNullOrWhiteSpace(row["DATE OF LAST PROMOTION (TEMPORARY STATUS)"].ToString()))
                    model.Emp_LastPromotionDate_Temp =
                        Convert.ToDateTime(row["DATE OF LAST PROMOTION (TEMPORARY STATUS)"].ToString());
            }
            if (column.HasEmp_LastPromotionDate_Permanent)
            {
                if (!string.IsNullOrWhiteSpace(row["DATE OF LAST PROMOTION (PERMANENT STATUS)"].ToString()))
                    model.Emp_LastPromotionDate_Permanent =
                        Convert.ToDateTime(row["DATE OF LAST PROMOTION (PERMANENT STATUS)"].ToString());
            }
            if (column.HasEmp_AssumedOfficerDate)
            {
                if (!string.IsNullOrWhiteSpace(row["DATE ASSUMED OFFICER"].ToString()))
                    model.Emp_AssumedOfficerDate = Convert.ToDateTime(row["DATE ASSUMED OFFICER"].ToString());
            }
            if (column.HasEmp_LastTrainingDate)
            {
                if (!string.IsNullOrWhiteSpace(row["LAST TRAINING DATE"].ToString()))
                    model.Emp_LastTrainingDate = Convert.ToDateTime(row["LAST TRAINING DATE"].ToString());
            }
            if (column.HasEmp_ItemNumber)
                model.Emp_ItemNumber = row["ITEM NUMBER"].ToString();
            if (column.HasEmp_BadgeNumber)
                model.Emp_BadgeNumber = row["BADGE NUMBER"].ToString();
            if (column.HasEmp_Curr_Rank)
            {
                var rankName = row["PRESENT RANK"].ToString();
                var tblrank = ranks.FirstOrDefault(a => a.Rank_Name.ToLower().Trim() == rankName.ToLower().Trim());
                model.Emp_Curr_Rank = tblrank?.Rank_Id;
            }
            if (column.HasEmp_PresentAsgmt_DO_BO_RO)
                model.Emp_PresentAsgmt_DO_BO_RO = row["PRESENT ASSIGNMENT DO/BO/RO"].ToString();
            if (column.HasEmp_Curr_ApptStatus)
            {
                if (!string.IsNullOrEmpty(row["APPOINTMENT STATUS"].ToString()))
                    model.Emp_Curr_ApptStatus =
                        (int)
                            Functions.GetEnumValueFromDescription<AppointmentStatuses>(
                                row["APPOINTMENT STATUS"].ToString().ToUpper().Trim());
            }
            if (column.HasEmp_AppointmentStatus_DO_BO_RO)
                model.Emp_AppointmentStatus_DO_BO_RO = row["Appointment Status DO/BO/RO"].ToString();
            if (column.HasEmp_DutyStatus)
            {
                var dutyStatus = row["DUTY STATUS"].ToString();
                var tblDutyStatus =
                    dutyStatusList.FirstOrDefault(a => a.DutyStat_Name.ToLower().Trim() == dutyStatus.ToLower().Trim());
                model.Emp_DutyStatus = tblDutyStatus?.DutyStat_Id;
            }
            if (column.HasEmp_Curr_SalaryGrade)
            {
                var salaryGrade = row["SALARY GRADE"].ToString();
                var tblSalaryGrades =
                    salaGradeList.FirstOrDefault(
                        a => a.SalaryGrade_Name.ToLower().Trim() == salaryGrade.ToLower().Trim());
                model.Emp_Curr_SalaryGrade = tblSalaryGrades?.SalaryGrade_Id;
            }
            if (column.HasEmp_Curr_JobFunc)
            {
                var presentDesg = row["PRESENT DESIGNATION (Code)"].ToString();
                var tblJobFunctions =
                    jobFunctionList.FirstOrDefault(a => a.JobFunc_Code.ToLower().Trim() == presentDesg.ToLower().Trim());
                model.Emp_Curr_JobFunc = tblJobFunctions?.JobFunc_Id;
            }
            //if (column.HasEmp_Curr_PosDesignationTitle)
            //    model.Emp_Curr_PosDesignationTitle = row["SPECIFY DESIGNATION"].ToString();
            if (column.HasEmp_Remarks)
                model.Emp_Remarks = row["REMARKS"].ToString();
            if (column.HasEmp_EducCourse)
            {
                var baseCourse = row["BASE COURSE"].ToString();
                var tblCourses =
                    courseList.FirstOrDefault(
                        a =>
                            a.Course_Name.ToLower().Trim() == baseCourse.ToLower().Trim() &&
                            a.Course_Category == (int) CourseType.Base);
                model.Emp_EducCourse = tblCourses?.Course_Id;
            }
            if (column.HasEmp_MACourse)
            {
                var graduatedCourse = row["GRADUATE STUDIES"].ToString();
                var tblCourses =
                    courseList.FirstOrDefault(
                        a =>
                            a.Course_Name.ToLower().Trim() == graduatedCourse.ToLower().Trim() &&
                            a.Course_Category == (int) CourseType.Masteral);
                model.Emp_MACourse = tblCourses?.Course_Id;
            }
            if (column.HasEmp_HighestEducAttainment)
            {
                if (!string.IsNullOrEmpty(row["HIGHEST EDUCATIONAL ATTAINMENT"].ToString()))
                    model.Emp_HighestEducAttainment =
                        (int)
                            Functions.GetEnumValueFromDescription<EducAttaintmentLevel>(
                                row["HIGHEST EDUCATIONAL ATTAINMENT"].ToString().ToUpper().Trim());
            }
            if (column.HasEmp_Eligibility_Type)
            {
                if (!string.IsNullOrEmpty(row["ELIGIBITY TYPE"].ToString()))
                    model.Emp_Eligibility_Type =
                        (int)
                            Functions.GetEnumValueFromDescription<EligibilityType>(
                                row["ELIGIBITY TYPE"].ToString().ToUpper().Trim());
            }
            if (column.HasEmp_Curr_Eligibility)
            {
                var highestEligibility = row["HIGHEST ELIGIBILITY"].ToString();
                var tblEligibilities =
                    eligibilities.FirstOrDefault(
                        a => a.Eligibity_Name.ToLower().Trim() == highestEligibility.ToLower().Trim());
                model.Emp_Curr_Eligibility = tblEligibilities?.Eligiblity_Id;
            }
            if (column.HasEmp_HighestMandatoryTraining)
            {
                var trainingName = row["HIGHEST MANDATORY TRAINING"].ToString();
                var tblMandatoryTrainings =
                    mandatoryTrainings.FirstOrDefault(
                        a => a.Training_Name.ToLower().Trim() == trainingName.ToLower().Trim());
                model.Emp_HighestMandatoryTraining = tblMandatoryTrainings?.Training_Id;
            }
            return model;
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblEmployees, EmployeeModel>();
            Mapper.CreateMap<tblEmployees, EmployeeModel>().ReverseMap();
            Mapper.CreateMap<List<tblEmployees>, List<EmployeeModel>>().ReverseMap();
            Mapper.CreateMap<List<tblEmployees>, List<EmployeeModel>>();
        }

        public bool IsValidUsername(string newUserName, int employeeID = 0)
        {
            var details = BFPContext.tblEmployees
                .Where(a => a.Emp_Username == newUserName && a.Emp_IsDeleted == false)
                .Select(a => new
                {
                    a.Emp_Id
                }).FirstOrDefault();

            if (details != null)
            {
                if (((employeeID > 0) && (details.Emp_Id != employeeID)) || (details.Emp_Id > 0))
                    throw new Exception("Username already in use.");
            }

            return true;
        }

        public List<EmployeeModel> GetAlphaList(EmployeeSearchModel searchModel,string fileName)
        {
            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            //var newFile =
            //    $"{applicationPath}{@"\Content\MISC\Generated\AlphaList_"}{DateTime.Now.Ticks}{".xlsx"}";
            var template = $"{applicationPath}{@"\Content\MISC\Generated\" + fileName}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["Sheet1"];
            var cell = worksheet.Cells;

            var oHRISReport = new HRISReport(BFPContext);
            var columns = oHRISReport.GetIncludedColumn(cell);

            var retEmps = new List<EmployeeModel>();
            BFPContext.Database.Log = s => Debug.WriteLine(s);
            var searchTerms = searchModel;
            var employees = BFPContext.tblEmployees.Where(a => a.Emp_IsDeleted == false);

            if (!PageSecurity.HasAccess(PageArea.HRIS_EmployeeRoster_CanViewAll))
            {
                if (PageSecurity.HasAccess(PageArea.HRIS_EmployeeRoster_RestricttoRegion))
                {
                    employees = employees.Where(
                        a => a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID);
                }

                if (PageSecurity.HasAccess(PageArea.HRIS_EmployeeRoster_RestricttoProvince))
                {
                    employees =
                        employees.Where(
                            a => a.tblUnits.tblCityMunicipality.Municipality_Province_Id == CurrentUser.ProvinceID);
                }

                if (PageSecurity.HasAccess(PageArea.HRIS_EmployeeRoster_RestricttoStation))
                {
                    employees = employees.Where(a => a.Emp_Curr_Unit == CurrentUser.EmployeeUnitId);
                }
            }

            if (!string.IsNullOrEmpty(searchTerms.LastName))
                employees = employees.Where(a => a.Emp_LastName.Contains(searchTerms.LastName));
            if (!string.IsNullOrEmpty(searchTerms.AccountNumber))
                employees = employees.Where(a => a.Emp_Number.Contains(searchTerms.AccountNumber));
            if (!string.IsNullOrEmpty(searchTerms.BadgeNumber))
                employees = employees.Where(a => a.Emp_BadgeNumber.Contains(searchTerms.BadgeNumber));
            if (!string.IsNullOrEmpty(searchTerms.ItemNumber))
                employees = employees.Where(a => a.Emp_ItemNumber.Contains(searchTerms.ItemNumber));
            if (!string.IsNullOrEmpty(searchTerms.Gender))
                employees = employees.Where(a => a.Emp_Gender.Contains(searchTerms.Gender));
            if (searchTerms.StartServiceDate.HasValue)
                employees = employees.Where(a => a.Emp_Service_StartDate == searchTerms.StartServiceDate);
            if (searchTerms.LastTrainingDate.HasValue)
                employees = employees.Where(a => a.Emp_LastTrainingDate == searchTerms.LastTrainingDate);
            if (searchTerms.Birthdate.HasValue)
                employees = employees.Where(a => a.Emp_BirthDate == searchTerms.Birthdate);
            if (searchTerms.CivilStatus > 0)
                employees = employees.Where(a => a.Emp_CivilStatus == searchTerms.CivilStatus);
            if (searchTerms.DutyStatusId > 0)
                employees = employees.Where(a => a.Emp_DutyStatus == searchTerms.DutyStatusId);
            if (searchTerms.DesignationId > 0)
                employees = employees.Where(a => a.Emp_Curr_JobFunc == searchTerms.DesignationId);
            if (searchTerms.RankId > 0) employees = employees.Where(a => a.Emp_Curr_Rank == searchTerms.RankId);
            if (searchTerms.CourseId > 0)
                employees = employees.Where(a => a.Emp_EducCourse == searchTerms.CourseId);
            if (searchTerms.EligibilityId > 0)
                employees = employees.Where(a => a.Emp_Curr_Eligibility == searchTerms.EligibilityId);
            if (searchTerms.AppointmentStatusId > 0)
                employees = employees.Where(a => a.Emp_Curr_ApptStatus == searchTerms.AppointmentStatusId);
            if (searchTerms.RegionId > 0)
                employees = employees.Where(a => a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == searchTerms.RegionId);
            if (searchTerms.ProvinceId > 0)
                employees = employees.Where(a => a.tblUnits.tblCityMunicipality.Municipality_Province_Id == searchTerms.ProvinceId);
            if (searchTerms.MunicipalityId > 0)
                employees = employees.Where(a => a.tblUnits.Unit_Municipality_Id == searchTerms.MunicipalityId);
            if (searchTerms.UnitId > 0) employees = employees.Where(a => a.Emp_Curr_Unit == searchTerms.UnitId);
             

            var selectList = new SelectList<tblEmployees>();
            selectList.Add(e => e.Emp_Number);
            selectList.Add(e => e.Emp_LastName);
            selectList.Add(e => e.Emp_FirstName);
            selectList.Add(e => e.Emp_MiddleName);
            selectList.Add(e => e.Emp_SuffixName);
            selectList.Add(e => e.Emp_Curr_Unit);
            selectList.Add(e => e.Emp_DES);
            selectList.Add(e => e.Emp_BP_Number);
            selectList.Add(e => e.Emp_Tax_Code);
            selectList.Add(e => e.Emp_Atm_Number);
            selectList.Add(e => e.Emp_Id);
            if (columns.HasEmp_BirthPlace) selectList.Add(e => e.Emp_BirthPlace); 

            if (columns.HasEmp_BirthDate) selectList.Add(e => e.Emp_BirthDate);

            if (columns.HasEmp_CivilStatus) selectList.Add(e => e.Emp_CivilStatus); 

            if (columns.HasEmp_Citizenship)
            {
                selectList.Add(e => e.Emp_Citizenship);
                selectList.Add(e => e.Emp_Citizenship_Dual);
                selectList.Add(e => e.Emp_Citizenship_Country); 
            }

            if (columns.HasEmp_Gender) selectList.Add(e => e.Emp_Gender); 
            if (columns.HasEmp_Height) selectList.Add(e => e.Emp_Height); 
            if (columns.HasEmp_Weight) selectList.Add(e => e.Emp_Weight); 
            if (columns.HasEmp_BloodType) selectList.Add(e => e.Emp_BloodType); 
            if (columns.HasEmp_GSISNumber) selectList.Add(e => e.Emp_GSISNumber); 
            if (columns.HasEmp_PAGIBIGNumber) selectList.Add(e => e.Emp_PAGIBIGNumber); 
            if (columns.HasEmp_PHICNumber) selectList.Add(e => e.Emp_PHICNumber); 
            if (columns.HasEmp_SSSNumber) selectList.Add(e => e.Emp_SSSNumber); 
            if (columns.HasEmp_TINNumber) selectList.Add(e => e.Emp_TINNumber); 
            if (columns.HasEmp_Religion)
            {
                selectList.Add(e => e.Emp_Religion);
                selectList.Add(e => e.Emp_Religion_Others); 
            }
            if (columns.HasEmp_EmailAddress) selectList.Add(e => e.Emp_EmailAddress); 
            if (columns.HasEmp_MobileNumber) selectList.Add(e => e.Emp_MobileNumber); 
            if (columns.HasEmp_AgencyEmpNumber) selectList.Add(e => e.Emp_AgencyEmpNumber); 
            if (columns.HasEmp_Residential_HouseNo) selectList.Add(e => e.Emp_Residential_HouseNo); 
            if (columns.HasEmp_Residential_Street) selectList.Add(e => e.Emp_Residential_Street); 
            if (columns.HasEmp_Residential_Village) selectList.Add(e => e.Emp_Residential_Village); 
            if (columns.HasEmp_Residential_Barangay) selectList.Add(e => e.Emp_Residential_Barangay); 
            if (columns.HasEmp_Residential_Municipality) selectList.Add(e => e.Emp_Residential_Municipality); 
            if (columns.HasEmp_Residential_Province) selectList.Add(e => e.Emp_Residential_Province); 
            if (columns.HasEmp_Residential_ZipCode) selectList.Add(e => e.Emp_Residential_ZipCode); 
            if (columns.HasEmp_Residential_PhoneNumber) selectList.Add(e => e.Emp_Residential_PhoneNumber); 
            if (columns.HasEmp_Permanent_HouseNo) selectList.Add(e => e.Emp_Permanent_HouseNo); 
            if (columns.HasEmp_Permanent_Street) selectList.Add(e => e.Emp_Permanent_Street); 
            if (columns.HasEmp_Permanent_Village) selectList.Add(e => e.Emp_Permanent_Village); 
            if (columns.HasEmp_Permanent_Barangay) selectList.Add(e => e.Emp_Permanent_Barangay); 
            if (columns.HasEmp_Permanent_Municipality) selectList.Add(e => e.Emp_Permanent_Municipality); 
            if (columns.HasEmp_Permanent_Province) selectList.Add(e => e.Emp_Permanent_Province); 
            if (columns.HasEmp_Permanent_ZipCode) selectList.Add(e => e.Emp_Permanent_ZipCode); 
            if (columns.HasEmp_Permanent_PhoneNumber) selectList.Add(e => e.Emp_Permanent_PhoneNumber);

            //BFP Information
            if (columns.HasEmp_Service_GovtStartDate || columns.YearsGovtServiceStartDate || columns.UniformedOptionalRetirementDate || columns.NonUniformedOptionalRetirementDate)
                selectList.Add(e => e.Emp_Service_GovtStartDate);  

            if (columns.HasEmp_Service_UniformGovtStartDate) selectList.Add(e => e.Emp_Service_UniformGovtStartDate); 
            if (columns.YearsUniformedFireService || columns.HasEmp_Service_StartDate) selectList.Add(e => e.Emp_Service_StartDate);

            if (columns.HasEmp_Curr_Rank || columns.MandatoryRetirementDate)
                selectList.Add(e => e.Emp_Curr_Rank);

            if (columns.MandatoryRetirementDate) selectList.Add(e => e.Emp_BirthDate);

            if (columns.HasEmp_LastPromotionDate_Temp) selectList.Add(e => e.Emp_LastPromotionDate_Temp); 
            if (columns.HasEmp_LastPromotionDate_Permanent) selectList.Add(e => e.Emp_LastPromotionDate_Permanent); 
            if (columns.HasEmp_AssumedOfficerDate) selectList.Add(e => e.Emp_AssumedOfficerDate); 
            if (columns.HasEmp_LastTrainingDate) selectList.Add(e => e.Emp_LastTrainingDate); 
            if (columns.HasEmp_ItemNumber) selectList.Add(e => e.Emp_ItemNumber); 
            if (columns.HasEmp_BadgeNumber) selectList.Add(e => e.Emp_BadgeNumber);  
            if (columns.HasEmp_PresentAsgmt_DO_BO_RO) selectList.Add(e => e.Emp_PresentAsgmt_DO_BO_RO);
            if (columns.HasEmp_Curr_ApptStatus) selectList.Add(e => e.Emp_Curr_ApptStatus); 
            if (columns.HasEmp_AppointmentStatus_DO_BO_RO) selectList.Add(e => e.Emp_AppointmentStatus_DO_BO_RO); 
            if (columns.HasEmp_DutyStatus) selectList.Add(e => e.Emp_DutyStatus);
          
            if (columns.HasEmp_Curr_SalaryGrade) selectList.Add(e => e.Emp_Curr_SalaryGrade);
            if (columns.HasEmp_Curr_JobFunc) selectList.Add(e => e.Emp_Curr_JobFunc);
            //if (columns.HasEmp_Curr_PosDesignationTitle) selectList.Add(e => e.Emp_Curr_PosDesignationTitle); 
            if (columns.HasEmp_Remarks) selectList.Add(e => e.Emp_Remarks); 
            if (columns.HasEmp_EducCourse) selectList.Add(e => e.Emp_EducCourse);
            if (columns.HasEmp_MACourse) selectList.Add(e => e.Emp_MACourse);
            if (columns.HasEmp_HighestEducAttainment) selectList.Add(e => e.Emp_HighestEducAttainment); 
            if (columns.HasEmp_Eligibility_Type) selectList.Add(e => e.Emp_Eligibility_Type); 
            if (columns.HasEmp_Curr_Eligibility) selectList.Add(e => e.Emp_Curr_Eligibility);
            
            if (columns.HasEmp_HighestMandatoryTraining) selectList.Add(e => e.Emp_HighestMandatoryTraining); 
            var employeeList = selectList.Select<EmployeeModel>(employees).OrderBy(a => a.Emp_LastName).ToList();
             
            return employeeList;
        }

        public string GetPositionTitle(int empId)
        {
            using (var context = new EBFPEntities())
            {
                var positionTitle = "";
                var positionList =
                    context.tblEmployeeSpecifyDesignation.Where(a => a.SpecifyDesig_Emp_Id == empId).ToList();
                if (positionList.Count > 0)
                {
                    foreach (var item in positionList)
                    {
                        var title = item.SpecifyDesig_Title + ",";
                        positionTitle = positionTitle + title;
                    }
                }
                return positionTitle != ""  ? positionTitle.TrimEnd(',') : positionTitle;
            }
        }

        public void UpdateValidatedEmp(string type, int empId, int validatedBy)
        {
            var details = BFPContext.tblEmployees
                .FirstOrDefault(a => a.Emp_Id == empId);

            if (details == null) throw new Exception("Employee cannot be found!");

            if (type == "Validated")
            {
                details.Emp_CheckedBy = validatedBy;
                details.Emp_CheckedDate = DateTime.Now;
                details.Emp_IsChecked = true;
            }
            else
            {
                details.Emp_CheckedBy = null;
                details.Emp_CheckedDate = null;
                details.Emp_IsChecked = false;
            }

            BFPContext.Entry(details).State = EntityState.Modified;
            BFPContext.SaveChanges();
        }

        public bool CheckRank(int rankId)
        {
            var employee = BFPContext.tblEmployees.FirstOrDefault(a => a.Emp_Curr_Rank == rankId);
            if (employee != null)
                return true;
            else
                return false;
        }
    }

    public class SelectList<IQueryable>
    {
        private List<MemberInfo> members = new List<MemberInfo>();
        public SelectList<IQueryable> Add<TValue>(Expression<Func<IQueryable, TValue>> selector)
        {
            var member = ((MemberExpression)selector.Body).Member;
            members.Add(member);
            return this;
        }
        public IQueryable<TResult> Select<TResult>(IQueryable<IQueryable> source)
        {
            var sourceType = typeof(IQueryable);
            var resultType = typeof(TResult);
            var parameter = Expression.Parameter(sourceType, "e");
            var bindings = members.Select(member => Expression.Bind(
                resultType.GetProperty(member.Name), Expression.MakeMemberAccess(parameter, member)));
            var body = Expression.MemberInit(Expression.New(resultType), bindings);
            var selector = Expression.Lambda<Func<IQueryable, TResult>>(body, parameter);
            return source.Select(selector);
        }
    }
}