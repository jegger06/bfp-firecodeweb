using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.InspectionOrder
{
    public class ClosureOrderBL : Repository<tblClosureOrder, ClosureOrderModel>, IClosureOrder
    {
        public ClosureOrderBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }
        public void CreateMapping()
        {
            Mapper.CreateMap<tblClosureOrder, ClosureOrderModel>().ReverseMap();
            Mapper.CreateMap<List<tblClosureOrder>, List<ClosureOrderModel>>().ReverseMap();
            Mapper.CreateMap<List<tblClosureOrder>, List<ClosureOrderModel>>();
        }

        public void SyncClosureOrderLocalToServer(List<ClosureOrderModel> closureOrder)
        {

            foreach (var ao in closureOrder)
            {
                var ClosureOrder = BFPContext.tblClosureOrder
                                            .FirstOrDefault(a => a.Ref_CO_Id == ao.Ref_CO_Id && a.CO_Unit_Id == ao.CO_Unit_Id);

                if (ClosureOrder == null)
                {
                    ClosureOrder = new tblClosureOrder();
                    Mapper.Map(ao, ClosureOrder);
                    BFPContext.tblClosureOrder.Add(ClosureOrder);
                }
                else
                {
                    var coId = ClosureOrder.CO_Id;
                    Mapper.Map(ao, ClosureOrder);
                    ClosureOrder.CO_Id = coId;
                }
            }

            BFPContext.SaveChanges();
        }
    }
}