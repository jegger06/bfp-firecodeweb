using System;

namespace EBFP.BL.InspectionOrder
{
    public class InspectionOrderViolationModel
    {
        public int IOViolation_Id { get; set; }
        public string Ref_IOViolation_Id { get; set; }
        public int IOViolation_ViolationId { get; set; }
        public string IOViolation_IO_Id { get; set; }
        public int IOViolation_UnitId { get; set; }
        public bool IsSynced { get; set; }
        public DateTime? IOViolation_CreatedDate { get; set; }
        public int? IOViolation_Created_EmpId { get; set; }
        public string IOViolation_OV_Id { get; set; }
    }
}