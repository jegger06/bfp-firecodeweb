using EBFP.BL.Helper;
using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.CIS
{
    public interface IPhysicalInventory : IRepository<tblPhysicalInventory, PhysicalInventoryModel>
    {
        PhysicalInventoryListResult GetListResult(GridInfo gridInfo);
        void UpdatePhysicalInventory(PhysicalInventoryModel model);
        bool DeleteByID(int inventoryGroupId);
        bool PhysicalInventoryExists(string propertyNumber, int id = 0);
        PhysicalInventoryModel GetPhysicalInventoryById(int physicalInventoryId);
        PhysicalInventoryListResult GetUnserviceablePIListResult(GridInfo gridInfo);
    }
}