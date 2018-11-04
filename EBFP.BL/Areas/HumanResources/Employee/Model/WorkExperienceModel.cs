

namespace EBFP.BL.HumanResources
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public partial class WorkExperienceModel
    {
        public int EWE_Id { get; set; }
        [Required]
        public string EWE_CompanyName { get; set; }
        [Required]
        public string EWE_PositionTitle { get; set; }
        [Required]
        public Nullable<System.DateTime> EWE_StartDate { get; set; } 
        public Nullable<System.DateTime> EWE_EndDate { get; set; }
        public string EWE_MonthlySalary { get; set; }
        public string EWE_SalaryGrade { get; set; }
        public string EWE_ApptStatus { get; set; }
        public bool EWE_GovtService { get; set; }
        public int EWE_Emp_Id { get; set; }
    }
}
