using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.Establishment
{
    public class OtherClearancesBL :
        Repository<tblOtherClearances, OtherClearancesModel>, IOtherClearances
    {
        public OtherClearancesBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }


        public void SyncOtherClearancesEstLocalToServer(List<OtherClearancesModel> otherClearances)
        {
            foreach (var est in otherClearances)
            {
                var OtherClearances = BFPContext.tblOtherClearances
                    .FirstOrDefault(a => a.Ref_OC_Id == est.Ref_OC_Id && a.OC_Unit_Id == est.OC_Unit_Id);

                if (OtherClearances == null)
                {
                    OtherClearances = new tblOtherClearances();
                    Mapper.Map(est, OtherClearances);
                    OtherClearances.OC_Id = 0;
                    OtherClearances.Ref_OC_Id = est.Ref_OC_Id;
                    BFPContext.tblOtherClearances.Add(OtherClearances);
                }
                else
                {
                    var OC_Id = OtherClearances.OC_Id;
                    Mapper.Map(est, OtherClearances);
                    OtherClearances.Ref_OC_Id = est.Ref_OC_Id;
                    OtherClearances.OC_Id = OC_Id;
                }
            }

            BFPContext.SaveChanges();
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblOtherClearances, OtherClearancesModel>().ReverseMap();
            Mapper.CreateMap<List<tblOtherClearances>, List<OtherClearancesModel>>().ReverseMap();
            Mapper.CreateMap<List<tblOtherClearances>, List<OtherClearancesModel>>();
        }
    }
}