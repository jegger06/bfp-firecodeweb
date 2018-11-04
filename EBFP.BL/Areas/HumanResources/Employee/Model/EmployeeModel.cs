using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using EBFP.BL.Helper;
using EBFP.DataAccess;
using EBFP.Helper;

namespace EBFP.BL.HumanResources
{
    public class EmployeeListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<EmployeeListModel> EmployeeListModel { get; set; }
    }


    public class EmployeeListModel
    {
        public string Emp_Id { get; set; }
        public string Emp_FirstName { get; set; }
        public string Emp_MiddleName { get; set; }
        public string Emp_LastName { get; set; }
        public string Emp_Number { get; set; }
        public string Emp_Curr_Rank { get; set; }
        public string Emp_Curr_Eligibility { get; set; }
        public string Emp_Curr_Unit_Region { get; set; }
        public string Emp_Curr_Unit_Fullname { get; set; }
        public int? Emp_Curr_Unit_Region_Id { get; set; }
    }

    public class EmployeeModel : EmployeeBase
    {
        public EmployeeModel()
        {
            //SET DEFAULT VALUES
            EmployeeChildren = new List<EmployeeChildModel>();
            EducationalBackgrounds = new List<EducBackgroundModel>();
            CivilServiceEligibilities = new List<CivilServiceEligibilityModel>();
            OtherInformation = new OtherInformationModel();
            TrainingPrograms = new List<TrainingProgramModel>();
            VoluntaryWorks = new List<VoluntaryWorkModel>();
            WorkExperiences = new List<WorkExperienceModel>();
            SpecialSkillsHobbies = new List<SpecialSkillsHobbyModel>();
            NonAcademicDistinctions = new List<NonAcademicDistinctionModel>();
            MembershipInAssociationOrganizations = new List<MembershipInAssociationOrganizationModel>();
            References = new List<ReferenceModel>();
            ServiceAppointment = new List<ServiceAppointmentModel>();
            //this.EducationalBackgrounds.Add(new EducBackgroundModel() { EEB_EducType = (int)EducationLevel.ELEMENTARY });
            //this.EducationalBackgrounds.Add(new EducBackgroundModel() { EEB_EducType = (int)EducationLevel.SECONDARY });
            user = new UserModel();
            SpecifyDesignation = new List<SpecifyDesignationModel>();
            Emp_Gender = "M";
            Emp_CivilStatus = 1;
            Emp_Citizenship = "Filipino";
        }

        //NOTE : Required 

        [Required]
        public string sEmp_Id
        {
            get
            {
                var encryptID = Emp_Id.ToString().Encrypt();
                return encryptID;
            }
        }

        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Emp_Id { get; set; }

        [Required]
        [Display(Name = "Employee Number")]
        [StringLength(10)]
        public string Emp_Number { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string Emp_FirstName { get; set; }

        [Required]
        [Display(Name = "Middle Name")]
        public string Emp_MiddleName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string Emp_LastName { get; set; }

        //[Required]
        [Display(Name = "Birth Date")]
        public DateTime? Emp_BirthDate { get; set; }

        //[Required]
        [Display(Name = "Birth Place")]
        public string Emp_BirthPlace { get; set; }

        //[Required]
        [Display(Name = "Gender")]
        public string Emp_Gender { get; set; }

        //[Required]
        [Display(Name = "Civil Status")]
        public int? Emp_CivilStatus { get; set; }

        public string Emp_CivilStatus_Other { get; set; }

        //[Required]
        [Display(Name = "Citizenship")]
        public string Emp_Citizenship { get; set; }

        public string Emp_Citizenship_Dual { get; set; }
        public string Emp_Citizenship_Country { get; set; }


        [StringLength(50)]
        public string Emp_SuffixName { get; set; }

        //[Required]
        [Display(Name = "Height")]
        public string Emp_Height { get; set; }

        //[Required]
        [Display(Name = "Weight")]
        [MaxLength]
        public string Emp_Weight { get; set; }

        //[Required]
        [Display(Name = "Blood Type")]
        public string Emp_BloodType { get; set; }

        public string Emp_GSISNumber { get; set; }
        public string Emp_PAGIBIGNumber { get; set; }
        public string Emp_PHICNumber { get; set; }

        public string Emp_SSSNumber { get; set; }

        //[Required]
        //[Display(Name = "Residential Address")]
        //public string Emp_Residential_Address { get; set; }
       // [Required]
        public string Emp_Residential_HouseNo { get; set; }

        public string Emp_Residential_Street { get; set; }
        public string Emp_Residential_Village { get; set; }

       // [Required]
        public string Emp_Residential_Barangay { get; set; }

       // [Required]
        public string Emp_Residential_Municipality { get; set; }

       // [Required]
        public string Emp_Residential_Province { get; set; }


        //[Required]
        [Display(Name = "Residential ZipCode")]
        public string Emp_Residential_ZipCode { get; set; }

        public string Emp_Residential_PhoneNumber { get; set; }

        ////[Required]
        //[Display(Name = "Permanent Address")]
        //public string Emp_Permanent_Address { get; set; }
        public string Emp_Permanent_HouseNo { get; set; }

        public string Emp_Permanent_Street { get; set; }
        public string Emp_Permanent_Village { get; set; }
        public string Emp_Permanent_Barangay { get; set; }
        public string Emp_Permanent_Municipality { get; set; }
        public string Emp_Permanent_Province { get; set; }

        //[Required]
        [Display(Name = "Permanent ZipCode")]
        public string Emp_Permanent_ZipCode { get; set; }

        public string Emp_Permanent_PhoneNumber { get; set; }
        public string Emp_EmailAddress { get; set; }
        public string Emp_MobileNumber { get; set; }
        public string Emp_AgencyEmpNumber { get; set; }
        public string Emp_TINNumber { get; set; }
        public string Emp_Spouse_FirstName { get; set; }
        public string Emp_Spouse_MiddleName { get; set; }
        public string Emp_Spouse_LastName { get; set; }
        public string Emp_Spouse_SuffixName { get; set; }
        public string Emp_Spouse_Occupation { get; set; }
        public string Emp_Spouse_EmpBusName { get; set; }
        public string Emp_Spouse_EmpBusAddress { get; set; }
        public string Emp_Spouse_EmpBusPhoneNumber { get; set; }
        public string Emp_Father_FirstName { get; set; }
        public string Emp_Father_MiddleName { get; set; }
        public string Emp_Father_LastName { get; set; }
        public string Emp_Father_SuffixName { get; set; }
        public string Emp_Mother_FirstName { get; set; }
        public string Emp_Mother_MiddleName { get; set; }
        public string Emp_Mother_MaidenName { get; set; }
        public string Emp_Mother_LastName { get; set; }
        public string Emp_Elem_SchoolName { get; set; }
        public string Emp_Elem_DegreeCourse { get; set; }
        public string Emp_Elem_GraduateYear { get; set; }
        public string Emp_Elem_HighestLevel { get; set; }
        public string Emp_Elem_StartDate { get; set; }
        public string Emp_Elem_EndDate { get; set; }
        public string Emp_Elem_Awards { get; set; }
        public string Emp_HS_SchoolName { get; set; }
        public string Emp_HS_DegreeCourse { get; set; }
        public string Emp_HS_GraduateYear { get; set; }
        public string Emp_HS_HighestLevel { get; set; }
        public string Emp_HS_StartDate { get; set; }
        public string Emp_HS_EndDate { get; set; }
        public string Emp_HS_Awards { get; set; }
        public string Emp_Voc_SchoolName { get; set; }
        public string Emp_Voc_DegreeCourse { get; set; }
        public string Emp_Voc_GraduateYear { get; set; }
        public string Emp_Voc_HighestLevel { get; set; }
        public string Emp_Voc_StartDate { get; set; }
        public string Emp_Voc_EndDate { get; set; }
        public string Emp_Voc_Awards { get; set; }
        public string Emp_Col1_SchoolName { get; set; }
        public string Emp_Col1_DegreeCourse { get; set; }
        public string Emp_Col1_GraduateYear { get; set; }
        public string Emp_Col1_HighestLevel { get; set; }
        public string Emp_Col1_StartDate { get; set; }
        public string Emp_Col1_EndDate { get; set; }
        public string Emp_Col1_Awards { get; set; }
        public string Emp_Col2_SchoolName { get; set; }
        public string Emp_Col2_DegreeCourse { get; set; }
        public string Emp_Col2_GraduateYear { get; set; }
        public string Emp_Col2_HighestLevel { get; set; }
        public string Emp_Col2_StartDate { get; set; }
        public string Emp_Col2_EndDate { get; set; }
        public string Emp_Col2_Awards { get; set; }
        public string Emp_Grad1_SchoolName { get; set; }
        public string Emp_Grad1_DegreeCourse { get; set; }
        public string Emp_Grad1_GraduateYear { get; set; }
        public string Emp_Grad1_StartDate { get; set; }
        public string Emp_Grad1_EndDate { get; set; }
        public string Emp_Grad1_Awards { get; set; }
        public string Emp_Grad2_SchoolName { get; set; }
        public string Emp_Grad2_DegreeCourse { get; set; }
        public string Emp_Grad2_GraduateYear { get; set; }
        public string Emp_Grad2_StartDate { get; set; }
        public string Emp_Grad2_EndDate { get; set; }
        public string Emp_Grad2_Awards { get; set; }
        public bool Impersonate { get; set; }

        [Required]
        public string Emp_Username { get; set; }

       // [RequiredIf("Item Number")]
        public string Emp_ItemNumber { get; set; }

      //  [RequiredIf("Badge Number")]
        public string Emp_BadgeNumber { get; set; }

       // [RequiredIf("Date entered in uniformed fire service")]
        public DateTime? Emp_Service_StartDate { get; set; }

        public DateTime? Emp_Service_UniformGovtStartDate { get; set; }

      //  [RequiredIf("Date entered in government service")]
        public DateTime? Emp_Service_GovtStartDate { get; set; }

        public DateTime? Emp_Retired_Date { get; set; }
        public DateTime? Emp_LastPromotionDate_Temp { get; set; }
        public DateTime? Emp_LastPromotionDate_Permanent { get; set; }
        public DateTime? Emp_AssumedOfficerDate { get; set; }
        public DateTime? Emp_LastTrainingDate { get; set; }
        public int? Emp_EducCourse { get; set; }
        public int? Emp_HighestEducAttainment { get; set; }
        public int? Emp_HighestMandatoryTraining { get; set; }

        [RequiredDutyStatus]
        public int? Emp_DutyStatus { get; set; }

       // [RequiredIf("Appointment Status")]
        public int? Emp_Curr_ApptStatus { get; set; }

        //[RequiredIf("Present Designation")]
        public int? Emp_Curr_JobFunc { get; set; }

        //[RequiredIf("Specify Designation")]
        public string Emp_Curr_PosDesignationTitle { get; set; }

        [RequiredIf("Present Rank")]
        public int? Emp_Curr_Rank { get; set; }

        [RequiredIf("Present Assignement")]
        public int? Emp_Curr_Unit { get; set; }

        public int? Emp_Curr_Eligibility { get; set; }

      //  [RequiredIf("Salary Grade")]
        public int? Emp_Curr_SalaryGrade { get; set; }
        public string Emp_PresentAsgmt_DO_BO_RO { get; set; }
        public string Emp_AppointmentStatus_DO_BO_RO { get; set; }
        public decimal? Emp_Curr_Salary { get; set; }
        public string Emp_Remarks { get; set; }
        public string Emp_Rank_Txt { get; set; }
        public byte[] Emp_Photo { get; set; }
        public string AccessType { get; set; }
        public string UploadResult { get; set; }
        public int? Emp_Religion { get; set; }
        public string Emp_Religion_Others { get; set; }
        public int? Emp_MACourse { get; set; }
        public int? Emp_Eligibility_Type { get; set; }
        public DateTime? Emp_DES { get; set; }
        public string Emp_BP_Number { get; set; }
        public string Emp_Tax_Code { get; set; }
        public string Emp_Atm_Number { get; set; }

        public bool? Emp_IsChecked { get; set; }
        public int? Emp_CheckedBy { get; set; }
        public DateTime? Emp_CheckedDate { get; set; }
        public HttpPostedFileBase EmployeeImage { get; set; }
        public HttpPostedFileBase Alphalist { get; set; }
        public List<EmployeeChildModel> EmployeeChildren { get; set; }
        public List<EducBackgroundModel> EducationalBackgrounds { get; set; }
        public List<CivilServiceEligibilityModel> CivilServiceEligibilities { get; set; }
        public OtherInformationModel OtherInformation { get; set; }
        public List<TrainingProgramModel> TrainingPrograms { get; set; }
        public List<VoluntaryWorkModel> VoluntaryWorks { get; set; }
        public List<WorkExperienceModel> WorkExperiences { get; set; }
        public List<SpecialSkillsHobbyModel> SpecialSkillsHobbies { get; set; }
        public List<NonAcademicDistinctionModel> NonAcademicDistinctions { get; set; }
        public List<MembershipInAssociationOrganizationModel> MembershipInAssociationOrganizations { get; set; }
        public UserModel user { get; set; }
        public List<ReferenceModel> References { get; set; }
        public List<ServiceAppointmentModel> ServiceAppointment { get; set; }
        public List<SpecifyDesignationModel> SpecifyDesignation { get; set; }
    }

    public class EmployeeBase
    {
        public int MunicipalityID { get; set; }
        public int ProvinceID { get; set; }
        public int RegionID { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }

        public string RegionName { get; set; }
        public string ProvinceName { get; set; }
        public string UnitName { get; set; }

    }

    public class EmployeeSearchModel
    {
        public int RegionId { get; set; }
        public int ProvinceId { get; set; }
        public int MunicipalityId { get; set; }
        public int UnitId { get; set; }
        public int DutyStatusId { get; set; }
        public string Gender { get; set; }
        public int CivilStatus { get; set; }
        public int DesignationId { get; set; }
        public int CourseId { get; set; }
        public int EligibilityId { get; set; }
        public DateTime? LastTrainingDate { get; set; }
        public string LastName { get; set; }
        public string AccountNumber { get; set; }
        public string BadgeNumber { get; set; }
        public string ItemNumber { get; set; }
        public int AppointmentStatusId { get; set; }
        public int HighestMandatoryTraining { get; set; }
        public int RankId { get; set; }
        public DateTime? StartServiceDate { get; set; }
        public DateTime? EndServiceDate { get; set; }
        public DateTime? Birthdate { get; set; }
        public int isChecked { get; set; }
        public bool IsSearch { get; set; }
    }


    public class RequiredIfAttribute : ValidationAttribute
    {
        private readonly string _displayName = string.Empty;
        private readonly RequiredAttribute _innerAttribute = new RequiredAttribute();

        public RequiredIfAttribute(string displayName)
        {
            _displayName = displayName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (PageSecurity.HasAccess(PageArea.HRIS_EmployeeRoster_BFPInformation_Modify))
                if (string.IsNullOrWhiteSpace(value?.ToString()))
                {
                    return new ValidationResult(_displayName + " is required.");
                }
                else
                {
                    if (!_innerAttribute.IsValid(value))
                        return new ValidationResult(_displayName + " is required.");
                }


            return ValidationResult.Success;
        }
    }

    public class RequiredDutyStatusAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (PageSecurity.HasAccess(PageArea.HRIS_EmployeeRoster_BFPInformation_Modify) &&
                (CurrentUser.RoleName == "REGIONAL ADMIN" || CurrentUser.RoleName == "MAIN ADMIN"))
            {
                if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
                    return ValidationResult.Success;

                return new ValidationResult("Duty Status is required.");
            }
            return ValidationResult.Success;
        }
    }

    public class AlphaColumnListModel
    {
        public bool HasEmp_Number { get; set; }
        public bool HasEmp_LastName { get; set; }
        public bool HasEmp_FirstName { get; set; }
        public bool HasEmp_MiddleName { get; set; }
        public bool HasEmp_SuffixName { get; set; }
        public bool HasEmp_BP_Number { get; set; }
        public bool HasEmp_Tax_Code { get; set; }
        public bool HasEmp_Atm_Number { get; set; }
        public bool HasEmp_DES { get; set; }
        public bool HasEmp_Curr_Unit { get; set; }
        public bool HasEmp_BirthDate { get; set; }
        public bool HasEmp_BirthPlace { get; set; }
        public bool HasEmp_CivilStatus { get; set; }
        public bool HasEmp_Citizenship { get; set; }
        public bool HasEmp_Gender { get; set; }
        public bool HasEmp_Height { get; set; }
        public bool HasEmp_Weight { get; set; }
        public bool HasEmp_BloodType { get; set; }
        public bool HasEmp_GSISNumber { get; set; }
        public bool HasEmp_PAGIBIGNumber { get; set; }
        public bool HasEmp_PHICNumber { get; set; }
        public bool HasEmp_SSSNumber { get; set; }
        public bool HasEmp_TINNumber { get; set; }
        public bool HasEmp_Religion { get; set; }
        public bool HasEmp_EmailAddress { get; set; }
        public bool HasEmp_MobileNumber { get; set; }
        public bool HasEmp_AgencyEmpNumber { get; set; }
        public bool HasEmp_Residential_HouseNo { get; set; }
        public bool HasEmp_Residential_Street { get; set; }
        public bool HasEmp_Residential_Village { get; set; }
        public bool HasEmp_Residential_Barangay { get; set; }
        public bool HasEmp_Residential_Municipality { get; set; }
        public bool HasEmp_Residential_Province { get; set; }
        public bool HasEmp_Residential_ZipCode { get; set; }
        public bool HasEmp_Residential_PhoneNumber { get; set; }
        public bool HasEmp_Permanent_HouseNo { get; set; }
        public bool HasEmp_Permanent_Street { get; set; }
        public bool HasEmp_Permanent_Village { get; set; }
        public bool HasEmp_Permanent_Barangay { get; set; }
        public bool HasEmp_Permanent_Municipality { get; set; }
        public bool HasEmp_Permanent_Province { get; set; }
        public bool HasEmp_Permanent_ZipCode { get; set; }
        public bool HasEmp_Permanent_PhoneNumber { get; set; }
        //BFP Information
        public bool HasEmp_Service_GovtStartDate { get; set; }
        public bool YearsGovtServiceStartDate { get; set; }
        public bool HasEmp_Service_UniformGovtStartDate { get; set; }
        public bool HasEmp_Service_StartDate { get; set; }
        public bool HasEmp_LastPromotionDate_Temp { get; set; }
        public bool HasEmp_LastPromotionDate_Permanent { get; set; }
        public bool HasEmp_AssumedOfficerDate { get; set; }
        public bool HasEmp_LastTrainingDate { get; set; }
        public bool HasEmp_ItemNumber { get; set; }
        public bool HasEmp_BadgeNumber { get; set; }
        public bool HasEmp_Curr_Rank { get; set; }
        public bool HasEmp_PresentAsgmt_DO_BO_RO { get; set; }
        public bool HasEmp_Curr_ApptStatus { get; set; }
        public bool HasEmp_AppointmentStatus_DO_BO_RO { get; set; }
        public bool HasEmp_DutyStatus { get; set; }
        public bool HasEmp_Curr_SalaryGrade { get; set; }
        public bool HasEmp_Curr_JobFunc { get; set; }
        public bool HasEmp_Curr_PosDesignationTitle { get; set; }
        public bool HasEmp_Remarks { get; set; }
        public bool HasEmp_EducCourse { get; set; }
        public bool HasEmp_MACourse { get; set; }
        public bool HasEmp_HighestEducAttainment { get; set; }
        public bool HasEmp_Eligibility_Type { get; set; }
        public bool HasEmp_Curr_Eligibility { get; set; }
        public bool HasEmp_HighestMandatoryTraining { get; set; }
        public bool YearsUniformedFireService { get; set; }
        public bool MandatoryRetirementDate { get; set; }  
        public bool UniformedOptionalRetirementDate { get; set; }
        public bool NonUniformedOptionalRetirementDate { get; set; }

        public string Column_NonUniformedOptionalRetirementDate { get; set; }
        public string Column_UniformedOptionalRetirementDate { get; set; }
        public string Column_YearsUniformedFireService { get; set; }
        public string Column_MandatoryRetirementDate { get; set; }
        public string Column_YearsGovtServiceStartDate { get; set; }
        public string Column_Emp_Number { get; set; }
        public string Column_Emp_LastName { get; set; }
        public string Column_Emp_FirstName { get; set; }
        public string Column_Emp_MiddleName { get; set; }
        public string Column_Emp_SuffixName { get; set; }
        public string Column_Emp_BP_Number { get; set; }
        public string Column_Emp_Tax_Code { get; set; }
        public string Column_Emp_Atm_Number { get; set; }
        public string Column_Emp_DES { get; set; }
        public string Column_Emp_Curr_Unit { get; set; }
        public string Column_Emp_BirthDate { get; set; }

        public string Column_Emp_CivilStatus { get; set; }
        public string Column_Emp_Citizenship { get; set; }

        public string Column_Emp_Bybirth_ByNaturalization { get; set; }
        public string Column_Emp_Citizenship_Country { get; set; }


        public string Column_Emp_BirthPlace { get; set; }
        public string Column_Emp_Gender { get; set; }
        public string Column_Emp_Height { get; set; }
        public string Column_Emp_Weight { get; set; }
        public string Column_Emp_BloodType { get; set; }
        public string Column_Emp_GSISNumber { get; set; }
        public string Column_Emp_PAGIBIGNumber { get; set; }
        public string Column_Emp_PHICNumber { get; set; }
        public string Column_Emp_SSSNumber { get; set; }
        public string Column_Emp_TINNumber { get; set; }
        public string Column_Emp_Religion { get; set; }
        public string Column_Emp_OtherReligion { get; set; }
        public string Column_Emp_EmailAddress { get; set; }
        public string Column_Emp_MobileNumber { get; set; }
        public string Column_Emp_AgencyEmpNumber { get; set; }
        public string Column_Emp_Residential_HouseNo { get; set; }
        public string Column_Emp_Residential_Street { get; set; }
        public string Column_Emp_Residential_Village { get; set; }
        public string Column_Emp_Residential_Barangay { get; set; }
        public string Column_Emp_Residential_Municipality { get; set; }
        public string Column_Emp_Residential_Province { get; set; }
        public string Column_Emp_Residential_ZipCode { get; set; }
        public string Column_Emp_Residential_PhoneNumber { get; set; }
        public string Column_Emp_Permanent_HouseNo { get; set; }
        public string Column_Emp_Permanent_Street { get; set; }
        public string Column_Emp_Permanent_Village { get; set; }
        public string Column_Emp_Permanent_Barangay { get; set; }
        public string Column_Emp_Permanent_Municipality { get; set; }
        public string Column_Emp_Permanent_Province { get; set; }
        public string Column_Emp_Permanent_ZipCode { get; set; }
        public string Column_Emp_Permanent_PhoneNumber { get; set; }
        //BFP Information
        public string Column_Emp_Service_GovtStartDate { get; set; }
        public string Column_Emp_Service_UniformGovtStartDate { get; set; }
        public string Column_Emp_Service_StartDate { get; set; }
        public string Column_Emp_LastPromotionDate_Temp { get; set; }
        public string Column_Emp_LastPromotionDate_Permanent { get; set; }
        public string Column_Emp_AssumedOfficerDate { get; set; }
        public string Column_Emp_LastTrainingDate { get; set; }
        public string Column_Emp_ItemNumber { get; set; }
        public string Column_Emp_BadgeNumber { get; set; }
        public string Column_Emp_Curr_Rank { get; set; }
        public string Column_Emp_PresentAsgmt_DO_BO_RO { get; set; }
        public string Column_Emp_Curr_ApptStatus { get; set; }
        public string Column_Emp_AppointmentStatus_DO_BO_RO { get; set; }
        public string Column_Emp_DutyStatus { get; set; }
        public string Column_Emp_Curr_SalaryGrade { get; set; }
        public string Column_Emp_Curr_JobFunc { get; set; }
        public string Column_Emp_Curr_PosDesignationTitle { get; set; }
        public string Column_Emp_Remarks { get; set; }
        public string Column_Emp_EducCourse { get; set; }
        public string Column_Emp_MACourse { get; set; }
        public string Column_Emp_HighestEducAttainment { get; set; }
        public string Column_Emp_Eligibility_Type { get; set; }
        public string Column_Emp_Curr_Eligibility { get; set; }
        public string Column_Emp_HighestMandatoryTraining { get; set; }
    }

    public class EmployeeInventoryDashModel
    {
        public tblRanks tblRanks { get; set; }
    }
}