using System;
using System.Web.Mvc;
using EBFP.BL.CIS;
using EBFP.BL.Helper;
using EBFP.Helper;
using EBFP.Web.Areas.HelpPage;

namespace EBFP.Web.Areas.CIS.Controllers
{
    public class InventoryReportsController : Controller
    {
        private readonly ICISUnitOfWork unitOfWork = new CISUnitOfWork();

        public ActionResult InventoryReports()
        {
            var inventoryReportModel = new InventoryReportModel();
            inventoryReportModel.ProvinceId = CurrentUser.ProvinceID;
            inventoryReportModel.Municipality_Id = CurrentUser.MunicipalityID;
            return View(inventoryReportModel);
        }

        [HttpPost]
        public string GeneratePhysicalInventoryReport(int employee, int group, DateTime endDate, DateTime assumptionDate,
            int type)
        {
            if (employee > 0 && group > 0 && type > 0)
            {
                return unitOfWork.CISReport.PrintPhysicalInventory(employee, group, endDate, assumptionDate, type);
            }
            return "No excel file generated.";
        }

        [HttpPost]
        public string GenerateUnserviceableReport(int employee, int startmonth, DateTime assumptionDate,
         int endMonth, int year)
        {
            if (employee > 0  && startmonth > 0 && endMonth > 0)
            {
                return unitOfWork.CISReport.PrintUnserviceable(employee, startmonth, assumptionDate, endMonth, year);
            }
            return "No excel file generated.";
        }

        [HttpPost]
        public string GenerateSummaryPARReport(DateTime endDate)
        {
            unitOfWork.CISReport.MergePresentaion();
            return "";//unitOfWork.CISReport.PrintPhysicalInventorySummary(endDate, (int) PhysicalInventoryReportType.PAR);
        }

        [HttpPost]
        public string GenerateSummaryICSReport(DateTime endDate)
        {
            return unitOfWork.CISReport.PrintPhysicalInventorySummary(endDate, (int)PhysicalInventoryReportType.ICS);
        }

        [HttpPost]
        public string GenerateInventorySuppliesReport(int employee, DateTime assumptionDate, DateTime endDate)
        {
            return employee > 0 ? unitOfWork.CISReport.PrintInventorySupplies(employee, assumptionDate, endDate) : "No excel file generated.";
        }

        [HttpPost]
        public string GenerateSCBANationwideReport(int preparedBy, int notedBy)
        {
            return unitOfWork.CISReport.PrintSCBANationwide(preparedBy,notedBy);
        }

        [HttpPost]
        public string GenerateSCBAByRegionReport(int preparedBy,int certifiedBy, int notedBy,int region)
        {
            return unitOfWork.CISReport.PrintSCBAReport(preparedBy, certifiedBy, notedBy, region,"Region");
        }

        [HttpPost]
        public string GenerateSCBAByProvinceReport(int preparedBy, int certifiedBy, int notedBy, int provinceId)
        {
            return unitOfWork.CISReport.PrintSCBAReport(preparedBy, certifiedBy, notedBy, provinceId,"Province");
        }

        [HttpPost]
        public string GenerateSCBAByMunicipalityReport(int preparedBy, int certifiedBy, int notedBy, int municipalityId)
        {
            return unitOfWork.CISReport.PrintSCBAReport(preparedBy, certifiedBy, notedBy, municipalityId, "Municipality");
        }

        [HttpPost]
        public string GeneratePPENationwideReport(int preparedBy, int notedBy)
        {
            return unitOfWork.CISReport.PrintPPENationwide(preparedBy, notedBy);
        }

        [HttpPost]
        public string GenerateEquipmentNationwideReport(int preparedBy, int notedBy)
        {
            return unitOfWork.CISReport.PrintEquipmentNationwide(preparedBy, notedBy);
        }

        [HttpGet]
        [DeleteFile]
        public ActionResult Download(string file)
        {
            var rootPath = AppDomain.CurrentDomain.BaseDirectory;
            var fullPath = $"{rootPath}{"\\Content\\MISC\\Generated\\"}{file}";
            return File(fullPath, "application/pdf", file);
        }
    }
}