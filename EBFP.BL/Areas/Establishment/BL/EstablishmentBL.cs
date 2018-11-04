using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EBFP.BL.Helper;
using EBFP.DataAccess;
using EBFP.Helper;
using Queries.Core.Repositories;

namespace EBFP.BL.Establishment
{
    public class EstablishmentBL : Repository<tblEstablishments, EstablishmentModel>, IEstablishment
    {
        public EstablishmentBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public void SyncEstablishmentLocalToServer(List<EstablishmentModel> establishments)
        {
            foreach (var est in establishments)
            {
                var establishment = BFPContext.tblEstablishments
                    .FirstOrDefault(a => a.Ref_Est_Id == est.Ref_Est_Id && a.Est_Unit_Id == est.Est_Unit_Id);

                if (establishment == null)
                {
                    establishment = new tblEstablishments();
                    Mapper.Map(est, establishment);
                    BFPContext.tblEstablishments.Add(establishment);
                }
                else
                {
                    var estId = establishment.Est_Id;
                    Mapper.Map(est, establishment);
                    establishment.Est_Id = estId;
                }
            }

            BFPContext.SaveChanges();
        }

        //public List<EstablishmentModel> GetEstablishmentListByUnit(int unitId)
        //{
        //    var establishments = BFPContext.tblEstablishments.Where(a => a.Est_Unit_Id == unitId);

        //    return establishments.Project().To<EstablishmentModel>().ToList();
        //}
        public EstablishmentListResult GetEstablishmentListByUnit(GridInfo gridInfo, int unitId)
        {
            var retEmps = new List<EstablishmentModel>();

            var SearchTerms = gridInfo.searchEstablishmentModel;

            var establishments = BFPContext.tblEstablishments.Where(a => a.Est_Unit_Id == unitId).AsQueryable();

            //if (!isAdmin)
            //    establishments = BFPContext.tblEstablishments.Where(a => a.Est_Unit_Id == unitId);


            if (SearchTerms.EstablishmentName > 0)
                establishments =
                    establishments.Where(
                        a => a.Est_Id == SearchTerms.EstablishmentName);
            if (SearchTerms.EstablishmentStatus > 0)
                establishments =
                    establishments.Where(
                        a => a.Est_EstablishmentStatus == SearchTerms.EstablishmentStatus);

            if (!string.IsNullOrEmpty(SearchTerms.TradeName))
                establishments = establishments.Where(a => a.Est_BusinessTradeName.Contains(SearchTerms.TradeName));
            if (!string.IsNullOrEmpty(SearchTerms.OwnerName))
                establishments = establishments.Where(a => a.Est_OwnerName.Contains(SearchTerms.OwnerName));
            if (!string.IsNullOrEmpty(SearchTerms.BusinessPermit))
                establishments = establishments.Where(a => a.Est_BusinessPermitNumber.Contains(SearchTerms.BusinessPermit));
            if (!string.IsNullOrEmpty(SearchTerms.MPNumber))
                establishments = establishments.Where(a => a.Est_MP_Number.Contains(SearchTerms.MPNumber));


            gridInfo.recordsTotal = establishments.Select(a => a.Est_Id).Count();
            var establishmentResult = establishments.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
                .Skip(gridInfo.start)
                .Take(gridInfo.length)
                .ToList();

            foreach (var est in establishmentResult)
            {
                retEmps.Add(new EstablishmentModel
                {
                    Est_BusinessName = est.Est_BusinessName,
                    Est_BusinessTradeName = est.Est_BusinessTradeName,
                    Est_MP_Number = est.Est_MP_Number,
                    Est_BusinessPermitNumber = est.Est_BusinessPermitNumber,
                    Est_NatureOfBusiness = est.Est_NatureOfBusiness,
                    Est_OwnerName = est.Est_OwnerName,
                    Est_EstablishmentStatus = est.Est_EstablishmentStatus,
                    Est_Id = est.Est_Id,
                    Est_IsPEZA = est.Est_IsPEZA == null ? false : est.Est_IsPEZA
                });
            }

            return new EstablishmentListResult
            {
                EstablishmentList = retEmps.OrderBy(a=> a.Est_CreatedDate).ToList(),
                DatatableInfo = gridInfo
            };
        }
        public EstablishmentDetailModel GetEstablishmentDetails(int unitId)
        {
            var unit = BFPContext.tblUnits.FirstOrDefault(a => a.Unit_Id == unitId);
            if (unit != null)
            {
                var province =
                    BFPContext.tblProvinces.FirstOrDefault(
                        a => a.Province_Id == unit.tblCityMunicipality.Municipality_Province_Id);
                var region = BFPContext.tblRegions.FirstOrDefault(a => a.Reg_Id == province.Region_Id);

                if (region != null && province != null)
                    return new EstablishmentDetailModel
                    {
                        RegionName = region.Reg_Title,
                        ProvinceName = province.Province_Name,
                        MunicipalityName = unit.Unit_StationName
                    };
            }
            return new EstablishmentDetailModel();
        }

