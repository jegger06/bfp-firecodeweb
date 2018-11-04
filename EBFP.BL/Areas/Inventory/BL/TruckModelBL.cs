using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.Inventory
{
    public class TruckModelBL : Repository<tblTruckModel, TruckModelList>, ITruckModel
    {
        public TruckModelBL(EBFPEntities context) : base(context)
        {
        }
    }
}