using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.Helper;

namespace EBFP.BL.CIS
{
    public class InventoryArticleListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<InventoryArticleModel> InventoryArticleListModel { get; set; }
    }

    public class InventoryArticleSearchModel
    {
        public string Art_Code { get; set; }
        public string Art_Name { get; set; }
        public bool IsSearch { get; set; }
    }

    public class InventoryArticleModel
    {
        public string sArt_Id
        {
            get { return Art_Id.ToNullSafeString().Encrypt(); }

        }

        public int Art_Id { get; set; }
        public string Art_Code { get; set; }
        public string Art_Name { get; set; }
        public int Art_Created_Emp_Id { get; set; }
        public DateTime Art_CreatedDate { get; set; }
        public int? Art_LastUpdate_Emp_Id { get; set; }
        public DateTime? Art_LastUpdateDate { get; set; }
        //public string Art_CreatedBy { get; set; }
        //public string Art_LastUpdateBy { get; set; }

        public string Art_CreatedBy
        {
            get
            {
                return CRRank + " " + CRFirstName + " " + CRMiddleName.First() + " " + CRLastname + " " +
                       CRSuffixName;
            }

        }
        public string CRRank { get; set; }
        public string CRFirstName { get; set; }
        public string CRMiddleName { get; set; }
        public string CRLastname { get; set; }
        public string CRSuffixName { get; set; }


        public string Art_LastUpdateBy
        {
            get
            {
                return string.IsNullOrEmpty(MDFirstName) ? "" :  MDRank + " " + MDFirstName + " " + MDMiddleName.First() + " " + MDLastname + " " +
                       MDSuffixName;
            }

        }
        public string MDRank { get; set; }
        public string MDFirstName { get; set; }
        public string MDMiddleName { get; set; }
        public string MDLastname { get; set; }
        public string MDSuffixName { get; set; }
    }
}