        //public EstablishmentListResult GetListResult(GridInfo gridInfo)
        //{
        //    var retEst = new List<EstablishmentModel>();

        //    var searchTerms = gridInfo.searchEstablishmentModel;
        //    var establishment = BFPContext.tblEstablishments.AsQueryable();

        //    if (!PageSecurity.HasAccess(PageArea.FPSS_Establishment_CanViewAll))
        //    {
        //        if (PageSecurity.HasAccess(PageArea.FPSS_Establishment_RestricttoRegion))
        //            establishment =
        //                establishment.Where(
        //                    a => a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID);

        //        if (PageSecurity.HasAccess(PageArea.FPSS_Establishment_RestricttoProvince))
        //            establishment =
        //                establishment.Where(
        //                    a => a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == CurrentUser.ProvinceID);

        //        if (PageSecurity.HasAccess(PageArea.FPSS_Establishment_RestricttoMunicipality))
        //            establishment =
        //                establishment.Where(
        //                    a => a.tblUnits.tblCityMunicipality.Municipality_Id == CurrentUser.MunicipalityID);

        //        if (PageSecurity.HasAccess(PageArea.FPSS_Establishment_RestricttoStation))
        //            establishment = establishment.Where(a => a.Est_Unit_Id == CurrentUser.EmployeeUnitId);
        //    }

        //    if (!string.IsNullOrEmpty(searchTerms.Est_BusinessName))
        //        establishment = establishment.Where(a => a.Est_BusinessName.Contains(searchTerms.Est_BusinessName));
        //    if (!string.IsNullOrEmpty(searchTerms.Est_BusinessTradeName))
        //        establishment =
        //            establishment.Where(a => a.Est_BusinessTradeName.Contains(searchTerms.Est_BusinessTradeName));
        //    if (!string.IsNullOrEmpty(searchTerms.Est_MP_Number))
        //        establishment = establishment.Where(a => a.Est_MP_Number.Contains(searchTerms.Est_MP_Number));
        //    if (!string.IsNullOrEmpty(searchTerms.Est_BusinessPermitNumber))
        //        establishment =
        //            establishment.Where(a => a.Est_BusinessPermitNumber.Contains(searchTerms.Est_BusinessPermitNumber));
        //    if (!string.IsNullOrEmpty(searchTerms.Est_NatureOfBusiness))
        //        establishment =
        //            establishment.Where(a => a.Est_NatureOfBusiness.Contains(searchTerms.Est_NatureOfBusiness));
        //    if (!string.IsNullOrEmpty(searchTerms.Est_OwnerName))
        //        establishment = establishment.Where(a => a.Est_OwnerName.Contains(searchTerms.Est_OwnerName));
        //    if (searchTerms.Est_EstablishmentStatus > 0)
        //        establishment =
        //            establishment.Where(a => a.Est_EstablishmentStatus == searchTerms.Est_EstablishmentStatus);
        //    if (searchTerms.RegionId > 0)
        //        establishment =
        //            establishment.Where(
        //                a => a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == searchTerms.RegionId);
        //    if (searchTerms.ProvinceId > 0)
        //        establishment =
        //            establishment.Where(
        //                a => a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == searchTerms.ProvinceId);
        //    if (searchTerms.MunicipalityId > 0)
        //        establishment =
        //            establishment.Where(
        //                a => a.tblUnits.tblCityMunicipality.Municipality_Id == searchTerms.MunicipalityId);
        //    if (searchTerms.Est_Unit_Id > 0)
        //        establishment = establishment.Where(a => a.Est_Unit_Id == searchTerms.Est_Unit_Id);


