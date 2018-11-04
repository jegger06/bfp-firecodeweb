
using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.HumanResources
{
    public interface ISalaryGrade : IRepository<tblSalaryGrades, SalaryGradeModel>
    { 
    }
}