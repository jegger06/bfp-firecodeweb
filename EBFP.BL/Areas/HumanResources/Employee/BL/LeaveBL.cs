using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EBFP.BL.Helper;
using EBFP.DataAccess;
using EBFP.Helper;

namespace EBFP.BL.HumanResources
{
    public class LeaveBL : EntityFrameworkBase, ILeave
    {
        public LeaveBL(EBFPEntities _context)
        {
            context_ = _context;
            CreateMapping();
        }

        public LeaveListResult GetEmployeeLeave(GridInfo gridInfo)
        {
            var employeeId = Convert.ToInt32(gridInfo.searchValue.Decrypt());
            var leaveRecords = context.tblEmployeeLeaveRecords
                .Where(a => a.ELR_Emp_Id == employeeId);
            //TODO :add where clause if neccesary

            //TODO :add where clause if neccesary
            gridInfo.recordsTotal = leaveRecords.Select(a => a.ELR_Id).Count();


            var leaveResult = leaveRecords.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
                .Skip(gridInfo.start)
                .Take(gridInfo.length);

            var retLeave = new List<LeaveModel>();
            retLeave = leaveResult.Project().To<LeaveModel>().ToList();
            retLeave.ForEach(a => a.ELR_LeaveType_Desc = ((LeaveParticular) a.ELR_LeaveType).ToString());

            return new LeaveListResult
            {
                LeaveListModel = retLeave,
                DatatableInfo = gridInfo
            };
        }

        public double CalculateTotalAccruedLeave(DateTime dateHired)
        {
            return CalculateTotalAccruedLeave(dateHired, DateTime.Now);
        }

        public double CalculateTotalAccruedLeave(DateTime dtStart, DateTime dtEnd)
        {
            var total = 0.0;
            var accruedStart = 0.0;
            var accruedEnd = 0.0;

            var startDate = dtStart.Date;
            var endDate = dtEnd.Date;

            var dateStart = startDate;

            //if startDate day is not 1 (month not completed), get accrued leave for that month
            if (dateStart.Day != 1)
            {
                var daysInMonth = DateTime.DaysInMonth(dateStart.Year, dateStart.Month);
                var endOfTheMonth = new DateTime(dateStart.Year, dateStart.Month, daysInMonth).Date;
                double daysCompleted = 0;
                var accruedPerDay = 1.25/daysInMonth;

                if (endOfTheMonth <= endDate.Date)
                {
                    daysCompleted = (endOfTheMonth.Date.AddDays(1) - dateStart.Date).TotalDays;
                }
                else
                {
                    daysCompleted = (endDate.Date.AddDays(1) - dateStart.Date).TotalDays;
                }

                accruedStart = daysCompleted*accruedPerDay; //compute Accrued Leave for the month

                //Set the startDate as the firstDay of the next month (Day must be 1)    
                dateStart = endOfTheMonth.AddDays(1);
            }

            //All completed month, add 1.25
            if (dateStart.Day == 1)
            {
                for (var date = dateStart.Date; date <= endDate.Date; date = date.AddMonths(1))
                {
                    var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
                    var endOfTheMonth = new DateTime(date.Year, date.Month, daysInMonth).Date;

                    if (endOfTheMonth <= endDate.Date)
                    {
                        total += 1.25;
                    }
                }
            }

            //if endDate day is not the end of that month (month not completed), get accrued leave for that month
            var daysInMonth_endDate = DateTime.DaysInMonth(endDate.Year, endDate.Month);
            if (endDate.Day != daysInMonth_endDate &&
                (startDate.Month != endDate.Month || startDate.Year != endDate.Year))
            {
                var firstDayOfMonth = new DateTime(endDate.Year, endDate.Month, 1).Date;
                var daysCompleted = (endDate.AddDays(1).Date - firstDayOfMonth.Date).TotalDays;
                var accruedPerDay = 1.25/daysInMonth_endDate;

                accruedEnd = daysCompleted*accruedPerDay; //compute Accrued Leave for the month                
            }

            total = total + accruedStart + accruedEnd;

            return total;
        }

        public LeaveModel LeaveRecord(int leaveId)
        {
            var leave = new LeaveModel();
            var resLeave = context.tblEmployeeLeaveRecords.Find(leaveId);

            if (resLeave != null)
                Mapper.Map(resLeave, leave);

            return leave;
        }

