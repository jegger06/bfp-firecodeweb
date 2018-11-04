using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.DataAccess;
using Queries.Core.Repositories;
using System.Data.Entity;
using System.Web.Mvc;
using EBFP.BL.Areas.FSIC.Model;

namespace EBFP.BL.FSIC
{
    public class FireSafetyInspectionCertificate : Repository<tblFSICApplication, FSICModel>, IFireSafetyInspectionCertificate
    {
        public FireSafetyInspectionCertificate(EBFPEntities context) : base(context)
        {
        }

        //public List<FSICEstablismentsModel> Establishments(FSIC_Status status, string searchTerm, int unitId)
        //{
        //    int fsicStatus = (int)status;
        //    var Establishments = new List<FSICEstablismentsModel>();

        //    var FSICItems = BFPContext.tblFSICApplication.Join(BFPContext.tblEstablishments,
        //        fsic => fsic.FSIC_Est_Id,
        //        est => est.Est_Id,
        //        (fsic, est) => new
        //        {
        //            fsic.FSIC_Unit_Id,
        //            est.Est_Id,
        //            est.Est_ExpiryDate,
        //            fsic.FSIC_App_Id,
        //            fsic.FSIC_Status,
        //            fsic.FSIC_ApplicationType,
        //            OwnerName = est.Est_OwnerName,
        //            TradeName = est.Est_BusinessTradeName,
        //            BusinessName = est.Est_BusinessName,
        //            NatureOfBusiness = est.Est_NatureOfBusiness,
        //            fsic.FSIC_IsApproveChiefFSES,
        //            fsic.FSIC_IsApproveMarshall,
        //            fsic.FSIC_AIR_IsApproveChiefFSES,
        //            fsic.FSIC_AIR_IsApproveMarshall,

        //            fsic.FSIC_ApplicationDate,
        //            fsic.FSIC_Assesed_Date,
        //            fsic.FSIC_ChiefFSES_Date,
        //            fsic.FSIC_Collected_Date,
        //            fsic.FSIC_Evaluated_Date,
        //            fsic.FSIC_ForReleased_Date,
        //            fsic.FSIC_Released_Date,
        //            fsic.FSIC_Marshall_Date,
        //            est.Est_RegistrationStatus,
        //            est.Est_MP_Number,
        //            Amount =
        //                fsic.FSIC_ConstructionTax + fsic.FSIC_RealtyTax + fsic.FSIC_PremiumTax + fsic.FSIC_SalesTax +
        //                fsic.FSIC_ProceedsTax
        //                + fsic.FSIC_FireSafetyInspectionFee + fsic.FSIC_StorageClearanceFee +
        //                fsic.FSIC_ConveyanceClearanceFee + fsic.FSIC_InstallationClearanceFee
        //                + fsic.FSIC_FireCodeAdminFine + fsic.FSIC_OtherFee,
        //            InspectionOrder =
        //                BFPContext.tblInspectionOrders.OrderByDescending(a => a.Auto_IO_Id)
        //                    .FirstOrDefault(b => b.IO_Est_Id == est.Est_Id && b.IO_Unit_Id == unitId),
        //            fsic.FSIC_ClaimStub_Number,
        //            fsic.FSIC_BusinessType,
        //            FsicLastPayment = BFPContext.tblFSICApplication.OrderByDescending(a => a.FSIC_Collected_Date).FirstOrDefault(a => a.FSIC_Est_Id == est.Est_Id && a.FSIC_Collected_Date != null)
        //        });
        //    .Where(a => a.FSIC_Status == fsicStatus && a.FSIC_Unit_Id == unitId);

        //    if (status == FSIC_Status.Collected)
        //    {
        //        var marshall = (int)FSIC_Status.Marshall;
        //        FSICItems =
        //            FSICItems.Where(a => (a.FSIC_Status == fsicStatus || a.FSIC_Status == marshall) && a.FSIC_Unit_Id == unitId);
        //    }
        //    else if (status == FSIC_Status.ChiefFSES)
        //    {
        //        var marshall = (int)FSIC_Status.Marshall;
        //        FSICItems =
        //            FSICItems.Where(a => (a.FSIC_Status == fsicStatus || a.FSIC_Status == marshall) && a.FSIC_Unit_Id == unitId);
        //    }
        //    else
        //        FSICItems =
        //                FSICItems.Where(a => a.FSIC_Status == fsicStatus && a.FSIC_Unit_Id == unitId);

