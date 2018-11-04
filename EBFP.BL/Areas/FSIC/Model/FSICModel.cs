using System;
using EBFP.BL.Helper;
using EBFP.Utils;

namespace EBFP.BL.Areas.FSIC.Model
{
    public partial class FSICModel
    {
        //    public int Auto_FSIC_App_Id { get; set; }
        //    public string FSIC_App_Id { get; set; }

        public int FSIC_App_Id { get; set; }

        public string Ref_FSIC_App_Id { get; set; }

        public DateTime FSIC_ApplicationDate { get; set; }

        public int? FSIC_ApplicationType { get; set; }

        public int FSIC_TaxYear { get; set; }

        public string FSIC_BillingNumber { get; set; }

        public string FSIC_Est_Id { get; set; }

        public int? FSIC_Requirement1 { get; set; }

        public int? FSIC_Requirement2 { get; set; }

        public int? FSIC_Requirement3 { get; set; }

        public int? FSIC_Requirement4 { get; set; }

        public int? FSIC_Requirement5 { get; set; }

        public int? FSIC_Requirement6 { get; set; }

        public int? FSIC_Requirement7 { get; set; }

        public int? FSIC_Requirement8 { get; set; }

        public int? FSIC_Requirement9 { get; set; }

        public int? FSIC_Requirement10 { get; set; }

        public decimal? FSIC_ConstructionTax { get; set; }

        public decimal? FSIC_RealtyTax { get; set; }

        public decimal? FSIC_PremiumTax { get; set; }

        public decimal? FSIC_SalesTax { get; set; }

        public decimal? FSIC_ProceedsTax { get; set; }

        public decimal? FSIC_FireSafetyInspectionFee { get; set; }

        public decimal? FSIC_StorageClearanceFee { get; set; }

        public decimal? FSIC_ConveyanceClearanceFee { get; set; }

        public decimal? FSIC_InstallationClearanceFee { get; set; }

        public decimal? FSIC_FireCodeAdminFine { get; set; }

        public decimal? FSIC_OtherFee { get; set; }

        public int FSIC_Status { get; set; }

        public string FSIC_Remarks { get; set; }

        public int? FSIC_Evaluator_Emp_Id { get; set; }

        public DateTime? FSIC_Evaluated_Date { get; set; }

        public int? FSIC_Assesor_Emp_Id { get; set; }

        public DateTime? FSIC_Assesed_Date { get; set; }

        public int? FSIC_Collector_Emp_Id { get; set; }

        public DateTime? FSIC_Collected_Date { get; set; }

        public int? FSIC_ForReleasing_Emp_Id { get; set; }

        public DateTime? FSIC_ForReleased_Date { get; set; }

        public int? FSIC_Released_Emp_Id { get; set; }

        public DateTime? FSIC_Released_Date { get; set; }

        public DateTime? FSIC_Issue_Date { get; set; }

        public string FSIC_Issue_No { get; set; }

        public string FSIC_OPS_Number { get; set; }

        public bool IsSynced { get; set; }

        public int FSIC_Unit_Id { get; set; }

        public int? FSIC_Marshall_Emp_Id { get; set; }
        public int? FSIC_ChiefFSES_Emp_Id { get; set; }
        public DateTime? FSIC_ChiefFSES_Date { get; set; }
        public DateTime? FSIC_Marshall_Date { get; set; }
        public int? FSIC_AIR_ChiefFSES_Emp_Id { get; set; }
        public int? FSIC_AIR_Marshall_Emp_Id { get; set; }
        public DateTime? FSIC_AIR_ChiefFSES_Date { get; set; }
        public DateTime? FSIC_AIR_Marshall_Date { get; set; }

