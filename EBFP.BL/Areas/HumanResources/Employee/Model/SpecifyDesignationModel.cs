
namespace EBFP.BL.HumanResources
{
    using System.ComponentModel.DataAnnotations;
    public partial class SpecifyDesignationModel
    {
        public int SpecifyDesig_Id { get; set; }
        [Required]
        public string SpecifyDesig_Title { get; set; }
        public int SpecifyDesig_Emp_Id { get; set; }
    }

}
