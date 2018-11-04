
namespace EBFP.BL.InspectionOrder
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface INTCDeficiencies : IRepository<tblNoticeToComplyDeficiencies, NTCDeficienciesModel>
    {
        void SyncNTCDeficienciesLocalToServer(List<NTCDeficienciesModel> model);
    }
}