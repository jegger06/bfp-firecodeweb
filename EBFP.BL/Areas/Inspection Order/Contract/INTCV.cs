﻿
namespace EBFP.BL.InspectionOrder
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface INTCV : IRepository<tblNoticeToCorrectViolations, NTCVModel>
    {
        void SyncNTCVLocalToServer(List<NTCVModel> model);
    }
}