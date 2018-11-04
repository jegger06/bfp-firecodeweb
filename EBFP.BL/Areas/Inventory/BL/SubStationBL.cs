using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.DataAccess;
using EBFP.Helper;
using Queries.Core.Repositories;
using EBFP.BL.HumanResources;

namespace EBFP.BL.Inventory
{
    public class SubStationBL : Repository<tblUnitSubStation, SubStationModel>, ISubStation
    {
        public SubStationBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }
        public void CreateMapping()
        {
            Mapper.CreateMap<tblUnitSubStation, SubStationModel>();
            Mapper.CreateMap<tblUnitSubStation, SubStationModel>().ReverseMap();
        }

        public SubStationListResult GetListResult(GridInfo gridInfo)
        {
            var subStation = BFPContext.tblUnitSubStation
                .Where(a => a.tblUnits.Unit_Municipality_Id == gridInfo.searchSubStationModel.Municipality_Id)
                .Select(a => new SubStationModel
                {
                    Sub_Id = a.Sub_Id,
                    Sub_Unit_Id = a.Sub_Unit_Id,
                    Sub_Station_Code = a.Sub_Station_Code,
                    Sub_Station_Name = a.Sub_Station_Name,
                    Sub_BuildingStatus = a.Sub_BuildingStatus,
                    Sub_BuildingOwner = a.Sub_BuildingOwner,
                    Sub_LotOwner = a.Sub_LotOwner,
                    Sub_LotStatus = a.Sub_LotStatus,
                    Sub_Unit_Name = a.tblUnits.Unit_StationName
                });

            var search = gridInfo.searchSubStationModel ?? new SubStationSearchModel();
            if (!string.IsNullOrEmpty(search.Sub_Station_Code))
                subStation = subStation.Where(a => a.Sub_Station_Code.Contains(search.Sub_Station_Code));
            if (!string.IsNullOrEmpty(search.Sub_Station_Name))
                subStation = subStation.Where(a => a.Sub_Station_Name.Contains(search.Sub_Station_Name));
            if (search.Sub_Unit_Id > 0)
                subStation = subStation.Where(a => a.Sub_Unit_Id == search.Sub_Unit_Id);
            if (search.Sub_BuildingStatus > 0)
                subStation = subStation.Where(a => a.Sub_BuildingStatus == search.Sub_BuildingStatus);
            if (search.Sub_BuildingOwner > 0)
                subStation = subStation.Where(a => a.Sub_BuildingOwner == search.Sub_BuildingOwner);
            if (search.Sub_LotStatus > 0)
                subStation = subStation.Where(a => a.Sub_LotStatus == search.Sub_LotStatus);
            if (search.Sub_LotOwner > 0)
                subStation = subStation.Where(a => a.Sub_LotOwner == search.Sub_LotOwner);

            //total
            gridInfo.recordsTotal = subStation.Select(a => a.Sub_Unit_Id).Count();

            subStation = subStation.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
                .Skip(gridInfo.start)
                .Take(gridInfo.length);

            return new SubStationListResult
            {
                SubStationList = subStation.ToList(),
                DatatableInfo = gridInfo
            };
        }

        public List<SubStationModel> GetSubStationByStation(int unitId)
        {
            var subStationList = new List<SubStationModel>();
            var ret = BFPContext.tblUnitSubStation.Where(a => a.Sub_Unit_Id == unitId);

            foreach (var item in ret)
            {
                var model = new SubStationModel
                {
                    Sub_Station_Name = item.Sub_Station_Name.Trim(),
                    Sub_Id = item.Sub_Id
                };
                subStationList.Add(model);
            }

            return subStationList.OrderBy(a => a.Sub_Station_Name).ToList(); ;
        }

        public SubStationModel GetSubStationById(int subStationId)
        {
            var subStationDetails = new SubStationModel();
            var ret = BFPContext.tblUnitSubStation.FirstOrDefault(a => a.Sub_Id == subStationId);

            if(ret != null)
            {
                ret.Sub_WithBuilding = ret.Sub_WithBuilding == null ? false : ret.Sub_WithBuilding.Value;
                Mapper.Map(ret, subStationDetails);
                subStationDetails.RegionId = ret.tblUnits?.tblCityMunicipality?.tblProvinces?.Region_Id ?? 0;
                subStationDetails.ProvinceId = ret.tblUnits?.tblCityMunicipality?.Municipality_Province_Id ?? 0;
                subStationDetails.MunicipalityId = ret.tblUnits?.Unit_Municipality_Id ?? 0;
                subStationDetails.MunicipalityName = ret.tblUnits?.tblCityMunicipality?.Municipality_Name;
                subStationDetails.NSCB = ret.tblUnits?.tblCityMunicipality?.Municipality_NSCB;

                subStationDetails.SubStationEmployees = new List<SubStationEmployeeModel>();
                var employees = BFPContext.tblEmployees.Where(a => a.Emp_Curr_Unit == ret.Sub_Unit_Id && a.Emp_SubStation_Id == ret.Sub_Id);
                foreach(var emp in employees.OrderByDescending(a => a.tblRanks.Rank_Id))
                {
                    subStationDetails.SubStationEmployees.Add(new SubStationEmployeeModel
                    {
                        Unit_Id = ret.Sub_Unit_Id,
                        SubStation_Id = ret.Sub_Id,
                        Emp_Id = emp.Emp_Id
                    });
                }
            }

            return subStationDetails;
        }

