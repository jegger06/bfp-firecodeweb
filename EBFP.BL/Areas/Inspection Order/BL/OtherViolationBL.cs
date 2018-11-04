using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.InspectionOrder
{
    public class OtherViolationBL : Repository<tblOtherViolations, OtherViolationModel>, IOtherViolations
    {
        public OtherViolationBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public void SyncOtherViolationsLocalToServer(List<OtherViolationModel> model)
        {
            foreach (var violations in model)
            {
                var OtherViolations = BFPContext.tblOtherViolations
                    .FirstOrDefault(
                        a => a.Ref_OV_Id == violations.Ref_OV_Id && a.OV_Unit_Id == violations.OV_Unit_Id);

                if (OtherViolations == null)
                {
                    OtherViolations = new tblOtherViolations();
                    Mapper.Map(violations, OtherViolations);
                    BFPContext.tblOtherViolations.Add(OtherViolations);
                }
                else
                {
                    var otherViolationId = OtherViolations.OV_Id;
                    Mapper.Map(violations, OtherViolations);
                    OtherViolations.OV_Id = otherViolationId;
                }
            }

            BFPContext.SaveChanges();
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblOtherViolations, OtherViolationModel>().ReverseMap();
            Mapper.CreateMap<List<tblOtherViolations>, List<OtherViolationModel>>().ReverseMap();
            Mapper.CreateMap<List<tblOtherViolations>, List<OtherViolationModel>>();
        }
    }
}