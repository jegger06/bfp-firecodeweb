using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Linq;
using AutoMapper;
using EBFP.BL.Helper;

namespace EBFP.BL.HumanResources
{
    using EBFP.DataAccess;
    using EBFP.Helper;
    using Queries.Core.Repositories;

    public class ProvinceBL : Repository<tblProvinces, ProvinceModel>, IProvince
    {
        protected EBFPEntities context_;
        protected EBFPEntities context
        {
            get
            {
                if (context_ == null)
                    context_ = new EBFPEntities();

                return context_;
            }
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblProvinces, ProvinceModel>();
            Mapper.CreateMap<List<tblProvinces>, List<ProvinceModel>>();
        }
        public ProvinceBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public List<ProvinceModel> GetProvinces()
        {
            var provinceList = new List<ProvinceModel>();
            var ret = context.tblProvinces.ToList();

            foreach (var item in ret)
            {
                var model = new ProvinceModel
                {
                    Province_Name = item.Province_Name,
                    Province_Id = item.Province_Id,
                    Region_Id = item.Region_Id
                    
                };
                provinceList.Add(model);
            }
            
            return provinceList;
        }

        public List<ProvinceModel> GetProvincePerRegion(int reg_Id)
        {
            var provinceList = new List<ProvinceModel>();
            var ret = context.tblProvinces.Where(a => a.Region_Id == reg_Id);
            foreach (var item in ret.ToList())
            {
                var model = new ProvinceModel
                {
                    Province_Name = item.Province_Name.Trim(),
                    Province_Id = item.Province_Id
                };
                provinceList.Add(model);
            }

            return provinceList.OrderBy(a => a.Province_Name).ToList(); ;
        }
    } 
}
