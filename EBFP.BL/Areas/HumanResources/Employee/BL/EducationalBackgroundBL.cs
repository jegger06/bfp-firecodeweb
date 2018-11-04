namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public  class EducationalBackgroundBL : Repository<tblEmployeeEducationalBackground, EducBackgroundModel>, IEducationalBackground
    {
        public EducationalBackgroundBL(EBFPEntities context) : base(context)
        {
        }

        public void InsertBulk(List<EducBackgroundModel> model, int Emp_Id)
        {
                

            if (model != null)
            {
                foreach (var item in model)
                    item.EEB_Emp_Id = Emp_Id;
            }

            InsertBulk(model, a => a.EEB_Emp_Id == Emp_Id);
        }
    }
}
