
namespace EBFP.BL.Administration
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface IApplicationPayment : IRepository<tblApplicationPayments, ApplicationPaymentModel>
    {
        void SyncApplicationPaymentLocalToServer(List<ApplicationPaymentModel> model);
    }
}