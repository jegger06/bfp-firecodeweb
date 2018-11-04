
using EBFP.DataAccess;

namespace EBFP.BL.Inventory
{
    using Queries.Core.Repositories;

    public interface IOVModel : IRepository<tblOtherVehicleModel, OVModel>
    {
    }
}