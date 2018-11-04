using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.InspectionOrder
{
    public class AbatementOrderBL : Repository<tblAbatementOrder, AbatementOrderModel>, IAbatementOrder
    {
        public AbatementOrderBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }
        public void CreateMapping()
        {
            Mapper.CreateMap<tblAbatementOrder, AbatementOrderModel>().ReverseMap();
            Mapper.CreateMap<List<tblAbatementOrder>, List<AbatementOrderModel>>().ReverseMap();
            Mapper.CreateMap<List<tblAbatementOrder>, List<AbatementOrderModel>>();
        }

        public void SyncAbatementOrderLocalToServer(List<AbatementOrderModel> abatementOrder)
        {

            foreach (var ao in abatementOrder)
            {
                var AbatementOrder = BFPContext.tblAbatementOrder
                                            .FirstOrDefault(a => a.Ref_AO_Id == ao.Ref_AO_Id && a.AO_Unit_Id == ao.AO_Unit_Id);

                if (AbatementOrder == null)
                {
                    AbatementOrder = new tblAbatementOrder();
                    Mapper.Map(ao, AbatementOrder);
                    BFPContext.tblAbatementOrder.Add(AbatementOrder);
                }
                else
                {
                    var aoId = AbatementOrder.AO_Id;
                    Mapper.Map(ao, AbatementOrder);
                    AbatementOrder.AO_Id = aoId;
                }
            }

            BFPContext.SaveChanges();
        }
    }
}