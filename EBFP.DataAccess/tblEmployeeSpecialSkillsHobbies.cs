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
    
    public partial class tblEmployeeSpecialSkillsHobbies
    {
        public int ESSH_Id { get; set; }
        public string ESSH_Title { get; set; }
        public int ESSH_Emp_Id { get; set; }
    
        public virtual tblEmployees tblEmployees { get; set; }
    }
}
