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
    
    public partial class tblAfterFireOperationDispatchedVehicles
    {
        public int DV_Id { get; set; }
        public int DV_AFO_Id { get; set; }
        public string DV_VehicleName { get; set; }
        public Nullable<System.DateTime> DV_VehicleLeftDate { get; set; }
        public Nullable<System.DateTime> DV_VehicleSceneArrival { get; set; }
        public Nullable<System.DateTime> DV_VehicleStationArrival { get; set; }
        public int DV_WaterFilling { get; set; }
        public string DV_GasConsumed { get; set; }
        public int DV_Created_Emp_Id { get; set; }
        public System.DateTime DV_CreatedDate { get; set; }
        public Nullable<int> DV_LastUpdate_Emp_Id { get; set; }
        public Nullable<System.DateTime> DV_LastUpdateDate { get; set; }
    
        public virtual tblAfterFireOperations tblAfterFireOperations { get; set; }
    }
}
