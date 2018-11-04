using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.BL.HumanResources;
using EBFP.DataAccess;
using EBFP.Helper;
using Queries.Core.Repositories;

namespace EBFP.BL.Inventory
{
    public class MunicipalityBL : Repository<tblCityMunicipality, MunicipalityModel>, IMunicipality
    {
        public MunicipalityBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }
        public void CreateMapping()
        {
            Mapper.CreateMap<tblCityMunicipality, MunicipalityModel>();
            Mapper.CreateMap<tblCityMunicipality, MunicipalityModel>().ReverseMap();
        }

        public void UpdateInventoryMunicipality(MunicipalityModel model)
        {

            var municipalityDet = BFPContext.tblCityMunicipality.FirstOrDefault(a => a.Municipality_Id == model.Municipality_Id);
            if (municipalityDet == null) throw new Exception("Municipality cannot be found!");

            Mapper.Map(model, municipalityDet);


            BFPContext.Entry(municipalityDet).State = EntityState.Modified;
            BFPContext.SaveChanges();

            IInventoryUnitOfWork oUnitOfWork = new InventoryUnitOfWork();
            oUnitOfWork.FireRecord.InsertBulk(model.FireRecordsList, model.Municipality_Id);
            oUnitOfWork.Population.InsertBulk(model.PopulationList, model.Municipality_Id);
            oUnitOfWork.Complete();
        }

        public int GetUnitIdByMunicipality(int municipalityId)
        {
            var res = (from a in BFPContext.tblUnits
                       join b in BFPContext.tblCityMunicipality on a.Unit_Municipality_Id equals b.Municipality_Id
                       where b.Municipality_Id == municipalityId
                       select new
                       {
                           UnitId = a.Unit_Id
                       }).FirstOrDefault();

            if (res != null)
                return res.UnitId;

            return 0;
        }

        public MunicipalitySearchModel GetIdsByMunicipality(int municipalityId)
        {
            var res = BFPContext.tblCityMunicipality.FirstOrDefault(a => a.Municipality_Id == municipalityId);
            var model = new MunicipalitySearchModel();
            if (res != null)
            {
                model.Municipality_Id = res.Municipality_Id;
                model.ProvinceId = res.Municipality_Province_Id;
                model.RegionId = res.tblProvinces?.Region_Id ?? 0;

            }
            return model;
        }

