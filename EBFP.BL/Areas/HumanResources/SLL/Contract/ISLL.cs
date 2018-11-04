
namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Helper;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface ISLL : IRepository<tblEmployees, EmployeeModel>
    {
        SeniorityLinealListResult GetListResult(GridInfo gridInfo);
    }
}