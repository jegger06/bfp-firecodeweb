
using System.Linq;
using EBFP.BL.Helper;

namespace EBFP.BL.Establishment
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System;
    using System.Collections.Generic;

    public interface IEstablishment : IRepository<tblEstablishments, EstablishmentModel>
    {
        //List<EstablishmentModel> GetEstablishmentListByUnit(int unitId);
        EstablishmentDetailModel GetEstablishmentDetails(int unitId);
        void SyncEstablishmentLocalToServer(List<EstablishmentModel> model);
        //EstablishmentListResult GetListResult(GridInfo gridInfo);
        EstablishmentModel GetEstablishmentById(int establishmentId);
        EstablishmentListResult GetEstablishmentListByUnit(GridInfo gridInfo,int unitId);
        bool DeleteDuplicateEstablishments(int unitId);

        ///
        List<string> EstablishmentsName(int unitID);
        List<string> EstablishmentsMPNumber(int unitID);
        List<CalendarModel> GetCalendarDetails(DateTime selectedDate, int unitId);
        List<CalendarModel> GetExpiredFSIC(int unitId);
        void UploadBPLOEstablishment(List<EstablishmentModel> model);
        List<string> EstablishmentsOwnerName(int unitID);

        long GetEstCompliantCount(int unitId);
        long GetEstNonCompliantCount(int unitId);
        long GetEstHazardHighCount(int unitId);
        long GetEstHazardModLowCount(int unitId);
        void UpdateExpiryDate(int unitId, string estId);
        void UpdateRegistrationStatus(int unitId, string estId, int regStat);
        bool UpdateEstablishment(EstablishmentModel model);
        int GetEstablishmentIdByRefId(string ref_est_id);
    }
}