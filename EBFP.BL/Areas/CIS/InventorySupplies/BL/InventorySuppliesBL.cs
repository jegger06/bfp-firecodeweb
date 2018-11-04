using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using EBFP.BL.CIS;
using EBFP.BL.Helper;
using EBFP.DataAccess;
using EBFP.Helper;
using Queries.Core.Repositories;

namespace EBFP.BL.CIS
{
    public class InventorySuppliesBL : Repository<tblSuppliesInventory, InventorySuppliesModel>, IInventorySupplies
    {
        public InventorySuppliesBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblSuppliesInventory, InventorySuppliesModel>();
            Mapper.CreateMap<tblSuppliesInventory, InventorySuppliesModel>().ReverseMap();
        }

        public InventorySuppliesListResult GetListResult(GridInfo gridInfo)
        {
            var supplies = from si in BFPContext.tblSuppliesInventory
                           join art in BFPContext.tblArticles on si.SI_Art_Id equals art.Art_Id
                           where si.SI_IsDeleted == false && si.SI_Unit_Id == CurrentUser.EmployeeUnitId
                           select new InventorySuppliesModel
                {
                    SI_Id = si.SI_Id,
                    SI_Art_Id = si.SI_Art_Id,
                    SI_Art_Name = art.Art_Name,
                    SI_Description = si.SI_Description,
                    SI_StockNumber = si.SI_StockNumber,
                    SI_UnitOfMeasure = si.SI_UnitOfMeasure,
                    SI_UnitValue = si.SI_UnitValue,
                    SI_Quantity = si.SI_Quantity,
                    SI_DateAcquired = si.SI_DateAcquired,
                    SI_OnHand = (si.SI_Quantity ?? 0) > 0  ?
                        si.SI_Quantity.Value - (si.tblSuppliesInventoryOut.Count > 0 ? si.tblSuppliesInventoryOut.Sum(a => a.SIO_QuantityOut) : 0) :
                        0,
                    SI_TotalAmount = ((si.SI_UnitValue ?? 0) * (si.SI_Quantity ?? 0))
                };

            var searchSuppliesInventory = gridInfo.searchSuppliesInventory;
            if (searchSuppliesInventory.SI_WithOnHand)
                supplies = supplies.Where(a => a.SI_OnHand > 0);
            if (!string.IsNullOrEmpty(searchSuppliesInventory.SI_Art_Name))
                supplies = supplies.Where(a => a.SI_Art_Name.Contains(searchSuppliesInventory.SI_Art_Name));
            if (!string.IsNullOrEmpty(searchSuppliesInventory.SI_Description))
                supplies = supplies.Where(a => a.SI_Description.Contains(searchSuppliesInventory.SI_Description));

            gridInfo.recordsTotal = supplies.Select(a => a.SI_Id).Count();

            supplies = supplies.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
                .Skip(gridInfo.start)
                .Take(gridInfo.length);

            return new InventorySuppliesListResult
            {
                InventorySuppliesModel = supplies.ToList(),
                DatatableInfo = gridInfo
            };
        }

        public void SaveSuppliesInventory(InventorySuppliesModel model)
        {
            var details = new tblSuppliesInventory();

            if(model.SI_Id > 0)
                details = BFPContext.tblSuppliesInventory.FirstOrDefault(a => a.SI_Id == model.SI_Id);

            if (details != null)
            {
                model.SI_CreatedDate = details.SI_CreatedDate;
                model.SI_Created_Emp_Id = details.SI_Created_Emp_Id;
                model.SI_IsDeleted = details.SI_IsDeleted;
                Mapper.Map(model, details);
                details.SI_Unit_Id = CurrentUser.EmployeeUnitId;
                if (model.SI_Id > 0)
                {
                    details.SI_LastUpdate_Emp_Id = CurrentUser.EmployeeId;
                    details.SI_LastUpdateDate = DateTime.Now;

                    BFPContext.Entry(details).State = EntityState.Modified;
                }
                else
                {
                    details.SI_Created_Emp_Id = CurrentUser.EmployeeId;
                    details.SI_CreatedDate = DateTime.Now;
                    BFPContext.tblSuppliesInventory.Add(details);
                }
                BFPContext.SaveChanges();
            }
          
        }


        public bool DeleteByID(int suppliesInventoryId)
        {
            var details = BFPContext.tblSuppliesInventory
                .FirstOrDefault(a => a.SI_Id == suppliesInventoryId);

            if (details != null)
            {
                details.SI_IsDeleted = true;
                BFPContext.Entry(details).State = EntityState.Modified;
                BFPContext.SaveChanges();
            }
            return true;
        }
    }
}