using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EBFP.DataAccess;
using Queries.Core.Repositories;
using System;
using EBFP.BL.Helper;
using System.Data.Entity;
using System.Linq.Dynamic;

namespace EBFP.BL.Administration
{
    public class ORSeriesBL : Repository<tblORSeries, ORSeriesModel>, IORSeries
    {
        public ORSeriesBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public void SyncORSeriesLocalToServer(List<ORSeriesModel> orSeries)
        {
            foreach (var or in orSeries)
            {
                var ORSeries = BFPContext.tblORSeries
                    .FirstOrDefault(a => a.Ref_OR_Id == or.Ref_OR_Id && a.OR_Unit_Id == or.OR_Unit_Id);

                if (ORSeries == null)
                {
                    ORSeries = new tblORSeries();
                    Mapper.Map(or, ORSeries);
                    BFPContext.tblORSeries.Add(ORSeries);
                }
                else
                {
                    var orId = ORSeries.OR_Id;
                    Mapper.Map(or, ORSeries);
                    ORSeries.OR_Id = orId;
                }
            }

            BFPContext.SaveChanges();
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblORSeries, ORSeriesModel>().ReverseMap();
            Mapper.CreateMap<List<tblORSeries>, List<ORSeriesModel>>().ReverseMap();
            Mapper.CreateMap<List<tblORSeries>, List<ORSeriesModel>>();
        }

        ////
        public long GetORNumber(int unitId)
        {
            var LastOR = BFPContext.tblApplicationPayments
                                            .Where(a => a.AP_Unit_Id == unitId)
                                            .OrderByDescending(a => a.AP_ORNumber)
                                            .Select(a => a.AP_ORNumber)
                                            .FirstOrDefault();

            if (LastOR > 0)
                return Convert.ToInt32(LastOR) + 1;
            return 1;
        }

        public void ValidateOR(int orSeries, int unitId)
        {
            var ORSeries = BFPContext.tblORSeries
                                    .Where(a => a.OR_Unit_Id == unitId && a.IsDeleted != true)
                                    .OrderByDescending(a => a.OR_EndSeries)
                                    .FirstOrDefault();

            if ((ORSeries == null))
                throw new Exception("No existing OR series yet.  Please create one to continue.");

            if ((orSeries > ORSeries.OR_EndSeries))
                throw new Exception("OR number does not exist.");

            var Payment = BFPContext.tblApplicationPayments
                                    .FirstOrDefault(a => a.AP_ORNumber == orSeries && a.AP_Unit_Id == unitId);

            if (Payment != null)
                throw new Exception("OR number already used.");

            var SploiledORSeries = BFPContext.tblSpoiledOR
                                           .FirstOrDefault(a => a.SOR_Number == orSeries && a.SOR_Unit_Id == unitId);

            if (SploiledORSeries != null)
                throw new Exception("OR number already spoiled.");
        }

        public bool ReleaseOrNumber(int orNumber, int unitId)
        {
            var fsecOR = (from payment in BFPContext.tblApplicationPayments
                          join fsec in BFPContext.tblFSECApplication on payment.AP_App_Id equals fsec.Ref_FSEC_App_Id
                          where
                              payment.AP_Unit_Id == unitId && payment.AP_ApplicationType == (int)AP_ApplicationType.FSEC &&
                              fsec.FSEC_Status >= (int)FSEC_Status.Collected && payment.AP_ORNumber == orNumber
                          select new
                          {
                              payment.AP_ORNumber
                          }).FirstOrDefault();

            if (fsecOR != null)
                return true;

            var fsicOR = (from payment in BFPContext.tblApplicationPayments
                          join fsic in BFPContext.tblFSICApplication on payment.AP_App_Id equals fsic.Ref_FSIC_App_Id
                          where
                              payment.AP_Unit_Id == unitId && payment.AP_ApplicationType == (int)AP_ApplicationType.FSIC &&
                              fsic.FSIC_Status >= (int)FSIC_Status.Collected && payment.AP_ORNumber == orNumber
                          select new
                          {
                              payment.AP_ORNumber
                          }).FirstOrDefault();

            if (fsicOR != null)
                return true;

            var OtherFeesOR = (from payment in BFPContext.tblApplicationPayments
                               join otherFees in BFPContext.tblOtherFees on payment.AP_App_Id equals otherFees.Ref_OF_Id
                               where
                                   payment.AP_Unit_Id == unitId && payment.AP_ApplicationType == (int)AP_ApplicationType.OtherFees &&
                                   otherFees.OF_Status >= (int)OTHERPYMT_Status.Collected && payment.AP_ORNumber == orNumber
                               select new
                               {
                                   payment.AP_ORNumber
                               }).FirstOrDefault();

            if (OtherFeesOR != null)
                return true;

            return false;
        }

