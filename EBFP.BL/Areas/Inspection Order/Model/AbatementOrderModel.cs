
namespace EBFP.BL.InspectionOrder
{
    using System;
    public class AbatementOrderModel
    {
        public int AO_Id { get; set; }
        public string Ref_AO_Id { get; set; }
        public DateTime AO_IssueDate { get; set; }
        public string AO_Est_Id { get; set; }
        public string AO_Ref_IO_Id { get; set; }
        public int AO_Created_Emp_Id { get; set; }
        public DateTime AO_CreatedDate { get; set; }
        public int? AO_LastUpdate_Emp_Id { get; set; }
        public DateTime? AO_LastUpdateDate { get; set; }
        public int AO_Status { get; set; }
        public string AO_Result_IO_Id { get; set; }
        public int AO_Unit_Id { get; set; }
        public bool IsSynced { get; set; }
    }
}
