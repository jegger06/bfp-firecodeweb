using System;
using System.Collections.Generic;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.Helper;

namespace EBFP.BL.CIS
{
    public class InventoryOutSuppliesListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<InventoryOutSuppliesModel> InventoryOutSuppliesModel { get; set; }
    }
    
    public class InventoryOutSuppliesModel
    {
        public string sSIO_Id
        {
            get { return SIO_Id.ToNullSafeString().Encrypt(); }

        }

        public int SIO_Id { get; set; }
        public int SI_Id { get; set; }
        public System.DateTime SIO_OutDate { get; set; }
        public int SIO_QuantityOut { get; set; }
        public Nullable<int> SIO_Emp_Id { get; set; }
        public string SIO_Remarks { get; set; }
        public int SIO_Created_Emp_Id { get; set; }
        public System.DateTime SIO_CreatedDate { get; set; }
        public Nullable<int> SIO_LastUpdate_Emp_Id { get; set; }
        public Nullable<System.DateTime> SIO_LastUpdateDate { get; set; }
        public string SIO_Emp_Name { get; set; }
    }
}
