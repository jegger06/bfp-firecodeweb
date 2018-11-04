using EBFP.BL.Helper;
using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.HumanResources
{
    public interface IDirectorates : IRepository<tblDirectorates, DirectoratesModel>
    {
        DirectoratesListResult GetListResult(GridInfo gridInfo);
        void UpdateDirectorates(DirectoratesModel model);
        bool DeleteByID(int dirId);
        bool DirectorateExists(string dirCode, string dirName, int id = 0);
        bool CheckIfCurrentlyUsed(int igId);
        DirectoratesModel GetDirectoratesById(int dirId);
    }
}