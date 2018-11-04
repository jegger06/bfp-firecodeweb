using System.Security.Cryptography;
using EBFP.BL.Administration;
using EBFP.BL.Helper;
using EBFP.BL.HumanResources;

namespace EBFP.Helper
{
    public class PageSecurity
    {
       
        public static bool HasAccess(PageArea oPageArea)
        {
            IAdministrationUnitOfWork UnitOfWork = new AdministrationUnitOfWork();
            return UnitOfWork.RoleAccess.HasAccess(oPageArea);
        }

        public static byte[] GetImage(int empId)
        {
            IHRISUnitOfWork UnitOfWork = new HRISUnitOfWork();
            return UnitOfWork.Employee.GetImage(empId);
        }
    }
}