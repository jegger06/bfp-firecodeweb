
namespace EBFP.BL.Administration
{
    using Helper;
    using System;
    using System.Collections.Generic;

    //public class ORSeriesModel
    //{
    //    public int OR_Id { get; set; }
    //    public DateTime OR_Issue_Date { get; set; }
    //    public long OR_StartSeries { get; set; }
    //    public long OR_EndSeries { get; set; }
    //    public int OR_Created_Emp_Id { get; set; }
    //    public DateTime OR_CreatedDate { get; set; }
    //    public int? OR_LastUpdate_Emp_Id { get; set; }
    //    public DateTime? OR_LastUpdateDate { get; set; }
    //    public int? OR_LocalDB_OR_Id { get; set; }
    //    public string Ref_OR_Id { get; set; }
    //    public int OR_Unit_Id { get; set; }
    //    public bool IsSynced { get; set; }
    //    public bool? IsDeleted { get; set; }
    //}

    public class ORListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<ORSeriesModel> ORList { get; set; }
    }

    public class ORSearchModel
    {
        public long StartORSeries { get; set; }
        public long EndORSeries { get; set; }
        public bool IsSearch { get; set; }
    }

    public partial class ORSeriesModel
    {
        public int OR_Id { get; set; }
        public string Ref_OR_Id { get; set; }
        public long OR_StartSeries { get; set; }

        public long OR_EndSeries { get; set; }

        public int OR_Created_Emp_Id { get; set; }

        public DateTime OR_CreatedDate { get; set; }

        public int? OR_LastUpdate_Emp_Id { get; set; }

        public DateTime? OR_LastUpdateDate { get; set; }

        public DateTime OR_Issue_Date { get; set; }

        public int? OR_LocalDB_OR_Id { get; set; }

        public int OR_Unit_Id { get; set; }

        public bool IsSynced { get; set; }

        public bool? IsDeleted { get; set; }
    }

    public class MRAAFModel
    {
        public int OR_Id { get; set; }
        public DateTime OR_Issue_Date { get; set; }
        public long OR_StartSeries { get; set; }
        public long OR_EndSeries { get; set; }
        public int OR_Created_Emp_Id { get; set; }
        public DateTime OR_CreatedDate { get; set; }
        public int? OR_LastUpdate_Emp_Id { get; set; }
        public DateTime? OR_LastUpdateDate { get; set; }
        public int? OR_LocalDB_OR_Id { get; set; }
        public string Ref_OR_Id { get; set; }
        public int OR_Unit_Id { get; set; }
        public bool IsSynced { get; set; }
        public bool? IsDeleted { get; set; }

        public int RegionId { get; set; }
        public int ProvinceId { get; set; }
        public int UnitId { get; set; }
    }
}
