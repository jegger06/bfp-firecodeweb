
namespace EBFP.BL.InspectionOrder
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface INTCVDeficiencies : IRepository<tblNoticeToCorrectViolationDeficiencies, NTCVDeficienciesModel>
    {
        void SyncNTCVDeficienciesLocalToServer(List<NTCVDeficienciesModel> model);
    }
}