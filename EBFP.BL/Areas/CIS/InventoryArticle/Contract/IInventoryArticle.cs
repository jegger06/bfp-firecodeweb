using System.Collections.Generic;
using EBFP.BL.CIS;
using EBFP.BL.Helper;

namespace EBFP.BL.CIS
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    public interface IInventoryArticle : IRepository<tblArticles, InventoryArticleModel>
    {
        InventoryArticleListResult GetListResult(GridInfo gridInfo);
        void UpdateInventoryArticle(InventoryArticleModel model);
        bool DeleteByID(int inventoryGroupId);
        bool InventoryArticleExists(string artCode, string artName, int artId = 0);
        bool CheckIfCurrentlyUsed(int iaId);
        InventoryArticleModel GetInventoryArticleById(int iaId);
    }
}
