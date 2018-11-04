namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;

    public class MandatoryTrainingBL : Repository<tblMandatoryTrainings, MandatoryTrainingModel>, IMandatoryTraining
    {
        public MandatoryTrainingBL(EBFPEntities context) : base(context)
        {
        }
    } 
}
