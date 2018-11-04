using System.Collections.Generic;
using System.Data.Entity;

namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Linq;

    public class UserBL : Repository<tblEmployees, UserModel>, IUser
    {  
        public UserBL(EBFPEntities context) : base(context)
        { 
        } 

        public bool IsMember(int employeeID)
        {

            var user = BFPContext.webpages_Membership
                               .FirstOrDefault(a => a.UserId == employeeID);

            if (user != null) return true;

            return false;
        }

        public void UpdateDecryptedPassword(int employeeID,string password)
        {
            var userdetails = BFPContext.webpages_Membership.FirstOrDefault(a => a.UserId == employeeID);
            if (userdetails != null)
            {
                userdetails.PasswordDecrypted = password;
                BFPContext.SaveChanges();
            }

        }
     
        public void UpdateDecryptedPasswordByUsername(string userName, string password)
        {
            using (var context = new EBFPEntities())
            {
                var employee = context.tblEmployees.FirstOrDefault(a => a.Emp_Username == userName);
                if (employee == null) return;
                {
                    var userdetails = context.webpages_Membership.FirstOrDefault(a => a.UserId == employee.Emp_Id);
                    if (userdetails != null)
                    {
                        userdetails.PasswordDecrypted = password;
                        context.Entry(userdetails).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
            }
        }

        public UserModel GetByID(int employeeID)
        {
            var ret = new UserModel();

            var emp = BFPContext.tblEmployees
                .Where(a => a.Emp_Id == employeeID).Select(a => new
                {
                    a.Emp_Username
                })
                .FirstOrDefault() ;

            var user = BFPContext.webpages_Membership
                               .FirstOrDefault(a => a.UserId == employeeID);

            //if (emp != null)
            //    ret.UserName = emp.Emp_Username;

            if (user != null)
            {
                ret.Password = user.PasswordDecrypted;
            }

            return ret;
        } 
    }
}