        public List<LeaveModel> LeaveRecordList(int empId)
        {
            var leave = new List<LeaveModel>();
            var resLeave = context.tblEmployeeLeaveRecords.Where(a => a.ELR_Emp_Id == empId).ToList();

            if (resLeave.Count > 0)
                Mapper.Map(resLeave, leave);

            foreach (var item in resLeave)
            {
                var leaveModel = new LeaveModel();

                leaveModel.Emp_FirstName = item.tblEmployees.Emp_FirstName;

                leave.Add(leaveModel);
            }

            return leave;
        }

        public LeaveModel SaveLeaveRecord(LeaveModel model)
        {
            if (model == null)
                throw new NullReferenceException();

            var leave = new tblEmployeeLeaveRecords();
            if (model.ELR_Id > 0)
            {
                leave = context.tblEmployeeLeaveRecords.FirstOrDefault(a => a.ELR_Id == model.ELR_Id);

                if (leave != null)
                    Mapper.Map(model, leave);
            }
            else
            {
                Mapper.Map(model, leave);
                context.tblEmployeeLeaveRecords.Add(leave);
            }


            context.SaveChanges();
            model.ELR_Id = leave.ELR_Id;

            return model;
        }

        public void DeleteLeaveRecord(int leaveId)
        {
            var resLeave = context.tblEmployeeLeaveRecords.FirstOrDefault(a => a.ELR_Id == leaveId);

            if (resLeave != null)
            {
                context.tblEmployeeLeaveRecords.Remove(resLeave);
                context.SaveChanges();
            }
        }

        public async Task<LeaveCreditsModel> GetLeaveCredits(int emp_Id)
        {
            IHRISUnitOfWork unitOfWork = new HRISUnitOfWork();

            var leaveCredit = new LeaveCreditsModel();
            var retEmployee = unitOfWork.Employee.EmployeeDetails(emp_Id);

            double earned = 0;
            if (retEmployee.Emp_Service_StartDate != null)
                earned = CalculateTotalAccruedLeave(retEmployee.Emp_Service_StartDate.Value);


            var serviceRecord =
                context.tblEmployeeServiceAppointments.Where(
                    a => a.ESA_Emp_Id == emp_Id && a.ESA_DutyStatus == (int) DutyStatuses.AWOL).ToList();

            double totalAwol = 0;
            if (serviceRecord.Count > 0)
            {
                foreach (var item in serviceRecord)
                {
                    var awol = CalculateTotalAccruedLeave(item.ESA_ApptDate, item.ESA_EndDate.Value);
                    totalAwol = totalAwol + awol;
                }
            }
            //Earned
            earned = earned - totalAwol;
            leaveCredit.EarnedVacationLeave = earned;
            leaveCredit.EarnedSickLeave = earned;

            var resLeave = context.tblEmployeeLeaveRecords.Where(a => a.ELR_Emp_Id == emp_Id).ToList();

            if (resLeave.Count > 0)
            {
                var leave = resLeave.Where(a => a.ELR_Emp_Id == emp_Id && a.ELR_WithPay);

                //Enjoyed
                leaveCredit.EnjoyedVacationLeave = (double)
                    leave.Where(a => a.ELR_ChargeTo == (int) LeaveParticular.Vacation)
                        .Select(a => a.ELR_TotalDays)
                        .Sum();

                leaveCredit.EnjoyedSickLeave = (double)
                    leave.Where(a => a.ELR_ChargeTo == (int) LeaveParticular.Sick).Select(a => a.ELR_TotalDays).Sum();
            }

            //Remaining 
            leaveCredit.RemainingVacationLeave = earned - leaveCredit.EnjoyedVacationLeave;
            leaveCredit.RemainingSickLeave = earned - leaveCredit.EnjoyedSickLeave;
            return leaveCredit;
        }

