using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EBFP.BL.Helper;
using EBFP.BL.Inventory;
using EBFP.DataAccess;
using EBFP.Helper;
using Queries.Core.Repositories;

namespace EBFP.BL.HumanResources
{
    public class EmployeeAppointmentsBL : Repository<tblEmployeeAppointments, EmployeeAppointmentsModel>, IEmployeeAppointments
    {
        public EmployeeAppointmentsBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public EmployeeAppointmentsListResult GetListResult(GridInfo gridInfo)
        {
            var employeeAppointments = BFPContext.tblEmployeeAppointments
                .Select(a => new EmployeeAppointmentsModel
                {
                    EA_Id = a.EA_Id,
                    EA_Emp_Id = a.EA_Emp_Id,
                    EA_Emp_Name = a.tblEmployees.Emp_FirstName + " " + a.tblEmployees.Emp_LastName,
                    EA_Rank_Id = a.EA_Rank_Id,
                    EA_Rank_Name = a.tblRanks.Rank_Name,
                    EA_AppoitmentStatus = a.EA_AppoitmentStatus,
                    EA_AppointingAuthority = a.EA_AppointingAuthority,
                    EA_AppointmentDate = a.EA_AppointmentDate,
                    EA_AppoitmentNature = a.EA_AppoitmentNature,
                    EA_AttestingAuthority = a.EA_AttestingAuthority,
                    EA_AttestingAuthorityDesignation = a.EA_AttestingAuthorityDesignation,
                    EA_AttestmentDate = a.EA_AttestmentDate,
                    EA_ItemNumber = a.EA_ItemNumber,
                    EA_NonRenewalReason = a.EA_NonRenewalReason,
                    EA_PositionVacatedBy = a.EA_PositionVacatedBy,
                    UnitId = a.tblEmployees.Emp_Curr_Unit,
                    ProvinceId = a.tblEmployees.tblUnits.Unit_ProvDistrict,
                    RegionId = a.tblEmployees.tblUnits.tblCityMunicipality.tblProvinces.Region_Id,
                    EA_Emp_AccountNumber = a.tblEmployees.Emp_Number
                });

            if (!PageSecurity.HasAccess(PageArea.HRIS_EmployeeAppointment_CanViewAll))
            {
                if (PageSecurity.HasAccess(PageArea.HRIS_EmployeeAppointment_RestricttoRegion))
                {
                    employeeAppointments = employeeAppointments.Where(
                        a => a.RegionId == CurrentUser.RegionID);
                }

                if (PageSecurity.HasAccess(PageArea.HRIS_EmployeeAppointment_RestricttoProvince))
                {
                    employeeAppointments = employeeAppointments.Where(
                            a => a.ProvinceId == CurrentUser.ProvinceID);
                }

                if (PageSecurity.HasAccess(PageArea.HRIS_EmployeeAppointment_RestricttoStation))
                {
                    employeeAppointments = employeeAppointments.Where(a => a.UnitId == CurrentUser.EmployeeUnitId);
                }
            }

            var searchEmployeeAppointment = gridInfo.searchEmployeeAppointment;

            if (!string.IsNullOrEmpty(searchEmployeeAppointment.EA_Emp_AccountNumber))
                employeeAppointments =
                    employeeAppointments.Where(a => a.EA_Emp_AccountNumber.Contains(searchEmployeeAppointment.EA_Emp_AccountNumber));

            if (!string.IsNullOrEmpty(searchEmployeeAppointment.EA_AppointingAuthority))
                employeeAppointments =
                    employeeAppointments.Where(a => a.EA_AppointingAuthority.Contains(searchEmployeeAppointment.EA_AppointingAuthority));

            if (!string.IsNullOrEmpty(searchEmployeeAppointment.EA_AttestingAuthority))
                employeeAppointments =
                    employeeAppointments.Where(a => a.EA_AttestingAuthority.Contains(searchEmployeeAppointment.EA_AttestingAuthority));

            if (!string.IsNullOrEmpty(searchEmployeeAppointment.EA_ItemNumber))
                employeeAppointments =
                    employeeAppointments.Where(a => a.EA_ItemNumber.Contains(searchEmployeeAppointment.EA_ItemNumber));

            if (!string.IsNullOrEmpty(searchEmployeeAppointment.EA_PositionVacatedBy))
                employeeAppointments =
                    employeeAppointments.Where(a => a.EA_PositionVacatedBy.Contains(searchEmployeeAppointment.EA_PositionVacatedBy));

            if (searchEmployeeAppointment.EA_AppoitmentNature > 0)
                employeeAppointments = employeeAppointments.Where(a => a.EA_AppoitmentNature == searchEmployeeAppointment.EA_AppoitmentNature);

            if (searchEmployeeAppointment.EA_AppoitmentStatus > 0)
                employeeAppointments =
                    employeeAppointments.Where(a => a.EA_AppoitmentStatus == searchEmployeeAppointment.EA_AppoitmentStatus);

            if (searchEmployeeAppointment.EA_Emp_Id > 0)
                employeeAppointments =
                    employeeAppointments.Where(a => a.EA_Emp_Id == searchEmployeeAppointment.EA_Emp_Id);

            if (searchEmployeeAppointment.EA_Rank_Id > 0)
                employeeAppointments =
                    employeeAppointments.Where(a => a.EA_Rank_Id == searchEmployeeAppointment.EA_Rank_Id);

            if (searchEmployeeAppointment.EA_AppointmentDate_From.HasValue && searchEmployeeAppointment.EA_AppointmentDate_To.HasValue)
                employeeAppointments =
                    employeeAppointments.Where(a => a.EA_AppointmentDate >= searchEmployeeAppointment.EA_AppointmentDate_From &&
                                                    a.EA_AppointmentDate <= searchEmployeeAppointment.EA_AppointmentDate_To);
            
            gridInfo.recordsTotal = employeeAppointments.Select(a => a.EA_Emp_Id).Count();
            employeeAppointments = employeeAppointments.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
                .Skip(gridInfo.start)
                .Take(gridInfo.length);

            return new EmployeeAppointmentsListResult
            {
                EmployeeAppointmentsList = employeeAppointments.ToList(),
                DatatableInfo = gridInfo
            };
        }
        
