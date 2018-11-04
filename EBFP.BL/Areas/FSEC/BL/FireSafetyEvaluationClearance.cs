using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EBFP.BL.Helper;
using EBFP.DataAccess;
using EBFP.Helper;
using Queries.Core.Repositories;
using EBFP.BL.Establishment;
using System.Data.Entity;

namespace EBFP.BL.FSEC
{
    public class FSECBL : Repository<tblFSECApplication, FSECModel>, IFireSafetyEvaluationClearance
    {
        public FSECBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblFSECApplication, FSECModel>().ReverseMap();
            Mapper.CreateMap<List<tblFSECApplication>, List<FSECModel>>().ReverseMap();
            Mapper.CreateMap<List<tblFSECApplication>, List<FSECModel>>();
        }
        public List<FSECEstablismentsModel> Establishments(FSEC_Status status, string searchTerm, int unitId)
        {
            int FSEC_Status = (int)status;
            var Establishments = new List<FSECEstablismentsModel>();
                    var CommercialItems = BFPContext.tblFSECApplication.Join(BFPContext.tblEstablishments,
                                            fsec => fsec.FSEC_Est_Id,
                                            est => est.Ref_Est_Id,
                                            (fsec, est) => new 
                                            {
                                                Est_Id = est.Ref_Est_Id,
                                                fsec.Ref_FSEC_App_Id,
                                                fsec.FSEC_Status,
                                                fsec.FSEC_EstablishmentType,
                                                fsec.FSEC_Unit_Id,
                                                OwnerName = est.Est_OwnerName,
                                                Address = est.Est_BusinessAddress,
                                                AuthorizedRepresentative = est.Est_AuthorizedRepresentative,
                                                BusinessName = est.Est_BusinessName,
                                                fsec.FSEC_IsApprovePlanEvaluated,
                                                fsec.FSEC_IsApproveChiefFSES,
                                                fsec.FSEC_IsApproveMarshall,
                                                fsec.FSEC_ApplicationDate,
                                                fsec.FSEC_Assesed_Date,
                                                fsec.FSEC_ChiefFSES_Date,
                                                fsec.FSEC_Collected_Date,
                                                fsec.FSEC_Evaluated_Date,
                                                fsec.FSEC_ForReleased_Date,
                                                fsec.FSEC_Released_Date,
                                                fsec.FSEC_Mashall_Date,
                                                fsec.FSEC_PlanEvaluated_Date,
                                                est.Est_MP_Number,
                                                fsec.FSEC_ClaimStub_Number
                                            }).Where(a => a.FSEC_Status == FSEC_Status &&
                                          a.FSEC_EstablishmentType == (int)FSEC_EstablishmentType.Commercial && a.FSEC_Unit_Id == unitId);

            if (!string.IsNullOrEmpty(searchTerm) )
            {
                var type = -1;
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    if (FSEC_EstablishmentType.Commercial.ToDescription().ToLower().Contains(searchTerm.ToLower()))
                        type = 0;                    
                }

                if (type != 0)
                {
                    CommercialItems = CommercialItems.Where(a => a.OwnerName.Contains(searchTerm)
                    || a.Address.Contains(searchTerm)
                    || a.AuthorizedRepresentative.Contains(searchTerm)
                    || a.BusinessName.Contains(searchTerm)
                    || a.OwnerName.Contains(searchTerm)
                    || a.FSEC_ClaimStub_Number.Contains(searchTerm)
                    || a.Est_MP_Number.Contains(searchTerm));
                }
            }

