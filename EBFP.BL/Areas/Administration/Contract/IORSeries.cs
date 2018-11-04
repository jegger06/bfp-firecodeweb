
namespace EBFP.BL.Administration
{
    using EBFP.DataAccess;
    using Helper;
    using Queries.Core.Repositories;
    using System;
    using System.Collections.Generic;

    public interface IORSeries : IRepository<tblORSeries, ORSeriesModel>
    {
        void SyncORSeriesLocalToServer(List<ORSeriesModel> model);
        void UpdateORSeries(ORSeriesModel model);
        long GetORNumber(int unitId);
        void ValidateOR(int orSeries, int unitId);
        //void SyncORSeriesServerToLocal(List<ORSeriesModel> model);
        //void TagAsSycned(List<ORSeriesModel> model);
        ORSeriesModel GetOrSeriesById(string orId, int unitId);
        void TagAsDeleted(ORSeriesModel model, int empId);
        bool ReleaseOrNumber(int orNumber, int unitId);

        long GetCountIssuedOR(int unitId);
        int GetCountUsedOR(int unitId);
        int GetCountSpoiledOR(int unitId);
        List<UsedORModel> GetUsedOR(int unitId, DateTime from, DateTime to, bool isFirstLoad = false);
        List<NumberOfCustomerProcessedModel> GetNumberOfCustomerProcessed(int unitId, DateTime from, DateTime to);
        ORListResult GetSpoiledORList(GridInfo gridInfo, int unitId);

    }
}