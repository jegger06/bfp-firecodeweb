using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.Helper;

namespace EBFP.BL.Inventory
{ 
    public class OtherVehicleListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<OtherVehicleModel> OtherVehicleList { get; set; }
    }
    public class OtherVehicleSearchModel
    {
        public string Vehicle_Id_Code { get; set; }
        public int Vehicle_UnitId { get; set; }
        public int Vehicle_Type { get; set; }
        public int Vehicle_Status { get; set; }
        public int Vehicle_Owner { get; set; }
        public string Vehicle_PlateNumber { get; set; }
        public int Municipality_Id { get; set; }
    }
    public class OtherVehicleModel
    {
        public string sVehicle_Id
        {
            get { return Vehicle_Id.ToNullSafeString().Encrypt(); }
        }
        public int Vehicle_Id { get; set; }

        [Required]
        [Range(1, int.MaxValue,
         ErrorMessage = "Station name is required")]
        public int Vehicle_UnitId { get; set; }
        public int? Vehicle_SubStationId { get; set; }
        public string Vehicle_Id_Code { get; set; }
        public int? Vehicle_Model { get; set; }
        public int? Vehicle_Owner { get; set; }
        public int? Vehicle_Status { get; set; }
        public int? Vehicle_Type { get; set; }
        public string Vehicle_PlateNumber { get; set; }
        public string Vehicle_EngineNumber { get; set; }
        public string Vehicle_ChasisNumber { get; set; }
        private string _sVehicle_AcquisitionCost = "";
        public string sVehicle_AcquisitionCost
        {
            get
            {
                return string.IsNullOrWhiteSpace(_sVehicle_AcquisitionCost) ?
                    Vehicle_AcquisitionCost.ToString() :
                    _sVehicle_AcquisitionCost;
            }
            set
            {
                Vehicle_AcquisitionCost = Functions.ConvertToSafeDecimal(value);
            }
        }
        public decimal? Vehicle_AcquisitionCost { get; set; }
        public int? Vehicle_ManufactureDate { get; set; }
        public int? Vehicle_AcquisitionDate { get; set; }
        public string Vehicle_Remarks { get; set; }
        public string Vehicle_FrontView { get; set; }
        public string Vehicle_RearView { get; set; }
        public string Vehicle_LeftView { get; set; }
        public string Vehicle_RightView { get; set; }
        public int? Vehicle_CreatedBy { get; set; }
        public DateTime? Vehicle_CreatedDate { get; set; }
        public int? Vehicle_ModifiedBy { get; set; }
        public DateTime? Vehicle_ModifiedDate { get; set; }
        public string Unit_StationName { get; set; }

        public string Vehicle_OwnerName
        {
            get
            {
                return Vehicle_Owner != null ? ((VehicleOwner)Vehicle_Owner).ToDescription() : null;
            }
        }

        public string Vehicle_TypeName
        {
            get
            {
                return Vehicle_Type != null ? ((VehicleType)Vehicle_Type).ToDescription() : null;
            }
        }
        public string Vehicle_StatusName
        {
            get
            {
                return Vehicle_Status != null ? ((Truck_Status)Vehicle_Status).ToDescription() : null;
            }
        }

        public int? Vehicle_AcquisitionAge
        {
            get
            {
                return Vehicle_AcquisitionDate != null ? (DateTime.Now.Year - 1) - (Vehicle_AcquisitionDate ?? 0) : 0;
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

    public class VehicleCountChartModel
    {
        public int Type { get; set; }
        public string TypeName { get; set; }
        public int BFPOwnedCount { get; set; }
        public int LGUOwnedCount { get; set; }
    }

    public class VehicleCountSummaryModel
    {
        public int TotalVehicle { get; set; }
        public int TotalAmbulances { get; set; }
        public int TotalRescueVehicle { get; set; }
        public int TotalFireBoats { get; set; }
        public int TotalFireTrucks { get; set; }
    }
}
