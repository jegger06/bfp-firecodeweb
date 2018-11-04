using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using EBFP.BL.Helper;
using EBFP.DataAccess;
using Queries.Core.Repositories;
using EBFP.Helper;

namespace EBFP.BL.HumanResources
{
    public class SLLBL : Repository<tblEmployees, EmployeeModel>, ISLL
    {
        public SLLBL(EBFPEntities context) : base(context)
        {
           
        }
        public SeniorityLinealListResult GetListResult(GridInfo gridInfo)
        {
            var rankList = BFPContext.tblRanks.ToList();

            var empList = BFPContext.spGetSeniorityLinealListing()
                .Select(emp => new SeniorityLinealModel
                {
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    RegionId = emp.RegionId,
                    RegionName = emp.RegionName,
                    ProvinceId = emp.ProvinceId,
                    ProvinceName = emp.ProvinceName,
                    UnitId = emp.UnitId.Value,
                    UnitName = emp.UnitName,
                    PresentRank = emp.PresentRank,
                    PresentRankName = emp.PresentRankName,
                    QualifiedRank = emp.QualifiedRank
                });

            if (!PageSecurity.HasAccess(PageArea.HRIS_SeniorityLineal_CanViewAll))
            {
                if (PageSecurity.HasAccess(PageArea.HRIS_SeniorityLineal_RestricttoRegion))
                {
                    empList = empList.Where(
                        a => a.RegionId == CurrentUser.RegionID);
                }

                if (PageSecurity.HasAccess(PageArea.HRIS_SeniorityLineal_RestricttoProvince))
                {
                    empList =
                        empList.Where(
                            a => a.ProvinceId == CurrentUser.ProvinceID);
                }

                if (PageSecurity.HasAccess(PageArea.HRIS_SeniorityLineal_RestricttoStation))
                {
                    empList = empList.Where(a => a.UnitId == CurrentUser.EmployeeUnitId);
                }
            }

            var search = gridInfo.searchSLLModel;
            if (!string.IsNullOrEmpty(search.FirstName))
                empList = empList.Where(a => a.FirstName.Contains(search.FirstName));
            if (!string.IsNullOrEmpty(search.LastName))
                empList = empList.Where(a => a.LastName.Contains(search.LastName));

            if (search.RegionId > 0)
                empList = empList.Where(a => a.RegionId == search.RegionId);
            if (search.ProvinceId > 0)
                empList = empList.Where(a => a.ProvinceId == search.ProvinceId);
            if (search.UnitId > 0)
                empList = empList.Where(a => a.UnitId == search.UnitId);
            if (search.PresentRank > 0)
                empList = empList.Where(a => a.PresentRank == search.PresentRank);
            if (search.QualifiedRankId > 0)
            {
                var rankName = rankList.FirstOrDefault(a => a.Rank_Id == search.QualifiedRankId);
                if(rankName != null)
                empList = empList.Where(a => a.QualifiedRank.Contains(rankName.Rank_Name));
            }
                
            var seniorityList = new List<SeniorityLinealModel>();

            var employeesResult = empList
           .Skip(gridInfo.start)
           .Take(gridInfo.length);

            foreach (var item in employeesResult.ToList())
                {
                  
                    var model = new SeniorityLinealModel
                    {
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        RegionId = item.RegionId,
                        RegionName = item.RegionName,
                        ProvinceId = item.ProvinceId,
                        ProvinceName = item.ProvinceName,
                        UnitName = item.UnitName,
                        PresentRankName = item.PresentRankName,
                        PresentRank = item.PresentRank,
                        QualifiedRank = item.QualifiedRank,
                    };
                    seniorityList.Add(model);
                }

            var resList = seniorityList.Where(a => a.QualifiedRank.Length > 0).AsQueryable()
            .OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
            .Skip(gridInfo.start)
            .Take(gridInfo.length);

            resList = resList.OrderByDescending(a => a.PresentRank);
            gridInfo.recordsTotal = resList.Count();

            return new SeniorityLinealListResult
            {
                SeniorityLinealList = resList.ToList(),
                DatatableInfo = gridInfo
            };
        }
        public string GetQualifiedRank(tblEmployees emp)
        {
            if (emp.Emp_Curr_Rank.HasValue)
            {
                var highestTraining = emp.Emp_HighestMandatoryTraining;
                var service = emp.tblEmployeeServiceAppointments.FirstOrDefault(a => a.ESA_Rank == emp.Emp_Curr_Rank);

                if (highestTraining.HasValue && highestTraining > 0 && service != null &&
                    emp.Emp_Curr_ApptStatus == (int)AppointmentStatuses.Permanent)

                {
                    switch (emp.tblRanks.Rank_Name)
                    {
                        case "NUP":
                            break;
                        case "FO1":
                            if (highestTraining >= (int)HighestMandatoryTraining.FBRC &&
                                IsQualifiedYearsInService(service))
                                return "FO2";
                            break;
                        case "FO2":
                            if (highestTraining >= (int)HighestMandatoryTraining.FBRC &&
                                IsQualifiedYearsInService(service))
                                return "FO3";
                            break;
                        case "FO3":
                            if (highestTraining >= (int)HighestMandatoryTraining.FAIIC &&
                                IsQualifiedYearsInService(service))
                                return "SFO1";
                            break;
                        case "SFO1":
                            if (highestTraining >= (int)HighestMandatoryTraining.FPSC &&
                                IsQualifiedYearsInService(service))
                                return "SFO2";
                            break;
                        case "SFO2":
                            if (highestTraining >= (int)HighestMandatoryTraining.FPSC &&
                                IsQualifiedYearsInService(service))
                                return "SFO3";
                            break;
                        case "SFO3":
                            if (highestTraining >= (int)HighestMandatoryTraining.FPSC &&
                                IsQualifiedYearsInService(service))
                                return "SFO4";
                            break;
                        case "SFO4":
                            if (highestTraining >= (int)HighestMandatoryTraining.OCC &&
                                IsQualifiedYearsInService(service))
                                return "INSP";
                            break;
                        case "INSP":
                            if (highestTraining >= (int)HighestMandatoryTraining.OBC &&
                                IsQualifiedYearsInService(service, 3) &&
                                HasMasteralDegree(emp))
                                return "SINSP";
                            break;
                        case "SINSP":
                            if (highestTraining >= (int)HighestMandatoryTraining.OAC &&
                                IsQualifiedYearsInService(service, 3) &&
                                HasMasteralDegree(emp))
                                return "CINSP";
                            break;
                        case "CINSP":
                            if (highestTraining >= (int)HighestMandatoryTraining.OAC &&
                                IsQualifiedYearsInService(service, 3) &&
                                HasMasteralDegree(emp))
                                return "SUPT";
                            break;
                        case "SUPT":
                            if (highestTraining >= (int)HighestMandatoryTraining.OSEC &&
                                IsQualifiedYearsInService(service, 3) &&
                                HasMasteralDegree(emp))
                                return "SSUPT";
                            break;
                        case "SSUPT":
                            if (highestTraining >= (int)HighestMandatoryTraining.OSEC &&
                                IsQualifiedYearsInService(service) &&
                                HasMasteralDegree(emp))
                                return "CSUPT";
                            break;
                        case "CSUPT":
                            if (highestTraining >= (int)HighestMandatoryTraining.OSEC &&
                                IsQualifiedYearsInService(service) &&
                                HasMasteralDegree(emp))
                                return "DIR";
                            break;
                        case "DIR":
                            break;
                        default:
                            break;
                    }
                }
            }


            return "";
        }