        //    if (!string.IsNullOrEmpty(searchTerm))
        //    {
        //        var type = -1;
        //        if (FSIC_Type.Occupancy.ToDesc().ToLower().Contains(searchTerm.ToLower()))
        //            type = 0;
        //        if (FSIC_Type.Business.ToDesc().ToLower().Contains(searchTerm.ToLower()))
        //            type = 1;
        //        if (FSIC_Type.PermitToOperate.ToDesc().ToLower().Contains(searchTerm.ToLower()))
        //            type = 2;

        //        FSICItems = FSICItems.Where(a => (a.OwnerName.Contains(searchTerm)
        //                                         || a.TradeName.Contains(searchTerm)
        //                                         || a.BusinessName.Contains(searchTerm)
        //                                         || a.NatureOfBusiness.Contains(searchTerm)
        //                                         || a.Est_MP_Number.Contains(searchTerm)
        //                                         || a.OwnerName.Contains(searchTerm))
        //                                         || a.FSIC_ClaimStub_Number.Contains(searchTerm)
        //                                         || (type > -1 && a.FSIC_ApplicationType == type));
        //    }

        //    foreach (var item in FSICItems.ToList())
        //    {
        //        var model = new FSICEstablismentsModel
        //        {
        //            Est_Id = item.Est_Id,
        //            FSIC_App_Id = item.FSIC_App_Id,
        //            BusinessName = item.BusinessName,
        //            BusinessTradeName = item.TradeName,
        //            NatureOfBusiness = item.NatureOfBusiness,
        //            OwnerName = item.OwnerName,
        //            ApplicationType = item.FSIC_ApplicationType.Value,
        //            Amount = item.Amount ?? 0,
        //            FSIC_IsApproveChiefFSES = item.FSIC_IsApproveChiefFSES,
        //            FSIC_IsApproveMarshall = item.FSIC_IsApproveMarshall,
        //            FSIC_AIR_IsApproveChiefFSES = item.FSIC_AIR_IsApproveChiefFSES,
        //            FSIC_AIR_IsApproveMarshall = item.FSIC_AIR_IsApproveMarshall,
        //            FSIC_status = item.FSIC_Status,
        //            FSIC_ApplicationDate = item.FSIC_ApplicationDate,
        //            FSIC_Assesed_Date = item.FSIC_Assesed_Date,
        //            FSIC_ChiefFSES_Date = item.FSIC_ChiefFSES_Date,
        //            FSIC_Collected_Date = item.FSIC_Collected_Date,
        //            FSIC_Evaluated_Date = item.FSIC_Evaluated_Date,
        //            FSIC_ForReleased_Date = item.FSIC_ForReleased_Date,
        //            FSIC_Released_Date = item.FSIC_Released_Date,
        //            FSIC_Marshall_Date = item.FSIC_Marshall_Date,
        //            FSIC_MP_NUMBER = item.Est_MP_Number,
        //            FSIC_BusinessType = item.FSIC_BusinessType,
        //            FSIC_ExpiryDate = item.Est_ExpiryDate,
        //            RegistrationStatus = item.Est_RegistrationStatus,

        //        };

        //        if (item.FsicLastPayment != null)
        //            model.FSIC_LastPaymentDate = item.FsicLastPayment.FSIC_Collected_Date;

        //        if (item.InspectionOrder != null)
        //        {
        //            model.IONumber = item.InspectionOrder.IO_Number;
        //            model.IO_Id = item.InspectionOrder.IO_Id;
        //            model.IORemarks = item.InspectionOrder.IO_Remarks ?? 0;

        //            model.IO_ApprovalMarshall_Emp_Id = item.InspectionOrder.IO_ApprovalMarshall_Emp_Id;
        //            model.IO_ApprovalChiefFSES_Emp_Id = item.InspectionOrder.IO_ApprovalChiefFSES_Emp_Id;
        //            model.IO_Approval_AIR_Marshall_Emp_Id = item.InspectionOrder.IO_Approval_AIR_Marshall_Emp_Id;
        //            model.IO_Approval_AIR_ChiefFSES_Emp_Id = item.InspectionOrder.IO_Approval_AIR_ChiefFSES_Emp_Id;
        //        }
        //        Establishments.Add(model);
        //    }

        //    return Establishments;
        //}

