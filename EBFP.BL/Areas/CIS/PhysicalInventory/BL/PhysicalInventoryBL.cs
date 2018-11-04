using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.BL.Inventory;
using EBFP.DataAccess;
using EBFP.Helper;
using Queries.Core.Repositories;

namespace EBFP.BL.CIS
{
    public class PhysicalInventoryBL : Repository<tblPhysicalInventory, PhysicalInventoryModel>, IPhysicalInventory
    {
        public PhysicalInventoryBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }
        public void CreateMapping()
        {
            Mapper.CreateMap<tblPhysicalInventory, PhysicalInventoryModel>().ReverseMap();
            Mapper.CreateMap<List<tblPhysicalInventory>, List<PhysicalInventoryModel>>().ReverseMap();
            Mapper.CreateMap<List<tblPhysicalInventory>, List<PhysicalInventoryModel>>();
        }
        public PhysicalInventoryListResult GetListResult(GridInfo gridInfo)
        {
            var retPhysicalInventory = new List<PhysicalInventoryModel>();
            var physicalInventory = BFPContext.tblPhysicalInventory.Where(a => a.PI_IsDeleted == false && a.PI_Unit_Id == CurrentUser.EmployeeUnitId);
            

            var searchPhysicalInventoryModel = gridInfo.searchPhysicalInventoryModel;

            if (!string.IsNullOrEmpty(searchPhysicalInventoryModel.PI_Description))
                physicalInventory = physicalInventory.Where(a => a.PI_Description.Contains(searchPhysicalInventoryModel.PI_Description));
            if (!string.IsNullOrEmpty(searchPhysicalInventoryModel.PI_PropertyNumber))
                physicalInventory =
                    physicalInventory.Where(a => a.PI_PropertyNumber.Contains(searchPhysicalInventoryModel.PI_PropertyNumber));
            if (!string.IsNullOrEmpty(searchPhysicalInventoryModel.PI_UnitOfMeasure))
                physicalInventory =
                    physicalInventory.Where(a => a.PI_UnitOfMeasure.Contains(searchPhysicalInventoryModel.PI_UnitOfMeasure));
            if (!string.IsNullOrEmpty(searchPhysicalInventoryModel.PI_Office))
                physicalInventory =
                    physicalInventory.Where(a => a.PI_Office.Contains(searchPhysicalInventoryModel.PI_Office));

            if (searchPhysicalInventoryModel.PI_Dir_Id > 0)
                physicalInventory = physicalInventory.Where(a => a.PI_Dir_Id == searchPhysicalInventoryModel.PI_Dir_Id);
            if (searchPhysicalInventoryModel.PI_Unit_Id > 0)
                physicalInventory = physicalInventory.Where(a => a.PI_Unit_Id == searchPhysicalInventoryModel.PI_Unit_Id);
            if (searchPhysicalInventoryModel.PI_IG_Id > 0)
                physicalInventory = physicalInventory.Where(a => a.PI_IG_Id == searchPhysicalInventoryModel.PI_IG_Id);
            if (searchPhysicalInventoryModel.PI_Art_Id > 0)
                physicalInventory = physicalInventory.Where(a => a.PI_Art_Id == searchPhysicalInventoryModel.PI_Art_Id);

            if (searchPhysicalInventoryModel.PI_DateAcquired.HasValue) physicalInventory = physicalInventory.Where(a => EntityFunctions.TruncateTime(a.PI_DateAcquired) == searchPhysicalInventoryModel.PI_DateAcquired);


            gridInfo.recordsTotal = physicalInventory.Select(a => a.PI_Id).Count();

            var physicalInventoryResult = physicalInventory.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
             .Skip(gridInfo.start)
             .Take(gridInfo.length)
             .ToList();

            foreach (var item in physicalInventoryResult)
            {
                retPhysicalInventory.Add(new PhysicalInventoryModel
                {
                    PI_Unit_Name = item.tblUnits.Unit_StationName,
                    PI_Id = item.PI_Id,
                    PI_Dir_Name = item.tblDirectorates.Dir_Name,
                    PI_Art_Name = item.tblArticles.Art_Name,
                    PI_IG_Name = item.tblInventoryGroups.IG_Description,
                    PI_Description = item.PI_Description,
                    PI_PropertyNumber = item.PI_PropertyNumber,
                    PI_UnitOfMeasure = item.PI_UnitOfMeasure,
                    PI_DateAcquired = item.PI_DateAcquired,
                    PI_UnitValue = item.PI_UnitValue,
                    PI_Office = item.PI_Office,
                    PI_ARENumber = item.PI_ARENumber,
                    PI_End_User = item.tblEmployees != null ? 
                                item.tblEmployees.tblRanks.Rank_Name + " " + item.tblEmployees.Emp_FirstName + " " + " " + item.tblEmployees.Emp_MiddleName.First() + " " +
                                item.tblEmployees.Emp_LastName + " " + item.tblEmployees.Emp_SuffixName : ""
                });
            }

            return new PhysicalInventoryListResult
            {
                PhysicalInventoryListModel = retPhysicalInventory,
                DatatableInfo = gridInfo
            };
        }


