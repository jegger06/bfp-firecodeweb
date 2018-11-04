
namespace EBFP.BL.FSEC
{
    using Helper;
    using System;

    public partial class FSECModel
    {
        public int Auto_FSEC_App_Id { get; set; }

        public string FSEC_App_Id { get; set; }

        public string Ref_FSEC_App_Id { get; set; }

        public DateTime FSEC_ApplicationDate { get; set; }

        public int FSEC_EstablishmentType { get; set; }

        public string FSEC_Est_Id { get; set; }

        public bool? FSEC_BuildingPlans { get; set; }

        public bool? FSEC_Specification { get; set; }

        public bool? FSEC_BOM { get; set; }

        public bool? FSEC_LocationClearance { get; set; }

        public bool? FSEC_ElectricalPermit { get; set; }

        public bool? FSEC_ElectricalPlans { get; set; }

        public bool? FSEC_ElectricalBOM { get; set; }

        public bool? FSEC_TotalConnectedLoad { get; set; }

        public bool? FSEC_MechanicalPermit { get; set; }

        public bool? FSEC_MechanicalPlans { get; set; }

        public bool? FSEC_MechanicalBOM { get; set; }

        public int FSEC_Status { get; set; }
         
        public string FSEC_Remarks { get; set; }

        public decimal? FSEC_ConstructionTax { get; set; }

        public decimal? FSEC_OtherFee { get; set; }

        public int? FSEC_Evaluator_Emp_Id { get; set; }

        public int? FSEC_Assesor_Emp_Id { get; set; }

        public int? FSEC_Collector_Emp_Id { get; set; }

        public int? FSEC_ForReleasing_Emp_Id { get; set; }

        public int? FSEC_Released_Emp_Id { get; set; }

        public DateTime? FSEC_Evaluated_Date { get; set; }

        public DateTime? FSEC_Assesed_Date { get; set; }

        public DateTime? FSEC_Collected_Date { get; set; }

        public DateTime? FSEC_ForReleased_Date { get; set; }

        public DateTime? FSEC_Released_Date { get; set; }

        public string FSEC_OPS_Number { get; set; }

        public bool IsSynced { get; set; }

        public int FSEC_Unit_Id { get; set; }

        public DateTime? FSEC_PlanEvaluated_Date { get; set; }
        public int? FSEC_PlanEvaluator_Emp_Id { get; set; }
        public string FSEC_PlanEvaluation_Remarks { get; set; }
        public DateTime? FSEC_ChiefFSES_Date { get; set; }
        public int? FSEC_ChiefFSES_Emp_Id { get; set; }
        public string FSEC_ChiefFSES_Remarks { get; set; }
        public DateTime? FSEC_Mashall_Date { get; set; }
        public int? FSEC_Marshall_Emp_Id { get; set; }
        public string FSEC_Marshall_Remarks { get; set; }
        public bool? FSEC_IsApprovePlanEvaluated { get; set; }
        public bool? FSEC_IsApproveChiefFSES { get; set; }
        public bool? FSEC_IsApproveMarshall { get; set; }
        public string FSEC_Number { get; set; }
        public string FSEC_Disapproval_Number { get; set; }
        public string FSEC_ClaimStub_Number { get; set; }
        public bool? FSEC_IsManual { get; set; }

        public bool? FSEC_BPS1 { get; set; }
        public bool? FSEC_BPS2 { get; set; }
        public bool? FSEC_BPS3 { get; set; }
        public bool? FSEC_BPS4 { get; set; }
        public bool? FSEC_BPS5 { get; set; }
        public bool? FSEC_BPS6 { get; set; }
        public bool? FSEC_BPS7 { get; set; }
        public bool? FSEC_BPS8 { get; set; }

        public decimal? FSEC_ValueOfBuilding { get; set; }
    }
   
