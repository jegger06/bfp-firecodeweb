
namespace EBFP.BL.HumanResources
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class EducBackgroundModel
    {
        public EducBackgroundModel()
        {
            //this.EEB_StartDate = DateTime.Now.ToString();
            //this.EEB_EndDate = DateTime.Now.ToString();
        }

        public bool IsReadonly
        {
            get
            {
                //if (EEB_EducType == (int)EducationLevel.ELEMENTARY || EEB_EducType == (int)EducationLevel.SECONDARY)
                //    return true;

                return false;
            }
        }

        public bool CreatedFromAjax { get; set; }
        public int EEB_Id { get; set; }

        [Required]
        public int EEB_EducType { get; set; }

        [Required]
        public string EEB_SchoolName { get; set; }

        [Required]
        public string EEB_DegreeCourse { get; set; }

        [Required]
        public string EEB_GraduateYear { get; set; }

        [Required]
        public string EEB_HighestLevel { get; set; }

        [Required]
        public string EEB_StartDate { get; set; }

        [Required]
        public string EEB_EndDate { get; set; }

        [Required]
        public string EEB_Awards { get; set; }
        public int EEB_Emp_Id { get; set; }
    }
}
