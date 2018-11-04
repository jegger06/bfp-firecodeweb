using System.Collections.Generic;

namespace EBFP.Helper
{
    public static class SiteRoles
    {
        public static readonly string PortalAdmin = "PortalAdmin";
        public static readonly string HRISAdmin = "HRISAdmin";
        public static readonly string HRISMember = "HRISMember";
        public static readonly string CISAdmin = "CISAdmin";
        public static readonly string CISMember = "CISMember";
        public static readonly string FCRSAdmin = "FCRSAdmin";
        public static readonly string FCRSMember = "FCRSMember";
        public static readonly string FPSSAdmin = "FPSSAdmin";
        public static readonly string FPSSMember = "FPSSMember";
        public static readonly string FSISAdmin = "FSISAdmin";
        public static readonly string FSISMember = "FSISMember";
        public static List<string> Roles
        {
            get
            {
                var roles = new List<string>();
                roles.Add(PortalAdmin);
                roles.Add(HRISAdmin);
                roles.Add(HRISMember);
                roles.Add(CISAdmin);
                roles.Add(CISMember);
                roles.Add(FCRSAdmin);
                roles.Add(FCRSMember);
                roles.Add(FPSSAdmin);
                roles.Add(FPSSMember);
                roles.Add(FSISAdmin);
                roles.Add(FSISMember);
                return roles;
            }
        } 
    }
}
