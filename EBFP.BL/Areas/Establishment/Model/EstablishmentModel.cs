
using System.Collections.Generic;
using System;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.BL.HumanResources;
using EBFP.Helper;

namespace EBFP.BL.Establishment
{

    public class EstablishmentListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<EstablishmentModel> EstablishmentList { get; set; }
    }
    public class EstablishmentSearchModel
    {
        //public string Est_BusinessName { get; set; }
        //public string Est_BusinessTradeName { get; set; }
        //public string Est_MP_Number { get; set; }
        //public string Est_BusinessPermitNumber { get; set; }
        //public string Est_NatureOfBusiness { get; set; }
        //public string Est_OwnerName { get; set; }
        //public int Est_EstablishmentStatus { get; set; }
        //public int RegionId { get; set; }
        //public int ProvinceId { get; set; }
        //public int MunicipalityId { get; set; }
        //public int Est_Unit_Id { get; set; }
        //public int Est_DashboardType { get; set; }
        public string OwnerName { get; set; }
        public string BusinessPermit { get; set; }
        public string MPNumber { get; set; }
        public string TradeName { get; set; }
        public int EstablishmentName { get; set; }
        public int EstablishmentStatus { get; set; }
        public bool IsSearch { get; set; }
    }

    public class EstablishmentModel
    {
        public string sEst_Id
        {
            get { return Est_Id.ToNullSafeString().Encrypt(); }

        }

        public int Est_Id { get; set; }
        public string Ref_Est_Id { get; set; }
        public string Est_MP_Number { get; set; }
        public string Est_BusinessName { get; set; }
        public string Est_BusinessTradeName { get; set; }
        public string Est_BusinessPermitNumber { get; set; }
        public string Est_NatureOfBusiness { get; set; }
        public string Est_BusinessAddress { get; set; }
        public string Est_BuildingConstruction { get; set; }
        public string Est_OwnerName { get; set; }
        public string Est_PhoneFaxNumber { get; set; }
        public string Est_MobileNumber { get; set; }
        public int Est_OccupancyType { get; set; }
        public Nullable<int> Est_OwnershipType { get; set; }
        public string Est_LandArea { get; set; }
        public Nullable<int> Est_NumberOfFloor { get; set; }
        public string Est_FloorArea { get; set; }
        public string Est_TotalFloorArea { get; set; }
        public Nullable<int> Est_OccupantsEmployeeNumber { get; set; }
        public int Est_RegistrationStatus { get; set; }
        public int Est_HazardType { get; set; }
        public int Est_EstablishmentStatus { get; set; }
        public int Est_Unit_Id { get; set; }
        public int Est_Created_Emp_Id { get; set; }
        public System.DateTime Est_CreatedDate { get; set; }
        public Nullable<int> Est_LastUpdate_Emp_Id { get; set; }
        public Nullable<System.DateTime> Est_LastUpdateDate { get; set; }
        public string Est_AuthorizedRepresentative { get; set; }
        public bool IsSynced { get; set; }
        public string Est_BuildingType { get; set; }
        public Nullable<System.DateTime> Est_ExpiryDate { get; set; }
        public Nullable<bool> Est_IsPEZA { get; set; }

        public string Est_EstablishmentStatusName
        {
            get
            {
                EstablishmentStatus Status = (EstablishmentStatus)Est_EstablishmentStatus;
                return Status == 0 ? "" : Status.ToDescription();
            }
        }

        public string Est_EstablishmentHazardName
        {
            get
            {
                HazardType hazard = (HazardType)Est_HazardType;
                return hazard == 0 ? "" : hazard.ToDescription();
            }
        }

        public string Est_RegistrationStatusName
        {
            get
            {
                RegistrationStatus registrationStatus = (RegistrationStatus)Est_RegistrationStatus;
                return registrationStatus == 0 ? "" : registrationStatus.ToDescription();
            }
        }
        //public virtual tblUnits tblUnits { get; set; }
    }

    public class EstablishmentModelx
    {
        public string sEst_Id
        {
            get { return Est_Id.ToNullSafeString().Encrypt(); }

        }

        public int Est_Id { get; set; }

        public string Ref_Est_Id { get; set; }

        public string Est_MP_Number { get; set; }

        public string Est_BusinessName { get; set; }

        public string Est_BusinessTradeName { get; set; }

        public string Est_BusinessPermitNumber { get; set; }

        public string Est_NatureOfBusiness { get; set; }

        public string Est_BusinessAddress { get; set; }

        public string Est_BuildingConstruction { get; set; }

        public string Est_OwnerName { get; set; }

        public string Est_PhoneFaxNumber { get; set; }

        public string Est_MobileNumber { get; set; }

        public int Est_OccupancyType { get; set; }

        public int? Est_OwnershipType { get; set; }

        public string Est_LandArea { get; set; }

        public int? Est_NumberOfFloor { get; set; }

        public string Est_FloorArea { get; set; }

        public string Est_TotalFloorArea { get; set; }

        public int? Est_OccupantsEmployeeNumber { get; set; }

        public int Est_RegistrationStatus { get; set; }

        public int Est_HazardType { get; set; }

        public int Est_EstablishmentStatus { get; set; }

        public int Est_Unit_Id { get; set; }

        public int Est_Created_Emp_Id { get; set; }

        public DateTime Est_CreatedDate { get; set; }

        public int Est_LastUpdate_Emp_Id { get; set; }

        public DateTime Est_LastUpdateDate { get; set; }

        public string Est_AuthorizedRepresentative { get; set; }

        public bool IsSynced { get; set; }

        public string Est_BuildingType { get; set; }

        public bool? Est_IsPEZA { get; set; }

        public DateTime? Est_ExpiryDate { get; set; }

        public string Est_EstablishmentStatusName
        {
            get
            {
                EstablishmentStatus Status = (EstablishmentStatus)Est_EstablishmentStatus;
                return Status == 0 ? "" : Status.ToDescription();
            }
        }

        public string Est_EstablishmentHazardName
        {
            get
            {
                HazardType hazard = (HazardType)Est_HazardType;
                return hazard == 0 ? "" : hazard.ToDescription();
            }
        }

        public string Est_RegistrationStatusName
        {
            get
            {
                RegistrationStatus registrationStatus = (RegistrationStatus)Est_RegistrationStatus;
                return registrationStatus == 0 ? "" : registrationStatus.ToDescription();
            }
        }
        //public string sEst_Id
        //{
        //    get { return Est_Id.ToNullSafeString().Encrypt(); }

        //}
        //public string Est_EstablishmentStatusName
        //{
        //    get
        //    {
        //        return Est_EstablishmentStatus > 0 ?((EstablishmentStatus)Est_EstablishmentStatus).ToDescription() : null;
        //    }
        //}
        //public string Est_OwnershipTypeName
        //{
        //    get
        //    {
        //        return Est_OwnershipType != null ? ((OwnershipType)Est_OwnershipType).ToDescription() : null;
        //    }
        //}
        //public string Est_OccupancyTypeName
        //{
        //    get
        //    {
        //        return Est_OccupancyType > 0 ? ((OccupancyType)Est_OccupancyType).ToDescription() : null;
        //    }
        //}
        //public string Est_HazardTypeName
        //{
        //    get
        //    {
        //        return Est_HazardType > 0 ? ((HazardType)Est_HazardType).ToDescription() : null;
        //    }
        //}
        //public string Est_RegistrationStatusName
        //{
        //    get
        //    {
        //        return Est_RegistrationStatus > 0 ? ((RegistrationStatus)Est_RegistrationStatus).ToDescription() : null;
        //    }
        //}
        //public string Est_Unit_Name { get; set; }
        //public int Est_Id { get; set; }
        //public string Ref_Est_Id { get; set; }
        //public string Est_MP_Number { get; set; }
        //public string Est_BusinessName { get; set; }
        //public string Est_BusinessTradeName { get; set; }
        //public string Est_BusinessPermitNumber { get; set; }
        //public string Est_NatureOfBusiness { get; set; }
        //public string Est_BusinessAddress { get; set; }
        //public string Est_BuildingConstruction { get; set; }
        //public string Est_OwnerName { get; set; }
        //public string Est_PhoneFaxNumber { get; set; }
        //public string Est_MobileNumber { get; set; }
        //public int Est_OccupancyType { get; set; }
        //public int? Est_OwnershipType { get; set; }
        //public string Est_LandArea { get; set; }
        //public int? Est_NumberOfFloor { get; set; }
        //public string Est_FloorArea { get; set; }
        //public string Est_TotalFloorArea { get; set; }
        //public int? Est_OccupantsEmployeeNumber { get; set; }
        //public int Est_RegistrationStatus { get; set; }
        //public int Est_HazardType { get; set; }
        //public int Est_EstablishmentStatus { get; set; }
        //public int Est_Unit_Id { get; set; }
        //public int Est_Created_Emp_Id { get; set; }
        //public DateTime Est_CreatedDate { get; set; }
        //public int? Est_LastUpdate_Emp_Id { get; set; }
        //public DateTime? Est_LastUpdateDate { get; set; }
        //public string Est_AuthorizedRepresentative { get; set; }
        //public int ProvinceID { get; set; }
        //public int RegionID { get; set; }
        //public int MunicipalityId { get; set; }
        ////public int? LocalDB_Est_Id { get; set; }
        //public bool IsSynced { get; set; }
        //public bool fromInventory { get; set; }
        //public string Est_BuildingType { get; set; }
        //public int DashboardType { get; set; }
        //public string Est_LatestIO { get; set; }
        //public string Est_Inspector { get; set; }
        //public DateTime? Est_ExpiryDate { get; set; }
        //public bool? Est_IsPEZA { get; set; }
        //public bool IsPositive()
        //{
        //    List<int> _positive = new List<int> { 1, 2, 3, 4 };
        //    return _positive.Contains(Est_EstablishmentStatus);
        //}
        
    }

    public class EstablishmentDetailModel
    {
        public string RegionName { get; set; }
        public string ProvinceName { get; set; }
        public string MunicipalityName { get; set; }
    }

    public class FCREstablishmentModel
    {
        public int Compliant { get; set; }
        public int NonCompliant { get; set; }
        public int Closure { get; set; }
        public int Total { get; set; }
    }


    //to do
    public class CalendarModel
    {
        public string Est_Id { get; set; }

        public string BusinessName { get; set; }
        public string TradeName { get; set; }
        public string MPNumber { get; set; }
        public string PermitNumber { get; set; }
        public string NatureOfBusiness { get; set; }
        public string Owner { get; set; }
        public int EstablishmentStatus { get; set; }
        public string BusinessAddress { get; set; }
        public DateTime? Issue_Date { get; set; }

        public string ApproveBy { get; set; }
        public string Est_BusinessName { get; set; }
        public string IO_Number { get; set; }
        public string IO_Id { get; set; }
        public string IO_ScheduledStatus { get; set; }
        public DateTime IO_IssueDate { get; set; }
        public DateTime? IO_InspectionDate { get; set; }


        public string StatusName => ((EstablishmentStatus)EstablishmentStatus).ToDescription();
    }
}
