using System.Collections.Generic;
using EBFP.BL.CIS;
using EBFP.BL.Helper;

namespace EBFP.BL.CIS
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    public interface IInventoryGroup : IRepository<tblInventoryGroups, InventoryGroupModel>
    {
        InventoryGroupListResult GetListResult(GridInfo gridInfo);
        void UpdateInventoryGroup(InventoryGroupModel model);
        bool DeleteByID(int inventoryGroupId);
        bool InventoryGroupExists(string igCode, string igDescription, int id = 0);
        bool CheckIfCurrentlyUsed(int igId);
        InventoryGroupModel GetInventoryGroupById(int igId);
    }
}
