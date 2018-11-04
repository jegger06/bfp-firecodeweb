
namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using Helper;
    using Queries.Core.Repositories;
    using System.Collections.Generic;

    public interface IEmployeeAppointments : IRepository<tblEmployeeAppointments, EmployeeAppointmentsModel>
    {
        EmployeeAppointmentsModel GetEmployeeAppointmentById(int appointmentId);
        EmployeeAppointmentsListResult GetListResult(GridInfo gridInfo);
        void UpdateEmployeeAppointment(EmployeeAppointmentsModel model);
        bool DeleteByID(int appointmentId);
        EmployeeAppointmentDashboardCounter GetDashboardAppointmentCounter();
    }
}