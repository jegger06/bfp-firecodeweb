//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EBFP.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblApplicationPayments
    {
        public int AP_Id { get; set; }
        public string Ref_AP_Id { get; set; }
        public decimal AP_AmountTendered { get; set; }
        public Nullable<decimal> AP_AmountChange { get; set; }
        public int AP_ApplicationType { get; set; }
        public string AP_App_Id { get; set; }
        public System.DateTime AP_ORDate { get; set; }
        public int AP_ORNumber { get; set; }
        public Nullable<decimal> AP_ORAmount { get; set; }
        public int AP_PaymentMode { get; set; }
        public string AP_PaymentRefNumber { get; set; }
        public string AP_PaymentRefBank { get; set; }
        public int AP_Unit_Id { get; set; }
        public bool IsSynced { get; set; }
        public Nullable<System.DateTime> AP_PaymentRefDate { get; set; }
    
        public virtual tblUnits tblUnits { get; set; }
    }
}
