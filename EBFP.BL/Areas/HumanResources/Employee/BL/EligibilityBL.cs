using System;
using System.Linq;

namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;

    public class EligibilityBL : Repository<tblEligibilities, EligibilityModel>, IEligibility
    {
        public EligibilityBL(EBFPEntities context) : base(context)
        {
        }

        public string GetEligibilities(int eligibility_Id)
        {
            try
            {
                var eligibityDet= BFPContext.tblEligibilities.FirstOrDefault(a => a.Eligiblity_Id == eligibility_Id);
                return eligibityDet?.Eligibity_Name;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    } 
}
