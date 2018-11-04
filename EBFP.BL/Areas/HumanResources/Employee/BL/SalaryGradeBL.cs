namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;

    public class SalaryGradeBL : Repository<tblSalaryGrades, SalaryGradeModel>, ISalaryGrade
    {
        public SalaryGradeBL(EBFPEntities context) : base(context)
        {
        }
    } 
}
