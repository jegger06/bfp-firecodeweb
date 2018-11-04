namespace EBFP.BL.HumanResources
{
    using DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public class VoluntaryWorkBL : Repository<tblEmployeeVoluntaryWorks, VoluntaryWorkModel>, IVoluntaryWork
    {
        public VoluntaryWorkBL(EBFPEntities context) : base(context)
        {
        }

        public void InsertBulk(List<VoluntaryWorkModel> model, int Emp_Id)
        {
            if (model != null)
            {
                foreach (var item in model)
                    item.EVW_Emp_Id = Emp_Id;
            }
            InsertBulk(model, a => a.EVW_Emp_Id == Emp_Id);
        }
    }
}
