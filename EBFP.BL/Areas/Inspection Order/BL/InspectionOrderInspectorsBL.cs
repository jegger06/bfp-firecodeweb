using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.InspectionOrder
{
    public class InspectionOrderInspectorsBL : Repository<tblInspectionOrderInspectors, InspectionOrderInspectorsModel>,
        IInspectionOrderInspectors
    {
        public InspectionOrderInspectorsBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public void SyncInspectionOrderInspectorsLocalToServer(List<InspectionOrderInspectorsModel> model)
        {
            foreach (var inspectors in model)
            {
                var InspectionOrderInspectors = BFPContext.tblInspectionOrderInspectors
                    .FirstOrDefault(
                        a => a.Ref_Insp_Id == inspectors.Ref_Insp_Id && a.Insp_Unit_Id == inspectors.Insp_Unit_Id);

                if (InspectionOrderInspectors == null)
                {
                    InspectionOrderInspectors = new tblInspectionOrderInspectors();
                    Mapper.Map(inspectors, InspectionOrderInspectors);
                    BFPContext.tblInspectionOrderInspectors.Add(InspectionOrderInspectors);
                }
                else
                {
                    var inspId = InspectionOrderInspectors.Insp_Id;
                    Mapper.Map(inspectors, InspectionOrderInspectors);
                    InspectionOrderInspectors.Insp_Id = inspId;
                }
            }

            BFPContext.SaveChanges();
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblInspectionOrderInspectors, InspectionOrderInspectorsModel>().ReverseMap();
            Mapper.CreateMap<List<tblInspectionOrderInspectors>, List<InspectionOrderInspectorsModel>>().ReverseMap();
            Mapper.CreateMap<List<tblInspectionOrderInspectors>, List<InspectionOrderInspectorsModel>>();
        }
    }
}