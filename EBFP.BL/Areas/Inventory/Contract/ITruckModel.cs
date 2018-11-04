using EBFP.DataAccess;

namespace EBFP.BL.Inventory
{
    using Queries.Core.Repositories;

    public interface ITruckModel : IRepository<tblTruckModel, TruckModelList>
    {
    }
}