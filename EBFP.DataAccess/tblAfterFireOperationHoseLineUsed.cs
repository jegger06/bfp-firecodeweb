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
    
    public partial class tblAfterFireOperationHoseLineUsed
    {
        public int HLU_Id { get; set; }
        public Nullable<int> HLU_AFO_Id { get; set; }
        public string HLU_Number { get; set; }
        public string HLU_Type { get; set; }
        public string HLU_Quantity { get; set; }
        public int HLU_Created_Emp_Id { get; set; }
        public System.DateTime HLU_CreatedDate { get; set; }
        public Nullable<int> HLU_LastUpdate_Emp_Id { get; set; }
        public Nullable<System.DateTime> HLU_LastUpdateDate { get; set; }
    
        public virtual tblAfterFireOperations tblAfterFireOperations { get; set; }
    }
}
