namespace EBFP.BL.HumanResources
{
    using DataAccess;
    using Helper;
    using Queries.Core.Repositories;
    using System.Threading.Tasks;

    public interface IMedical
    {
        Task<EmployeeMedicalModel> EmployeeMedicalDetails(int Emp_Id);
        EmployeeMedicalModel SaveMedicalDetails(EmployeeMedicalModel model);
    }
    
}