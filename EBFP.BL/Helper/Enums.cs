using System.ComponentModel;

namespace EBFP.BL.Helper
{
    /// <summary>

    public enum BuildingType
    {
        [Description("Establishment")]
        Establishment = 0,
        [Description("Residential")]
        Residential = 1
    }

    public enum OccupancyType
    {
        [Description("Assembly")]
        Assembly = 1,
        [Description("Educational")]
        Educational = 2,
        [Description("Health Care")]
        Health_Care = 3,
        [Description("Correction and Detention Center")]
        Correction_and_Detention_Center = 4,
        [Description("Mercantile")]
        Mercantile = 5,
        [Description("Industrial")]
        Industrial = 6,
        [Description("Business")]
        Business = 7,
        [Description("Storage")]
        Storage = 8,
        [Description("Mixed")]
        Mixed = 9,
        [Description("Miscellaneous")]
        Miscellaneous = 10,
        [Description("Residential")]
        Residential = 11
    }

    public enum OwnershipType
    {
        [Description("Owned")]
        Owned = 0,
        [Description("Lessee")]
        Lessee = 1
    }

    public enum RegistrationStatus
    {
        [Description("None")]
        None = 0,
        [Description("New")]
        New = 1,
        [Description("Renewal")]
        Renewal = 2
    }

    public enum HazardType
    {
        [Description("High")]
        High = 1,
        [Description("Moderate/Low")]
        Moderate_Low = 2
    }

    public enum EstablishmentStatus
    {
        [Description("For Issuance of FSEC")]
        For_Issuance_of_FSEC = 1,
        [Description("Issued FSEC")]
        Issued_FSEC = 2,
        [Description("For Issuance of FSIC")]
        For_Issuance_of_FSIC = 3,
        [Description("Issued FSIC")]
        Issued_FSIC = 4,
        [Description("For Issuance of NTC")]
        For_Issuance_of_NTC = 5,
        [Description("Issued NTC")]
        Issued_NTC = 6,
        [Description("For Issuance of NTCV")]
        For_Issuance_of_NTCV = 7,
        [Description("Issued NTCV")]
        Issued_NTCV = 8,
        [Description("For Issuance of Abatement Order")]
        For_Issuance_of_Abatement_Order = 9,
        [Description("Issued Abatement Order")]
        Issued_Abatement_Order = 10,
        [Description("For Issuance of Closure Notice")]
        For_Issuance_of_Closure_Notice = 11,
        [Description("Issued Closure Notice")]
        Issued_Closure_Notice = 12,
        [Description("Closed")]
        Closed = 13
    }

    public enum OTHERPYMT_EstablishmentType
    {
        [Description("Commercial")]
        Commercial = 0,
        [Description("NonCommercial")]
        NonCommercial = 1
    }

    public enum FSEC_EstablishmentType
    {
        [Description("Commercial")]
        Commercial = 0,
        [Description("NonCommercial")]
        NonCommercial = 1
    }

    public enum FSIC_EstablishmentType
    {
        [Description("Occupancy")]
        Occupancy = 0,
        [Description("Business")]
        Business = 1,
        [Description("Permit To Operate")]
        PermitToOperate = 2
    }

    public enum FSEC_Status
    {
        [Description("Evaluation")]
        Evaluation = 0,
        [Description("Evaluated")]
        Evaluated = 1,
        [Description("Assessed")]
        Assessed = 2,
        [Description("Collected")]
        Collected = 3,
        [Description("Plan Evaluator")]
        PlanEvaluator = 4,
        [Description("Chief FSES")]
        ChiefFSES = 5,
        [Description("Marshall")]
        Marshall = 6,
        [Description("Released")]
        Released = 7,
        [Description("None")]
        None = 8,
        [Description("Releasing")]
        Releasing = 9,
    }

    public enum OTHERPYMT_Status
    {
        [Description("Evaluation")]
        Evaluation = 0,
        [Description("Evaluated")]
        Evaluated = 1,
        [Description("Assessed")]
        Assessed = 2,
        [Description("Collected")]
        Collected = 3,
        [Description("Released")]
        Released = 4,
        [Description("None")]
        None = 5,
        [Description("Chief FSES")]
        ChiefFSES = 6,
        [Description("Marshal")]
        Marshal = 7,
        [Description("Releasing")]
        Releasing = 8,
    }

    public enum CollectionPurpose
    {
        [Description("Others")]
        Others = 0,
        [Description("Fire Incident")]
        Fire_Clearance = 1,
        [Description("Hotworks CLR")]
        Hotworks_Clearance = 2,
        [Description("Storage CLR")]
        Storage_Clearance = 3,
        [Description("Conveyance CLR")]
        Conveyance_Clearance = 4,
        [Description("Electrical CLR")]
        Electrical_Clearance = 5,
        //[Description("Installation Fee")]
        //Installation_Fee = 6,
        [Description("LPGAS Instl")]
        LPG_Installation = 7,
        [Description("Fire Drill Fee")]
        Fire_Drill_Fee = 8,
        [Description("Fireworks Display")]
        Fire_Works_Display = 9,
        [Description("Fumigation/Fogging")]
        Fumigation_Fogging = 10,
        //[Description("Others")]
        //Others = 11,
        [Description("Premium Tax")]
        Premium_Tax = 12,
        [Description("Sales Tax")]
        Sales_Tax = 13,
        [Description("Proceeds Tax")]
        Proceeds_Tax = 14,
        [Description("AFSS Instl")]
        AFSS_Installation = 15,
        [Description("Dust Prod. Machine")]
        Dust_Producing_Machine = 16,
        //[Description("Storage for Flammable and Combustible Liquids")]
        //Storage_for_Flammable_and_Combustible_Liquids = 20,
        [Description("Bldg Srv Equip")]
        Installation_Building_Service_Equipment = 18,
        [Description("Kitchen Hood")]
        Kitchen_Hood = 19,
        [Description("Flam/Comb Liq. Sto")]
        Installation_Flammable_Combustible_Liquids_Storage_Tanks = 17,
    }

    public enum PaymentType
    {
        [Description("Cash")]
        Cash = 1,
        [Description("Check")]
        Check = 2,
        [Description("Money Order")]
        Money_Order = 3,
    }

    public enum FSIC_Type
    {
        [Description("Occupancy")]
        Occupancy = 0,
        [Description("Business")]
        Business = 1,
        [Description("Permit to Operate")]
        PermitToOperate = 2,
        [Description("BPLO Payments")]
        BPLOPayments = 3
    }

    public enum FSIC_Status
    {
        [Description("Evaluation")]
        Evaluation = 0,
        [Description("Evaluated")]
        Evaluated = 1,
        [Description("Assessed")]
        Assessed = 2,
        [Description("Collected")]
        Collected = 3,
        [Description("Chief FSES")]
        ChiefFSES = 4,
        [Description("Marshall")]
        Marshall = 5,
        [Description("Released")]
        Released = 6,
        [Description("Releasing")]
        Releasing = 8,
        [Description("None")]
        None = 7,

    }

    public enum AP_ApplicationType
    {
        [Description("FSEC")]
        FSEC = 1,
        [Description("FSIC")]
        FSIC = 2,
        [Description("BPLOPayment")]
        BPLOPayment = 3,
        [Description("OtherFees")]
        OtherFees = 4
    }

