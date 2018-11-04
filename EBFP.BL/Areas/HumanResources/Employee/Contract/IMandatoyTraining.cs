
namespace EBFP.BL.HumanResources
{
    using DataAccess;
    using Queries.Core.Repositories;
    public interface IMandatoryTraining : IRepository<tblMandatoryTrainings, MandatoryTrainingModel>
    { 
    }
}