
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBFP.BL.Administration
{
    using AutoMapper;
    using EBFP.Helper;
    using Helper;
    using System;
    using System.Collections.Generic;

    //public class DepositsModel
    //{
    //    public int Dep_Id { get; set; }

    //    public string Ref_Dep_Id { get; set; }

    //    public int Dep_Unit_Id { get; set; }

    //    [StringLength(50)]
    //    public string Dep_LC_No { get; set; }

    //    public decimal Dep_Amount { get; set; }

    //    [StringLength(150)]
    //    public string Dep_Bank { get; set; }

    //    [Column(TypeName = "date")]
    //    public DateTime Dep_Collection_StartDate { get; set; }

    //    [Column(TypeName = "date")]
    //    public DateTime Dep_Collection_EndDate { get; set; }

    //    [Column(TypeName = "date")]
    //    public DateTime Dep_DepositDate { get; set; }

    //    public int Dep_Depositor_Emp_Id { get; set; }

    //    public string Dep_Depositor_Emp_Name { get; set; }

    //    public int Dep_Created_Emp_Id { get; set; }

    //    public DateTime Dep_CreatedDate { get; set; }

    //    public int? Dep_LastUpdate_Emp_Id { get; set; }

    //    public DateTime? Dep_LastUpdateDate { get; set; }

    //    public int? Dep_LocalDB_Dep_Id { get; set; }

    //    public bool IsSynced { get; set; }
    //}
    public class DepositListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<DepositsModel> DepositList { get; set; }
    }

    public class DepositSearchModel
    {
        public string LCNumber { get; set; }
        public string BankName { get; set; }
        public int Depositor { get; set; }
        public bool IncludeDates { get; set; }
        public DateTime DepositFrom { get; set; }
        public DateTime DepositTo { get; set; }
        public bool IsSearch { get; set; }
    }

    public class DepositsModel
    {
        public string sDep_Id
        {
            get { return Dep_Id.ToNullSafeString().Encrypt(); }

        }

        public int Dep_Id { get; set; }

        public string Ref_Dep_Id { get; set; }

        public int Dep_Unit_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Dep_LC_No { get; set; }

        public decimal Dep_Amount { get; set; }

        [Required]
        [StringLength(150)]
        public string Dep_Bank { get; set; }

        [Column(TypeName = "date")]
        public DateTime Dep_Collection_StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime Dep_Collection_EndDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime Dep_DepositDate { get; set; }

        public int Dep_Depositor_Emp_Id { get; set; }

        public string Dep_Depositor_Emp_Name { get; set; }

        public int Dep_Created_Emp_Id { get; set; }

        public DateTime Dep_CreatedDate { get; set; }

        public int? Dep_LastUpdate_Emp_Id { get; set; }

        public DateTime? Dep_LastUpdateDate { get; set; }

        public int? Dep_LocalDB_Dep_Id { get; set; }

        public bool IsSynced { get; set; }

        public string Formatted_Dep_Collection_StartDate
        {
            get
            {
                return !string.IsNullOrEmpty(Dep_Collection_StartDate.ToString()) ? Dep_Collection_StartDate.ToString("MMM/dd/yyyy") : "";
            }

        }

        public string Formatted_Dep_Collection_EndDate
        {
            get
            {
                return !string.IsNullOrEmpty(Dep_Collection_EndDate.ToString()) ? Dep_Collection_EndDate.ToString("MMM/dd/yyyy") : "";
            }

        }

        public string Formatted_Dep_DepositDate
        {
            get
            {
                return !string.IsNullOrEmpty(Dep_DepositDate.ToString()) ? Dep_DepositDate.ToString("MMM/dd/yyyy") : "";
            }

        }
    }
}