    public enum FSIC_StorageClearanceApplication
    {
        [Description("Flammable/Combustible Solids")]
        FlammableCombustibleSolids = 1,
        [Description("Flammable/Combustible Liquids")]
        FlammableCombustibleLiquids = 2,
        [Description("Flammable Gases")]
        FlammableGases = 3
    }
    public enum FSIC_StorageClearanceSolidClass
    {
        [Description("Carbide")]
        Carbide = 1,
        [Description("Pyroxylin")]
        Pyroxylin = 2,
        [Description("Matches")]
        Matches = 3,
        [Description("Of similar flammable, explosive, oxidizing or lacrymatory properties")]
        Similar = 4,
        [Description("Shredded combustible materials")]
        ShreddedCombustibleMaterials = 5,
        [Description("Tar, resin, waxes, copra, rubber, cork, bituminous coal")]
        Tar = 6
    }

    public enum FSIC_StorageClearanceLiquidClass
    {
        [Description("Having flashpoint of -6.67°C or below")]
        HavingFlashpoint = 1,
        [Description("Having flashpoint of above -6.67°C and below 22.8°C")]
        HavingFlashpointOfAbove = 2,
        [Description("Having flashpoint of 22.8°C and below 93.3°C")]
        HavingFlashpointOf228 = 3,
        [Description("Having flashpoint greater than 93.3°C")]
        HavingFlashpointGreaterThan = 4
    }


    public enum FSIC_StorageClearanceGasClass
    {
        [Description("Liquefied Petroleum Gas for Bulk Storage")]
        LiquefiedPetroleumGasForBulk = 1,
        [Description("Liquefied Petroleum Gas for other than Bulk Storage")]
        LiquefiedPetroleumGasforOther = 2,
        [Description("Other flammable gases")]
        OtherFlammableGases = 3
    }

    public enum FSIC_ConveyanceClearanceApplication
    {
        [Description("a. For every conveyance clearance issued on cargo trucks or motor vehicles with a load capacity not exceeding 2,000 liters of inflammable liquid with a flash point of 93.3oC payable annually")]
        A = 1,
        [Description("b. For every conveyance clearance issued on cargo trucks or motor vehicles with a load capacity of not exceeding 500 kilograms of explosives and/or combustible materials, including hazardous chemicals and gases payable annually")]
        B = 2,
        [Description("c. For every conveyance clearance issued on tank trucks, tank trailers, and tank semi-trailers carrying inflammable liquids described in sub-paragraph “a” hereof with 2,000 liters capacity tanks which shall be good for period of one(1) year")]
        C = 3,
        [Description("d. For every conveyance clearance issued to cover the transfer of inflammable liquids described in subparagraph" +
        "a., to shore tanks at terminal, including the discharge of inflammable cargo to bulk lighters undertaken at bay, and its subsequent transportation by water to petroleum wharves, or transfer by bulk lighters from said terminals to vessel at bay:")]
        D = 4,
        [Description("e. Provided, that for discharge of flammable liquids with flash points of not less than 65.5oC, clearance fees shall be as follows:")]
        E = 5,
        [Description("f. If the transfer or conveyance of liquids in bulk done by lighters or through pipelines from refineries, the following fees shall be imposed to wit:")]
        F = 6,
        [Description("g. For flammable liquids having flash point of 65.5 to 93.3oC")]
        G = 7,
        [Description("h. For every clearance issued covering the whole operations of loading and unloading to or from a boat, vessel, craft, or railway tanks cars and the transfer of packages of containers of explosives, flammable liquids or combustible materials, including hazardous chemicals and gases at terminals or piers:")]
        H = 8
    }

    public enum FSIC_ConveyanceMeasurement
    {
        [Description("Kilogram")]
        Kilogram = 1,
        [Description("Liter")]
        Liter = 2
    }

    public enum FSIC_InstallationClearanceApplication
    {
        [Description("A. Gases (LPG, CNG and other compressed gases)")]
        A = 1,
        [Description("B. Flammable and combustible liquids in aboveground & underground tanks")]
        B = 2
    }


    public enum FSIC_AdministrativeFinesDesc
    {
        [Description("A. Cellulose nitrate plastic of any kind")]
        A = 1,
        [Description("B. Combustible fibers")]
        B = 2,
        [Description("C. Cellular materials such as foam rubber, sponge rubber and plastic foam")]
        C = 3,
        [Description("D. Flammable and combustible liquids or gases of any classification")]
        D = 4,
        [Description("E. Flammable paints, varnishes, stains and organic coatings")]
        E = 5,
        [Description("F. High piled or widely spread combustible stock")]
        F = 6,
        [Description("G. Metallic magnesium in any form")]
        G = 7,
        [Description("H. Corrosive liquids, oxidizing materials, organic peroxide, nitromethane, ammonium nitrate or any amount of highly toxic pyrophoric, hypergolic or cryogenic materials or poisonous gases as well as material compounds which when exposed to heat[of] OR flame become a fire conductor or generate excessive smoke or toxic gases")]
        H = 8,
        [Description("I. Blasting agents, explosives and special industrial explosive materials, blasting caps, black powder, liquid nitro-glycerin, dynamite, nitro-cellulose, fulminates of any kind and plastic explosives containing ammonium salt or chlorate")]
        I = 9,
        [Description("J. Liquid Nitroglycerine and liquid Trinitrotoluene")]
        J = 10,
        [Description("K. Firework materials of any kind or form")]
        K = 11,
        [Description("L. Matches in commercial quantities")]
        L = 12,
        [Description("M. Hot ashes, live coals and embers")]
        M = 13,
        [Description("N. Mineral, vegetable or animal oils and other derivative/by-products")]
        N = 14,
        [Description("O. Recycling, reuse and resale of combustible and flammable liquids and other waste materials combustible waste materials for recycling or resale")]
        O = 15,
        [Description("P. Explosives dusts and vapors")]
        P = 16,
        [Description("Q. Agriculture, forest, marine or mineral products which may undergo spontaneous combustion")]
        Q = 17,
    }

    public enum FSIC_OtherDescription
    {
        [Description("A. Fireworks display")]
        A = 300,
        [Description("B. Fumigation / Fogging ")]
        B = 100,
        [Description("C. Fire Drill")]
        C = 200,
        [Description("D. Hotworks")]
        D = 150,
        [Description("E. Fire Incident Clearance")]
        E = 1002,
        [Description("F. Certified true copy of Fire Safety Inspection Certificate")]
        F = 1003,
        [Description("G. Certified true copy of Building Fire Safety Clearance")]
        G = 1004,
        [Description("H. Certified true copy of Fire Clearance")]
        H = 1005,
    }

    public enum FSIC_TaxYear
    {
        [Description("1-1")]
        A = 1,
        [Description("1-2")]
        B = 2,
        [Description("1-3")]
        C = 3,
        [Description("1-4")]
        D = 4
    }

    public enum Checked
    {
        True = 1,
        False = 0
    }

    public enum SqlScriptType
    {
        BACKUP,
        RESTORE
    }


    public enum AdminGrid
    {
        Credential = 3,
        Access = 4
    }

    public enum UnitAdminIds
    {
        UnitAdmin = 1,
        DBAdmin = 2
    }

    public enum ServerAdminIds
    {
        PortalAdmin = 1
    }