        public SubStationModel GetSubStationByUnitId(int unitId)
        {
            var subStationDetails = new SubStationModel();
            var ret = BFPContext.tblUnits.FirstOrDefault(a => a.Unit_Id == unitId);

            if (ret != null)
            {
                subStationDetails.RegionId = ret.tblCityMunicipality?.tblProvinces?.Region_Id ?? 0;
                subStationDetails.ProvinceId = ret.tblCityMunicipality?.Municipality_Province_Id ?? 0;
                subStationDetails.MunicipalityId = ret.Unit_Municipality_Id;
                subStationDetails.MunicipalityName = ret.tblCityMunicipality?.Municipality_Name;
                subStationDetails.NSCB = ret.tblCityMunicipality?.Municipality_NSCB;
            }

            return subStationDetails;
        }

        public SelectionFireMarshallModel GetFireMarshall(int EmpId)
        {
            var model = new SelectionFireMarshallModel();
            var ret = from a in BFPContext.tblEmployees
                      where a.Emp_Id == EmpId
                      orderby a.Emp_FirstName ascending
                      select new
                {
                    a.Emp_Id,
                    a.Emp_FirstName,
                    a.Emp_LastName,
                    a.Emp_MiddleName,
                    a.Emp_SuffixName,
                    a.Emp_Curr_JobFunc,
                    a.Emp_MobileNumber,
                    //a.Emp_Curr_PosDesignationTitle

                };

            var res = ret.FirstOrDefault();
            if (res != null)
            {
                var unitOfWork = new HRISUnitOfWork();
                model.FireMarshall_Fullname = res.Emp_FirstName + " " + res.Emp_MiddleName + " " + res.Emp_LastName;
                model.FireMarshall_Fullname = model.FireMarshall_Fullname.FirstCharToUpper();
                model.FireMarshall_Id = res.Emp_Id.ToString();
                model.FireMarshall_ContactNumber = res.Emp_MobileNumber;
                model.FireMarshall_Position = res.Emp_Curr_JobFunc?.ToString();
                //model.FireMarshall_Designation = res.Emp_Curr_PosDesignationTitle;
                model.FireMarshall_Designation = unitOfWork.Employee.GetPositionTitle(res.Emp_Id);
            }

            return model;
        }

        public SubStationModel SaveSubStationDetails(SubStationModel model)
        {
            try
            {
                var subStation = new tblUnitSubStation();
                if (model.Sub_Id > 0)
                {
                    subStation = BFPContext.tblUnitSubStation
                        .FirstOrDefault(a => a.Sub_Id == model.Sub_Id);

                    Mapper.Map(model, subStation);
                }
                else
                {
                    Mapper.Map(model, subStation);
                    BFPContext.tblUnitSubStation.Add(subStation);
            
                }

                BFPContext.SaveChanges();
                model.Sub_Id = subStation?.Sub_Id ?? 0;

                if(model.Sub_Unit_Id > 0 && model.SubStationEmployees.Count > 0)
                {
                    var filtered = model.SubStationEmployees.Where(a => a.Emp_Id > 0);
                    foreach (var emp in filtered)
                    {
                        if (emp.SubStation_Id > 0)
                        {
                            var employee = BFPContext.tblEmployees.FirstOrDefault(a => a.Emp_Id == emp.Emp_Id);
                            if (employee != null)
                            {
                                if (emp.toDelete)
                                {
                                    employee.Emp_SubStation_Id = null;
                                }
                                else
                                {
                                    employee.Emp_SubStation_Id = emp.SubStation_Id;
                                }
                            }
                        }
                    }



                    BFPContext.SaveChanges();
                }

                return model;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
       
        }

        public bool DeleteByID(int subStationId)
        {
            var subStation = BFPContext.tblUnitSubStation
                .FirstOrDefault(a => a.Sub_Id == subStationId);

            if (subStation != null)
            {
                BFPContext.tblUnitSubStation.Remove(subStation);
                BFPContext.SaveChanges();
            }

            return true;
        }
    }
}