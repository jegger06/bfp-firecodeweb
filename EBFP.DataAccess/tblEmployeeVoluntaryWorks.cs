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
    
    public partial class tblEmployeeVoluntaryWorks
    {
        public int EVW_Id { get; set; }
        public string EVW_OrgName { get; set; }
        public System.DateTime EVW_StartDate { get; set; }
        public System.DateTime EVW_EndDate { get; set; }
        public Nullable<decimal> EVW_Hours { get; set; }
        public string EVW_PosNatureWork { get; set; }
        public int EVW_Emp_Id { get; set; }
    
        public virtual tblEmployees tblEmployees { get; set; }
    }
}
