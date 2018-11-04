
namespace EBFP.BL.InspectionOrder
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface IClosureOrderDeficiencies : IRepository<tblClosureOrderDeficiencies, ClosureOrderDeficienciesModel>
    {
        void SyncClosureOrderDeficienciesLocalToServer(List<ClosureOrderDeficienciesModel> model);
    }
}