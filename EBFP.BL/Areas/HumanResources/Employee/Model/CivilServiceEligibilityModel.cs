

namespace EBFP.BL.HumanResources
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public partial class CivilServiceEligibilityModel
    {
        public int ECSE_Id { get; set; }
        [Required]
        public string ECSE_Title { get; set; }
        public string ECSE_Rating { get; set; }

        [Required]
        public Nullable<System.DateTime> ECSE_ExamDate { get; set; }
        [Required]
        public string ECSE_ExamPlace { get; set; }
        public string ECSE_LicNumber { get; set; }
        public Nullable<System.DateTime> ECSE_LicReleaseDate { get; set; }
        public int ECSE_Emp_Id { get; set; }
    }
}
