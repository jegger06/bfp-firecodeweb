using System;
using System.Collections.Generic;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.Helper;

namespace EBFP.BL.CIS
{
    public class InventoryGroupListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<InventoryGroupModel> InventoryGroupListModel { get; set; }
    }

    public class InventoryGroupSearchModel
    {
        public string IG_Code { get; set; }
        public string IG_Description { get; set; }
        public bool IsSearch { get; set; }
    }

    public class InventoryGroupModel
    {
        public string sIG_Id
        {
            get { return IG_Id.ToNullSafeString().Encrypt(); }

        }

        public int IG_Id { get; set; }
        public string IG_Code { get; set; }
        public string IG_Description { get; set; }
        public int IG_Created_Emp_Id { get; set; }
        public DateTime IG_CreatedDate { get; set; }
        public int? IG_LastUpdate_Emp_Id { get; set; }
        public DateTime? IG_LastUpdateDate { get; set; }
        public string IG_CreatedBy { get; set; }
        public string IG_LastUpdateBy { get; set; }
    }
}
