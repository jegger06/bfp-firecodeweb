using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.DataAccess;
using EBFP.Helper;
using Queries.Core.Repositories;

namespace EBFP.BL.InspectionOrder
{
    public class InspectionOrderBL : Repository<tblInspectionOrders, InspectionOrderModel>, IInspectionOrder
    {
        public InspectionOrderBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }
        public void CreateMapping()
        {
            Mapper.CreateMap<tblInspectionOrders, InspectionOrderModel>().ReverseMap();
            Mapper.CreateMap<List<tblInspectionOrders>, List<InspectionOrderModel>>().ReverseMap();
            Mapper.CreateMap<List<tblInspectionOrders>, List<InspectionOrderModel>>();
        }

        public void SyncInspectionOrderLocalToServer(List<InspectionOrderModel> inspectionOrder)
        {

            foreach (var inspection in inspectionOrder)
            {
                var InspectionOrder = BFPContext.tblInspectionOrders
                                            .FirstOrDefault(a => a.Ref_IO_Id == inspection.Ref_IO_Id && a.IO_Unit_Id == inspection.IO_Unit_Id);

                if (InspectionOrder == null)
                {
                    InspectionOrder = new tblInspectionOrders();
                    Mapper.Map(inspection, InspectionOrder);
                    BFPContext.tblInspectionOrders.Add(InspectionOrder);
                }
                else
                {
                    var ioId = InspectionOrder.IO_Id;
                    Mapper.Map(inspection, InspectionOrder);
                    InspectionOrder.IO_Id = ioId;
                }
            }

            BFPContext.SaveChanges();
        }

        public InspectionOrderListResult GetListResult(GridInfo gridInfo)
        {
            var retIO = new List<InspectionOrderModel>();
            var searchTerms = gridInfo.searchInspectionOrderModel;

            var inspectionOrder = BFPContext.tblInspectionOrders.AsQueryable();

            if (!PageSecurity.HasAccess(PageArea.FPSS_InspectionOrder_CanViewAll))
            {
                if (PageSecurity.HasAccess(PageArea.FPSS_InspectionOrder_RestricttoRegion))
                    inspectionOrder =
                        inspectionOrder.Where(
                            a => a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == CurrentUser.RegionID);

                if (PageSecurity.HasAccess(PageArea.FPSS_InspectionOrder_RestricttoProvince))
                    inspectionOrder =
                        inspectionOrder.Where(
                            a => a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == CurrentUser.ProvinceID);

                if (PageSecurity.HasAccess(PageArea.FPSS_InspectionOrder_RestricttoMunicipality))
                    inspectionOrder =
                        inspectionOrder.Where(
                            a => a.tblUnits.tblCityMunicipality.Municipality_Id == CurrentUser.MunicipalityID);

                if (PageSecurity.HasAccess(PageArea.FPSS_InspectionOrder_RestricttoStation))
                    inspectionOrder = inspectionOrder.Where(a => a.IO_Unit_Id == CurrentUser.EmployeeUnitId);
            }

            if (!string.IsNullOrEmpty(searchTerms.IO_Number))
                inspectionOrder = inspectionOrder.Where(a => a.IO_Number.Contains(searchTerms.IO_Number));
            if (!string.IsNullOrEmpty(searchTerms.IO_Findings))
                inspectionOrder =
                    inspectionOrder.Where(a => a.IO_Findings.Contains(searchTerms.IO_Findings));
            if (searchTerms.IO_Est_RegistrationStatus > 0)
                inspectionOrder =
                    inspectionOrder.Where(a => a.IO_Est_RegistrationStatus == searchTerms.IO_Est_RegistrationStatus);
            if (searchTerms.IO_Est_HazardType > 0)
                inspectionOrder =
                    inspectionOrder.Where(a => a.IO_Est_HazardType == searchTerms.IO_Est_HazardType);
            if (searchTerms.RegionId > 0)
                inspectionOrder =
                    inspectionOrder.Where(
                        a => a.tblUnits.tblCityMunicipality.tblProvinces.Region_Id == searchTerms.RegionId);
            if (searchTerms.ProvinceId > 0)
                inspectionOrder =
                    inspectionOrder.Where(
                        a => a.tblUnits.tblCityMunicipality.tblProvinces.Province_Id == searchTerms.ProvinceId);
            if (searchTerms.MunicipalityId > 0)
                inspectionOrder =
                    inspectionOrder.Where(
                        a => a.tblUnits.tblCityMunicipality.Municipality_Id == searchTerms.MunicipalityId);
            if (searchTerms.IO_Unit_Id > 0)
                inspectionOrder = inspectionOrder.Where(a => a.IO_Unit_Id == searchTerms.IO_Unit_Id);



            gridInfo.recordsTotal = inspectionOrder.Select(a => a.IO_Id).Count();
            var inspectionOrderResult = inspectionOrder.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
                .Skip(gridInfo.start)
                .Take(gridInfo.length)
                .ToList();

            foreach (var io in inspectionOrderResult)
            {
                retIO.Add(new InspectionOrderModel
                {
                    IO_Number = io.IO_Number,
                    IO_Est_HazardType = io.IO_Est_HazardType.Value,
                    IO_Est_RegistrationStatus = io.IO_Est_RegistrationStatus.Value,
                    IO_Findings = io.IO_Findings,
                    IO_Id = io.IO_Id,
                    IO_Unit_Name = io.tblUnits.Unit_StationName
                });
            }

            return new InspectionOrderListResult
            {
                InspectionOrderList = retIO,
                DatatableInfo = gridInfo
            };
        }