        public ORSeriesModel GetOrSeriesById(string orId, int unitId)
        {
            var ORSeriesModel = new ORSeriesModel();
            var ORSeries = BFPContext.tblORSeries
                                           .FirstOrDefault(a => a.Ref_OR_Id == orId && a.OR_Unit_Id == unitId);

            if (ORSeries != null)
                Mapper.Map(ORSeries, ORSeriesModel);

            return ORSeriesModel;
        }

        public void TagAsDeleted(ORSeriesModel model, int empId)
        {
            var ORSeries = BFPContext.tblORSeries
                .FirstOrDefault(a => a.OR_Id == model.OR_Id && a.OR_Unit_Id == model.OR_Unit_Id);

            if (ORSeries != null)
            {
                ORSeries.OR_LastUpdateDate = DateTime.Now;
                ORSeries.OR_LastUpdate_Emp_Id = empId;
                ORSeries.IsDeleted = true;
                ORSeries.IsSynced = false;
            }

            BFPContext.SaveChanges();
        }

        public long GetCountIssuedOR(int unitId)
        {
            var issued = BFPContext.tblORSeries.Where(a => a.OR_Unit_Id == unitId)
                                                .OrderByDescending(a => a.OR_Issue_Date)
                                                .FirstOrDefault();
            if (issued != null)
            {
                return issued.OR_EndSeries;
            }

            return 0;
        }

        public int GetCountUsedOR(int unitId)
        {
            return BFPContext.tblApplicationPayments.Where(a => a.AP_Unit_Id == unitId).Count();
        }

        public int GetCountSpoiledOR(int unitId)
        {
            return BFPContext.tblSpoiledOR.Where(a => a.SOR_Unit_Id == unitId).Count();
        }

