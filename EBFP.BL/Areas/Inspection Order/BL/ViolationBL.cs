using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.InspectionOrder
{
    public class ViolationBL : Repository<tblViolations, ViolationModel>, IViolations
    {
        public ViolationBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public void SyncViolationsLocalToServer(List<ViolationModel> model)
        {
            foreach (var violations in model)
            {
                var Violations = BFPContext.tblViolations
                    .FirstOrDefault(
                        a => a.Auto_Violation_Id == violations.Auto_Violation_Id && a.Violation_Code == violations.Violation_Code);

                if (Violations == null)
                {
                    Violations = new tblViolations();
                    Mapper.Map(violations, Violations);
                    BFPContext.tblViolations.Add(Violations);
                }
                else
                {
                    var violationId = Violations.Auto_Violation_Id;
                    Mapper.Map(violations, Violations);
                    Violations.Auto_Violation_Id = violationId;
                }
            }

            BFPContext.SaveChanges();
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblViolations, ViolationModel>().ReverseMap();
            Mapper.CreateMap<List<tblViolations>, List<ViolationModel>>().ReverseMap();
            Mapper.CreateMap<List<tblViolations>, List<ViolationModel>>();
        }
    }
}