    public enum InspectionType
    {
        [Description("Building Under Construction")]
        BuildingUnderConstruction = 1,
        [Description("Application for Occupancy Permit")]
        AppForOccupancyPermit = 2,
        [Description("Application for Business Permit")]
        AppForBusinessPermit = 3,
        [Description("Periodic Inspection of Occupancy")]
        PeriodicInspectionOfOccupancy = 4,
        [Description("Verification Inspection of Compliance to NTCV")]
        VerificationInspectionOfComplianceToNTCV = 5,
        [Description("Verification Inspection of Compliance Received")]
        VerificationInspectionOfComplianceReceived = 6,
        [Description("Others")]
        Others = 7
    }

    public enum Establishment_Reports
    {
        [Description("POSITIVE / NEGATIVE REPORT")]
        POSITIVENEGATIVEREPORT = 0,
    }

    public enum CalendarType
    {
        Expired = 0,
        ThisMonth = 1,
        NextMonth = 2,
        ExpiringThisMonth = 3
    }

    public enum ForComplianceType
    {
        None = 0,
        NTC = 1,
        NTCV = 2,
        AbatementOrder = 3,
        Closure = 4
    }
    public enum IORemarks
    {
        Compliant = 6,
        NotInspected = 15,
        FORCLOSURE = 10,
        FORCOMPLIANCE = 11

    }
    public enum ForComplianceActionType
    {
        NTC = 5,
        IssuedNTC = 6,
        PendingEvaluation = 7,
        AbatementOrder = 8,
        NTCV = 9,
        IssuedWithAbatement = 10,
        IssuedWithNTCV = 11,
        IssuedClosureOrder = 14

    }

    public enum ExpiringCompliance
    {
        Today,
        InThreeDays
    }
    public enum DutyStatuses
    {
        Active = 1,
        Inactive = 2,
        OnLeave = 3,
        OnTraining = 4,
        AWOL = 5,
        Dropped = 6,
        Retired = 7,
        Deceased = 8,
        Suspended = 9
    }

    public enum ViolationOccupancyType
    {
        [Description("All")]
        All = 0,
        [Description("Assembly")]
        Assembly = 1,
        [Description("Educational")]
        Educational = 2,
        [Description("Health Care")]
        Health_Care = 3,
        [Description("Correction and Detention Center")]
        Correction_and_Detention_Center = 4,
        [Description("Mercantile")]
        Mercantile = 5,
        [Description("Industrial")]
        Industrial = 6,
        [Description("Business")]
        Business = 7,
        [Description("Storage")]
        Storage = 8,
        [Description("Mixed")]
        Mixed = 9,
        [Description("Miscellaneous")]
        Miscellaneous = 10,
        [Description("Residential")]
        Residential = 11
    }

    public enum FSIC_BusinessType
    {
        [Description("None")]
        None = 0,
        [Description("New Business")]
        New_Business = 1,
        [Description("Renewal")]
        Renewal = 2,
        [Description("Without Valid FSIC")]
        Without_Valid_FSIC = 3,
        [Description("With Valid FSIC")]
        With_Valid_FSIC = 4,
        [Description("New Business Without Valid FSIC")]
        New_Business_Without_Valid_FSIC = 5,
    }

    public enum TypeofInspection
    {
        [Description("None")]
        None = 0,
        [Description("Regular Inspection")]
        Regular_Inspection = 1,
        [Description("Advanced Inspection")]
        Advanced_Inspection = 2,

    }
    /// </summary>
    public enum EducationLevel
    {
        [Description("ELEMENTARY")]
        ELEMENTARY = 1,
        [Description("SECONDARY")]
        SECONDARY = 2,
        [Description("VOCATIONAL/TRADE COURSE")]
        VOCATIONAL_TRADE_COURSE = 3,
        [Description("COLLEGE")]
        COLLEGE = 4,
        [Description("GRADUATE STUDIES")]
        GRADUATE_STUDIES = 5
    }

    public enum EducAttaintmentLevel
    {
        [Description("DOCTORATE")]
        Doctorate = 1,
        [Description("MASTERAL DEGREE")]
        MasteralDegree = 2,
        [Description("BACHELOR DEGREE")]
        Bachelor_Degree = 3,
        [Description("VOCATIONAL LEVEL")]
        Vocational_Level = 4,
        [Description("HIGH SCHOOL LEVEL")]
        High_School_Level = 5,
        [Description("ELEMENTARY LEVEL")]
        Elementary_Level = 6,
    }

    public enum AppointmentStatuses
    {
        [Description("PERMANENT")]
        Permanent = 1,
        [Description("TEMPORARY")]
        Temporary = 2,
        [Description("RENEWAL")]
        Renewal = 3,
        [Description("UNDER APPEAL")]
        Under_Appeal = 4,
        [Description("RE-APPOINTMENT")]
        Re_Appointment = 5
    }

    //public enum DutyStatuses
    //{
    //    Active = 1,
    //    Inactive = 2,
    //    OnLeave = 3,
    //    OnTraining = 4,
    //    AWOL = 5,
    //    Dropped = 6,
    //    Retired = 7,
    //    Deceased = 8,
    //    Suspended = 9
    //}

    public enum AccessType
    {
        Edit = 0,
        View = 1,
        Create = 2
    }

    public enum DashboardEstablihmentType
    {
        All = 1,
        Compliant = 2,
        NonCompliant = 3,
        Closure = 4

    }

    public enum PhysicalExam
    {
        [Description("General")]
        General = 1,
        [Description("Head")]
        Head = 2,
        [Description("Eyes")]
        Eyes = 3,
        [Description("ENT")]
        ENT = 4,
        [Description("Neck")]
        Neck = 5,
        [Description("Respiratory")]
        Respiratory = 6,
        [Description("Cardiac")]
        Cardiac = 7,
        [Description("Vascular")]
        Vascular = 8,
        [Description("ChestBreast")]
        ChestBreast = 9,
        [Description("GI")]
        GI = 10,
        [Description("GU")]
        GU = 11,
        [Description("Lymph")]
        Lymph = 12,
        [Description("Musculoskeletal")]
        Musculoskeletal = 13,
        [Description("Skin")]
        Skin = 14,
        [Description("Psych")]
        Psych = 15,
        [Description("Neuro")]
        Neuro = 16,

        [Description("Deferred")]
        Deferred = 17,
        [Description("FamilyHistory")]
        FamilyHistory = 18,
        [Description("PatientInformation")]
        PatientInformation = 19,
        [Description("HealthMaintenance")]
        HealthMaintenance = 20,
        [Description("PhysicalExamOtherInfo")]
        PhysicalExamOtherInfo = 21,
        [Description("DiagnosisList")]
        DiagnosisList = 22
    }


    public enum Medical
    {
        [Description("MedicalHistory")]
        MedicalHistory = 1,
        [Description("MedicalEmployeeInfo")]
        MedicalEmployeeInfo = 2,
        [Description("MedicalAdditionalInformation")]
        MedicalAdditionalInformation = 3,
        [Description("CurrentMedication")]
        CurrentMedication = 4,
        [Description("HealthCareProvider")]
        HealthCareProvider = 5,
        [Description("AllergicReaction")]
        AllergicReaction = 6,
        [Description("HealthRecord")]
        HealthRecord = 7,
        [Description("PastSurgicalHistory")]
        PastSurgicalHistory = 8
    }

