
namespace EBFP.BL.Administration
{
    using Helper;
    using System;
    using System.Collections.Generic;

    public class SpoiledORListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<SpoiledORModel> SpoiledORList { get; set; }
    }

    public class SpoiledORSearchModel
    {
        public string SpoiledORNumber { get; set; }
        public bool IsSearch { get; set; }
    }

    public class SpoiledORModel
    {
        public int SOR_Id { get; set; }
        public long SOR_Number { get; set; }
        public int SOR_Created_Emp_Id { get; set; }
        public DateTime SOR_CreatedDate { get; set; }
        public bool IsSynced { get; set; }
        public string Ref_SOR_Id { get; set; }
        public int SOR_Unit_Id { get; set; }
    }
}
