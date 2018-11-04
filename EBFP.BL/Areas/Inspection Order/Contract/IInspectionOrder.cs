
using EBFP.BL.Helper;

namespace EBFP.BL.InspectionOrder
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface IInspectionOrder : IRepository<tblInspectionOrders, InspectionOrderModel>
    {
        void SyncInspectionOrderLocalToServer(List<InspectionOrderModel> model);
        InspectionOrderListResult GetListResult(GridInfo gridInfo);
        InspectionOrderModel GetInspectionOrderById(int inspectionOrderId);
    }
}