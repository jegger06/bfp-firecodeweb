namespace EBFP.BL.HumanResources
{
    using DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public class ReferenceBL : Repository<tblEmployeeReferences, ReferenceModel>, IReference
    {
        public ReferenceBL(EBFPEntities context) : base(context)
        {
        }

        public void InsertBulk(List<ReferenceModel> model, int Emp_Id)
        {
            if (model != null)
            {
                foreach (var item in model)
                    item.ER_Emp_Id = Emp_Id;
            }
            InsertBulk(model, a => a.ER_Emp_Id == Emp_Id);
        }
    }
}
