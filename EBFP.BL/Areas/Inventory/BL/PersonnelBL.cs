using System.Linq;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.BL.HumanResources;
using EBFP.DataAccess;
using EBFP.Helper;

namespace EBFP.BL.Inventory
{
    public class PersonnelBL : EntityFrameworkBase, IPersonnel
    {
        public PersonnelBL(EBFPEntities _context)
        {
            context_ = _context;
        }

        public PersonnelListResult GetPersonneListResult(GridInfo gridInfo)
        {
            var SearchTerms = gridInfo.searchPersonnelModel;
            var employees = context.tblEmployees.Where(a => a.Emp_IsDeleted == false);


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

            if (SearchTerms.Municipality_Id > 0)
                employees = employees.Where(a => a.tblUnits.Unit_Municipality_Id == SearchTerms.Municipality_Id);
            
            if (SearchTerms.Station_Id > 0)
                employees = employees.Where(a => a.Emp_Curr_Unit == SearchTerms.Station_Id && a.Emp_SubStation_Id == null);

     
            gridInfo.recordsTotal = employees.Select(a => a.Emp_Id).Count();
            var employeesResult = employees.OrderByDescending(a => a.tblRanks.Rank_Id)
                .Skip(gridInfo.start)
                .Take(gridInfo.length)
                .ToList();

            var unitOfWork = new HRISUnitOfWork();
            var retEmps = employeesResult.Select(employee => new PersonnelModel
            {
                Rank_Name = employee.Emp_Curr_Rank.ToNullSafeString().ToRankFullName(),
                Full_Name = employee.Emp_FirstName + " " + employee.Emp_LastName,
                Municipality_Id = employee.tblUnits?.Unit_Municipality_Id ?? 0,
                Station_Id = employee.Emp_Curr_Unit ?? 0,
                Contact_Number = employee.Emp_MobileNumber,
                Email = employee.Emp_EmailAddress,
                //Specific_Designation = employee.Emp_Curr_PosDesignationTitle,
                Specific_Designation = unitOfWork.Employee.GetPositionTitle(employee.Emp_Id),
                Present_Designation = employee.Emp_Curr_JobFunc.ToNullSafeString().ToPresentDesginationFullName()
            }).ToList();

            return new PersonnelListResult
            {
                PersonnelListModel = retEmps,
                DatatableInfo = gridInfo
            };
        }
    }
}