        //    var compliant = Functions.CompliantStatus();
        //    var nonCompliant = Functions.NonCompliantStatus();
        //    var closure = Functions.ClosureStatus();

        //    if (searchTerms.Est_DashboardType == (int)DashboardEstablihmentType.Compliant)
        //        establishment = establishment.Where(a => compliant.Contains(a.Est_EstablishmentStatus));

        //    if (searchTerms.Est_DashboardType == (int)DashboardEstablihmentType.NonCompliant)
        //        establishment = establishment.Where(a => nonCompliant.Contains(a.Est_EstablishmentStatus));

        //    if (searchTerms.Est_DashboardType == (int)DashboardEstablihmentType.Closure)
        //        establishment = establishment.Where(a => closure.Contains(a.Est_EstablishmentStatus));

        //    gridInfo.recordsTotal = establishment.Select(a => a.Est_Id).Count();

        //    var establishmentResult = new List<tblEstablishments>();

        //    if (gridInfo.sortColumnName == "Est_EstablishmentStatusName")
        //    {
        //        establishmentResult = establishment.OrderBy("Est_EstablishmentStatus" + " " + gridInfo.sortOrder)
        //            .Skip(gridInfo.start)
        //            .Take(gridInfo.length)
        //            .ToList();
        //    }
        //    else
        //    {
        //        establishmentResult = establishment.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
        //           .Skip(gridInfo.start)
        //           .Take(gridInfo.length)
        //           .ToList();
        //    }

        //    var estIds = establishmentResult.Select(a => a.Ref_Est_Id).Distinct();
        //    var inspectionOrder = BFPContext.tblInspectionOrders.Where(a => estIds.Contains(a.IO_Est_Id));
        //    var ioInspectionIds = inspectionOrder.Select(a => a.Ref_IO_Id);
        //    var inspectors = BFPContext.tblInspectionOrderInspectors.Where(a => ioInspectionIds.Contains(a.Insp_IO_Id));
        //    var ioInpectorsIds = inspectors.Select(a => a.Insp_Emp_Id).Distinct();
        //    var employees = BFPContext.tblEmployees.Where(a => ioInpectorsIds.Contains(a.Emp_Id)).ToList();
        //    foreach (var est in establishmentResult)
        //    {
        //        var model = new EstablishmentModel
        //        {
        //            Est_BusinessName = est.Est_BusinessName,
        //            Est_BusinessTradeName = est.Est_BusinessTradeName,
        //            Est_MP_Number = est.Est_MP_Number,
        //            Est_BusinessPermitNumber = est.Est_BusinessPermitNumber,
        //            Est_EstablishmentStatus = est.Est_EstablishmentStatus,
        //            Est_NatureOfBusiness = est.Est_NatureOfBusiness,
        //            Est_OwnerName = est.Est_OwnerName,
        //            Est_Id = est.Est_Id,
        //            Est_Unit_Name = est.tblUnits.Unit_StationName,
        //        };

        //        var io = inspectionOrder.Where(a => a.IO_Est_Id == est.Ref_Est_Id).OrderByDescending(a => a.IO_InspectionDate).FirstOrDefault();
        //        if (io != null)
        //        {
        //            model.Est_LatestIO = io.IO_Number;
        //            foreach(var inspector in inspectors.Where(a => a.Insp_IO_Id == io.Ref_IO_Id).Select(a => a.Insp_Emp_Id).Distinct())
        //            {
        //                var employee = employees.FirstOrDefault(a => a.Emp_Id == inspector);
        //                if (employee != null)
        //                {
        //                    model.Est_Inspector = model.Est_Inspector + employee.Emp_FirstName + " " + employee.Emp_LastName + ",";
        //                }
        //            }
        //        }
        //        model.Est_Inspector = model.Est_Inspector?.Remove(model.Est_Inspector.Length - 1);


