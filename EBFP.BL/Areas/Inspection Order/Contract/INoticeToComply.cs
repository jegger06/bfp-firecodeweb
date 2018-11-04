
namespace EBFP.BL.InspectionOrder
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface INoticeToComply : IRepository<tblNoticeToComply, NoticeToComplyModel>
    {
        void SyncNTCLocalToServer(List<NoticeToComplyModel> model);
    }
}