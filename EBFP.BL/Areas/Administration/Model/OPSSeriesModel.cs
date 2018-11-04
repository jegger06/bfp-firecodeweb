
namespace EBFP.BL.Administration
{
    using System;
    public class OPSSeriesModel
    {
        public int OPS_Id { get; set; }
        public string Ref_OPS_Id { get; set; }
        public DateTime OPS_Issue_Date { get; set; }
        public long OPS_StartSeries { get; set; }
        public long OPS_EndSeries { get; set; }
        public int OPS_Created_Emp_Id { get; set; }
        public DateTime OPS_CreatedDate { get; set; }
        public int? OPS_LastUpdate_Emp_Id { get; set; }
        public DateTime? OPS_LastUpdateDate { get; set; }
        public int OPS_Unit_Id { get; set; }
        public bool IsSynced { get; set; }
    }
}
