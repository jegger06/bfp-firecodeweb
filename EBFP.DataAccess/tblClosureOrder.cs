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
    
    public partial class tblClosureOrder
    {
        public int CO_Id { get; set; }
        public string Ref_CO_Id { get; set; }
        public System.DateTime CO_IssueDate { get; set; }
        public string CO_Est_Id { get; set; }
        public string CO_Ref_IO_Id { get; set; }
        public int CO_Created_Emp_Id { get; set; }
        public System.DateTime CO_CreatedDate { get; set; }
        public Nullable<int> CO_LastUpdate_Emp_Id { get; set; }
        public Nullable<System.DateTime> CO_LastUpdateDate { get; set; }
        public int CO_Status { get; set; }
        public string CO_Result_IO_Id { get; set; }
        public int CO_Unit_Id { get; set; }
        public bool IsSynced { get; set; }
    }
}
