
using EBFP.DataAccess;
using Queries.Core.Repositories;
using System.Collections.Generic;

namespace EBFP.BL.HumanResources
{
    public interface IReference : IRepository<tblEmployeeReferences, ReferenceModel>
    {
        void InsertBulk(List<ReferenceModel> model, int Emp_Id);
    }
}