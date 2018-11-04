using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EBFP.DataAccess;
using Queries.Core.Repositories;
using EBFP.BL.FSEC;
using EBFP.BL.Helper;

namespace EBFP.BL.Establishment
{
    public class NonCommercialEstablishmentBL :
        Repository<tblNonCommercialEstablishments, NonCommercialEstablishmentModel>, INonCommercialEstablishment
    {
        public NonCommercialEstablishmentBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }

        public void CreateMapping()
        {
            Mapper.CreateMap<tblNonCommercialEstablishments, NonCommercialEstablishmentModel>().ReverseMap();
            Mapper.CreateMap<List<tblNonCommercialEstablishments>, List<NonCommercialEstablishmentModel>>().ReverseMap();
            Mapper.CreateMap<List<tblNonCommercialEstablishments>, List<NonCommercialEstablishmentModel>>();
        }
        public void SyncNonCommercialEstLocalToServer(List<NonCommercialEstablishmentModel> nonCommercialestablishments)
        {
            foreach (var est in nonCommercialestablishments)
            {
                var NonCommercialEstablishment = BFPContext.tblNonCommercialEstablishments
                    .FirstOrDefault(a => a.Ref_NCE_Id == est.Ref_NCE_Id && a.NCE_Unit_Id == est.NCE_Unit_Id);

                if (NonCommercialEstablishment == null)
                {
                    NonCommercialEstablishment = new tblNonCommercialEstablishments();
                    Mapper.Map(est, NonCommercialEstablishment);
                    NonCommercialEstablishment.NCE_Id = 0;
                    NonCommercialEstablishment.Ref_NCE_Id = est.Ref_NCE_Id;
                    BFPContext.tblNonCommercialEstablishments.Add(NonCommercialEstablishment);
                }
                else
                {
                    var NCE_Id = NonCommercialEstablishment.NCE_Id;
                    Mapper.Map(est, NonCommercialEstablishment);
                    NonCommercialEstablishment.Ref_NCE_Id = est.Ref_NCE_Id;
                    NonCommercialEstablishment.NCE_Id = NCE_Id;
                }
            }

            BFPContext.SaveChanges();
        }

        ///

        public string SaveNonCommercialEst(NonCommercialEstablishmentModel model)
        {
            var nonCommercialEstablishment = new tblNonCommercialEstablishments();

            nonCommercialEstablishment.NCE_OwnerName = model.NCE_OwnerName;
            nonCommercialEstablishment.NCE_ConstructionAddress = model.NCE_ConstructionAddress;
            nonCommercialEstablishment.NCE_AuthorizedRepresentative = model.NCE_AuthorizedRepresentative;
            nonCommercialEstablishment.NCE_ContactNumber = model.NCE_ContactNumber;
            nonCommercialEstablishment.NCE_Unit_Id = model.NCE_Unit_Id;
            nonCommercialEstablishment.NCE_Created_Emp_Id = model.NCE_Created_Emp_Id;
            nonCommercialEstablishment.NCE_CreatedDate = model.NCE_CreatedDate;
            nonCommercialEstablishment.NCE_LastUpdate_Emp_Id = model.NCE_LastUpdate_Emp_Id;
            nonCommercialEstablishment.NCE_LastUpdateDate = model.NCE_LastUpdateDate;

            var result = BFPContext.tblNonCommercialEstablishments.Add(nonCommercialEstablishment);
            BFPContext.SaveChanges();

            return result.Ref_NCE_Id;
        }
        
