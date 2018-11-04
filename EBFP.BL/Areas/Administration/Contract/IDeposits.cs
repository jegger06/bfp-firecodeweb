
namespace EBFP.BL.Administration
{
    using EBFP.DataAccess;
    using Helper;
    using Queries.Core.Repositories;
    using System;
    using System.Collections.Generic;

    public interface IDeposits : IRepository<tblUnitDeposits, DepositsModel>
    {
        void SyncDepositLocalToServer(List<DepositsModel> model);

        //
        List<string> BankName(int unitId);
        List<string> EmployeeName(int unitId);
        //List<DepositsModel> GetDepositList(int unitId, DateTime from, DateTime to, bool showAll = true);
        DepositListResult GetDepositList(GridInfo gridInfo,int unitId);
    }
}