            var NonCommercialItems = BFPContext.tblFSECApplication.Join(BFPContext.tblNonCommercialEstablishments,
                                           fsec => fsec.FSEC_Est_Id,
                                           est => est.Ref_NCE_Id,
                                           (fsec, est) => new
                                           {
                                               Est_Id = est.Ref_NCE_Id,
                                               fsec.Ref_FSEC_App_Id,
                                               fsec.FSEC_Status,
                                               fsec.FSEC_EstablishmentType,
                                               fsec.FSEC_Unit_Id,
                                               OwnerName = est.NCE_OwnerName,
                                               Address = est.NCE_ConstructionAddress,
                                               AuthorizedRepresentative = est.NCE_AuthorizedRepresentative,
                                               BusinessName = "",
                                               fsec.FSEC_IsApprovePlanEvaluated,
                                               fsec.FSEC_IsApproveChiefFSES,
                                               fsec.FSEC_IsApproveMarshall,
                                               fsec.FSEC_ApplicationDate,
                                               fsec.FSEC_Assesed_Date,
                                               fsec.FSEC_ChiefFSES_Date,
                                               fsec.FSEC_Collected_Date,
                                               fsec.FSEC_Evaluated_Date,
                                               fsec.FSEC_ForReleased_Date,
                                               fsec.FSEC_Released_Date,
                                               fsec.FSEC_Mashall_Date,
                                               fsec.FSEC_PlanEvaluated_Date,
                                               Est_MP_Number = "",
                                               fsec.FSEC_ClaimStub_Number
                                           }).Where(a => a.FSEC_Status == FSEC_Status &&
                                         a.FSEC_EstablishmentType == (int)FSEC_EstablishmentType.NonCommercial && a.FSEC_Unit_Id == unitId);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var type = -1;
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    if (FSEC_EstablishmentType.NonCommercial.ToDescription().ToLower().Contains(searchTerm.ToLower()))
                        type = 1;
                }

                if (type != 1)
                {
                    NonCommercialItems = NonCommercialItems.Where(a => a.OwnerName.Contains(searchTerm)
                    || a.Address.Contains(searchTerm)
                    || a.AuthorizedRepresentative.Contains(searchTerm)
                    || a.FSEC_ClaimStub_Number.Contains(searchTerm)
                    || a.BusinessName.Contains(searchTerm));
                }
            }

            var FilteredEstablisments =  CommercialItems.Concat(NonCommercialItems);

            foreach (var item in FilteredEstablisments.ToList())
            {
                Establishments.Add(new FSECEstablismentsModel
                {
                    Est_Id = item.Est_Id,
                    FSEC_App_Id = item.Ref_FSEC_App_Id,
                    OwnerName = item.OwnerName,
                    Address = item.Address,
                    AuthorizedRepresentative = item.AuthorizedRepresentative,
                    BusinessName = item.BusinessName,
                    EstablishmentType = item.FSEC_EstablishmentType,
                    FSEC_IsApprovePlanEvaluated = item.FSEC_IsApprovePlanEvaluated,
                    FSEC_IsApproveChiefFSES = item.FSEC_IsApproveChiefFSES,
                    FSEC_IsApproveMarshall = item.FSEC_IsApproveMarshall,
                    FSEC_status = item.FSEC_Status,
                    FSEC_ApplicationDate = item.FSEC_ApplicationDate,
                    FSEC_Assesed_Date = item.FSEC_Assesed_Date,
                    FSEC_ChiefFSES_Date = item.FSEC_ChiefFSES_Date,
                    FSEC_Collected_Date = item.FSEC_Collected_Date,
                    FSEC_Evaluated_Date = item.FSEC_Evaluated_Date,
                    FSEC_ForReleased_Date = item.FSEC_ForReleased_Date,
                    FSEC_Released_Date = item.FSEC_Released_Date,
                    FSEC_Marshall_Date = item.FSEC_Mashall_Date,
                    FSEC_PlanEvaluated_Date = item.FSEC_PlanEvaluated_Date,
                    FSEC_MP_NUMBER = item.Est_MP_Number
                });
            }

            return Establishments;
        }

        public List<FSECEstablismentsModel> ForReleasingEstablishments(FSEC_Status status, string searchTerm, int unitId)
        {
            int FSEC_Status = (int)status;
            var Establishments = new List<FSECEstablismentsModel>();

            var CommercialItems = BFPContext.tblFSECApplication.Join(BFPContext.tblEstablishments,
                                            fsec => fsec.FSEC_Est_Id,
                                            est => est.Ref_Est_Id,
                                            (fsec, est) => new
                                            {
                                                Est_Id = est.Ref_Est_Id,
                                                fsec.Ref_FSEC_App_Id,
                                                fsec.FSEC_Status,
                                                fsec.FSEC_EstablishmentType,
                                                fsec.FSEC_Unit_Id,
                                                OwnerName = est.Est_OwnerName,
                                                Address = est.Est_BusinessAddress,
                                                AuthorizedRepresentative = est.Est_AuthorizedRepresentative,
                                                BusinessName = est.Est_BusinessName,
                                                fsec.FSEC_IsApprovePlanEvaluated,
                                                fsec.FSEC_IsApproveChiefFSES,
                                                fsec.FSEC_IsApproveMarshall,
                                                fsec.FSEC_ForReleased_Date,
                                                fsec.FSEC_PlanEvaluated_Date,
                                                fsec.FSEC_ChiefFSES_Date,
                                                fsec.FSEC_Mashall_Date,
                                                est.Est_MP_Number,
                                                fsec.FSEC_Released_Date,
                                                fsec.FSEC_ClaimStub_Number,
                                                Status = fsec.FSEC_IsApprovePlanEvaluated == false || fsec.FSEC_IsApproveChiefFSES == false || fsec.FSEC_IsApproveMarshall == false ? "DISAPPROVED"
                                                : fsec.FSEC_Status == (int)Helper.FSEC_Status.PlanEvaluator || fsec.FSEC_Status == (int)Helper.FSEC_Status.ChiefFSES || fsec.FSEC_Status == (int)Helper.FSEC_Status.Marshall
                                                || fsec.FSEC_Status == (int)Helper.FSEC_Status.Released ? "APPROVED" : "",
                                                fsec.FSEC_IsManual
                                            }).Where(a => (a.FSEC_Status == FSEC_Status || a.FSEC_IsApprovePlanEvaluated == false
                                            || a.FSEC_IsApproveChiefFSES == false || a.FSEC_IsApproveMarshall == false) &&
                                          a.FSEC_EstablishmentType == (int)FSEC_EstablishmentType.Commercial && a.FSEC_Unit_Id == unitId);

            if (!string.IsNullOrEmpty(searchTerm))
                CommercialItems = CommercialItems.Where(a => a.OwnerName.Contains(searchTerm)
                || a.Address.Contains(searchTerm)
                || a.AuthorizedRepresentative.Contains(searchTerm)
                || a.FSEC_ClaimStub_Number.Contains(searchTerm)
                || a.BusinessName.Contains(searchTerm) || a.Status.Contains(searchTerm));

            var NonCommercialItems = BFPContext.tblFSECApplication.Join(BFPContext.tblNonCommercialEstablishments,
                                           fsec => fsec.FSEC_Est_Id,
                                           est => est.Ref_NCE_Id,
                                           (fsec, est) => new
                                           {
                                               Est_Id = est.Ref_NCE_Id,
                                               fsec.Ref_FSEC_App_Id,
                                               fsec.FSEC_Status,
                                               fsec.FSEC_EstablishmentType,
                                               fsec.FSEC_Unit_Id,
                                               OwnerName = est.NCE_OwnerName,
                                               Address = est.NCE_ConstructionAddress,
                                               AuthorizedRepresentative = est.NCE_AuthorizedRepresentative,
                                               BusinessName = "",
                                               fsec.FSEC_IsApprovePlanEvaluated,
                                               fsec.FSEC_IsApproveChiefFSES,
                                               fsec.FSEC_IsApproveMarshall,
                                               fsec.FSEC_ForReleased_Date,
                                               fsec.FSEC_PlanEvaluated_Date,
                                               fsec.FSEC_ChiefFSES_Date,
                                               fsec.FSEC_Mashall_Date,
                                               Est_MP_Number = "",
                                               fsec.FSEC_Released_Date,
                                               fsec.FSEC_ClaimStub_Number,
                                               Status = fsec.FSEC_IsApprovePlanEvaluated == false || fsec.FSEC_IsApproveChiefFSES == false || fsec.FSEC_IsApproveMarshall == false ? "DISAPPROVED"
                                                : fsec.FSEC_Status == (int)Helper.FSEC_Status.PlanEvaluator || fsec.FSEC_Status == (int)Helper.FSEC_Status.ChiefFSES || fsec.FSEC_Status == (int)Helper.FSEC_Status.Marshall
                                                || fsec.FSEC_Status == (int)Helper.FSEC_Status.Released ? "APPROVED" : "",
                                               fsec.FSEC_IsManual
                                           }).Where(a => (a.FSEC_Status == FSEC_Status || a.FSEC_IsApprovePlanEvaluated == false
                                            || a.FSEC_IsApproveChiefFSES == false || a.FSEC_IsApproveMarshall == false) &&
                                         a.FSEC_EstablishmentType == (int)FSEC_EstablishmentType.NonCommercial && a.FSEC_Unit_Id == unitId);

            if (!string.IsNullOrEmpty(searchTerm))
                NonCommercialItems = NonCommercialItems.Where(a => a.OwnerName.Contains(searchTerm)
                || a.Address.Contains(searchTerm)
                || a.AuthorizedRepresentative.Contains(searchTerm)
                || a.FSEC_ClaimStub_Number.Contains(searchTerm)
                || a.BusinessName.Contains(searchTerm) || a.Status.Contains(searchTerm));

            var FilteredEstablisments = CommercialItems.Concat(NonCommercialItems);

            foreach (var item in FilteredEstablisments.ToList())
            {
                Establishments.Add(new FSECEstablismentsModel
                {
                    Est_Id = item.Est_Id,
                    FSEC_App_Id = item.Ref_FSEC_App_Id,
                    OwnerName = item.OwnerName,
                    Address = item.Address,
                    AuthorizedRepresentative = item.AuthorizedRepresentative,
                    BusinessName = item.BusinessName,
                    EstablishmentType = item.FSEC_EstablishmentType,
                    FSEC_IsApprovePlanEvaluated = item.FSEC_IsApprovePlanEvaluated,
                    FSEC_ForReleased_Date = item.FSEC_ForReleased_Date,
                    FSEC_PlanEvaluated_Date = item.FSEC_PlanEvaluated_Date,
                    FSEC_ChiefFSES_Date = item.FSEC_ChiefFSES_Date,
                    FSEC_Marshall_Date = item.FSEC_Mashall_Date,
                    FSEC_status = item.FSEC_Status,
                    FSEC_MP_NUMBER = item.Est_MP_Number,
                    FSEC_IsApproveChiefFSES = item.FSEC_IsApproveChiefFSES,
                    FSEC_IsApproveMarshall = item.FSEC_IsApproveMarshall,
                    FSEC_Released_Date = item.FSEC_Released_Date,
                    FSEC_IsManual = item.FSEC_IsManual == null || item.FSEC_IsManual == false ? "NO" : "YES"
                });
            }

            return Establishments;
        }
        public void UpdateFSECForCollection(FSECModel model)
        {
            var fsecDet = BFPContext.tblFSECApplication.Find(model.FSEC_App_Id);
            if (fsecDet == null) throw new Exception("FSEC could not be found.");

            fsecDet.FSEC_Status = model.FSEC_Status;
            fsecDet.FSEC_Assesor_Emp_Id = model.FSEC_Assesor_Emp_Id;
            fsecDet.FSEC_OtherFee = model.FSEC_OtherFee;
            fsecDet.FSEC_ConstructionTax = model.FSEC_ConstructionTax;
            fsecDet.IsSynced = false;
            BFPContext.Entry(fsecDet).State = EntityState.Modified;
            BFPContext.SaveChanges();
        }

        public void UpdateFSECForReleasing(FSECModel model)
        {
            var fsecDet = BFPContext.tblFSECApplication.SingleOrDefault(a=> a.Ref_FSEC_App_Id == model.FSEC_App_Id && a.FSEC_Unit_Id == model.FSEC_Unit_Id);
            if (fsecDet == null) throw new Exception("FSEC could not be found.");

            fsecDet.FSEC_Status = model.FSEC_Status;
            fsecDet.FSEC_Collector_Emp_Id = model.FSEC_Collector_Emp_Id;
            fsecDet.IsSynced = false;
            BFPContext.Entry(fsecDet).State = EntityState.Modified;
            BFPContext.SaveChanges();
        }

        public void SaveRemarks(FSECModel model)
        {
            var fsecDet = BFPContext.tblFSECApplication.SingleOrDefault(a=> a.Ref_FSEC_App_Id == model.FSEC_App_Id && a.FSEC_Unit_Id== model.FSEC_Unit_Id);
            if (fsecDet == null) throw new Exception("FSEC could not be found.");

            fsecDet.FSEC_Remarks = model.FSEC_Remarks;
            fsecDet.IsSynced = false;
            BFPContext.Entry(fsecDet).State = EntityState.Modified;
            BFPContext.SaveChanges();
        }

        public void UpdateToReleaseFSEC(FSECModel model)
        {
            var fsecDet = BFPContext.tblFSECApplication.SingleOrDefault(a=> a.Ref_FSEC_App_Id == model.FSEC_App_Id && a.FSEC_Unit_Id == model.FSEC_Unit_Id);
            if (fsecDet == null) throw new Exception("FSEC could not be found.");

            fsecDet.FSEC_Status = model.FSEC_Status;
            fsecDet.FSEC_Released_Emp_Id = model.FSEC_Released_Emp_Id;
            fsecDet.IsSynced = false;
            BFPContext.Entry(fsecDet).State = EntityState.Modified;
            BFPContext.SaveChanges();
        }

        public List<string> ReleasedFSECNumber(int unitID)
        {
            var rets = (from a in BFPContext.tblFSECApplication
                        join b in BFPContext.tblNonCommercialEstablishments on a.FSEC_Est_Id equals b.Ref_NCE_Id
                        where a.FSEC_Unit_Id == unitID && a.FSEC_Status == (int)FSEC_Status.Released && !string.IsNullOrEmpty(a.FSEC_Number) 
                        orderby a.FSEC_Number
                        select a.FSEC_Number).ToList();

            return rets;
        }

        public List<string> ReleasedORNumber(int unitID)
        {
            var rets = (from a in BFPContext.tblFSECApplication
                        join b in BFPContext.tblNonCommercialEstablishments on a.FSEC_Est_Id equals b.Ref_NCE_Id
                        join c in BFPContext.tblApplicationPayments on a.FSEC_App_Id equals c.AP_Id
                        where a.FSEC_Unit_Id == unitID && a.FSEC_Status == (int)FSEC_Status.Released && !string.IsNullOrEmpty(a.FSEC_Number) && c.AP_ApplicationType == (int)AP_ApplicationType.FSEC
                        orderby c.AP_ORNumber
                        select c.AP_ORNumber.ToString()).ToList();

            return rets;
        }

        public EstablishmentModel NonEstByFsecNumber(string fsecNumber, int unitID)
        {
            var rets = (from a in BFPContext.tblFSECApplication
                join b in BFPContext.tblNonCommercialEstablishments on a.FSEC_Est_Id equals b.Ref_NCE_Id
                where a.FSEC_Unit_Id == unitID && a.FSEC_Number == fsecNumber && a.FSEC_Status == (int)FSEC_Status.Released && !string.IsNullOrEmpty(a.FSEC_Number)
                        select new EstablishmentModel()
                {
                    Est_OwnerName = b.NCE_OwnerName,
                    Est_Unit_Id = b.NCE_Unit_Id,
                    Est_BusinessAddress = b.NCE_ConstructionAddress,
                    Est_MobileNumber = b.NCE_ContactNumber,
                    Est_NatureOfBusiness = b.NCE_NatureOfConstruction,
                    Est_AuthorizedRepresentative = b.NCE_AuthorizedRepresentative
                }).FirstOrDefault();
            
            return rets;
        }

        public FSECOccupancyModel NonEstByORNumber(int orNumber, int unitID)
        {
            var rets = (from a in BFPContext.tblFSECApplication
                        join b in BFPContext.tblNonCommercialEstablishments on a.FSEC_Est_Id equals b.Ref_NCE_Id
                        join c in BFPContext.tblApplicationPayments on a.FSEC_App_Id equals c.AP_Id
                        where a.FSEC_Unit_Id == unitID && c.AP_ORNumber == orNumber && c.AP_ApplicationType == (int)AP_ApplicationType.FSEC && a.FSEC_Status == (int)FSEC_Status.Released && !string.IsNullOrEmpty(a.FSEC_Number)
                        select new FSECOccupancyModel()
                        {
                            Est_OwnerName = b.NCE_OwnerName,
                            Est_Unit_Id = b.NCE_Unit_Id,
                            Est_BusinessAddress = b.NCE_ConstructionAddress,
                            Est_MobileNumber = b.NCE_ContactNumber,
                            Est_NatureOfBusiness = b.NCE_NatureOfConstruction,
                            Est_AuthorizedRepresentative = b.NCE_AuthorizedRepresentative,
                            FSEC_Number = a.FSEC_Number,
                            OR_Number = c.AP_ORNumber > 0 ? c.AP_ORNumber.ToString() : ""
                        }).FirstOrDefault();


            return rets;
        }

        public List<FSECEstablismentsModel> FsecNotification(FSEC_Status status, int unitId)
        {
            int FSEC_Status = (int)status;
            var Establishments = new List<FSECEstablismentsModel>();

            var NonCommercialItems = BFPContext.tblFSECApplication.Join(BFPContext.tblNonCommercialEstablishments,
                                           fsec => fsec.FSEC_Est_Id,
                                           est => est.Ref_NCE_Id,
                                           (fsec, est) => new
                                           {
                                               Est_Id = est.Ref_NCE_Id,
                                               fsec.Ref_FSEC_App_Id,
                                               fsec.FSEC_Status,
                                               fsec.FSEC_EstablishmentType,
                                               fsec.FSEC_Unit_Id,
                                               fsec.FSEC_IsApprovePlanEvaluated,
                                               fsec.FSEC_IsApproveChiefFSES,
                                               fsec.FSEC_IsApproveMarshall
                                           }).Where(a => a.FSEC_Status == FSEC_Status &&
                                         a.FSEC_EstablishmentType == (int)FSEC_EstablishmentType.NonCommercial && a.FSEC_Unit_Id == unitId);
        
            foreach (var item in NonCommercialItems.ToList())
            {
                Establishments.Add(new FSECEstablismentsModel
                {
                    Est_Id = item.Est_Id,
                    FSEC_App_Id = item.Ref_FSEC_App_Id,
                    EstablishmentType = item.FSEC_EstablishmentType,
                    FSEC_IsApprovePlanEvaluated = item.FSEC_IsApprovePlanEvaluated,
                    FSEC_IsApproveChiefFSES = item.FSEC_IsApproveChiefFSES,
                    FSEC_IsApproveMarshall = item.FSEC_IsApproveMarshall,
                    FSEC_status = item.FSEC_Status
                });
            }

            return Establishments;
        }

        public void UpdateTranferFSEC(FSECModel model, int unitId)
        {
            //var fsicDet = BFPContext.tblFSICApplication.Find(model.FSIC_App_Id);

            var fsecDet = BFPContext.tblFSECApplication.FirstOrDefault(a => a.FSEC_Unit_Id == unitId && a.Ref_FSEC_App_Id == model.FSEC_App_Id);
            if (fsecDet == null) throw new Exception("FSEC could not be found.");

            fsecDet.FSEC_Status = (int)FSEC_Status.Collected;
            fsecDet.FSEC_Collected_Date = DateTime.Now;
            fsecDet.FSEC_Collector_Emp_Id = model.FSEC_Collector_Emp_Id;
            fsecDet.IsSynced = false;
            BFPContext.Entry(fsecDet).State = EntityState.Modified;
            BFPContext.SaveChanges();
        }
    }
}
