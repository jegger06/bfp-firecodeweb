
using EBFP.BL.Areas.FSIC.Model;
using EBFP.BL.Helper;
using EBFP.DataAccess;
using Queries.Core.Repositories;
using System.Collections.Generic;

namespace EBFP.BL.FSIC
{

    public interface IFireSafetyInspectionCertificate : IRepository<tblFSICApplication, FSICModel>
    {
        //List<FSICEstablismentsModel> Establishments(FSIC_Status status, string searchTerm, int unitId);
        void UpdateFSIC(FSICModel model);
        void UpdateFSICStatus(FSICModel model, string type = "", string forComplianceType = "");
        void SaveRemarks(FSICModel model);
        string SavePaymentFSIC(FSICModel model);
        OPSFSICModel GetFSIC(string fsicId, int unitId);
        //FSICReportModel GetFSICReport(string fsicId, int unitId);
        //List<FSICEstablismentsModel> FsicNotification(FSIC_Status status, int unitId);
        string GetFSICLastPaymentDate(string est_Id);
        void UpdateTranferFSIC(FSICModel model, int unitId);
    }
}