    public partial class FSECEstablismentsModel
    {
        public string Est_Id { get; set; }
        public string FSEC_App_Id { get; set; }
        public string OwnerName { get; set; }
        public string Address { get; set; }
        public string AuthorizedRepresentative { get; set; }
        public string BusinessName { get; set; }
        public int EstablishmentType { get; set; }

        public bool? FSEC_IsApprovePlanEvaluated { get; set; }
        public bool? FSEC_IsApproveChiefFSES { get; set; }
        public bool? FSEC_IsApproveMarshall { get; set; }
        public int FSEC_status { get; set; }        
        public DateTime? FSEC_ApplicationDate { get; set; }
        public DateTime? FSEC_Assesed_Date { get; set; }
        public DateTime? FSEC_ChiefFSES_Date { get; set; }
        public DateTime? FSEC_Collected_Date { get; set; }
        public DateTime? FSEC_Evaluated_Date { get; set; }
        public DateTime? FSEC_ForReleased_Date { get; set; }
        public DateTime? FSEC_Released_Date { get; set; }
        public DateTime? FSEC_Marshall_Date { get; set; }
        public DateTime? FSEC_PlanEvaluated_Date { get; set; }
        public string FSEC_IsManual { get; set; }
        public string FSEC_MP_NUMBER { get; set; }

        public string Formatted_ActionDate
        {
            get
            {
                return ActionDate.HasValue ? ActionDate.Value.ToString("MMM/dd/yyyy") : "";
            }

        }
        public DateTime? ActionDate
        {
            get
            {
                if((FSEC_Status)FSEC_status != FSEC_Status.Released)
                {
                    if (FSEC_IsApproveMarshall ==  false)
                        return FSEC_Marshall_Date;
                    if (FSEC_IsApproveChiefFSES == false)
                        return FSEC_ChiefFSES_Date;
                    if (FSEC_IsApprovePlanEvaluated == false)
                        return FSEC_PlanEvaluated_Date;
                }

                switch ((FSEC_Status)FSEC_status)
                {
                    case FSEC_Status.Evaluated:
                        return FSEC_Evaluated_Date;
                    case FSEC_Status.Assessed:
                        return FSEC_Assesed_Date;
                    case FSEC_Status.Collected:
                        return FSEC_Collected_Date;
                    case FSEC_Status.PlanEvaluator:
                        return FSEC_PlanEvaluated_Date;
                    case FSEC_Status.ChiefFSES:
                        return FSEC_ChiefFSES_Date;
                    case FSEC_Status.Marshall:
                        return FSEC_Marshall_Date;
                    case FSEC_Status.Released:
                        return FSEC_Released_Date;
                    default:
                        return FSEC_ApplicationDate;
                }
            }
        }
        public string isApproved
        {
            get
            {
                if (FSEC_IsApprovePlanEvaluated == false || FSEC_IsApproveChiefFSES == false || FSEC_IsApproveMarshall == false)
                    return "DISAPPROVED";
                if ((FSEC_Status)FSEC_status == FSEC_Status.PlanEvaluator || 
                    (FSEC_Status)FSEC_status == FSEC_Status.ChiefFSES || (FSEC_Status)FSEC_status == FSEC_Status.Marshall || 
                    (FSEC_Status)FSEC_status == FSEC_Status.Released)
                    return "APPROVED";

                return "";
            }
        }
        public string EstablishmentTypeName
        {
            get
            {
                var EstType = (FSEC_EstablishmentType)EstablishmentType;
                return EstType.ToDescription();
            }
        }
    }

    public partial class FSECOccupancyModel
    {
        public string Est_OwnerName { get; set; }
        public int Est_Unit_Id { get; set; }
        public string Est_BusinessAddress { get; set; }
        public string Est_MobileNumber { get; set; }
        public string Est_NatureOfBusiness { get; set; }
        public string Est_AuthorizedRepresentative { get; set; }
        public string FSEC_Number { get; set; }
        public string OR_Number { get; set; }
        public bool FSEC_IsManual { get; set; }
    }
}