        public NCEOPSModel GetNCE(string nceId, int unitId)
        {
            var nceModel = new NCEOPSModel();

            var nceDet = (
                   from a in BFPContext.tblNonCommercialEstablishments
                   join b in BFPContext.tblUnits on a.NCE_Unit_Id equals b.Unit_Id
                   join c in BFPContext.tblCityMunicipality on b.Unit_Municipality_Id equals c.Municipality_Id
                   join d in BFPContext.tblProvinces on c.Municipality_Province_Id equals d.Province_Id
                   join e in BFPContext.tblRegions on d.Region_Id equals e.Reg_Id
                   join f in BFPContext.tblFSECApplication on a.Ref_NCE_Id equals f.FSEC_Est_Id
                   where a.Ref_NCE_Id == nceId && a.NCE_Unit_Id == unitId
                   select new
                   {
                       a.NCE_OwnerName,
                       a.NCE_ConstructionAddress,
                       b.Unit_StationName,
                       f.FSEC_OPS_Number,
                       f.FSEC_Assesed_Date,
                       b.Unit_Address,
                       d.Province_Name,
                       e.Reg_Title,
                       f.FSEC_OtherFee,
                       f.FSEC_ConstructionTax,
                       Assessor = BFPContext.tblEmployees.FirstOrDefault(g => g.Emp_Id == f.FSEC_Assesor_Emp_Id)
                   }).FirstOrDefault();

            if (nceDet != null)
            {
                nceModel.NCE_OwnerName = nceDet.NCE_OwnerName;
                nceModel.NCE_ConstructionAddress = nceDet.NCE_ConstructionAddress;
                nceModel.Unit_StationName = nceDet.Unit_StationName;
                nceModel.FSEC_OPS_Number = nceDet.FSEC_OPS_Number;
                nceModel.FSEC_Assesed_Date = nceDet.FSEC_Assesed_Date;
                nceModel.Unit_Address = nceDet.Unit_Address;
                nceModel.Province_Name = nceDet.Province_Name;
                nceModel.Reg_Title = nceDet.Reg_Title;
                nceModel.FSEC_OtherFee = nceDet.FSEC_OtherFee;
                nceModel.FSEC_ConstructionTax = nceDet.FSEC_ConstructionTax;
                var employee = nceDet.Assessor;
                if (employee != null)
                {
                    var rank = BFPContext.tblRanks.FirstOrDefault(a => a.Rank_Id == employee.Emp_Curr_Rank);
                    var middleInitial = employee.Emp_MiddleName[0].ToString() ?? "";
                    if (rank != null)
                        nceModel.Assessor = rank.Rank_Name + " " + employee.Emp_FirstName + " " + middleInitial + " " + employee.Emp_LastName;
                    else
                        nceModel.Assessor = employee.Emp_FirstName + " " + middleInitial + " " + employee.Emp_LastName;

                    if (!string.IsNullOrEmpty(employee.Emp_SuffixName) && employee.Emp_SuffixName.ToLower() != "n/a" && employee.Emp_SuffixName.ToLower() != "none")
                        nceModel.Assessor = nceModel.Assessor + " " + employee.Emp_SuffixName;
                }

            }
            return nceModel;
        }