        public List<UsedORModel> GetUsedOR(int unitId, DateTime from, DateTime to, bool isFirstLoad = false)
        {
            var fromDt = from.Date;
            var toDt = to.Date;

            var usedORList = (from pay in BFPContext.tblApplicationPayments
                              join fsec in BFPContext.tblFSECApplication on
                              new { AP_App_Id = pay.AP_App_Id, AP_ApplicationType = pay.AP_ApplicationType } equals
                                    new { AP_App_Id = fsec.Ref_FSEC_App_Id, AP_ApplicationType = 1 }
                                    into tempFSEC
                              from fsec in tempFSEC.DefaultIfEmpty()

                              join fsic in BFPContext.tblFSICApplication on
                                            new { AP_App_Id = pay.AP_App_Id, AP_ApplicationType = pay.AP_ApplicationType } equals
                                            new { AP_App_Id = fsic.Ref_FSIC_App_Id, AP_ApplicationType = 2 }
                                            into tempFSIC
                              from fsic in tempFSIC.DefaultIfEmpty()

                              join other in BFPContext.tblOtherFees on
                                            new { AP_App_Id = pay.AP_App_Id, AP_ApplicationType = pay.AP_ApplicationType } equals
                                            new { AP_App_Id = other.Ref_OF_Id, AP_ApplicationType = 4 }
                                            into tempOther
                              from other in tempOther.DefaultIfEmpty()

                              join fsecEst in BFPContext.tblNonCommercialEstablishments on fsec.FSEC_Est_Id equals fsecEst.Ref_NCE_Id
                                          into tempfsecEst
                              from fsecEst in tempfsecEst.DefaultIfEmpty()
                              join fsicEst in BFPContext.tblEstablishments on fsic.FSIC_Est_Id equals fsicEst.Ref_Est_Id
                                          into tempfsicEst
                              from fsicEst in tempfsicEst.DefaultIfEmpty()
                              join otherEst in BFPContext.tblEstablishments on other.OF_Est_Id equals otherEst.Ref_Est_Id
                                          into tempotherEst
                              from otherEst in tempotherEst.DefaultIfEmpty()
                              join otherNonCom in BFPContext.tblNonCommercialEstablishments on other.OF_Est_Id equals otherNonCom.Ref_NCE_Id
                                          into tempotherNonCom
                              from otherNonCom in tempotherNonCom.DefaultIfEmpty()
                              join otherClear in BFPContext.tblOtherClearances on other.OF_Est_Id equals otherClear.Ref_OC_Id
                                          into tempotherClear
                              from otherClear in tempotherClear.DefaultIfEmpty()

                              where pay.AP_Unit_Id == unitId
                              orderby pay.AP_ORNumber
                              select new UsedORModel
                              {
                                  OR_Amount = pay.AP_ORAmount ?? 0,
                                  OR_Date = pay.AP_ORDate,
                                  OR_Number = pay.AP_ORNumber,
                                  OR_Type = pay.AP_ApplicationType == 1 ? "FSEC" :
                                             pay.AP_ApplicationType == 2 ? "FSIC - " + (fsic.FSIC_BusinessType > 0 ? "Business" : "Occupancy") :
                                             pay.AP_ApplicationType == 4 ? "Other Fee - " + (other.OF_CollectionType == 1 ? "Fire Incident" :
                                                                                             other.OF_CollectionType == 2 ? "Hotworks CLR" :
                                                                                             other.OF_CollectionType == 3 ? "Storage CLR" :
                                                                                             other.OF_CollectionType == 4 ? "Conveyance CLR" :
                                                                                             other.OF_CollectionType == 5 ? "Electrical CLR" :
                                                                                             other.OF_CollectionType == 7 ? "LPGAS Instl" :
                                                                                             other.OF_CollectionType == 8 ? "Fire Drill" :
                                                                                             other.OF_CollectionType == 9 ? "Fireworks" :
                                                                                             other.OF_CollectionType == 10 ? "Fumigation/Fogging" :
                                                                                             other.OF_CollectionType == 12 ? "Premium Tax" :
                                                                                             other.OF_CollectionType == 13 ? "Sales Tax" :
                                                                                             other.OF_CollectionType == 14 ? "Proceeds Tax" :
                                                                                             other.OF_CollectionType == 15 ? "AFSS Instl" :
                                                                                             other.OF_CollectionType == 16 ? "Dust Prod. Machine" :
                                                                                             other.OF_CollectionType == 17 ? "Flam/Comb Liq. Stor" :
                                                                                             other.OF_CollectionType == 18 ? "Bldg Srv Equip" :
                                                                                             other.OF_CollectionType == 19 ? "Kitchen Hood" : "") : "",
                                  OR_App_Number = pay.AP_ApplicationType == 1 ? fsec.FSEC_Number :
                                             pay.AP_ApplicationType == 2 ? fsic.FSIC_Number :
                                             pay.AP_ApplicationType == 4 ? other.OF_Number : "",
                                  OR_Est_Name = pay.AP_ApplicationType == 1 ? fsecEst.NCE_OwnerName :
                                             pay.AP_ApplicationType == 2 ? fsicEst.Est_BusinessName :
                                             pay.AP_ApplicationType == 4 ?
                                                    (string.IsNullOrEmpty(otherClear.OC_BusinessName) ? otherClear.OC_BusinessName : otherClear.OC_BusinessName)
                                                    : "",
                                  //pay.AP_ApplicationType == 4 ?
                                  //       (string.IsNullOrEmpty(otherEst.Est_BusinessName) ? otherNonCom.NCE_OwnerName : otherEst.Est_BusinessName)
                                  //       : "",
                                  OR_Est_Address = pay.AP_ApplicationType == 1 ? fsecEst.NCE_ConstructionAddress :
                                             pay.AP_ApplicationType == 2 ? fsicEst.Est_BusinessAddress :
                                             pay.AP_ApplicationType == 4 ? otherClear.OC_BusinessAddress : "",
                                  OR_BAN_Number = pay.AP_ApplicationType == 1 ? "" :
                                             pay.AP_ApplicationType == 2 ? fsicEst.Est_MP_Number :
                                             pay.AP_ApplicationType == 4 ? "" : "",
                              });

            if (!isFirstLoad)
                usedORList = usedORList.Where(a => (DbFunctions.TruncateTime(a.OR_Date) >= fromDt && DbFunctions.TruncateTime(a.OR_Date) <= toDt));

            return usedORList.ToList();
        }

