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
    
    public partial class tblClosureOrderDeficiencies
    {
        public int COD_Id { get; set; }
        public string Ref_COD_Id { get; set; }
        public string COD_CO_Id { get; set; }
        public string COD_Deficiency { get; set; }
        public string COD_Compliance { get; set; }
        public string COD_GracePeriod { get; set; }
        public bool COD_Complied { get; set; }
        public string COD_Remarks { get; set; }
        public int COD_Unit_Id { get; set; }
        public bool IsSynced { get; set; }
        public Nullable<decimal> COD_Fine { get; set; }
    }
}