        public EmployeeAppointmentsModel GetEmployeeAppointmentById(int appointmentId)
        {
            var ret = new EmployeeAppointmentsModel();

            var res = BFPContext.tblEmployeeAppointments.FirstOrDefault(a => a.EA_Id == appointmentId);

            Mapper.Map(res, ret);
            if (res != null) ret.EA_Emp_AccountNumber = res.tblEmployees.Emp_Number;

            return ret;
        }

        public void UpdateEmployeeAppointment(EmployeeAppointmentsModel model)
        {
            var entity = BFPContext.tblEmployeeAppointments.FirstOrDefault(a => a.EA_Id == model.EA_Id);

            if (entity == null) throw new Exception("Employee Appointment cannot be found!");

            model.EA_CreatedDate = entity.EA_CreatedDate;
            model.EA_Created_Emp_Id = entity.EA_Created_Emp_Id;
            model.EA_LastUpdateDate = entity.EA_LastUpdateDate;
            model.EA_LastUpdate_Emp_Id = entity.EA_LastUpdate_Emp_Id;
            Mapper.Map(model, entity);

            entity.EA_LastUpdate_Emp_Id = CurrentUser.EmployeeId;
            entity.EA_LastUpdateDate = DateTime.Now;

            BFPContext.Entry(entity).State = EntityState.Modified;
            BFPContext.SaveChanges();
        }

        public bool DeleteByID(int appointmentId)
        {
            var employeeAppointment = BFPContext.tblEmployeeAppointments
                .FirstOrDefault(a => a.EA_Id == appointmentId);

            if (employeeAppointment != null)
            {
                BFPContext.tblEmployeeAppointments.Remove(employeeAppointment);
                BFPContext.SaveChanges();
            }

            return true;
        }

