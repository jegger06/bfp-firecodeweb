
using System.Collections.Generic;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.Helper;

namespace EBFP.BL.InspectionOrder
{
    using System;
    public class InspectionOrderListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<InspectionOrderModel> InspectionOrderList { get; set; }
    }

    public class InspectionOrderSearchModel
    {
        public string IO_Number { get; set; }
        public int IO_Unit_Id { get; set; }
        public int IO_Est_RegistrationStatus { get; set; }
        public int IO_Est_HazardType { get; set; }
        public string IO_Findings { get; set; }
        public int RegionId { get; set; }
        public int ProvinceId { get; set; }
        public int MunicipalityId { get; set; }
        public bool IsSearch { get; set; }
    }

    public class InspectionOrderModel
    {
        public string sIO_Id
        {
            get { return IO_Id.ToNullSafeString().Encrypt(); }

        }
        public string IO_Est_RegistrationStatusName
        {
            get
            {
                return IO_Est_RegistrationStatus > 0 ? ((RegistrationStatus)IO_Est_RegistrationStatus).ToDescription() : null;
            }
        }
        public string IO_Est_HazardTypeName
        {
            get
            {
                return IO_Est_HazardType > 0 ? ((HazardType)IO_Est_HazardType).ToDescription() : null;

            }
        }
        public string IO_Est_EstablishmentStatusName
        {
            get
            {
                return IO_Est_EstablishmentStatus > 0 ? ((EstablishmentStatus)IO_Est_EstablishmentStatus).ToDescription() : null;

            }
        }
        public string IO_Est_OccupancyTypeName
        {
            get
            {
                return IO_Est_OccupancyType > 0 ? ((OccupancyType)IO_Est_OccupancyType).ToDescription() : null;

            }
        }
        public string IO_InspectionTypeName
        {
            get
            {
                return IO_InspectionType > 0 ? ((InspectionType)IO_InspectionType).ToDescription() : null;

            }
        }
        public string IO_ActionTakenName { get; set; }
        public string IO_RemarksName { get; set; }
        public string IO_Unit_Name { get; set; }
        public int IO_Id { get; set; }
        public string Ref_IO_Id { get; set; }
        public string IO_Number { get; set; }
        public DateTime IO_IssueDate { get; set; }
        public int IO_Approval_Emp_Id { get; set; }
        public DateTime? IO_ApprovalDate { get; set; }
        public int IO_InspectionType { get; set; }
        public DateTime IO_InspectionDate { get; set; }
        public string IO_Est_Id { get; set; }
        public int IO_Est_RegistrationStatus { get; set; }
        public int IO_Est_HazardType { get; set; }
        public int IO_Est_EstablishmentStatus { get; set; }
        public int IO_Est_OccupancyType { get; set; }
        public string IO_Findings { get; set; }
        public int IO_Remarks { get; set; }
        public string IO_Remarks_Others { get; set; }
        public int IO_ActionTaken { get; set; }
        public DateTime IO_ActionTakenDate { get; set; }
        public string IO_Est_Details_String { get; set; }
        public int IO_Status { get; set; }
        public int IO_Created_Emp_Id { get; set; }
        public DateTime IO_CreatedDate { get; set; }
        public int? IO_LastUpdate_Emp_Id { get; set; }
        public DateTime? IO_LastUpdateDate { get; set; }
        public int IO_Unit_Id { get; set; }
        public int ProvinceID { get; set; }
        public int RegionId { get; set; }
        public int MunicipalityId { get; set; }
        public bool IsSynced { get; set; }
        public byte[] IO_AIR { get; set; }
        public string IO_AIR_Ext { get; set; }
        public int? IO_ApprovalMarshall_Emp_Id { get; set; }
        public int? IO_ApprovalChiefFSES_Emp_Id { get; set; }
        public int? IO_Approval_AIR_Marshall_Emp_Id { get; set; }
        public int? IO_Approval_AIR_ChiefFSES_Emp_Id { get; set; }
        public List<string> Inspectors { get; set; }
        public DateTime? IO_DashInspectionDate { get; set; }
    }
}
