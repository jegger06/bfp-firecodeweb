using System.Collections.Generic;
using System.Data;
using EBFP.BL.CIS;

namespace EBFP.BL.HumanResources
{
    using DataAccess;
    using Helper;
    using Queries.Core.Repositories;
    using System.Threading.Tasks;
    using System.Web;

    public interface IEmployee : IRepository<tblEmployees, EmployeeModel>
    {
        List<EmployeeModel> GetEmployeeByUnitId(int unitId);
        EmployeeModel GetEmployeeByUserName(string userName);
        EmployeeListResult GetEmployees(GridInfo dataTableInfo);
        EmployeeModel EmployeeDetails(int Emp_Id);
        EmployeeModel SaveEmployeeDetails(EmployeeModel model, bool editProfile);
        List<EmployeeModel> GetEmployeeByCurrUnit(int curr_Unit);
        List<EmployeeListModel> GetEmployeeList();
        bool DeleteByID(int employeeID);
        EmployeeModel GetEmployeeById(int Emp_Id);
        tblEmployees GetEmployeeByID(int Emp_Id);
        List<EmployeeListModel> GetRetiredEmployeeList();
        void UpdatePersonalInformation(EmployeeModel model);
        List<EmployeeModel> EmployeeWithRankSelection(string search);
        byte[] GetImage(int empId);
        List<EmployeeModel> GetEmployees();
        void UpdateByAlphaList(HttpPostedFileBase file, ref List<string> newUsers);
        PersonnelStrengthModel GetPersonnelStrenght();
        List<EmployeeModel> GetAlphaList(EmployeeSearchModel searchModel,string fileName);
        string GetPositionTitle(int empId);
        void UpdateValidatedEmp(string type, int empId, int validatedBy);
        string GetEmployeeNameById(int empId);
        bool CheckRank(int rankId);
    }

    public interface IEducationalBackground : IRepository<tblEmployeeEducationalBackground, EducBackgroundModel>
    {
        void InsertBulk(List<EducBackgroundModel> EducBackgroundModel, int Emp_Id);
    }

    public interface ICivilServiceEligibility : IRepository<tblEmployeeCivilServiceEligibilities, CivilServiceEligibilityModel>
    {
        void InsertBulk(List<CivilServiceEligibilityModel> model, int Emp_Id);
    }

    public interface IOtherInformation : IRepository<tblEmployeeOtherInformation, OtherInformationModel>
    {
        OtherInformationModel GetByEmployee(int employeeID);
        void Insert(OtherInformationModel model, int employeeID);
    }

    public interface ITrainingProgram : IRepository<tblEmployeeTrainingPrograms, TrainingProgramModel>
    {
        void InsertBulk(List<TrainingProgramModel> model, int Emp_Id);
    }

    public interface IVoluntaryWork : IRepository<tblEmployeeVoluntaryWorks, VoluntaryWorkModel>
    {
        void InsertBulk(List<VoluntaryWorkModel> model, int Emp_Id);
    }

    public interface IWorkExperience : IRepository<tblEmployeeWorkExperiences, WorkExperienceModel>
    {
       void InsertBulk(List<WorkExperienceModel> model, int Emp_Id);
    }

    public interface ISpecialSkillsHobby : IRepository<tblEmployeeSpecialSkillsHobbies, SpecialSkillsHobbyModel>
    {
        void InsertBulk(List<SpecialSkillsHobbyModel> model, int Emp_Id);
    }

    public interface INonAcademicDistinction : IRepository<tblEmployeeNonAcademicDistinctions, NonAcademicDistinctionModel>
    {
        void InsertBulk(List<NonAcademicDistinctionModel> model, int Emp_Id);
    }

    public interface IMembershipInAssociationOrganization : IRepository<tblEmployeeMembershipInAssociationOrganizations, MembershipInAssociationOrganizationModel>
    {
        void InsertBulk(List<MembershipInAssociationOrganizationModel> model, int Emp_Id);
    }

    public interface ISpecifyDesignation : IRepository<tblEmployeeSpecifyDesignation, SpecifyDesignationModel>
    {
        void InsertBulk(List<SpecifyDesignationModel> model, int Emp_Id);
    }
}