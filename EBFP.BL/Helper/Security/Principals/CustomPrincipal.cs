using EBFP.BL.HumanResources;
using System.Linq;
using System.Security.Principal;

namespace EBFP.Helper
{
    // a custom principal with authenticated user information

    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        string[] roles;
        public CustomPrincipal()
        {
        }

        public CustomPrincipal(string email, string[] roles)
        {
            this.Identity = new GenericIdentity(email);
            this.roles = roles;
        }

        public bool IsInRole(string role) { return roles.Contains(role); } 
        public int? UserId { get; set; }
        public int EmployeeUnitId { get; set; }
        public string RankName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public int UnitID { get; set; }
        public int MunicipalityID { get; set; }
        public int ProvinceID { get; set; }
        public int RegionID { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public bool Impersonate { get; set; }
    }
}
