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
    
    public partial class tblSuppliesInventoryOut
    {
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
    
        public virtual tblSuppliesInventory tblSuppliesInventory { get; set; }
    }
}