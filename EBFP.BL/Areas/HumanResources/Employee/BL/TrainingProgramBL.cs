namespace EBFP.BL.HumanResources
{
    using DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public class TrainingProgramBL : Repository<tblEmployeeTrainingPrograms, TrainingProgramModel>, ITrainingProgram
    {
        public TrainingProgramBL(EBFPEntities context) : base(context)
        {
        }

        public void InsertBulk(List<TrainingProgramModel> model, int Emp_Id)
        {
            if (model != null)
            {
                foreach (var item in model)
                    item.ETP_Emp_Id = Emp_Id;
            }
            InsertBulk(model, a => a.ETP_Emp_Id == Emp_Id);
        }
    }
}
