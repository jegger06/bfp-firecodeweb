using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public class ServiceRecordBL : Repository<tblEmployeeServiceAppointments,ServiceAppointmentModel>, IServiceRecord
    {
        public ServiceRecordBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblEmployeeServiceAppointments, ServiceAppointmentModel>().ReverseMap();
            Mapper.CreateMap<tblEmployeeServiceAppointments, ServiceAppointmentModel>();
            Mapper.CreateMap<List<tblEmployeeServiceAppointments>, List<ServiceAppointmentModel>>().ReverseMap();
            Mapper.CreateMap<List<tblEmployeeServiceAppointments>, List<ServiceAppointmentModel>>();
        }

        public void InsertBulk(List<ServiceAppointmentModel> model, int Emp_Id)
        {
            if (model != null)
            {
                foreach (var item in model)
                {
                    item.ESA_Emp_Id = Emp_Id;
                }
            }

            InsertBulk(model, a => a.ESA_Emp_Id == Emp_Id);
        }

        public List<ServiceAppointmentModel> GetServiceAppointmentByEmpId(int empId)
        {
            var ret = BFPContext.tblEmployeeServiceAppointments.Where(a => a.ESA_Emp_Id == empId).Project().To<ServiceAppointmentModel>().ToList();
            return ret;
        }
    }
}
