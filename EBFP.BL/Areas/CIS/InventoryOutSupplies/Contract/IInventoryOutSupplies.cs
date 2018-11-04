using System.Collections.Generic;
using EBFP.BL.CIS;
using EBFP.BL.Helper;

namespace EBFP.BL.CIS
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    public interface IInventoryOutSupplies : IRepository<tblSuppliesInventoryOut, InventoryOutSuppliesModel>
    {
        InventoryOutSuppliesListResult GetListResult(GridInfo gridInfo);
        void SaveOutSuppliesInventory(InventoryOutSuppliesModel model);
        bool DeleteByID(int inventoryGroupId);
    }
}
