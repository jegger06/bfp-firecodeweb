using System.Collections.Generic;
using EBFP.BL.Helper;

namespace EBFP.BL.HumanResources
{
    public class SeniorityLinealListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<SeniorityLinealModel> SeniorityLinealList { get; set; }
    }

    public class SeniorityLinealModel
    {
        public int EmpId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public int? PresentRank { get; set; }
        public string PresentRankName { get; set; }
        public int QualifiedRankId { get; set; }
        public string QualifiedRank { get; set; }
    }

    public class SLLSearchModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RegionId { get; set; }
        public int ProvinceId { get; set; }
        public int UnitId { get; set; }
        public int PresentRank { get; set; }
        public int QualifiedRankId { get; set; }
        public string QualifiedRank { get; set; }
    }


}