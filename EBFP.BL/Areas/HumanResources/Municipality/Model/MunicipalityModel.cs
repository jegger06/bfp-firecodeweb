

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EBFP.BL.Helper;
using EBFP.BL.Inventory;
using AutoMapper;
using EBFP.Helper;

namespace EBFP.BL.HumanResources
{
    public class MunicipalityListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<MunicipalityModel> MunicipalityList { get; set; }
    }

    public class MunicipalitySearchModel
    {
        public string Municipality_NSCB { get; set; }
        public int RegionId { get; set; }
        public int ProvinceId { get; set; }
        public int Municipality_Id { get; set; }
        public string MunicipalityId { get; set; }
        public bool IsSearch { get; set; }
    }

    public class MunicipalityModel
    {
        public MunicipalityModel()
        {
            //SET DEFAULT VALUES
            this.FireRecordsList = new List<FireRecordsModel>();
            this.PopulationList = new List<PopulationModel>();
        }
        
        public string sMunicipality_Id
        {
            get { return Municipality_Id.ToNullSafeString().Encrypt(); }
        }
        public int Municipality_Id { get; set; }
        public int Municipality_Province_Id { get; set; }
        public string Municipality_NSCB { get; set; }
        public string Municipality_Name { get; set; }
        public int? Municipality_Classification { get; set; }

        public decimal? Municipality_LandArea { get; set; }
        private string _sMunicipality_LandArea = "";
        public string sMunicipality_LandArea
        {
            get
            {
                return string.IsNullOrWhiteSpace(_sMunicipality_LandArea) ?
                    Municipality_LandArea.ToString() :
                    _sMunicipality_LandArea;
            }
            set
            {
                this.Municipality_LandArea = Functions.ConvertToSafeDecimal(value);
            }
        }

        public decimal? Municipality_PopulationDensity { get; set; }
        private string _sMunicipality_PopulationDensity = "";
        public string sMunicipality_PopulationDensity
        {
            get
            {
                return string.IsNullOrWhiteSpace(_sMunicipality_PopulationDensity) ?
                    Municipality_PopulationDensity.ToString() :
                    _sMunicipality_PopulationDensity;
            }
            set
            {
                this.Municipality_PopulationDensity = Functions.ConvertToSafeDecimal(value);
            }
        }

        public int? Municipality_IncomeClass { get; set; }
        public string Municipality_FireFightingCapability { get; set; }
        public int? Municipality_Urban { get; set; }
        public int? Municipality_Rural { get; set; }
        public int? Municipality_Barangays { get; set; }
        public int? Municipality_CSUPT { get; set; }
        public int? Municipality_SSUPT { get; set; }
        public int? Municipality_SUPT { get; set; }
        public int? Municipality_CINSP { get; set; }
        public int? Municipality_SINSP { get; set; }
        public int? Municipality_INSP { get; set; }

        public int? Total_Stations { get; set; }
        public int? Total_Trucks { get; set; }
        public int? Total_OtherVehicles { get; set; }

        public int? Municipality_Officers
        {
            get
            {
                return Municipality_CSUPT + Municipality_SSUPT +
                       Municipality_SUPT + Municipality_CINSP +
                       Municipality_SINSP + Municipality_INSP;
            }
        }

        public int? Municipality_SFO4 { get; set; }
        public int? Municipality_SFO3 { get; set; }
        public int? Municipality_SFO2 { get; set; }
        public int? Municipality_SFO1 { get; set; }
        public int? Municipality_FO3 { get; set; }
        public int? Municipality_FO2 { get; set; }
        public int? Municipality_FO1 { get; set; }

        public int? Municipality_NCOS
        {
            get
            {
                return Municipality_SFO4 + Municipality_SFO3 +
                       Municipality_SFO2 + Municipality_SFO1 + Municipality_FO3 +
                       Municipality_FO2 + Municipality_FO1;
            }
        }

        public int? Municipality_UPersonnel
        {
            get
            {
                return Municipality_Officers + Municipality_NCOS;

            }
        }

        public int? Municipality_NUP { get; set; }

        public int? Municipality_TotalPersonnel
        {
            get { return Municipality_UPersonnel + Municipality_NUP; }
        }

        public int? Municipality_Hydrant_Functional { get; set; }
        public int? Municipality_Hydrant_NonFunctional { get; set; }
        public int? Municipality_Hydrant_UnderRepair { get; set; }

        public int? Municipality_Hydrant_Total
        {
            get
            {
                return Municipality_Hydrant_Functional +
                       Municipality_Hydrant_NonFunctional +
                       Municipality_Hydrant_UnderRepair;
            }
        }

        public int Municipality_Reg_Id { get; set; }
        public string Province_Name { get; set; }
        public string Reg_Description { get; set; }
        public int ProvinceId { get; set; }
        public int UnitId { get; set; }
        public int? Municipality_Type { get; set; }
        public bool Municipality_WithBuilding { get; set; }
        public int? Municipality_SCBA_Serviceable { get; set; }
        public int? Municipality_SCBA_ServiceableForReplacement { get; set; }
        public int? Municipality_FireCoat_Serviceable { get; set; }
        public int? Municipality_FireCoat_ServiceableForReplacement { get; set; }
        public int? Municipality_Trouser_Serviceable { get; set; }
        public int? Municipality_Trouser_ServiceableForReplacement { get; set; }
        public int? Municipality_Boots_Serviceable { get; set; }
        public int? Municipality_Boots_ServiceableForReplacement { get; set; }
        public int? Municipality_Gloves_Serviceable { get; set; }
        public int? Municipality_Gloves_ServiceableForReplacement { get; set; }
        public int? Municipality_Helmet_Serviceable { get; set; }
        public int? Municipality_Helmet_ServiceableForReplacement { get; set; }
        public int? Municipality_FireHose15_Serviceable { get; set; }
        public int? Municipality_FireHose15_ServiceableForReplacement { get; set; }
        public int? Municipality_FireHose25_Serviceable { get; set; }
        public int? Municipality_FireHose25_ServiceableForReplacement { get; set; }
        public int? Municipality_FireNozzle15_Serviceable { get; set; }
        public int? Municipality_FireNozzle15_ServiceableForReplacement { get; set; }
        public int? Municipality_FireNozzle25_Serviceable { get; set; }
        public int? Municipality_FireNozzle25_ServiceableForReplacement { get; set; }
        public List<FireRecordsModel> FireRecordsList { get; set; }
        public List<PopulationModel> PopulationList { get; set; }
    }

    public class PersonnelSearchModel
    {
        public int Municipality_Id { get; set; }
        public int? Station_Id { get; set; }
    }

    public class PPEDashboardModel
    {
        public int HelmetExisting { get; set; }
        public decimal HelmetServiceablePecent { get; set; }
        public int HelmetShortage { get; set; }
        public decimal HelmetShortagePercent { get; set; }

        public int TrouserExisting { get; set; }
        public decimal TrouserServiceablePecent { get; set; }
        public int TrouserShortage { get; set; }
        public decimal TrouserShortagePercent { get; set; }

        public int CoatExisting { get; set; }
        public decimal CoatServiceablePecent { get; set; }
        public int CoatShortage { get; set; }
        public decimal CoatShortagePercent { get; set; }

        public int GlovesExisting { get; set; }
        public decimal GlovesServiceablePecent { get; set; }
        public int GlovesShortage { get; set; }
        public decimal GlovesShortagePercent { get; set; }

        public int BoatsExisting { get; set; }
        public decimal BoatsServiceablePecent { get; set; }
        public int BoatsShortage { get; set; }
        public decimal BoatsShortagePercent { get; set; }

        public int BootsExisting { get; set; }
        public decimal BootsServiceablePecent { get; set; }
        public int BootsShortage { get; set; }
        public decimal BootsShortagePercent { get; set; }

        public int SCBAExisting { get; set; }
        public decimal SCBAServiceablePecent { get; set; }
        public int SCBAShortage { get; set; }
        public decimal SCBAShortagePercent { get; set; }


    }
}