        public List<NumberOfCustomerProcessedModel> GetNumberOfCustomerProcessed(int unitId, DateTime from, DateTime to)
        {
            var fromDt = from.Date;
            var toDt = to.Date;
            var combineList = new List<NumberOfCustomerProcessedModel>();
            var estList = (from est in BFPContext.tblEstablishments
                           join fsec in BFPContext.tblFSECApplication on est.Ref_Est_Id equals fsec.FSEC_Est_Id
                                 into tempFSEC
                           from fsec in tempFSEC.DefaultIfEmpty()

                           join fsic in BFPContext.tblFSICApplication on est.Ref_Est_Id equals fsic.FSIC_Est_Id
                                         into tempFSIC
                           from fsic in tempFSIC.DefaultIfEmpty()

                           join other in BFPContext.tblOtherFees on est.Ref_Est_Id equals other.OF_Est_Id
                                         into tempOther
                           from other in tempOther.DefaultIfEmpty()

                           where est.Est_Unit_Id == unitId
                           select new NumberOfCustomerProcessedModel
                           {
                               Processed_Type = (fsec != null) ? "FSEC" :
                                           (fsic != null) ? "FSIC" :
                                           (other != null) ? "Other Fee" : "",
                               Processed_App_Number = (fsec != null) ? fsec.FSEC_Number :
                                           (fsic != null) ? fsic.FSIC_Number :
                                           (other != null) ? other.OF_Number : "",
                               Processed_Est_Name = est.Est_BusinessName,
                               Processed_Date =
                                          (fsic != null) ?
                                                 (fsic.FSIC_Released_Date.HasValue ? fsic.FSIC_Released_Date :
                                                  fsic.FSIC_Collected_Date.HasValue ? fsic.FSIC_Collected_Date :
                                                   fsic.FSIC_Assesed_Date.HasValue ? fsic.FSIC_Assesed_Date :
                                                   fsic.FSIC_Evaluated_Date.HasValue ? fsic.FSIC_Evaluated_Date :
                                                   fsic.FSIC_ApplicationDate) :
                                          (other != null) ?
                                           (other.OF_Released_Date.HasValue ? other.OF_Released_Date :
                                                  other.OF_Collected_Date.HasValue ? other.OF_Collected_Date :
                                                   other.OF_Assesed_Date.HasValue ? other.OF_Assesed_Date :
                                                   other.OF_Evaluated_Date.HasValue ? other.OF_Evaluated_Date :
                                                   other.OF_ApplicationDate) : (DateTime?)null
                           });
            estList = estList.Where(a => ((DbFunctions.TruncateTime(a.Processed_Date) >= fromDt && DbFunctions.TruncateTime(a.Processed_Date) <= toDt)));
            combineList = estList.ToList();


            var nonCommercialList = (from nonCom in BFPContext.tblNonCommercialEstablishments
                                     join fsec in BFPContext.tblFSECApplication on nonCom.Ref_NCE_Id equals fsec.FSEC_Est_Id
                                           into tempFSEC
                                     from fsec in tempFSEC.DefaultIfEmpty()

                                     join fsic in BFPContext.tblFSICApplication on nonCom.Ref_NCE_Id equals fsic.FSIC_Est_Id
                                                   into tempFSIC
                                     from fsic in tempFSIC.DefaultIfEmpty()

                                     join other in BFPContext.tblOtherFees on nonCom.Ref_NCE_Id equals other.OF_Est_Id
                                                   into tempOther
                                     from other in tempOther.DefaultIfEmpty()

                                     where nonCom.NCE_Unit_Id == unitId
                                     select new NumberOfCustomerProcessedModel
                                     {
                                         Processed_Type = (fsec != null) ? "FSEC" :
                                                     (fsic != null) ? "FSIC" :
                                                     (other != null) ? "Other Fee" : "",
                                         Processed_App_Number = (fsec != null) ? fsec.FSEC_Number :
                                                     (fsic != null) ? fsic.FSIC_Number :
                                                     (other != null) ? other.OF_Number : "",
                                         Processed_Est_Name = nonCom.NCE_OwnerName,
                                         Processed_Date = (fsec != null) ?
                                                              (fsec.FSEC_Released_Date.HasValue ? fsec.FSEC_Released_Date :
                                                               fsec.FSEC_Collected_Date.HasValue ? fsec.FSEC_Collected_Date :
                                                                fsec.FSEC_Assesed_Date.HasValue ? fsec.FSEC_Assesed_Date :
                                                                fsec.FSEC_Evaluated_Date.HasValue ? fsec.FSEC_Evaluated_Date :
                                                                fsec.FSEC_ApplicationDate) :
                                                    (fsic != null) ?
                                                           (fsic.FSIC_Released_Date.HasValue ? fsic.FSIC_Released_Date :
                                                            fsic.FSIC_Collected_Date.HasValue ? fsic.FSIC_Collected_Date :
                                                             fsic.FSIC_Assesed_Date.HasValue ? fsic.FSIC_Assesed_Date :
                                                             fsic.FSIC_Evaluated_Date.HasValue ? fsic.FSIC_Evaluated_Date :
                                                             fsic.FSIC_ApplicationDate) :
                                                    (other != null) ?
                                                     (other.OF_Released_Date.HasValue ? other.OF_Released_Date :
                                                            other.OF_Collected_Date.HasValue ? other.OF_Collected_Date :
                                                             other.OF_Assesed_Date.HasValue ? other.OF_Assesed_Date :
                                                             other.OF_Evaluated_Date.HasValue ? other.OF_Evaluated_Date :
                                                             other.OF_ApplicationDate) : (DateTime?)null
                                     });
            nonCommercialList = nonCommercialList.Where(a => ((DbFunctions.TruncateTime(a.Processed_Date) >= fromDt && DbFunctions.TruncateTime(a.Processed_Date) <= toDt)));
            var nonComList = nonCommercialList.ToList();
            combineList.AddRange(nonComList);

            return combineList;
        }

