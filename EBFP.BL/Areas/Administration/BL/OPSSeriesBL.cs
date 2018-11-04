using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EBFP.DataAccess;
using Queries.Core.Repositories;
using EBFP.BL.Helper;

namespace EBFP.BL.Administration
{
    public class OPSSeriesBL : Repository<tblOPSSeries, OPSSeriesModel>, IOPSSeries
    {
        public OPSSeriesBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public void SyncOPSSeriesLocalToServer(List<OPSSeriesModel> opsSeries)
        {
            foreach (var ops in opsSeries)
            {
                var OPSSeries = BFPContext.tblOPSSeries
                    .FirstOrDefault(a => a.Ref_OPS_Id == ops.Ref_OPS_Id && a.OPS_Unit_Id == ops.OPS_Unit_Id);

                if (OPSSeries == null)
                {
                    OPSSeries = new tblOPSSeries();
                    Mapper.Map(ops, OPSSeries);
                    BFPContext.tblOPSSeries.Add(OPSSeries);
                }
                else
                {
                    var opsId = OPSSeries.OPS_Id;
                    Mapper.Map(ops, OPSSeries);
                    OPSSeries.OPS_Id = opsId;
                }
            }

            BFPContext.SaveChanges();
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblOPSSeries, OPSSeriesModel>().ReverseMap();
            Mapper.CreateMap<List<tblOPSSeries>, List<OPSSeriesModel>>().ReverseMap();
            Mapper.CreateMap<List<tblOPSSeries>, List<OPSSeriesModel>>();
        }

        public bool ReleasedOPS(string opsNumber, int unitId)
        {
            var fsecOR =
                BFPContext.tblFSECApplication.FirstOrDefault(
                    a => a.FSEC_Status == (int)FSEC_Status.Released && a.FSEC_OPS_Number == opsNumber && a.FSEC_Unit_Id == unitId);

            if (fsecOR != null)
                return true;


            var fsicOR =
                BFPContext.tblFSICApplication.FirstOrDefault(
                    a => a.FSIC_Status == (int)FSIC_Status.Released && a.FSIC_OPS_Number == opsNumber && a.FSIC_Unit_Id == unitId);

            if (fsicOR != null)
                return true;

            var OtherFeesOR = BFPContext.tblOtherFees.FirstOrDefault(
                 a => a.OF_Status == (int)OTHERPYMT_Status.Released && a.OF_OPS_Number == opsNumber && a.OF_Unit_Id == unitId);

            if (OtherFeesOR != null)
                return true;

            return false;
        }
    }
}