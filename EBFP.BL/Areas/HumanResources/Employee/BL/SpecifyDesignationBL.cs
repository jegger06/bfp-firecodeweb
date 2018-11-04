namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public class SpecifyDesignationBL : Repository<tblEmployeeSpecifyDesignation, SpecifyDesignationModel>, ISpecifyDesignation
    {
        public SpecifyDesignationBL(EBFPEntities context) : base(context)
        {
        }

        public void InsertBulk(List<SpecifyDesignationModel> model, int Emp_Id)
        {
            if (model != null)
            {
                foreach (var item in model)
                    item.SpecifyDesig_Emp_Id = Emp_Id;
            }
            InsertBulk(model, a => a.SpecifyDesig_Emp_Id == Emp_Id);
        }
    }
}
