using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EBFP.BL.CIS;
using EBFP.BL.Helper;
using EBFP.DataAccess;
using EBFP.Helper;
using EBFP.BL.Administration;

namespace EBFP.BL.HumanResources
{
    public class ReportBL : EntityFrameworkBase, IReport
    {
        public ReportBL(EBFPEntities _context)
        {
            context_ = _context;
        }

        //public List<FireCodeFeesCollectionModel> GetFireCodeFeesCollection(int month)
        //{
        //    var year = DateTime.Now.Year;

        //    var feeList = (from fee in context.vwFees
        //        where DbFunctions.TruncateTime(fee.ApplicationDate).Value.Month == month &&
        //              DbFunctions.TruncateTime(fee.ApplicationDate).Value.Year == year
        //        select new FireCodeFeesCollectionModel
        //        {
        //            Region = fee.Region,
        //            ConstructionTax = fee.ConstructionTax,
        //            RealtyTax = fee.RealtyTax,
        //            PremiumTax = fee.PremiumTax,
        //            SalesTax = fee.SalesTax,
        //            ProceedsTax = fee.ProceedsTax,
        //            InstallationClearanceFee = fee.InstallationClearanceFee,
        //            FireCodeAdminFine = fee.FireCodeAdminFine,
        //            OtherFee = fee.OtherFee,
        //            ApplicationType = fee.ApplicationType.Value,
        //            FireSafetyInspectionFee = fee.FireSafetyInspectionFee
        //        }).ToList();


        //    var regions = feeList.Select(a => a.Region).Distinct().ToList();
        //    var fees = new List<FireCodeFeesCollectionModel>();
        //    foreach (var reg in regions)
        //    {
        //        var feebyRegion = feeList.Where(a => a.Region == reg).ToList();
        //        var fee = new FireCodeFeesCollectionModel();
        //        fee.Region = reg;
        //        fee.ConstructionTax = feebyRegion.Sum(a => a.ConstructionTax);
        //        fee.FireCodeAdminFine = feebyRegion.Sum(a => a.FireCodeAdminFine);
        //        fee.PremiumTax = feebyRegion.Sum(a => a.PremiumTax);
        //        fee.RealtyTax = feebyRegion.Sum(a => a.RealtyTax);
        //        fee.ProceedsTax = feebyRegion.Sum(a => a.ProceedsTax);
        //        fee.SalesTax = feebyRegion.Sum(a => a.SalesTax);
        //        fee.OtherFee = feebyRegion.Sum(a => a.OtherFee);

        //        fee.ForOccupancyFee = feebyRegion.Where(a => a.ApplicationType == 0)
        //            .Sum(a => a.FireSafetyInspectionFee);
        //        fee.ForBusinessOtherFee = feebyRegion.Where(a => a.ApplicationType != 0)
        //            .Sum(a => a.FireSafetyInspectionFee);
        //        fee.Total = fee.ConstructionTax + fee.RealtyTax + fee.PremiumTax +
        //                    +fee.SalesTax + fee.ProceedsTax + fee.ForOccupancyFee + fee.ForBusinessOtherFee +
        //                    fee.OtherFee + fee.FireCodeAdminFine;

        //        fees.Add(fee);
        //    }

        //    return fees;
        //}

        public List<FireCodeFeesCollectionModel> GetFireCodeFeesCollection(int month)
        {
            var year = DateTime.Now.Year;

            var feeList = (from fee in context.vwFees
                           where DbFunctions.TruncateTime(fee.ApplicationDate).Value.Month == month &&
                                 DbFunctions.TruncateTime(fee.ApplicationDate).Value.Year == year
                           select new FireCodeFeesCollectionModel
                           {
                               Region = fee.Region,
                               ConstructionTax = fee.ConstructionTax,
                               RealtyTax = fee.RealtyTax,
                               PremiumTax = fee.PremiumTax,
                               SalesTax = fee.SalesTax,
                               ProceedsTax = fee.ProceedsTax,
                               InstallationClearanceFee = fee.InstallationClearanceFee,
                               FireCodeAdminFine = fee.FireCodeAdminFine,
                               OtherFee = fee.OtherFee,
                               ApplicationType = fee.ApplicationType.Value,
                               FireSafetyInspectionFee = fee.FireSafetyInspectionFee,

                               RegionId = fee.RegionId,
                               ProvinceId = fee.ProvinceId,
                               UnitId = fee.UnitId

                           }).ToList();

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
            {
                var fees = new List<FireCodeFeesCollectionModel>();
                var provinces = context.tblProvinces.Where(a => a.Region_Id == CurrentUser.RegionID);
                foreach (var prov in provinces)
                {
                    var feeByProvince = feeList.Where(a => a.ProvinceId == prov.Province_Id).ToList();
                    var fee = new FireCodeFeesCollectionModel();
                    fee.Region = prov.Province_Name;
                    fee.ConstructionTax = feeByProvince.Sum(a => a.ConstructionTax);
                    fee.FireCodeAdminFine = feeByProvince.Sum(a => a.FireCodeAdminFine);
                    fee.PremiumTax = feeByProvince.Sum(a => a.PremiumTax);
                    fee.RealtyTax = feeByProvince.Sum(a => a.RealtyTax);
                    fee.ProceedsTax = feeByProvince.Sum(a => a.ProceedsTax);
                    fee.SalesTax = feeByProvince.Sum(a => a.SalesTax);
                    fee.OtherFee = feeByProvince.Sum(a => a.OtherFee);

                    fee.ForOccupancyFee = feeByProvince.Where(a => a.ApplicationType == 0)
                        .Sum(a => a.FireSafetyInspectionFee);
                    fee.ForBusinessOtherFee = feeByProvince.Where(a => a.ApplicationType != 0)
                        .Sum(a => a.FireSafetyInspectionFee);
                    fee.Total = fee.ConstructionTax + fee.RealtyTax + fee.PremiumTax +
                                +fee.SalesTax + fee.ProceedsTax + fee.ForOccupancyFee + fee.ForBusinessOtherFee +
                                fee.OtherFee + fee.FireCodeAdminFine;

                    fees.Add(fee);
                }

                return fees;
            }

            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
            {
                var fees = new List<FireCodeFeesCollectionModel>();
                var units = context.tblUnits.Where(a => a.Unit_ProvDistrict == CurrentUser.ProvinceID);
                foreach (var unit in units)
                {
                    var feeByStation = feeList.Where(a => a.UnitId == unit.Unit_Id).ToList();
                    var fee = new FireCodeFeesCollectionModel();
                    fee.Region = unit.Unit_StationName;
                    fee.ConstructionTax = feeByStation.Sum(a => a.ConstructionTax);
                    fee.FireCodeAdminFine = feeByStation.Sum(a => a.FireCodeAdminFine);
                    fee.PremiumTax = feeByStation.Sum(a => a.PremiumTax);
                    fee.RealtyTax = feeByStation.Sum(a => a.RealtyTax);
                    fee.ProceedsTax = feeByStation.Sum(a => a.ProceedsTax);
                    fee.SalesTax = feeByStation.Sum(a => a.SalesTax);
                    fee.OtherFee = feeByStation.Sum(a => a.OtherFee);

                    fee.ForOccupancyFee = feeByStation.Where(a => a.ApplicationType == 0)
                        .Sum(a => a.FireSafetyInspectionFee);
                    fee.ForBusinessOtherFee = feeByStation.Where(a => a.ApplicationType != 0)
                        .Sum(a => a.FireSafetyInspectionFee);
                    fee.Total = fee.ConstructionTax + fee.RealtyTax + fee.PremiumTax +
                                +fee.SalesTax + fee.ProceedsTax + fee.ForOccupancyFee + fee.ForBusinessOtherFee +
                                fee.OtherFee + fee.FireCodeAdminFine;

                    fees.Add(fee);
                }

                return fees;
            }
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
            {
                var fees = new List<FireCodeFeesCollectionModel>();
                var units = context.tblUnits.FirstOrDefault(a => a.Unit_Id == CurrentUser.EmployeeUnitId);

                var feeByStation = feeList.Where(a => a.UnitId == units.Unit_Id).ToList();
                var fee = new FireCodeFeesCollectionModel();
                fee.Region = units.Unit_StationName;
                fee.ConstructionTax = feeByStation.Sum(a => a.ConstructionTax);
                fee.FireCodeAdminFine = feeByStation.Sum(a => a.FireCodeAdminFine);
                fee.PremiumTax = feeByStation.Sum(a => a.PremiumTax);
                fee.RealtyTax = feeByStation.Sum(a => a.RealtyTax);
                fee.ProceedsTax = feeByStation.Sum(a => a.ProceedsTax);
                fee.SalesTax = feeByStation.Sum(a => a.SalesTax);
                fee.OtherFee = feeByStation.Sum(a => a.OtherFee);

                fee.ForOccupancyFee = feeByStation.Where(a => a.ApplicationType == 0)
                    .Sum(a => a.FireSafetyInspectionFee);
                fee.ForBusinessOtherFee = feeByStation.Where(a => a.ApplicationType != 0)
                    .Sum(a => a.FireSafetyInspectionFee);
                fee.Total = fee.ConstructionTax + fee.RealtyTax + fee.PremiumTax +
                            +fee.SalesTax + fee.ProceedsTax + fee.ForOccupancyFee + fee.ForBusinessOtherFee +
                            fee.OtherFee + fee.FireCodeAdminFine;

                fees.Add(fee);

                return fees;
            }
            else
            {
                var regions = feeList.Select(a => a.Region).Distinct().ToList();
                var fees = new List<FireCodeFeesCollectionModel>();
                foreach (var reg in regions)
                {
                    var feebyRegion = feeList.Where(a => a.Region == reg).ToList();
                    var fee = new FireCodeFeesCollectionModel();
                    fee.Region = reg;
                    fee.ConstructionTax = feebyRegion.Sum(a => a.ConstructionTax);
                    fee.FireCodeAdminFine = feebyRegion.Sum(a => a.FireCodeAdminFine);
                    fee.PremiumTax = feebyRegion.Sum(a => a.PremiumTax);
                    fee.RealtyTax = feebyRegion.Sum(a => a.RealtyTax);
                    fee.ProceedsTax = feebyRegion.Sum(a => a.ProceedsTax);
                    fee.SalesTax = feebyRegion.Sum(a => a.SalesTax);
                    fee.OtherFee = feebyRegion.Sum(a => a.OtherFee);

                    fee.ForOccupancyFee = feebyRegion.Where(a => a.ApplicationType == 0)
                        .Sum(a => a.FireSafetyInspectionFee);
                    fee.ForBusinessOtherFee = feebyRegion.Where(a => a.ApplicationType != 0)
                        .Sum(a => a.FireSafetyInspectionFee);
                    fee.Total = fee.ConstructionTax + fee.RealtyTax + fee.PremiumTax +
                                +fee.SalesTax + fee.ProceedsTax + fee.ForOccupancyFee + fee.ForBusinessOtherFee +
                                fee.OtherFee + fee.FireCodeAdminFine;

                    fees.Add(fee);
                }

                return fees;
            }
        }

