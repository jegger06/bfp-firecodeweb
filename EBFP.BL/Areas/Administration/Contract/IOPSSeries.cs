
namespace EBFP.BL.Administration
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface IOPSSeries : IRepository<tblOPSSeries, OPSSeriesModel>
    {
        void SyncOPSSeriesLocalToServer(List<OPSSeriesModel> model);
        bool ReleasedOPS(string opsNumber, int unitId);
    }
}