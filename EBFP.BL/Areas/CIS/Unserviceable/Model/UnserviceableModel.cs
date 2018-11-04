using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.Helper;

namespace EBFP.BL.CIS
{
    public class UnserviceableListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<UnserviceableModel> UnserviceableListModel { get; set; }
    }

    public class UnserviceableSearchModel
    {
        public string UPI_WMR { get; set; }
        public string UPI_ReportingOffice { get; set; }
        public bool IsSearch { get; set; }
    }

    public class UnserviceableModel
    {
        public UnserviceableModel()
        {
            this.PhysicalInventoryList = new List<PhysicalInventoryModel>();
        }
        public List<PhysicalInventoryModel> PhysicalInventoryList { get; set; }

        public string sUPI_Id
        {
            get { return UPI_Id.ToNullSafeString().Encrypt(); }

        }
        public string UPI_CreatedBy { get; set; }
        public string UPI_LastUpdateBy { get; set; }

        public int UPI_Id { get; set; }
        [Required]
        public string UPI_WMR { get; set; }
        [Required]
        public string UPI_ReportingOffice { get; set; }
        public int UPI_Created_Emp_Id { get; set; }
        public DateTime UPI_CreatedDate { get; set; }
        public int? UPI_LastUpdate_Emp_Id { get; set; }
        public DateTime? UPI_LastUpdateDate { get; set; }

    }
}
