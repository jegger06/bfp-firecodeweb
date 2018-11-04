
namespace EBFP.BL.HumanResources
{
    using DataAccess;
    using Queries.Core.Repositories;
    public interface IEligibility : IRepository<tblEligibilities, EligibilityModel>
    {
        string GetEligibilities(int eligibility_Id);
    }
}