//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EBFP.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblTrucks
    {
        public int Truck_Id { get; set; }
        public int Truck_UnitId { get; set; }
        public Nullable<int> Truck_SubStationId { get; set; }
        public string Truck_Id_Code { get; set; }
        public Nullable<int> Truck_Model { get; set; }
        public Nullable<int> Truck_Owner { get; set; }
        public Nullable<int> Truck_Status { get; set; }
        public Nullable<int> Truck_Capacity { get; set; }
        public Nullable<int> Truck_Type { get; set; }
        public string Truck_PlateNumber { get; set; }
        public string Truck_EngineNumber { get; set; }
        public string Truck_ChassisNumber { get; set; }
        public string Truck_MotorVehicleNumber { get; set; }
        public string Truck_VehicleCodeNumber { get; set; }
        public Nullable<decimal> Truck_AcquisitionCost { get; set; }
        public Nullable<int> Truck_ManufactureDate { get; set; }
        public Nullable<int> Truck_AcquisitionDate { get; set; }
        public string Truck_Remarks { get; set; }
        public string Truck_FrontView { get; set; }
        public string Truck_RearView { get; set; }
        public string Truck_LeftView { get; set; }
        public string Truck_RightView { get; set; }
        public Nullable<System.DateTime> Truck_DateCreated { get; set; }
        public Nullable<int> Truck_CreatedBy { get; set; }
        public Nullable<System.DateTime> Truck_ModifiedDate { get; set; }
        public Nullable<int> Truck_ModifiedBy { get; set; }
    
        public virtual tblUnitSubStation tblUnitSubStation { get; set; }
        public virtual tblUnits tblUnits { get; set; }
    }
}
