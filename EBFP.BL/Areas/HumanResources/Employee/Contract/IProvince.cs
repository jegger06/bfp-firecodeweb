using System.Collections.Generic;
using EBFP.DataAccess;
using Queries.Core.Repositories;

namespace EBFP.BL.HumanResources
{
    public interface IProvince : IRepository<tblProvinces, ProvinceModel>
    {
        List<ProvinceModel> GetProvincePerRegion(int reg_Id);
        List<ProvinceModel> GetProvinces();
    }
}