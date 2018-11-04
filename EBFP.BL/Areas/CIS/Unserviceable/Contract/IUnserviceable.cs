using EBFP.BL.Helper;
using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.CIS
{
    public interface IUnserviceable : IRepository<tblUnserviceablePhysicalInventory, UnserviceableModel>
    {
        UnserviceableListResult GetListResult(GridInfo gridInfo);
        void UpdateUnserviceable(UnserviceableModel model);
        bool DeleteByID(int unserviceableId);
        UnserviceableModel GetUnserviceableById(int unserviceableId);
        bool UnserviceableExists(string wmr, int id = 0);
        void UpdateToUnserviceable(UnserviceableModel model);
        int Add(UnserviceableModel model);
    }
}