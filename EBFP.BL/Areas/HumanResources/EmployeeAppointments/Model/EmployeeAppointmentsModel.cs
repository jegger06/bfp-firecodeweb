using System;
using System.Collections.Generic;
using EBFP.BL.Helper;
using EBFP.BL.Inventory;
using AutoMapper;
using EBFP.Helper;

namespace EBFP.BL.HumanResources
{
    public class EmployeeAppointmentsListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<EmployeeAppointmentsModel> EmployeeAppointmentsList { get; set; }
    }

    public class EmployeeAppointmentsSearchModel
    {
        public int EA_Emp_Id { get; set; }
        public string EA_Emp_AccountNumber { get; set; }
        public int EA_Rank_Id { get; set; }
        public int EA_AppoitmentStatus { get; set; }
        public string EA_ItemNumber { get; set; }
        public int EA_AppoitmentNature { get; set; }
        public string EA_AttestingAuthority { get; set; }
        public string EA_PositionVacatedBy { get; set; }
        public string EA_AppointingAuthority { get; set; }
        public DateTime? EA_AppointmentDate_From { get; set; }
        public DateTime? EA_AppointmentDate_To { get; set; }
    }

    public class EmployeeAppointmentDashboardCounter
    {
        public int Today { get; set; }
        public int ThreeDays { get; set; }
        public int OneMonth { get; set; }
    }

    public class EmployeeAppointmentsModel
    {
        public string sEA_Id
        {
            get { return EA_Id.ToNullSafeString().Encrypt(); }
        }
        public int EA_Id { get; set; }
        public int EA_Emp_Id { get; set; }
        public string EA_Emp_Name { get; set; }
        public string EA_Emp_AccountNumber { get; set; }
        public int EA_Rank_Id { get; set; }
        public string EA_Rank_Name { get; set; }
        public int EA_AppoitmentStatus { get; set; }
        public string EA_ItemNumber { get; set; }
        public int EA_AppoitmentNature { get; set; }
        public int? EA_NonRenewalReason { get; set; }
        public string EA_PositionVacatedBy { get; set; }
        public string EA_AppointingAuthority { get; set; }
        public DateTime? EA_AppointmentDate { get; set; }
        public string EA_AttestingAuthority { get; set; }
        public string EA_AttestingAuthorityDesignation { get; set; }
        public DateTime? EA_AttestmentDate { get; set; }
        public int EA_Created_Emp_Id { get; set; }
        public DateTime EA_CreatedDate { get; set; }
        public int? EA_LastUpdate_Emp_Id { get; set; }
        public DateTime? EA_LastUpdateDate { get; set; }

        public int? UnitId { get; set; }
        public int? ProvinceId { get; set; }
        public int? RegionId { get; set; }
    }
}