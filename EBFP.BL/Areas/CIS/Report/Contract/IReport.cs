
using System;
using System.Collections.Generic;
using EBFP.BL.Helper;

namespace EBFP.BL.CIS
{
    public interface IReport
    {
        List<DirectoratesReportModel> GetDirectoratesByGroup(int groupId, DateTime endDate);
        List<PhysicalInventoryReportModel> GetArticlesByDirectorates(int dirId, DateTime endDate);
        List<UnserviceableItemList> GetUnserviceableItem(int unserviceableId);
        List<UnserviceableReportModel> GetUnserviceable(int startmonth, int endMonth, int year);
        List<SummaryReportModel> GetPhyicalInventorySummaryReport(DateTime endDate, int type);
        List<PhysicalInventorySuppliesModel> GetPhyicalInventorySuppliesReport(DateTime endDate);
        List<SCBANationWideModel> GetSCBANationwide();
        List<SCBAReportModel> GetSCBAByRegion(int region);
        List<SCBAReportModel> GetSCBAByProvince(int provinceId);
        List<SCBAReportModel> GetSCBAByStation(int municipality);
        List<PPENationWideModel> GetPPENationwide();
        List<EquipmentNationWideModel> GetEquipmentNationwide();
        List<PPEReportModel> GetPPEByRegion(int region);
        List<PPEReportModel> GetPPEByProvince(int provinceId);
    }
}