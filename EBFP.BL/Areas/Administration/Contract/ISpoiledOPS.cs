
namespace EBFP.BL.Administration
{
    using EBFP.DataAccess;
    using Helper;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface ISpoiledOPS : IRepository<tblSpoiledOPS, SpoiledOPSModel>
    {
        SpoiledOPSListResult GetSpoiledOPSList(GridInfo gridInfo, int unitId);
    }
}