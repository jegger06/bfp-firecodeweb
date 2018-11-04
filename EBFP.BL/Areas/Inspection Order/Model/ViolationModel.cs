using System;

namespace EBFP.BL.InspectionOrder
{
    public class ViolationModel
    {
        public int Auto_Violation_Id { get; set; }
        public string Violation_Code { get; set; }
        public string Violation_Desc { get; set; }
        public string Violation_Section { get; set; }
        public string Violation_Graceperiod { get; set; }
        public int? Violation_OccupancyType { get; set; }
        public string Violation_Compliance { get; set; }
        public decimal? Violation_Fine { get; set; }
        public int? Violation_Created_EmpId { get; set; }
        public DateTime? Violation_CreatedDate { get; set; }
        public decimal? Violation_Fine2 { get; set; }
        public bool? IsSynced { get; set; }
    }
}