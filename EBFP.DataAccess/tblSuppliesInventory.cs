//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EBFP.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblSuppliesInventory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblSuppliesInventory()
        {
            this.tblSuppliesInventoryOut = new HashSet<tblSuppliesInventoryOut>();
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
    
        public virtual tblArticles tblArticles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblSuppliesInventoryOut> tblSuppliesInventoryOut { get; set; }
        public virtual tblUnits tblUnits { get; set; }
    }
}