        public List<NumberOfCustomerProcessedModel> GetNumberOfCustomerProcessedx(int unitId, DateTime from, DateTime to)
        {
            var fromDt = from.Date;
            var toDt = to.Date;

            var usedORList = (from pay in BFPContext.tblApplicationPayments
                              join fsec in BFPContext.tblFSECApplication on
                              new { AP_App_Id = pay.AP_App_Id, AP_ApplicationType = pay.AP_ApplicationType } equals
                                    new { AP_App_Id = fsec.Ref_FSEC_App_Id, AP_ApplicationType = 1 }
                                    into tempFSEC
                              from fsec in tempFSEC.DefaultIfEmpty()

                              join fsic in BFPContext.tblFSICApplication on
                                            new { AP_App_Id = pay.AP_App_Id, AP_ApplicationType = pay.AP_ApplicationType } equals
                                            new { AP_App_Id = fsic.Ref_FSIC_App_Id, AP_ApplicationType = 2 }
                                            into tempFSIC
                              from fsic in tempFSIC.DefaultIfEmpty()

                              join other in BFPContext.tblOtherFees on
                                            new { AP_App_Id = pay.AP_App_Id, AP_ApplicationType = pay.AP_ApplicationType } equals
                                            new { AP_App_Id = other.Ref_OF_Id, AP_ApplicationType = 4 }
                                            into tempOther
                              from other in tempOther.DefaultIfEmpty()

                              join fsecEst in BFPContext.tblNonCommercialEstablishments on fsec.FSEC_Est_Id equals fsecEst.Ref_NCE_Id
                                          into tempfsecEst
                              from fsecEst in tempfsecEst.DefaultIfEmpty()
                              join fsicEst in BFPContext.tblEstablishments on fsic.FSIC_Est_Id equals fsicEst.Ref_Est_Id
                                          into tempfsicEst
                              from fsicEst in tempfsicEst.DefaultIfEmpty()
                              join otherEst in BFPContext.tblEstablishments on other.OF_Est_Id equals otherEst.Ref_Est_Id
                                          into tempotherEst
                              from otherEst in tempotherEst.DefaultIfEmpty()
                              join otherNonCom in BFPContext.tblNonCommercialEstablishments on other.OF_Est_Id equals otherNonCom.Ref_NCE_Id
                                          into tempotherNonCom
                              from otherNonCom in tempotherNonCom.DefaultIfEmpty()

                              where pay.AP_Unit_Id == unitId
                              orderby pay.AP_ORNumber
                              select new NumberOfCustomerProcessedModel
                              {
                                  Processed_Type = pay.AP_ApplicationType == 1 ? "FSEC" :
                                             pay.AP_ApplicationType == 2 ? "FSIC" :
                                             pay.AP_ApplicationType == 4 ? "Other Fee" : "",
                                  Processed_App_Number = pay.AP_ApplicationType == 1 ? fsec.FSEC_Number :
                                             pay.AP_ApplicationType == 2 ? fsic.FSIC_Number :
                                             pay.AP_ApplicationType == 4 ? other.OF_Number : "",
                                  Processed_Est_Name = pay.AP_ApplicationType == 1 ? fsecEst.NCE_OwnerName :
                                             pay.AP_ApplicationType == 2 ? fsicEst.Est_BusinessName :
                                             pay.AP_ApplicationType == 4 ?
                                                    (string.IsNullOrEmpty(otherEst.Est_BusinessName) ? otherNonCom.NCE_OwnerName : otherEst.Est_BusinessName)
                                                    : "",
                                  Processed_Date = pay.AP_ApplicationType == 1 ?
                                                    (fsec.FSEC_Released_Date.HasValue ? fsec.FSEC_Released_Date :
                                                     fsec.FSEC_Collected_Date.HasValue ? fsec.FSEC_Collected_Date :
                                                      fsec.FSEC_Assesed_Date.HasValue ? fsec.FSEC_Assesed_Date :
                                                      fsec.FSEC_Evaluated_Date.HasValue ? fsec.FSEC_Evaluated_Date :
                                                      fsec.FSEC_ApplicationDate) :
                                             pay.AP_ApplicationType == 2 ?
                                                    (fsic.FSIC_Released_Date.HasValue ? fsic.FSIC_Released_Date :
                                                     fsic.FSIC_Collected_Date.HasValue ? fsic.FSIC_Collected_Date :
                                                      fsic.FSIC_Assesed_Date.HasValue ? fsic.FSIC_Assesed_Date :
                                                      fsic.FSIC_Evaluated_Date.HasValue ? fsic.FSIC_Evaluated_Date :
                                                      fsic.FSIC_ApplicationDate) :
                                             pay.AP_ApplicationType == 4 ?
                                              (other.OF_Released_Date.HasValue ? other.OF_Released_Date :
                                                     other.OF_Collected_Date.HasValue ? other.OF_Collected_Date :
                                                      other.OF_Assesed_Date.HasValue ? other.OF_Assesed_Date :
                                                      other.OF_Evaluated_Date.HasValue ? other.OF_Evaluated_Date :
                                                      other.OF_ApplicationDate) : (DateTime?)null
                                  //fsec.FSEC_ApplicationDate,
                                  //fsec.FSEC_Assesed_Date,
                                  //fsec.FSEC_Collected_Date,
                                  //fsec.FSEC_Evaluated_Date,
                                  //fsec.FSEC_Released_Date,


                                  //fsic.FSIC_ApplicationDate,
                                  //fsic.FSIC_Assesed_Date,
                                  //fsic.FSIC_Collected_Date,
                                  //fsic.FSIC_Evaluated_Date,
                                  //fsic.FSIC_Released_Date,

                                  //other.OF_ApplicationDate,
                                  //other.OF_Assesed_Date,
                                  //other.OF_Collected_Date,
                                  //other.OF_Evaluated_Date,
                                  //other.OF_Released_Date,
                              });


            usedORList = usedORList.Where(a => ((DbFunctions.TruncateTime(a.Processed_Date) >= fromDt && DbFunctions.TruncateTime(a.Processed_Date) <= toDt)));

            return usedORList.ToList();
        }

