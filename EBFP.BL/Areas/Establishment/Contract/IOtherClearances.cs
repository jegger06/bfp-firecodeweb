
namespace EBFP.BL.Establishment
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface IOtherClearances : IRepository<tblOtherClearances, OtherClearancesModel>
    {
        void SyncOtherClearancesEstLocalToServer(List<OtherClearancesModel> model);
    }
}