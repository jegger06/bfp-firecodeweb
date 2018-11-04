using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.InspectionOrder
{
    public class AbatementOrderDeficienciesBL : Repository<tblAbatementOrderDeficiencies, AbatementOrderDeficienciesModel>, IAbatementOrderDeficiencies
    {
        public AbatementOrderDeficienciesBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }
        public void CreateMapping()
        {
            Mapper.CreateMap<tblAbatementOrderDeficiencies, AbatementOrderDeficienciesModel>().ReverseMap();
            Mapper.CreateMap<List<tblAbatementOrderDeficiencies>, List<AbatementOrderDeficienciesModel>>().ReverseMap();
            Mapper.CreateMap<List<tblAbatementOrderDeficiencies>, List<AbatementOrderDeficienciesModel>>();
        }

        public void SyncAbatementOrderDeficienciesLocalToServer(List<AbatementOrderDeficienciesModel> abatementOrderDefeciencies)
        {

            foreach (var aod in abatementOrderDefeciencies)
            {
                var AbatementOrderDeficiencies = BFPContext.tblAbatementOrderDeficiencies
                                            .FirstOrDefault(a => a.Ref_AOD_Id == aod.Ref_AOD_Id && a.AOD_Unit_Id == aod.AOD_Unit_Id);

                if (AbatementOrderDeficiencies == null)
                {
                    AbatementOrderDeficiencies = new tblAbatementOrderDeficiencies();
                    Mapper.Map(aod, AbatementOrderDeficiencies);
                    BFPContext.tblAbatementOrderDeficiencies.Add(AbatementOrderDeficiencies);
                }
                else
                {
                    var aodId = AbatementOrderDeficiencies.AOD_Id;
                    Mapper.Map(aod, AbatementOrderDeficiencies);
                    AbatementOrderDeficiencies.AOD_Id = aodId;
                }
            }

            BFPContext.SaveChanges();
        }
    }
}