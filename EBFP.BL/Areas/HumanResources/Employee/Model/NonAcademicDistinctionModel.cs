

namespace EBFP.BL.HumanResources
{
    using System.ComponentModel.DataAnnotations;
    public partial class NonAcademicDistinctionModel
    {
        public int ENAD_Id { get; set; }
        [Required]
        public string ENAD_Title { get; set; }
        public int ENAD_Emp_Id { get; set; }
    }
}
