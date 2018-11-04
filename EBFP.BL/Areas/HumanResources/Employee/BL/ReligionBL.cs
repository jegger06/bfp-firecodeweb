using System;
using System.Linq;

namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;

    public class ReligionBL : Repository<tblReligions, ReligionModel>, IReligion
    {
        public ReligionBL(EBFPEntities context) : base(context)
        {
        }
    } 
}
