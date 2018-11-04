using System.Collections.Generic;
using EBFP.BL.Helper;

namespace EBFP.BL.Inventory
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    public interface IPersonnel
    {
        PersonnelListResult GetPersonneListResult(GridInfo gridInfo);
    }
}