        public void UpdateFSIC(FSICModel model)
        {
            var fsicDet = BFPContext.tblFSICApplication.Find(model.FSIC_App_Id);
            if (fsicDet == null) throw new Exception("FSIC could not be found.");

            fsicDet.FSIC_Status = model.FSIC_Status;
            fsicDet.FSIC_Assesor_Emp_Id = model.FSIC_Assesor_Emp_Id;
            fsicDet.FSIC_TaxYear = model.FSIC_TaxYear;
            fsicDet.FSIC_ConstructionTax = model.FSIC_ConstructionTax;
            fsicDet.FSIC_RealtyTax = model.FSIC_RealtyTax;
            fsicDet.FSIC_PremiumTax = model.FSIC_PremiumTax;
            fsicDet.FSIC_SalesTax = model.FSIC_SalesTax;
            fsicDet.FSIC_ProceedsTax = model.FSIC_ProceedsTax;
            fsicDet.FSIC_FireSafetyInspectionFee = model.FSIC_FireSafetyInspectionFee;
            fsicDet.FSIC_StorageClearanceFee = model.FSIC_StorageClearanceFee;
            fsicDet.FSIC_ConveyanceClearanceFee = model.FSIC_ConveyanceClearanceFee;
            fsicDet.FSIC_InstallationClearanceFee = model.FSIC_InstallationClearanceFee;
            fsicDet.FSIC_FireCodeAdminFine = model.FSIC_FireCodeAdminFine;
            fsicDet.FSIC_OtherFee = model.FSIC_OtherFee;

            BFPContext.Entry(fsicDet).State = EntityState.Modified;
            BFPContext.SaveChanges();
        }
        
        public void SaveRemarks(FSICModel model)
        {
            var fsecDet = BFPContext.tblFSICApplication.Find(model.FSIC_App_Id);
            if (fsecDet == null) throw new Exception("FSIC could not be found.");

            fsecDet.FSIC_Remarks = model.FSIC_Remarks;

            BFPContext.Entry(fsecDet).State = EntityState.Modified;
            BFPContext.SaveChanges();
        }

        public string SavePaymentFSIC(FSICModel model)
        {
            var entity = new tblFSICApplication();
            Mapper.Map(model, entity);
            BFPContext.tblFSICApplication.Add(entity);
            BFPContext.SaveChanges();

            return entity.Ref_FSIC_App_Id;
        }