        public List<tblCityMunicipality> FilteredMunicipalities()
        {
            var municipalityList = new List<tblCityMunicipality>();
            var municipality = BFPContext.tblCityMunicipality.AsQueryable();

            if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToRegion))
                municipalityList = municipality.Where(a => a.tblProvinces.Region_Id == CurrentUser.RegionID).ToList();
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToProvince))
                municipalityList = municipality.Where(a => a.tblProvinces.Province_Id == CurrentUser.ProvinceID).ToList();
            else if (PageSecurity.HasAccess(PageArea.AllAreas_RestrictToStation))
                municipalityList = municipality.Where(a => a.Municipality_Id == CurrentUser.MunicipalityID).ToList();

            municipalityList = municipality.ToList();

            return municipalityList;
        }

        public PPEDashboardModel GetPPEDashboardCount(string sMunicipalityId = "")
        {
            if (!string.IsNullOrWhiteSpace(sMunicipalityId))
            {
                var municipalityId = Convert.ToInt32(sMunicipalityId.Decrypt());

                var generalJobFunction = (int) JobFunction.GeneralAdmin;
                var personnelCount = BFPContext.tblEmployees.Count(a => a.tblUnits.Unit_Municipality_Id == municipalityId && 
                                                                a.Emp_Curr_JobFunc != generalJobFunction && a.Emp_Curr_JobFunc != null);
                var equipments = FilteredMunicipalities().FirstOrDefault(a => a.Municipality_Id == municipalityId);

                if (equipments != null)
                {
                    var model = new PPEDashboardModel();
                    model.HelmetExisting = (equipments.Municipality_Helmet_Serviceable ?? 0) + (equipments.Municipality_Helmet_ServiceableForReplacement ?? 0);
                    model.HelmetServiceablePecent = GetPPEServiceablePercentage(model.HelmetExisting, (equipments.Municipality_Helmet_Serviceable ?? 0));
                    model.HelmetShortage = GetPPEShortage(personnelCount, (equipments.Municipality_Helmet_Serviceable ?? 0), (equipments.Municipality_Helmet_ServiceableForReplacement ?? 0));
                    model.HelmetShortagePercent = GetPPEShortagePecentage(personnelCount, model.HelmetShortage);


                    model.TrouserExisting = (equipments.Municipality_Trouser_Serviceable ?? 0) + (equipments.Municipality_Trouser_ServiceableForReplacement ?? 0);
                    model.TrouserServiceablePecent = GetPPEServiceablePercentage(model.TrouserExisting, (equipments.Municipality_Trouser_Serviceable ?? 0));
                    model.TrouserShortage = GetPPEShortage(personnelCount, (equipments.Municipality_Trouser_Serviceable ?? 0), (equipments.Municipality_Trouser_ServiceableForReplacement ?? 0));
                    model.TrouserShortagePercent = GetPPEShortagePecentage(personnelCount, model.TrouserShortage);


                    model.CoatExisting = (equipments.Municipality_FireCoat_Serviceable ?? 0) + (equipments.Municipality_FireCoat_ServiceableForReplacement ?? 0);
                    model.CoatServiceablePecent = GetPPEServiceablePercentage(model.CoatExisting, (equipments.Municipality_FireCoat_Serviceable ?? 0));
                    model.CoatShortage = GetPPEShortage(personnelCount, (equipments.Municipality_FireCoat_Serviceable ?? 0), (equipments.Municipality_FireCoat_ServiceableForReplacement ?? 0));
                    model.CoatShortagePercent = GetPPEShortagePecentage(personnelCount, model.CoatShortage);


                    model.GlovesExisting = (equipments.Municipality_Gloves_Serviceable ?? 0) + (equipments.Municipality_Gloves_ServiceableForReplacement ?? 0);
                    model.GlovesServiceablePecent = GetPPEServiceablePercentage(model.GlovesExisting, (equipments.Municipality_Gloves_Serviceable ?? 0));
                    model.GlovesShortage = GetPPEShortage(personnelCount, (equipments.Municipality_Gloves_Serviceable ?? 0), (equipments.Municipality_Gloves_ServiceableForReplacement ?? 0));
                    model.GlovesShortagePercent = GetPPEShortagePecentage(personnelCount, model.GlovesShortage);


                    model.BootsExisting = (equipments.Municipality_Boots_Serviceable ?? 0) + (equipments.Municipality_Boots_ServiceableForReplacement ?? 0);
                    model.BootsServiceablePecent = GetPPEServiceablePercentage(model.BootsExisting, (equipments.Municipality_Boots_Serviceable ?? 0));
                    model.BootsShortage = GetPPEShortage(personnelCount, (equipments.Municipality_Boots_Serviceable ?? 0), (equipments.Municipality_Boots_ServiceableForReplacement ?? 0));
                    model.BootsShortagePercent = GetPPEShortagePecentage(personnelCount, model.BootsShortage);



                    model.SCBAExisting = (equipments.Municipality_SCBA_Serviceable ?? 0) + (equipments.Municipality_SCBA_ServiceableForReplacement ?? 0);
                    model.SCBAServiceablePecent = GetPPEServiceablePercentage(model.SCBAExisting, (equipments.Municipality_SCBA_Serviceable ?? 0));
                    model.SCBAShortage = GetPPEShortage(personnelCount, (equipments.Municipality_SCBA_Serviceable ?? 0), (equipments.Municipality_SCBA_ServiceableForReplacement ?? 0));
                    model.SCBAShortagePercent = GetPPEShortagePecentage(personnelCount, model.SCBAShortage);

                    return model;
                }
            }

            return new PPEDashboardModel();
        }

        private decimal GetPPEServiceablePercentage(int existing, int serviceable)
        {
            decimal percentage = existing > 0 ? Math.Round((serviceable / (decimal)existing) * 100) : 0;

            return percentage;
        }

        private int GetPPEShortage(int personnelCount, int serviceable, int forReplacement)
        {
            var count = (personnelCount - serviceable) + forReplacement;

            return count;
        }

        private decimal GetPPEShortagePecentage(int personnelCount, int shortage)
        {
            decimal percentage = personnelCount > 0 ? Math.Round((shortage / (decimal)personnelCount) * 100) : 0;

            return percentage;
        }

    }
}