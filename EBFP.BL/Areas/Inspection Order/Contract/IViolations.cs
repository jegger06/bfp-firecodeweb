
namespace EBFP.BL.InspectionOrder
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface IViolations : IRepository<tblViolations, ViolationModel>
    {
        void SyncViolationsLocalToServer(List<ViolationModel> model);
    }
}