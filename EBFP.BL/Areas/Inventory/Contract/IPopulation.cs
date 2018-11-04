
using System.Collections.Generic;

namespace EBFP.BL.Inventory
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;

    public interface IPopulation : IRepository<tblPopulations, PopulationModel>
    {
        void InsertBulk(List<PopulationModel> model, int municipalityid);

    }
}