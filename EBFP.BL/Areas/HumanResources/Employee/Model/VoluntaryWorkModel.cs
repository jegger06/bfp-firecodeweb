
namespace EBFP.BL.HumanResources
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public partial class VoluntaryWorkModel
    {
        public int EVW_Id { get; set; }
        [Required]
        public string EVW_OrgName { get; set; }
        [Required]
        public DateTime? EVW_StartDate { get; set; }
        [Required]
        public DateTime? EVW_EndDate { get; set; }
        public decimal? EVW_Hours { get; set; }
        public string EVW_PosNatureWork { get; set; }
        public int EVW_Emp_Id { get; set; }
    }
}
