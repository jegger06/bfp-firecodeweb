using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.InspectionOrder
{
    public class NTCDeficienciesBL : Repository<tblNoticeToComplyDeficiencies, NTCDeficienciesModel>, INTCDeficiencies
    {
        public NTCDeficienciesBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }
        public void CreateMapping()
        {
            Mapper.CreateMap<tblNoticeToComplyDeficiencies, NTCDeficienciesModel>().ReverseMap();
            Mapper.CreateMap<List<tblNoticeToComplyDeficiencies>, List<NTCDeficienciesModel>>().ReverseMap();
            Mapper.CreateMap<List<tblNoticeToComplyDeficiencies>, List<NTCDeficienciesModel>>();
        }

        public void SyncNTCDeficienciesLocalToServer(List<NTCDeficienciesModel> ntcDefeciencies)
        {

            foreach (var ntcd in ntcDefeciencies)
            {
                var NTCDeficiencies = BFPContext.tblNoticeToComplyDeficiencies
                                            .FirstOrDefault(a => a.Ref_NTCD_Id == ntcd.Ref_NTCD_Id && a.NTCD_Unit_Id == ntcd.NTCD_Unit_Id);

                if (NTCDeficiencies == null)
                {
                    NTCDeficiencies = new tblNoticeToComplyDeficiencies();
                    Mapper.Map(ntcd, NTCDeficiencies);
                    BFPContext.tblNoticeToComplyDeficiencies.Add(NTCDeficiencies);
                }
                else
                {
                    var ntcdId = NTCDeficiencies.NTCD_Id;
                    Mapper.Map(ntcd, NTCDeficiencies);
                    NTCDeficiencies.NTCD_Id = ntcdId;
                }
            }

            BFPContext.SaveChanges();
        }
    }
}