    public enum LeaveParticular
    {
        Vacation = 1,
        Sick = 2,
        Maternity = 3,
        Paternity = 4,
        Mandatory = 5,
        Calamity = 6,
        Others = 7
    }

    public enum ApplicationType
    {
        FSEC = 1,
        FSIC = 2,
        BPLOPayments = 3,
        OtherFees = 4
    }

    //public enum PaymentType
    //{
    //    ConstructionTax = 1,
    //    RealtyTax = 2,
    //    PremiumTax = 3,
    //    SalesTax = 4,
    //    ProceedsTax = 5,
    //    FireSafetyInspectionFee = 6,
    //    StorageClearanceFee = 7,
    //    ConveyanceClearanceFee = 8,
    //    InstallationClearanceFee = 9,
    //    FireCodeAdminFine = 10,
    //    OtherFee = 11
    //}

    //public enum FSIC_Status
    //{
    //    [Description("Evaluation")]
    //    Evaluation = 0,
    //    [Description("Evaluated")]
    //    Evaluated = 1,
    //    [Description("Assessed")]
    //    Assessed = 2,
    //    [Description("Collected")]
    //    Collected = 3,
    //    [Description("Released")]
    //    Released = 4
    //}

    //public enum FSEC_EstablishmentType
    //{
    //    [Description("Commercial")]
    //    Commercial = 0,
    //    [Description("NonCommercial")]
    //    NonCommercial = 1
    //}

    //public enum FSEC_Status
    //{
    //    [Description("Evaluation")]
    //    Evaluation = 0,
    //    [Description("Evaluated")]
    //    Evaluated = 1,
    //    [Description("Assessed")]
    //    Assessed = 2,
    //    [Description("Collected")]
    //    Collected = 3,
    //    [Description("Plan Evaluator")]
    //    PlanEvaluator = 4,
    //    [Description("Chief FSES")]
    //    ChiefFSES = 5,
    //    [Description("Marshall")]
    //    Marshall = 6,
    //    [Description("Released")]
    //    Released = 7,
    //    [Description("None")]
    //    None = 8,
    //    [Description("Releasing")]
    //    Releasing = 9
    //}

    public enum OtherFees_Status
    {
        [Description("Evaluation")]
        Evaluation = 0,
        [Description("Evaluated")]
        Evaluated = 1,
        [Description("Assessed")]
        Assessed = 2,
        [Description("Collected")]
        Collected = 3,
        [Description("Released")]
        Released = 4
    }

    public enum ComplianceExpiry
    {
        Today = 1,
        InThreeDays = 2
    }

    public enum StationCategory
    {
        Station = 1,
        Office = 2
    }
    public enum TruckType
    {
        [Description("Chemical Truck")]
        ChemicalTruck = 1,
        [Description("Hydrant Spotter")]
        HydrantSpotter = 2,
        [Description("Ladder (Squirt)")]
        Ladder_Squirt = 3,
        [Description("Ladder (With basket)")]
        Ladder_Withbasket = 4,
        [Description("Mini Pumper")]
        MiniPumper = 5,
        [Description("Pumper")]
        Pumper = 6,
        [Description("Tanker")]
        Tanker = 7
    }

    public enum Truck_Status
    {
        [Description("Beyond Economic Repair")]
        BeyondEconomicRepair = 1,
        [Description("Serviceable")]
        ServiceAble = 2,
        [Description("Unserviceable")]
        UnserviceAble = 4,
        [Description("Under Repair")]
        UnderRepair = 3,
        [Description("Serviceable but for replacement")]
        ServiceableButForReplacement = 5
    }

    public enum Truck_Owner
    {
        [Description("BFP")]
        BFP = 1,
        [Description("LGU")]
        LGU = 2
    }
    public enum Truck_Age
    {
        [Description("0-7")]
        Zero_Seven = 1,
        [Description("8-10")]
        Eight_Ten = 2,
        [Description("11-15")]
        Eleven_Fifthteen = 3,
        [Description("16-20")]
        Sixteen_Twenty = 4,
        [Description("21-24")]
        TwentyOne_twentyFour = 5,
        [Description("25")]
        TwentyFive = 6,
        [Description("26-30")]
        TwentySix_Thirty = 7,
        [Description("31-35")]
        ThirtyOne_ThirtyFive = 8,
        [Description("36-40")]
        ThirtySix_Forty = 9,
        [Description("41-45")]
        FortyOne_FortyFive = 10,
        [Description("46-50")]
        FortySix_Fifty = 11,
        [Description("50 Above")]
        FiftyAbove = 12
    }
    public enum VehicleOwner
    {
        [Description("BFP")]
        BFP = 1,
        [Description("LGU")]
        LGU = 2,
        [Description("OTHER AGENCY")]
        OTHERAGENCY = 3,
        [Description("PRIVATE")]
        PRIVATE = 4
    }

    public enum VehicleType
    {
        [Description("AMBULANCE")]
        AMBULANCE = 1,
        [Description("MOTORCYCLE")]
        MOTORCYCLE = 2,
        [Description("RESCUE VEHICLE")]
        RESCUE_VEHICLE = 3,
        [Description("RUBBER BOAT")]
        RUBBER_BOAT = 4,
        [Description("SERVICE VEHICLE")]
        SERVICE_VEHICLE = 5
    }

    public enum VehicleDashboardType
    {

        [Description("FIRE TRUCKS")]
        FIRE_TRUCKS = 0,
        [Description("AMBULANCE")]
        AMBULANCE = 1,
        [Description("RESCUE VANS")]
        RESCUE_VEHICLE = 3,
        [Description("FIRE BOATS")]
        RUBBER_BOAT = 4
    }

    public enum VehicleAge
    {
        [Description("0 24")]
        Zero_TwentyFour = 1,
        [Description("25 and older")]
        TwentyFive_Older = 2,
    }


    public enum Classification
    {
        [Description("City")]
        City = 1,
        [Description("Municipality")]
        Municipality = 2
    }

    public enum IncomeClass
    {
        [Description("1st")]
        First = 1,
        [Description("2nd")]
        Second = 2,
        [Description("3rd")]
        Third = 3,
        [Description("4th")]
        Fourth = 4,
        [Description("5th")]
        Fifth = 5,
        [Description("6th")]
        Sixth = 6
    }

    public enum BuildingStatus
    {
        [Description("Attached To Other Building")]
        Attached = 1,
        [Description("Seperate Building")]
        Seperate = 2
    }

    public enum BuildingOwner
    {
        [Description("BFP")]
        BFP = 1,
        [Description("LGU")]
        LGU = 2,
        [Description("Other Agency")]
        OtherAgency = 3,
        [Description("Private")]
        Private = 4
    }

    public enum LotOwner
    {
        [Description("BFP")]
        BFP = 1,
        [Description("LGU")]
        LGU = 2,
        [Description("Other Agency")]
        OtherAgency = 3,
        [Description("Private")]
        Private = 4
    }

    public enum LotStatus
    {
        [Description("Donated")]
        Donated = 1,
        [Description("MOA")]
        MOA = 2,
        [Description("Resolution")]
        Resolution = 3,
        [Description("U Sufruct")]
        USufruct = 4
    }