        public bool? FSIC_IsApproveChiefFSES { get; set; }
        public bool? FSIC_IsApproveMarshall { get; set; }
        public bool? FSIC_AIR_IsApproveChiefFSES { get; set; }
        public bool? FSIC_AIR_IsApproveMarshall { get; set; }
        public DateTime? FSIC_Approve_Marshall_Date { get; set; }
        public DateTime? FSIC_Approve_ChiefFSES_Date { get; set; }
        public DateTime? FSIC_AIR_Approve_Marshall_Date { get; set; }
        public DateTime? FSIC_AIR_Approve_ChiefFSES_Date { get; set; }
        public string FSIC_Number { get; set; }
        public string FSIC_ClaimStub_Number { get; set; }
        public int? FSIC_BusinessType { get; set; }
    }

    public partial class FSICEstablismentsModel
    {
        public string Est_Id { get; set; }
        public string FSIC_App_Id { get; set; }
        public string BusinessName { get; set; }
        public string BusinessTradeName { get; set; }
        public string NatureOfBusiness { get; set; }
        public string OwnerName { get; set; }
        public int ApplicationType { get; set; }
        public decimal Amount { get; set; }
        public int? FSIC_ChiefFSES_Emp_Id { get; set; }
        public int? FSIC_Marshall_Emp_Id { get; set; }
        public int? FSIC_AIR_ChiefFSES_Emp_Id { get; set; }
        public int? FSIC_AIR_Marshall_Emp_Id { get; set; }
        public int FSIC_status { get; set; }
        public bool? FSIC_IsApproveChiefFSES { get; set; }
        public bool? FSIC_IsApproveMarshall { get; set; }
        public bool? FSIC_AIR_IsApproveChiefFSES { get; set; }
        public bool? FSIC_AIR_IsApproveMarshall { get; set; }
        public DateTime? FSIC_ApplicationDate { get; set; }
        public DateTime? FSIC_Assesed_Date { get; set; }
        public DateTime? FSIC_ChiefFSES_Date { get; set; }
        public DateTime? FSIC_Collected_Date { get; set; }
        public DateTime? FSIC_Evaluated_Date { get; set; }
        public DateTime? FSIC_ForReleased_Date { get; set; }
        public DateTime? FSIC_Released_Date { get; set; }
        public DateTime? FSIC_Marshall_Date { get; set; }
        public DateTime? FSIC_AIR_ChiefFSES_Date { get; set; }
        public DateTime? FSIC_AIR_Marshall_Date { get; set; }
        public DateTime? FSIC_ExpiryDate { get; set; }
        public DateTime? FSIC_LastPaymentDate { get; set; }
        public string FSIC_MP_NUMBER { get; set; }
        public int RegistrationStatus { get; set; }

        public string Formatted_LastPaymentDate
        {
            get
            {
                return FSIC_LastPaymentDate.HasValue ? FSIC_LastPaymentDate.Value.ToString("MMM/dd/yyyy") : "";
            }

        }

        public string Formatted_ActionDate
        {
            get
            {
                return ActionDate.HasValue ? ActionDate.Value.ToString("MMM/dd/yyyy") : "";
            }

        }

        public string Formatted_ExpiryDate
        {
            get
            {
                return FSIC_ExpiryDate.HasValue ? FSIC_ExpiryDate.Value.ToString("MMM/dd/yyyy") : "";
            }

        }
        public DateTime? ActionDate
        {
            get
            {
                if ((FSIC_Status)FSIC_status != FSIC_Status.Released)
                {
                    if (FSIC_IsApproveMarshall == false)
                        return FSIC_Marshall_Date;
                    if (FSIC_IsApproveChiefFSES == false)
                        return FSIC_ChiefFSES_Date;
                    if (FSIC_AIR_IsApproveMarshall == false)
                        return FSIC_AIR_Marshall_Date;
                    if (FSIC_AIR_IsApproveChiefFSES == false)
                        return FSIC_ChiefFSES_Date;
                }

                switch ((FSIC_Status)FSIC_status)
                {
                    case FSIC_Status.Evaluated:
                        return FSIC_Evaluated_Date;
                    case FSIC_Status.Assessed:
                        return FSIC_Assesed_Date;
                    case FSIC_Status.Collected:
                        return FSIC_Collected_Date;
                    case FSIC_Status.ChiefFSES:
                        return FSIC_ChiefFSES_Date;
                    case FSIC_Status.Marshall:
                        return FSIC_Marshall_Date;
                    case FSIC_Status.Released:
                        return FSIC_Released_Date;
                    default:
                        return FSIC_ApplicationDate;
                }
            }
        }
        public string ApplicationTypeName
        {
            get
            {
                var appType = (FSIC_Type)ApplicationType;
                return appType.ToDesc();
            }
        }
        public string IONumber { get; set; }
        public string IO_Id { get; set; }
        public int IORemarks { get; set; }
        public int? FSIC_BusinessType { get; set; }