        public int CountYears(DateTime? start, DateTime? end)
        {
            if (start.HasValue && end.HasValue)
            {

                int age = end.Value.Year - start.Value.Year;
                if (start > end.Value.AddYears(-age))
                    age--;

                return age;
            }

            return 0;
        }

        public bool IsQualifiedYearsInService(tblEmployeeServiceAppointments service, int yearsNeeded = 2)
        {
            if (service != null)
            {
                var esaEndDate = service.ESA_EndDate != null ? service.ESA_EndDate : DateTime.Now;
                var yearsInService = CountYears(service.ESA_ApptDate, esaEndDate);
                if (yearsInService >= yearsNeeded &&
                    Convert.ToInt32(service.ESA_Appt_Status) == (int)AppointmentStatuses.Permanent)
                    return true;
            }

            return false;
        }
        public bool HasMasteralDegree(tblEmployees emp)
        {
            if (emp == null)
                return false;

            var degree = emp.Emp_HighestEducAttainment ?? 0;
            if (degree == (int)EducAttaintmentLevel.MasteralDegree)
                return true;

            var educBackground = emp.tblEmployeeEducationalBackground?
                .FirstOrDefault(a => a.EEB_EducType == (int)EducationLevel.GRADUATE_STUDIES);

            if (!string.IsNullOrWhiteSpace(educBackground?.EEB_HighestLevel) &&
                educBackground.EEB_HighestLevel.ToLower().Contains("units"))
            {
                var units = educBackground.EEB_HighestLevel.ToLower().Replace("units", "").Trim();
                int num;
                bool isNumeric = int.TryParse(units, out num);

                if (isNumeric && num >= 12)
                {
                    return true;
                }
            }

            return false;
        }
    }
}