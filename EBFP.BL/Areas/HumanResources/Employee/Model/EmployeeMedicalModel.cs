namespace EBFP.BL.HumanResources
{
    using EBFP.BL.Helper;
    using EBFP.Helper;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public partial class EmployeeMedicalModel
    {
        public EmployeeMedicalModel()
        {
            //SET DEFAULT VALUES
            this.CurrentMedication = new List<CurrentMedicationModel>();
            this.HealthCareProvider = new List<HealthCareProviderModel>();
            this.AllergicReaction = new List<AllergicReactionModel>();
            this.HealthRecord = new List<HealthRecordModel>();
            this.MedicalHistory = new MedicalHistoryModel();
            this.PastSurgicalHistory = new List<PastSurgicalHistoryModel>();
            this.MedicalEmployeeInfo = new MedicalEmployeeInfoModel();
            this.MedicalAdditionalInformation = new MedicalAdditionalInformationModel();
        }

        
        public List<CurrentMedicationModel> CurrentMedication { get; set; }
        public List<HealthCareProviderModel> HealthCareProvider { get; set; }
        public List<AllergicReactionModel> AllergicReaction { get; set; }
        public List<HealthRecordModel> HealthRecord { get; set; }
        public MedicalHistoryModel MedicalHistory { get; set; }
        public List<PastSurgicalHistoryModel> PastSurgicalHistory { get; set; }
        public MedicalEmployeeInfoModel MedicalEmployeeInfo { get; set; }
        public MedicalAdditionalInformationModel MedicalAdditionalInformation { get; set; }
    }

    public partial class PastSurgicalHistoryModel
    {
        public PastSurgicalHistoryModel()
        {
        }
        public bool CreatedFromAjax { get; set; }

        public int SH_ID { get; set; }
        [Required]
        public Nullable<System.DateTime> SH_Date { get; set; }
        [Required]
        public string SH_Description { get; set; }
    }

    public class MedicalEmployeeInfoModel
    {
        public int Emp_Id { get; set; }
        public int Med_Id { get; set; }
        public string sEmp_Id { get; set; }
        public string sMed_Id { get; set; }

        [Display(Name = "Employee Number")]
        [StringLength(10)]
        public string Emp_Number { get; set; }


        [Display(Name = "First Name")]
        public string Emp_FirstName { get; set; }


        [Display(Name = "Middle Name")]
        public string Emp_MiddleName { get; set; }


        [Display(Name = "Last Name")]
        public string Emp_LastName { get; set; }

        [Display(Name = "Suffix Name")]
        public string Emp_SuffixName { get; set; }

        [Display(Name = "Birth Date")]
        public Nullable<System.DateTime> Emp_BirthDate { get; set; }


        [Display(Name = "Gender")]
        public string Emp_Gender { get; set; }


        [Display(Name = "Civil Status")]
        public int Emp_CivilStatus { get; set; }

        public string Emp_CivilStatus_Other { get; set; }


        [Display(Name = "Citizenship")]
        public string Emp_Citizenship { get; set; }

        [Display(Name = "Age")]
        public string Emp_Age { get; set; }

        [Display(Name = "Allergies")]
        public string Emp_Allergies { get; set; }

        public string Emp_BloodType { get; set; }
        public string Emp_Religion { get; set; }
        public string Emp_Curr_Rank { get; set; }
        public string Emp_Curr_Unit { get; set; }
        public string Emp_Curr_JobFunc { get; set; }
        public string Emp_MobileNumber { get; set; }

        [Display(Name = "Residential Address")]
        public string Emp_Residential_Address { get; set; }
    }

    public class MedicalAdditionalInformationModel
    {
        //Current Medication
        public string Med_Preffered_Pharmacy { get; set; }
        public string Med_Location { get; set; }

        //Additional Information
        public Nullable<System.DateTime> Med_LastMammogram_Date { get; set; }
        public string Med_LastMammogram_Where { get; set; }
        public Nullable<System.DateTime> Med_LastPap { get; set; }
        public string Med_Gyn { get; set; }
        public bool Med_FuturePap { get; set; }
        public Nullable<System.DateTime> Med_LastColonoscopy_Date { get; set; }
        public string Med_LastColonoscopy_Normal { get; set; }
        public string Med_LastColonoscopy_Dr { get; set; }
        public Nullable<System.DateTime> Med_ReapeatColonoscopy_Date { get; set; }
        public Nullable<System.DateTime> Med_LastBloodWork_Date { get; set; }
        public string Med_RectalExam { get; set; }
        public Nullable<System.DateTime> Med_Tetanus_Date { get; set; }
        public Nullable<System.DateTime> Med_Pnuemonia_Date { get; set; }
        public Nullable<System.DateTime> Med_Flu_Date { get; set; }
        public Nullable<System.DateTime> Med_Hepatitis_Date { get; set; }

    }

}