namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;
    public class CivilServiceEligibilityBL : Repository<tblEmployeeCivilServiceEligibilities, CivilServiceEligibilityModel>, ICivilServiceEligibility
    {
        public CivilServiceEligibilityBL(EBFPEntities context) : base(context)
        {
        }

        public void InsertBulk(List<CivilServiceEligibilityModel> model, int Emp_Id)
        {
            if (model != null)
            {
                foreach (var item in model)
                    item.ECSE_Emp_Id = Emp_Id;
            }

            InsertBulk(model, a => a.ECSE_Emp_Id == Emp_Id);
        }
    }
}
