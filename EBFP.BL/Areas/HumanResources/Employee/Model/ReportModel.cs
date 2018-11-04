using System;
using EBFP.BL.Helper;
using EBFP.DataAccess;
using System.Collections.Generic;

namespace EBFP.BL.HumanResources
{
    public class FireCodeFeesCollectionModel
    {
        public string Region { get; set; }
        public int ApplicationType { get; set; }
        public decimal? ConstructionTax { get; set; }
        public decimal? RealtyTax { get; set; }
        public decimal? PremiumTax { get; set; }
        public decimal? SalesTax { get; set; }
        public decimal? ProceedsTax { get; set; }
        public decimal? ForOccupancyFee { get; set; }
        public decimal? ForBusinessOtherFee { get; set; }
        public decimal? StorageClearanceFee { get; set; }
        public decimal? ConveyanceClearanceFee { get; set; }
        public decimal? InstallationClearanceFee { get; set; }
        public decimal? FireCodeAdminFine { get; set; }
        public decimal? FireSafetyInspectionFee { get; set; }
        public decimal? OtherFee { get; set; }
        public decimal? Total { get; set; }
        public string Month { get; set; }

        public int RegionId { get; set; }
        public int ProvinceId { get; set; }
        public int UnitId { get; set; }

        public int MunicipalityID { get; set; }
        public int Emp_Curr_Unit { get; set; }
    }

    public class FireCodeDepositCollectionModel
    {
        public string NameOfNCO { get; set; }
        public string Agency { get; set; }
        public string Address { get; set; }
        public DateTime CollectionDate { get; set; }
        public int ORNumber { get; set; }
        public decimal? CollectionAmount { get; set; }
        public DateTime DepositDate { get; set; }
        public string LCNumber { get; set; }
        public decimal? DepositAmount { get; set; }
        public decimal? Undeposited { get; set; }
        public int RegionId { get; set; }
        public int ProvinceId { get; set; }
        public int UnitId { get; set; }
    }

    public class PersonnelModel
    {
        public int RegId { get; set; }
        public string Region { get; set; }
        public int ProvinceId { get; set; }
        public string Province { get; set; }
        public int StationId { get; set; }
        public string Station { get; set; }
        public int SFO4 { get; set; }
        public int SFO3 { get; set; }
        public int SFO2 { get; set; }
        public int SFO1 { get; set; }
        public int FO3 { get; set; }
        public int FO2 { get; set; }
        public int FO1 { get; set; }
        public int DIR { get; set; }
        public int NUP { get; set; }
        public int CSUPT { get; set; }
        public int SSUPT { get; set; }
        public int SUPT { get; set; }
        public int CINSP { get; set; }
        public int SINSP { get; set; }
        public int INSP { get; set; }
        public int GeneralAdmin { get; set; }
        public int Operations { get; set; }

        //public int NUPGeneralAdmin { get; set; }
        //public int NUPOperations { get; set; }
    }

    public class ActualVsAuthorizedModel
    {
        public int EmpId { get; set; }
        public int RankId { get; set; }
        public string Rank { get; set; }
        public int DBMAuthorized { get; set; }
        public int ActualStrength { get; set; }
        public int Variance { get; set; }
        public tblEmployees tblemloyees { get; set; }
        public int RegionId { get; set; }
        public int ProvinceId { get; set; }
        public int? Emp_Curr_Unit { get; set; }

    }

