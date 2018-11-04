
namespace EBFP.BL.InspectionOrder
{
    using System;
    public class ClosureOrderModel
    {
        public int CO_Id { get; set; }
        public string Ref_CO_Id { get; set; }
        public DateTime CO_IssueDate { get; set; }
        public string CO_Est_Id { get; set; }
        public string CO_Ref_IO_Id { get; set; }
        public int CO_Created_Emp_Id { get; set; }
        public DateTime CO_CreatedDate { get; set; }
        public int? CO_LastUpdate_Emp_Id { get; set; }
        public DateTime? CO_LastUpdateDate { get; set; }
        public int CO_Status { get; set; }
        public string CO_Result_IO_Id { get; set; }
        public int CO_Unit_Id { get; set; }
        public bool IsSynced { get; set; }
    }
}
