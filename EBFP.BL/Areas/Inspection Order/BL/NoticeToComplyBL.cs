using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.InspectionOrder
{
    public class NoticeToComplyBL : Repository<tblNoticeToComply, NoticeToComplyModel>, INoticeToComply
    {
        public NoticeToComplyBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }
        public void CreateMapping()
        {
            Mapper.CreateMap<tblNoticeToComply, NoticeToComplyModel>().ReverseMap();
            Mapper.CreateMap<List<tblNoticeToComply>, List<NoticeToComplyModel>>().ReverseMap();
            Mapper.CreateMap<List<tblNoticeToComply>, List<NoticeToComplyModel>>();
        }

        public void SyncNTCLocalToServer(List<NoticeToComplyModel> noticeToComply)
        {

            foreach (var ntc in noticeToComply)
            {
                var NoticeToComply = BFPContext.tblNoticeToComply
                                            .FirstOrDefault(a => a.Ref_NTC_Id == ntc.Ref_NTC_Id && a.NTC_Unit_Id == ntc.NTC_Unit_Id);

                if (NoticeToComply == null)
                {
                    NoticeToComply = new tblNoticeToComply();
                    Mapper.Map(ntc, NoticeToComply);
                    BFPContext.tblNoticeToComply.Add(NoticeToComply);
                }
                else
                {
                    var ntcId = NoticeToComply.NTC_Id;
                    Mapper.Map(ntc, NoticeToComply);
                    NoticeToComply.NTC_Id = ntcId;
                }
            }

            BFPContext.SaveChanges();
        }
    }
}