        public List<FireCodeDepositCollectionModel> GetFireCodeCollections(int month)
        {
            var year = DateTime.Now.Year;
            var collectionList = new List<FireCodeDepositCollectionModel>();
            var collections = (
                      from appPayment in context.tblApplicationPayments
                      join unit in context.tblUnits on appPayment.AP_Unit_Id equals unit.Unit_Id
                      join muni in context.tblCityMunicipality on unit.Unit_Municipality_Id equals muni.Municipality_Id
                      join prov in context.tblProvinces on muni.Municipality_Province_Id equals prov.Province_Id
                      join reg in context.tblRegions on prov.Region_Id equals reg.Reg_Id
                      where appPayment.AP_ORDate.Month == month && appPayment.AP_ORDate.Year == year
                      select new FireCodeDepositCollectionModel
                      {
                          CollectionDate = appPayment.AP_ORDate,
                          CollectionAmount = appPayment.AP_ORAmount,
                          ORNumber = appPayment.AP_ORNumber,
                          RegionId = reg.Reg_Id,
                          ProvinceId = prov.Province_Id,
                          UnitId = appPayment.AP_Unit_Id
                      });

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
            {
                collectionList = collections.Where(a => a.RegionId == CurrentUser.RegionID).ToList();
            }
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
            {
                collectionList = collections.Where(a => a.ProvinceId == CurrentUser.ProvinceID).ToList();
            }
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
            {
                collectionList = collections.Where(a => a.UnitId == CurrentUser.EmployeeUnitId).ToList();
            }
            else
                collectionList = collections.ToList();

            return collectionList;
        }

        public List<FireCodeDepositCollectionModel> GetFireCodeDeposits(int month)
        {
            var year = DateTime.Now.Year;
            var depositLits = new List<FireCodeDepositCollectionModel>();
            var deposits = (
                     from dep in context.tblUnitDeposits
                     join unit in context.tblUnits on dep.Dep_Unit_Id equals unit.Unit_Id
                     join muni in context.tblCityMunicipality on unit.Unit_Municipality_Id equals muni.Municipality_Id
                     join prov in context.tblProvinces on muni.Municipality_Province_Id equals prov.Province_Id
                     join reg in context.tblRegions on prov.Region_Id equals reg.Reg_Id
                     where dep.Dep_DepositDate.Month == month && dep.Dep_DepositDate.Year == year
                     select new FireCodeDepositCollectionModel
                     {
                         DepositDate = dep.Dep_DepositDate,
                         DepositAmount = dep.Dep_Amount,
                         LCNumber = dep.Dep_LC_No,
                         RegionId = reg.Reg_Id,
                         ProvinceId = prov.Province_Id,
                         UnitId = dep.Dep_Unit_Id
                     }).ToList();

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
            {
                depositLits = deposits.Where(a => a.RegionId == CurrentUser.RegionID).ToList();
            }
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
            {
                depositLits = deposits.Where(a => a.ProvinceId == CurrentUser.ProvinceID).ToList();
            }
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
            {
                depositLits = deposits.Where(a => a.UnitId == CurrentUser.EmployeeUnitId).ToList();
            }
            else
                depositLits = deposits.ToList();

            return depositLits;
        }

