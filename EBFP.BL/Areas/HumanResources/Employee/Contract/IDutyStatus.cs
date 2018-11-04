
namespace EBFP.BL.HumanResources
{
    using DataAccess;
    using Queries.Core.Repositories;
    public interface IDutyStatus : IRepository<tblDutyStatus, DutyStatusModel>
    {
        
    }
}