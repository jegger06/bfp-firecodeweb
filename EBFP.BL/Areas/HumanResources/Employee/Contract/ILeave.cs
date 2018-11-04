using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EBFP.BL.HumanResources
{
    using Helper;
    public interface ILeave
    {
        LeaveListResult GetEmployeeLeave(GridInfo dataTableInfo);
        double CalculateTotalAccruedLeave(DateTime dateHired);
        LeaveModel LeaveRecord(int leaveId);
        LeaveModel SaveLeaveRecord(LeaveModel model);
        void DeleteLeaveRecord(int leaveId);
        Task<LeaveCreditsModel> GetLeaveCredits(int emp_Id);
        List<LeaveModel> LeaveRecordList(int empId);
        List<LeaveRecordModel> GetLeaveReport(DateTime serviceStartDate, int emp_Id, DateTime endDate);
        decimal GetLeavePerYear(int empId, DateTime startDate, int leaveType, bool withPay);
        double CalculateTotalAccruedLeave(DateTime dtStart, DateTime dtEnd);
        decimal GetLeaveCredits(int empId, int leaveType);
    }
    
}