        public OPSFSICModel GetFSIC(string fsicId, int unitId)
        {
            var fsicModel = new OPSFSICModel();

            var fsicDet = (
                   from a in BFPContext.tblFSICApplication
                   join b in BFPContext.tblUnits on a.FSIC_Unit_Id equals b.Unit_Id
                   join c in BFPContext.tblCityMunicipality on b.Unit_Municipality_Id equals c.Municipality_Id
                   join d in BFPContext.tblProvinces on c.Municipality_Province_Id equals d.Province_Id
                   join e in BFPContext.tblRegions on d.Region_Id equals e.Reg_Id
                   join f in BFPContext.tblEstablishments on a.FSIC_Est_Id equals f.Ref_Est_Id
                   where a.Ref_FSIC_App_Id == fsicId && a.FSIC_Unit_Id == unitId
                   select new
                   {
                       f.Est_MP_Number,
                       f.Est_BusinessName,
                       f.Est_OwnerName,
                       f.Est_BusinessAddress,
                       b.Unit_StationName,
                       a.FSIC_OPS_Number,
                       a.FSIC_Assesed_Date,
                       b.Unit_Address,
                       d.Province_Name,
                       e.Reg_Title,
                       a.FSIC_ConstructionTax,
                       a.FSIC_RealtyTax,
                       a.FSIC_PremiumTax,
                       a.FSIC_SalesTax,
                       a.FSIC_ProceedsTax,
                       a.FSIC_FireSafetyInspectionFee,
                       a.FSIC_StorageClearanceFee,
                       a.FSIC_ConveyanceClearanceFee,
                       a.FSIC_InstallationClearanceFee,
                       a.FSIC_FireCodeAdminFine,
                       a.FSIC_OtherFee,
                       Assessor = BFPContext.tblEmployees.FirstOrDefault(g => g.Emp_Id == a.FSIC_Assesor_Emp_Id),
                       a.FSIC_BusinessType
                   }).FirstOrDefault();

            if (fsicDet != null)
            {
                fsicModel.Est_MP_Number = fsicDet.Est_MP_Number;
                fsicModel.Est_BusinessName = fsicDet.Est_BusinessName;
                fsicModel.Est_OwnerName = fsicDet.Est_OwnerName;
                fsicModel.Est_BusinessAddress = fsicDet.Est_BusinessAddress;
                fsicModel.Unit_StationName = fsicDet.Unit_StationName;
                fsicModel.FSIC_OPS_Number = fsicDet.FSIC_OPS_Number;
                fsicModel.FSIC_Assesed_Date = fsicDet.FSIC_Assesed_Date;
                fsicModel.Unit_Address = fsicDet.Unit_Address;
                fsicModel.Province_Name = fsicDet.Province_Name;
                fsicModel.Reg_Title = fsicDet.Reg_Title;
                fsicModel.FSIC_OtherFee = fsicDet.FSIC_OtherFee;
                fsicModel.FSIC_ConstructionTax = fsicDet.FSIC_ConstructionTax;
                fsicModel.FSIC_RealtyTax = fsicDet.FSIC_RealtyTax;
                fsicModel.FSIC_PremiumTax = fsicDet.FSIC_PremiumTax;
                fsicModel.FSIC_SalesTax = fsicDet.FSIC_SalesTax;
                fsicModel.FSIC_ProceedsTax = fsicDet.FSIC_ProceedsTax;
                fsicModel.FSIC_FireSafetyInspectionFee = fsicDet.FSIC_FireSafetyInspectionFee;
                fsicModel.FSIC_StorageClearanceFee = fsicDet.FSIC_StorageClearanceFee;
                fsicModel.FSIC_ConveyanceClearanceFee = fsicDet.FSIC_ConveyanceClearanceFee;
                fsicModel.FSIC_InstallationClearanceFee = fsicDet.FSIC_InstallationClearanceFee;
                fsicModel.FSIC_FireCodeAdminFine = fsicDet.FSIC_FireCodeAdminFine;
                fsicModel.FSIC_BusinessType = fsicDet.FSIC_BusinessType;
                var employee = fsicDet.Assessor;
                if (employee != null)
                {
                    var rank = BFPContext.tblRanks.FirstOrDefault(a => a.Rank_Id == employee.Emp_Curr_Rank);
                    var middleInitial = employee.Emp_MiddleName[0].ToString() ?? "";
                    if (rank != null)
                        fsicModel.Assessor = rank.Rank_Name + " " + employee.Emp_FirstName + " " + middleInitial + " " + employee.Emp_LastName;
                    else
                        fsicModel.Assessor = employee.Emp_FirstName + " " + middleInitial + " " + employee.Emp_LastName;

                    if (!string.IsNullOrEmpty(employee.Emp_SuffixName) && employee.Emp_SuffixName.ToLower() != "none" && employee.Emp_SuffixName.ToLower() != "n/a")
                        fsicModel.Assessor = fsicModel.Assessor + " " + employee.Emp_SuffixName;
                }

            }
            return fsicModel;
        }

        //public FSICReportModel GetFSICReport(string fsicId, int unitId)
        //{
        //    var fsicModel = new FSICReportModel();

        //    var fsicDet = (
        //           from a in BFPContext.tblFSICApplication
        //           join b in BFPContext.tblUnits on a.FSIC_Unit_Id equals b.Unit_Id
        //           join c in BFPContext.tblCityMunicipality on b.Unit_Municipality_Id equals c.Municipality_Id
        //           join d in BFPContext.tblProvinces on c.Municipality_Province_Id equals d.Province_Id
        //           join e in BFPContext.tblRegions on d.Region_Id equals e.Reg_Id
        //           join f in BFPContext.tblEstablishments on a.FSIC_Est_Id equals f.Est_Id
        //           join g in BFPContext.tblApplicationPayments on a.FSIC_App_Id equals g.AP_App_Id
        //           where a.FSIC_App_Id == fsicId && a.FSIC_Unit_Id == unitId
        //           select new
        //           {
        //               f.Est_MP_Number,
        //               f.Est_BusinessName,
        //               f.Est_BusinessAddress,
        //               f.Est_ExpiryDate,
        //               b.Unit_StationName,
        //               b.Unit_Address,
        //               d.Province_Name,
        //               e.Reg_Title,
        //               b.Unit_PhoneNumber,
        //               f.Est_AuthorizedRepresentative,
        //               g.AP_ORNumber,
        //               g.AP_ORDate,
        //               g.AP_ORAmount,
        //               g.AP_AmountTendered,
        //               f.Est_OwnerName,
        //               a.FSIC_Number,
        //               a.FSIC_ApplicationType,
        //               b.Unit_ChiefFSES_Signature,
        //               b.Unit_FireMarshall_Signature,
        //               fireMarshall = b.Unit_FireMarshall_Emp_Id != null ? BFPContext.tblEmployees.FirstOrDefault(h => h.Emp_Id == b.Unit_FireMarshall_Emp_Id) : null,
        //               chiefFSES = b.Unit_ChiefFSES_Emp_Id != null ? BFPContext.tblEmployees.FirstOrDefault(h => h.Emp_Id == b.Unit_ChiefFSES_Emp_Id) : null,
        //               a.FSIC_BusinessType,
        //               InspectionOrder =
        //                BFPContext.tblInspectionOrders.OrderByDescending(a => a.Auto_IO_Id)
        //                    .FirstOrDefault(b => b.IO_Est_Id == f.Est_Id && b.IO_Unit_Id == unitId),

