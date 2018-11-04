
namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface IEmployeeChildren : IRepository<tblEmployeeChildren,EmployeeChildModel> 
    {
        void InsertBulk(List<EmployeeChildModel> EmployeeChildren, int Emp_Id);
    }
}