        public ORListResult GetSpoiledORList(GridInfo gridInfo, int unitId)
        {

            var retORSeries = new List<ORSeriesModel>();

            var SearchTerms = gridInfo.searchOR;
            var orSeries = BFPContext.tblORSeries.Where(a => a.OR_Unit_Id == unitId  && a.IsDeleted != true).AsQueryable();

            if (SearchTerms.StartORSeries > 0)
                orSeries = orSeries.Where(a => a.OR_StartSeries == SearchTerms.StartORSeries);
            if (SearchTerms.EndORSeries > 0)
                orSeries = orSeries.Where(a => a.OR_StartSeries == SearchTerms.EndORSeries);


            gridInfo.recordsTotal = orSeries.Select(a => a.OR_Id).Count();
            var orListResult = orSeries.OrderBy(gridInfo.sortColumnName + " " + gridInfo.sortOrder)
             .Skip(gridInfo.start)
             .Take(gridInfo.length)
             .ToList();

            foreach (var or in orListResult)
            {
                retORSeries.Add(new ORSeriesModel
                {
                    OR_StartSeries = or.OR_StartSeries,
                    OR_CreatedDate = or.OR_CreatedDate,
                    OR_Created_Emp_Id = or.OR_Created_Emp_Id,
                    OR_Unit_Id = or.OR_Unit_Id,
                    OR_Id = or.OR_Id,
                    Ref_OR_Id = or.Ref_OR_Id,
                    OR_Issue_Date = or.OR_Issue_Date,
                    OR_EndSeries = or.OR_EndSeries
                });
            }

            return new ORListResult
            {
                ORList = retORSeries.OrderBy(a => a.OR_Created_Emp_Id).ToList(),
                DatatableInfo = gridInfo
            };
        }

        public void UpdateORSeries(ORSeriesModel model)
        {
            var or = BFPContext.tblORSeries.FirstOrDefault(a => a.OR_Id == model.OR_Id);

            or.OR_StartSeries = model.OR_StartSeries;
            or.OR_EndSeries = model.OR_EndSeries;
            or.OR_Issue_Date = model.OR_Issue_Date;
            or.OR_LastUpdateDate = model.OR_LastUpdateDate;
            or.OR_LastUpdate_Emp_Id = model.OR_LastUpdate_Emp_Id;
            BFPContext.SaveChanges();

        }
    }
}