    public enum Rank
    {
        NUP = 1,
        FO1 = 2,
        FO2 = 3,
        FO3 = 4,
        SFO1 = 5,
        SFO2 = 6,
        SFO3 = 7,
        SFO4 = 8,
        INSP = 9,
        SINSP = 10,
        CINSP = 11,
        SUPT = 12,
        SSUPT = 13,
        CSUPT = 14,
        DIR = 15
    }

    public enum JobFunction
    {
        GeneralAdmin = 1,
        Prevention = 2,
        Suppression = 3,
        Investigation = 4,
        EMSAndRescue = 5,
    }


    public enum HighestMandatoryTraining
    {
        FBRC = 1,
        FAIIC = 2,
        FPSC = 3,
        OCC = 4,
        OBC = 5,
        OAC = 6,
        OSEC = 7
    }
    public enum LongevityYear
    {
        FiveYears = 5,
        TenYears = 10,
        FifteenYears = 15,
        TwentyYears = 20,
        TwentyFiveYears = 25
    }

    public enum AccessRole
    {
        PortalAdmin = 1,
        RegionalAdmin = 2,
        ProvincialAdmin = 3,
        StationAdmin = 4,
    }

    public enum PageArea
    {
        #region HRIS
        [Description("HRIS")]
        HRIS = 1000,
        [Description("HRIS -> Employee Roster -> Can View All")]
        HRIS_EmployeeRoster_CanViewAll = 1100,
        [Description("HRIS -> Employee Roster -> Restrict to Region")]
        HRIS_EmployeeRoster_RestricttoRegion = 1101,
        [Description("HRIS -> Employee Roster -> Restrict to Province")]
        HRIS_EmployeeRoster_RestricttoProvince = 1102,
        [Description("HRIS -> Employee Roster ->  Restrict to Station")]
        HRIS_EmployeeRoster_RestricttoStation = 1103,
        [Description("HRIS -> Employee Roster -> Add/Edit/Delete")]
        HRIS_EmployeeRoster_Modify = 1104,
        [Description("HRIS -> Employee Roster -> View Employee Details")]
        HRIS_EmployeeRoster_ViewDetails = 1105,
        [Description("HRIS -> Employee Roster -> View Employee Details - > Print PDS")]
        HRIS_EmployeeRoster_ViewDetails_PrintPDS = 1106,
        [Description("HRIS -> Employee Roster -> View Employee Details - > Medical")]
        HRIS_EmployeeRoster_ViewDetails_Medical = 1107,
        [Description("HRIS -> Employee Roster -> View Employee Details - > Medical Modify")]
        HRIS_EmployeeRoster_ViewDetails_Medical_Modify = 1108,
        [Description("HRIS -> Employee Roster -> View Employee Details - > Leave")]
        HRIS_EmployeeRoster_ViewDetails_Leave = 1109,
        [Description("HRIS -> Employee Roster -> View Employee Details - > Leave Modify")]
        HRIS_EmployeeRoster_ViewDetails_Leave_Modify = 1110,
        [Description("HRIS -> Employee Roster -> Modify BFP Information")]
        HRIS_EmployeeRoster_BFPInformation_Modify = 1111,



        //Municipality
        [Description("HRIS -> Municipality -> Can View All")]
        HRIS_Municipality_CanViewAll = 1200,
        [Description("HRIS -> Municipality -> Restrict to Region")]
        HRIS_Municipality_RestricttoRegion = 1201,
        [Description("HRIS -> Municipality -> Restrict to Province")]
        HRIS_Municipality_RestricttoProvince = 1202,
        [Description("HRIS -> Municipality ->  Restrict to Station")]
        HRIS_Municipality_RestricttoStation = 1203,
        [Description("HRIS -> Municipality -> Add/Edit/Delete")]
        HRIS_Municipality_Modify = 1204,
        [Description("HRIS -> Municipality -> View Municipality Details")]
        HRIS_Municipality_ViewDetails = 1205,

        //Station
        [Description("HRIS -> Unit -> Can View All")]
        HRIS_Unit_CanViewAll = 1300,
        [Description("HRIS -> Unit -> Restrict to Region")]
        HRIS_Unit_RestricttoRegion = 1301,
        [Description("HRIS -> Unit -> Restrict to Province")]
        HRIS_Unit_RestricttoProvince = 1302,
        [Description("HRIS -> Unit ->  Restrict to Station")]
        HRIS_Unit_RestricttoStation = 1303,
        [Description("HRIS -> Unit -> Add/Edit/Delete")]
        HRIS_Unit_Modify = 1304,
        [Description("HRIS -> Unit -> View Unit Details")]
        HRIS_Unit_ViewDetails = 1305,

        //Rank
        [Description("HRIS -> Rank -> Can View All")]
        HRIS_Rank_CanViewAll = 1400,
        [Description("HRIS -> Rank -> Add/Edit/Delete")]
        HRIS_Rank_Modify = 1401,
        [Description("HRIS -> Rank -> View Rank Details")]
        HRIS_Rank_ViewDetails = 1402,


        //Seniority Lineal Listing
        [Description("HRIS -> Seniority Lineal -> Can View All")]
        HRIS_SeniorityLineal_CanViewAll = 1500,
        [Description("HRIS -> Seniority Lineal -> Restrict to Region")]
        HRIS_SeniorityLineal_RestricttoRegion = 1501,
        [Description("HRIS -> Seniority Lineal -> Restrict to Province")]
        HRIS_SeniorityLineal_RestricttoProvince = 1502,
        [Description("HRIS -> Seniority Lineal ->  Restrict to Station")]
        HRIS_SeniorityLineal_RestricttoStation = 1503,

        //HRIS Reports
        [Description("HRIS -> Reports -> Can View All")]
        HRIS_Reports_CanViewAll = 1600,

        //HRIS - Directorates
        [Description("HRIS -> Directorates -> Can View All")]
        HRIS_Directorates_CanViewAll = 1700,
        [Description("HRIS -> Directorates -> Add/Edit/Delete")]
        HRIS_Directorates_Modify = 1701,

        //HRIS - Employee Appointment
        [Description("HRIS -> Employee Appointment -> Can View All")]
        HRIS_EmployeeAppointment_CanViewAll = 1800,
        [Description("HRIS -> Employee Appointment -> Add/Edit/Delete")]
        HRIS_EmployeeAppointment_Modify = 1801,
        [Description("HRIS -> Employee Appointment -> View Employee Appointment Details")]
        HRIS_EmployeeAppointment_ViewDetails = 1802,
        [Description("HRIS -> Employee Appointment -> Restrict to Region")]
        HRIS_EmployeeAppointment_RestricttoRegion = 1803,
        [Description("HRIS -> Employee Appointment -> Restrict to Province")]
        HRIS_EmployeeAppointment_RestricttoProvince = 1804,
        [Description("HRIS -> Employee Appointment ->  Restrict to Station")]
        HRIS_EmployeeAppointment_RestricttoStation = 1805,

        //HRIS Dashboard
        [Description("HRIS -> Dashboard -> Can View Dashboard")]
        HRIS_Reports_CanViewDashboard = 1900,
        #endregion

        #region FCRS
        //Fire Code Revenue 
        [Description("FCRS")]
        FCRS = 2000,
        [Description("FCRS -> Dashboard -> Can View Dashboard")]
        FCRS_Reports_CanViewDashboard = 2100,
        [Description("FCRS -> Reports -> Can View All")]
        FCRS_Reports_CanViewAll = 2101,

