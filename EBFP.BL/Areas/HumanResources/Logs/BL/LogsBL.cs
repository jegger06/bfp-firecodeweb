using System;
using System.Collections.Generic;
using AutoMapper;
using EBFP.DataAccess;
using EBFP.Helper;
using Queries.Core.Repositories;

namespace EBFP.BL.HumanResources
{
    public class LogsBL : Repository<tblLogs, LogsModel>, ILogs
    {
        public LogsBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblLogs, LogsModel>().ReverseMap();
            Mapper.CreateMap<List<tblLogs>, List<LogsModel>>().ReverseMap();
            Mapper.CreateMap<List<tblLogs>, List<LogsModel>>();
        }

        public void InsertLogs(LogsModel model)
        {
            //var oldDutyStatus = dutyStatus > 0 ? ((DutyStatuses)dutyStatus).ToDescription() : "None";
            //var newDutyStatus = model.Emp_DutyStatus > 0 && model.Emp_DutyStatus != null ? ((DutyStatuses)model.Emp_DutyStatus).ToDescription() : "None";

            //var logs = new tblLogs
            //{
            //    Log_Emp_Id = model.Emp_Id > 0 ? model.Emp_Id : newEmployee.Emp_Id,
            //    Log_UpdatedDate = DateTime.Now,
            //    Log_Updated_Emp_Id = CurrentUser.EmployeeId
            //};
            //if (model.Emp_Id > 0)
            //    logs.Log_Remarks = "Duty Status From " + oldDutyStatus + " to " + newDutyStatus;
            //else
            //    logs.Log_Remarks = "Newly created employee.";
            var logs = new tblLogs();
            model.Log_UpdatedDate = DateTime.Now;
            model.Log_Updated_Emp_Id = CurrentUser.EmployeeId;

            Mapper.Map(model, logs);

            BFPContext.tblLogs.Add(logs);
            BFPContext.SaveChanges();
        }
    }
}