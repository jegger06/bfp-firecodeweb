namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;

    public class DutyStatusBL : Repository<tblDutyStatus, DutyStatusModel>, IDutyStatus
    {
        public DutyStatusBL(EBFPEntities context) : base(context)
        {
        }
    } 
}
