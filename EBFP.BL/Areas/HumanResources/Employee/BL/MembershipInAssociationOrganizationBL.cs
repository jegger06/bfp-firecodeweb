namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public class MembershipInAssociationOrganizationBL : Repository<tblEmployeeMembershipInAssociationOrganizations, MembershipInAssociationOrganizationModel> ,  IMembershipInAssociationOrganization
    {
        public MembershipInAssociationOrganizationBL(EBFPEntities context) : base(context)
        {
        }

        public void InsertBulk(List<MembershipInAssociationOrganizationModel> model, int Emp_Id)
        {
            if (model != null)
            {
                foreach (var item in model)
                    item.EMIAO_Emp_Id = Emp_Id;
            }
            InsertBulk(model, a => a.EMIAO_Emp_Id == Emp_Id);
        }
    }
}
