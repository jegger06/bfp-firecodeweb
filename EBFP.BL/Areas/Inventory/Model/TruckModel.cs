using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.Helper;

namespace EBFP.BL.Inventory
{ 
    public class TruckListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<TruckModel> TruckList { get; set; }
    }
    public class TruckSearchModel
    {
        public string Truck_Id_Code { get; set; }
        public int Truck_UnitId { get; set; }
        public int Truck_Type { get; set; }
        public int Truck_Capacity { get; set; }
        public int Truck_Status { get; set; }
        public int Truck_Owner { get; set; }
        public string Truck_PlateNumber { get; set; }
        public int Municipality_Id { get; set; }
    }
    public class TruckModel
    {
        public string sTruck_Id
        {
            get { return Truck_Id.ToNullSafeString().Encrypt(); }
            
        }
      
        public int Truck_Id { get; set; }
        [Required]
        [Range(1, int.MaxValue,
         ErrorMessage = "Station name is required")]
        public int Truck_UnitId { get; set; }
        public int? Truck_SubStationId { get; set; }
        public string Truck_Id_Code { get; set; }
        public int? Truck_Model { get; set; }
        public int? Truck_Owner { get; set; }
        public int? Truck_Status { get; set; }
        public int? Truck_Capacity { get; set; }
        public int? Truck_Type { get; set; }
        public string Truck_PlateNumber { get; set; }
        public string Truck_EngineNumber { get; set; }
        public string Truck_ChassisNumber { get; set; }
        public string Truck_MotorVehicleNumber { get; set; }
        public string Truck_VehicleCodeNumber { get; set; }

        private string _sTruck_AcquisitionCost = "";
        public string sTruck_AcquisitionCost
        {
            get
            {
                return string.IsNullOrWhiteSpace(_sTruck_AcquisitionCost) ?
                    Truck_AcquisitionCost.ToString() :
                    _sTruck_AcquisitionCost;
            }
            set
            {
                Truck_AcquisitionCost = Functions.ConvertToSafeDecimal(value);
            }
        }
        public decimal? Truck_AcquisitionCost { get; set; }
        public int? Truck_ManufactureDate { get; set; }
        public int? Truck_AcquisitionDate { get; set; }
        public int? Truck_ManufactureAge
        {
            get
            {
                return Truck_ManufactureDate != null ? (DateTime.Now.Year - 1) - (Truck_ManufactureDate ?? 0) : 0;
            }
        }
        public int? Truck_AcquisitionAge
        {
            get
            {
                return Truck_AcquisitionDate != null ? (DateTime.Now.Year - 1) - (Truck_AcquisitionDate ?? 0) : 0;
            }
        }
        public string Truck_Remarks { get; set; }
        public string Truck_FrontView { get; set; }
        public string Truck_RearView { get; set; }
        public string Truck_LeftView { get; set; }
        public string Truck_RightView { get; set; }
        public DateTime? Truck_DateCreated { get; set; }
        public int? Truck_CreatedBy { get; set; }
        public DateTime? Truck_ModifiedDate { get; set; }
        public int? Truck_ModifiedBy { get; set; }
        public string Unit_StationName { get; set; }

        public string Truck_OwnerName
        {
            get
            {
                return Truck_Owner != null ? ((Truck_Owner)Truck_Owner).ToDescription() : null;
            }
        }

        public string Truck_TypeName
        {
            get
            {
                return Truck_Type != null ? ((TruckType)Truck_Type).ToDescription() : null;
            }
        }
        public string Truck_StatusName
        {
            get
            {
                return Truck_Status != null ? ((Truck_Status)Truck_Status).ToDescription() : null;
            }
        }

        public int Municipality_Reg_Id { get; set; }
        public int Municipality_Province_Id { get; set; }
        public string Municipality_Name { get; set; }
        public int Municipality_Id { get; set; }

        public HttpPostedFileBase RightView { get; set; }
        public HttpPostedFileBase LeftView { get; set; }
        public HttpPostedFileBase FrontView { get; set; }
        public HttpPostedFileBase RearView { get; set; }
    }

    public class TruckCountChartModel
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int BFPOwnedCount { get; set; }
        public int LGUOwnedCount { get; set; }

        public int SubTotal => BFPOwnedCount + LGUOwnedCount;
    }
    public class TruckAgeCountChartModel
    {
        public int AgeId { get; set; }
        public string Age { get; set; }
        public int BFPOwnedCount { get; set; }
        public int LGUOwnedCount { get; set; }
        public int SubTotal { get; set; }
        public int Share { get; set; }
    }

    public class TruckCountSummaryModel
    {
        public int TotalTruck { get; set; }
        public int TotalServiceable { get; set; }
        public int TotalUnserviceable { get; set; }
        public int TotalUnderRepair { get; set; }
        public int TotalBFPOwned { get; set; }
        public int TotalLGUOwned { get; set; }
    }
}
