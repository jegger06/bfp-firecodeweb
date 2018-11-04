using System.Collections.Generic;
using EBFP.BL.CIS;
using EBFP.BL.Helper;

namespace EBFP.BL.CIS
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    public interface IInventorySupplies : IRepository<tblSuppliesInventory, InventorySuppliesModel>
    {
        InventorySuppliesListResult GetListResult(GridInfo gridInfo);
        void SaveSuppliesInventory(InventorySuppliesModel model);
        bool DeleteByID(int inventoryGroupId);
    }
}
