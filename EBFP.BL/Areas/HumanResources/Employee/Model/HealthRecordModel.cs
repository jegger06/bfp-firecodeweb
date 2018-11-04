
namespace EBFP.BL.HumanResources
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class HealthRecordModel
    {
        public HealthRecordModel()
        {
        }
        public bool CreatedFromAjax { get; set; }
        public int HR_Id { get; set; }

        [Required]
        public string HR_Treatment { get; set; }

        [Required]
        public Nullable<System.DateTime> HR_Date { get; set; }

        [Required]
        public string HR_Diagnosis { get; set; }
    }
}
