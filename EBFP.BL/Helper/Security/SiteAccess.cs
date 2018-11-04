using EBFP.BL.Helper;
using System;

namespace EBFP.Helper
{
    public static class SiteAccess
    {

        public static bool CanEdit(string value)
        {
            try
            {
                AccessType access = (AccessType)Enum.Parse(typeof(AccessType), value, true);
                return (access == AccessType.Edit);
            }
            catch
            {

            }

            return false;
        }
        public static bool Viewing(string value)
        {
            try
            {
                AccessType access = (AccessType)Enum.Parse(typeof(AccessType), value, true);
                return (access == AccessType.View);
            }
            catch
            {

            }

            return false;
        }

        public static bool hasAccess(string value)
        {
            try
            {
                AccessType access = (AccessType)Enum.Parse(typeof(AccessType), value, true);
                return (access == AccessType.View || access == AccessType.Edit || access == AccessType.Create);
            }
            catch
            {

            }

            return false;
        }
    }
}