        //FireCode Revenue
        [Description(" FCRS -> Fire Code Revenue -> Can View All")]
        FCRS_FireCodeRevenue_CanViewAll = 2200,
        [Description("FCRS -> Fire Code Revenue -> Restrict to Region")]
        FCRS_FireCodeRevenue_RestricttoRegion = 2201,
        [Description("FCRS -> Fire Code Revenue -> Restrict to Province")]
        FCRS_FireCodeRevenue_RestricttoProvince = 2202,
        [Description("FCRS -> Fire Code Revenue -> Restrict to Municipality")]
        FCRS_FireCodeRevenue_RestricttoMunicipality = 2203,
        [Description("FCRS -> Fire Code Revenue ->  Restrict to Station")]
        FCRS_FireCodeRevenue_RestricttoStation = 2204,

        #endregion


        #region FSIS
        //Fire Suppresion
        [Description("FSIS")]
        FSIS = 3000,

        //FSIS Fire Incident
        [Description("FSIS -> Fire Incident -> Can View All")]
        FSIS_FireIncident_CanViewAll = 3100,
        [Description("FSIS -> Fire Incident -> Add/Edit/Delete")]
        FSIS_FireIncident_Modify = 3101,

        //FSIS Fire Investigation
        [Description("FSIS -> Fire Investigation -> Can View All")]
        FSIS_FireInvestigation_CanViewAll = 3200,
        [Description("FSIS -> Fire Investigation -> Add/Edit/Delete")]
        FSIS_FireInvestigation_Modify = 3201,


        //FSIS After Fire Operation
        [Description("FSIS -> After Fire Operation -> Can View All")]
        FSIS_AfterFireOperation_CanViewAll = 3300,
        [Description("FSIS -> After Fire Operation -> Add/Edit/Delete")]
        FSIS_AfterFireOperation_Modify = 3301,


        //FSIS After Fire Operation
        [Description("FSIS -> Reports -> Can View All")]
        FSIS_Reports_CanViewAll = 3400,

        //FSIS Progress Report
        [Description("FSIS -> Progress Report -> Can View All")]
        FSIS_ProgressReport_CanViewAll = 3500,
        [Description("FSIS -> Progress Report -> Add/Edit/Delete")]
        FSIS_ProgressReport_Modify = 3501,

        //FSIS Final Investigation Report
        [Description("FSIS -> Final Investigation Report -> Can View All")]
        FSIS_FinalInvestigation_CanViewAll = 3600,
        [Description("FSIS -> Final Investigation Report -> Add/Edit/Delete")]
        FSIS_FinalInvestigation_Modify = 3601,
        #endregion

        #region FPSS 
        //Fire Prevention 
        [Description("FPSS")]
        FPSS = 4000,

        //Establishment
        [Description(" FPSS -> Establishment -> Can View All")]
        FPSS_Establishment_CanViewAll = 4100,
        [Description("FPSS -> Establishment -> Restrict to Region")]
        FPSS_Establishment_RestricttoRegion = 4101,
        [Description("FPSS -> Establishment -> Restrict to Province")]
        FPSS_Establishment_RestricttoProvince = 4102,
        [Description("FPSS -> Establishment -> Restrict to Municipality")]
        FPSS_Establishment_RestricttoMunicipality = 4103,
        [Description("FPSS -> Establishment ->  Restrict to Station")]
        FPSS_Establishment_RestricttoStation = 4104,

        //Inspection order
        [Description(" FPSS -> InspectionOrder -> Can View All")]
        FPSS_InspectionOrder_CanViewAll = 4200,
        [Description("FPSS -> InspectionOrder -> Restrict to Region")]
        FPSS_InspectionOrder_RestricttoRegion = 4201,
        [Description("FPSS -> InspectionOrder -> Restrict to Province")]
        FPSS_InspectionOrder_RestricttoProvince = 4202,
        [Description("FPSS -> InspectionOrder -> Restrict to Municipality")]
        FPSS_InspectionOrder_RestricttoMunicipality = 4203,
        [Description("FPSS -> InspectionOrder ->  Restrict to Station")]
        FPSS_InspectionOrder_RestricttoStation = 4204,

        //FPSS Dashboard
        [Description("FPSS -> Dashboard -> Can View Dashboard")]
        FPSS_Reports_CanViewDashboard = 4300,
        #endregion

        #region Inventory
        //Inventory
        [Description("Inventory")]
        Inventory = 5000,
        [Description("Inventory -> Municipality -> View Municipality Details")]
        Inventory_Municipality_ViewDetails = 5100,
        [Description("Inventory -> Municipality -> Add/Edit/Delete")]
        Inventory_Municipality_Modify = 5101,
        [Description("Inventory -> Municipality -> Restrict to Region")]
        Inventory_Municipality_RestricttoRegion = 5102,
        [Description("Inventory -> Municipality -> Restrict to Province")]
        Inventory_Municipality_RestricttoProvince = 5103,
        [Description("Inventory -> Municipality ->  Restrict to Station")]
        Inventory_Municipality_RestricttoStation = 5104,
        [Description("Inventory -> Municipality -> Can View All")]
        Inventory_Municipality_CanViewAll = 5105,

        //Invetory Station
        [Description("Inventory -> Station -> Can View All")]
        Inventory_Station_CanViewAll = 5200,
        [Description("Inventory -> Station -> View Station Details")]
        Inventory_Station_ViewDetails = 5201,
        [Description("Inventory -> Station -> Add/Edit/Delete")]
        Inventory_Station_Modify = 5202,


        //Invetory Sub Station
        [Description("Inventory -> Sub Station -> Can View All")]
        Inventory_SubStation_CanViewAll = 5300,
        [Description("Inventory -> Sub Station -> Add/Edit/Delete")]
        Inventory_SubStation_Modify = 5301,
        [Description("Inventory -> Sub Station -> View Sub Station Details")]
        Inventory_SubStation_ViewDetails = 5302,


        //Invetory Trucks
        [Description("Inventory -> Truck -> Can View All")]
        Inventory_Truck_CanViewAll = 5400,
        [Description("Inventory -> Truck -> Add/Edit/Delete")]
        Inventory_Truck_Modify = 5401,
        [Description("Inventory -> Truck -> View Truck Details")]
        Inventory_Truck_ViewDetails = 5402,

        //Invetory Trucks
        [Description("Inventory -> Other Vehicle -> Can View All")]
        Inventory_OtherVehicle_CanViewAll = 5500,
        [Description("Inventory -> Other Vehicle -> Add/Edit/Delete")]
        Inventory_OtherVehicle_Modify = 5501,
        [Description("Inventory -> Other Vehicle -> View Other Vehicle Details")]
        Inventory_OtherVehicle_ViewDetails = 5502,
        [Description("Inventory -> Fire Code Revenue -> View Details")]
        Inventory_FireCodeRevenue_CanViewAll = 5503,
        [Description("Inventory -> Personnel -> View Details")]
        Inventory_Personnel_CanViewAll = 5403,

        //CIS Supplies Inventory
        [Description("Inventory -> Supplies -> Can View All")]
        Inventory_Supplies_CanViewAll = 5600,
        [Description("Inventory -> Supplies -> View Supplies Details")]
        Inventory_Supplies_ViewDetails = 5601,
        [Description("Inventory -> Supplies -> Add/Edit/Delete")]
        Inventory_Supplies_Modify = 5602,