        public PhysicalInventoryListResult GetUnserviceablePIListResult(GridInfo gridInfo)
        {
            var unserviceablePhysicalInventory = from pi in BFPContext.tblPhysicalInventory
                              where pi.PI_UPI_Id == gridInfo.idValue
                              select new PhysicalInventoryModel
                              {
                                  PI_Id = pi.PI_Id,
                                  PI_Description = pi.PI_Description,
                                  PI_DateAcquired = pi.PI_DateAcquired,
                                  PI_UnitValue = pi.PI_UnitValue,
                                  PI_PropertyNumber = pi.PI_PropertyNumber
                              };

            gridInfo.recordsTotal = unserviceablePhysicalInventory.Select(a => a.PI_Id).Count();

            unserviceablePhysicalInventory = unserviceablePhysicalInventory.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
                .Skip(gridInfo.start)
                .Take(gridInfo.length);

            return new PhysicalInventoryListResult
            {
                PhysicalInventoryListModel = unserviceablePhysicalInventory.ToList(),
                DatatableInfo = gridInfo
            };

        }

        public void UpdatePhysicalInventory(PhysicalInventoryModel model)
        {
           
            var inventoryGroupDet =
                BFPContext.tblPhysicalInventory.FirstOrDefault(a => a.PI_Id == model.PI_Id);
            if (inventoryGroupDet == null) throw new Exception("Physical Inventory cannot be found!");

            model.PI_CreatedDate = inventoryGroupDet.PI_CreatedDate;
            model.PI_Created_Emp_Id = inventoryGroupDet.PI_Created_Emp_Id;
            model.PI_Unit_Id = inventoryGroupDet.PI_Unit_Id;
            Mapper.Map(model, inventoryGroupDet);
            inventoryGroupDet.PI_LastUpdateDate = DateTime.Now;
            inventoryGroupDet.PI_LastUpdate_Emp_Id = CurrentUser.EmployeeId;

            BFPContext.Entry(inventoryGroupDet).State = EntityState.Modified;
            BFPContext.SaveChanges();
        }


        public bool DeleteByID(int physicalInventoryId)
        {
            var physicalInventory =
              BFPContext.tblPhysicalInventory.FirstOrDefault(a => a.PI_Id == physicalInventoryId);
            if (physicalInventory == null) throw new Exception("Physical Inventory cannot be found!");

            physicalInventory.PI_IsDeleted = true;

            BFPContext.Entry(physicalInventory).State = EntityState.Modified;
            BFPContext.SaveChanges();
            return true;
        }

        public bool PhysicalInventoryExists(string propertyNumber, int id = 0)
        {

            var details = BFPContext.tblPhysicalInventory
             .Where(a => a.PI_PropertyNumber == propertyNumber);

            if (id > 0)
                details = details.Where(a => a.PI_Id != id);

            return details.Any();
        }

        public PhysicalInventoryModel GetPhysicalInventoryById(int physicalInventoryId)
        {
            var ret = new PhysicalInventoryModel();

            var physicalInventory = BFPContext.tblPhysicalInventory.FirstOrDefault(a => a.PI_Id == physicalInventoryId);

            if (physicalInventory != null)
            {
                Mapper.Map(physicalInventory, ret);       
            }
            return ret;
        }
    }
}