        public List<LeaveRecordModel> GetLeaveReport(DateTime serviceStartDate, int emp_Id, DateTime reportEndDate)
        {
            var leaveRecordList = new List<LeaveRecordModel>();

            var startDate = serviceStartDate;
            var endDate = reportEndDate;
            var newStartDate = DateTime.Now;
            var newEndDate = DateTime.Now;

            //If StartDate is not January 1, get earned leave from startdate to Dec 31
            if (startDate.Month != 1 && startDate.Day != 1)
            {
                var end = new DateTime(startDate.Year, 12, 31);

                var leaveRecordModel = GetLeaveRecords(startDate, end, emp_Id);
                leaveRecordList.Add(leaveRecordModel);
                newStartDate = new DateTime(startDate.AddYears(1).Year, 1, 1);
            }

            if (endDate.Month != 12 && endDate.Day != 31)
                newEndDate = new DateTime(endDate.AddYears(-1).Year, 12, 31);

            //Loop date with startDate = Jan 1 and endDate Dec 31
            for (var dt = newStartDate; dt <= newEndDate; dt = dt.AddYears(1))
            {
                if (startDate.Month != 1 && startDate.Day != 1 &&
                    endDate.Month != 12 && endDate.Day != 31)
                {
                    var endDt = new DateTime(dt.Year, 12, 31);
                    var leaveRecordModel = GetLeaveRecords(dt, endDt, emp_Id);
                    leaveRecordList.Add(leaveRecordModel);
                }
            }

            //If endDate is not Dec 31, get earned leave from Jan 1 to endDate
            if (endDate.Month != 12 && endDate.Day != 31)
            {
                var start = new DateTime(endDate.Year, 1, 1);
                var leaveRecordModel = GetLeaveRecords(start, endDate, emp_Id);
                leaveRecordList.Add(leaveRecordModel);
            }
            return leaveRecordList;
        }

        public decimal GetLeavePerYear(int empId, DateTime startDate, int leaveType, bool withPay)
        {
            decimal totalLeave = 0;
            var leaveTypes = new List<int>();
            if (leaveType == (int) LeaveParticular.Vacation)
            {
                leaveTypes = new List<int>
                {
                    (int) LeaveParticular.Vacation,
                    (int) LeaveParticular.Calamity,
                    (int) LeaveParticular.Mandatory,
                    (int) LeaveParticular.Maternity,
                    (int) LeaveParticular.Paternity,
                    (int) LeaveParticular.Others
                };
            }
            else
            {
                leaveTypes = new List<int>
                {
                    (int) LeaveParticular.Sick
                };
            }

            var leavePerYear = context.tblEmployeeLeaveRecords
                .Where(a => a.ELR_WithPay == withPay &&
                            leaveTypes.Contains(a.ELR_ChargeTo) &&
                            a.ELR_Emp_Id == empId &&
                            a.ELR_StartDate.Year == startDate.Year)
                .Select(a => new {a.ELR_TotalDays}).ToList();

            if (leavePerYear.Count > 0)
            {
                totalLeave = leavePerYear.Sum(a => a.ELR_TotalDays);
                return totalLeave;
            }
            return totalLeave;
        }

        public decimal GetLeaveCredits(int empId, int leaveType)
        {
            decimal totalLeave = 0;
            var leaveTypes = new List<int>();
            if (leaveType == (int) LeaveParticular.Vacation)
            {
                leaveTypes = new List<int>
                {
                    (int) LeaveParticular.Vacation,
                    (int) LeaveParticular.Calamity,
                    (int) LeaveParticular.Mandatory,
                    (int) LeaveParticular.Maternity,
                    (int) LeaveParticular.Paternity,
                    (int) LeaveParticular.Others
                };
            }
            else
            {
                leaveTypes = new List<int>
                {
                    (int) LeaveParticular.Sick
                };
            }

            var leavePerYear = context.tblEmployeeLeaveRecords
                .Where(a => a.ELR_WithPay &&
                            leaveTypes.Contains(a.ELR_ChargeTo) &&
                            a.ELR_Emp_Id == empId)
                .Select(a => new {a.ELR_TotalDays}).ToList();

            if (leavePerYear.Count > 0)
            {
                totalLeave = leavePerYear.Sum(a => a.ELR_TotalDays);
                return totalLeave;
            }
            return totalLeave;
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblEmployeeLeaveRecords, LeaveModel>().ReverseMap();
            Mapper.CreateMap<List<tblEmployeeLeaveRecords>, List<LeaveModel>>().ReverseMap();
            Mapper.CreateMap<List<tblEmployeeLeaveRecords>, List<LeaveModel>>();
        }