        //           }).FirstOrDefault();

        //    if (fsicDet != null)
        //    {
        //        fsicModel.Est_MP_Number = fsicDet.Est_MP_Number;
        //        fsicModel.BusinessName = fsicDet.Est_BusinessName;
        //        fsicModel.OwnerName = string.IsNullOrEmpty(fsicDet.Est_OwnerName) ? fsicDet.Est_AuthorizedRepresentative : fsicDet.Est_OwnerName;
        //        fsicModel.BusinessAddress = fsicDet.Est_BusinessAddress ?? "";
        //        fsicModel.Region = fsicDet.Reg_Title ?? "";
        //        fsicModel.Province = fsicDet.Province_Name ?? "";
        //        fsicModel.StationName = fsicDet.Unit_StationName ?? "";
        //        fsicModel.PhoneNumber = fsicDet.Unit_PhoneNumber ?? "";
        //        fsicModel.ORNumber = fsicDet.AP_ORNumber;
        //        fsicModel.PaymentDate = fsicDet.AP_ORDate;
        //        fsicModel.AmountPaid = fsicDet.AP_ORAmount;
        //        fsicModel.FSIC_Number = fsicDet.FSIC_Number;
        //        fsicModel.FSIC_BusinessType = fsicDet.FSIC_BusinessType;
        //        fsicModel.FSIC_ApplicationType = fsicDet.FSIC_ApplicationType;
        //        fsicModel.FSIC_ChiefFSESSignature = fsicDet.Unit_ChiefFSES_Signature;
        //        fsicModel.FSIC_FireMarshallSignature = fsicDet.Unit_FireMarshall_Signature;
        //        fsicModel.Est_ExpiryDate = fsicDet.Est_ExpiryDate;
        //        if (fsicDet.fireMarshall != null)
        //        {
        //            var rank = BFPContext.tblRanks.FirstOrDefault(a => a.Rank_Ref_Id == fsicDet.fireMarshall.Emp_Curr_Rank);
        //            var middleInitial = fsicDet.fireMarshall.Emp_MiddleName[0].ToString() ?? "";
        //            if (rank != null)
        //                fsicModel.FireMarshall = rank.Rank_Name + " " + fsicDet.fireMarshall.Emp_FirstName + " " + middleInitial + " " + fsicDet.fireMarshall.Emp_LastName;
        //            else
        //                fsicModel.FireMarshall = fsicDet.fireMarshall.Emp_FirstName + " " + middleInitial + " " + fsicDet.fireMarshall.Emp_LastName;

        //            if (!string.IsNullOrEmpty(fsicDet.fireMarshall.Emp_SuffixName) && fsicDet.fireMarshall.Emp_SuffixName != "none" && fsicDet.fireMarshall.Emp_SuffixName != "n/a")
        //                fsicModel.FireMarshall = fsicModel.FireMarshall + " " + fsicDet.fireMarshall.Emp_SuffixName;
        //        }
        //        if (fsicDet.chiefFSES != null)
        //        {
        //            var rank = BFPContext.tblRanks.FirstOrDefault(a => a.Rank_Ref_Id == fsicDet.chiefFSES.Emp_Curr_Rank);
        //            var middleInitial = fsicDet.chiefFSES.Emp_MiddleName[0].ToString() ?? "";
        //            if (rank != null)
        //                fsicModel.ChiefFSES = rank.Rank_Name + " " + fsicDet.chiefFSES.Emp_FirstName + " " + middleInitial + " " + fsicDet.chiefFSES.Emp_LastName;
        //            else
        //                fsicModel.ChiefFSES = fsicDet.chiefFSES.Emp_FirstName + " " + middleInitial + " " + fsicDet.chiefFSES.Emp_LastName;

