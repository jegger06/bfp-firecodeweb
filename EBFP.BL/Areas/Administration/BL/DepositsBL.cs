using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EBFP.DataAccess;
using Queries.Core.Repositories;
using System.Data.Entity;
using EBFP.BL.Helper;
using System.Linq.Dynamic;

namespace EBFP.BL.Administration
{
    public class DepositsBL : Repository<tblUnitDeposits, DepositsModel>, IDeposits
    {
        public DepositsBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }
        public void CreateMapping()
        {
            Mapper.CreateMap<tblUnitDeposits, DepositsModel>().ReverseMap();
            Mapper.CreateMap<List<tblUnitDeposits>, List<DepositsModel>>().ReverseMap();
            Mapper.CreateMap<List<tblUnitDeposits>, List<DepositsModel>>();
        }

        public void SyncDepositLocalToServer(List<DepositsModel> deposits)
        {

            foreach (var dep in deposits)
            {
                var Deposits = BFPContext.tblUnitDeposits
                                            .FirstOrDefault(a => a.Ref_Dep_Id == dep.Ref_Dep_Id && a.Dep_Unit_Id == dep.Dep_Unit_Id);

                if (Deposits == null)
                {
                    Deposits = new tblUnitDeposits();
                    Mapper.Map(dep, Deposits);
                    BFPContext.tblUnitDeposits.Add(Deposits);
                }
                else
                {
                    var depId = Deposits.Dep_Id;
                    Mapper.Map(dep, Deposits);
                    Deposits.Dep_Id = depId;
                }
            }

            BFPContext.SaveChanges();
        }

        //

        public List<string> BankName(int unitId)
        {
            var rets = base.BFPContext.tblUnitDeposits
                                        .Where(a => a.Dep_Unit_Id == unitId)
                                        .OrderBy(a => a.Dep_Bank)
                                        .Select(a => a.Dep_Bank)
                                        .ToList();
            return rets;
        }

        public List<string> EmployeeName(int unitId)
        {
            var rets = base.BFPContext.tblEmployees
                                        .Where(a => a.Emp_FirstName != "Administrator" && a.Emp_Curr_Unit == unitId)
                                        .OrderBy(a => a.Emp_FirstName)
                                        .Select(a => a.Emp_FirstName + " " + a.Emp_LastName)
                                        .ToList();
            return rets;
        }

        public DepositListResult GetDepositList(GridInfo gridInfo,int unitId)
        {

            var retDeposits = new List<DepositsModel>();

            var SearchTerms = gridInfo.searchDepositModel;

            var deposits = (from a in BFPContext.tblUnitDeposits
                            join b in BFPContext.tblEmployees on a.Dep_Depositor_Emp_Id equals b.Emp_Id
                            join rank in BFPContext.tblRanks on b.Emp_Curr_Rank equals rank.Rank_Id
                                  into tempRank
                            from rank in tempRank.DefaultIfEmpty()
                            where a.Dep_Unit_Id == unitId
                           // orderby a.Dep_LC_No ascending
                            select new DepositsModel
                            {
                                Dep_Id = a.Dep_Id,
                                Dep_Bank = a.Dep_Bank,
                                Dep_Depositor_Emp_Id = a.Dep_Depositor_Emp_Id,
                                Dep_LC_No = a.Dep_LC_No,
                                Dep_DepositDate = a.Dep_DepositDate,
                                Dep_Amount = a.Dep_Amount,
                                Dep_Collection_EndDate = a.Dep_Collection_EndDate,
                                Dep_Collection_StartDate = a.Dep_Collection_StartDate,
                                Dep_Depositor_Emp_Name = rank != null ? rank.Rank_Name + " " + b.Emp_FirstName + " " + b.Emp_LastName : b.Emp_FirstName + " " + b.Emp_LastName
                            }).AsQueryable();

            if (!string.IsNullOrEmpty(SearchTerms.LCNumber))
                deposits = deposits.Where(a => a.Dep_LC_No.Contains(SearchTerms.LCNumber));
            if (!string.IsNullOrEmpty(SearchTerms.BankName))
                deposits = deposits.Where(a => a.Dep_Bank.Contains(SearchTerms.BankName));
            if (SearchTerms.Depositor > 0)
                deposits = deposits.Where(a => a.Dep_Depositor_Emp_Id == SearchTerms.Depositor);

            if (SearchTerms.IncludeDates)
                deposits = deposits.Where(a => DbFunctions.TruncateTime(a.Dep_DepositDate) >= SearchTerms.DepositFrom.Date && DbFunctions.TruncateTime(a.Dep_DepositDate) <= SearchTerms.DepositTo.Date);
           
            gridInfo.recordsTotal = deposits.Select(a => a.Dep_Id).Count();
            var depositListResult = deposits.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
             .Skip(gridInfo.start)
             .Take(gridInfo.length)
             .ToList();

            foreach (var dep in depositListResult)
            {
                retDeposits.Add(new DepositsModel
                {
                    Dep_Id = dep.Dep_Id,
                    Dep_Bank = dep.Dep_Bank,
                    Dep_Depositor_Emp_Id = dep.Dep_Depositor_Emp_Id,
                    Dep_LC_No = dep.Dep_LC_No,
                    Dep_DepositDate = dep.Dep_DepositDate,
                    Dep_Amount = dep.Dep_Amount,
                    Dep_Collection_EndDate = dep.Dep_Collection_EndDate,
                    Dep_Collection_StartDate = dep.Dep_Collection_StartDate,
                    Dep_Depositor_Emp_Name = dep.Dep_Depositor_Emp_Name
                });
            }

            return new DepositListResult
            {
                DepositList = retDeposits.OrderBy(a => a.Dep_CreatedDate).ToList(),
                DatatableInfo = gridInfo
            };
        }
    }
}