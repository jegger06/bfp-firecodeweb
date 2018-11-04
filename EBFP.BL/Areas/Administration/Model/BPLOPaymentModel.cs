
using System;
using System.Collections.Generic;
using System.Web;

namespace EBFP.BL.Administration
{
    public class BPLOPaymentModel
    {
        public int BPLOP_Id { get; set; }
        public string Ref_BPLOP_Id { get; set; }
        public string BPLOP_App_Id { get; set; }
        public decimal BPLOP_PayAmount { get; set; }
        public System.DateTime BPLOP_PayDate { get; set; }
        public int BPLOP_Created_Emp_Id { get; set; }
        public System.DateTime BPLOP_CreatedDate { get; set; }
        public int BPLOP_Unit_Id { get; set; }
        public bool IsSynced { get; set; }
        public string BPLOP_BasisOfAmountPaid { get; set; }
    }

    public class BPLOPaymentsUploadModel
    {
        public BPLOPaymentsUploadModel()
        {
            this.paymentList = new List<PaymentModel>();
        }
        public List<PaymentModel> paymentList { get; set; }

        public HttpPostedFileBase uploadList { get; set; }
    }

    public class BPLOPaymentsModel
    {

        public int BPLOP_Id { get; set; }

        public string Ref_BPLOP_Id { get; set; }

        public string BPLOP_App_Id { get; set; }

        public decimal BPLOP_PayAmount { get; set; }

        public DateTime BPLOP_PayDate { get; set; }

        public int BPLOP_Created_Emp_Id { get; set; }

        public DateTime BPLOP_CreatedDate { get; set; }

        public string BPLOP_BasisOfAmountPaid { get; set; }

        public int BPLOP_Unit_Id { get; set; }

        public bool IsSynced { get; set; }

        //public HttpPostedFileBase PaymentList { get; set; }
    }

    public class PaymentModel
    {
        public string Est_Id { get; set; }

        public string BusinessIdNumber { get; set; }

        public string BusinessName { get; set; }

        public string BusinessAddress { get; set; }

        public string TradeName { get; set; }

        public string TaxYear { get; set; }

        public string Amount { get; set; }

        public string IssueDate { get; set; }

        public string BasisOfAmountPaid { get; set; }

        public string Remarks { get; set; }
    }
}
