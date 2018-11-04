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
    public class InventoryArticleBL : Repository<tblArticles, InventoryArticleModel>, IInventoryArticle
    {
        public InventoryArticleBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblArticles, InventoryArticleModel>().ReverseMap();
            Mapper.CreateMap<List<tblArticles>, List<InventoryArticleModel>>().ReverseMap();
            Mapper.CreateMap<List<tblArticles>, List<InventoryArticleModel>>();
        }

        public InventoryArticleListResult GetListResult(GridInfo gridInfo)
        {
            var inventoryGroups = from ia in BFPContext.tblArticles
                join cr in BFPContext.tblEmployees on ia.Art_Created_Emp_Id equals cr.Emp_Id
                    into tempCr
                from cr in tempCr.DefaultIfEmpty()
                join md in BFPContext.tblEmployees on ia.Art_LastUpdate_Emp_Id equals md.Emp_Id
                    into tempMd
                from md in tempMd.DefaultIfEmpty()
                select new InventoryArticleModel
                {
                    Art_Id = ia.Art_Id,
                    Art_Code = ia.Art_Code,
                    Art_Name = ia.Art_Name,
                    Art_CreatedDate = ia.Art_CreatedDate,
                    Art_LastUpdateDate = ia.Art_LastUpdateDate
                    //Art_CreatedBy =
                    //    cr.tblRanks.Rank_Name + " " + cr.Emp_FirstName + " " + " " + cr.Emp_MiddleName + " " +
                    //    cr.Emp_LastName + " " + cr.Emp_SuffixName,
                    //Art_LastUpdateBy =
                    //    md.tblRanks.Rank_Name + " " + md.Emp_FirstName + " " + md.Emp_MiddleName + " " + md.Emp_LastName +
                    //    " " + md.Emp_SuffixName

                        ,
                    CRRank = cr.tblRanks.Rank_Name,
                    CRFirstName = cr.Emp_FirstName,      
                    CRMiddleName = cr.Emp_MiddleName,
                    CRLastname = cr.Emp_LastName,
                    CRSuffixName = cr.Emp_SuffixName,


                    MDRank = md.tblRanks.Rank_Name,
                    MDFirstName = md.Emp_FirstName,
                    MDMiddleName = md.Emp_MiddleName,
                    MDLastname = md.Emp_LastName,
                    MDSuffixName = md.Emp_SuffixName
                };

            var searchInventoryArticleModel = gridInfo.searchInventoryArticleModel;
            if (!string.IsNullOrEmpty(searchInventoryArticleModel.Art_Code))
                inventoryGroups = inventoryGroups.Where(a => a.Art_Code.Contains(searchInventoryArticleModel.Art_Code));
            if (!string.IsNullOrEmpty(searchInventoryArticleModel.Art_Name))
                inventoryGroups = inventoryGroups.Where(a => a.Art_Name.Contains(searchInventoryArticleModel.Art_Name));

            gridInfo.recordsTotal = inventoryGroups.Select(a => a.Art_Id).Count();

            inventoryGroups = inventoryGroups.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
                .Skip(gridInfo.start)
                .Take(gridInfo.length);

            return new InventoryArticleListResult
            {
                InventoryArticleListModel = inventoryGroups.ToList(),
                DatatableInfo = gridInfo
            };
        }

        public void UpdateInventoryArticle(InventoryArticleModel model)
        {
            var inventoryArticleDet =
                BFPContext.tblArticles.FirstOrDefault(a => a.Art_Id == model.Art_Id);
            if (inventoryArticleDet == null) throw new Exception("Inventory Article cannot be found!");

            inventoryArticleDet.Art_Code = model.Art_Code;
            inventoryArticleDet.Art_Name = model.Art_Name;
            inventoryArticleDet.Art_LastUpdate_Emp_Id = CurrentUser.EmployeeId;
            inventoryArticleDet.Art_LastUpdateDate = DateTime.Now;

            BFPContext.Entry(inventoryArticleDet).State = EntityState.Modified;
            BFPContext.SaveChanges();
        }


        public bool DeleteByID(int inventoryArticleId)
        {
            var inventoryArticle = BFPContext.tblArticles
                .FirstOrDefault(a => a.Art_Id == inventoryArticleId);

            if (inventoryArticle != null)
            {
                BFPContext.tblArticles.Remove(inventoryArticle);
                BFPContext.SaveChanges();
            }
            return true;
        }

        public bool InventoryArticleExists(string artCode, string artName, int artId = 0)
        {
            var details = BFPContext.tblArticles
                .Where(a => a.Art_Code == artCode && a.Art_Name == artCode);

            if (artId > 0)
                details = details.Where(a => a.Art_Id != artId);

            return details.Any();
        }

        public bool CheckIfCurrentlyUsed(int iaId)
        {
            var physicalInventory = BFPContext.tblPhysicalInventory
                .FirstOrDefault(a => a.PI_Art_Id == iaId);

            return physicalInventory != null;
        }


        public InventoryArticleModel GetInventoryArticleById(int iaId)
        {
            var model = new InventoryArticleModel();
            var inventoryArticleRet = BFPContext.tblArticles.FirstOrDefault(a => a.Art_Id == iaId);

            if (inventoryArticleRet != null)
                Mapper.Map(inventoryArticleRet, model);

            return model;
        }
    }
}