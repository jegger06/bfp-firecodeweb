using System;
using System.Collections.Generic;
using EBFP.BL.Helper;
using EBFP.Helper;

namespace EBFP.BL.HumanResources
{
    public class RankListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<RankModel> RankList { get; set; }
    }

    public class RankSearchModel
    {
        public string Rank_Name { get; set; }
    }

    public class RankModel
    {
        public string sRank_Id
        {
            get { return Rank_Id.ToString().Encrypt(); }
        }

        public int Rank_Id { get; set; }
        public string Rank_Name { get; set; }
        public int Rank_DBM_Authorized { get; set; }
        public int? Rank_CreatedBy { get; set; }
        public DateTime? Rank_CreatedDate { get; set; }
        public int? Rank_UpdatedBy { get; set; }
        public DateTime? Rank_UpdatedDate { get; set; }
    }
}