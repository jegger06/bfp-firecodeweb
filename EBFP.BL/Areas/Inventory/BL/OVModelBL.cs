using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.Inventory
{
    public class OVModelBL : Repository<tblOtherVehicleModel, OVModel>, IOVModel
    {
        public OVModelBL(EBFPEntities context) : base(context)
        {
        }
    }
}