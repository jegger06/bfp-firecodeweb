using System;

namespace EBFP.BL.HumanResources
{
    using DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public class WorkExperienceBL : Repository<tblEmployeeWorkExperiences, WorkExperienceModel> , IWorkExperience
    {
        public WorkExperienceBL(EBFPEntities context) : base(context)
        {
        }

        public void InsertBulk(List<WorkExperienceModel> model, int Emp_Id)
        {
            if (model != null)
            {
                foreach (var item in model)
                {
                    item.EWE_Emp_Id = Emp_Id;
                }
            }                
            InsertBulk(model, a => a.EWE_Emp_Id == Emp_Id);
        }
    }
}
