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
    
    public partial class tblInspectionOrderViolations
    {
        public int IOViolation_Id { get; set; }
        public string Ref_IOViolation_Id { get; set; }
        public int IOViolation_ViolationId { get; set; }
        public string IOViolation_IO_Id { get; set; }
        public int IOViolation_UnitId { get; set; }
        public bool IsSynced { get; set; }
        public Nullable<System.DateTime> IOViolation_CreatedDate { get; set; }
        public Nullable<int> IOViolation_Created_EmpId { get; set; }
        public string IOViolation_OV_Id { get; set; }
    }
}
