using System.Drawing;
using Newtonsoft.Json;
using System.Threading;
using System.Web.Security;
using AutoMapper;

namespace EBFP.Helper
{
    // wrapper which gets stateful user data from the current thread
    // ** small facade pattern

    public static class CurrentUser
    {
        private static CustomPrincipal oCustomPrincipal
        {
            get
            {
                if (Thread.CurrentPrincipal.Identity is RolePrincipal || Thread.CurrentPrincipal.Identity is FormsIdentity)
                {
                    var principal = Thread.CurrentPrincipal.Identity as FormsIdentity;
                    var user = JsonConvert.DeserializeObject<CustomPrincipal>(principal.Ticket.UserData);
                    return user;
                }

                return (Thread.CurrentPrincipal is CustomPrincipal ? (Thread.CurrentPrincipal as CustomPrincipal) : new CustomPrincipal());
            }
        }

        public static int EmployeeUnitId { get { return oCustomPrincipal?.EmployeeUnitId ?? 0; } }
        public static int EmployeeId{ get{ return oCustomPrincipal?.UserId?? 0;} }
        public static string FirstName { get { return oCustomPrincipal?.FirstName; } }
        public static string Username { get { return oCustomPrincipal?.Username; } }
        public static string LastName { get { return oCustomPrincipal?.LastName; } }
        public static string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }
        public static int RoleID { get { return oCustomPrincipal?.RoleID ?? 0; } }
        public static string RoleName { get {  return oCustomPrincipal.RoleName == null ? "" : oCustomPrincipal.RoleName.ToNullSafeString().ToUpper(); } }
        public static int RegionID { get { return oCustomPrincipal?.RegionID ?? 0; } } 
        public static int ProvinceID { get { return oCustomPrincipal?.ProvinceID ?? 0; } } 
        public static int MunicipalityID { get { return oCustomPrincipal?.MunicipalityID ?? 0; } }
        public static string RankName { get { return oCustomPrincipal?.RankName; } }
        public static bool Impersonate { get { return oCustomPrincipal?.Impersonate ?? false; } }
    }
}
