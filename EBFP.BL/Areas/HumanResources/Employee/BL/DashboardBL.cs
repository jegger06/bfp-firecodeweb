using System;
using EBFP.BL.Helper;
using EBFP.DataAccess;
using System.Collections.Generic;
using System.Linq;
using EBFP.Helper;

namespace EBFP.BL.HumanResources
{
    public class DashboardBL : EntityFrameworkBase , IDashboard
    {
        public DashboardBL(EBFPEntities _context)
        {
            base.context_ = _context;
        }
        
        public List<HRISChartModel> GetDashboardDetails()
        {
            var ranks = context.tblRanks.ToList();
            return ranks.Select(rank => new HRISChartModel
            {
                RankName = rank.Rank_Name,
                FemaleCount = GetGenderCount("F", rank.Rank_Id),
                MaleCount = GetGenderCount("M", rank.Rank_Id),
            }).ToList();
        }

        private int GetGenderCount(string gender,int rankId)
        {
            var genderCount = context.tblEmployees.Count(a =>  a.Emp_Gender == gender && a.Emp_Curr_Rank == rankId && a.Emp_DutyStatus == (int) DutyStatuses.Active && a.Emp_IsDeleted == false);

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
            {
                genderCount = context.tblEmployees.Count(a => a.Emp_Gender == gender && a.Emp_Curr_Rank == rankId && a.Emp_DutyStatus == (int) DutyStatuses.Active &&
                            a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID && a.Emp_IsDeleted == false);
            }
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
            {
                genderCount = context.tblEmployees.Count(a => a.Emp_Gender == gender && a.Emp_Curr_Rank == rankId && a.Emp_DutyStatus == (int)DutyStatuses.Active &&
                            a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == CurrentUser.ProvinceID && a.Emp_IsDeleted == false);
            }
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
            {
                genderCount =context.tblEmployees.Count(a => a.Emp_Gender == gender && a.Emp_Curr_Rank == rankId && a.Emp_DutyStatus == (int)DutyStatuses.Active &&
                            a.Emp_Curr_Unit == CurrentUser.EmployeeUnitId && a.Emp_IsDeleted == false);
            }

            return genderCount;
        }

        public DashboardCounterTotal GetHRISDashboardCounter()
        {
            var unit = context.tblUnits.Count();
            var totalStrength = context.tblEmployees.Count(a => a.Emp_DutyStatus == (int) DutyStatuses.Active && a.Emp_IsDeleted == false);
            var female =
                context.tblEmployees.Count(a => a.Emp_Gender == "F" && a.Emp_DutyStatus == (int) DutyStatuses.Active && a.Emp_IsDeleted == false);
            var male =
                context.tblEmployees.Count(a => a.Emp_Gender == "M" && a.Emp_DutyStatus == (int) DutyStatuses.Active && a.Emp_IsDeleted == false);

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
            {
                unit = context.tblUnits.Count(a => a.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID);
                totalStrength = context.tblEmployees.Count(a => a.Emp_DutyStatus == (int)DutyStatuses.Active && a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID && a.Emp_IsDeleted == false);
                female = context.tblEmployees.Count(a => a.Emp_Gender == "F" && a.Emp_DutyStatus == (int)DutyStatuses.Active && a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID && a.Emp_IsDeleted == false);
                male = context.tblEmployees.Count(a => a.Emp_Gender == "M" && a.Emp_DutyStatus == (int)DutyStatuses.Active && a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID && a.Emp_IsDeleted == false);
            }
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
            {
                unit = context.tblUnits.Count(a => a.tblCityMunicipality.tblProvinces.Province_Id == CurrentUser.ProvinceID);
                totalStrength = context.tblEmployees.Count(a => a.Emp_DutyStatus == (int)DutyStatuses.Active && a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == CurrentUser.ProvinceID && a.Emp_IsDeleted == false);
                female = context.tblEmployees.Count(a => a.Emp_Gender == "F" && a.Emp_DutyStatus == (int)DutyStatuses.Active && a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == CurrentUser.ProvinceID && a.Emp_IsDeleted == false);
                male = context.tblEmployees.Count(a => a.Emp_Gender == "M" && a.Emp_DutyStatus == (int)DutyStatuses.Active && a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == CurrentUser.ProvinceID && a.Emp_IsDeleted == false);
            }
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
            {
                unit = context.tblUnits.Count(a => a.Unit_Id == CurrentUser.EmployeeUnitId);
                totalStrength = context.tblEmployees.Count(a => a.Emp_DutyStatus == (int)DutyStatuses.Active && a.Emp_Curr_Unit == CurrentUser.EmployeeUnitId && a.Emp_IsDeleted == false);
                female = context.tblEmployees.Count(a => a.Emp_Gender == "F" && a.Emp_DutyStatus == (int)DutyStatuses.Active && a.Emp_Curr_Unit == CurrentUser.EmployeeUnitId && a.Emp_IsDeleted == false);
                male = context.tblEmployees.Count(a => a.Emp_Gender == "M" && a.Emp_DutyStatus == (int)DutyStatuses.Active && a.Emp_Curr_Unit == CurrentUser.EmployeeUnitId && a.Emp_IsDeleted == false);
            }

            var model = new DashboardCounterTotal
            {
                Unit = unit,
                TotalStrength = totalStrength,
                Female = female,
                Male = male
            };

            return model;
        }

        public List<DutyStatusChartModel> GetDutyStatusChartDetails()
        {
            var dutyStatuses = Enum.GetValues(typeof(DutyStatuses)).Cast<DutyStatuses>().Cast<int>().ToList();

            return dutyStatuses.Select(dutyStatus => new DutyStatusChartModel
            {           
                Duty_Status = ((DutyStatuses)dutyStatus).ToDescription(),
                DutyStatusCount = GetDutyStatusCount(dutyStatus),
            }).ToList();
        }

        private int GetDutyStatusCount(int dutystatus)
        {
            var dutyStatusCount = context.tblEmployees.Count(a => a.Emp_DutyStatus == dutystatus && a.Emp_IsDeleted == false);

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
            {
                dutyStatusCount = context.tblEmployees.Count(a => a.Emp_DutyStatus == dutystatus &&
                            a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID && a.Emp_IsDeleted == false);
            }
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
            {
                dutyStatusCount = context.tblEmployees.Count(a => a.Emp_DutyStatus == dutystatus &&
                            a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == CurrentUser.ProvinceID && a.Emp_IsDeleted == false);
            }
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
            {
                dutyStatusCount = context.tblEmployees.Count(a => a.Emp_DutyStatus == dutystatus &&
                             a.Emp_Curr_Unit == CurrentUser.EmployeeUnitId && a.Emp_IsDeleted == false);
            }

            return dutyStatusCount;
        }
    }


}