        //            if (!string.IsNullOrEmpty(fsicDet.chiefFSES.Emp_SuffixName) && fsicDet.chiefFSES.Emp_SuffixName != "none" && fsicDet.chiefFSES.Emp_SuffixName != "n/a")
        //                fsicModel.ChiefFSES = fsicModel.ChiefFSES + " " + fsicDet.chiefFSES.Emp_SuffixName;
        //        }

        //        if (fsicDet.InspectionOrder != null)
        //        {
        //            fsicModel.OccupancyType = fsicDet.InspectionOrder.IO_Est_OccupancyType ?? 0;
        //            fsicModel.InspectionType = fsicDet.InspectionOrder.IO_InspectionType ?? 0;
        //        }

        //    }
        //    return fsicModel;
        //}

        public void UpdateFSICStatus(FSICModel model, string type = "", string forComplianceType = "")
        {
            var fsecDet = BFPContext.tblFSICApplication.FirstOrDefault(a => a.FSIC_App_Id == model.FSIC_App_Id);
            if (fsecDet == null) throw new Exception("FSIC could not be found.");

            if (type == "ChiefFsesAIR")
            {
                fsecDet.FSIC_AIR_Approve_ChiefFSES_Date = DateTime.Now;
                fsecDet.FSIC_AIR_IsApproveChiefFSES = true;
                fsecDet.FSIC_AIR_ChiefFSES_Emp_Id = model.FSIC_AIR_ChiefFSES_Emp_Id;
                fsecDet.FSIC_Status = (int)FSIC_Status.ChiefFSES;
                fsecDet.FSIC_AIR_ChiefFSES_Date = DateTime.Now;
            }
            else if (type == "ChiefFsesIO")
            {
                fsecDet.FSIC_Approve_ChiefFSES_Date = DateTime.Now;
                fsecDet.FSIC_IsApproveChiefFSES = true;
                fsecDet.FSIC_ChiefFSES_Emp_Id = model.FSIC_ChiefFSES_Emp_Id;
                fsecDet.FSIC_Status = (int)FSIC_Status.ChiefFSES;
                fsecDet.FSIC_ChiefFSES_Date = DateTime.Now;
            }

            else if (type == "MarshallAIR")
            {
                fsecDet.FSIC_AIR_Approve_Marshall_Date = DateTime.Now;
                fsecDet.FSIC_AIR_IsApproveMarshall = true;
                fsecDet.FSIC_AIR_Marshall_Emp_Id = model.FSIC_AIR_Marshall_Emp_Id;

                fsecDet.FSIC_AIR_Marshall_Date = DateTime.Now;
                fsecDet.FSIC_Number = model.FSIC_Number;
                fsecDet.FSIC_Issue_Date = DateTime.Now;
                if (forComplianceType == "NTC" || forComplianceType == "NTCV" || forComplianceType == "Abatement Order" || forComplianceType == "Closure Order")
                {
                    fsecDet.FSIC_ForReleased_Date = DateTime.Now;
                    fsecDet.FSIC_ForReleasing_Emp_Id = model.FSIC_AIR_Marshall_Emp_Id;
                    fsecDet.FSIC_Status = (int)FSIC_Status.ChiefFSES; //For Marshal Tab
                }
                else
                {
                    fsecDet.FSIC_Status = (int)FSIC_Status.Marshall; //For Releasing Tab
                    fsecDet.FSIC_Released_Date = DateTime.Now;
                    fsecDet.FSIC_Released_Emp_Id = model.FSIC_Marshall_Emp_Id;
                }
            }

            else if (type == "MarshallIO")
            {
                fsecDet.FSIC_Approve_Marshall_Date = DateTime.Now;
                fsecDet.FSIC_IsApproveMarshall = true;
                fsecDet.FSIC_Marshall_Emp_Id = model.FSIC_Marshall_Emp_Id;
                fsecDet.FSIC_Status = (int)FSIC_Status.Marshall;
                fsecDet.FSIC_Marshall_Date = DateTime.Now;

                //fsecDet.FSIC_ForReleased_Date = DateTime.Now;
                //fsecDet.FSIC_ForReleasing_Emp_Id = model.FSIC_Marshall_Emp_Id;
            }

            fsecDet.IsSynced = false;
            BFPContext.Entry(fsecDet).State = EntityState.Modified;
            BFPContext.SaveChanges();
        }

