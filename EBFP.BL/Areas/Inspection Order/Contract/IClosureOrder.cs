
namespace EBFP.BL.InspectionOrder
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface IClosureOrder : IRepository<tblClosureOrder, ClosureOrderModel>
    {
        void SyncClosureOrderLocalToServer(List<ClosureOrderModel> model);
    }
}