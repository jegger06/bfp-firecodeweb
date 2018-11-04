using System;
using EBFP.BL.HumanResources;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EBFP.BL.Administration;
using EBFP.BL.CIS;
using EBFP.BL.Establishment;
using EBFP.BL.Inventory;
using EBFP.Helper;

namespace EBFP.BL.Helper
{
    public class Selections
    {
        private static List<System.Web.Mvc.SelectListItem> _Genders;
        private static List<System.Web.Mvc.SelectListItem> _Citizenship;
        private static List<System.Web.Mvc.SelectListItem> _DualCitizenship;
        private static List<System.Web.Mvc.SelectListItem> _CivilStatus;
        private static List<System.Web.Mvc.SelectListItem> _Ranks;
        private static List<System.Web.Mvc.SelectListItem> _Eligibilities;
        private static List<System.Web.Mvc.SelectListItem> _Units;
        private static List<System.Web.Mvc.SelectListItem> _Municipality;
        private static List<System.Web.Mvc.SelectListItem> _MandatoyTrainings;
        private static List<System.Web.Mvc.SelectListItem> _Courses;
        private static List<System.Web.Mvc.SelectListItem> _DutyStatuses;
        private static List<System.Web.Mvc.SelectListItem> _JobFunctions;
        private static List<System.Web.Mvc.SelectListItem> _SalaryGrades;
        private static List<System.Web.Mvc.SelectListItem> _DiagnosisStatus;
        private static List<System.Web.Mvc.SelectListItem> _Region;
        private static List<System.Web.Mvc.SelectListItem> _Province;
        private static List<System.Web.Mvc.SelectListItem> _EmployeeList;
        private static List<System.Web.Mvc.SelectListItem> _LeaveType;
        private static List<System.Web.Mvc.SelectListItem> _Month;
        private static List<System.Web.Mvc.SelectListItem> _FireMarshall;
        private static List<System.Web.Mvc.SelectListItem> _Roles;
        private static List<System.Web.Mvc.SelectListItem> _Classification;
        private static List<System.Web.Mvc.SelectListItem> _IncomeClass;
        private static List<System.Web.Mvc.SelectListItem> _StationCategory;
        private static List<System.Web.Mvc.SelectListItem> _TruckModel;
        private static List<System.Web.Mvc.SelectListItem> _StationName;
        private static List<System.Web.Mvc.SelectListItem> _OVModel;
        private static List<System.Web.Mvc.SelectListItem> _RetiredEmployees;

        private static List<System.Web.Mvc.SelectListItem> _Directorates;
        private static List<System.Web.Mvc.SelectListItem> _InventoryGroup;
        private static List<System.Web.Mvc.SelectListItem> _InventoryArticle;
        private static List<System.Web.Mvc.SelectListItem> _Articles;
        private static List<System.Web.Mvc.SelectListItem> _EstablishmentList;
        private static List<System.Web.Mvc.SelectListItem> _MasteralCourses;
        private static List<System.Web.Mvc.SelectListItem> _Religions;
        private static List<System.Web.Mvc.SelectListItem> _BaseCourses;
        private static List<System.Web.Mvc.SelectListItem> _MunicipalityType;
        private static IAdministrationUnitOfWork aUnitOfWork = new AdministrationUnitOfWork();
        private static IHRISUnitOfWork UnitOfWork = new HRISUnitOfWork();
        private static IInventoryUnitOfWork oUnitOfWork = new InventoryUnitOfWork();
        private static ICISUnitOfWork cisUnitOfWork = new CISUnitOfWork();
        private static IEstablishmentUnitOfWork estUnitOfWork = new EstablishmentUnitOfWork();
        public static void SelectionReset()
        {
            _Genders = null;
            _Citizenship = null;
            _DualCitizenship = null;
            _CivilStatus = null;
            _Ranks = null;
            _Eligibilities = null;
            _Units = null;
            _Municipality = null;
            _MandatoyTrainings = null;
            _Courses = null;
            _DutyStatuses = null;
            _JobFunctions = null;
            _SalaryGrades = null;
            _DiagnosisStatus = null;
            _Region = null;
            _Province = null;
            _EmployeeList = null;
            _Month = null;
            _FireMarshall = null;
            _Roles = null;
            _Classification = null;
            _IncomeClass = null;
            _TruckModel = null;
            _StationName = null;
            _OVModel = null;
            _RetiredEmployees = null;
            _Directorates = null;
            _InventoryGroup = null;
            _InventoryArticle = null;
            _EstablishmentList = null;
            _MasteralCourses = null;
            _Religions = null;
            _BaseCourses = null;
            _MunicipalityType = null;
        }

