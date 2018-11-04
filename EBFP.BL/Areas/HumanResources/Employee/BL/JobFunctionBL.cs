namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;

    public class JobFunctionBL : Repository<tblJobFunctions, JobFuntionModel>, IJobFunction
    {
        public JobFunctionBL(EBFPEntities context) : base(context)
        {
        }
    } 
}