    public class LeaveRecordModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Earned { get; set; }
        public decimal VacLeaveWithPay { get; set; }
        public decimal VacLeaveWithOutPay { get; set; }
        public decimal SickLeaveWithPay { get; set; }
        public decimal SickLeaveWithOutPay { get; set; }
        public int VL { get; set; }
        public int SL { get; set; }
        public int PL { get; set; }
        public int ML { get; set; }
        public int MD { get; set; }
        public int CL { get; set; }
        public int OL { get; set; }
        public int LeaveType { get; set; }
        public string Particular { get; set; }
    }

    public class LeaveParticulars
    {
        public int LeaveType { get; set; }
        public decimal TotalDays { get; set; }
    }
    public class LongevityPayModel
    {
        public string FullName { get; set; }
        public string UnitName { get; set; }
        public string Batch { get; set; }
        public string Rank { get; set; }
        public DateTime EffectiveDate { get; set; }
    }

    public class CommutationLeaveCreditsModel
    {
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string SuffixName { get; set; }
        public string UnitName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DateRetired { get; set; }
        public string Rank { get; set; }
        public double Earned { get; set; }
        public decimal EnjoyedLeave { get; set; }
        public decimal EnjoyedSickLeave { get; set; }
    }

    public class AgeProfileModel
    {
        public int RankId { get; set; }
        public string RankName { get; set; }
        public int TwentyOneTwentyFive { get; set; }
        public int TwentySixThirty { get; set; }
        public int ThirtyOneThirtyFive { get; set; }
        public int ThirtySixFourty { get; set; }
        public int FourtyOneFourtyFive { get; set; }
        public int FourtySixFifty { get; set; }
        public int FiftyOneFiftyFive { get; set; }
        public DateTime? BirthDate { get; set; }
        public int Age
        {
            get
            {
                if (BirthDate.HasValue)
                    return Functions.GetAge(BirthDate);

                return 0;
            }
        }
    }

    public class RatioOfFemaleFireFightersModel
    {
        public int RegId { get; set; }
        public string Region { get; set; }
        public int SFO4_Male { get; set; }
        public int SFO4_Female { get; set; }

        public int SFO3_Male { get; set; }
        public int SFO3_Female { get; set; }


        public int SFO2_Male { get; set; }
        public int SFO2_Female { get; set; }

        public int SFO1_Male { get; set; }
        public int SFO1_Female { get; set; }

        public int FO3_Male { get; set; }
        public int FO3_Female { get; set; }

        public int FO2_Male { get; set; }
        public int FO2_Female { get; set; }

        public int FO1_Male { get; set; }
        public int FO1_Female { get; set; }

        public int DIR_Male { get; set; }
        public int DIR_Female { get; set; }

        public int NUP_Male { get; set; }
        public int NUP_Female { get; set; }

        public int CSUPT_Male { get; set; }
        public int CSUPT_Female { get; set; }

        public int SSUPT_Male { get; set; }
        public int SSUPT_Female { get; set; }

        public int SUPT_Male { get; set; }
        public int SUPT_Female { get; set; }

        public int CINSP_Male { get; set; }
        public int CINSP_Female { get; set; }

        public int SINSP_Male { get; set; }
        public int SINSP_Female { get; set; }

        public int INSP_Male { get; set; }
        public int INSP_Female { get; set; }
    }


    public class RetiringPersonnelModel
    {
        public string AccountNumber { get; set; }
        public string Rank { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string SuffixName { get; set; }
        public DateTime RetirementDate { get; set; }
    }

    public class SpoiledORModel
    {
        public long SpoiledQty { get; set; }
        public long SpoiledStart { get; set; }
        public long SpoiledEnd { get; set; }
    }

    public class IssuedOrModel
    {
        public long IssuedQty { get; set; }
        public long IssuedStart { get; set; }
        public long IssuedEnd { get; set; }
    }

    public class MRAAFModel
    {
        public MRAAFModel()
        {
            SpoiledORList = new List<SpoiledORModel>();
            IssuedOrList = new List<IssuedOrModel>();
        }
        public string ProvinceName { get; set; }
        public string CityMunicipalityName { get; set; }
        public string UnitStationName { get; set; }

        public string Place { get; set; }
        public long BeginningQty { get; set; }
        public long BeginningStart { get; set; }
        public long BeginningEnd { get; set; }
        public DateTime IssueDate { get; set; }
        public string Source { get; set; }
        //public long IssuedQty { get; set; }
        //public long IssuedStart { get; set; }
        //public long IssuedEnd { get; set; }

        //public long SpoiledQty { get; set; }
        //public long SpoiledStart { get; set; }
        //public long SpoiledEnd { get; set; }

        public long EndingQty { get; set; }
        public long EndingStart { get; set; }
        public long EndingEnd { get; set; }

        public int RegionId { get; set; }
        public int ProvinceId { get; set; }
        public int UnitId { get; set; }
        public int MunicipalityId { get; set; }
        public int MunicipalityProvinceId { get; set; }

        public string RegionName { get; set; }
        public long PaymentOrNumber { get; set; }
        public long SpoiledOrNumber { get; set; }
        public List<SpoiledORModel> SpoiledORList { get; set; }
        public List<IssuedOrModel> IssuedOrList { get; set; }
    }
}