        public List<PersonnelModel> GetPersonnelPerRegion()
        {
            var personnelList = new List<PersonnelModel>();
            var regions = context.tblRegions.ToList();
            var emp = context.tblEmployees.Where(a => a.Emp_Curr_Rank != null && a.Emp_Curr_Unit != null &&
                                                      a.Emp_DutyStatus == (int)DutyStatuses.Active);

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
                emp = emp.Where(a => a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID);
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
                emp = emp.Where(a => a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == CurrentUser.ProvinceID);
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                emp = emp.Where(a => a.Emp_Curr_Unit == CurrentUser.EmployeeUnitId);

            foreach (var reg in regions.OrderBy(a => a.Reg_Id))
            {
                var personnelModel = new PersonnelModel();
                var byRegion = emp.Where(a => a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == reg.Reg_Id);

                personnelModel.RegId = reg.Reg_Id;
                personnelModel.Region = reg.Reg_Title;
                personnelModel.NUP = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.NUP);
                personnelModel.FO1 = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.FO1);
                personnelModel.FO2 = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.FO2);
                personnelModel.FO3 = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.FO3);
                personnelModel.SFO1 = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.SFO1);
                personnelModel.SFO2 = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.SFO2);
                personnelModel.SFO3 = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.SFO3);
                personnelModel.SFO4 = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.SFO4);
                personnelModel.INSP = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.INSP);
                personnelModel.SINSP = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.SINSP);
                personnelModel.CINSP = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.CINSP);
                personnelModel.SUPT = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.SUPT);
                personnelModel.SSUPT = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.SSUPT);
                personnelModel.CSUPT = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.CSUPT);
                personnelModel.DIR = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.DIR);
                personnelModel.GeneralAdmin = byRegion.Count(a => a.Emp_Curr_JobFunc == (int)JobFunction.GeneralAdmin);
                personnelModel.Operations = byRegion.Count(a => a.Emp_Curr_JobFunc != (int)JobFunction.GeneralAdmin && a.Emp_Curr_JobFunc != null);

                personnelList.Add(personnelModel);
            }

            return personnelList;
        }

        public List<LongevityPayModel> GetLongevityPay(DateTime startDate, DateTime endDate, int year)
        {
            startDate = startDate.AddYears(-year);
            endDate = endDate.AddYears(-year);
            var longevityPay = (
                from employee in context.tblEmployees
                from rank in context.tblRanks.Where(x => employee.Emp_Curr_Rank == x.Rank_Id)
                    .DefaultIfEmpty()
                where employee.Emp_Service_GovtStartDate >= startDate.Date &&
                      employee.Emp_Service_GovtStartDate <= endDate.Date &&
                      employee.Emp_DutyStatus == (int)DutyStatuses.Active
                select new LongevityPayModel
                {
                    UnitName = employee.tblUnits.Unit_StationName + ", " +
                               employee.tblUnits.tblCityMunicipality.tblProvinces.Province_Name,
                    Rank = rank.Rank_Name,
                    FullName = employee.Emp_FirstName + " " + employee.Emp_MiddleName + " " + employee.Emp_LastName +
                               " " + employee.Emp_SuffixName,
                    EffectiveDate = employee.Emp_Service_GovtStartDate.Value
                }).ToList();


            var filtered = longevityPay
                .Select(cl => new LongevityPayModel
                {
                    UnitName = cl.UnitName,
                    Rank = cl.Rank,
                    FullName = cl.FullName,
                    EffectiveDate = cl.EffectiveDate,
                    Batch = year == (int)LongevityYear.FiveYears
                        ? "1st"
                        : year == (int)LongevityYear.TenYears
                            ? "2nd"
                            : year == (int)LongevityYear.FifteenYears
                                ? "3rd"
                                : year == (int)LongevityYear.TwentyYears
                                    ? "4th"
                                    : "5th"
                }).OrderBy(a => a.EffectiveDate).ToList();

            return filtered;
        }

        public CommutationLeaveCreditsModel GetCommLeaveCredits(int retiredEmpId)
        {
            IHRISUnitOfWork unitOfWork = new HRISUnitOfWork();

            var commutationLeave = (
                from employee in context.tblEmployees
                from rank in context.tblRanks.Where(x => employee.Emp_Curr_Rank == x.Rank_Id)
                    .DefaultIfEmpty()
                where employee.Emp_Id == retiredEmpId
                select new CommutationLeaveCreditsModel
                {
                    UnitName = employee.tblUnits.Unit_StationName + ", " +
                               employee.tblUnits.tblCityMunicipality.tblProvinces.Province_Name,
                    Rank = rank.Rank_Name,
                    FirstName = employee.Emp_FirstName,
                    MiddleName = employee.Emp_MiddleName,
                    LastName = employee.Emp_LastName,
                    SuffixName = employee.Emp_SuffixName,
                    StartDate = employee.Emp_Service_StartDate.Value,
                    DateRetired = employee.Emp_Retired_Date.Value
                }).FirstOrDefault();

            commutationLeave.FullName = commutationLeave.FirstName + " " + commutationLeave.MiddleName[0] + " " +
                                        commutationLeave.LastName +
                                        " " + commutationLeave.SuffixName;

            var serviceRecord = context.tblEmployeeServiceAppointments.Where(a => a.ESA_Emp_Id == retiredEmpId && a.ESA_DutyStatus == (int)DutyStatuses.AWOL).ToList();

            double totalAwol = 0;
            if (serviceRecord.Count > 0)
            {
                foreach (var item in serviceRecord)
                {
                    if (item.ESA_EndDate != null)
                    {
                        var awol = unitOfWork.Leave.CalculateTotalAccruedLeave(item.ESA_ApptDate, item.ESA_EndDate.Value);
                        totalAwol = (totalAwol + awol);
                    }

                }

            }

            commutationLeave.Earned =
                (unitOfWork.Leave.CalculateTotalAccruedLeave(commutationLeave.StartDate, commutationLeave.DateRetired) - totalAwol);
            commutationLeave.EnjoyedLeave =
                unitOfWork.Leave.GetLeaveCredits(retiredEmpId, (int)LeaveParticular.Vacation);
            commutationLeave.EnjoyedSickLeave =
                unitOfWork.Leave.GetLeaveCredits(retiredEmpId, (int)LeaveParticular.Sick);

            return commutationLeave;
        }

        public List<AgeProfileModel> GetAgeProfile()
        {
            var ranks = (
                from rank in context.tblRanks
                from employee in context.tblEmployees
                    .Where(x => rank.Rank_Id == x.Emp_Curr_Rank && x.Emp_Curr_Unit != null &&
                                x.Emp_DutyStatus == (int)DutyStatuses.Active && rank.Rank_Name != "NUP")
                    .DefaultIfEmpty()
                select new AgeProfileModel
                {
                    BirthDate = employee.Emp_BirthDate,
                    RankId = rank.Rank_Id,
                    RankName = rank.Rank_Name
                }).ToList();

            var ageProfileModel = new List<AgeProfileModel>();
            foreach (var rank in context.tblRanks.ToList().OrderByDescending(a => a.Rank_Id))
            {
                var model = new AgeProfileModel();
                model.RankId = rank.Rank_Id;
                model.RankName = rank.Rank_Name;
                model.TwentyOneTwentyFive = GetAgePerRank(rank.Rank_Id, 21, 25, ranks);
                model.TwentySixThirty = GetAgePerRank(rank.Rank_Id, 26, 30, ranks);
                model.ThirtyOneThirtyFive = GetAgePerRank(rank.Rank_Id, 31, 35, ranks);
                model.ThirtySixFourty = GetAgePerRank(rank.Rank_Id, 36, 40, ranks);
                model.FourtyOneFourtyFive = GetAgePerRank(rank.Rank_Id, 41, 45, ranks);
                model.FourtySixFifty = GetAgePerRank(rank.Rank_Id, 46, 50, ranks);
                model.FiftyOneFiftyFive = GetAgePerRank(rank.Rank_Id, 51, 55, ranks);

                ageProfileModel.Add(model);
            }

            return ageProfileModel;
        }

        public int GetAgePerRank(int rank, int startAge, int endAge, List<AgeProfileModel> list)
        {
            return list.Count(a => a.RankId == rank && a.Age >= startAge && a.Age <= endAge);
        }

        public List<RatioOfFemaleFireFightersModel> GetRatioOfFemaleFireFighters()
        {
            var ratioOfFemaleFireFighters = new List<RatioOfFemaleFireFightersModel>();
            var regions = context.tblRegions.ToList();
            var emp = context.tblEmployees.Where(a => a.Emp_Curr_Rank != null && a.Emp_Curr_Unit != null &&
                                                      a.Emp_DutyStatus == (int)DutyStatuses.Active);

            foreach (var reg in regions.OrderBy(a => a.Reg_Id))
            {
                var ratioOfFemaleModel = new RatioOfFemaleFireFightersModel();
                var byRegion = emp.Where(a => a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == reg.Reg_Id);

                ratioOfFemaleModel.RegId = reg.Reg_Id;
                ratioOfFemaleModel.Region = reg.Reg_Title;
                ratioOfFemaleModel.NUP_Male = byRegion.Count(a => a.Emp_Gender == "M" && a.Emp_Curr_Rank == (int)Rank.NUP);
                ratioOfFemaleModel.NUP_Female = byRegion.Count(a => a.Emp_Gender == "F" && a.Emp_Curr_Rank == (int)Rank.NUP);
                ratioOfFemaleModel.FO1_Male = byRegion.Count(a => a.Emp_Gender == "M" && a.Emp_Curr_Rank == (int)Rank.FO1);
                ratioOfFemaleModel.FO1_Female = byRegion.Count(a => a.Emp_Gender == "F" && a.Emp_Curr_Rank == (int)Rank.FO1);
                ratioOfFemaleModel.FO2_Male = byRegion.Count(a => a.Emp_Gender == "M" && a.Emp_Curr_Rank == (int)Rank.FO2);
                ratioOfFemaleModel.FO2_Female = byRegion.Count(a => a.Emp_Gender == "F" && a.Emp_Curr_Rank == (int)Rank.FO2);
                ratioOfFemaleModel.FO3_Male = byRegion.Count(a => a.Emp_Gender == "M" && a.Emp_Curr_Rank == (int)Rank.FO3);
                ratioOfFemaleModel.FO3_Female = byRegion.Count(a => a.Emp_Gender == "F" && a.Emp_Curr_Rank == (int)Rank.FO3);
                ratioOfFemaleModel.SFO1_Male = byRegion.Count(a => a.Emp_Gender == "M" && a.Emp_Curr_Rank == (int)Rank.SFO1);
                ratioOfFemaleModel.SFO1_Female = byRegion.Count(a => a.Emp_Gender == "F" && a.Emp_Curr_Rank == (int)Rank.SFO1);
                ratioOfFemaleModel.SFO2_Male = byRegion.Count(a => a.Emp_Gender == "M" && a.Emp_Curr_Rank == (int)Rank.SFO2);
                ratioOfFemaleModel.SFO2_Female = byRegion.Count(a => a.Emp_Gender == "F" && a.Emp_Curr_Rank == (int)Rank.SFO2);
                ratioOfFemaleModel.SFO3_Male = byRegion.Count(a => a.Emp_Gender == "M" && a.Emp_Curr_Rank == (int)Rank.SFO3);
                ratioOfFemaleModel.SFO3_Female = byRegion.Count(a => a.Emp_Gender == "F" && a.Emp_Curr_Rank == (int)Rank.SFO3);
                ratioOfFemaleModel.SFO4_Male = byRegion.Count(a => a.Emp_Gender == "M" && a.Emp_Curr_Rank == (int)Rank.SFO4);
                ratioOfFemaleModel.SFO4_Female = byRegion.Count(a => a.Emp_Gender == "F" && a.Emp_Curr_Rank == (int)Rank.SFO4);
                ratioOfFemaleModel.INSP_Male = byRegion.Count(a => a.Emp_Gender == "M" && a.Emp_Curr_Rank == (int)Rank.INSP);
                ratioOfFemaleModel.INSP_Female = byRegion.Count(a => a.Emp_Gender == "F" && a.Emp_Curr_Rank == (int)Rank.INSP);
                ratioOfFemaleModel.SINSP_Male = byRegion.Count(a => a.Emp_Gender == "M" && a.Emp_Curr_Rank == (int)Rank.SINSP);
                ratioOfFemaleModel.SINSP_Female = byRegion.Count(a => a.Emp_Gender == "F" && a.Emp_Curr_Rank == (int)Rank.SINSP);
                ratioOfFemaleModel.CINSP_Male = byRegion.Count(a => a.Emp_Gender == "M" && a.Emp_Curr_Rank == (int)Rank.CINSP);
                ratioOfFemaleModel.CINSP_Female = byRegion.Count(a => a.Emp_Gender == "F" && a.Emp_Curr_Rank == (int)Rank.CINSP);
                ratioOfFemaleModel.SUPT_Male = byRegion.Count(a => a.Emp_Gender == "M" && a.Emp_Curr_Rank == (int)Rank.SUPT);
                ratioOfFemaleModel.SUPT_Female = byRegion.Count(a => a.Emp_Gender == "F" && a.Emp_Curr_Rank == (int)Rank.SUPT);
                ratioOfFemaleModel.SSUPT_Male = byRegion.Count(a => a.Emp_Gender == "M" && a.Emp_Curr_Rank == (int)Rank.SSUPT);
                ratioOfFemaleModel.SSUPT_Female = byRegion.Count(a => a.Emp_Gender == "F" && a.Emp_Curr_Rank == (int)Rank.SSUPT);
                ratioOfFemaleModel.CSUPT_Male = byRegion.Count(a => a.Emp_Gender == "M" && a.Emp_Curr_Rank == (int)Rank.CSUPT);
                ratioOfFemaleModel.CSUPT_Female = byRegion.Count(a => a.Emp_Gender == "F" && a.Emp_Curr_Rank == (int)Rank.CSUPT);
                ratioOfFemaleModel.DIR_Male = byRegion.Count(a => a.Emp_Gender == "M" && a.Emp_Curr_Rank == (int)Rank.DIR);
                ratioOfFemaleModel.DIR_Female = byRegion.Count(a => a.Emp_Gender == "F" && a.Emp_Curr_Rank == (int)Rank.DIR);

                ratioOfFemaleFireFighters.Add(ratioOfFemaleModel);
            }

            return ratioOfFemaleFireFighters;
        }

        public List<RetiringPersonnelModel> GetRetiringPersonnelPerYear(string type, int? month)
        {
            var year = DateTime.Now.Year;
            var retiringPersonnelList = new List<RetiringPersonnelModel>();
            var emp = from a in context.tblEmployees
                      join b in context.tblRanks on a.Emp_Curr_Rank equals b.Rank_Id
                      where a.Emp_IsDeleted == false && a.Emp_Curr_Rank != null && a.Emp_BirthDate != null
                            && a.Emp_DutyStatus != (int)DutyStatuses.Retired
                      select new
                      {
                          a.tblUnits,
                          a.Emp_Curr_Unit,
                          b.Rank_Name,
                          a.Emp_BirthDate,
                          a.Emp_Number,
                          a.Emp_FirstName,
                          a.Emp_MiddleName,
                          a.Emp_LastName,
                          a.Emp_SuffixName,
                          a.Emp_Curr_Rank
                      };


            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
                emp = emp.Where(a => a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID);
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
                emp = emp.Where(a => a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == CurrentUser.ProvinceID);
            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                emp = emp.Where(a => a.Emp_Curr_Unit == CurrentUser.EmployeeUnitId);

            foreach (var item in emp.ToList())
            {
                var retirementDate = Functions.GetRetirementDate(item.Emp_BirthDate, item.Emp_Curr_Rank);

                if (retirementDate != null && type == "Year" && retirementDate.Value.Year == year)
                {
                    var retiringPersonnelModel = new RetiringPersonnelModel
                    {
                        FirstName = item.Emp_FirstName,
                        MiddleName = item.Emp_MiddleName,
                        LastName = item.Emp_LastName,
                        SuffixName = item.Emp_SuffixName,
                        AccountNumber = item.Emp_Number,
                        Rank = item.Rank_Name,
                        RetirementDate = retirementDate.Value
                    };

                    retiringPersonnelList.Add(retiringPersonnelModel);
                }
                else if (retirementDate != null && type == "Month" && retirementDate.Value.Year == year && retirementDate.Value.Month == month)
                {
                    var retiringPersonnelModel = new RetiringPersonnelModel
                    {
                        FirstName = item.Emp_FirstName,
                        MiddleName = item.Emp_MiddleName,
                        LastName = item.Emp_LastName,
                        SuffixName = item.Emp_SuffixName,
                        AccountNumber = item.Emp_Number,
                        Rank = item.Rank_Name,
                        RetirementDate = retirementDate.Value
                    };

                    retiringPersonnelList.Add(retiringPersonnelModel);
                }
            }

            return retiringPersonnelList;
        }

        public PersonnelModel GetPersonnelNumberPerRegion(int regionId)
        {
            var emp = from a in context.tblEmployees
                      where a.Emp_IsDeleted == false && a.Emp_Curr_Rank != null && a.Emp_Curr_Unit != null
                            && a.Emp_DutyStatus == (int)DutyStatuses.Active
                      select new
                      {
                          a.tblUnits,
                          a.Emp_Curr_Unit,
                          a.Emp_Curr_Rank,
                          a.Emp_Curr_JobFunc
                      };

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
                emp = emp.Where(a => a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID);
            //if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
            //    emp = emp.Where(a => a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == CurrentUser.ProvinceID);
            //if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
            //    emp = emp.Where(a => a.Emp_Curr_Unit == CurrentUser.EmployeeUnitId);

            var personnelModel = new PersonnelModel();
            var byRegion = emp.Where(a => a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == regionId);
            var reg = context.tblRegions.FirstOrDefault(a => a.Reg_Id == regionId);
            personnelModel.RegId = reg.Reg_Id;
            personnelModel.Region = reg.Reg_Title;
            personnelModel.NUP = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.NUP);
            personnelModel.FO1 = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.FO1);
            personnelModel.FO2 = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.FO2);
            personnelModel.FO3 = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.FO3);
            personnelModel.SFO1 = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.SFO1);
            personnelModel.SFO2 = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.SFO2);
            personnelModel.SFO3 = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.SFO3);
            personnelModel.SFO4 = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.SFO4);
            personnelModel.INSP = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.INSP);
            personnelModel.SINSP = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.SINSP);
            personnelModel.CINSP = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.CINSP);
            personnelModel.SUPT = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.SUPT);
            personnelModel.SSUPT = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.SSUPT);
            personnelModel.CSUPT = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.CSUPT);
            personnelModel.DIR = byRegion.Count(a => a.Emp_Curr_Rank == (int)Rank.DIR);
            personnelModel.GeneralAdmin = byRegion.Count(a => a.Emp_Curr_JobFunc == (int)JobFunction.GeneralAdmin);
            personnelModel.Operations = byRegion.Count(a => a.Emp_Curr_JobFunc != (int)JobFunction.GeneralAdmin && a.Emp_Curr_JobFunc != null);

            return personnelModel;
        }

        public PersonnelModel GetPersonnelNumberPerProvince(int provinceId)
        {
            var emp = from a in context.tblEmployees
                      where a.Emp_IsDeleted == false && a.Emp_Curr_Rank != null && a.Emp_Curr_Unit != null
                            && a.Emp_DutyStatus == (int)DutyStatuses.Active
                      select new
                      {
                          a.tblUnits,
                          a.Emp_Curr_Unit,
                          a.Emp_Curr_Rank,
                          a.Emp_Curr_JobFunc
                      };

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
                emp = emp.Where(a => a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == CurrentUser.ProvinceID);

            var personnelModel = new PersonnelModel();
            var byProvince = emp.Where(a => a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == provinceId);
            var prov = context.tblProvinces.FirstOrDefault(a => a.Province_Id == provinceId);

            personnelModel.ProvinceId = prov.Province_Id;
            personnelModel.Province = prov.Province_Name;
            personnelModel.NUP = byProvince.Count(a => a.Emp_Curr_Rank == (int)Rank.NUP);
            personnelModel.FO1 = byProvince.Count(a => a.Emp_Curr_Rank == (int)Rank.FO1);
            personnelModel.FO2 = byProvince.Count(a => a.Emp_Curr_Rank == (int)Rank.FO2);
            personnelModel.FO3 = byProvince.Count(a => a.Emp_Curr_Rank == (int)Rank.FO3);
            personnelModel.SFO1 = byProvince.Count(a => a.Emp_Curr_Rank == (int)Rank.SFO1);
            personnelModel.SFO2 = byProvince.Count(a => a.Emp_Curr_Rank == (int)Rank.SFO2);
            personnelModel.SFO3 = byProvince.Count(a => a.Emp_Curr_Rank == (int)Rank.SFO3);
            personnelModel.SFO4 = byProvince.Count(a => a.Emp_Curr_Rank == (int)Rank.SFO4);
            personnelModel.INSP = byProvince.Count(a => a.Emp_Curr_Rank == (int)Rank.INSP);
            personnelModel.SINSP = byProvince.Count(a => a.Emp_Curr_Rank == (int)Rank.SINSP);
            personnelModel.CINSP = byProvince.Count(a => a.Emp_Curr_Rank == (int)Rank.CINSP);
            personnelModel.SUPT = byProvince.Count(a => a.Emp_Curr_Rank == (int)Rank.SUPT);
            personnelModel.SSUPT = byProvince.Count(a => a.Emp_Curr_Rank == (int)Rank.SSUPT);
            personnelModel.CSUPT = byProvince.Count(a => a.Emp_Curr_Rank == (int)Rank.CSUPT);
            personnelModel.DIR = byProvince.Count(a => a.Emp_Curr_Rank == (int)Rank.DIR);
            personnelModel.GeneralAdmin = byProvince.Count(a => a.Emp_Curr_JobFunc == (int)JobFunction.GeneralAdmin);
            personnelModel.Operations = byProvince.Count(a => a.Emp_Curr_JobFunc != (int)JobFunction.GeneralAdmin && a.Emp_Curr_JobFunc != null);

            return personnelModel;
        }

        public PersonnelModel GetPersonnelNumberPerStation(int station)
        {
            var emp = from a in context.tblEmployees
                      where a.Emp_IsDeleted == false && a.Emp_Curr_Rank != null && a.Emp_Curr_Unit != null
                            && a.Emp_DutyStatus == (int)DutyStatuses.Active
                      select new
                      {
                          a.tblUnits,
                          a.Emp_Curr_Unit,
                          a.Emp_Curr_Rank,
                          a.Emp_Curr_JobFunc
                      };

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                emp = emp.Where(a => a.Emp_Curr_Unit == CurrentUser.EmployeeUnitId);

            var personnelModel = new PersonnelModel();
            var byUnit = emp.Where(a => a.Emp_Curr_Unit == station);
            var unit = context.tblUnits.FirstOrDefault(a => a.Unit_Id == station);

            personnelModel.StationId = unit.Unit_Id;
            personnelModel.Station = unit.Unit_StationName;
            personnelModel.NUP = byUnit.Count(a => a.Emp_Curr_Rank == (int)Rank.NUP);
            personnelModel.FO1 = byUnit.Count(a => a.Emp_Curr_Rank == (int)Rank.FO1);
            personnelModel.FO2 = byUnit.Count(a => a.Emp_Curr_Rank == (int)Rank.FO2);
            personnelModel.FO3 = byUnit.Count(a => a.Emp_Curr_Rank == (int)Rank.FO3);
            personnelModel.SFO1 = byUnit.Count(a => a.Emp_Curr_Rank == (int)Rank.SFO1);
            personnelModel.SFO2 = byUnit.Count(a => a.Emp_Curr_Rank == (int)Rank.SFO2);
            personnelModel.SFO3 = byUnit.Count(a => a.Emp_Curr_Rank == (int)Rank.SFO3);
            personnelModel.SFO4 = byUnit.Count(a => a.Emp_Curr_Rank == (int)Rank.SFO4);
            personnelModel.INSP = byUnit.Count(a => a.Emp_Curr_Rank == (int)Rank.INSP);
            personnelModel.SINSP = byUnit.Count(a => a.Emp_Curr_Rank == (int)Rank.SINSP);
            personnelModel.CINSP = byUnit.Count(a => a.Emp_Curr_Rank == (int)Rank.CINSP);
            personnelModel.SUPT = byUnit.Count(a => a.Emp_Curr_Rank == (int)Rank.SUPT);
            personnelModel.SSUPT = byUnit.Count(a => a.Emp_Curr_Rank == (int)Rank.SSUPT);
            personnelModel.CSUPT = byUnit.Count(a => a.Emp_Curr_Rank == (int)Rank.CSUPT);
            personnelModel.DIR = byUnit.Count(a => a.Emp_Curr_Rank == (int)Rank.DIR);
            personnelModel.GeneralAdmin = byUnit.Count(a => a.Emp_Curr_JobFunc == (int)JobFunction.GeneralAdmin);
            personnelModel.Operations = byUnit.Count(a => a.Emp_Curr_JobFunc != (int)JobFunction.GeneralAdmin && a.Emp_Curr_JobFunc != null);

            return personnelModel;
        }
        
        public List<FireCodeDepositCollectionModel> GetFireCodeCollectionByUnit(int month, int unitId)
        {
            var year = DateTime.Now.Year;
            var collectionList = new List<FireCodeDepositCollectionModel>();
            collectionList = (
                     from appPayment in context.tblApplicationPayments
                     where appPayment.AP_ORDate.Month == month && appPayment.AP_ORDate.Year == year
                     && appPayment.AP_Unit_Id == unitId
                     select new FireCodeDepositCollectionModel
                     {
                         CollectionDate = appPayment.AP_ORDate,
                         CollectionAmount = appPayment.AP_ORAmount,
                         ORNumber = appPayment.AP_ORNumber,
                         UnitId = appPayment.AP_Unit_Id
                     }).ToList();

            return collectionList;
        }

        public List<FireCodeDepositCollectionModel> GetFireCodeDepositByUnit(int month, int unitId)
        {
            var year = DateTime.Now.Year;
            var depositLits = new List<FireCodeDepositCollectionModel>();
            depositLits = (
                    from dep in context.tblUnitDeposits
                    where dep.Dep_DepositDate.Month == month && dep.Dep_DepositDate.Year == year
                    && dep.Dep_Unit_Id == unitId
                    select new FireCodeDepositCollectionModel
                    {
                        DepositDate = dep.Dep_DepositDate,
                        DepositAmount = dep.Dep_Amount,
                        LCNumber = dep.Dep_LC_No,
                        UnitId = dep.Dep_Unit_Id
                    }).ToList();

            return depositLits;
        }

        public List<MRAAFModel> GetMRAAF2(int month)
        {
            var year = DateTime.Now.Year;

            var issuedORList = (from or in context.tblORSeries
                                join unit in context.tblUnits on or.OR_Unit_Id equals unit.Unit_Id
                                join muni in context.tblCityMunicipality on unit.Unit_Municipality_Id equals muni.Municipality_Id
                                join prov in context.tblProvinces on muni.Municipality_Province_Id equals prov.Province_Id
                                join reg in context.tblRegions on prov.Region_Id equals reg.Reg_Id
                                where DbFunctions.TruncateTime(or.OR_Issue_Date).Value.Month >= 1 && DbFunctions.TruncateTime(or.OR_Issue_Date).Value.Month <= month &&
                                       DbFunctions.TruncateTime(or.OR_Issue_Date).Value.Year == year
                                select new MRAAFModel
                                {
                                    UnitId = or.OR_Unit_Id,
                                    BeginningStart = or.OR_StartSeries,
                                    BeginningEnd = or.OR_EndSeries,
                                    RegionId = reg.Reg_Id,
                                    ProvinceId = prov.Province_Id,
                                    BeginningQty = or.OR_EndSeries - or.OR_StartSeries + 1,
                                    RegionName = reg.Reg_Description,
                                    IssueDate = or.OR_Issue_Date
                                });

            var applicationPaymentList = (from or in context.tblApplicationPayments
                                          join unit in context.tblUnits on or.AP_Unit_Id equals unit.Unit_Id
                                          join muni in context.tblCityMunicipality on unit.Unit_Municipality_Id equals muni.Municipality_Id
                                          join prov in context.tblProvinces on muni.Municipality_Province_Id equals prov.Province_Id
                                          join reg in context.tblRegions on prov.Region_Id equals reg.Reg_Id
                                          select new MRAAFModel
                                          {
                                              UnitId = or.AP_Unit_Id,
                                              PaymentOrNumber = or.AP_ORNumber,
                                          });

            var spoiledOr = (from or in context.tblSpoiledOR
                             join unit in context.tblUnits on or.SOR_Unit_Id equals unit.Unit_Id
                             join muni in context.tblCityMunicipality on unit.Unit_Municipality_Id equals muni.Municipality_Id
                             join prov in context.tblProvinces on muni.Municipality_Province_Id equals prov.Province_Id
                             join reg in context.tblRegions on prov.Region_Id equals reg.Reg_Id
                             select new MRAAFModel
                             {
                                 UnitId = or.SOR_Unit_Id,
                                 SpoiledOrNumber = or.SOR_Number,
                             });

            var orList = new List<MRAAFModel>();

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
            {
                var provinces = context.tblProvinces.Where(a => a.Region_Id == CurrentUser.RegionID);
                foreach (var prov in provinces)
                {
                    var orByProvince = issuedORList.Where(a => a.ProvinceId == prov.Province_Id).ToList();
                    foreach (var or in orByProvince.OrderBy(a => a.IssueDate))
                    {
                        var model = new MRAAFModel();
                        //model.Place = prov.Province_Name;
                        model.Place = orList.Count > 0 && orList.SingleOrDefault(a => a.Place == prov.Province_Name) != null ? "" : prov.Province_Name;
                        model.BeginningQty = (or.BeginningEnd - or.BeginningStart) + 1;
                        model.BeginningStart = or.BeginningStart;
                        model.BeginningEnd = or.BeginningEnd;
                        model.Source = "BFP";
                        model.IssueDate = or.IssueDate;
                        model.ProvinceId = or.ProvinceId;

                        var spoiled = spoiledOr.Where(a => a.SpoiledOrNumber >= or.BeginningStart && a.SpoiledOrNumber <= or.BeginningEnd && a.UnitId == or.UnitId).ToList();


                        var payments = applicationPaymentList.Where(a => a.PaymentOrNumber >= or.BeginningStart && a.PaymentOrNumber <= or.BeginningEnd && a.UnitId == or.UnitId).ToList();
                        var issuedOrList = new List<IssuedOrModel>();

                        if (spoiled != null && spoiled.Count > 0)
                        {
                            var spoiledORList = new List<SpoiledORModel>();
                            var count = 0;
                            foreach (var item in spoiled.OrderBy(a => a.SpoiledOrNumber))
                            {
                                var spoiledORModel = new SpoiledORModel();
                                spoiledORModel.SpoiledQty = 1;
                                spoiledORModel.SpoiledStart = item.SpoiledOrNumber;
                                spoiledORModel.SpoiledEnd = item.SpoiledOrNumber;

                                spoiledORList.Add(spoiledORModel);

                                var issuedOrModel = new IssuedOrModel();
                                long paymentEnd = 0;
                                if (payments.Count > 0)
                                {
                                    paymentEnd = payments.OrderByDescending(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;
                                    paymentEnd = item.SpoiledOrNumber > paymentEnd ? paymentEnd : item.SpoiledOrNumber - 1;

                                    issuedOrModel.IssuedStart = count > 0 ? issuedOrList.LastOrDefault().IssuedEnd + 2 : payments.OrderBy(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;

                                    issuedOrModel.IssuedEnd = paymentEnd;
                                    issuedOrModel.IssuedQty = (issuedOrModel.IssuedEnd - issuedOrModel.IssuedStart) + 1;

                                    issuedOrList.Add(issuedOrModel);
                                }

                                count++;
                            }

                            if (payments.Count > spoiled.Count)
                            {
                                var issuedOrModel = new IssuedOrModel();
                                issuedOrModel.IssuedStart = count > 0 ? issuedOrList.LastOrDefault().IssuedEnd + 2 : payments.OrderBy(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;

                                issuedOrModel.IssuedEnd = payments.OrderByDescending(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;
                                issuedOrModel.IssuedQty = (issuedOrModel.IssuedEnd - issuedOrModel.IssuedStart) + 1;

                                issuedOrList.Add(issuedOrModel);
                            }


                            model.SpoiledORList = spoiledORList;
                            model.IssuedOrList = issuedOrList;
                        }
                        else
                        {
                            if (payments != null && payments.Count > 0)
                            {
                                var issuedOrModel = new IssuedOrModel();
                                issuedOrModel.IssuedStart = payments.OrderBy(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;
                                issuedOrModel.IssuedEnd = payments.OrderByDescending(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;
                                issuedOrModel.IssuedQty = (issuedOrModel.IssuedEnd - issuedOrModel.IssuedStart) + 1;

                                issuedOrList.Add(issuedOrModel);

                                model.IssuedOrList = issuedOrList;
                            }
                        }


                        orList.Add(model);
                    }
                }
                return orList;
            }
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
            {
                var units = context.tblUnits.Where(a => a.Unit_ProvDistrict == CurrentUser.ProvinceID);
                foreach (var unit in units)
                {
                    var orByStation = issuedORList.Where(a => a.UnitId == unit.Unit_Id).ToList();
                    foreach (var or in orByStation.OrderBy(a => a.IssueDate))
                    {
                        var model = new MRAAFModel();
                        //model.Place = unit.Unit_StationName;
                        model.Place = orList.Count > 0 && orList.SingleOrDefault(a => a.Place == unit.Unit_StationName) != null ? "" : unit.Unit_StationName;
                        model.BeginningQty = (or.BeginningEnd - or.BeginningStart) + 1;
                        model.BeginningStart = or.BeginningStart;
                        model.BeginningEnd = or.BeginningEnd;
                        model.Source = "BFP";
                        model.IssueDate = or.IssueDate;
                        model.UnitId = or.UnitId;

                        var spoiled = spoiledOr.Where(a => a.SpoiledOrNumber >= or.BeginningStart && a.SpoiledOrNumber <= or.BeginningEnd && a.UnitId == or.UnitId).ToList();

                        var payments = applicationPaymentList.Where(a => a.PaymentOrNumber >= or.BeginningStart && a.PaymentOrNumber <= or.BeginningEnd && a.UnitId == or.UnitId).ToList();
                        var issuedOrList = new List<IssuedOrModel>();

                        if (spoiled != null && spoiled.Count > 0)
                        {
                            var spoiledORList = new List<SpoiledORModel>();
                            var count = 0;
                            foreach (var item in spoiled.OrderBy(a => a.SpoiledOrNumber))
                            {
                                var spoiledORModel = new SpoiledORModel();
                                spoiledORModel.SpoiledQty = 1;
                                spoiledORModel.SpoiledStart = item.SpoiledOrNumber;
                                spoiledORModel.SpoiledEnd = item.SpoiledOrNumber;

                                spoiledORList.Add(spoiledORModel);

                                var issuedOrModel = new IssuedOrModel();
                                long paymentEnd = 0;
                                if (payments.Count > 0)
                                {
                                    paymentEnd = payments.OrderByDescending(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;
                                    paymentEnd = item.SpoiledOrNumber > paymentEnd ? paymentEnd : item.SpoiledOrNumber - 1;

                                    issuedOrModel.IssuedStart = count > 0 ? issuedOrList.LastOrDefault().IssuedEnd + 2 : payments.OrderBy(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;

                                    issuedOrModel.IssuedEnd = paymentEnd;
                                    issuedOrModel.IssuedQty = (issuedOrModel.IssuedEnd - issuedOrModel.IssuedStart) + 1;

                                    issuedOrList.Add(issuedOrModel);
                                }

                                count++;
                            }

                            if (payments.Count > spoiled.Count)
                            {
                                var issuedOrModel = new IssuedOrModel();
                                issuedOrModel.IssuedStart = count > 0 ? issuedOrList.LastOrDefault().IssuedEnd + 2 : payments.OrderBy(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;

                                issuedOrModel.IssuedEnd = payments.OrderByDescending(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;
                                issuedOrModel.IssuedQty = (issuedOrModel.IssuedEnd - issuedOrModel.IssuedStart) + 1;

                                issuedOrList.Add(issuedOrModel);
                            }


                            model.SpoiledORList = spoiledORList;
                            model.IssuedOrList = issuedOrList;
                        }
                        else
                        {
                            if (payments != null && payments.Count > 0)
                            {
                                var issuedOrModel = new IssuedOrModel();
                                issuedOrModel.IssuedStart = payments.OrderBy(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;
                                issuedOrModel.IssuedEnd = payments.OrderByDescending(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;
                                issuedOrModel.IssuedQty = (issuedOrModel.IssuedEnd - issuedOrModel.IssuedStart) + 1;

                                issuedOrList.Add(issuedOrModel);

                                model.IssuedOrList = issuedOrList;
                            }
                        }


                        orList.Add(model);
                    }
                }
                return orList;
            }
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
            {
                var units = context.tblUnits.FirstOrDefault(a => a.Unit_Id == CurrentUser.EmployeeUnitId);

                var orByStation = issuedORList.Where(a => a.UnitId == units.Unit_Id).ToList();
                foreach (var or in orByStation.OrderBy(a => a.IssueDate))
                {
                    var model = new MRAAFModel();
                    //model.Place = units.Unit_StationName;
                    model.Place = orList.Count > 0 && orList.SingleOrDefault(a => a.Place == units.Unit_StationName) != null ? "" : units.Unit_StationName;
                    model.BeginningQty = (or.BeginningEnd - or.BeginningStart) + 1;
                    model.BeginningStart = or.BeginningStart;
                    model.BeginningEnd = or.BeginningEnd;
                    model.Source = "BFP";
                    model.IssueDate = or.IssueDate;
                    model.UnitId = or.UnitId;

                    var spoiled = spoiledOr.Where(a => a.SpoiledOrNumber >= or.BeginningStart && a.SpoiledOrNumber <= or.BeginningEnd && a.UnitId == or.UnitId).ToList();

                    var payments = applicationPaymentList.Where(a => a.PaymentOrNumber >= or.BeginningStart && a.PaymentOrNumber <= or.BeginningEnd && a.UnitId == or.UnitId).ToList();
                    var issuedOrList = new List<IssuedOrModel>();

                    if (spoiled != null && spoiled.Count > 0)
                    {
                        var spoiledORList = new List<SpoiledORModel>();
                        var count = 0;
                        foreach (var item in spoiled.OrderBy(a => a.SpoiledOrNumber))
                        {
                            var spoiledORModel = new SpoiledORModel();
                            spoiledORModel.SpoiledQty = 1;
                            spoiledORModel.SpoiledStart = item.SpoiledOrNumber;
                            spoiledORModel.SpoiledEnd = item.SpoiledOrNumber;

                            spoiledORList.Add(spoiledORModel);

                            var issuedOrModel = new IssuedOrModel();
                            long paymentEnd = 0;
                            if (payments.Count > 0)
                            {
                                paymentEnd = payments.OrderByDescending(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;
                                paymentEnd = item.SpoiledOrNumber > paymentEnd ? paymentEnd : item.SpoiledOrNumber - 1;

                                issuedOrModel.IssuedStart = count > 0 ? issuedOrList.LastOrDefault().IssuedEnd + 2 : payments.OrderBy(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;

                                issuedOrModel.IssuedEnd = paymentEnd;
                                issuedOrModel.IssuedQty = (issuedOrModel.IssuedEnd - issuedOrModel.IssuedStart) + 1;

                                issuedOrList.Add(issuedOrModel);
                            }

                            count++;
                        }

                        if (payments.Count > spoiled.Count)
                        {
                            var issuedOrModel = new IssuedOrModel();
                            issuedOrModel.IssuedStart = count > 0 ? issuedOrList.LastOrDefault().IssuedEnd + 2 : payments.OrderBy(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;

                            issuedOrModel.IssuedEnd = payments.OrderByDescending(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;
                            issuedOrModel.IssuedQty = (issuedOrModel.IssuedEnd - issuedOrModel.IssuedStart) + 1;

                            issuedOrList.Add(issuedOrModel);
                        }


                        model.SpoiledORList = spoiledORList;
                        model.IssuedOrList = issuedOrList;
                    }
                    else
                    {
                        if (payments != null && payments.Count > 0)
                        {
                            var issuedOrModel = new IssuedOrModel();
                            issuedOrModel.IssuedStart = payments.OrderBy(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;
                            issuedOrModel.IssuedEnd = payments.OrderByDescending(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;
                            issuedOrModel.IssuedQty = (issuedOrModel.IssuedEnd - issuedOrModel.IssuedStart) + 1;

                            issuedOrList.Add(issuedOrModel);

                            model.IssuedOrList = issuedOrList;
                        }
                    }


                    orList.Add(model);
                }

                return orList;
            }
            else
            {
                var regions = issuedORList.Select(a => a.RegionName).Distinct().ToList();
                foreach (var reg in regions)
                {
                    var feebyRegion = issuedORList.Where(a => a.RegionName == reg).ToList();
                    foreach (var or in feebyRegion.OrderBy(a => a.IssueDate))
                    {
                        var model = new MRAAFModel();
                        //model.Place = reg;
                        model.Place = orList.Count > 0 && orList.SingleOrDefault(a => a.Place == reg) != null ? "" : reg;
                        model.BeginningQty = (or.BeginningEnd - or.BeginningStart) + 1;
                        model.BeginningStart = or.BeginningStart;
                        model.BeginningEnd = or.BeginningEnd;
                        model.Source = "BFP";
                        model.IssueDate = or.IssueDate;
                        model.RegionId = or.RegionId;

                        var spoiled = spoiledOr.Where(a => a.SpoiledOrNumber >= or.BeginningStart && a.SpoiledOrNumber <= or.BeginningEnd && a.UnitId == or.UnitId).ToList();

                        var payments = applicationPaymentList.Where(a => a.PaymentOrNumber >= or.BeginningStart && a.PaymentOrNumber <= or.BeginningEnd && a.UnitId == or.UnitId).ToList();
                        var issuedOrList = new List<IssuedOrModel>();

                        if (spoiled != null && spoiled.Count > 0)
                        {
                            var spoiledORList = new List<SpoiledORModel>();
                            var count = 0;
                            foreach (var item in spoiled.OrderBy(a => a.SpoiledOrNumber))
                            {
                                var spoiledORModel = new SpoiledORModel();
                                spoiledORModel.SpoiledQty = 1;
                                spoiledORModel.SpoiledStart = item.SpoiledOrNumber;
                                spoiledORModel.SpoiledEnd = item.SpoiledOrNumber;

                                spoiledORList.Add(spoiledORModel);

                                var issuedOrModel = new IssuedOrModel();
                                long paymentEnd = 0;
                                if (payments.Count > 0)
                                {
                                    paymentEnd = payments.OrderByDescending(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;
                                    paymentEnd = item.SpoiledOrNumber > paymentEnd ? paymentEnd : item.SpoiledOrNumber - 1;

                                    issuedOrModel.IssuedStart = count > 0 ? issuedOrList.LastOrDefault().IssuedEnd + 2 : payments.OrderBy(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;

                                    issuedOrModel.IssuedEnd = paymentEnd;
                                    issuedOrModel.IssuedQty = (issuedOrModel.IssuedEnd - issuedOrModel.IssuedStart) + 1;

                                    issuedOrList.Add(issuedOrModel);
                                }

                                count++;
                            }

                            if (payments.Count > spoiled.Count)
                            {
                                var issuedOrModel = new IssuedOrModel();
                                issuedOrModel.IssuedStart = count > 0 ? issuedOrList.LastOrDefault().IssuedEnd + 2 : payments.OrderBy(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;

                                issuedOrModel.IssuedEnd = payments.OrderByDescending(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;
                                issuedOrModel.IssuedQty = (issuedOrModel.IssuedEnd - issuedOrModel.IssuedStart) + 1;

                                issuedOrList.Add(issuedOrModel);
                            }


                            model.SpoiledORList = spoiledORList;
                            model.IssuedOrList = issuedOrList;
                        }
                        else
                        {
                            if (payments != null && payments.Count > 0)
                            {
                                var issuedOrModel = new IssuedOrModel();
                                issuedOrModel.IssuedStart = payments.OrderBy(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;
                                issuedOrModel.IssuedEnd = payments.OrderByDescending(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;
                                issuedOrModel.IssuedQty = (issuedOrModel.IssuedEnd - issuedOrModel.IssuedStart) + 1;

                                issuedOrList.Add(issuedOrModel);

                                model.IssuedOrList = issuedOrList;
                            }
                        }


                        orList.Add(model);
                    }
                }

                return orList;
            }
            
        }
        public List<MRAAFModel> GetMRAAF(int month)
        {
            var year = DateTime.Now.Year;

            var issuedORList = (from or in context.tblORSeries
                                join unit in context.tblUnits on or.OR_Unit_Id equals unit.Unit_Id
                                join muni in context.tblCityMunicipality on unit.Unit_Municipality_Id equals muni.Municipality_Id
                                join prov in context.tblProvinces on muni.Municipality_Province_Id equals prov.Province_Id
                                join reg in context.tblRegions on prov.Region_Id equals reg.Reg_Id
                                where DbFunctions.TruncateTime(or.OR_Issue_Date).Value.Month >= 1 && DbFunctions.TruncateTime(or.OR_Issue_Date).Value.Month <= month &&
                                       DbFunctions.TruncateTime(or.OR_Issue_Date).Value.Year == year
                                select new MRAAFModel
                                {
                                    UnitId = or.OR_Unit_Id,
                                    BeginningStart = or.OR_StartSeries,
                                    BeginningEnd = or.OR_EndSeries,
                                    RegionId = reg.Reg_Id,
                                    ProvinceId = prov.Province_Id,
                                    BeginningQty = or.OR_EndSeries - or.OR_StartSeries + 1,
                                    RegionName = reg.Reg_Description,
                                    IssueDate = or.OR_Issue_Date,
                                    MunicipalityId = muni.Municipality_Id,
                                    ProvinceName = prov.Province_Name,
                                    CityMunicipalityName = muni.Municipality_Name,
                                    MunicipalityProvinceId = muni.Municipality_Province_Id,
                                    UnitStationName = unit.Unit_StationName
                                });

            var applicationPaymentList = (from or in context.tblApplicationPayments
                                          join unit in context.tblUnits on or.AP_Unit_Id equals unit.Unit_Id
                                          join muni in context.tblCityMunicipality on unit.Unit_Municipality_Id equals muni.Municipality_Id
                                          join prov in context.tblProvinces on muni.Municipality_Province_Id equals prov.Province_Id
                                          join reg in context.tblRegions on prov.Region_Id equals reg.Reg_Id
                                          select new MRAAFModel
                                          {
                                              UnitId = or.AP_Unit_Id,
                                              PaymentOrNumber = or.AP_ORNumber,
                                          });

            var spoiledOr = (from or in context.tblSpoiledOR
                             join unit in context.tblUnits on or.SOR_Unit_Id equals unit.Unit_Id
                             join muni in context.tblCityMunicipality on unit.Unit_Municipality_Id equals muni.Municipality_Id
                             join prov in context.tblProvinces on muni.Municipality_Province_Id equals prov.Province_Id
                             join reg in context.tblRegions on prov.Region_Id equals reg.Reg_Id
                             select new MRAAFModel
                             {
                                 UnitId = or.SOR_Unit_Id,
                                 SpoiledOrNumber = or.SOR_Number,
                             });

            var orList = new List<MRAAFModel>();

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
                issuedORList = issuedORList.Where(a => a.RegionId == CurrentUser.RegionID);
            else if(PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
                issuedORList = issuedORList.Where(a => a.ProvinceId == CurrentUser.ProvinceID);
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                issuedORList = issuedORList.Where(a => a.UnitId == CurrentUser.EmployeeUnitId);

            var orRetList = issuedORList.Distinct().ToList();
            var cityMunicipality = issuedORList.Select(a => a.MunicipalityId).Distinct().ToList();

            foreach (var mun in cityMunicipality)
            {
                var feesByMunicipality = issuedORList.Where(a => a.MunicipalityId == mun).Distinct().ToList();

                var reg_Prov_Mun_Station_Name = (from a in orRetList
                             where a.MunicipalityId == mun
                             select new
                             {
                                 a.ProvinceName,
                                 a.RegionName,
                                 a.CityMunicipalityName,
                                 a.UnitStationName
                             }).FirstOrDefault();

                foreach (var or in feesByMunicipality.OrderBy(a => a.IssueDate))
                {
                    var model = new MRAAFModel();
                    model.RegionName = reg_Prov_Mun_Station_Name.RegionName;
                    model.ProvinceName = reg_Prov_Mun_Station_Name.ProvinceName;
                    if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                        model.Place = orList.Count > 0 && orList.SingleOrDefault(a => a.Place == reg_Prov_Mun_Station_Name.UnitStationName) != null ? "" : reg_Prov_Mun_Station_Name.UnitStationName;
                    else
                        model.Place = orList.Count > 0 && orList.SingleOrDefault(a => a.Place == reg_Prov_Mun_Station_Name.CityMunicipalityName) != null ? "" : reg_Prov_Mun_Station_Name.CityMunicipalityName;

                    model.BeginningQty = (or.BeginningEnd - or.BeginningStart) + 1;
                    model.BeginningStart = or.BeginningStart;
                    model.BeginningEnd = or.BeginningEnd;
                    model.Source = "BFP";
                    model.IssueDate = or.IssueDate;
                    model.RegionId = or.RegionId;
                    model.UnitId = or.UnitId;
                    model.ProvinceId = or.ProvinceId;

                    var spoiled = spoiledOr.Where(a => a.SpoiledOrNumber >= or.BeginningStart && a.SpoiledOrNumber <= or.BeginningEnd && a.UnitId == or.UnitId).ToList();

                    var payments = applicationPaymentList.Where(a => a.PaymentOrNumber >= or.BeginningStart && a.PaymentOrNumber <= or.BeginningEnd && a.UnitId == or.UnitId).ToList();
                    var issuedOrList = new List<IssuedOrModel>();

                    if(model.Place == "Marabut")
                    {

                    }
                    if (spoiled != null && spoiled.Count > 0)
                    {
                        var spoiledORList = new List<SpoiledORModel>();
                        var count = 0;
                        foreach (var item in spoiled.OrderBy(a => a.SpoiledOrNumber))
                        {
                            var spoiledORModel = new SpoiledORModel();
                            spoiledORModel.SpoiledQty = 1;
                            spoiledORModel.SpoiledStart = item.SpoiledOrNumber;
                            spoiledORModel.SpoiledEnd = item.SpoiledOrNumber;

                            spoiledORList.Add(spoiledORModel);

                            var issuedOrModel = new IssuedOrModel();
                            long paymentEnd = 0;
                            if (payments.Count > 0)
                            {
                                paymentEnd = payments.OrderByDescending(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;
                                paymentEnd = item.SpoiledOrNumber > paymentEnd ? paymentEnd : item.SpoiledOrNumber - 1;

                                issuedOrModel.IssuedStart = count > 0 ? issuedOrList.LastOrDefault().IssuedEnd + 2 : payments.OrderBy(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;

                                issuedOrModel.IssuedEnd = paymentEnd;
                                issuedOrModel.IssuedQty = (issuedOrModel.IssuedEnd - issuedOrModel.IssuedStart) + 1;

                                issuedOrList.Add(issuedOrModel);
                            }

                            count++;
                        }

                        if (payments.Count > spoiled.Count)
                        {
                            var issuedOrModel = new IssuedOrModel();
                            issuedOrModel.IssuedStart = count > 0 ? issuedOrList.LastOrDefault().IssuedEnd + 2 : payments.OrderBy(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;

                            //issuedOrModel.IssuedEnd = payments.OrderByDescending(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;
                            issuedOrModel.IssuedEnd = payments.OrderByDescending(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber < issuedOrModel.IssuedStart ? issuedOrModel.IssuedStart
                                : payments.OrderByDescending(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;
                            issuedOrModel.IssuedQty = (issuedOrModel.IssuedEnd - issuedOrModel.IssuedStart) + 1;

                            issuedOrList.Add(issuedOrModel);
                        }


                        model.SpoiledORList = spoiledORList;
                        model.IssuedOrList = issuedOrList;
                    }
                    else
                    {
                        if (payments != null && payments.Count > 0)
                        {
                            var issuedOrModel = new IssuedOrModel();
                            issuedOrModel.IssuedStart = payments.OrderBy(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;
                            issuedOrModel.IssuedEnd = payments.OrderByDescending(a => a.PaymentOrNumber).FirstOrDefault().PaymentOrNumber;
                            issuedOrModel.IssuedQty = (issuedOrModel.IssuedEnd - issuedOrModel.IssuedStart) + 1;

                            issuedOrList.Add(issuedOrModel);

                            model.IssuedOrList = issuedOrList;
                        }
                    }


                    orList.Add(model);

                }

            }

            return orList;
        }
        
    }
}