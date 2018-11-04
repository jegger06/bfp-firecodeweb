using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.Helper;

namespace EBFP.BL.CIS
{
    public class PhysicalInventoryListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<PhysicalInventoryModel> PhysicalInventoryListModel { get; set; }
    }

    public class PhysicalInventorySearchModel
    {
        public int PI_Dir_Id { get; set; }
        public int PI_IG_Id { get; set; }
        public int PI_Art_Id { get; set; }
        public string PI_Description { get; set; }
        public string PI_PropertyNumber { get; set; }
        public string PI_UnitOfMeasure { get; set; }
        public DateTime? PI_DateAcquired { get; set; }
        public string PI_Office { get; set; }
        public int PI_Unit_Id { get; set; }
        public string PI_Dir_Name { get; set; }
        public string PI_IG_Name { get; set; }
        public string PI_Art_Name { get; set; }
        public string PI_End_User { get; set; }
        public string PI_Unit_Name { get; set; }
        public bool IsSearch { get; set; }
    }

    public class PhysicalInventoryModel
    {
        public string sPI_Id
        {
            get { return PI_Id.ToNullSafeString().Encrypt(); }

        }

        private string _PI_UnitValue = "";
        public string sPI_UnitValue
        {
            get
            {
                return string.IsNullOrWhiteSpace(_PI_UnitValue) ?
                    PI_UnitValue.ToString() :
                    _PI_UnitValue;
            }
            set
            {
                PI_UnitValue = Functions.ConvertToSafeDecimal(value);
            }
        }

        public string PI_Dir_Name { get; set; }
        public string PI_IG_Name { get; set; }
        public string PI_Art_Name { get; set; }
        public string PI_End_User { get; set; }
        public string PI_Unit_Name { get; set; }

        public int PI_Id { get; set; }
        public int PI_Dir_Id { get; set; }
        public int PI_IG_Id { get; set; }
        public int PI_Art_Id { get; set; }
        public int PI_Unit_Id { get; set; }
        public string PI_Description { get; set; }
        public string PI_PropertyNumber { get; set; }
        public string PI_StockNumber { get; set; }
        public string PI_UnitOfMeasure { get; set; }
        public DateTime? PI_DateAcquired { get; set; }
        public decimal? PI_UnitValue { get; set; }
        public string PI_ARENumber { get; set; }
        public string PI_ICSNUmber { get; set; }
        public int? PI_Emp_Id { get; set; }
        public string PI_Remarks { get; set; }
        public string PI_Office { get; set; }
        public int PI_Created_Emp_Id { get; set; }
        public DateTime PI_CreatedDate { get; set; }
        public int? PI_LastUpdate_Emp_Id { get; set; }
        public DateTime? PI_LastUpdateDate { get; set; }
        public int? PI_UPI_Id { get; set; }
        public bool PI_IsDeleted { get; set; }

    }
}
