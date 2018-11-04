
namespace EBFP.BL.Administration
{
    using System;
    public class ApplicationPaymentModel
    {
        public int AP_Id { get; set; }
        public string Ref_AP_Id { get; set; }
        public int AP_ApplicationType { get; set; }
        public string AP_App_Id { get; set; }
        public DateTime AP_ORDate { get; set; }
        public int AP_ORNumber { get; set; }
        public decimal? AP_ORAmount { get; set; }
        public int AP_PaymentMode { get; set; }
        public string AP_PaymentRefNumber { get; set; }
        public string AP_PaymentRefBank { get; set; }
        public int AP_Unit_Id { get; set; }
        public bool IsSynced { get; set; }
        public decimal AP_AmountTendered { get; set; }
        public decimal? AP_AmountChange { get; set; }
    }
}
