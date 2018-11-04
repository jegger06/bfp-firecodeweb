using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.DataAccess;
using EBFP.Helper;
using Queries.Core.Repositories;

namespace EBFP.BL.CIS
{
    public class UnserviceableBL : Repository<tblUnserviceablePhysicalInventory, UnserviceableModel>, IUnserviceable
    {
        public UnserviceableBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public UnserviceableListResult GetListResult(GridInfo gridInfo)
        {
            var unserviceable = from us in BFPContext.tblUnserviceablePhysicalInventory
                join cr in BFPContext.tblEmployees on us.UPI_Created_Emp_Id equals cr.Emp_Id
                    into tempCr
                from cr in tempCr.DefaultIfEmpty()
                join md in BFPContext.tblEmployees on us.UPI_LastUpdate_Emp_Id equals md.Emp_Id
                    into tempMd
                from md in tempMd.DefaultIfEmpty()
                select new UnserviceableModel
                {
                    UPI_Id = us.UPI_Id,
                    UPI_ReportingOffice = us.UPI_ReportingOffice,
                    UPI_WMR = us.UPI_WMR,
                    UPI_CreatedDate = us.UPI_CreatedDate,
                    UPI_LastUpdateDate = us.UPI_LastUpdateDate,
                    UPI_CreatedBy =
                        cr.tblRanks.Rank_Name + " " + cr.Emp_FirstName + " " + " " + cr.Emp_MiddleName + " " +
                        cr.Emp_LastName + " " + cr.Emp_SuffixName,
                    UPI_LastUpdateBy =
                        md.tblRanks.Rank_Name + " " + md.Emp_FirstName + " " + md.Emp_MiddleName + " " + md.Emp_LastName +
                        " " + md.Emp_SuffixName
                };

            var searchUnserviceable = gridInfo.searchUnserviceable;
            if (!string.IsNullOrEmpty(searchUnserviceable.UPI_ReportingOffice))
                unserviceable =
                    unserviceable.Where(a => a.UPI_ReportingOffice.Contains(searchUnserviceable.UPI_ReportingOffice));
            if (!string.IsNullOrEmpty(searchUnserviceable.UPI_WMR))
                unserviceable =
                    unserviceable.Where(a => a.UPI_WMR.Contains(searchUnserviceable.UPI_WMR));

            gridInfo.recordsTotal = unserviceable.Select(a => a.UPI_Id).Count();

            unserviceable = unserviceable.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
                .Skip(gridInfo.start)
                .Take(gridInfo.length);

            return new UnserviceableListResult
            {
                UnserviceableListModel = unserviceable.ToList(),
                DatatableInfo = gridInfo
            };
        }

        public void UpdateUnserviceable(UnserviceableModel model)
        {
            var unserviceableDet =
                BFPContext.tblUnserviceablePhysicalInventory.FirstOrDefault(a => a.UPI_Id == model.UPI_Id);
            if (unserviceableDet == null) throw new Exception("Unserviceable Physical Inventory cannot be found!");

            model.UPI_CreatedDate = unserviceableDet.UPI_CreatedDate;
            model.UPI_Created_Emp_Id = unserviceableDet.UPI_Created_Emp_Id;
            Mapper.Map(model, unserviceableDet);
            unserviceableDet.UPI_LastUpdateDate = DateTime.Now;
            unserviceableDet.UPI_LastUpdate_Emp_Id = CurrentUser.EmployeeId;

            BFPContext.Entry(unserviceableDet).State = EntityState.Modified;
            BFPContext.SaveChanges();

            UpdateToUnserviceable(model);
        }

        public void UpdateToUnserviceable(UnserviceableModel model)
        {
                var existing = BFPContext.tblPhysicalInventory.Where(a => a.PI_UPI_Id == model.UPI_Id).ToList();

                foreach (var item in existing)
                {
                    if (model.PhysicalInventoryList.Count(a => a.PI_Id == item.PI_Id) == 0)
                    {
                        item.PI_UPI_Id = null;
                    }
                    BFPContext.SaveChanges();
                }

                var existingIds = existing.Select(a => a.PI_Id).ToList();
                var toAdd = model.PhysicalInventoryList.Where(a => !existingIds.Contains(a.PI_Id)).ToList();

                foreach (var item in toAdd)
                {
                    if (model.PhysicalInventoryList.Count(a => a.PI_Id == item.PI_Id) > 0)
                    {
                        var update = BFPContext.tblPhysicalInventory.FirstOrDefault(a => a.PI_Id == item.PI_Id);
                        if (update != null)
                            update.PI_UPI_Id = model.UPI_Id;
                    }
                    BFPContext.SaveChanges();
                }

        }

        public void UpdatePhysicalInventoryToNull(int upiId)
        {
            var physicalInventory = BFPContext.tblPhysicalInventory.Where(a => a.PI_UPI_Id == upiId).ToList();
            if (physicalInventory.Count <= 0) return;
            foreach (var pi in physicalInventory)
            {
                pi.PI_UPI_Id = null;
                BFPContext.Entry(pi).State = EntityState.Modified;
            }

            BFPContext.SaveChanges();
        }

        public bool DeleteByID(int unserviceableId)
        {
            var physicalInventory =
              BFPContext.tblUnserviceablePhysicalInventory.FirstOrDefault(a => a.UPI_Id == unserviceableId);
            if (physicalInventory == null) throw new Exception("Unserviceable Physical Inventory cannot be found!");

            BFPContext.tblUnserviceablePhysicalInventory.Remove(physicalInventory);
            BFPContext.SaveChanges();

            //Update Physical Inventory to null
            UpdatePhysicalInventoryToNull(unserviceableId);

            return true;
        }

        public bool UnserviceableExists(string wmr, int id = 0)
        {
            var details = BFPContext.tblUnserviceablePhysicalInventory
             .Where(a => a.UPI_WMR == wmr);

            if (id > 0)
                details = details.Where(a => a.UPI_Id != id);

            return details.Any();
        }

        public UnserviceableModel GetUnserviceableById(int unserviceableId)
        {
            var ret = new UnserviceableModel();

            var unserviceable =
                BFPContext.tblUnserviceablePhysicalInventory.FirstOrDefault(a => a.UPI_Id == unserviceableId);

            if (unserviceable != null)
            {
                Mapper.Map(unserviceable, ret);
            }
            return ret;
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblUnserviceablePhysicalInventory, UnserviceableModel>().ReverseMap();
            Mapper.CreateMap<List<tblUnserviceablePhysicalInventory>, List<UnserviceableModel>>().ReverseMap();
            Mapper.CreateMap<List<tblUnserviceablePhysicalInventory>, List<UnserviceableModel>>();
        }

        public int Add(UnserviceableModel model)
        {
            var unserviceableDet = new tblUnserviceablePhysicalInventory();
                      
            Mapper.Map(model, unserviceableDet);

           BFPContext.tblUnserviceablePhysicalInventory.Add(unserviceableDet);
           BFPContext.SaveChanges();

            return unserviceableDet.UPI_Id;
        }
    }
}