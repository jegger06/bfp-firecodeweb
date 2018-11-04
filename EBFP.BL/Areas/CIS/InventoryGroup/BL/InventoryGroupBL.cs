using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.BL.HumanResources;
using EBFP.DataAccess;
using EBFP.Helper;
using Queries.Core.Repositories;

namespace EBFP.BL.CIS
{
    public class InventoryGroupBL : Repository<tblInventoryGroups, InventoryGroupModel>, IInventoryGroup
    {
        public InventoryGroupBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblInventoryGroups, InventoryGroupModel>().ReverseMap();
            Mapper.CreateMap<List<tblInventoryGroups>, List<InventoryGroupModel>>().ReverseMap();
            Mapper.CreateMap<List<tblInventoryGroups>, List<InventoryGroupModel>>();
        }

        public InventoryGroupListResult GetListResult(GridInfo gridInfo)
        {
            var inventoryGroups = from ig in BFPContext.tblInventoryGroups
                join cr in BFPContext.tblEmployees on ig.IG_Created_Emp_Id equals cr.Emp_Id
                    into tempCr
                from cr in tempCr.DefaultIfEmpty()
                join md in BFPContext.tblEmployees on ig.IG_LastUpdate_Emp_Id equals md.Emp_Id
                    into tempMd
                from md in tempMd.DefaultIfEmpty()
                select new InventoryGroupModel
                {
                    IG_Id = ig.IG_Id,
                    IG_Code = ig.IG_Code,
                    IG_Description = ig.IG_Description,
                    IG_CreatedDate = ig.IG_CreatedDate,
                    IG_LastUpdateDate = ig.IG_LastUpdateDate,
                    IG_CreatedBy =
                        cr.tblRanks.Rank_Name + " " + cr.Emp_FirstName + " " + " " + cr.Emp_MiddleName + " " +
                        cr.Emp_LastName + " " + cr.Emp_SuffixName,
                    IG_LastUpdateBy =
                        md.tblRanks.Rank_Name + " " + md.Emp_FirstName + " " + md.Emp_MiddleName + " " + md.Emp_LastName +
                        " " + md.Emp_SuffixName
                };

            var searchInventoryGroupModel = gridInfo.searchInventoryGroupModel;
            if (!string.IsNullOrEmpty(searchInventoryGroupModel.IG_Code))
                inventoryGroups = inventoryGroups.Where(a => a.IG_Code.Contains(searchInventoryGroupModel.IG_Code));
            if (!string.IsNullOrEmpty(searchInventoryGroupModel.IG_Description))
                inventoryGroups =
                    inventoryGroups.Where(a => a.IG_Description.Contains(searchInventoryGroupModel.IG_Description));

            gridInfo.recordsTotal = inventoryGroups.Select(a => a.IG_Id).Count();

            inventoryGroups = inventoryGroups.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
                .Skip(gridInfo.start)
                .Take(gridInfo.length);

            return new InventoryGroupListResult
            {
                InventoryGroupListModel = inventoryGroups.ToList(),
                DatatableInfo = gridInfo
            };
        }

        public void UpdateInventoryGroup(InventoryGroupModel model)
        {
            var inventoryGroupDet =
                BFPContext.tblInventoryGroups.FirstOrDefault(a => a.IG_Id == model.IG_Id);
            if (inventoryGroupDet == null) throw new Exception("Inventory Group cannot be found!");

            inventoryGroupDet.IG_Code = model.IG_Code;
            inventoryGroupDet.IG_Description = model.IG_Description;
            inventoryGroupDet.IG_LastUpdate_Emp_Id = CurrentUser.EmployeeId;
            inventoryGroupDet.IG_LastUpdateDate = DateTime.Now;

            BFPContext.Entry(inventoryGroupDet).State = EntityState.Modified;
            BFPContext.SaveChanges();
        }


        public bool DeleteByID(int inventoryGroupId)
        {
            var inventoryGroup = BFPContext.tblInventoryGroups
                .FirstOrDefault(a => a.IG_Id == inventoryGroupId);

            if (inventoryGroup != null)
            {
                BFPContext.tblInventoryGroups.Remove(inventoryGroup);
                BFPContext.SaveChanges();
            }
            return true;
        }

        public bool InventoryGroupExists(string igCode, string igDescription, int id = 0)
        {
            var details = BFPContext.tblInventoryGroups
             .Where(a => a.IG_Code == igCode && a.IG_Description == igDescription);

            if (id > 0)
                details = details.Where(a => a.IG_Id != id);

            return details.Any();
        }

        public bool CheckIfCurrentlyUsed(int igId)
        {
            var physicalInventory = BFPContext.tblPhysicalInventory
                .FirstOrDefault(a => a.PI_IG_Id == igId);

            return physicalInventory != null;
        }


        public InventoryGroupModel GetInventoryGroupById(int igId)
        {
            var model = new InventoryGroupModel();
            var inventoryGroupRet = BFPContext.tblInventoryGroups.FirstOrDefault(a => a.IG_Id == igId);

            if (inventoryGroupRet != null)
                Mapper.Map(inventoryGroupRet, model);

            return model;
        }
    }
}