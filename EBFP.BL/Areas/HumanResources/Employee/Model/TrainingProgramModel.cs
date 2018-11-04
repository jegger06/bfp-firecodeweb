

namespace EBFP.BL.HumanResources
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public partial class TrainingProgramModel
    {
        public int ETP_Id { get; set; }
        [Required]
        public string ETP_TrainingTitle { get; set; }
        [Required]
        public Nullable<System.DateTime> ETP_StartDate { get; set; }
        [Required]
        public Nullable<System.DateTime> ETP_EndDate { get; set; }
        public decimal ETP_Hours { get; set; }
        public string ETP_ConductSponsor { get; set; }
        public string ETP_LDType { get; set; }
        public bool ETP_Commend { get; set; }
        public Nullable<int> ETP_CommendOfficer { get; set; }
        public int ETP_Emp_Id { get; set; }
        public Nullable<int> ETP_LDType_ID { get; set; }
        public string ETP_Remarks { get; set; }
    }
}
