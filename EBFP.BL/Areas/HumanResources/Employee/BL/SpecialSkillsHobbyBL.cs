namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public class SpecialSkillsHobbyBL : Repository<tblEmployeeSpecialSkillsHobbies, SpecialSkillsHobbyModel>, ISpecialSkillsHobby
    {
        public SpecialSkillsHobbyBL(EBFPEntities context) : base(context)
        {
        }

        public void InsertBulk(List<SpecialSkillsHobbyModel> model, int Emp_Id)
        {
            if (model != null)
            {
                foreach (var item in model)
                    item.ESSH_Emp_Id = Emp_Id;
            }
            InsertBulk(model, a => a.ESSH_Emp_Id == Emp_Id);
        }
    }
}
