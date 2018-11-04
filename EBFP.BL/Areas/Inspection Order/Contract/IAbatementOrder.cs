
namespace EBFP.BL.InspectionOrder
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface IAbatementOrder : IRepository<tblAbatementOrder, AbatementOrderModel>
    {
        void SyncAbatementOrderLocalToServer(List<AbatementOrderModel> model);
    }
}