        public EmployeeAppointmentDashboardCounter GetDashboardAppointmentCounter()
        {
            var today = (DateTime.Now).AddYears(-1).Date;
            var threeDays = (DateTime.Now.AddDays(3)).AddYears(-1).Date;
            var oneMonth = (DateTime.Now.AddMonths(1)).AddYears(-1).Date;


            var retToday =
                BFPContext.tblEmployeeAppointments.Count(
                    a =>
                        a.EA_AppoitmentStatus == (int) AppointmentStatuses.Temporary &&
                        DbFunctions.TruncateTime(a.EA_AppointmentDate) == today);
            var retThreeDays =
                BFPContext.tblEmployeeAppointments.Count(
                    a =>
                        a.EA_AppoitmentStatus == (int) AppointmentStatuses.Temporary &&
                        (a.EA_AppointmentDate >= today && a.EA_AppointmentDate <= threeDays));
            var retOneMonth =
                BFPContext.tblEmployeeAppointments.Count(
                    a =>
                        a.EA_AppoitmentStatus == (int) AppointmentStatuses.Temporary &&
                        (a.EA_AppointmentDate >= today && a.EA_AppointmentDate <= oneMonth));


            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
            {
                 retToday = BFPContext.tblEmployeeAppointments.Count(a => a.EA_AppoitmentStatus == (int)AppointmentStatuses.Temporary && DbFunctions.TruncateTime(a.EA_AppointmentDate) == today && a.tblEmployees.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID);
                 retThreeDays = BFPContext.tblEmployeeAppointments.Count(a => a.EA_AppoitmentStatus == (int)AppointmentStatuses.Temporary && a.tblEmployees.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID &&
                            (a.EA_AppointmentDate >= today && a.EA_AppointmentDate <= threeDays) );
                 retOneMonth =  BFPContext.tblEmployeeAppointments.Count( a => a.EA_AppoitmentStatus == (int)AppointmentStatuses.Temporary && a.tblEmployees.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID &&
                            (a.EA_AppointmentDate >= today && a.EA_AppointmentDate <= oneMonth));
            }
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
            {

                retToday = BFPContext.tblEmployeeAppointments.Count(a => a.EA_AppoitmentStatus == (int)AppointmentStatuses.Temporary && DbFunctions.TruncateTime(a.EA_AppointmentDate) == today && a.tblEmployees.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == CurrentUser.ProvinceID);
                retThreeDays = BFPContext.tblEmployeeAppointments.Count(a => a.EA_AppoitmentStatus == (int)AppointmentStatuses.Temporary && a.tblEmployees.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == CurrentUser.ProvinceID &&
                           (a.EA_AppointmentDate >= today && a.EA_AppointmentDate <= threeDays));
                retOneMonth = BFPContext.tblEmployeeAppointments.Count(a => a.EA_AppoitmentStatus == (int)AppointmentStatuses.Temporary && a.tblEmployees.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == CurrentUser.ProvinceID &&
                         (a.EA_AppointmentDate >= today && a.EA_AppointmentDate <= oneMonth));
            }
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
            {

                retToday = BFPContext.tblEmployeeAppointments.Count(a => a.EA_AppoitmentStatus == (int)AppointmentStatuses.Temporary && DbFunctions.TruncateTime(a.EA_AppointmentDate) == today && a.tblEmployees.Emp_Curr_Unit == CurrentUser.EmployeeUnitId);
                retThreeDays = BFPContext.tblEmployeeAppointments.Count(a => a.EA_AppoitmentStatus == (int)AppointmentStatuses.Temporary && a.tblEmployees.Emp_Curr_Unit == CurrentUser.EmployeeUnitId &&
                           (a.EA_AppointmentDate >= today && a.EA_AppointmentDate <= threeDays));
                retOneMonth = BFPContext.tblEmployeeAppointments.Count(a => a.EA_AppoitmentStatus == (int)AppointmentStatuses.Temporary && a.tblEmployees.Emp_Curr_Unit == CurrentUser.EmployeeUnitId &&
                         (a.EA_AppointmentDate >= today && a.EA_AppointmentDate <= oneMonth));
            }

            var model = new EmployeeAppointmentDashboardCounter
            {
                //Today = BFPContext.tblEmployeeAppointments.Count( a => a.EA_AppoitmentStatus == (int)AppointmentStatuses.Temporary && DbFunctions.TruncateTime(a.EA_AppointmentDate) == today),
                //ThreeDays = BFPContext.tblEmployeeAppointments.Count(a => a.EA_AppoitmentStatus == (int)AppointmentStatuses.Temporary && (a.EA_AppointmentDate >= today &&  a.EA_AppointmentDate <= threeDays)),
                //OneMonth = BFPContext.tblEmployeeAppointments.Count(a => a.EA_AppoitmentStatus == (int)AppointmentStatuses.Temporary  && (a.EA_AppointmentDate >= today && a.EA_AppointmentDate <= oneMonth))
                Today = retToday,
                ThreeDays = retThreeDays,
                OneMonth = retOneMonth
            };

            return model;
        }


        public void CreateMapping()
        {
            Mapper.CreateMap<tblEmployeeAppointments, EmployeeAppointmentsModel>().ReverseMap();
            Mapper.CreateMap<tblEmployeeAppointments, EmployeeAppointmentsModel>();
        }
    }
}