using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.DataAccess;
using EBFP.Helper;
using Queries.Core.Repositories;
using System.Linq.Dynamic;

namespace EBFP.BL.CIS
{
    public class InventoryOutSuppliesBL : Repository<tblSuppliesInventoryOut, InventoryOutSuppliesModel>, IInventoryOutSupplies
    {
        public InventoryOutSuppliesBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblSuppliesInventoryOut, InventoryOutSuppliesModel>();
            Mapper.CreateMap<tblSuppliesInventoryOut, InventoryOutSuppliesModel>().ReverseMap();
        }

        public InventoryOutSuppliesListResult GetListResult(GridInfo gridInfo)
        {
            var outSupplies = from sio in BFPContext.tblSuppliesInventoryOut
                              join si in BFPContext.tblSuppliesInventory on sio.SI_Id equals si.SI_Id
                              join emp in BFPContext.tblEmployees on sio.SIO_Emp_Id equals emp.Emp_Id
                              join rank in BFPContext.tblRanks on emp.Emp_Curr_Rank equals rank.Rank_Id
                              into temp from rank in temp.DefaultIfEmpty()
                              where sio.SI_Id == gridInfo.idValue
                              select new InventoryOutSuppliesModel
                              {
                                  SI_Id = si.SI_Id,
                                  SIO_Id = sio.SIO_Id,
                                  SIO_Emp_Id = sio.SIO_Emp_Id,
                                  SIO_Emp_Name = (rank != null ? rank.Rank_Name : "") + " " + emp.Emp_FirstName + " " + emp.Emp_LastName,
                                  SIO_QuantityOut = sio.SIO_QuantityOut,
                                  SIO_OutDate = sio.SIO_OutDate,
                                  SIO_Remarks = sio.SIO_Remarks
                              };

           

            gridInfo.recordsTotal = outSupplies.Select(a => a.SI_Id).Count();

            outSupplies = outSupplies.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
                .Skip(gridInfo.start)
                .Take(gridInfo.length);

            return new InventoryOutSuppliesListResult
            {
                InventoryOutSuppliesModel = outSupplies.ToList(),
                DatatableInfo = gridInfo
            };
            
        }

        public void SaveOutSuppliesInventory(InventoryOutSuppliesModel model)
        {
            var details = new tblSuppliesInventoryOut();

            if(model.SIO_Id > 0)
                details = BFPContext.tblSuppliesInventoryOut.FirstOrDefault(a => a.SIO_Id == model.SIO_Id);

            if (details != null)
            {
                model.SIO_CreatedDate = details.SIO_CreatedDate;
                model.SIO_Created_Emp_Id = details.SIO_Created_Emp_Id;
                Mapper.Map(model, details);
                if (model.SIO_Id > 0)
                {
                    details.SIO_LastUpdate_Emp_Id = CurrentUser.EmployeeId;
                    details.SIO_LastUpdateDate = DateTime.Now;

                    BFPContext.Entry(details).State = EntityState.Modified;
                }
                else
                {
                    details.SIO_Created_Emp_Id = CurrentUser.EmployeeId;
                    details.SIO_CreatedDate = DateTime.Now;
                    BFPContext.tblSuppliesInventoryOut.Add(details);
                }
                BFPContext.SaveChanges();
            }
          
        }


        public bool DeleteByID(int sIO_Id)
        {
            var details = BFPContext.tblSuppliesInventoryOut
                .FirstOrDefault(a => a.SIO_Id == sIO_Id);

            if (details != null)
            {
                BFPContext.tblSuppliesInventoryOut.Remove(details);
                BFPContext.SaveChanges();
            }
            return true;
        }
    }
}