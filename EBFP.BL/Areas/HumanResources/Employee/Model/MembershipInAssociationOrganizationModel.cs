

namespace EBFP.BL.HumanResources
{
    using System.ComponentModel.DataAnnotations;
    public partial class MembershipInAssociationOrganizationModel
    {
        public int EMIAO_Id { get; set; }
        [Required]
        public string EMIAO_Title { get; set; }
        public int EMIAO_Emp_Id { get; set; }
    }
}
