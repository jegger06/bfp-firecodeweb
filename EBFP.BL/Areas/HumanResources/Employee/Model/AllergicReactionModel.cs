
namespace EBFP.BL.HumanResources
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class AllergicReactionModel
    {
        public AllergicReactionModel()
        {
        }
        public bool CreatedFromAjax { get; set; }
        public int ALR_Id { get; set; }
        
        [Required]
        public string ALR_MedicationName { get; set; }

        [Required]
        public string ALR_AdverseReaction { get; set; }
        
    }
}