        public FSECNCEReportModel GetNCEReport(string nceId, int unitId)
        {
            var nceModel = new FSECNCEReportModel();

            var nceDet = (
                  from a in BFPContext.tblNonCommercialEstablishments
                  join b in BFPContext.tblUnits on a.NCE_Unit_Id equals b.Unit_Id
                  join c in BFPContext.tblCityMunicipality on b.Unit_Municipality_Id equals c.Municipality_Id
                  join d in BFPContext.tblProvinces on c.Municipality_Province_Id equals d.Province_Id
                  join e in BFPContext.tblRegions on d.Region_Id equals e.Reg_Id
                  join f in BFPContext.tblFSECApplication on a.Ref_NCE_Id equals f.FSEC_Est_Id
                  join g in BFPContext.tblApplicationPayments on f.Ref_FSEC_App_Id equals g.AP_App_Id into appPayment
                  from payment in appPayment.DefaultIfEmpty()
                  where a.Ref_NCE_Id == nceId && a.NCE_Unit_Id == unitId
                  select new
                  {
                      a.NCE_OwnerName,
                      a.NCE_ConstructionAddress,
                      a.NCE_NatureOfConstruction,
                      b.Unit_StationName,
                      b.Unit_Address,
                      d.Province_Name,
                      e.Reg_Title,
                      b.Unit_PhoneNumber,
                      a.NCE_AuthorizedRepresentative,
                      AP_ORNumber = payment != null ? payment.AP_ORNumber : 0,
                      AP_ORDate = payment != null ? payment.AP_ORDate : new System.DateTime(),
                      AP_AmountTendered = payment != null ? payment.AP_AmountTendered : 0,
                      AP_ORAmount = payment != null ? payment.AP_ORAmount : 0,
                      f.FSEC_Number,
                      f.FSEC_IsManual,
                      b.Unit_ChiefFSES_Signature,
                      b.Unit_FireMarshall_Signature,
                      fireMarshall = b.Unit_FireMarshall_Emp_Id != null ? BFPContext.tblEmployees.FirstOrDefault(h => h.Emp_Id == b.Unit_FireMarshall_Emp_Id) : null,
                      chiefFSES = b.Unit_ChiefFSES_Emp_Id != null ? BFPContext.tblEmployees.FirstOrDefault(h => h.Emp_Id == b.Unit_ChiefFSES_Emp_Id) : null
                  }).FirstOrDefault();
            //var nceDet2= (
            //       from a in BFPContext.tblNonCommercialEstablishments
            //       join b in BFPContext.tblUnits on a.NCE_Unit_Id equals b.Unit_Id
            //       join c in BFPContext.tblCityMunicipality on b.Unit_Municipality_Id equals c.Municipality_Id
            //       join d in BFPContext.tblProvinces on c.Municipality_Province_Id equals d.Province_Id
            //       join e in BFPContext.tblRegions on d.Region_Id equals e.Reg_Id
            //       join f in BFPContext.tblFSECApplication on a.NCE_Id equals f.FSEC_Est_Id
            //       join g in BFPContext.tblApplicationPayments on f.FSEC_App_Id equals g.AP_App_Id
            //       where a.NCE_Id == nceId && a.NCE_Unit_Id == unitId
            //       select new
            //       {
            //           a.NCE_OwnerName,
            //           a.NCE_ConstructionAddress,
            //           a.NCE_NatureOfConstruction,
            //           b.Unit_StationName,
            //           b.Unit_Address,
            //           d.Province_Name,
            //           e.Reg_Title,
            //           b.Unit_PhoneNumber,
            //           a.NCE_AuthorizedRepresentative,
            //           g.AP_ORNumber,
            //           g.AP_ORDate,
            //           g.AP_AmountTendered,
            //           f.FSEC_Number,
            //           b.Unit_ChiefFSES_Signature,
            //           b.Unit_FireMarshall_Signature,
            //           fireMarshall = b.Unit_FireMarshall_Emp_Id != null ? BFPContext.tblEmployees.FirstOrDefault(h => h.Emp_Id == b.Unit_FireMarshall_Emp_Id) : null,
            //           chiefFSES = b.Unit_ChiefFSES_Emp_Id != null ? BFPContext.tblEmployees.FirstOrDefault(h => h.Emp_Id == b.Unit_ChiefFSES_Emp_Id) : null
            //       }).FirstOrDefault();

            if (nceDet != null)
            {
                //nceModel.BusinessName = fsicDet.Est_BusinessName;
                nceModel.OwnerName = string.IsNullOrEmpty(nceDet.NCE_OwnerName) ? nceDet.NCE_AuthorizedRepresentative : nceDet.NCE_OwnerName;
                nceModel.BusinessAddress = nceDet.NCE_ConstructionAddress;
                nceModel.Region = nceDet.Reg_Title ?? "";
                nceModel.Province = nceDet.Province_Name ?? "";
                nceModel.StationName = nceDet.Unit_StationName ?? "";
                nceModel.PhoneNumber = nceDet.Unit_PhoneNumber ?? "";
                nceModel.ORNumber = nceDet.AP_ORNumber;
                nceModel.PaymentDate = nceDet.AP_ORDate;
                nceModel.AmountPaid = nceDet.AP_ORAmount ?? 0;
                nceModel.FSECNumber = nceDet.FSEC_Number;
                nceModel.Unit_Address = nceDet.Unit_Address;
                nceModel.ChiefFSESSignature = nceDet.Unit_ChiefFSES_Signature;
                nceModel.FireMarshallSignature = nceDet.Unit_FireMarshall_Signature;
                nceModel.NCE_NatureOfConstruction = nceDet.NCE_NatureOfConstruction;
                nceModel.FSEC_IsManual = nceDet.FSEC_IsManual;
                if (nceDet.fireMarshall != null)
                {
                    var rank = BFPContext.tblRanks.FirstOrDefault(a => a.Rank_Id == nceDet.fireMarshall.Emp_Curr_Rank);
                    var middleInitial = nceDet.fireMarshall.Emp_MiddleName[0].ToString() ?? "";
                    if (rank != null)
                        nceModel.FireMarshall = rank.Rank_Name + " " + nceDet.fireMarshall.Emp_FirstName + " " + middleInitial + " " + nceDet.fireMarshall.Emp_LastName;
                    else
                        nceModel.FireMarshall = nceDet.fireMarshall.Emp_FirstName + " " + middleInitial + " " + nceDet.fireMarshall.Emp_LastName;

                    if (!string.IsNullOrEmpty(nceDet.fireMarshall.Emp_SuffixName) && nceDet.fireMarshall.Emp_SuffixName.ToLower() != "n/a" && nceDet.fireMarshall.Emp_SuffixName.ToLower() != "none")
                        nceModel.FireMarshall = nceModel.FireMarshall + " " + nceDet.fireMarshall.Emp_SuffixName;
                }
                else
                {
                    nceModel.FireMarshall = "";
                }
                if (nceDet.chiefFSES != null)
                {
                    var rank = BFPContext.tblRanks.FirstOrDefault(a => a.Rank_Id == nceDet.chiefFSES.Emp_Curr_Rank);
                    var middleInitial = nceDet.chiefFSES.Emp_MiddleName[0].ToString() ?? "";
                    if (rank != null)
                        nceModel.ChiefFSES = rank.Rank_Name + " " + nceDet.chiefFSES.Emp_FirstName + " " + middleInitial + " " + nceDet.chiefFSES.Emp_LastName;
                    else
                        nceModel.ChiefFSES = nceDet.chiefFSES.Emp_FirstName + " " + middleInitial + " " + nceDet.chiefFSES.Emp_LastName;

                    if (!string.IsNullOrEmpty(nceDet.chiefFSES.Emp_SuffixName) && nceDet.chiefFSES.Emp_SuffixName.ToLower() != "n/a" && nceDet.chiefFSES.Emp_SuffixName.ToLower() != "none")
                        nceModel.ChiefFSES = nceModel.ChiefFSES + " " + nceDet.chiefFSES.Emp_SuffixName;
                }
                else
                {
                    nceModel.ChiefFSES = "";
                }
            }
            return nceModel;
        }