        //public List<FSICEstablismentsModel> FsicNotification(FSIC_Status status, int unitId)
        //{
        //    int fsicStatus = (int)status;
        //    var Establishments = new List<FSICEstablismentsModel>();

        //    var FSICItems = BFPContext.tblFSICApplication.Join(BFPContext.tblEstablishments,
        //        fsic => fsic.FSIC_Est_Id,
        //        est => est.Est_Id,
        //        (fsic, est) => new
        //        {
        //            fsic.FSIC_Unit_Id,
        //            est.Est_Id,
        //            fsic.FSIC_App_Id,
        //            fsic.FSIC_Status,
        //            fsic.FSIC_ApplicationType,
        //            fsic.FSIC_IsApproveChiefFSES,
        //            fsic.FSIC_IsApproveMarshall,
        //            fsic.FSIC_AIR_IsApproveChiefFSES,
        //            fsic.FSIC_AIR_IsApproveMarshall,
        //            InspectionOrder =
        //                BFPContext.tblInspectionOrders.OrderByDescending(a => a.Auto_IO_Id)
        //                    .FirstOrDefault(b => b.IO_Est_Id == est.Est_Id && b.IO_Unit_Id == unitId)
        //        }).Where(a => a.FSIC_Unit_Id == unitId);

        //    if (status == FSIC_Status.Collected)
        //    {
        //        var marshall = (int)FSIC_Status.Marshall;
        //        FSICItems =
        //            FSICItems.Where(a => (a.FSIC_Status == fsicStatus || a.FSIC_Status == marshall) && a.FSIC_Unit_Id == unitId);
        //    }
        //    else
        //        FSICItems =
        //                FSICItems.Where(a => a.FSIC_Status == fsicStatus && a.FSIC_Unit_Id == unitId);

        //    foreach (var item in FSICItems.ToList())
        //    {
        //        var model = new FSICEstablismentsModel
        //        {
        //            Est_Id = item.Est_Id,
        //            FSIC_App_Id = item.FSIC_App_Id,
        //            ApplicationType = item.FSIC_ApplicationType.Value,
        //            FSIC_IsApproveChiefFSES = item.FSIC_IsApproveChiefFSES,
        //            FSIC_IsApproveMarshall = item.FSIC_IsApproveMarshall,
        //            FSIC_AIR_IsApproveChiefFSES = item.FSIC_AIR_IsApproveChiefFSES,
        //            FSIC_AIR_IsApproveMarshall = item.FSIC_AIR_IsApproveMarshall,
        //            FSIC_status = item.FSIC_Status
        //        };

        //        if (item.InspectionOrder != null)
        //        {
        //            model.IONumber = item.InspectionOrder.IO_Number;
        //            model.IO_Id = item.InspectionOrder.IO_Id;
        //            model.IORemarks = item.InspectionOrder.IO_Remarks ?? 0;
        //        }

        //        Establishments.Add(model);
        //    }

        //    return Establishments;
        //}

        public string GetFSICLastPaymentDate(string est_Id)
        {
            var fsic = BFPContext.tblFSICApplication.OrderByDescending(a => a.FSIC_Collected_Date).FirstOrDefault(a => a.FSIC_Est_Id == est_Id && a.FSIC_Collected_Date != null);
            if (fsic != null)
            {
                return fsic.FSIC_Collected_Date.Value.ToString("MMM/dd/yyyy");
            }

            return "";
        }

        public void UpdateTranferFSIC(FSICModel model, int unitId)
        {
            var fsicDet = BFPContext.tblFSICApplication.FirstOrDefault(a => a.FSIC_Unit_Id == unitId && a.FSIC_App_Id == model.FSIC_App_Id);
            if (fsicDet == null) throw new Exception("FSIC could not be found.");

            fsicDet.FSIC_Status = (int)FSIC_Status.Collected;
            fsicDet.FSIC_Collected_Date = DateTime.Now;
            fsicDet.FSIC_Collector_Emp_Id = model.FSIC_Collector_Emp_Id;
            fsicDet.IsSynced = false;
            BFPContext.Entry(fsicDet).State = EntityState.Modified;
            BFPContext.SaveChanges();
        }


    }
}
