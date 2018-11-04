namespace EBFP.BL.HumanResources
{
	using EBFP.BL.Helper;
	using EBFP.Helper;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class PhysicalExamListResult
	{
		public GridInfo DatatableInfo { get; set; }
		public List<PhysicalExamTableModel> PhysicalExamListModel { get; set; }
	}

    public class PhysicalExamTableModel
    {
        public string sPE_Id { get; set; }
        public int PE_Id { get; set; }
        public Nullable<System.DateTime> PE_Date { get; set; }
    }

    public class PhysicalExamListModel
	{
        public string sPE_Id { get; set; }
        public int PE_Id { get; set; }
        public Nullable<System.DateTime> PE_Date { get; set; }
        public int PE_CategoryId { get; set; }
        public string PE_Details_Json { get; set; }
    }


    public class PhysicalExamModel
    {
        public PhysicalExamModel()
        {
            this.General = new GeneralModel();
            this.Head = new HeadModel();
            this.Eyes = new EyesModel();
            this.ENT = new ENTModel();
            this.Neck = new NeckModel();
            this.Respiratory = new RespiratoryModel();
            this.Cardiac = new CardiacModel();
            this.Vascular = new VascularModel();
            this.ChestBreast = new ChestBreastModel();
            this.GI = new GIModel();
            this.GU = new GUModel();
            this.Lymph = new LymphModel();
            this.DiagnosisList = new List<PhysicalExamDiagnosisModel>();
            this.Musculoskeletal = new MusculoskeletalModel();
            this.Skin = new SkinModel();
            this.Psych = new PsychModel();
            this.Neuro = new NeuroModel();

            this.Deferred = new DeferredModel();
            this.FamilyHistory = new FamilyHistoryModel();
            this.PatientInformation = new PatientInformationModel();
            this.HealthMaintenance = new HealthMaintenanceModel();
            this.PhysicalExamOtherInfo = new PhysicalExamOtherInfoModel();  
        }
        
        public GeneralModel General { get; set; }
        public HeadModel Head { get; set; }
        public EyesModel Eyes { get; set; }
        public ENTModel ENT { get; set; }
        public NeckModel Neck { get; set; }
        public RespiratoryModel Respiratory { get; set; }
        public CardiacModel Cardiac { get; set; }
        public VascularModel Vascular { get; set; }
        public ChestBreastModel ChestBreast { get; set; }
        public GIModel GI { get; set; }
        public GUModel GU { get; set; }
        public LymphModel Lymph { get; set; }

        public MusculoskeletalModel Musculoskeletal { get; set; }
        public SkinModel Skin { get; set; }
        public PsychModel Psych { get; set; }
        public NeuroModel Neuro { get; set; }
        
        public DeferredModel Deferred { get; set; }
        public FamilyHistoryModel FamilyHistory { get; set; }
        public PatientInformationModel PatientInformation { get; set; }
        public HealthMaintenanceModel HealthMaintenance { get; set; }
        public PhysicalExamOtherInfoModel PhysicalExamOtherInfo { get; set; }

        public List<PhysicalExamDiagnosisModel> DiagnosisList { get; set; }
    }

    public class PhysicalExamOtherInfoModel
    {
        //Misc
        public string PE_Misc_Text { get; set; }
        public string PE_Lab_Results { get; set; }
        public string PE_Radiology_Results { get; set; }

        public string PE_RTC { get; set; }
        public string PE_Referrals { get; set; }
        public string PE_Other_Orders { get; set; }
        public string PE_Print_Provider_Name { get; set; }
        public string PE_Group_Name { get; set; }
        public string PE_Provider_Id { get; set; }
        public string PE_Tax_Id_number { get; set; }
        public string PE_Provider_Address { get; set; }
        public string PE_City_States_Zip { get; set; }
        public string PE_Provider_Signature { get; set; }
        public int PE_Provider_Type { get; set; }
        public Nullable<System.DateTime> PE_Date { get; set; }
    }

    public class HealthMaintenanceModel
    {
        public bool PE_Atleast_One_PCP { get; set; }
        public bool PE_Breast_Cancer_Screening { get; set; }
        public bool PE_Colorectal_Cancer_Screening { get; set; }
        public bool PE_Cholesterol_Screening_Diabetes { get; set; }
        public bool PE_Cholesterol_Screening_Heart { get; set; }
        public bool PE_Glaucoma_Testing { get; set; }
        public bool PE_Monitoring_LongTerm_Medication { get; set; }
        public bool PE_ACE { get; set; }
        public bool PE_ARB { get; set; }
        public bool PE_Digoxin { get; set; }
        public bool PE_Diurectics { get; set; }
        public bool PE_Anticonvulsants { get; set; }
    }

    public class PatientInformationModel
    {
        [Required]
        public string sPE_Id
        {
            get
            {
                var encryptID = this.PE_Id.ToString().Encrypt();
                return encryptID;
            }
        }
        public int PE_Id { get; set; }
        public int Emp_Id { get; set; }
        public Nullable<System.DateTime> PE_Service_Date { get; set; }
        public string PE_Patient_Name { get; set; }
        public string PE_DOB { get; set; }
        public string PE_Member_Id { get; set; }
        public string PE_Health_Plan { get; set; }
        public string PE_Provider_Name { get; set; }
        public string PE_Allergies { get; set; }
        public string PE_Height { get; set; }
        public string PE_Weight { get; set; }
        public string PE_BMI { get; set; }
        public string PE_Sex { get; set; }
        public string PE_RR { get; set; }
        public string PE_HR { get; set; }
        public string PE_BP { get; set; }
        public string PE_Oxygen_Status { get; set; }
        public bool PE_Oxygen_Use { get; set; }
        public bool PE_WheelChair_Dependent { get; set; }
        public string PE_Chief_Complaint { get; set; }
        public string PE_History_Present_Illness { get; set; }
    }

    public class FamilyHistoryModel
    {
        public bool PE_Fam_History_NonContributory { get; set; }
        public bool PE_Fam_History_Pos_CKD { get; set; }
        public bool PE_Fam_History_Pos_HeartDisease { get; set; }
        public bool PE_Fam_History_Pos_Cancer { get; set; }
        public bool PE_Fam_History_Pos_Diabetes { get; set; }
        public bool PE_Fam_History_Pos_Other { get; set; }
        public string PE_Fam_History_Pos_Other_Text { get; set; }
        public string PE_Past_Surgery { get; set; }
        public string PE_Social_Birthplace { get; set; }
        public string PE_Social_Primary_Language { get; set; }
        public int PE_Social_Employed { get; set; }
        public string PE_Social_Occupation { get; set; }
        public int PE_Social_CivilStatus { get; set; }
        public int PE_Social_NoOfChildren { get; set; }
    }

    public class DeferredModel
    {
        public bool PE_General_Deferred { get; set; }
        public bool PE_Head_Deferred { get; set; }
        public bool PE_Eyes_Deferred { get; set; }
        public bool PE_ENT_Deferred { get; set; }
        public bool PE_Neck_Deferred { get; set; }
        public bool PE_Respiratory_Deferred { get; set; }
        public bool PE_Cardiac_Deferred { get; set; }
        public bool PE_Vascular_Deferred { get; set; }
        public bool PE_Chest_Breast_Deferred { get; set; }
        public bool PE_GI_Deferred { get; set; }
        public bool PE_GU_Deferred { get; set; }
        public bool PE_LYMPH_Deferred { get; set; }
        public bool PE_Musculoskeletal_Deferred { get; set; }
        public bool PE_Skin_Deferred { get; set; }
        public bool PE_Psych_Deferred { get; set; }
        public bool PE_Neuro_Deferred { get; set; }
        public bool PE_Misc_Deferred { get; set; }
    }

    public class PhysicalExamDiagnosisModel
    {
        public bool CreatedFromAjax { get; set; }
        public int PE_Diagnosis_Id { get; set; }
        public string PE_Diagnosis_Desc { get; set; }
        public int PE_Diagnosis_Status { get; set; }
        public string PE_Diagnosis_Plan_Care { get; set; }
        public string PE_Diagnosis_Current_Rx { get; set; }
    }
    
    public class GeneralModel
    {
        public bool PE_Gen_Appearance_WellNourished { get; set; }
        public bool PE_Gen_Appearance_WellDeveloped { get; set; }
        public bool PE_Gen_Appearance_Other { get; set; }
        public string PE_Gen_Appearance_Other_Text { get; set; }

        public bool PE_Gen_Alert { get; set; }
        public bool PE_Gen_Anxiuos { get; set; }

        ////Physical Exam - General > Level of Distress
        public bool PE_LD_NoAcuteDistress { get; set; }
        public bool PE_LD_Mild { get; set; }
        public bool PE_LD_Moderate { get; set; }
        public bool PE_LD_Severe { get; set; }
        public string PE_Race { get; set; }
        public string PE_Gen_Abnormal_Findings { get; set; }
    }

    public class HeadModel
    {  
        public bool PE_FacialFeaturesSymmetric { get; set; }
        public bool PE_SkullNormocephalic { get; set; }
        public bool PE_HairScalpNormal { get; set; }
        public string PE_Head_Abnormal_Findings { get; set; }
    }

    public class EyesModel
    {
        public bool PE_Vision_Normal { get; set; }
        public int PE_Lids { get; set; }
        public bool PE_Conjunctivae { get; set; }
        public bool PE_PERRLA { get; set; }
        public string PE_PERRLA_Text { get; set; }
        public bool PE_ScleralIcterus { get; set; }
        public bool PE_PaleConjunctivae { get; set; }
        public bool PE_EOM_Normal { get; set; }
        public bool PE_AV_Nicking { get; set; }
        public bool PE_VisualAcuity { get; set; }
        public string PE_VisualAcuity_Left { get; set; }
        public string PE_VisualAcuity_Right { get; set; }
        public string PE_Eyes_Abnormal_Findings { get; set; }
    }

    public class ENTModel
    { 
        public bool PE_ENT_Inspection_Normal { get; set; }
        public bool PE_Throat_Normal { get; set; }
        public bool PE_MucusMembranes_PinkMoist { get; set; }
        public bool PE_NasalSeptum_Normal { get; set; }
        public bool PE_TMs_Normal { get; set; }
        public bool PE_AuditoryCanal_Normal { get; set; }
        public bool PE_HearingGrosslyIntact { get; set; }
        public bool PE_SinusTenderness { get; set; }
        public string PE_SinusTenderness_Location { get; set; }
        public string PE_ENT_Abnormal_Findings { get; set; }
    }

    public class NeckModel
    { 
        public bool PE_Supple_Normal { get; set; }
        public bool PE_No_Cervical_Adenopathy { get; set; }
        public bool PE_Thyroid_Normal { get; set; }
        public bool PE_Thyromegaly { get; set; }
        public bool PE_Nodules_Present { get; set; }
        public bool PE_JVP_Absent { get; set; }
        public bool PE_JVD_Present { get; set; }
        public int PE_JVD_Present_Location { get; set; }
        public bool PE_CarotidBruit_Present { get; set; }
        public int PE_CarotidBruit_Present_Location { get; set; }
        public string PE_Neck_Abnormal_Findings { get; set; }
    }

    public class RespiratoryModel
    {
        public bool PE_Lungs_Clear_Bilaterally { get; set; }
        public bool PE_No_ChestWall_Tenderness { get; set; }
        public bool PE_Cough_Absent { get; set; }
        public bool PE_Percussion_Normal { get; set; }
        public bool PE_SOB { get; set; }
        public bool PE_Crackles_Present { get; set; }
        public string PE_Crackles_Present_Text { get; set; }
        public bool PE_Rhonchi_Present { get; set; }
        public string PE_Rhonchi_Present_Text { get; set; }
        public bool PE_Wheezes_Present { get; set; }
        public string PE_Wheezes_Present_Text { get; set; }
        public bool PE_Tracheotomy_Present { get; set; }
        public string PE_Year_Placed { get; set; }
        public string PE_Respiratory_Abnormal_Findings { get; set; }
    }

    public class CardiacModel
    { 
        public bool PE_Normal_S1_S2 { get; set; }
        public bool PE_S3_Present { get; set; }
        public bool PE_S4_Present { get; set; }
        public bool PE_Rate_Normal { get; set; }
        public bool PE_Tachycardia { get; set; }
        public bool PE_Bradycardia { get; set; }
        public bool PE_Rhythm_Regular { get; set; }
        public bool PE_Rubs_Present { get; set; }
        public bool PE_No_Murmurs { get; set; }
        public string PE_Murmur_Location { get; set; }
        public string PE_Cardio_Abnormal_Findings { get; set; }
    }

    public class VascularModel
    {  
        public bool PE_PedalPulses_Normal { get; set; }
        public bool PE_LowerExtremities { get; set; }
        public bool PE_Warm_Cool { get; set; }
        public bool PE_No_Varicosities { get; set; }
        public bool PE_Venous_Statis_Absent { get; set; }
        public bool PE_HairLossNoticable { get; set; }
        public bool PE_No_Cyanosis { get; set; }
        public bool PE_No_Ulceration_Present { get; set; }
        public bool PE_No_Edema { get; set; }
        public bool PE_No_CalfTenderness { get; set; }
        public bool PE_No_Clubbing { get; set; }
        public bool PE_Edema_Present { get; set; }
        public string PE_Edema_Location { get; set; }
        public string PE_Vascular_Abnormal_Findings { get; set; }
    }

    public class ChestBreastModel
    {
        public bool PE_ChestGrossly { get; set; }
        public bool PE_BreastExam_Deferred { get; set; }
        public bool PE_No_Breast_Dippling { get; set; }
        public bool PE_No_Drainage { get; set; }
        public bool PE_No_Breast_Masses { get; set; }
        public bool PE_No_Chest_Breast_Nodules { get; set; }
        public bool PE_No_Nipple_Inversion { get; set; }
        public bool PE_No_Axillary_Nodes_Bilaterally { get; set; }
        public bool PE_Breast_Absent { get; set; }
        public bool PE_Breast_Absent_Left { get; set; }
        public bool PE_Breast_Absent_Right { get; set; }
        public bool PE_Breast_Implant { get; set; }
        public bool PE_Breast_Implant_Left { get; set; }
        public bool PE_Breast_Implant_Right { get; set; }
        public bool PE_Cosmetic { get; set; }
        public bool PE_Reconstructive { get; set; }
        public string PE_Chest_Breast_Abnormal_Findings { get; set; }
    }

    public class GIModel
    {
        public bool PE_Abdomen_Symmetrical { get; set; }
        public bool PE_No_Abnormal_Distension { get; set; }
        public bool PE_Mass { get; set; }
        public string PE_Mass_Locations { get; set; }
        public bool PE_Percussion_Wthin_Normal_Limits { get; set; }
        public bool PE_Soft { get; set; }
        public bool PE_No_Tenderness { get; set; }
        public bool PE_Scars_Present { get; set; }
        public bool PE_Hernias_Present { get; set; }
        public bool PE_Organomegaly { get; set; }
        public bool PE_Auscultation { get; set; }
        public bool PE_BS_Hypoactive { get; set; }
        public bool PE_BS_Hyperactive { get; set; }
        public bool PE_BS_Absent { get; set; }
        public bool PE_Rectal_Exam_Deferred { get; set; }
        public bool PE_Rectal_Exam_Reveals { get; set; }
        public bool PE_Stool_Brown { get; set; }
        public bool PE_Deep_Palpation_Normal { get; set; }
        public bool PE_Ostomy_Present { get; set; }
        public string PE_Ostomy_Present_Text { get; set; }
        public bool PE_Stool_Guaiac { get; set; }
        public bool PE_Stool_Guaiac_Positive { get; set; }
        public bool PE_Internal_External_Hemorrhoids { get; set; }
        public bool PE_Sphincter_Tone_Poor { get; set; }
        public string PE_GI_Abnormal_Findings { get; set; }
    }

    public class GUModel
    { 
        public bool PE_CVA_Tenderness { get; set; }
        public bool PE_Suprapubic_Tenderness { get; set; }
        public bool PE_Urostomy_Present { get; set; }
        public bool PE_Urostomy_Yes_No { get; set; }
        public bool PE_Prostate_Exam_Deferred { get; set; }
        public bool PE_Prostate_Exam_Normal { get; set; }
        public bool PE_Prostate_Enlargement { get; set; }
        public bool PE_Prostate_Tenderness { get; set; }
        public bool PE_Prostate_Nodules { get; set; }
        public bool PE_Pelvic_Deferred { get; set; }
        public bool PE_Pelvic_Normal { get; set; }
        public bool PE_Uterus_Absent { get; set; }
        public bool PE_Cervix_Absent { get; set; }
        public bool PE_Kidney_Transplant { get; set; }
        public bool PE_Kidney_Left { get; set; }
        public bool PE_Kidney_Right { get; set; }
        public bool PE_Dialysis_Status { get; set; }
        public bool PE_Dialysis_Status_Yes_No { get; set; }
        public string PE_GU_Abnormal_Findings { get; set; }
    }

    public class LymphModel
    {
        public bool PE_Palpation_Lymph { get; set; }
        public string PE_Palpation_Lymph_Neck { get; set; }
        public string PE_Palpation_Lymph_Axilla { get; set; }
        public string PE_Palpation_Lymph_Groin { get; set; }
        public string PE_Palpation_Lymph_OtherSite { get; set; }
        public bool PE_No_Lymph_Enlargement { get; set; }
        public bool PE_Lymphadenopathy_Present { get; set; }
        public bool PE_Lymphadenopathy_Anterior_Cervical { get; set; }
        public bool PE_Lymphadenopathy_Posterior_Cervical { get; set; }
        public bool PE_Lymphadenopathy_Postauricular { get; set; }
        public bool PE_Lymphadenopathy_Submental { get; set; }
        public bool PE_Supraclavicular { get; set; }
        public bool PE_Inguinal { get; set; }
        public bool PE_Axillary { get; set; }
        public string PE_Lymph_Abnormal_Findings { get; set; }
    }


    public class MusculoskeletalModel
    {
        public bool PE_No_Joint_Abnormality { get; set; }
        public bool PE_Amputations { get; set; }
        public string PE_Amputations_Text { get; set; }
        public bool PE_Joint_Abnormality { get; set; }
        public string PE_Joint_Abnormality_Text { get; set; }
        public bool PE_Kyphosis { get; set; }
        public bool PE_Scoliosis { get; set; }
        public bool PE_Osteoarthritis { get; set; }
        public bool PE_Bouchards_Nodes_Present { get; set; }
        public bool PE_Heberdens_Nodes_Present { get; set; }
        public bool PE_Paronychia_Present { get; set; }
        public bool PE_Swelling_Present { get; set; }
        public string PE_Swelling_Present_Location { get; set; }
        public string PE_Peripheral_Findings { get; set; }
        public string PE_Central_Findings { get; set; }
        public string PE_Other_Abnormal_Findings { get; set; }
    }

    public class SkinModel
    {
        public bool PE_Skin_Warm_Dry_Intact { get; set; }
        public bool PE_Good_Skin_Turgor { get; set; }
        public bool PE_No_Rashes { get; set; }
        public bool PE_Poor_Skin_Turgor { get; set; }
        public bool PE_No_Abnormal_Lesions { get; set; }
        public bool PE_No_Ulcers { get; set; }
        public bool PE_Cyanosis_Present { get; set; }
        public bool PE_Diaphoresis_Present { get; set; }
        public bool PE_Nails { get; set; }
        public bool PE_Foot_Exam { get; set; }
        public bool PE_Scars { get; set; }
        public bool PE_Ulcer_Present { get; set; }
        public string PE_Ulcer_Type { get; set; }
        public string PE_Ulcer_Location { get; set; }
        public string PE_Ulcer_Stage { get; set; }
        public string PE_Skin_Abnormal_Findings { get; set; }
    }

    public class PsychModel
    {
        public bool PE_Mood_Affect { get; set; }
        //public string PE_Mood_Affect_Text { get; set; }
        public string PE_Nomral_Text { get; set; }
        public string PE_Depressed_Text { get; set; }
        public string PE_Anxiuos_Text { get; set; }
        public string PE_Agitated_Text { get; set; }
        public string PE_Psych_Abnormal_Findings { get; set; }
    }

    public class NeuroModel
    {
        public bool PE_Orientation { get; set; }
        public string PE_Time { get; set; }
        public string PE_Place { get; set; }
        public string PE_Person { get; set; }
        public string PE_Other { get; set; }
        public bool PE_Follow_Commands { get; set; }

        //Nuero
        public bool PE_Hearing { get; set; }
        public int PE_Hearing_Type { get; set; }
        public bool PE_Sense_Smell_Normal { get; set; }
        public bool PE_Gait { get; set; }
        public string PE_Gait_Text { get; set; }
        public bool PE_Balance { get; set; }
        public string PE_Balance_Text { get; set; }
        public bool PE_Gross_Motor { get; set; }
        public string PE_Gross_Motor_Text { get; set; }
        public bool PE_Fine_Motor { get; set; }
        public string PE_Fine_Motor_Text { get; set; }
        public bool PE_DTR_Upper { get; set; }
        public string PE_Upper_RT { get; set; }
        public string PE_Upper_LT { get; set; }
        public bool PE_DTR_Lower { get; set; }
        public string PE_Lower_RT { get; set; }
        public string PE_Lower_LT { get; set; }
        public bool PE_LOPS { get; set; }
        public bool PE_Normal_PinPrick_Sensation { get; set; }
        public bool PE_Tremors { get; set; }
        public bool PE_Coordination { get; set; }
        public string PE_Coordination_Text { get; set; }
        public bool PE_Speech { get; set; }
        public string PE_Speech_Text { get; set; }
        public bool PE_Vibration { get; set; }
        public bool PE_Vibration_RT { get; set; }
        public bool PE_Vibration_LT { get; set; }
        public bool PE_Monofilament_Testing { get; set; }
        public bool PE_Monofilament_RT { get; set; }
        public bool PE_Monofilament_LT { get; set; }
        public bool PE_CN_II_XII { get; set; }
        public string PE_Nuero_Abnormal_Findings { get; set; }
    }

}