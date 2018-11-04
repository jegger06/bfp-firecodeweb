using System.Collections.Generic;

namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;

    public interface IUser : IRepository<tblEmployees, UserModel>
    {
        UserModel GetByID(int employeeID);
        bool IsMember(int employeeID);
        void UpdateDecryptedPassword(int employeeID, string password);
        void UpdateDecryptedPasswordByUsername(string userName, string password);
    }
}