        public NCEOPSModel GetNonCommercialOtherFee(string nceId, int unitId)
        {
            var nceModel = new NCEOPSModel();

            var nceDet = (
                   from a in BFPContext.tblNonCommercialEstablishments
                   join b in BFPContext.tblUnits on a.NCE_Unit_Id equals b.Unit_Id
                   join c in BFPContext.tblCityMunicipality on b.Unit_Municipality_Id equals c.Municipality_Id
                   join d in BFPContext.tblProvinces on c.Municipality_Province_Id equals d.Province_Id
                   join e in BFPContext.tblRegions on d.Region_Id equals e.Reg_Id
                   join f in BFPContext.tblOtherFees on a.Ref_NCE_Id equals f.OF_Est_Id
                   where a.Ref_NCE_Id == nceId && a.NCE_Unit_Id == unitId
                   select new
                   {
                       a.NCE_OwnerName,
                       a.NCE_ConstructionAddress,
                       b.Unit_StationName,
                       f.OF_OPS_Number,
                       f.OF_Assesed_Date,
                       b.Unit_Address,
                       d.Province_Name,
                       e.Reg_Title,
                       f.OF_CollectionType,
                       f.OF_Fee,
                       Assessor = BFPContext.tblEmployees.FirstOrDefault(g => g.Emp_Id == f.OF_Assesor_Emp_Id)
                   }).FirstOrDefault();

            if (nceDet != null)
            {
                nceModel.NCE_OwnerName = nceDet.NCE_OwnerName;
                nceModel.NCE_ConstructionAddress = nceDet.NCE_ConstructionAddress;
                nceModel.Unit_StationName = nceDet.Unit_StationName;
                nceModel.OF_OPS_Number = nceDet.OF_OPS_Number;
                nceModel.OF_Assesed_Date = nceDet.OF_Assesed_Date;
                nceModel.Unit_Address = nceDet.Unit_Address;
                nceModel.Province_Name = nceDet.Province_Name;
                nceModel.Reg_Title = nceDet.Reg_Title;
                nceModel.OF_Fee = nceDet.OF_Fee;
                nceModel.OF_Collection_Type = nceDet.OF_CollectionType;
                var employee = nceDet.Assessor;
                if (employee != null)
                {
                    var rank = BFPContext.tblRanks.FirstOrDefault(a => a.Rank_Id == employee.Emp_Curr_Rank);
                    var middleInitial = employee.Emp_MiddleName[0].ToString() ?? "";
                    if (rank != null)
                        nceModel.Assessor = rank.Rank_Name + " " + employee.Emp_FirstName + " " + middleInitial + " " + employee.Emp_LastName;
                    else
                        nceModel.Assessor = employee.Emp_FirstName + " " + middleInitial + " " + employee.Emp_LastName;

                    if (!string.IsNullOrEmpty(employee.Emp_SuffixName) && employee.Emp_SuffixName.ToLower() != "n/a" && employee.Emp_SuffixName.ToLower() != "none")
                        nceModel.Assessor = nceModel.Assessor + " " + employee.Emp_SuffixName;
                }

            }
            return nceModel;
        }

