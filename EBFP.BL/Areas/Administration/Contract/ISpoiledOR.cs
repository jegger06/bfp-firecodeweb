
namespace EBFP.BL.Administration
{
    using EBFP.DataAccess;
    using Helper;
    using Queries.Core.Repositories;

    public interface ISpoiledOR : IRepository<tblSpoiledOR, SpoiledORModel>
    {
        SpoiledORListResult GetSpoiledORList(GridInfo gridInfo, int unitId);
    }
}