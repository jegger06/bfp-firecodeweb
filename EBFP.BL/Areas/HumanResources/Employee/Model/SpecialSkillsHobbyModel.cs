
namespace EBFP.BL.HumanResources
{
    using System.ComponentModel.DataAnnotations;
    public partial class SpecialSkillsHobbyModel
    {
        public int ESSH_Id { get; set; }
        [Required]
        public string ESSH_Title { get; set; }
        public int ESSH_Emp_Id { get; set; }
    }

}
