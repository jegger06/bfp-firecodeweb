using System;
using System.Collections.Generic;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.BL.HumanResources;
using EBFP.Helper;

namespace EBFP.BL.CIS
{
    public class InventorySuppliesListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<InventorySuppliesModel> InventorySuppliesModel { get; set; }
    }

    public class InventorySuppliesSearchModel
    {
        public string SI_Art_Name { get; set; }
        public string SI_Description { get; set; }
        public bool SI_WithOnHand { get; set; }
        public bool IsSearch { get; set; }
    }

    public class InventorySuppliesModel
    {
        public string sSI_Id
        {
            get { return SI_Id.ToNullSafeString().Encrypt(); }

        }

        public int SI_Id { get; set; }
        public int SI_Art_Id { get; set; }
        public int SI_Unit_Id { get; set; }
        public string SI_Description { get; set; }
        public string SI_StockNumber { get; set; }
        public string SI_UnitOfMeasure { get; set; }
        public Nullable<System.DateTime> SI_DateAcquired { get; set; }
        public Nullable<decimal> SI_UnitValue { get; set; }
        public Nullable<int> SI_Quantity { get; set; }
        public int SI_Created_Emp_Id { get; set; }
        public System.DateTime SI_CreatedDate { get; set; }
        public Nullable<int> SI_LastUpdate_Emp_Id { get; set; }
        public Nullable<System.DateTime> SI_LastUpdateDate { get; set; }
        public bool SI_IsDeleted { get; set; }
        public string SI_Art_Name { get; set; }
        public int SI_OnHand { get; set; }
        public decimal SI_TotalAmount { get; set; }

        public List<InventoryArticleModel> ArticleList { get; set; }
        public List<InventoryOutSuppliesModel> OutSuppliesList { get; set; }
        public List<UnitModel> UnitList { get; set; }
    }
}
