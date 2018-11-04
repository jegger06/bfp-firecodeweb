using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.InspectionOrder
{
    public class ClosureOrderDeficienciesBL : Repository<tblClosureOrderDeficiencies, ClosureOrderDeficienciesModel>, IClosureOrderDeficiencies
    {
        public ClosureOrderDeficienciesBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }
        public void CreateMapping()
        {
            Mapper.CreateMap<tblClosureOrderDeficiencies, ClosureOrderDeficienciesModel>().ReverseMap();
            Mapper.CreateMap<List<tblClosureOrderDeficiencies>, List<ClosureOrderDeficienciesModel>>().ReverseMap();
            Mapper.CreateMap<List<tblClosureOrderDeficiencies>, List<ClosureOrderDeficienciesModel>>();
        }

        public void SyncClosureOrderDeficienciesLocalToServer(List<ClosureOrderDeficienciesModel> closureOrderDefeciencies)
        {

            foreach (var aod in closureOrderDefeciencies)
            {
                var ClosureOrderDeficiencies = BFPContext.tblClosureOrderDeficiencies
                                            .FirstOrDefault(a => a.Ref_COD_Id == aod.Ref_COD_Id && a.COD_Unit_Id == aod.COD_Unit_Id);

                if (ClosureOrderDeficiencies == null)
                {
                    ClosureOrderDeficiencies = new tblClosureOrderDeficiencies();
                    Mapper.Map(aod, ClosureOrderDeficiencies);
                    BFPContext.tblClosureOrderDeficiencies.Add(ClosureOrderDeficiencies);
                }
                else
                {
                    var codId = ClosureOrderDeficiencies.COD_Id;
                    Mapper.Map(aod, ClosureOrderDeficiencies);
                    ClosureOrderDeficiencies.COD_Id = codId;
                }
            }

            BFPContext.SaveChanges();
        }
    }
}