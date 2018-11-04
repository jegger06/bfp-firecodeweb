using System;

namespace EBFP.BL.InspectionOrder
{
    public class OtherViolationModel
    {
        public int OV_Id { get; set; }
        public string Ref_OV_Id { get; set; }
        public int OV_Unit_Id { get; set; }
        public string OV_Code { get; set; }
        public string OV_Desc { get; set; }
        public string OV_Section { get; set; }
        public string OV_Graceperiod { get; set; }
        public Nullable<int> OV_OccupancyType { get; set; }
        public string OV_Compliance { get; set; }
        public Nullable<decimal> OV_Fine { get; set; }
        public Nullable<int> OV_Created_EmpId { get; set; }
        public Nullable<System.DateTime> OV_CreatedDate { get; set; }
        public bool IsSynced { get; set; }
    }
}