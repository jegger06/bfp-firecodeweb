using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.Helper;

namespace EBFP.BL.CIS
{
    public class DashboardModel
    {
     
    }

    public class FireFightingModel
    {
        public int RegionId { get; set; }
        public string Region { get; set; }
        public int ProvinceId { get; set; }
        public string Province { get; set; }
        public int UnitId { get; set; }
        public string Unit { get; set; }
        public int TotalCitiesWithBuildingAndFireTrucks { get; set; }
        public int TotalMunicipalitiesWithBuildingAndFireTrucks { get; set; }
        public int WithBuildingAndFireTrucksSubtotal { get; set; }
        public int MunicipalitiesWithFireTrucksOnly { get; set; }
        public int MunicipalitiesWithoutFireTrucks { get; set; }
        public int MunicipalitiesWithFireStationBLDGOnly { get; set; }
        public int MunicipalitiesWithoutFireTrucksAndStation { get; set; }
        public int MunicipalitiesWithoutFireStationBLDG { get; set; }
        public int TotalCities { get; set; }
        public int TotalMunicipalities { get; set; }
        public int TotalCitiesAndMunicipalities { get; set; }
        public int NumberOfFireSubStationBuilding { get; set; }
    }

    public class PersonnelStrengthModel
    {
        public string Region { get; set; }
        public string Province { get; set; }
        public string Unit { get; set; }
        public int NonOfficerRanks { get; set; }
        public int OfficerRanks { get; set; }
        public int NonUniformedPersonnel { get; set; }

        public int TotalPersonnel { get; set; }
        public int IdealPersonnel { get; set; }
        public int FireTrucks { get; set; }
        public int AdditionalManpower { get; set; }
        public int AdminPersonnel { get; set; }
        public int OperationPersonnel { get; set; }
        public int ActualFireTrucks { get; set; }
        public int NUPAdminPersonnel { get; set; }
        public int NUPOperationPersonnel { get; set; }


        public int TotalDBMOfficer { get; set; }
        public int TotalActualOfficer { get; set; }
        public int TotalVarianceOfficer { get; set; }

        public int TotalDBMNonOfficer { get; set; }
        public int TotalActualNonOfficer { get; set; }
        public int TotalVarianceNonOfficer { get; set; }

        public int TotalDBMUnifomedPersonnel { get; set; }
        public int TotalActualUnifomedPersonnel { get; set; }
        public int TotalVarianceUnifomedPersonnel { get; set; }

        public int TotalDBMNonUnifomedPersonnel { get; set; }
        public int TotalActualNonUnifomedPersonnel { get; set; }
        public int TotalVarianceNonUnifomedPersonnel { get; set; }

        public int TotalFireTrucks { get; set; }
    }

    public class DashboardPersonnelModel
    {
        public int RegId { get; set; }
        public string Region { get; set; }
        public int ProvinceId { get; set; }
        public string Province { get; set; }
        public int UnitId { get; set; }
        public string Unit { get; set; }
        public int NUP { get; set; }
        public int Officer { get; set; }
        public int NonOfficer { get; set; }
     
        public int GeneralAdmin { get; set; }
        public int Operations { get; set; }

        public int NUPGeneralAdmin { get; set; }
        public int NUPOperations { get; set; }
    }

    public class FiveYearStatisticModel
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public int Year1 { get; set; }
        public int Year2 { get; set; }
        public int Year3 { get; set; }
        public int Year4 { get; set; }
        public int Year5 { get; set; }
        public decimal Average { get; set; }
    }
    public class FireFeesCollectionModel
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal Year1 { get; set; }
        public decimal Year2 { get; set; }
        public decimal Year3 { get; set; }
        public decimal Year4 { get; set; }
        public decimal Year5 { get; set; }
        public decimal Average { get; set; }

        public decimal TotalYear1 { get; set; }
        public decimal TotalYear2 { get; set; }
        public decimal TotalYear3 { get; set; }
        public decimal TotalYear4 { get; set; }
        public decimal TotalYear5 { get; set; }
        public decimal TotalAverage { get; set; }
    }

    public class FiveYearInspectionStatisticModel
    {

        public int Year { get; set; }
        public int Type1 { get; set; }
        public int Type2 { get; set; }
        public int Type3 { get; set; }
        public int Type4 { get; set; }
        public int Type5 { get; set; }
        public int Type6 { get; set; }
        public int Type7 { get; set; }
        public int Type8 { get; set; }
        public int Type9 { get; set; }
        public int Type10 { get; set; }
        public int Type11 { get; set; }

        public int Total { get; set; }

    }

    public class InspectionOrderDashboardModel
    {
        public int Year { get; set; }
        public int OccupancyType { get; set; }
        public string OccupancyTypeName { get; set; }
        public int Count { get; set; }
    }

    public class FirePreventionDashboardModel
    {
        public int Year { get; set; }
        public int BuildingPlans { get; set; }
        public int FSIC_Occupancy { get; set; }
        public int FSIC_Business_Permit { get; set; }
        public int FSIC_Total { get; set; }
        public int NTC { get; set; }
        public int NTCV { get; set; }
        public int Abatement { get; set; }
        public int Closure { get; set; }
        public decimal Amount_FireCodeFees_Assessed { get; set; }
        public decimal Total_Amount_FireCodeFees_Assessed { get; set; }
    }
}
