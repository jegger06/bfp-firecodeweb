
using System.Web;

namespace EBFP.BL.Administration
{
    using EBFP.DataAccess;
    using Establishment;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface IBPLOPayment : IRepository<tblBPLOPayments, BPLOPaymentModel>
    {
        void SyncBPLOPaymentLocalToServer(List<BPLOPaymentModel> model);
        List<EstablishmentModel> GetExistingEstablishments(List<string> mpNumberList, int unitId);
        bool SubmitPayment(BPLOPaymentsModel payment);
        bool GenerateUploadedPayments(List<PaymentModel> paymentList, string destinationPath);
        void UploadPayments(HttpPostedFileBase file, ref List<PaymentModel> _listBusiness);
    }
}