        public InspectionOrderModel GetInspectionOrderById(int inspectionOrderId)
        {
            var ret = new InspectionOrderModel();

            var res = BFPContext.tblInspectionOrders.FirstOrDefault(a => a.IO_Id == inspectionOrderId);

            if (res != null)
            {
                Mapper.Map(res, ret);
                ret.Inspectors = GetInspectionOrderInspectors(res.Ref_IO_Id);
                ret.IO_RemarksName = res.IO_Remarks == null || res.IO_Remarks <= 0 ? null : GetRemarksById(res.IO_Remarks.Value);
                ret.IO_ActionTakenName = res.IO_ActionTaken == null || res.IO_ActionTaken <= 0 ? null : GetActionById(res.IO_ActionTaken.Value);
            }
            else
            {
                throw new Exception("Inspection Order not found!");
            }
            return ret;
        }

        public List<string> GetInspectionOrderInspectors(string io_Id)
        {
            var InspectionOrderInspectors = new List<string>();

            var InspectionOrderInspectorsDet = BFPContext.tblInspectionOrderInspectors.Join(BFPContext.tblEmployees,
                                            inspector => inspector.Insp_Emp_Id,
                                            employee => employee.Emp_Id,
                                            (inspector, employee) => new
                                            {
                                                inspector.Insp_IO_Id,
                                                Fullname = employee.Emp_FirstName + " " + employee.Emp_MiddleName + " " + employee.Emp_LastName + (employee.Emp_SuffixName.Length > 0 ? " " + employee.Emp_SuffixName : ""),
                                                employee.Emp_SuffixName
                                            })
                                            .Where(a => a.Insp_IO_Id == io_Id)
                                            .Distinct();



            foreach (var item in InspectionOrderInspectorsDet.ToList())
            {
                InspectionOrderInspectors.Add( !string.IsNullOrEmpty(item.Emp_SuffixName) ? item.Fullname + " " + item.Emp_SuffixName : item.Fullname);
            }

            return InspectionOrderInspectors;
        }

        public string GetRemarksById(int remarksId)
        {
            var remarksName =
                BFPContext.tblInspectionOrderRemarks.FirstOrDefault(a => a.IOR_Id == remarksId).IOR_RemarkName;

            return remarksName;
        }

        public string GetActionById(int actionId)
        {
            var actionName =
                BFPContext.tblInspectionOrderActions.FirstOrDefault(a => a.IOA_Id == actionId).IOA_ActionName;

            return actionName;
        }
    }
}