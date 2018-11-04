using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EBFP.BL.Helper;
using EBFP.Helper;

namespace EBFP.BL.CIS
{
    public class PhysicalInventoryReportModel
    {
        public string PI_Dir_Name { get; set; }
        public string PI_IG_Name { get; set; }
        public string PI_Art_Name { get; set; }

        public int PI_Dir_Id { get; set; }

        public string PI_Dir_Code { get; set; }
        public string PI_IG_Code { get; set; }
        public string PI_Art_Code { get; set; }


        public string PI_End_User { get; set; }
        public string PI_Description { get; set; }
        public string PI_PropertyNumber { get; set; }
        public string PI_UnitOfMeasure { get; set; }
        public DateTime? PI_DateAcquired { get; set; }
        public decimal? PI_UnitValue { get; set; }
        public string PI_ARENumber { get; set; }
        public string PI_ICSNUmber { get; set; }
        public string PI_Remarks { get; set; }
        public string PI_Office { get; set; }    
    }

    public class DirectoratesReportModel
    {
        public string Dir_Description { get; set; }
        public int Dir_Id { get; set; }

        public string IG_Code { get; set; }
        public string IG_Name { get; set; }

    }

    public class UnserviceableReportModel
    {
        public UnserviceableReportModel()
        {
            this.UnserviceableItemList = new List<UnserviceableItemList>();
        }

        public List<UnserviceableItemList> UnserviceableItemList { get; set; }

        public int UPI_Id { get; set; }
        public string UPI_WMR { get; set; }
        public string UPI_ReportingOffice { get; set; }
    }

    public class UnserviceableItemList
    {
        public string PI_Description { get; set; }
        public string PI_PropertyNumber { get; set; }
        public DateTime? PI_DateAcquired { get; set; }
        public decimal? PI_UnitValue { get; set; }

    }

    public class SummaryReportModel
    {
        public string IG_Code { get; set; }
        public string IG_Name { get; set; }
        public decimal? TotalCost { get; set; }

    }

    public class PhysicalInventoryList
    {
        public int PI_IG_Id { get; set; }
        public string PI_IG_Code { get; set; }
        public string PI_IG_Description { get; set; }

    }
    public class PhysicalInventorySuppliesModel
    {
        public string SI_Art_Name { get; set; }
        public string SI_Description { get; set; }
        public string SI_StockNumber { get; set; }
        public string SI_UnitOfMeasure { get; set; }       
        public decimal? SI_UnitValue { get; set; }
        public int? SI_Quantity { get; set; }
        public int SI_OnHand { get; set; }
        public decimal SI_TotalAmount { get; set; }
    }

    public class SCBANationWideModel
    {
        public int Serviceable { get; set; }
        public int Unserviceable { get; set; }
        public int UnderRepair { get; set; }
        public int RegId { get; set; }
        public string RegTitle { get; set; }
        public int SCBAServiceable { get; set; }
        public int SCBAServiceableButForRepl { get; set; }
    }

    public class SCBAReportModel
    {
        public string StationName { get; set; }
        public string SubstationName { get; set; }
        public int RegId { get; set; }
        public string MunicipalityName { get; set; }
        public string RegionName { get; set; }
        public int Serviceable { get; set; }
        public int Unserviceable { get; set; }
        public int UnderRepair { get; set; }
        public string Province { get; set; }
        public int SCBAServiceable { get; set; }
        public int SCBAServiceableButForRepl { get; set; }
    }
    public class InventoryReportModel
    {
        public int ProvinceId { get; set; }
        public int Municipality_Id { get; set; }
    }

    public class PPENationWideModel
    {
        public int OperationPersonnel { get; set; }
        public int RegId { get; set; }
        public string RegTitle { get; set; }
        public int FireCoatServiceable { get; set; }
        public int FireCoatServiceableButForRepl { get; set; }
        public int TrouserServiceable { get; set; }
        public int TrouserServiceableButForRepl { get; set; }
        public int BootsServiceable { get; set; }
        public int BootsServiceableButForRepl { get; set; }
        public int GlovesServiceable { get; set; }
        public int GlovesServiceableButForRepl { get; set; }
        public int HelmetServiceable { get; set; }
        public int HelmetServiceableButForRepl { get; set; }
    }

