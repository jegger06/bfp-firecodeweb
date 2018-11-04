using System.Collections.Generic;
using AutoMapper;
using EBFP.DataAccess;
using Queries.Core.Repositories;
using System.Linq;

namespace EBFP.BL.Administration
{
    public class ApplicationPaymentBL : Repository<tblApplicationPayments, ApplicationPaymentModel>, IApplicationPayment
    {
        public ApplicationPaymentBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }
        public void CreateMapping()
        {
            Mapper.CreateMap<tblApplicationPayments, ApplicationPaymentModel>().ReverseMap();
            Mapper.CreateMap<List<tblApplicationPayments>, List<ApplicationPaymentModel>>().ReverseMap();
            Mapper.CreateMap<List<tblApplicationPayments>, List<ApplicationPaymentModel>>();
        }


        public void SyncApplicationPaymentLocalToServer(List<ApplicationPaymentModel> localModelList)
        {
            foreach (var local in localModelList)
            {
                var payment = BFPContext.tblApplicationPayments
                    .FirstOrDefault(a => a.Ref_AP_Id == local.Ref_AP_Id &&
                                         a.AP_Unit_Id == local.AP_Unit_Id);

                if (payment == null)
                {
                    payment = new tblApplicationPayments();
                    Mapper.Map(local, payment);
                    BFPContext.tblApplicationPayments.Add(payment);
                }
                else
                {
                    var apId = payment.AP_Id;

                    Mapper.Map(local, payment);
                    payment.AP_Id = apId;
                }
            }

            BFPContext.SaveChanges();
        }
    }
}