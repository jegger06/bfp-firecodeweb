
namespace EBFP.BL.Administration
{
    using Helper;
    using System;
    using System.Collections.Generic;

    public class SpoiledOPSListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<SpoiledOPSModel> SpoiledOPSList { get; set; }
    }


    public class SpoiledOPSModel
    {
        public int SOPS_Id { get; set; }
        public string SOPS_Number { get; set; }
        public int SOPS_Created_Emp_Id { get; set; }
        public DateTime SOPS_CreatedDate { get; set; }
        public bool IsSynced { get; set; }
        public string Ref_SOPS_Id { get; set; }
        public int SOPS_Unit_Id { get; set; }
    }

    public class SpoiledOPSSearchModel
    {
        public string SpoiledOPSNumber { get; set; }
        public bool IsSearch { get; set; }
    }
}
