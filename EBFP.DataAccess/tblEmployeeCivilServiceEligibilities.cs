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
    
    public partial class tblEmployeeCivilServiceEligibilities
    {
        public int ECSE_Id { get; set; }
        public string ECSE_Title { get; set; }
        public string ECSE_Rating { get; set; }
        public System.DateTime ECSE_ExamDate { get; set; }
        public string ECSE_ExamPlace { get; set; }
        public string ECSE_LicNumber { get; set; }
        public Nullable<System.DateTime> ECSE_LicReleaseDate { get; set; }
        public int ECSE_Emp_Id { get; set; }
    
        public virtual tblEmployees tblEmployees { get; set; }
    }
}