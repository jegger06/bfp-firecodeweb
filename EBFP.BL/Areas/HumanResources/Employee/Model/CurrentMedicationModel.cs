
namespace EBFP.BL.HumanResources
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class CurrentMedicationModel
    {
        public CurrentMedicationModel()
        {
        }
        public bool CreatedFromAjax { get; set; }
        public int Med_Id { get; set; }
        
        [Required]
        public string Med_MedicineName { get; set; }

        [Required]
        public string Med_HowTaken { get; set; }

        [Required]
        public string Med_WhoPrescribes { get; set; }
        
        public bool Med_NeedRx { get; set; }
    }
}
