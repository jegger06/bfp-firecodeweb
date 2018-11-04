namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public class NonAcademicDistinctionBL : Repository<tblEmployeeNonAcademicDistinctions, NonAcademicDistinctionModel>,  INonAcademicDistinction
    {
        public NonAcademicDistinctionBL(EBFPEntities context) : base(context)
        {
        }

        public void InsertBulk(List<NonAcademicDistinctionModel> model, int Emp_Id)
        {
            if (model != null)
            {
                foreach (var item in model)
                    item.ENAD_Emp_Id = Emp_Id;
            }
            InsertBulk(model, a => a.ENAD_Emp_Id == Emp_Id);
        }
    }
}