        //        retEst.Add(model);
        //    }

        //    if (gridInfo.sortColumnName == "Est_EstablishmentStatusName")
        //    {
        //        retEst = gridInfo.sortOrder == "asc" ? 
        //            retEst.OrderBy(a => a.Est_EstablishmentStatusName).ToList() : 
        //            retEst.OrderByDescending(a => a.Est_EstablishmentStatusName).ToList();

        //        retEst = retEst.Skip(gridInfo.start)
        //            .Take(gridInfo.length)
        //            .ToList();
        //    }

        //    return new EstablishmentListResult
        //    {
        //        EstablishmentList = retEst,
        //        DatatableInfo = gridInfo
        //    };
        //}

        public EstablishmentModel GetEstablishmentById(int establishmentId)
        {
            var ret = new EstablishmentModel();

            var res = BFPContext.tblEstablishments.FirstOrDefault(a => a.Est_Id == establishmentId);

            if (res != null)
            {
                res.Est_IsPEZA = res.Est_IsPEZA == null ? false : res.Est_IsPEZA;
                //res.Est_IsPEZA = true;
                //res.Est_HazardType = 1;
                //res.Est_OccupancyType = 2;
                //res.Est_RegistrationStatus = 2;
                //res.Est_EstablishmentStatus = 2;
                Mapper.Map(res, ret);
            }
            else
            {
                throw new Exception("Establishment not found!");
            }
            return ret;
        }

