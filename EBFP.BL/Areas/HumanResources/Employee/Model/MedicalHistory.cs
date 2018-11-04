namespace EBFP.BL.HumanResources
{
    using EBFP.BL.Helper;
    using EBFP.Helper;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class MedicalHistoryModel
    {
        //Medical History
        public Nullable<System.DateTime> MH_History_Date { get; set; }
        public int MH_NoOfChildren { get; set; }
        public bool MH_Tobacco_Use { get; set; }
        public string MH_Tobacco_HowMuch_PerDay { get; set; }
        public string MH_Tobacco_HowLong { get; set; }
        public Nullable<System.DateTime> MH_Tobacco_Quit_Date { get; set; }
        public bool MH_Alcohol_Use { get; set; }
        public string MH_Alcohol_HowMuch_PerDay { get; set; }
        public int MH_Caffeine { get; set; }
        public string MH_Caffeine_HowMuch_PerDay { get; set; }

        //Past Illness of yourself and family
        public bool MH_Self_Alcoholism { get; set; }
        public bool MH_Self_Anemia { get; set; }
        public bool MH_Self_Asthma { get; set; }
        public bool MH_Self_Cancer { get; set; }
        public bool MH_Self_Diabetes { get; set; }
        public bool MH_Self_DrugAbuse { get; set; }
        public bool MH_Self_Depression { get; set; }
        public bool MH_Self_Epilepsy { get; set; }
        public bool MH_Self_Glaucoma { get; set; }
        public bool MH_Self_HeartDisease { get; set; }
        public bool MH_Self_HighBlood { get; set; }
        public bool MH_Self_KidneyDisease { get; set; }
        public bool MH_Self_LiverDisease { get; set; }
        public bool MH_Self_Hepatitis { get; set; }
        public bool MH_Self_LungDisease { get; set; }
        public bool MH_Self_MentalIllness { get; set; }
        public bool MH_Self_OsteoArthritis { get; set; }
        public bool MH_Self_OsteoPorosis { get; set; }
        public bool MH_Self_Phlebitis { get; set; }
        public bool MH_Self_RheumaticArthritis { get; set; }
        public bool MH_Self_Stroke { get; set; }
        public bool MH_Self_SuicideAttempt { get; set; }
        public bool MH_Self_ThyroidDisease { get; set; }
        public bool MH_Self_Tubercolosis { get; set; }
        public bool MH_Self_Ulcer { get; set; }
        public bool MH_Self_VenerealDisease { get; set; }
        public bool MH_Self_HighCholesterol { get; set; }
        public bool MH_Self_HIV { get; set; }
        public bool MH_Self_Other { get; set; }
        public string MH_Self_Other_Text { get; set; }

        public bool MH_Fam_Alcoholism { get; set; }
        public bool MH_Fam_Anemia { get; set; }
        public bool MH_Fam_Asthma { get; set; }
        public bool MH_Fam_Cancer { get; set; }
        public bool MH_Fam_Diabetes { get; set; }
        public bool MH_Fam_DrugAbuse { get; set; }
        public bool MH_Fam_Depression { get; set; }
        public bool MH_Fam_Epilepsy { get; set; }
        public bool MH_Fam_Glaucoma { get; set; }
        public bool MH_Fam_HeartDisease { get; set; }
        public bool MH_Fam_HighBlood { get; set; }
        public bool MH_Fam_KidneyDisease { get; set; }
        public bool MH_Fam_LiverDisease { get; set; }
        public bool MH_Fam_Hepatitis { get; set; }
        public bool MH_Fam_LungDisease { get; set; }
        public bool MH_Fam_MentalIllness { get; set; }
        public bool MH_Fam_OsteoArthritis { get; set; }
        public bool MH_Fam_OsteoPorosis { get; set; }
        public bool MH_Fam_Phlebitis { get; set; }
        public bool MH_Fam_RheumaticArthritis { get; set; }
        public bool MH_Fam_Stroke { get; set; }
        public bool MH_Fam_SuicideAttempt { get; set; }
        public bool MH_Fam_ThyroidDisease { get; set; }
        public bool MH_Fam_Tubercolosis { get; set; }
        public bool MH_Fam_Ulcer { get; set; }
        public bool MH_Fam_VenerealDisease { get; set; }
        public bool MH_Fam_HighCholesterol { get; set; }
        public bool MH_Fam_HIV { get; set; }
        public bool MH_Fam_Other { get; set; }
        public string MH_Fam_Other_Text { get; set; }


        //Constitutional
        public bool MH_WeightLoss { get; set; }
        public bool MH_Fatigue { get; set; }
        public bool MH_Fever { get; set; }

        //Respiratory
        public bool MH_Cough { get; set; }
        public bool MH_CoughingBlood { get; set; }
        public bool MH_Wheezing { get; set; }
        public bool MH_Chills { get; set; }

        //Hematology/Lymph
        public bool MH_EasyBruising { get; set; }
        public bool MH_GumsBleedEasily { get; set; }
        public bool MH_EnlargeGlands { get; set; }

        //Eyes
        public bool MH_Glasses_Contacts { get; set; }
        public bool MH_EyePain { get; set; }
        public bool MH_DoubleVision { get; set; }
        public bool MH_Cataracts { get; set; }

        //Gastro Intestinal
        public bool MH_Heartburn { get; set; }
        public bool MH_Nausea { get; set; }
        public bool MH_Constipation { get; set; }
        public bool MH_ChangeInBM { get; set; }
        public bool MH_Diarrhea { get; set; }
        public bool MH_Jaundince { get; set; }
        public bool MH_AbdominalPain { get; set; }
        public bool MH_BlackOrBloodyBM { get; set; }

        //Musculokeletal
        public bool MH_JointPain { get; set; }
        public bool MH_Stiffness { get; set; }
        public bool MH_MusclePain { get; set; }
        public bool MH_BackPain { get; set; }

        //Ear Nose Throat
        public bool MH_DifficultyHearing { get; set; }
        public bool MH_RingingInEars { get; set; }
        public bool MH_Vertigo { get; set; }
        public bool MH_SinusTrouble { get; set; }
        public bool MH_NasalStuffiness { get; set; }
        public bool MH_FrequentSoreThroat { get; set; }

        //Genitourinary
        public bool MH_Gen_Burning { get; set; }
        public bool MH_Gen_NightTime { get; set; }
        public bool MH_Gen_BloodInUrine { get; set; }
        public bool MH_Gen_ErectileDysfunction { get; set; }
        public bool MH_Gen_AbnormalDischarge { get; set; }
        public bool MH_Gen_BladderLeakage { get; set; }

        //Skin
        public bool MH_Rash { get; set; }
        public bool MH_Lesions { get; set; }
        public bool MH_Itching { get; set; }

        //Cardiovascular
        public bool MH_Murmur { get; set; }
        public bool MH_ChestPain { get; set; }
        public bool MH_Palpitations { get; set; }
        public bool MH_Dizziness { get; set; }
        public bool MH_FaintingSpells { get; set; }
        public bool MH_ShortnessOfBreath { get; set; }
        public bool MH_DifficultyLyingFlat { get; set; }
        public bool MH_SwellingAnkles { get; set; }

        //Allergic / Immunulogic
        public bool MH_Hives { get; set; }
        public bool MH_HayFever { get; set; }

        //Neurological
        public bool MH_LossOfStrength { get; set; }
        public bool MH_Numbness { get; set; }
        public bool MH_Headaches { get; set; }
        public bool MH_Tremors { get; set; }
        public bool MH_MemoryLoss { get; set; }

        //Endocrine
        public bool MH_LossOfHair { get; set; }
        public bool MH_Heat_ColdIntolerance { get; set; }

        //Psychiatric
        public bool MH_Anxiety { get; set; }
        public bool MH_MoodSwings { get; set; }
        public bool MH_DifficultSleeping { get; set; }

        //Females Only
        public Nullable<System.DateTime> MH_LastMammogram_Date { get; set; }
        public bool MH_LastMammogram_Normal { get; set; }
        public Nullable<System.DateTime> MH_LastPap_Date { get; set; }
        public bool MH_LastPap_Normal { get; set; }
        public int MH_Age_Onset_Periods { get; set; }
        public int MH_Age_Onset_Menopause { get; set; }
        public bool MH_Periods_Regular { get; set; }
        public int MH_Pregnancies_Number { get; set; }

        public string MH_ReviewingPhysician { get; set; }

    }
}