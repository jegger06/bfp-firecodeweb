using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.InspectionOrder
{
    public class NTCVDeficienciesBL : Repository<tblNoticeToCorrectViolationDeficiencies, NTCVDeficienciesModel>, INTCVDeficiencies
    {
        public NTCVDeficienciesBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }
        public void CreateMapping()
        {
            Mapper.CreateMap<tblNoticeToCorrectViolationDeficiencies, NTCVDeficienciesModel>().ReverseMap();
            Mapper.CreateMap<List<tblNoticeToCorrectViolationDeficiencies>, List<NTCVDeficienciesModel>>().ReverseMap();
            Mapper.CreateMap<List<tblNoticeToCorrectViolationDeficiencies>, List<NTCVDeficienciesModel>>();
        }

        public void SyncNTCVDeficienciesLocalToServer(List<NTCVDeficienciesModel> ntcvDefeciencies)
        {

            foreach (var ntcvd in ntcvDefeciencies)
            {
                var NTCVDeficiencies = BFPContext.tblNoticeToCorrectViolationDeficiencies
                                            .FirstOrDefault(a => a.Ref_NTCVD_Id == ntcvd.Ref_NTCVD_Id && a.NTCVD_Unit_Id == ntcvd.NTCVD_Unit_Id);

                if (NTCVDeficiencies == null)
                {
                    NTCVDeficiencies = new tblNoticeToCorrectViolationDeficiencies();
                    Mapper.Map(ntcvd, NTCVDeficiencies);
                    BFPContext.tblNoticeToCorrectViolationDeficiencies.Add(NTCVDeficiencies);
                }
                else
                {
                    var ntcvdId = NTCVDeficiencies.NTCVD_Id;
                    Mapper.Map(ntcvd, NTCVDeficiencies);
                    NTCVDeficiencies.NTCVD_Id = ntcvdId;
                }
            }

            BFPContext.SaveChanges();
        }
    }
}