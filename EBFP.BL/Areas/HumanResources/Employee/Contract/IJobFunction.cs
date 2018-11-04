 
namespace EBFP.BL.HumanResources
{
    using DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;
    public interface IJobFunction : IRepository<tblJobFunctions, JobFuntionModel>
    { 
    }
}