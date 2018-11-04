namespace EBFP.BL.HumanResources
{
    using DataAccess;
    using Helper;
    using Queries.Core.Repositories;
    using System.Threading.Tasks;

    public interface IPhysicalExam
    {
        PhysicalExamModel SavePhysicalExamDetails(PhysicalExamModel model);
        PhysicalExamListResult GetPhysicalExams(GridInfo dataTableInfo);
        Task<PhysicalExamModel> PhysicalExamDetails(int PE_Id);
        bool DeleteByPEID(int physicalExamId);
    }
    
}