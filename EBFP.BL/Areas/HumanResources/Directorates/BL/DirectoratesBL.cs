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

namespace EBFP.BL.HumanResources
{
    public class DirectoratesBL : Repository<tblDirectorates, DirectoratesModel>, IDirectorates
    {
        public DirectoratesBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public DirectoratesListResult GetListResult(GridInfo gridInfo)
        {
            var directorates = from dir in BFPContext.tblDirectorates
                join cr in BFPContext.tblEmployees on dir.Dir_Created_Emp_Id equals cr.Emp_Id
                    into tempCr
                from cr in tempCr.DefaultIfEmpty()
                join md in BFPContext.tblEmployees on dir.Dir_LastUpdate_Emp_Id equals md.Emp_Id
                    into tempMd
                from md in tempMd.DefaultIfEmpty()
                select new DirectoratesModel
                {
                    Dir_Id = dir.Dir_Id,
                    Dir_Code = dir.Dir_Code,
                    Dir_Name = dir.Dir_Name,
                    Dir_CreatedDate = dir.Dir_CreatedDate,
                    Dir_LastUpdateDate = dir.Dir_LastUpdateDate,
                    Dir_CreatedBy =
                        cr.tblRanks.Rank_Name + " " + cr.Emp_FirstName + " " + " " + cr.Emp_MiddleName + " " +
                        cr.Emp_LastName + " " + cr.Emp_SuffixName,
                    Dir_LastUpdateBy =
                        md.tblRanks.Rank_Name + " " + md.Emp_FirstName + " " + md.Emp_MiddleName + " " + md.Emp_LastName +
                        " " + md.Emp_SuffixName
                };

            var searchDirectoratesModel = gridInfo.searchDirectoratesModel;
            if (!string.IsNullOrEmpty(searchDirectoratesModel.Dir_Code))
                directorates = directorates.Where(a => a.Dir_Code.Contains(searchDirectoratesModel.Dir_Code));
            if (!string.IsNullOrEmpty(searchDirectoratesModel.Dir_Name))
                directorates =
                    directorates.Where(a => a.Dir_Name.Contains(searchDirectoratesModel.Dir_Name));

            gridInfo.recordsTotal = directorates.Select(a => a.Dir_Id).Count();

            directorates = directorates.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
                .Skip(gridInfo.start)
                .Take(gridInfo.length);

            return new DirectoratesListResult
            {
                DirectoratesListModel = directorates.ToList(),
                DatatableInfo = gridInfo
            };
        }

        public void UpdateDirectorates(DirectoratesModel model)
        {
            var directoratesDet =
                BFPContext.tblDirectorates.FirstOrDefault(a => a.Dir_Id == model.Dir_Id);
            if (directoratesDet == null) throw new Exception("Directorates cannot be found!");

            directoratesDet.Dir_Code = model.Dir_Code;
            directoratesDet.Dir_Name = model.Dir_Name;
            directoratesDet.Dir_LastUpdate_Emp_Id = CurrentUser.EmployeeId;
            directoratesDet.Dir_LastUpdateDate = DateTime.Now;

            BFPContext.Entry(directoratesDet).State = EntityState.Modified;
            BFPContext.SaveChanges();
        }


        public bool DeleteByID(int dirId)
        {
            var dir = BFPContext.tblDirectorates
                .FirstOrDefault(a => a.Dir_Id == dirId);

            if (dir != null)
            {
                BFPContext.tblDirectorates.Remove(dir);
                BFPContext.SaveChanges();
            }
            return true;
        }

        public bool DirectorateExists(string dirCode, string dirName, int dirId = 0)
        {
            var details = BFPContext.tblDirectorates
                .Where(a => a.Dir_Code == dirCode && a.Dir_Name == dirName);

            if (dirId > 0)
                details = details.Where(a => a.Dir_Id != dirId);

            return details.Any();
        }

        public bool CheckIfCurrentlyUsed(int dirId)
        {
            var physicalInventory = BFPContext.tblPhysicalInventory
                .FirstOrDefault(a => a.PI_Dir_Id == dirId);

            return physicalInventory != null;
        }

        public DirectoratesModel GetDirectoratesById(int dirId)
        {
            var model = new DirectoratesModel();
            var dirRet = BFPContext.tblDirectorates.FirstOrDefault(a => a.Dir_Id == dirId);

            if (dirRet != null)
                Mapper.Map(dirRet,model);

            return model;
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblDirectorates, DirectoratesModel>().ReverseMap();
            Mapper.CreateMap<List<tblDirectorates>, List<DirectoratesModel>>().ReverseMap();
            Mapper.CreateMap<List<tblDirectorates>, List<DirectoratesModel>>();
        }
    }
}