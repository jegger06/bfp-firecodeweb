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
    
    public partial class tblNoticeToComply
    {
        public int NTC_Id { get; set; }
        public string Ref_NTC_Id { get; set; }
        public System.DateTime NTC_IssueDate { get; set; }
        public string NTC_Est_Id { get; set; }
        public string NTC_Ref_IO_Id { get; set; }
        public int NTC_Created_Emp_Id { get; set; }
        public System.DateTime NTC_CreatedDate { get; set; }
        public Nullable<int> NTC_LastUpdate_Emp_Id { get; set; }
        public Nullable<System.DateTime> NTC_LastUpdateDate { get; set; }
        public int NTC_Status { get; set; }
        public string NTC_Result_IO_Id { get; set; }
        public int NTC_Unit_Id { get; set; }
        public bool IsSynced { get; set; }
    }
}
