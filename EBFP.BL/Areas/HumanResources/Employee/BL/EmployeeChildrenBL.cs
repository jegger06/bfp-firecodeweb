namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public class EmployeeChildrenBL : Repository<tblEmployeeChildren,EmployeeChildModel>, IEmployeeChildren
    {
        public EmployeeChildrenBL(EBFPEntities context) : base(context)
        {
        } 

        public void InsertBulk(List<EmployeeChildModel> model, int Emp_Id)
        {
            //if (model == null) return;
            if (model != null)
            {
                foreach (var item in model)
                {
                    item.EC_Emp_Id = Emp_Id;
                    item.EC_Gender = "";
                }
            }
          
            InsertBulk(model, a => a.EC_Emp_Id == Emp_Id);
        }
    }
}
