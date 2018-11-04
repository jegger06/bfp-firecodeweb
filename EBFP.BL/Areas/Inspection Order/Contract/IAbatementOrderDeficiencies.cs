
namespace EBFP.BL.InspectionOrder
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface IAbatementOrderDeficiencies : IRepository<tblAbatementOrderDeficiencies, AbatementOrderDeficienciesModel>
    {
        void SyncAbatementOrderDeficienciesLocalToServer(List<AbatementOrderDeficienciesModel> model);
    }
}