        //CIS Physical Inventory
        [Description("Inventory -> Physical Inventory -> Can View All")]
        Inventory_PhysicalInventory_CanViewAll = 5700,
        [Description("Inventory -> Physical Inventory -> View Physical Inventory Details")]
        Inventory_PhysicalInventory_ViewDetails = 5701,
        [Description("Inventory -> Physical Inventory -> Add/Edit/Delete")]
        Inventory_PhysicalInventory_Modify = 5702,

        //CIS Physical Inventory - Unserviceable
        [Description("Inventory -> Unserviceable -> Can View All")]
        Inventory_Unserviceable_CanViewAll = 5800,
        [Description("Inventory -> Unserviceable -> Add/Edit/Delete")]
        Inventory_Unserviceable_Modify = 5801,

        //CIS Physical Inventory - Report
        [Description("Inventory -> Inventory Report -> Can View All")]
        Inventory_InventoryReport_CanViewAll = 5802,

        //CIS Maintenance - Aricles
        [Description("Inventory -> Articles -> Can View All")]
        Inventory_Articles_CanViewAll = 5900,
        [Description("Inventory -> Articles -> Add/Edit/Delete")]
        Inventory_Articles_Modify = 5901,

        //CIS Maintenance - Directorates
        [Description("Inventory -> Directorates -> Can View All")]
        Inventory_Directorates_CanViewAll = 5902,
        [Description("Inventory -> Directorates -> Add/Edit/Delete")]
        Inventory_Directorates_Modify = 5903,

        //CIS Maintenance - Groups
        [Description("Inventory -> Groups -> Can View All")]
        Inventory_Groups_CanViewAll = 5904,
        [Description("Inventory -> Groups -> Add/Edit/Delete")]
        Inventory_Groups_Modify = 5905,

        [Description("Inventory -> Dashboard -> Can View Dashboard")]
        Inventory_CanView_Dashboard = 6000,


        #endregion





        //Admin
        [Description("Administration")]
        Administration = 100,

        [Description("Administration -> User Role")]
        Administration_UserRole = 101,
        [Description("Administration -> User In Role")]
        Administration_UserInRole = 102,

        //[Description("Resolution")]
        //Resolution = 103,
        //[Description("U Sufruct")]
        //USufruct = 104,

        [Description("All Areas -> Restrict To Region")]
        AllAreas_RestrictToRegion = 105,
        [Description("All Areas -> Restrict To Province")]
        AllAreas_RestrictToProvince = 106,
        [Description("All Areas -> Restrict To Station")]
        AllAreas_RestrictToStation = 107,

    }

    public enum Region
    {
        NHQ = 19,
        NTFI = 20
    }

    //public enum EstablishmentStatus
    //{
    //    [Description("For Issuance of FSEC")]
    //    For_Issuance_of_FSEC = 1,
    //    [Description("Issued FSEC")]
    //    Issued_FSEC = 2,
    //    [Description("For Issuance of FSIC")]
    //    For_Issuance_of_FSIC = 3,
    //    [Description("Issued FSIC")]
    //    Issued_FSIC = 4,
    //    [Description("For Issuance of NTC")]
    //    For_Issuance_of_NTC = 5,
    //    [Description("Issued NTC")]
    //    Issued_NTC = 6,
    //    [Description("For Issuance of NTCV")]
    //    For_Issuance_of_NTCV = 7,
    //    [Description("Issued NTCV")]
    //    Issued_NTCV = 8,
    //    [Description("For Issuance of Abatement Order")]
    //    For_Issuance_of_Abatement_Order = 9,
    //    [Description("Issued Abatement Order")]
    //    Issued_Abatement_Order = 10,
    //    [Description("For Issuance of Closure Notice")]
    //    For_Issuance_of_Closure_Notice = 11,
    //    [Description("Issued Closure Notice")]
    //    Issued_Closure_Notice = 12,
    //    [Description("Closed")]
    //    Closed = 13
    //}
    //public enum OwnershipType
    //{
    //    [Description("Owned")]
    //    Owned = 0,
    //    [Description("Lessee")]
    //    Lessee = 1
    //}

    //public enum OccupancyType
    //{
    //    [Description("Assembly")]
    //    Assembly = 1,
    //    [Description("Educational")]
    //    Educational = 2,
    //    [Description("Health Care")]
    //    Health_Care = 3,
    //    [Description("Correction and Detention Center")]
    //    Correction_and_Detention_Center = 4,
    //    [Description("Mercantile")]
    //    Mercantile = 5,
    //    [Description("Industrial")]
    //    Industrial = 6,
    //    [Description("Business")]
    //    Business = 7,
    //    [Description("Storage")]
    //    Storage = 8,
    //    [Description("Mixed")]
    //    Mixed = 9,
    //    [Description("Miscellaneous")]
    //    Miscellaneous = 10,
    //    [Description("Residential")]
    //    Residential = 11

    //}

    //public enum HazardType
    //{
    //    [Description("High")]
    //    High = 1,
    //    [Description("Moderate/Low")]
    //    Moderate_Low = 2
    //}

    //public enum RegistrationStatus
    //{
    //    [Description("New")]
    //    New = 1,
    //    [Description("Renewal")]
    //    Renewal = 2
    //}
    //public enum InspectionType
    //{
    //    [Description("Building Under Construction")]
    //    BuildingUnderConstruction = 1,
    //    [Description("Application for Occupancy Permit")]
    //    AppForOccupancyPermit = 2,
    //    [Description("Application for Business Permit")]
    //    AppForBusinessPermit = 3,
    //    [Description("Periodic Inspection of Occupancy")]
    //    PeriodicInspectionOfOccupancy = 4,
    //    [Description("Verification Inspection of Compliance to NTCV")]
    //    VerificationInspectionOfComplianceToNTCV = 5,
    //    [Description("Verification Inspection of Compliance Received")]
    //    VerificationInspectionOfComplianceReceived = 6,
    //    [Description("Others")]
    //    Others = 7
    //}

    public enum PhysicalInventoryReportType
    {
        [Description("PAR")]
        PAR = 1,
        [Description("ICS")]
        ICS = 2
    }

    public enum AFOClassification
    {
        [Description("Structural")]
        Structural = 1,
        [Description("Electrical")]
        Electrical = 2,
        [Description("Vehicular")]
        Vehicular = 3,
        [Description("Trash/Grass")]
        Trash_Grass = 4,
        [Description("Chemical")]
        Chemical = 5,
        [Description("Rubbish Fire Near Bldg")]
        Rubbish_Fire_Near_Bldg = 6,
        [Description("Rubbish Fire Vacant Lot")]
        Rubbish_Fire_Vacant_Lot = 7,
        [Description("Others")]
        Others = 8
    }

    public enum AFOMotives
    {
        [Description("Arson")]
        Arson = 1,
        [Description("Accidents")]
        Accidents = 2,
        [Description("Under Investigation")]
        UnderInvestigation = 3
    }
    public enum NonRenewalStatus
    {
        [Description("Training Deficiency")]
        TrainingDeficiency = 1,
        [Description("Eligibility")]
        Eligibility = 2,
        [Description("WE/TIG")]
        WET_IG = 3,
        [Description("Education")]
        Education = 4
    }

