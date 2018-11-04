
namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;

    public interface ILogs : IRepository<tblLogs, LogsModel>
    {
        void InsertLogs(LogsModel model);
    }
}