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
    
    public partial class vwFSICFeesSummary
    {
        public decimal ConstructionTax { get; set; }
        public decimal RealtyTax { get; set; }
        public decimal PremiumTax { get; set; }
        public decimal SalesTax { get; set; }
        public decimal ProceedsTax { get; set; }
        public decimal FireSafetyInspectionFee { get; set; }
        public decimal StorageClearanceFee { get; set; }
        public decimal ConveyanceClearanceFee { get; set; }
        public decimal InstallationClearanceFee { get; set; }
        public decimal FireCodeAdminFine { get; set; }
        public decimal OtherFee { get; set; }
        public Nullable<int> FSIC_ApplicationType { get; set; }
        public int FSIC_Status { get; set; }
        public Nullable<int> Unit_Id { get; set; }
        public Nullable<int> Reg_Id { get; set; }
        public Nullable<int> Province_Id { get; set; }
        public Nullable<int> Municipality_Id { get; set; }
    }
}
