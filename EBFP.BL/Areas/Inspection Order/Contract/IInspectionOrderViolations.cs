
namespace EBFP.BL.InspectionOrder
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface IInspectionOrderViolations : IRepository<tblInspectionOrderViolations, InspectionOrderViolationModel>
    {
        void SyncInspectionOrderViolationsLocalToServer(List<InspectionOrderViolationModel> model);
    }
}