using System;
using System.Web.Mvc;
using EBFP.BL.HumanResources;
using EBFP.Helper;
using EBFP.Web.Areas.HelpPage;

namespace EBFP.Web.Areas.HRIS.Controllers
{
    public class ReportsController : Controller
    {
        IHRISUnitOfWork unitOfWork = new HRISUnitOfWork();
        public ActionResult Reports()
        {
            var employeeModel = new EmployeeModel();
            employeeModel.ProvinceID = CurrentUser.ProvinceID;
            employeeModel.MunicipalityID = CurrentUser.MunicipalityID;
            employeeModel.Emp_Curr_Unit = CurrentUser.EmployeeUnitId;
            return View(employeeModel);
        }

        [HttpPost]
        public string GenerateLeaveReport(int employee,int processedBy,int preparedBy ,int certifiedBy,DateTime? endDate)
        {
            if ((employee > 0 && processedBy > 0 && preparedBy > 0 && certifiedBy > 0))
            {
                var reportEndDate = endDate ?? DateTime.Now;
              return  unitOfWork.HRISReport.PrintLeaveRecord(employee, processedBy, preparedBy, certifiedBy, reportEndDate);
            }

            return "No excel file generated.";
        }


        [HttpPost]
        public string GenerateRegionalAuthorizeStrReport()
        {
             return unitOfWork.HRISReport.PrintRegionalAuthorizeStrReport();
        }

        [HttpPost]
        public string GeneratePersonnelStrengthReport(int preparedBy, int reviewedBy, int notedBy)
        {
            if ((preparedBy > 0 && reviewedBy > 0 && notedBy > 0))
            {
                return unitOfWork.HRISReport.PrintPersonnelStrengthReport(preparedBy, reviewedBy, notedBy);
            }
            return "No excel file generated.";
        }


        [HttpPost]
        public string GenerateActualVsAuthorizedReport(int preparedBy, int reviewedBy, int notedBy)
        {
            if ((preparedBy > 0 && reviewedBy > 0 && notedBy > 0))
            {
                return unitOfWork.HRISReport.PrintActualVsAuthorizedReport(preparedBy, reviewedBy, notedBy);
            }
            return "No excel file generated.";
        }

        [HttpPost]
        public string GenerateServiceReport(int employee, int preparedBy,int verifiedBy, int certifiedBy, string remarks = "")
        {
            if ((employee > 0 && preparedBy > 0 && certifiedBy > 0))
            {
                return unitOfWork.HRISReport.PrintServiceRecord(employee, preparedBy, verifiedBy, certifiedBy, remarks);
            }

            return "No excel file generated.";
        }

        public string GenerateLongevityPayReport(int official1 , int official2,DateTime startDate , DateTime endDate)
        {
            if ((official1 > 0 && official2 > 0))
            {
                return unitOfWork.HRISReport.PrintLongevityPay(official1, official2, startDate, endDate);
            }

            return "No excel file generated.";
        }

        [HttpPost]
        public string GenerateAgeProfileReport()
        {
            return unitOfWork.HRISReport.PrintAgeProfile();
        }

        [HttpPost]
        public string GenerateJobFunctionDistribution()
        {
            return unitOfWork.HRISReport.PrintJobFuntionDistribution();
        }

        [HttpPost]
        public string GenerateRatioOfFemaleFireFighters()
        {
            return unitOfWork.HRISReport.PrintRatioOfFemaleFighters();
        }
        [HttpPost]
        public string GenerateCommLeaveCreditsReport(int retiredEmployee,int processedBy, int verifiedBy, int certifiedBy, string remarks = "")
        {
            if ((retiredEmployee > 0 && processedBy  > 0 && verifiedBy > 0 && certifiedBy > 0))
            {
                return unitOfWork.HRISReport.PrintCommLeaveCredits(retiredEmployee, processedBy,verifiedBy, certifiedBy, remarks);
            }

            return "No excel file generated.";
        }

        [HttpPost]
        public string GenerateAlphaListReport()
        {
            return unitOfWork.HRISReport.PrintAlphaList();
        }

        [HttpPost]
        public string GenerateRetiringPersonnelReport(string type, int? month)
        {
           return unitOfWork.HRISReport.PrintRetiringPersonnelReport(type, month);
        }

        [HttpPost]
        public string GeneratePersonnelPerRegionReport(int region)
        {
            return unitOfWork.HRISReport.PrintPersonnelPerRegionReport(region);
        }

        [HttpPost]
        public string GeneratePersonnelPerProvinceReport(int province)
        {
            return unitOfWork.HRISReport.PrintPersonnelPerProvinceReport(province);
        }

        [HttpPost]
        public string GeneratePersonnelPerStationReport(int station)
        {
            return unitOfWork.HRISReport.PrintPersonnelPerStationReport(station);
        }

        [HttpGet]
        [DeleteFile]
        public ActionResult Download(string file)
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            var fullPath = $"{rootPath}{"\\Content\\MISC\\Generated\\"}{file}";
            return File(fullPath, "application/pdf", file);
        }
    }
}