namespace EBFP.BL.FSEC
{
    using DataAccess;
    using Establishment;
    using Helper;
    using System.Collections.Generic;
    using Queries.Core.Repositories;
    public interface IFireSafetyEvaluationClearance : IRepository<tblFSECApplication, FSECModel>
    {
        List<FSECEstablismentsModel> Establishments(FSEC_Status status, string searchTerm, int unitId);
        void UpdateFSECForCollection(FSECModel model);
        void UpdateFSECForReleasing(FSECModel model);
        void UpdateToReleaseFSEC(FSECModel model);
        void SaveRemarks(FSECModel model);
        List<FSECEstablismentsModel> ForReleasingEstablishments(FSEC_Status status, string searchTerm, int unitId);
        List<string> ReleasedFSECNumber(int unitID);
        EstablishmentModel NonEstByFsecNumber(string fsecNumber, int unitID);
        List<string> ReleasedORNumber(int unitID);
        FSECOccupancyModel NonEstByORNumber(int orNumber, int unitID);
        List<FSECEstablismentsModel> FsecNotification(FSEC_Status status, int unitId);
        void UpdateTranferFSEC(FSECModel model, int unitId);
    }
}
