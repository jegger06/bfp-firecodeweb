
namespace EBFP.BL.InspectionOrder
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface IOtherViolations : IRepository<tblOtherViolations, OtherViolationModel>
    {
        void SyncOtherViolationsLocalToServer(List<OtherViolationModel> model);
    }
}