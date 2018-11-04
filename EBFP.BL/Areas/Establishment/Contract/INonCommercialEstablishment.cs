
namespace EBFP.BL.Establishment
{
    using EBFP.DataAccess;
    using FSEC;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface INonCommercialEstablishment : IRepository<tblNonCommercialEstablishments, NonCommercialEstablishmentModel>
    {
        void SyncNonCommercialEstLocalToServer(List<NonCommercialEstablishmentModel> model);

        ///

        string SaveNonCommercialEst(NonCommercialEstablishmentModel model);
        NCEOPSModel GetNCE(string nceId, int unitId);
        FSECNCEReportModel GetNCEReport(string nceId, int unitId);
        NCEOPSModel GetNonCommercialOtherFee(string nceId, int unitId);
        List<string> NonCommercialEstablishmentsOwnerName(int unitID);
        FSECOccupancyModel NonEstByOwnerName(string ownerName, int unitID);
    }
}