        public List<string> NonCommercialEstablishmentsOwnerName(int unitID)
        {
            var rets = (from a in BFPContext.tblFSECApplication
                        join b in BFPContext.tblNonCommercialEstablishments on a.FSEC_Est_Id equals b.Ref_NCE_Id
                        where a.FSEC_Unit_Id == unitID && a.FSEC_Status == (int)FSEC_Status.Released && !string.IsNullOrEmpty(a.FSEC_Number)
                        orderby b.NCE_OwnerName
                        select b.NCE_OwnerName).ToList();

            return rets;
        }

        public FSECOccupancyModel NonEstByOwnerName(string ownerName, int unitID)
        {
            var fsecDet = (from a in BFPContext.tblFSECApplication
                           join b in BFPContext.tblNonCommercialEstablishments on a.FSEC_Est_Id equals b.Ref_NCE_Id
                           where a.FSEC_Unit_Id == unitID && b.NCE_OwnerName.Contains(ownerName) && !string.IsNullOrEmpty(a.FSEC_Number) && a.FSEC_Status == (int)FSEC_Status.Released
                           select a.FSEC_IsManual).FirstOrDefault();

            if (fsecDet.HasValue == true)
            {
                var rets = (from a in BFPContext.tblFSECApplication
                            join b in BFPContext.tblNonCommercialEstablishments on a.FSEC_Est_Id equals b.Ref_NCE_Id
                            where a.FSEC_Unit_Id == unitID && b.NCE_OwnerName.Contains(ownerName) && !string.IsNullOrEmpty(a.FSEC_Number) && a.FSEC_Status == (int)FSEC_Status.Released
                            select new FSECOccupancyModel()
                            {
                                Est_OwnerName = b.NCE_OwnerName,
                                Est_Unit_Id = b.NCE_Unit_Id,
                                Est_BusinessAddress = b.NCE_ConstructionAddress,
                                Est_MobileNumber = b.NCE_ContactNumber,
                                Est_AuthorizedRepresentative = b.NCE_AuthorizedRepresentative,
                                FSEC_Number = a.FSEC_Number,
                                OR_Number = ""
                            }).FirstOrDefault();
                return rets;
            }
            else
            {
                var rets = (from a in BFPContext.tblFSECApplication
                            join b in BFPContext.tblNonCommercialEstablishments on a.FSEC_Est_Id equals b.Ref_NCE_Id
                            join c in BFPContext.tblApplicationPayments on a.FSEC_App_Id equals c.AP_Id
                            where a.FSEC_Unit_Id == unitID && b.NCE_OwnerName.Contains(ownerName) && c.AP_ApplicationType == (int)AP_ApplicationType.FSEC && !string.IsNullOrEmpty(a.FSEC_Number) && a.FSEC_Status == (int)FSEC_Status.Released
                            select new FSECOccupancyModel()
                            {
                                Est_OwnerName = b.NCE_OwnerName,
                                Est_Unit_Id = b.NCE_Unit_Id,
                                Est_BusinessAddress = b.NCE_ConstructionAddress,
                                Est_MobileNumber = b.NCE_ContactNumber,
                                Est_AuthorizedRepresentative = b.NCE_AuthorizedRepresentative,
                                FSEC_Number = a.FSEC_Number,
                                OR_Number = c.AP_ORNumber > 0 ? c.AP_ORNumber.ToString() : ""
                            }).FirstOrDefault();
                return rets;
            }
        }
    }
}