using System.Collections.Generic;
using EBFP.BL.Helper;
using EBFP.Helper;

namespace EBFP.BL.Administration
{
    public class UserInRoleListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<vwUsersInRoleModel> OvwUsersInRoleModel { get; set; }
    }

    public class vwUsersInRoleModel
    {
        public string sUIR_ID => UIR_ID.ToString().Encrypt();
        public int Emp_Id { get; set; }
        public string Emp_FirstName { get; set; }
        public string Emp_MiddleName { get; set; }
        public string Emp_LastName { get; set; }
        public int? Emp_DutyStatus { get; set; }
        public string Role_Name { get; set; }
        public int? UIR_ID { get; set; }
        public int? UIR_RoleID { get; set; }
        public string Emp_Number { get; set; }
        public string Rank_Name { get; set; }
        public string Reg_Title { get; set; }
        public string Province_Name { get; set; }
        public string Unit_StationName { get; set; }
    }

    public class UserInRoleSearchModel
    {
        public string EmployeeName { get; set; }
        public string Emp_Number { get; set; }
        public int RoleId { get; set; }
    }

    public class RoleModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class UserInRoleModel
    {
        public string sUIR_ID => UIR_ID.ToString().Encrypt();
        public int UIR_ID { get; set; }
        public int UIR_RoleID { get; set; }
        public int UIR_EmployeeID { get; set; }
        public string UIR_EmployeeName { get; set; }
        public string UIR_RoleName { get; set; }
        public int UIR_AccessCount { get; set; }
    }
}