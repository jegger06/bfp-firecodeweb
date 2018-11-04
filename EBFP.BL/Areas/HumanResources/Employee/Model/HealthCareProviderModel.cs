
namespace EBFP.BL.HumanResources
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class HealthCareProviderModel
    {
        public HealthCareProviderModel()
        {
        }
        public bool CreatedFromAjax { get; set; }
        public int HCP_Id { get; set; }
        
        [Required]
        public string HCP_Name { get; set; }

        [Required]
        public string HCP_City { get; set; }

        [Required]
        public string HCP_ProblemCaredFor { get; set; }
        
        public bool HCP_StillSeeing { get; set; }
        public bool HCP_Referral { get; set; }
    }
}