        public LeaveRecordModel GetLeaveRecords(DateTime startDate, DateTime endDate, int empId)
        {
            var leaveRecordModel = new LeaveRecordModel();
            {
                leaveRecordModel.StartDate = startDate;
                leaveRecordModel.EndDate = endDate;

                var serviceRecord =
                    context.tblEmployeeServiceAppointments.Where(
                        a =>
                            a.ESA_Emp_Id == empId && a.ESA_DutyStatus == (int) DutyStatuses.AWOL &&
                            a.ESA_EndDate.Value.Year >= startDate.Year && a.ESA_ApptDate.Year <= endDate.Year).ToList();
                double totalAwol = 0;
                if (serviceRecord.Count > 0)
                {
                    foreach (var item in serviceRecord)
                    {
                        var awolStart = item.ESA_ApptDate.Year == startDate.Year
                            ? item.ESA_ApptDate
                            : new DateTime(startDate.Year, 01, 01);

                        var awolEnd = item.ESA_EndDate.Value.Year == startDate.Year
                            ? item.ESA_EndDate.Value
                            : new DateTime(startDate.Year, 12, 31);

                        var awol = CalculateTotalAccruedLeave(awolStart, awolEnd);
                        totalAwol = totalAwol + awol;
                    }
                }

                leaveRecordModel.Earned = CalculateTotalAccruedLeave(startDate, endDate) - totalAwol;
                leaveRecordModel.VacLeaveWithOutPay = GetLeavePerYear(empId, startDate, (int) LeaveParticular.Vacation,
                    false);
                leaveRecordModel.VacLeaveWithPay = GetLeavePerYear(empId, startDate, (int) LeaveParticular.Vacation,
                    true);
                leaveRecordModel.SickLeaveWithOutPay = GetLeavePerYear(empId, startDate, (int) LeaveParticular.Sick,
                    false);
                leaveRecordModel.SickLeaveWithPay = GetLeavePerYear(empId, startDate, (int) LeaveParticular.Sick, true);

                var particularsPerYear = GetParticularsPerYear(empId, startDate);
                leaveRecordModel.Particular = (particularsPerYear.ML <= 0 ? "" : particularsPerYear.ML + "ML/") +
                                              (particularsPerYear.VL <= 0 ? "" : particularsPerYear.VL + "VL/") +
                                              (particularsPerYear.SL <= 0 ? "" : particularsPerYear.SL + "SL/") +
                                              (particularsPerYear.PL <= 0 ? "" : particularsPerYear.PL + "PL/") +
                                              (particularsPerYear.MD <= 0 ? "" : particularsPerYear.MD + "MD/") +
                                              (particularsPerYear.CL <= 0 ? "" : particularsPerYear.CL + "CL/") +
                                              (particularsPerYear.OL <= 0 ? "" : particularsPerYear.OL + "OL/");

                leaveRecordModel.Particular = leaveRecordModel.Particular.TrimEnd('/');
            }

            return leaveRecordModel;
        }

        private LeaveRecordModel GetParticularsPerYear(int empId, DateTime startDate)
        {
            var particularPerYear =
                context.tblEmployeeLeaveRecords.Where(
                    a => a.ELR_StartDate.Year == startDate.Year && a.ELR_Emp_Id == empId)
                    .Select(
                        a => new LeaveParticulars
                        {
                            TotalDays = a.ELR_TotalDays,
                            LeaveType = a.ELR_LeaveType
                        }
                    ).ToList();

            var leaveRecordModel = new LeaveRecordModel();
            leaveRecordModel.ML = GetTotalDays(particularPerYear, LeaveParticular.Maternity);
            leaveRecordModel.VL = GetTotalDays(particularPerYear, LeaveParticular.Vacation);
            leaveRecordModel.SL = GetTotalDays(particularPerYear, LeaveParticular.Sick);
            leaveRecordModel.PL = GetTotalDays(particularPerYear, LeaveParticular.Paternity);
            leaveRecordModel.MD = GetTotalDays(particularPerYear, LeaveParticular.Mandatory);
            leaveRecordModel.CL = GetTotalDays(particularPerYear, LeaveParticular.Calamity);
            leaveRecordModel.OL = GetTotalDays(particularPerYear, LeaveParticular.Others);

            return leaveRecordModel;
        }

        public int GetTotalDays(List<LeaveParticulars> particularPerYear, LeaveParticular leaveType)
        {
            return Convert.ToInt32(particularPerYear.Where(a => a.LeaveType == (int) leaveType).Sum(a => a.TotalDays));
        }
    }
}