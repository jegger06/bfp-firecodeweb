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
    
    public partial class tblEmployeeTrainingPrograms
    {
        public int ETP_Id { get; set; }
        public string ETP_TrainingTitle { get; set; }
        public System.DateTime ETP_StartDate { get; set; }
        public System.DateTime ETP_EndDate { get; set; }
        public decimal ETP_Hours { get; set; }
        public string ETP_ConductSponsor { get; set; }
        public bool ETP_Commend { get; set; }
        public Nullable<int> ETP_CommendOfficer { get; set; }
        public int ETP_Emp_Id { get; set; }
        public string ETP_LDType { get; set; }
        public Nullable<int> ETP_LDType_ID { get; set; }
        public string ETP_Remarks { get; set; }
    
        public virtual tblEmployees tblEmployees { get; set; }
    }
}
