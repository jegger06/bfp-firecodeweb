

namespace EBFP.BL.HumanResources
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public partial class EmployeeChildModel
    {
        public EmployeeChildModel()
        {
            EC_BirthDate = DateTime.Now;
        }

        public int EC_Id { get; set; }

        [Required]
        public string EC_FullName { get; set; }
        public string EC_Gender { get; set; }

        [Required]
        public System.DateTime EC_BirthDate { get; set; }
        public int EC_Emp_Id { get; set; }
    }
}