        public int? IO_ApprovalMarshall_Emp_Id { get; set; }
        public int? IO_ApprovalChiefFSES_Emp_Id { get; set; }
        public int? IO_Approval_AIR_Marshall_Emp_Id { get; set; }
        public int? IO_Approval_AIR_ChiefFSES_Emp_Id { get; set; }


        public string Status
        {
            get
            {
                if (IORemarks == 10)
                    return "FOR CLOSURE";
                if (FSIC_status != (int)FSIC_Status.Released && FSIC_BusinessType != (int)Helper.FSIC_BusinessType.With_Valid_FSIC
                    && (FSIC_status == (int)FSIC_Status.Collected && FSIC_IsApproveChiefFSES != true))
                    return "FOR FSES IO APPROVAL";

                else if (FSIC_status != (int)FSIC_Status.Released && FSIC_BusinessType != (int)Helper.FSIC_BusinessType.With_Valid_FSIC
                    && (FSIC_status == (int)FSIC_Status.Marshall && FSIC_IsApproveChiefFSES == true && FSIC_IsApproveChiefFSES == true
                    && FSIC_AIR_IsApproveChiefFSES == true && FSIC_AIR_IsApproveMarshall == true && (IORemarks == 0) && IO_ApprovalChiefFSES_Emp_Id == null))
                    return "FOR FSES IO APPROVAL (RE-INSPECTION)";

                else if (FSIC_status != (int)FSIC_Status.Released && FSIC_BusinessType != (int)Helper.FSIC_BusinessType.With_Valid_FSIC
                    && (FSIC_status == (int)FSIC_Status.Marshall && FSIC_IsApproveChiefFSES == true && FSIC_IsApproveMarshall == true && FSIC_AIR_IsApproveMarshall != true && FSIC_AIR_IsApproveChiefFSES != true && IORemarks > 0))
                    return "FOR FSES AIR APPROVAL";

                else if (FSIC_status != (int)FSIC_Status.Released && FSIC_BusinessType != (int)Helper.FSIC_BusinessType.With_Valid_FSIC
                  && (FSIC_status == (int)FSIC_Status.Marshall && FSIC_IsApproveChiefFSES == true && FSIC_IsApproveChiefFSES == true
                  && FSIC_AIR_IsApproveChiefFSES == true && FSIC_AIR_IsApproveMarshall == true && (IORemarks > 0) && IO_ApprovalChiefFSES_Emp_Id != null && IO_ApprovalMarshall_Emp_Id != null
                  && IO_Approval_AIR_ChiefFSES_Emp_Id == null))
                    return "FOR FSES AIR APPROVAL (RE-INSPECTION)";

                else if (FSIC_status != (int)FSIC_Status.Released && FSIC_BusinessType != (int)Helper.FSIC_BusinessType.With_Valid_FSIC
                    && (FSIC_IsApproveChiefFSES == true && FSIC_IsApproveMarshall != true && FSIC_status == (int)FSIC_Status.ChiefFSES))
                    return "FOR MARSHALL IO APPROVAL";

                else if (FSIC_status != (int)FSIC_Status.Released && FSIC_BusinessType != (int)Helper.FSIC_BusinessType.With_Valid_FSIC
                   && (FSIC_status == (int)FSIC_Status.ChiefFSES && FSIC_IsApproveChiefFSES == true && FSIC_IsApproveChiefFSES == true
                    && FSIC_AIR_IsApproveChiefFSES == true && FSIC_AIR_IsApproveMarshall == true && (IORemarks == 0) && IO_ApprovalChiefFSES_Emp_Id != null && IO_ApprovalMarshall_Emp_Id == null))
                    return "FOR MARSHALL IO APPROVAL (RE-INSPECTION)";

                else if (FSIC_status != (int)FSIC_Status.Released && FSIC_BusinessType != (int)Helper.FSIC_BusinessType.With_Valid_FSIC
                   && (FSIC_IsApproveChiefFSES == true && FSIC_IsApproveMarshall == true && FSIC_AIR_IsApproveChiefFSES == true
                     && FSIC_AIR_IsApproveMarshall != true && FSIC_status == (int)FSIC_Status.ChiefFSES))
                    return "FOR MARSHALL AIR APPROVAL";

                else if (FSIC_status != (int)FSIC_Status.Released && FSIC_BusinessType != (int)Helper.FSIC_BusinessType.With_Valid_FSIC
                   && (FSIC_status == (int)FSIC_Status.ChiefFSES && FSIC_IsApproveChiefFSES == true && FSIC_IsApproveChiefFSES == true
                   && FSIC_AIR_IsApproveChiefFSES == true && FSIC_AIR_IsApproveMarshall == true && (IORemarks > 0) && IO_ApprovalChiefFSES_Emp_Id != null && IO_ApprovalMarshall_Emp_Id != null
                   && IO_Approval_AIR_ChiefFSES_Emp_Id != null && IO_Approval_AIR_Marshall_Emp_Id == null))
                    return "FOR MARSHALL AIR APPROVAL (RE-INSPECTION)";
                //else if (FSIC_status != (int)FSIC_Status.Released && FSIC_BusinessType != (int)Helper.FSIC_BusinessType.With_Valid_FSIC
                //&& (FSIC_AIR_IsApproveMarshall == true && IORemarks != (int)Helper.IORemarks.Compliant && FSIC_ForReleased_Date != null && FSIC_status == (int)FSIC_Status.Marshall))
                //    return "FOR RE-INSPECTION";

                else if (FSIC_status != (int)FSIC_Status.Released && FSIC_BusinessType != (int)Helper.FSIC_BusinessType.With_Valid_FSIC
                              && (FSIC_IsApproveChiefFSES == true && FSIC_IsApproveMarshall == true && FSIC_AIR_IsApproveChiefFSES != true && IORemarks == 0 && FSIC_status == (int)FSIC_Status.Marshall))
                    return "FOR INSPECTION";

                else if (FSIC_status != (int)FSIC_Status.Released && FSIC_BusinessType != (int)Helper.FSIC_BusinessType.With_Valid_FSIC
                   && (FSIC_status == (int)FSIC_Status.Marshall && FSIC_IsApproveChiefFSES == true && FSIC_IsApproveChiefFSES == true
                    && FSIC_AIR_IsApproveChiefFSES == true && FSIC_AIR_IsApproveMarshall == true && (IORemarks == 0) && IO_ApprovalChiefFSES_Emp_Id != null && IO_ApprovalMarshall_Emp_Id != null && IO_Approval_AIR_ChiefFSES_Emp_Id == null))
                    return "FOR INSPECTION (RE-INSPECTION)";

                else if (FSIC_status != (int)FSIC_Status.Released && FSIC_BusinessType != (int)Helper.FSIC_BusinessType.With_Valid_FSIC
                                && (FSIC_AIR_IsApproveMarshall == true && (IORemarks > 0 && IORemarks != (int)Helper.IORemarks.Compliant) && FSIC_ForReleased_Date != null && FSIC_status == (int)FSIC_Status.Marshall))
                    return "FOR RE-INSPECTION";


                return "";

            }
        }