        public static List<System.Web.Mvc.SelectListItem> Genders
        {
            get
            {
                if (_Genders == null)
                {
                    _Genders = new List<System.Web.Mvc.SelectListItem>();
                    _Genders.Add(new System.Web.Mvc.SelectListItem() { Text = "Male", Value = "M" });
                    _Genders.Add(new System.Web.Mvc.SelectListItem() { Text = "Female", Value = "F" });
                }

                return _Genders;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> MunicipalityType
        {
            get
            {
                if (_MunicipalityType == null)
                {
                    _MunicipalityType = new List<System.Web.Mvc.SelectListItem>();
                    _MunicipalityType.Add(new System.Web.Mvc.SelectListItem() { Text = "City", Value = "1" });
                    _MunicipalityType.Add(new System.Web.Mvc.SelectListItem() { Text = "Municipality", Value = "2" });
                }

                return _MunicipalityType;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> Citizenship
        {
            get
            {
                if (_Citizenship == null)
                {
                    _Citizenship = new List<System.Web.Mvc.SelectListItem>();
                    _Citizenship.Add(new System.Web.Mvc.SelectListItem() { Text = "Filipino", Value = "Filipino" });
                    _Citizenship.Add(new System.Web.Mvc.SelectListItem() { Text = "Dual Citizenship", Value = "Dual Citizenship" });
                }

                return _Citizenship;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> DualCitizenship
        {
            get
            {
                if (_DualCitizenship == null)
                {
                    _DualCitizenship = new List<System.Web.Mvc.SelectListItem>();
                    _DualCitizenship.Add(new System.Web.Mvc.SelectListItem() { Text = "By Birth", Value = "By Birth" });
                    _DualCitizenship.Add(new System.Web.Mvc.SelectListItem() { Text = "By Naturalization", Value = "By Naturalization" });
                }

                return _DualCitizenship;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> CivilStatus
        {
            get
            {
                if (_CivilStatus == null)
                {
                    _CivilStatus = new List<System.Web.Mvc.SelectListItem>();
                    _CivilStatus.Add(new System.Web.Mvc.SelectListItem() { Text = "Single", Value = "1" });
                    _CivilStatus.Add(new System.Web.Mvc.SelectListItem() { Text = "Widowed", Value = "2" });
                    _CivilStatus.Add(new System.Web.Mvc.SelectListItem() { Text = "Married", Value = "3" });
                    _CivilStatus.Add(new System.Web.Mvc.SelectListItem() { Text = "Separated", Value = "4" });
                    _CivilStatus.Add(new System.Web.Mvc.SelectListItem() { Text = "Annulled", Value = "5" });
                    _CivilStatus.Add(new System.Web.Mvc.SelectListItem() { Text = "Others", Value = "6" });
                }
                return _CivilStatus;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> Eligibilities
        {
            get
            {
                if (_Eligibilities == null)
                {
                    var items = UnitOfWork.Eligibility.GetAll();
                    _Eligibilities = new List<System.Web.Mvc.SelectListItem>();
                    foreach (var item in items)
                        _Eligibilities.Add(new System.Web.Mvc.SelectListItem() { Text = item.Eligibity_Name, Value = item.Eligiblity_Id.ToString() });
                }
                else
                {
                    foreach (var item in _Eligibilities)
                        item.Selected = false;
                }
                return _Eligibilities;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> BaseCourses
        {
            get
            {
                if (_BaseCourses == null)
                {
                    var items = UnitOfWork.Course.GetAll().Where(a=> a.Course_Category == (int)CourseType.Base);
                    _BaseCourses = new List<System.Web.Mvc.SelectListItem>();
                    foreach (var item in items)
                        _BaseCourses.Add(new System.Web.Mvc.SelectListItem() { Text = item.Course_Name, Value = item.Course_Id.ToString() });
                }
                else
                {
                    foreach (var item in _BaseCourses)
                        item.Selected = false;
                }
                return _BaseCourses;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> Courses
        {
            get
            {
                if (_Courses == null)
                {
                    var items = UnitOfWork.Course.GetAll();
                    _Courses = new List<System.Web.Mvc.SelectListItem>();
                    foreach (var item in items)
                        _Courses.Add(new System.Web.Mvc.SelectListItem() { Text = item.Course_Name, Value = item.Course_Id.ToString() });
                }
                else
                {
                    foreach (var item in _Courses)
                        item.Selected = false;
                }
                return _Courses;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> MasteralCourses
        {
            get
            {
                if (_MasteralCourses == null)
                {
                    var items = UnitOfWork.Course.GetAll().Where(a => a.Course_Category == (int) CourseType.Masteral);
                    _MasteralCourses = new List<System.Web.Mvc.SelectListItem>();
                    foreach (var item in items)
                        _MasteralCourses.Add(new System.Web.Mvc.SelectListItem()
                        {
                            Text = item.Course_Name,
                            Value = item.Course_Id.ToString()
                        });
                }
                else
                {
                    foreach (var item in _MasteralCourses)
                        item.Selected = false;
                }
                return _MasteralCourses;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> MandatoyTrainings
        {
            get
            {
                if (_MandatoyTrainings == null)
                {
                    var items = UnitOfWork.MandatoryTraining.GetAll();
                    _MandatoyTrainings = new List<System.Web.Mvc.SelectListItem>();
                    foreach (var item in items)
                        _MandatoyTrainings.Add(new System.Web.Mvc.SelectListItem() { Text = item.Training_Name, Value = item.Training_Id.ToString() });
                }
                else
                {
                    foreach (var item in _MandatoyTrainings)
                        item.Selected = false;
                }
                return _MandatoyTrainings;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> Units
        {
            get
            {
                if (_Units == null)
                {
                    var regions = UnitOfWork.Unit.GetAllForSelection();
                    _Units = new List<System.Web.Mvc.SelectListItem>();
                    foreach (var region in regions)
                    {
                        var optionGroup = new System.Web.Mvc.SelectListGroup() { Name = region.Reg_Description };
                        foreach (var unit in region.Units)
                        {
                            _Units.Add(
                                new System.Web.Mvc.SelectListItem()
                                {
                                    Text = string.Concat("[", unit.Unit_Code, "] ", unit.Unit_StationName, " , ", unit.Province_Name),
                                    Value = unit.Unit_Id.ToString(),
                                    Group = optionGroup
                                });
                        }
                    }
                }
                else
                {
                    foreach (var item in _Units)
                        item.Selected = false;
                }

                return _Units;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> DutyStatuses
        {
            get
            {
                if (_DutyStatuses == null)
                {
                    var items = UnitOfWork.DutyStatus.GetAll();
                    _DutyStatuses = new List<System.Web.Mvc.SelectListItem>();
                    foreach (var item in items)
                        _DutyStatuses.Add(new System.Web.Mvc.SelectListItem() { Text = item.DutyStat_Name, Value = item.DutyStat_Id.ToString() });
                }
                else
                {
                    foreach (var item in _DutyStatuses)
                        item.Selected = false;
                }
                return _DutyStatuses;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> Religions
        {
            get
            {
                if (_Religions == null)
                {
                    var items = UnitOfWork.Religion.GetAll();
                    _Religions = new List<System.Web.Mvc.SelectListItem>();
                    foreach (var item in items)
                        _Religions.Add(new System.Web.Mvc.SelectListItem() { Text = item.Religion_Name, Value = item.Religion_Id.ToString() });
                    _Religions.Add(new System.Web.Mvc.SelectListItem() { Text = "OTHERS", Value = "29" });
                }
                else
                {
                    foreach (var item in _Religions)
                        item.Selected = false;
                }
                return _Religions;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> JobFunctions
        {
            get
            {
                if (_JobFunctions == null)
                {
                    var items = UnitOfWork.JobFunction.GetAll();
                    _JobFunctions = new List<System.Web.Mvc.SelectListItem>();
                    foreach (var item in items)
                        _JobFunctions.Add(new System.Web.Mvc.SelectListItem() { Text = string.Concat("[", item.JobFunc_Code, "] ", item.JobFunc_Name), Value = item.JobFunc_Id.ToString() });
                }
                else
                {
                    foreach (var item in _JobFunctions)
                        item.Selected = false;
                }
                return _JobFunctions;
            }
        }

        public static List<SelectListItem> EstablishmentList
        {
            get
            {
                if (_EstablishmentList == null)
                {

                    var items = estUnitOfWork.Establishment.GetList(a => a.Est_Unit_Id == CurrentUser.EmployeeUnitId);
                    _EstablishmentList = new List<SelectListItem>();
                    foreach (var item in items)
                        _EstablishmentList.Add(new SelectListItem() { Text = item.Est_BusinessName, Value = item.Est_Id.ToString() });
                }
                else
                {
                    foreach (var item in _EstablishmentList)
                        item.Selected = false;
                }
                return _EstablishmentList;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> SalaryGrades
        {
            get
            {
                if (_SalaryGrades == null)
                {
                    var items = UnitOfWork.SalaryGrade.GetAll();
                    _SalaryGrades = new List<System.Web.Mvc.SelectListItem>();
                    foreach (var item in items)
                        _SalaryGrades.Add(new System.Web.Mvc.SelectListItem() { Text = item.SalaryGrade_Name, Value = item.SalaryGrade_Id.ToString() });
                }
                else
                {
                    foreach (var item in _SalaryGrades)
                        item.Selected = false;
                }
                return _SalaryGrades;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> Ranks
        {
            get
            {
                if (_Ranks == null)
                {
                    var items = UnitOfWork.Rank.GetAll();
                    _Ranks = new List<System.Web.Mvc.SelectListItem>();
                    foreach (var item in items)
                        _Ranks.Add(new System.Web.Mvc.SelectListItem() { Text = item.Rank_Name, Value = item.Rank_Id.ToString() });
                }
                else
                {
                    foreach (var item in _Ranks)
                        item.Selected = false;
                }

                return _Ranks;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> SearchRanks
        {
            get
            {
                var items = UnitOfWork.Rank.GetAll();

                return
                    items.Select(
                        item =>
                            new System.Web.Mvc.SelectListItem() { Text = item.Rank_Name, Value = item.Rank_Id.ToString() })
                        .ToList();
            }
        }

        public static List<System.Web.Mvc.SelectListItem> DiagnosisStatus
        {
            get
            {
                if (_DiagnosisStatus == null)
                {
                    _DiagnosisStatus = new List<System.Web.Mvc.SelectListItem>();
                    _DiagnosisStatus.Add(new System.Web.Mvc.SelectListItem() { Text = "Active", Value = "1" });
                    _DiagnosisStatus.Add(new System.Web.Mvc.SelectListItem() { Text = "Stable", Value = "2" });
                    _DiagnosisStatus.Add(new System.Web.Mvc.SelectListItem() { Text = "Declining", Value = "3" });
                    _DiagnosisStatus.Add(new System.Web.Mvc.SelectListItem() { Text = "Acute", Value = "4" });
                    _DiagnosisStatus.Add(new System.Web.Mvc.SelectListItem() { Text = "End Stage", Value = "5" });
                    _DiagnosisStatus.Add(new System.Web.Mvc.SelectListItem() { Text = "Resolved", Value = "6" });
                }
                else
                {
                    foreach (var item in _DiagnosisStatus)
                        item.Selected = false;
                }

                return _DiagnosisStatus;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> Region
        {
            get
            {
                if (_Region != null && (_Region != null || _Region.Count > 0)) return _Region;


                var items = UnitOfWork.Region.GetAll();

                if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion) ||
                    PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince) ||
                    PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                {
                    items = items.Where(a => a.Reg_Id == CurrentUser.RegionID);
                }

                _Region = new List<System.Web.Mvc.SelectListItem>();
                foreach (var item in items)
                    _Region.Add(new System.Web.Mvc.SelectListItem()
                    {
                        Text = item.Reg_Title,
                        Value = item.Reg_Id.ToString()
                    });

                return _Region;
            }
        }


        public static List<System.Web.Mvc.SelectListItem> InventoryRegion
        {
            get
            {
                var items = UnitOfWork.Region.GetAll();

                if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion) ||
                  PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince) ||
                  PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                {
                    items = items.Where(a => a.Reg_Id == CurrentUser.RegionID);
                }

                return
                    items.Select(item => new SelectListItem() { Text = item.Reg_Title, Value = item.Reg_Id.ToString() })
                        .ToList();
            }
        }

        public static List<SelectListItem> SearchRegion
        {
            get
            {
                var items = UnitOfWork.Region.GetAll();

                if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion) ||
                    PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince) ||
                    PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                {
                    items = items.Where(a => a.Reg_Id == CurrentUser.RegionID);
                }

                return
                    items.Select(item => new SelectListItem() {Text = item.Reg_Title, Value = item.Reg_Id.ToString()})
                        .ToList();
            }
        }

        public static List<SelectListItem> FireMarshall
        {
            get
            {
                //if (_FireMarshall == null)
                //{
                var items = UnitOfWork.Employee.GetEmployeeList();
                _FireMarshall = new List<SelectListItem>();
                foreach (var item in items)
                    _FireMarshall.Add(new SelectListItem() { Text = item.Emp_Curr_Unit_Fullname, Value = item.Emp_Id });
                //}

                return _FireMarshall;
            }
        }

        public static List<SelectListItem> FireMarshallById(int empId)
        {
            var fireMarshall = new List<SelectListItem>();

            if (empId == 0)
                return fireMarshall;

            var empDetails = UnitOfWork.Employee.EmployeeDetails(empId);
            if (empDetails != null)
            {
                fireMarshall.Add(new SelectListItem() { Text = empDetails.Emp_Rank_Txt + " " + empDetails.Emp_FirstName + " " + empDetails.Emp_MiddleName + " " + empDetails.Emp_LastName, Value = empDetails.Emp_Id.ToString() });

            }

            return fireMarshall;
        }

        public static List<SelectListItem> FireMarshallReportById(int empId)
        {
            var fireMarshall = new List<SelectListItem>();

            if (empId == 0)
                return fireMarshall;

            var empDetails = UnitOfWork.Employee.EmployeeDetails(empId);
            if (empDetails != null)
            {
                fireMarshall.Add(new SelectListItem() { Text = empDetails.Emp_Rank_Txt + " " + empDetails.Emp_FirstName + " " + empDetails.Emp_MiddleName + " " + empDetails.Emp_LastName, Value = empDetails.Emp_Id.ToString() });

            }

            return fireMarshall;
        }

        public static List<SelectListItem> DirectoratesList(int dirId)
        {
            var directorates = new List<SelectListItem>();

            if (dirId == 0)
                return directorates;

            var dirDetails = UnitOfWork.Directorates.GetDirectoratesById(dirId);
            if (dirDetails != null)
            {
                directorates.Add(new SelectListItem() { Text = dirDetails.Dir_Name, Value = dirDetails.Dir_Id.ToString() });

            }

            return directorates;
        }

        public static List<SelectListItem> Directorates
        {
            get
            {
                if (_Directorates != null && (_Directorates != null || _Directorates.Count > 0)) return _Directorates;


                var items = UnitOfWork.Directorates.GetAll();



                _Directorates = new List<SelectListItem>();
                foreach (var item in items)
                    _Directorates.Add(new SelectListItem()
                    {
                        Text = item.Dir_Name,
                        Value = item.Dir_Id.ToString()
                    });

                return _Directorates;
            }
        }

        public static List<SelectListItem> InventoryGroupList(int igId)
        {
            var inventoryGroup = new List<SelectListItem>();

            if (igId == 0)
                return inventoryGroup;

            var igDetails = cisUnitOfWork.InventoryGroup.GetInventoryGroupById(igId);
            if (igDetails != null)
            {
                inventoryGroup.Add(new SelectListItem() { Text = igDetails.IG_Description, Value = igDetails.IG_Id.ToString() });

            }

            return inventoryGroup;
        }

        public static List<SelectListItem> InventoryGroup
        {
            get
            {
                if (_InventoryGroup != null && (_InventoryGroup != null || _InventoryGroup.Count > 0)) return _InventoryGroup;


                var items = cisUnitOfWork.InventoryGroup.GetAll();



                _InventoryGroup = new List<SelectListItem>();
                foreach (var item in items)
                    _InventoryGroup.Add(new SelectListItem()
                    {
                        Text = item.IG_Description,
                        Value = item.IG_Id.ToString()
                    });

                return _InventoryGroup;
            }
        }

        public static List<SelectListItem> InventoryArticleList(int iaId)
        {
            var inventoryArticle = new List<SelectListItem>();

            if (iaId == 0)
                return inventoryArticle;

            var iaDetails = cisUnitOfWork.InventoryArticle.GetInventoryArticleById(iaId);
            if (iaDetails != null)
            {
                inventoryArticle.Add(new SelectListItem() { Text = iaDetails.Art_Name, Value = iaDetails.Art_Id.ToString() });

            }

            return inventoryArticle;
        }

        public static List<SelectListItem> InventoryArticle
        {
            get
            {
                if (_InventoryArticle != null && (_InventoryArticle != null || _InventoryArticle.Count > 0)) return _InventoryArticle;


                var items = cisUnitOfWork.InventoryArticle.GetAll();



                _InventoryArticle = new List<SelectListItem>();
                foreach (var item in items)
                    _InventoryArticle.Add(new SelectListItem()
                    {
                        Text = item.Art_Name,
                        Value = item.Art_Id.ToString()
                    });

                return _InventoryArticle;
            }
        }

        public static List<SelectListItem> PhysicalInventoryList(int id)
        {
            var physicalInventory = new List<SelectListItem>();

            if (id == 0)
                return physicalInventory;

            var igDetails = cisUnitOfWork.PhysicalInventory.GetPhysicalInventoryById(id);
            if (igDetails != null)
            {
                physicalInventory.Add(new SelectListItem() { Text = "[" + igDetails.PI_PropertyNumber + "]-" + igDetails.PI_Description, Value = igDetails.PI_Id.ToString() });

            }

            return physicalInventory;
        }
        public static List<SelectListItem> UserRoles
        {
            get
            {
                var items = aUnitOfWork.UserInRole.GetUserRoleList();

                if (CurrentUser.RoleName == "MAIN ADMIN")
                {
                    return items.Select(item => new SelectListItem()
                    {
                        Text = item.RoleName,
                        Value = item.RoleId.ToString()
                    }).ToList();
                }

                if (CurrentUser.RoleName == "NATIONAL HRIS ADMIN")
                {
                    items = items.Where(a => (!a.RoleName.Contains("MAIN ADMIN") && !a.RoleName.Contains("NATIONAL HRIS ADMIN"))
                    && (a.RoleName.Contains("HRIS ADMIN") || a.RoleName.Contains("Employee"))
                    ).ToList();
                }
                else if (CurrentUser.RoleName == "NATIONAL FIRE CODE REVENUE ADMIN")
                {
                    items = items.Where(a => (!a.RoleName.Contains("MAIN ADMIN") && !a.RoleName.Contains("NATIONAL FIRE CODE REVENUE ADMIN"))
                   && (a.RoleName.Contains("FIRE CODE REVENUE ADMIN") || a.RoleName.Contains("Employee"))
                   ).ToList();
                }
                else if (CurrentUser.RoleName == "NATIONAL FIRE INSPECTION ADMIN")
                {
                    items = items.Where(a => (!a.RoleName.Contains("MAIN ADMIN") && !a.RoleName.Contains("NATIONAL FIRE INSPECTION ADMIN"))
                   && (a.RoleName.Contains("FIRE INSPECTION ADMIN") || a.RoleName.Contains("Employee"))
                   ).ToList();
                }
                else if (CurrentUser.RoleName == "NATIONAL FIRE SUPPRESSION ADMIN")
                {
                    items = items.Where(a => (!a.RoleName.Contains("MAIN ADMIN") && !a.RoleName.Contains("NATIONAL FIRE SUPPRESSION ADMIN"))
                   && (a.RoleName.Contains("FIRE SUPPRESSION ADMIN") || a.RoleName.Contains("Employee"))
                   ).ToList();
                }
                else if (CurrentUser.RoleName == "NATIONAL INVENTORY AND CAPABILITY ADMIN")
                {
                    items = items.Where(a => (!a.RoleName.Contains("MAIN ADMIN") && !a.RoleName.Contains("NATIONAL INVENTORY AND CAPABILITY ADMIN"))
                   && (a.RoleName.Contains("INVENTORY AND CAPABILITY ADMIN") || a.RoleName.Contains("Employee"))
                   ).ToList();
                }
                else if (CurrentUser.RoleName == "REGIONAL ADMIN")
                {
                    items =
                        items.Where(
                            a =>
                                (a.RoleName.Contains("REGIONAL") || a.RoleName.Contains("PROVINCIAL") ||
                                a.RoleName.Contains("FIRE STATION") || a.RoleName.Contains("Employee")) && a.RoleName != "REGIONAL ADMIN").ToList();
                }
                else if (CurrentUser.RoleName == "REGIONAL HRIS ADMIN")
                {
                    items =
                        items.Where(
                            a => a.RoleName == "Employee" || a.RoleName == "PROVINCIAL HRIS ADMIN" || a.RoleName == "FIRE STATION HRIS ADMIN").ToList();
                }
                else if (CurrentUser.RoleName == "REGIONAL FIRE CODE REVENUE ADMIN")
                {
                    items =
                        items.Where(
                            a => a.RoleName == "Employee" || a.RoleName == "PROVINCIAL FIRE CODE REVENUE ADMIN" || a.RoleName == "FIRE STATION FIRE CODE REVENUE ADMIN").ToList();
                }
                else if (CurrentUser.RoleName == "REGIONAL FIRE SUPPRESSION ADMIN")
                {
                    items =
                        items.Where(
                            a => a.RoleName == "Employee" || a.RoleName == "PROVINCIAL FIRE SUPPRESSION ADMIN" || a.RoleName == "FIRE STATION FIRE SUPPRESSION ADMIN").ToList();
                }
                else if (CurrentUser.RoleName == "REGIONAL INVENTORY AND CAPABILITY ADMIN")
                {
                    items =
                        items.Where(
                            a => a.RoleName == "Employee" || a.RoleName == "PROVINCIAL INVENTORY AND CAPABILITY ADMIN" || a.RoleName == "FIRE STATION INVENTORY AND CAPABILITY ADMIN").ToList();
                }
               
                else if (CurrentUser.RoleName == "PROVINCIAL ADMIN")
                {
                    items =
                        items.Where(a => a.RoleName.Contains("PROVINCIAL") || a.RoleName.Contains("FIRE STATION") || a.RoleName.Contains("Employee"))
                            .ToList();
                }

                else if (CurrentUser.RoleName == "PROVINCIAL HRIS ADMIN")
                {
                    items =
                        items.Where(
                            a => a.RoleName == "Employee" || a.RoleName == "FIRE STATION HRIS ADMIN").ToList();
                }
                else if (CurrentUser.RoleName == "PROVINCIAL FIRE CODE REVENUE ADMIN")
                {
                    items =
                        items.Where(
                            a => a.RoleName == "Employee" || a.RoleName == "FIRE STATION FIRE CODE REVENUE ADMIN").ToList();
                }
                else if (CurrentUser.RoleName == "PROVINCIAL FIRE SUPPRESSION ADMIN")
                {
                    items =
                        items.Where(
                            a => a.RoleName == "Employee" || a.RoleName == "FIRE STATION FIRE SUPPRESSION ADMIN").ToList();
                }
                else if (CurrentUser.RoleName == "PROVINCIAL INVENTORY AND CAPABILITY ADMIN")
                {
                    items =
                        items.Where(
                            a => a.RoleName == "Employee" || a.RoleName == "FIRE STATION INVENTORY AND CAPABILITY ADMIN").ToList();
                }

                else if (CurrentUser.RoleName == "FIRE STATION ADMIN")
                {
                    items = items.Where(a => a.RoleName.Contains("FIRE STATION") || a.RoleName.Contains("Employee")).ToList();
                }

                else if (CurrentUser.RoleName == "FIRE STATION HRIS ADMIN")
                {
                    items =
                        items.Where(
                           a => a.RoleName == "Employee").ToList();
                }
                else if (CurrentUser.RoleName == "FIRE STATION FIRE CODE REVENUE ADMIN")
                {
                    items =
                        items.Where(
                          a => a.RoleName == "Employee").ToList();
                }
                else if (CurrentUser.RoleName == "FIRE STATION FIRE SUPPRESSION ADMIN")
                {
                    items =
                        items.Where(
                          a => a.RoleName == "Employee").ToList();
                }
                else if (CurrentUser.RoleName == "FIRE STATION INVENTORY AND CAPABILITY ADMIN")
                {
                    items =
                        items.Where(
                            a => a.RoleName == "Employee").ToList();
                }
                else
                    return new List<SelectListItem>();

                return items.Select(item => new SelectListItem()
                {
                    Text = item.RoleName,
                    Value = item.RoleId.ToString()
                }).ToList();
            }
        }

        public static List<SelectListItem> RetiredEmployees
        {
            get
            {
                if (_RetiredEmployees == null)
                {
                    var items = UnitOfWork.Employee.GetRetiredEmployeeList();
                    _RetiredEmployees = new List<SelectListItem>();
                    foreach (var item in items)
                        _RetiredEmployees.Add(new SelectListItem() { Text = item.Emp_Curr_Unit_Fullname, Value = item.Emp_Id });
                }
                else
                {
                    foreach (var item in _RetiredEmployees)
                        item.Selected = false;
                }
                return _RetiredEmployees;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> GetProvince(int reg_id)
        {
            var items = UnitOfWork.Province.GetProvincePerRegion(reg_id);

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince) || PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                items = items.Where(a => a.Province_Id == CurrentUser.ProvinceID).ToList();

            _Province = new List<System.Web.Mvc.SelectListItem>();
            foreach (var item in items)
                _Province.Add(new System.Web.Mvc.SelectListItem() { Text = item.Province_Name, Value = item.Province_Id.ToString() });


            return _Province;
        }

        public static List<System.Web.Mvc.SelectListItem> EmployeeByUnitIdList(int curr_Unit, bool hasEmpty = false)
        {
           if(_EmployeeList == null)
               _EmployeeList = new List<SelectListItem>();

            if (_EmployeeList.Count == 0)
            {
                var items = UnitOfWork.Employee.GetEmployeeByCurrUnit(curr_Unit);
                if(hasEmpty)
                    _EmployeeList = new List<System.Web.Mvc.SelectListItem>();

                _EmployeeList.Add(new System.Web.Mvc.SelectListItem() { Text = "", Value = "" });
                foreach (var item in items)
                    _EmployeeList.Add(new System.Web.Mvc.SelectListItem() { Text = item.Emp_Rank_Txt?.ToUpper() + " " +  item.Emp_FirstName + " " + item.Emp_MiddleName + " " + item.Emp_LastName + " (" + item.Emp_Curr_PosDesignationTitle + ")", Value = item.Emp_Id.ToString() });
            }
            else
            {
                foreach (var item in _EmployeeList)
                    item.Selected = false;
            }
            return _EmployeeList;
        }

        public static List<System.Web.Mvc.SelectListItem> LeaveType
        {
            get
            {
                if (_LeaveType == null)
                {
                    _LeaveType = new List<System.Web.Mvc.SelectListItem>();
                    _LeaveType.Add(new System.Web.Mvc.SelectListItem() { Text = "Vacation", Value = "1" });
                    _LeaveType.Add(new System.Web.Mvc.SelectListItem() { Text = "Sick", Value = "2" });
                    _LeaveType.Add(new System.Web.Mvc.SelectListItem() { Text = "Maternity", Value = "3" });
                    _LeaveType.Add(new System.Web.Mvc.SelectListItem() { Text = "Paternity", Value = "4" });
                    _LeaveType.Add(new System.Web.Mvc.SelectListItem() { Text = "Mandatory", Value = "5" });
                    _LeaveType.Add(new System.Web.Mvc.SelectListItem() { Text = "Calamity", Value = "6" });
                    _LeaveType.Add(new System.Web.Mvc.SelectListItem() { Text = "Others", Value = "7" });
                }

                return _LeaveType;
            }
        }


        public static List<System.Web.Mvc.SelectListItem> DeductFrom
        {
            get
            {
                var deductFrom = new List<System.Web.Mvc.SelectListItem>();
                deductFrom.Add(new System.Web.Mvc.SelectListItem() { Text = "Vacation", Value = "1" });
                deductFrom.Add(new System.Web.Mvc.SelectListItem() { Text = "Sick", Value = "2" });

                return deductFrom;
            }
        }

        public static List<SelectListItem> GetDropDownListForYears
        {
            get
            {
                var ls = new List<SelectListItem>();

                for (var i = 1900; i <= 2099; i++)
                {
                    ls.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }

                return ls;
            }

        }

        public static List<SelectListItem> Months
        {
            get
            {
                if (_Month == null)
                {
                    _Month = new List<SelectListItem>();
                    _Month.Add(new SelectListItem() { Text = "January", Value = "1" });
                    _Month.Add(new SelectListItem() { Text = "February", Value = "2" });
                    _Month.Add(new SelectListItem() { Text = "March", Value = "3" });
                    _Month.Add(new SelectListItem() { Text = "April", Value = "4" });
                    _Month.Add(new SelectListItem() { Text = "May", Value = "5" });
                    _Month.Add(new SelectListItem() { Text = "June", Value = "6" });
                    _Month.Add(new SelectListItem() { Text = "July", Value = "7" });
                    _Month.Add(new SelectListItem() { Text = "August", Value = "8" });
                    _Month.Add(new SelectListItem() { Text = "September", Value = "9" });
                    _Month.Add(new SelectListItem() { Text = "October", Value = "10" });
                    _Month.Add(new SelectListItem() { Text = "November", Value = "11" });
                    _Month.Add(new SelectListItem() { Text = "December", Value = "12" });
                }

                return _Month;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> Classifications
        {
            get
            {
                if (_Classification == null)
                {
                    _Classification = Classification.Municipality.ToSelectList();
                }

                return _Classification;
            }
        }
        public static List<System.Web.Mvc.SelectListItem> IncomeClassList
        {
            get
            {
                if (_IncomeClass == null)
                {
                    _IncomeClass = IncomeClass.First.ToSelectList();
                }

                return _IncomeClass;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> StationCategory
        {
            get
            {
                var stationCategory = new List<System.Web.Mvc.SelectListItem>();
                stationCategory.Add(new System.Web.Mvc.SelectListItem() {Text = "Station", Value = "1"});
                stationCategory.Add(new System.Web.Mvc.SelectListItem() {Text = "Office", Value = "2"});
                
                return stationCategory;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> TruckModel
        {
            get
            {
                if (_TruckModel == null)
                {
                    var items = oUnitOfWork.TruckModel.GetList();
                    _TruckModel = new List<System.Web.Mvc.SelectListItem>();
                    foreach (var item in items)
                        _TruckModel.Add(new System.Web.Mvc.SelectListItem() { Text = item.TruckModel_Name, Value = item.TruckModel_Id.ToString() });
                }
                else
                {
                    foreach (var item in _TruckModel)
                        item.Selected = false;
                }
                return _TruckModel;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> StationName
        {
            get
            {
                if (_StationName == null)
                {
                    var items = UnitOfWork.Unit.GetUnitByMunicipality(CurrentUser.MunicipalityID);

                    if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                        items = items.Where(a => a.Unit_Id == CurrentUser.EmployeeUnitId).ToList();

                    _StationName = new List<System.Web.Mvc.SelectListItem>();
                    foreach (var item in items)
                        _StationName.Add(new System.Web.Mvc.SelectListItem()
                        {
                            Text = item.Unit_StationName,
                            Value = item.Unit_Id.ToString()
                        });
                }
                else
                {
                    foreach (var item in _StationName)
                        item.Selected = false;
                }
                return _StationName;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> SearchStationName
        {
            get
            {
                var unitId = CurrentUser.EmployeeUnitId;
                var municipalityId = UnitOfWork.Unit.GetUnitById(unitId);

                var items = UnitOfWork.Unit.GetUnitByMunicipality(municipalityId.Unit_Municipality_Id);
                var unitName = new List<System.Web.Mvc.SelectListItem>();
                foreach (var item in items)
                    unitName.Add(new System.Web.Mvc.SelectListItem() { Text = item.Unit_StationName, Value = item.Unit_Id.ToString() });

                return unitName;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> OVModel
        {
            get
            {
                if (_OVModel == null)
                {
                    var items = oUnitOfWork.OVModel.GetList();
                    _OVModel = new List<System.Web.Mvc.SelectListItem>();
                    foreach (var item in items)
                        _OVModel.Add(new System.Web.Mvc.SelectListItem() { Text = item.OVM_Name, Value = item.OVM_Id.ToString() });
                }
                else
                {
                    foreach (var item in _OVModel)
                        item.Selected = false;
                }
                return _OVModel;
            }
        }

        public static List<System.Web.Mvc.SelectListItem> Articles
        {
            get
            {
                if (_Articles == null)
                {
                    var items = cisUnitOfWork.InventoryArticle.GetAll();
                    _Articles = new List<System.Web.Mvc.SelectListItem>();
                    foreach (var item in items)
                        _Articles.Add(new System.Web.Mvc.SelectListItem() { Text = item.Art_Name, Value = item.Art_Id.ToString() });
                }
                else
                {
                    foreach (var item in _Articles)
                        item.Selected = false;
                }
                return _Articles;
            }
        }

        public static List<SelectListItem> TypeOfIndex
        {
            get
            {
                var indexType = new List<SelectListItem>();
                indexType.Add(new SelectListItem() { Text = "Index", Value = "Index" });
                indexType.Add(new SelectListItem() { Text = "Non-Index", Value = "Non-Index" });

                return indexType;
            }
        }

        public static List<SelectListItem> YearMonth
        {
            get
            {
                var indexType = new List<SelectListItem>();
                indexType.Add(new SelectListItem() { Text = "Year", Value = "Year" });
                indexType.Add(new SelectListItem() { Text = "Month", Value = "Month" });

                return indexType;
            }
        }
    }
}
