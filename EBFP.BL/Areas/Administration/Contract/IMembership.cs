
namespace EBFP.BL.Administration
{
    using DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface IMembership :  IRepository<webpages_Membership, MembershipModel>
    {
        List<MembershipModel> GetMembershipByUnitId(int unitId);
        void SyncMembership(List<MembershipModel> model);
    }
}