        public string isApproved
        {
            get
            {
                if (Status == "FOR FSES IO APPROVAL" || Status == "FOR MARSHALL IO APPROVAL" || Status == "FOR FSES IO APPROVAL (RE-INSPECTION)" || Status == "FOR MARSHALL IO APPROVAL (RE-INSPECTION)")
                    return "FOR IO APPROVAL";
                else if (Status == "FOR FSES AIR APPROVAL" || Status == "FOR MARSHALL AIR APPROVAL" || Status == "FOR FSES AIR APPROVAL (RE-INSPECTION)" || Status == "FOR MARSHALL AIR APPROVAL (RE-INSPECTION)")
                    return "FOR AIR APPROVAL";
                else if (Status == "FOR RE-INSPECTION")
                    return "FOR RE-INSPECTION";
                else if (Status == "FOR INSPECTION" || Status == "FOR INSPECTION (RE-INSPECTION)")
                    return "FOR INSPECTION";
                else if (Status == "FOR CLOSURE")
                    return "FOR CLOSURE";
                return "FOR RELEASING";
            }
        }
    }
    public partial class FSICReportModel
    {
        public string FSICNumber { get; set; }
        public string Region { get; set; }
        public string Province { get; set; }
        public string StationName { get; set; }
        public string PhoneNumber { get; set; }
        public string Est_MP_Number { get; set; }
        public string BusinessName { get; set; }
        public string OwnerName { get; set; }
        public string BusinessAddress { get; set; }
        public string FireMarshall { get; set; }
        public int ORNumber { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public string FSIC_Number { get; set; }
        public string ChiefFSES { get; set; }
        public int? FSIC_BusinessType { get; set; }
        public int? FSIC_ApplicationType { get; set; }
        public byte[] FSIC_ChiefFSESSignature { get; set; }
        public byte[] FSIC_FireMarshallSignature { get; set; }
        public DateTime? Est_ExpiryDate { get; set; }
        public int OccupancyType { get; set; }
        public int InspectionType { get; set; }

        public string Formatted_ExpiryDate
        {
            get
            {
                return Est_ExpiryDate.HasValue ? Est_ExpiryDate.Value.ToString("MMMM dd, yyyy") : "";
            }

        }
    }
    public partial class OPSFSICModel
    {
        public decimal? FSIC_ConstructionTax { get; set; }

