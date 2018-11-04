
namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface IServiceRecord : IRepository<tblEmployeeServiceAppointments,ServiceAppointmentModel> 
    {
        void InsertBulk(List<ServiceAppointmentModel> ServiceRecord, int Emp_Id);
        List<ServiceAppointmentModel> GetServiceAppointmentByEmpId(int empId);
    }
}