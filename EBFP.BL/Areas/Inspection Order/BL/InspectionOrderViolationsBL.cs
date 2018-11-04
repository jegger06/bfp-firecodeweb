using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.InspectionOrder
{
    public class InspectionOrderViolationsBL : Repository<tblInspectionOrderViolations, InspectionOrderViolationModel>,
        IInspectionOrderViolations
    {
        public InspectionOrderViolationsBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public void SyncInspectionOrderViolationsLocalToServer(List<InspectionOrderViolationModel> model)
        {
            foreach (var violations in model)
            {
                var InspectionOrderViolations = BFPContext.tblInspectionOrderViolations
                    .FirstOrDefault(
                        a => a.Ref_IOViolation_Id == violations.Ref_IOViolation_Id && a.IOViolation_UnitId == violations.IOViolation_UnitId);

                if (InspectionOrderViolations == null)
                {
                    InspectionOrderViolations = new tblInspectionOrderViolations();
                    Mapper.Map(violations, InspectionOrderViolations);
                    BFPContext.tblInspectionOrderViolations.Add(InspectionOrderViolations);
                }
                else
                {
                    var ioVioalationId = InspectionOrderViolations.IOViolation_Id;
                    Mapper.Map(violations, InspectionOrderViolations);
                    InspectionOrderViolations.IOViolation_Id = ioVioalationId;
                }
            }

            BFPContext.SaveChanges();
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblInspectionOrderViolations, InspectionOrderViolationModel>().ReverseMap();
            Mapper.CreateMap<List<tblInspectionOrderViolations>, List<InspectionOrderViolationModel>>().ReverseMap();
            Mapper.CreateMap<List<tblInspectionOrderViolations>, List<InspectionOrderViolationModel>>();
        }
    }
}