        public decimal? FSIC_RealtyTax { get; set; }

        public decimal? FSIC_PremiumTax { get; set; }

        public decimal? FSIC_SalesTax { get; set; }

        public decimal? FSIC_ProceedsTax { get; set; }

        public decimal? FSIC_FireSafetyInspectionFee { get; set; }

        public decimal? FSIC_StorageClearanceFee { get; set; }

        public decimal? FSIC_ConveyanceClearanceFee { get; set; }

        public decimal? FSIC_InstallationClearanceFee { get; set; }

        public decimal? FSIC_FireCodeAdminFine { get; set; }

        public decimal? FSIC_OtherFee { get; set; }

        public int? FSIC_Assesor_Emp_Id { get; set; }

        public DateTime? FSIC_Assesed_Date { get; set; }

        public string FSIC_OPS_Number { get; set; }
        public string Est_BusinessName { get; set; }
        public string Assessor { get; set; }

        public string Est_BusinessAddress { get; set; }
        public string Est_OwnerName { get; set; }
        public string Unit_StationName { get; set; }
        public string Unit_Address { get; set; }
        public string Province_Name { get; set; }
        public string Reg_Title { get; set; }
        public int? FSIC_BusinessType { get; set; }
        public string Est_MP_Number { get; set; }
    }
}
