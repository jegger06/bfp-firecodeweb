
namespace EBFP.BL.InspectionOrder
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface IInspectionOrderInspectors : IRepository<tblInspectionOrderInspectors, InspectionOrderInspectorsModel>
    {
        void SyncInspectionOrderInspectorsLocalToServer(List<InspectionOrderInspectorsModel> model);
    }
}