        public bool DeleteDuplicateEstablishments(int unitId)
        {
            var estList = BFPContext.tblEstablishments
                          .Where(a => a.Est_Unit_Id == unitId)
                           .GroupBy(a => new { a.Est_MP_Number })
                        .Select(g => new
                        {
                            g.Key.Est_MP_Number,
                            Count = g.Count()
                        }).ToList();

            foreach (var est in estList.Where(a => a.Count > 1))
            {
                try
                {
                    var establishments = BFPContext.tblEstablishments.Where(a => a.Est_MP_Number == est.Est_MP_Number && a.Est_Unit_Id == unitId).ToList();

                    foreach (var establishment in establishments)
                    {
                        var count = BFPContext.tblEstablishments.Count(a => a.Est_MP_Number == establishment.Est_MP_Number && a.Est_Unit_Id == establishment.Est_Unit_Id);
                        if (count > 1)
                        {
                            var toDelete =
                                BFPContext.tblEstablishments.FirstOrDefault(a => a.Est_MP_Number == establishment.Est_MP_Number && a.Est_Id == establishment.Est_Id);
                            if (toDelete != null)
                            {
                                var FSIC = BFPContext.tblFSICApplication.Count(a => a.FSIC_Est_Id == toDelete.Ref_Est_Id);
                                var FSEC = BFPContext.tblFSECApplication.Count(a => a.FSEC_Est_Id == toDelete.Ref_Est_Id);
                                var OTHERFEES = BFPContext.tblOtherFees.Count(a => a.OF_Est_Id == toDelete.Ref_Est_Id);
                                var IO = BFPContext.tblInspectionOrders.Count(a => a.IO_Est_Id == toDelete.Ref_Est_Id);
                                var NTC = BFPContext.tblNoticeToComply.Count(a => a.NTC_Est_Id == toDelete.Ref_Est_Id);
                                var NTCV =
                                    BFPContext.tblNoticeToCorrectViolations.Count(a => a.NTCV_Est_Id == toDelete.Ref_Est_Id);

                                if (FSIC > 0 || FSEC > 0 || OTHERFEES > 0 || IO > 0 || NTC > 0 || NTCV > 0)
                                {
                                    //Establishment used in other tables
                                }
                                else
                                {
                                    BFPContext.tblEstablishments.Remove(toDelete);
                                    BFPContext.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    var s = ex;
                    continue;

                }
            }

            return true;
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblEstablishments, EstablishmentModel>().ReverseMap();
            Mapper.CreateMap<List<tblEstablishments>, List<EstablishmentModel>>().ReverseMap();
            Mapper.CreateMap<List<tblEstablishments>, List<EstablishmentModel>>();
        }


        /////

        public List<string> EstablishmentsName(int unitID)
        {
            var rets = base.BFPContext.tblEstablishments
                                        .Where(a => a.Est_Unit_Id == unitID)
                                        .OrderBy(a => a.Est_BusinessName)
                                        .Select(a => a.Est_BusinessName)
                                        .ToList();
            return rets;
        }

        public List<string> EstablishmentsMPNumber(int unitID)
        {
            var rets = base.BFPContext.tblEstablishments
                                        .Where(a => a.Est_Unit_Id == unitID)
                                        .OrderBy(a => a.Est_MP_Number)
                                        .Select(a => a.Est_MP_Number)
                                        .ToList();
            return rets;
        }

        public List<string> EstablishmentsOwnerName(int unitID)
        {
            var rets = base.BFPContext.tblEstablishments
                                        .Where(a => a.Est_Unit_Id == unitID)
                                        .OrderBy(a => a.Est_OwnerName)
                                        .Select(a => a.Est_OwnerName)
                                        .ToList();
            return rets;
        }

        public List<CalendarModel> GetCalendarDetails(DateTime selectedDate, int unitId)
        {
            var selectedMonth = selectedDate.Month;
            var selectedYear = selectedDate.AddYears(-1).Year;
            var model = new List<CalendarModel>();
            var establishment = (from a in BFPContext.tblEstablishments
                                 join b in BFPContext.tblFSICApplication on a.Ref_Est_Id equals b.FSIC_Est_Id.ToString()
                                 where (b.FSIC_Issue_Date.Value.Month == selectedMonth &&
                                       b.FSIC_Issue_Date.Value.Year == selectedYear) && a.Est_Unit_Id == unitId
                                 select new CalendarModel
                                 {
                                     Est_Id = a.Ref_Est_Id,
                                     BusinessAddress = a.Est_BusinessAddress,
                                     BusinessName = a.Est_BusinessName,
                                     NatureOfBusiness = a.Est_NatureOfBusiness,
                                     TradeName = a.Est_BusinessTradeName,
                                     MPNumber = a.Est_MP_Number,
                                     PermitNumber = a.Est_BusinessPermitNumber,
                                     Owner = a.Est_OwnerName,
                                     EstablishmentStatus = a.Est_EstablishmentStatus,
                                     Issue_Date = b.FSIC_Issue_Date
                                 }).ToList();

            Mapper.Map(establishment, model);

            return model;
        }

        public List<CalendarModel> GetExpiredFSIC(int unitId)
        {
            var fsic = BFPContext.tblFSICApplication
                   .Where(c => (c.FSIC_Est_Id != null || c.FSIC_Est_Id != "") && c.FSIC_Issue_Date.HasValue && c.FSIC_Unit_Id == unitId)
                   .GroupBy(c => c.FSIC_Est_Id)
                   .Select(grp => new CalendarModel
                   {
                       Est_Id = grp.OrderByDescending(x => x.FSIC_Issue_Date).FirstOrDefault().FSIC_Est_Id,
                       Issue_Date = grp.OrderByDescending(x => x.FSIC_Issue_Date).FirstOrDefault().FSIC_Issue_Date
                   }).ToList();

            var Ids = fsic.Select(b => b.Est_Id).ToList();
            var establishment = (from a in BFPContext.tblEstablishments
                                 where Ids.Contains(a.Ref_Est_Id) && a.Est_Unit_Id == unitId
                                 select new CalendarModel
                                 {
                                     BusinessAddress = a.Est_BusinessAddress,
                                     BusinessName = a.Est_BusinessName,
                                     Est_Id = a.Ref_Est_Id,
                                     NatureOfBusiness = a.Est_NatureOfBusiness,
                                     TradeName = a.Est_BusinessTradeName,
                                     MPNumber = a.Est_MP_Number,
                                     PermitNumber = a.Est_BusinessPermitNumber,
                                     Owner = a.Est_OwnerName,
                                     EstablishmentStatus = a.Est_EstablishmentStatus
                                 }).ToList();


            var date = DateTime.Now;
            var expired = (from a in establishment
                           join b in fsic on a.Est_Id equals b.Est_Id
                           where b.Issue_Date.HasValue && b.Issue_Date.Value.AddYears(1) < date
                           select new CalendarModel
                           {
                               BusinessAddress = a.BusinessAddress,
                               BusinessName = a.BusinessName,
                               Est_Id = a.Est_Id,
                               NatureOfBusiness = a.NatureOfBusiness,
                               TradeName = a.TradeName,
                               MPNumber = a.MPNumber,
                               PermitNumber = a.PermitNumber,
                               Owner = a.Owner,
                               EstablishmentStatus = a.EstablishmentStatus,
                               Issue_Date = b.Issue_Date
                           }).ToList();

            return expired;
        }

        public void UploadBPLOEstablishment(List<EstablishmentModel> model)
        {
            foreach (var est in model)
            {
                var establishment = BFPContext.tblEstablishments
                                            .FirstOrDefault(a => a.Est_MP_Number == est.Est_MP_Number && a.Est_Unit_Id == est.Est_Unit_Id);

                if (establishment == null)
                {
                    establishment = new tblEstablishments();
                    Mapper.Map(est, establishment);
                    BFPContext.tblEstablishments.Add(establishment);
                }
                else
                {
                    //est.Auto_Est_Id = establishment.Auto_Est_Id;
                    //est.Est_Id = establishment.Est_Id;
                    //Mapper.Map(est, establishment);
                    establishment.Est_BusinessName = est.Est_BusinessName;
                    establishment.Est_BusinessAddress = est.Est_BusinessAddress;
                    establishment.Est_OwnerName = est.Est_OwnerName;
                    establishment.Est_LastUpdateDate = DateTime.Now;
                    establishment.Est_LastUpdate_Emp_Id = est.Est_LastUpdate_Emp_Id;
                    establishment.Est_EstablishmentStatus = est.Est_EstablishmentStatus;
                    establishment.Est_ExpiryDate = est.Est_ExpiryDate;
                }

                establishment.IsSynced = false;
            }

            BFPContext.SaveChanges();
        }
        public void DeleteDuplicates(int unitId)
        {
            var estList = BFPContext.tblEstablishments
                          .Where(a => a.Est_Unit_Id == unitId)
                           .GroupBy(a => new { a.Est_MP_Number })
                        .Select(g => new
                        {
                            g.Key.Est_MP_Number,
                            Count = g.Count()
                        }).ToList();

            foreach (var est in estList.Where(a => a.Count > 1))
            {
                try
                {
                    var establishments = BFPContext.tblEstablishments.Where(a => a.Est_MP_Number == est.Est_MP_Number && a.Est_Unit_Id == unitId).ToList();

                    foreach (var establishment in establishments)
                    {
                        var count = BFPContext.tblEstablishments.Count(a => a.Est_MP_Number == establishment.Est_MP_Number && a.Est_Unit_Id == establishment.Est_Unit_Id);
                        if (count > 1)
                        {
                            var toDelete =
                                BFPContext.tblEstablishments.FirstOrDefault(a => a.Est_MP_Number == establishment.Est_MP_Number && a.Est_Id == establishment.Est_Id);
                            if (toDelete != null)
                            {
                                var FSIC = BFPContext.tblFSICApplication.Count(a => a.FSIC_Est_Id == toDelete.Ref_Est_Id);
                                var FSEC = BFPContext.tblFSECApplication.Count(a => a.FSEC_Est_Id == toDelete.Ref_Est_Id);
                                var OTHERFEES = BFPContext.tblOtherFees.Count(a => a.OF_Est_Id == toDelete.Ref_Est_Id);
                                var IO = BFPContext.tblInspectionOrders.Count(a => a.IO_Est_Id == toDelete.Ref_Est_Id);
                                var NTC = BFPContext.tblNoticeToComply.Count(a => a.NTC_Est_Id == toDelete.Ref_Est_Id);
                                var NTCV =
                                    BFPContext.tblNoticeToCorrectViolations.Count(a => a.NTCV_Est_Id == toDelete.Ref_Est_Id);

                                if (FSIC > 0 || FSEC > 0 || OTHERFEES > 0 || IO > 0 || NTC > 0 || NTCV > 0)
                                {
                                    //Establishment used in other tables
                                }
                                else
                                {
                                    BFPContext.tblEstablishments.Remove(toDelete);
                                    BFPContext.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    var s = ex;
                    continue;

                }
            }
        }

        public void UpdateExpiryDate(int unitId, string estId)
        {
            ////var xx = DateTime.Now.AddYears(1);
            var month = DateTime.Now.Month;
            var day = DateTime.Now.Day;
            var establishment = BFPContext.tblEstablishments.FirstOrDefault(a => a.Est_Unit_Id == unitId && a.Ref_Est_Id == estId);

            if (establishment != null)
            {
                if (establishment.Est_ExpiryDate == null)
                    establishment.Est_ExpiryDate = DateTime.Now.AddYears(1);
                else
                {
                    var month2 = establishment.Est_ExpiryDate.Value.Month;
                    var day2 = establishment.Est_ExpiryDate.Value.Day;
                    if (month >= month2 && day > day2)
                    {
                        establishment.Est_ExpiryDate = DateTime.Now.AddYears(1);
                    }
                }
            }

            BFPContext.SaveChanges();
        }

        public void UpdateRegistrationStatus(int unitId, string estId, int regStat)
        {
            var establishment = BFPContext.tblEstablishments.FirstOrDefault(a => a.Est_Unit_Id == unitId && a.Ref_Est_Id == estId);

            if (establishment != null)
            {
                establishment.Est_RegistrationStatus = regStat;
            }

            BFPContext.SaveChanges();
        }

        public long GetEstCompliantCount(int unitId)
        {
            return BFPContext.tblEstablishments.Count(a => a.Est_EstablishmentStatus < (int)EstablishmentStatus.For_Issuance_of_NTC);
        }

        public long GetEstNonCompliantCount(int unitId)
        {
            return BFPContext.tblEstablishments.Count(a => a.Est_EstablishmentStatus >= (int)EstablishmentStatus.For_Issuance_of_NTC);
        }

        public long GetEstHazardHighCount(int unitId)
        {
            return BFPContext.tblEstablishments.Count(a => a.Est_HazardType == (int)HazardType.High);
        }
        public long GetEstHazardModLowCount(int unitId)
        {
            return BFPContext.tblEstablishments.Count(a => a.Est_HazardType == (int)HazardType.Moderate_Low);
        }

        public bool UpdateEstablishment(EstablishmentModel model)
        {
            var establishment = BFPContext.tblEstablishments.FirstOrDefault(a => a.Est_Id == model.Est_Id);

            if (establishment != null)
            {
                model.Est_Unit_Id = establishment.Est_Unit_Id;
                model.Est_CreatedDate = establishment.Est_CreatedDate;
                model.Est_Created_Emp_Id = establishment.Est_Created_Emp_Id;
                Mapper.Map(model, establishment);

                BFPContext.SaveChanges();

                return true;
            }

            return false;
        }

        public int GetEstablishmentIdByRefId(string ref_est_id)
        {
            var est_id = BFPContext.tblEstablishments.FirstOrDefault(a => a.Ref_Est_Id == ref_est_id).Est_Id;

            return est_id;
        }
    }
}