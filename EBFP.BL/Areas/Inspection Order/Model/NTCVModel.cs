
namespace EBFP.BL.InspectionOrder
{
    using System;
    public class NTCVModel
    {
        public int NTCV_Id { get; set; }
        public DateTime NTCV_IssueDate { get; set; }
        public string NTCV_Est_Id { get; set; }
        public string NTCV_Ref_IO_Id { get; set; }
        public int NTCV_Created_Emp_Id { get; set; }
        public DateTime NTCV_CreatedDate { get; set; }
        public int? NTCV_LastUpdate_Emp_Id { get; set; }
        public DateTime? NTCV_LastUpdateDate { get; set; }
        public int NTCV_Status { get; set; }
        public string NTCV_Result_IO_Id { get; set; }
        public int NTCV_Unit_Id { get; set; }
        public bool IsSynced { get; set; }
        public string Ref_NTCV_Id { get; set; }
    }
}
