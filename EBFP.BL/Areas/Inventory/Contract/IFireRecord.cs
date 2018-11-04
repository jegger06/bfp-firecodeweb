
using System.Collections.Generic;
namespace EBFP.BL.Inventory
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;

    public interface IFireRecord : IRepository<tblFireRecords, FireRecordsModel>
    {
        void InsertBulk(List<FireRecordsModel> model, int municipalityid);
        List<FireIncidentStatistics> GetFireIncidentsStatistics();
        List<FireIncidentFiveYearStat> GetFireIncidentRespondedTo();
        List<FireIncidentFiveYearStat> GetFireIncidentInjured();
        List<FireIncidentFiveYearStat> GetFireIncidentDeaths();
        List<FireIncidentFiveYearStat> GetFireIncidentDamages();
    }
}