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
    
    public partial class tblUserInRole
    {
        public int UIR_ID { get; set; }
        public int UIR_RoleID { get; set; }
        public int UIR_EmployeeID { get; set; }
    
        public virtual tblUserRoles tblUserRoles { get; set; }
        public virtual tblEmployees tblEmployees { get; set; }
    }
}
