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
    
    public partial class tblEmployeeWorkExperiences
    {
        public int EWE_Id { get; set; }
        public string EWE_CompanyName { get; set; }
        public string EWE_PositionTitle { get; set; }
        public System.DateTime EWE_StartDate { get; set; }
        public Nullable<System.DateTime> EWE_EndDate { get; set; }
        public string EWE_MonthlySalary { get; set; }
        public string EWE_SalaryGrade { get; set; }
        public string EWE_ApptStatus { get; set; }
        public bool EWE_GovtService { get; set; }
        public int EWE_Emp_Id { get; set; }
    
        public virtual tblEmployees tblEmployees { get; set; }
    }
}