    public class PPEReportModel
    {
        public string StationName { get; set; }
        public string SubstationName { get; set; }
        public int RegId { get; set; }
        public string MunicipalityName { get; set; }
        public string RegionName { get; set; }
        public int OperationPersonnel { get; set; }
        public string Province { get; set; }
        public int FireCoatServiceable { get; set; }
        public int FireCoatServiceableButForRepl { get; set; }
        public int TrouserServiceable { get; set; }
        public int TrouserServiceableButForRepl { get; set; }
        public int BootsServiceable { get; set; }
        public int BootsServiceableButForRepl { get; set; }
        public int GlovesServiceable { get; set; }
        public int GlovesServiceableButForRepl { get; set; }
        public int HelmetServiceable { get; set; }
        public int HelmetServiceableButForRepl { get; set; }
    }

    public class EquipmentNationWideModel
    {
        public int Serviceable { get; set; }
        public int Unserviceable { get; set; }
        public int UnderRepair { get; set; }
        public int RegId { get; set; }
        public string RegTitle { get; set; }
        public int FireHose15Serviceable { get; set; }
        public int FireHose15ServiceableButForRepl { get; set; }
        public int FireHose25Serviceable { get; set; }
        public int FireHose25ServiceableButForRepl { get; set; }
        public int FireNozzle15Serviceable { get; set; }
        public int FireNozzle15ServiceableButForRepl { get; set; }
        public int FireNozzle25Serviceable { get; set; }
        public int FireNozzle25ServiceableButForRepl { get; set; }
    }

    public class EquipmentReportModel
    {
        public string StationName { get; set; }
        public string SubstationName { get; set; }
        public int RegId { get; set; }
        public string MunicipalityName { get; set; }
        public string RegionName { get; set; }
        public int Serviceable { get; set; }
        public int Unserviceable { get; set; }
        public int UnderRepair { get; set; }
        public string Province { get; set; }
        public int FireHose15Serviceable { get; set; }
        public int FireHose15ServiceableButForRepl { get; set; }
        public int FireHose25Serviceable { get; set; }
        public int FireHose25ServiceableButForRepl { get; set; }
        public int FireNozzle15Serviceable { get; set; }
        public int FireNozzle15ServiceableButForRepl { get; set; }
        public int FireNozzle25Serviceable { get; set; }
        public int FireNozzle25ServiceableButForRepl { get; set; }
    }

    public class MunicipalityReportModel
    {
        public int MunicipalityId { get; set; }
        public int RegId { get; set; }
        public int ProvinceId { get; set; }
        public int UnitId { get; set; }
        public int SCBAServiceable { get; set; }
        public int SCBAServiceableButForRepl { get; set; }
        public int FireCoatServiceable { get; set; }
        public int FireCoatServiceableButForRepl { get; set; }
        public int TrouserServiceable { get; set; }
        public int TrouserServiceableButForRepl { get; set; }
        public int BootsServiceable { get; set; }
        public int BootsServiceableButForRepl { get; set; }
        public int GlovesServiceable { get; set; }
        public int GlovesServiceableButForRepl { get; set; }
        public int HelmetServiceable { get; set; }
        public int HelmetServiceableButForRepl { get; set; }
        public int FireHose15Serviceable { get; set; }
        public int FireHose15ServiceableButForRepl { get; set; }
        public int FireHose25Serviceable { get; set; }
        public int FireHose25ServiceableButForRepl { get; set; }
        public int FireNozzle15Serviceable { get; set; }
        public int FireNozzle15ServiceableButForRepl { get; set; }
        public int FireNozzle25Serviceable { get; set; }
        public int FireNozzle25ServiceableButForRepl { get; set; }
    }

    public class EmployeeCISReportModel
    {
        public int UnitId { get; set; }
        public int MunicipalityId { get; set; }
        public int ProvinceId { get; set; }
        public int RegId { get; set; }
        public int Emp_Curr_JobFunc { get; set; }
    }

    public class TruckCISReportModel
    {
        public int Truck_UnitId { get; set; }
        public int MunicipalityId { get; set; }
        public int ProvinceId { get; set; }
        public int RegId { get; set; }
        public int Truck_Owner { get; set; }
        public int Truck_Status { get; set; }
        public int Sub_Id { get; set; }
    }
}
