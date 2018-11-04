using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.InspectionOrder
{
    public class NTCVBL : Repository<tblNoticeToCorrectViolations, NTCVModel>, INTCV
    {
        public NTCVBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }
        public void CreateMapping()
        {
            Mapper.CreateMap<tblNoticeToCorrectViolations, NTCVModel>().ReverseMap();
            Mapper.CreateMap<List<tblNoticeToCorrectViolations>, List<NTCVModel>>().ReverseMap();
            Mapper.CreateMap<List<tblNoticeToCorrectViolations>, List<NTCVModel>>();
        }

        public void SyncNTCVLocalToServer(List<NTCVModel> noticeToCorrectViolations)
        {

            foreach (var ntcv in noticeToCorrectViolations)
            {
                var NoticeToCorrectViolations = BFPContext.tblNoticeToCorrectViolations
                                            .FirstOrDefault(a => a.Ref_NTCV_Id == ntcv.Ref_NTCV_Id && a.NTCV_Unit_Id == ntcv.NTCV_Unit_Id);

                if (NoticeToCorrectViolations == null)
                {
                    NoticeToCorrectViolations = new tblNoticeToCorrectViolations();
                    Mapper.Map(ntcv, NoticeToCorrectViolations);
                    BFPContext.tblNoticeToCorrectViolations.Add(NoticeToCorrectViolations);
                }
                else
                {
                    var ntcvId = NoticeToCorrectViolations.NTCV_Id;
                    Mapper.Map(ntcv, NoticeToCorrectViolations);
                    NoticeToCorrectViolations.NTCV_Id = ntcvId;
                }
            }

            BFPContext.SaveChanges();
        }
    }
}