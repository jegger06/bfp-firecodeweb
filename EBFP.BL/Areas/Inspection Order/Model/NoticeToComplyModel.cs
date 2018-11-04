
namespace EBFP.BL.InspectionOrder
{
    using System;
    public class NoticeToComplyModel
    {
        public int NTC_Id { get; set; }
        public DateTime NTC_IssueDate { get; set; }
        public string NTC_Est_Id { get; set; }
        public string NTC_Ref_IO_Id { get; set; }
        public int NTC_Created_Emp_Id { get; set; }
        public DateTime NTC_CreatedDate { get; set; }
        public int? NTC_LastUpdate_Emp_Id { get; set; }
        public DateTime? NTC_LastUpdateDate { get; set; }
        public int NTC_Status { get; set; }
        public string NTC_Result_IO_Id { get; set; }
        public int NTC_Unit_Id { get; set; }
        public bool IsSynced { get; set; }
        public string Ref_NTC_Id { get; set; }
    }
}
