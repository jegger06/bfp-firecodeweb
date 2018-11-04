


namespace EBFP.BL.HumanResources
{
    using System.ComponentModel.DataAnnotations;
    public partial class ReferenceModel
    {
        public int ER_Id { get; set; }
        [Required]
        public string ER_FullName { get; set; }
        [Required]
        public string ER_Address { get; set; }
        public string ER_PhoneNumber { get; set; }
        public int ER_Emp_Id { get; set; } 
    }
}