    public enum AppointmentNature
    {
        [Description("Original")]
        Original = 1,
        [Description("Renewal")]
        Renewal = 2,
        [Description("Re-Appointment")]
        Re_Appointment = 3,
        [Description("Re-Employment")]
        Re_Employment = 4,
        [Description("Promotion")]
        Promotion = 5
    }

    public enum CivilStatus
    {
        Single = 1,
        Widowed = 2,
        Married = 3,
        Separated = 4,
        Annulled = 5,
        Others = 6
    }

    public enum EducStatus
    {
        Elementary = 1,
        Secondary = 2,
        Vocational = 3,
        College = 4,
        Graduate = 5
    }

    public enum CourseType
    {
        Base = 1,
        Masteral = 2
    }

    public enum EligibilityType
    {
        [Description("1ST LEVEL")]
        First = 1,
        [Description("2ND LEVEL")]
        Second = 2,
        [Description("3RD LEVEL")]
        Third = 3
    }

    public enum MunicipalityType
    {
        City = 1,
        Municipality = 2
    }
    public enum OfficerType
    {
        Officer = 1,
        NonOfficer = 2
    }

    public enum Particulars
    {
        FireTrucks = 1,
        Firefighters = 2
    }

    //public enum FSIC_Type
    //{
    //    [Description("Occupancy")]
    //    Occupancy = 0,
    //    [Description("Business")]
    //    Business = 1,
    //    [Description("Permit to Operate")]
    //    PermitToOperate = 2,
    //    [Description("BPLO Payments")]
    //    BPLOPayments = 3
    //}

    public enum EmployeeStatus
    {
        [Description("Validated")]
        Validated = 1,
        [Description("Non-Validated")]
        Non_Validated = 2
    }
    public enum FieldTraining
    {
        [Description("Managerial")]
        Managerial = 1,
        [Description("Supervisory")]
        Supervisory = 2,
        [Description("Administrative")]
        Administrative = 3,
        [Description("Behavioural")]
        Behavioural = 4,
        [Description("Finance")]
        Finance = 5,
        [Description("Fire Fighting ")]
        Fire_Fighting = 6,
        [Description("Hazmat")]
        Hazmat = 7,
        [Description("Incident Management")]
        Incident_Management = 8,
        [Description("Inspection")]
        Inspection = 9,
        [Description("Investigation")]
        Investigation = 10,
        [Description("Law")]
        Law = 11,
        [Description("Logistical")]
        Logistical = 12,
        [Description("Mandatory")]
        Mandatory = 13,
        [Description("Medical")]
        Medical = 14,
        [Description("Rescue")]
        Rescue = 15
    }
    public enum FI_Details_TypeofAlarm
    {
        [Description("1st Alarm")]
        FirstAlarm = 1,
        [Description("2nd Alarm")]
        SecondAlarm = 2,
        [Description("3rd Alarm")]
        ThirdAlarm = 3,
        [Description("4th Alarm")]
        FourthAlarm = 4,
        [Description("5th Alarm")]
        Fifthalarm = 5,
        [Description("Alpha")]
        TaskAlpha = 6,
        [Description("Bravo")]
        TaskBravo = 7,
        [Description("Charlie")]
        TaskCharlie = 8,
        [Description("Delta")]
        TaskDelta = 9,
        [Description("General Alarm")]
        GeneralAlarm = 10,
        [Description("Fire Out Upon Arrival")]
        FireOutUponArrival = 11
    }
    public enum FI_Details_TypeofOccupancy
    {
        [Description("Residential")]
        Residential = 1,
        [Description("Industrial Factory")]
        IndustrialFactory = 2,
        [Description("Commercial / Mercantile")]
        CommercialMercantile = 3,
        [Description("Storage")]
        Storage = 4,
        [Description("Mixed Type Occupancy")]
        MixedTypeOccupancy = 5,
        [Description("Business Type/Government Offices")]
        BusinessTypeGovernmentOffices = 6,
        [Description("Educational")]
        Educational = 7,
        [Description("Health Care")]
        HealthCare = 8,
        [Description("Correction  And Detention Center")]
        CorrectionAndDetentionCenter = 9,
        [Description("Places Of Assembly")]
        PlacesOfAssembly = 10,
        [Description("Miscellaneous")]
        Miscellaneous = 11,
        [Description("Motor Vehicle")]
        MotorVehicle = 12,
        [Description("Ship/Water Vessel")]
        ShipWaterVessel = 13,
        [Description("Aircraft")]
        Aircraft = 14,
        [Description("Locomotive")]
        Locomotive = 15,
        [Description("Grass Fire/Rubbish Fire")]
        GrassFireRubbishFire = 16
    }
    public enum FI_Details_CauseOfFire
    {
        [Description("Electrical Connections")]
        ElectricalConnections = 1,
        [Description("Electrical Appliances ")]
        ElectricalAppliances = 2,
        [Description("Electrical Machineries")]
        ElectricalMachineries = 3,
        [Description("Spontaneous Combustion")]
        SpontaneousCombustion = 4,
        [Description("Open Flame Due To Unattended Cooking / Stove")]
        OpenFlameDueToUnattendedCookingStove = 5,
        [Description("Open Flame Due To Torch Or Sulo")]
        OpenFlameDueToTorchOrSulo = 6,
        [Description("Open Flame Due To Unattended Lighted Candle Or Gasera")]
        OpenFlameDueToUnattendedLightedCandleOfGasera = 7,
        [Description("Open Flame Due To Direct Flame Contact Or Static Electricity")]
        OpenFlameDueToDirectFlameContactOrStaticElectricity = 8,
        [Description("Lpg Explosion Due To Direct Flame Contact Or Static Electricity")]
        LpgExplosionDueToDirectFlameContactOrStaticElectricity = 9,
        [Description("Lighted Cigarette Butt")]
        LightedCigaretteButt = 10,
        [Description("Chemicals")]
        Chemicals = 11,
        [Description("Pyrotechnics")]
        Pyrotechnics = 12,
        [Description("Lighted Matchstick Or Lighter ")]
        LightedMatchstickOrLighter = 13,
        [Description("Incendiary Device / Mechanism Or Ignited Flammable Liquids")]
        IncendiaryDeviceMechanismOrIgnitedFlammableLiquids = 14,
        [Description("Lightning")]
        Lightning = 15,
        [Description("Bomb Explosion")]
        BombExplosion = 16,
        [Description("Under Investigation")]
        UnderInvestigation = 17,
        [Description("Others")]
        Others = 18
    }
    public enum FI_Details_OriginOfFire
    {
        [Description("Bedroom")]
        Bedroom = 1,
        [Description("Living Room")]
        LivingRoom = 2,
        [Description("Dining Room")]
        DiningRoom = 3,
        [Description("Kitchen")]
        Kitchen = 4,
        [Description("Garage")]
        Garage = 5,
        [Description("Front Yard")]
        FrontYard = 6,
        [Description("Backyard")]
        Backyard = 7,
        [Description("Vacant Lot (Private Lot)")]
        VacantLotPrivateLot = 8,
        [Description("Street Road")]
        StreetRoad = 9,
        [Description("Office")]
        Office = 10,
        [Description("Production Area")]
        ProductionArea = 11,
        [Description("Warehouse")]
        Warehouse = 12,
        [Description("Electrical Room ")]
        ElectricalRoom = 13
    }
}
