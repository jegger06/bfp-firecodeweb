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
    public class UnitBL : Repository<tblUnits, UnitModel>, IUnit
    {
        public UnitBL(EBFPEntities context) : base(context)
        {
        }

        public UnitModel GetUnitById(int unit_Id)
        {
            IHRISUnitOfWork UnitOfWork = new HRISUnitOfWork(BFPContext);
            var ret = new UnitModel();

            var units = BFPContext.tblUnits.Join(BFPContext.tblRegions,
                    c => c.tblCityMunicipality.tblProvinces.Region_Id,
                    o => o.Reg_Id,
                    (c, o) => new
                    {
                        o.Reg_Description,
                        c.tblCityMunicipality.tblProvinces.Region_Id,
                        c.tblCityMunicipality.tblProvinces.Province_Name,
                        c.tblCityMunicipality.Municipality_Province_Id,
                        c.tblCityMunicipality.Municipality_Id,
                        c.tblCityMunicipality.Municipality_Name,
                        c.Unit_StationName,
                        c.Unit_Id,
                        c.Unit_Code,
                        c.Unit_Description,
                        c.Unit_PhoneNumber,
                        c.Unit_Address,
                        c.Unit_FireMarshall_Emp_Id,
                        c.Unit_Municipality_Id,
                        c.Unit_Category,
                        c.Unit_ChiefFSES_Emp_Id,
                        c.Unit_ChiefFSES_Signature,
                        c.Unit_FireMarshall_Signature,
                        Reg_Id =
                            BFPContext.tblRegions.Where(a => a.Reg_Id == c.tblCityMunicipality.tblProvinces.Region_Id)
                                .Select(a => a.Reg_Id)
                                .FirstOrDefault(),
                        UnitUserInRoleModel =
                            BFPContext.tblUnitsUserInRole.Where(a => a.Unit_UIR_Unit_Id == c.Unit_Id).ToList()
                    }).FirstOrDefault(a => a.Unit_Id == unit_Id);


            if (units != null)
            {
                ret.Reg_Description = units.Reg_Description;
                ret.CityMunicipality_Name = units.Municipality_Name;
                ret.Province_Name = units.Province_Name;
                ret.Unit_StationName = units.Unit_StationName;
                ret.Unit_Id = units.Unit_Id;
                ret.Unit_Code = units.Unit_Code;
                ret.Unit_Description = units.Unit_Description;
                ret.Unit_PhoneNumber = units.Unit_PhoneNumber;
                ret.Unit_Address = units.Unit_Address;
                ret.Unit_ProvDistrict = units.Municipality_Province_Id;
                ret.Unit_Municipality_Id = units.Municipality_Id;
                ret.Unit_Reg_Id = units.Reg_Id;
                ret.Unit_FireMarshall_Emp_Id = units.Unit_FireMarshall_Emp_Id;
                ret.Unit_Category = units.Unit_Category;
                ret.Unit_ChiefFSES_Emp_Id = units.Unit_ChiefFSES_Emp_Id;
                ret.Unit_ChiefFSES_Signature = units.Unit_ChiefFSES_Signature;
                ret.Unit_FireMarshall_Signature = units.Unit_FireMarshall_Signature;
                ret.UnitUserInRoleModel = UnitOfWork.UnitUserInRole.GetList(a => a.Unit_UIR_Unit_Id == units.Unit_Id,
                    q => q.OrderBy(d => d.Unit_UIR_Collector));
            }
            return ret;
        }

        public UnitListResult GetListResult(GridInfo gridInfo)
        {
            var ret = new List<UnitModel>();

            var units = BFPContext.tblUnits.Select(
                c => new UnitModel
                {
                    Unit_Reg_Id = c.tblCityMunicipality.tblProvinces.Region_Id,
                    Reg_Description = c.tblCityMunicipality.tblProvinces.tblRegions.Reg_Description,
                    Unit_ProvDistrict = c.tblCityMunicipality.Municipality_Province_Id,
                    Province_Name = c.tblCityMunicipality.tblProvinces.Province_Name,
                    Unit_StationName = c.Unit_StationName,
                    Unit_Id = c.Unit_Id,
                    Unit_Code = c.Unit_Code,
                    Unit_Municipality_Id = c.Unit_Municipality_Id,
                    CityMunicipality_Name = c.tblCityMunicipality.Municipality_Name,
                    Unit_Category = c.Unit_Category,
                    Unit_FireMarshall_Emp_Id = c.Unit_FireMarshall_Emp_Id
                });

            if (!PageSecurity.HasAccess(PageArea.HRIS_Unit_CanViewAll))
            {
                if (PageSecurity.HasAccess(PageArea.HRIS_Unit_RestricttoRegion))
                {
                    units = units.Where(a => a.Unit_Reg_Id == CurrentUser.RegionID);
                }

                if (PageSecurity.HasAccess(PageArea.HRIS_Unit_RestricttoProvince))
                {
                    units = units.Where(a => a.Unit_ProvDistrict == CurrentUser.ProvinceID);
                }
            }

            var searchUnitModel = gridInfo.searchUnitModel;
            if (!string.IsNullOrEmpty(searchUnitModel.UnitCode))
                units = units.Where(a => a.Unit_Code.Contains(searchUnitModel.UnitCode));
            if (!string.IsNullOrEmpty(searchUnitModel.StationName))
                units = units.Where(a => a.Unit_StationName.Contains(searchUnitModel.StationName));
            if (!string.IsNullOrEmpty(searchUnitModel.UnitDescription))
                units = units.Where(a => a.Unit_Description.Contains(searchUnitModel.UnitDescription));
            if (searchUnitModel.RegionId > 0)
                units = units.Where(a => a.Unit_Reg_Id == searchUnitModel.RegionId);
            if (searchUnitModel.MunicipalityId > 0)
                units = units.Where(a => a.Unit_Municipality_Id == searchUnitModel.MunicipalityId);
            if (searchUnitModel.ProvinceId > 0)
                units = units.Where(a => a.Unit_ProvDistrict == searchUnitModel.ProvinceId);
            if (searchUnitModel.UnitId > 0)
                units = units.Where(a => a.Unit_Id == searchUnitModel.UnitId);
            if (searchUnitModel.UnitCategory > 0)
                units = units.Where(a => a.Unit_Category == searchUnitModel.UnitCategory);
            if (searchUnitModel.FireMashallId > 0)
                units = units.Where(a => a.Unit_FireMarshall_Emp_Id == searchUnitModel.FireMashallId);


            gridInfo.recordsTotal = units.Select(a => a.Unit_Id).Count();

            units = units.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
                .Skip(gridInfo.start)
                .Take(gridInfo.length);

            ret = units.ToList();

            return new UnitListResult
            {
                UnitList = ret,
                DatatableInfo = gridInfo
            };
        }

        public List<SelectionUnitModel> GetAllForSelection()
        {
            var ret = new List<SelectionUnitModel>();

            var Regions = BFPContext.tblRegions
                .OrderBy(a => a.Reg_Id)
                .ToList();

            var units = BFPContext.tblUnits
                .Select(a => new
                {
                    a.tblCityMunicipality.Municipality_Id,
                    a.tblCityMunicipality.Municipality_Name,
                    a.tblCityMunicipality.tblProvinces.Region_Id,
                    a.tblCityMunicipality.tblProvinces.Province_Name,
                    a.tblCityMunicipality.Municipality_Province_Id,
                    a.Unit_StationName,
                    a.Unit_Id,
                    a.Unit_Code
                }).ToList();

            foreach (var region in Regions)
            {
                var unitModel = new SelectionUnitModel();
                unitModel.Reg_Id = region.Reg_Id;
                unitModel.Reg_Description = region.Reg_Description;
                foreach (var unit in units.Where(a => a.Region_Id == region.Reg_Id))
                {
                    unitModel.Units.Add(new UnitModel
                    {
                        Province_Name = unit.Province_Name,
                        Unit_StationName = unit.Unit_StationName,
                        Unit_Id = unit.Unit_Id,
                        Unit_Code = unit.Unit_Code,
                        Unit_Municipality_Id = unit.Municipality_Id,
                        CityMunicipality_Name = unit.Municipality_Name,
                        Unit_ProvDistrict = unit.Municipality_Province_Id,
                        Unit_Reg_Id = unit.Region_Id,

                    });
                }
                ret.Add(unitModel);
            }

            return ret;
        }

        public void UpdateUnit(UnitModel model)
        {
            IHRISUnitOfWork UnitOfWork = new HRISUnitOfWork(BFPContext);

            var unitDet = BFPContext.tblUnits.FirstOrDefault(a => a.Unit_Id == model.Unit_Id);
            if (unitDet == null) throw new Exception("Unit cannot be found!");

            unitDet.Unit_Code = model.Unit_Code;
            unitDet.Unit_Address = model.Unit_Address;
            unitDet.Unit_PhoneNumber = model.Unit_PhoneNumber;
            unitDet.Unit_FireMarshall_Emp_Id = model.Unit_FireMarshall_Emp_Id;
            unitDet.Unit_Description = model.Unit_Description;
            unitDet.Unit_StationName = model.Unit_StationName;
            //unitDet.Unit_ProvDistrict = model.Unit_ProvDistrict;
            unitDet.Unit_Municipality_Id = model.Unit_Municipality_Id;
            unitDet.Unit_FireMarshall_Emp_Id = model.Unit_FireMarshall_Emp_Id;
            unitDet.Unit_Category = model.Unit_Category;
            unitDet.Unit_ChiefFSES_Emp_Id = model.Unit_ChiefFSES_Emp_Id;
            if(model.Unit_ChiefFSES_Signature != null)
                unitDet.Unit_ChiefFSES_Signature = model.Unit_ChiefFSES_Signature;
            if (model.Unit_FireMarshall_Signature != null)
                unitDet.Unit_FireMarshall_Signature = model.Unit_FireMarshall_Signature;

            BFPContext.Entry(unitDet).State = EntityState.Modified;

            UnitOfWork.UnitUserInRole.InsertBulk(model.UnitUserInRoleModel, a => a.Unit_UIR_Unit_Id == model.Unit_Id);

            if (model.UnitUserInRoleModel != null)
            {
                foreach (var item in model.UnitUserInRoleModel)
                    item.Unit_UIR_Unit_Id = model.Unit_Id;
            }

            BFPContext.SaveChanges();
        }

        public bool DeleteByID(int unitId)
        {
            var unit = BFPContext.tblUnits
                .FirstOrDefault(a => a.Unit_Id == unitId);

            BFPContext.tblUnits.Remove(unit);
            BFPContext.SaveChanges();

            return true;
        }
        
        public List<UnitModel> GetUnitByMunicipality(int municipalityId)
        {
            var list = new List<UnitModel>();
            if (municipalityId > 0)
            {
                var ret = BFPContext.tblUnits.Where(a => a.Unit_Municipality_Id == municipalityId); 
                foreach (var item in ret)
                {
                    var model = new UnitModel
                    {
                        Unit_Id = item.Unit_Id,
                        Unit_StationName = item.Unit_StationName.Trim()
                    };
                    list.Add(model);
                }
            }

            return list.OrderBy(a => a.Unit_StationName).ToList();
        }

        public List<UnitModel> GetUnitByProvince(int provinceId)
        {
            var list = new List<UnitModel>();
            var ret = BFPContext.tblUnits.Where(a => a.tblCityMunicipality.Municipality_Province_Id == provinceId);

            foreach (var item in ret)
            {
                var model = new UnitModel
                {
                    Unit_Id = item.Unit_Id,
                    Unit_StationName = item.Unit_StationName.Trim()
                };
                list.Add(model);
            }


            return list.OrderBy(a => a.Unit_StationName).ToList();
        }

        public bool CheckUnit(int unitId)
        {
            var employee = BFPContext.tblEmployees.FirstOrDefault(a => a.Emp_Curr_Unit == unitId);
            if (employee != null)
                return true;
            else
                return false;
        }
    }
}