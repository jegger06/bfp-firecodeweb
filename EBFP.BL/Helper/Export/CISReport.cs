using System.IO;
using System.Linq;
using EBFP.BL.CIS;
using EBFP.BL.HumanResources;
using GemBox.Presentation;
using Color = System.Drawing.Color;

namespace EBFP.BL.Helper
{
    using DataAccess;
    using GemBox.Presentation.Tables;
    using GemBox.Spreadsheet;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;
    public class CISReport : EntityFrameworkBase , ICISReport
    {
        public CISReport(EBFPEntities _context)
        {
            context_ = _context; 
        }

        public string PrintPhysicalInventory(int employee, int group, DateTime endDate, DateTime assumptionDate, int type)
        {
            ICISUnitOfWork oUnitOfWork = new CISUnitOfWork(context);
            IHRISUnitOfWork unitOfWork = new HRISUnitOfWork(context);

            var directorates = oUnitOfWork.Report.GetDirectoratesByGroup(group, endDate);
            var employeeDet = unitOfWork.Employee.GetEmployeeById(employee);
            var rank = unitOfWork.Rank.GetRankById(employeeDet.Emp_Curr_Rank ?? 0);

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            var newFile =
                $"{applicationPath}{@"\Content\MISC\Generated\PhysicalInventory_"}{DateTime.Now.Ticks}{".xlsx"}";
            var template = $"{applicationPath}{@"\Content\MISC\Template\PhysicalInventory.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["Sheet1"];
            var cell = worksheet.Cells;

            var unit = unitOfWork.Unit.GetUnitById(employeeDet.Emp_Curr_Unit ?? 0);
            cell["H8"].Value = "B F P- " + unit.Province_Name;

            cell["A4"].Value = directorates[0].IG_Name + "(" + directorates[0].IG_Code + ")";

            cell["A6"].Value = "as of " + endDate.ToString("dd MMMM yyyy");
            cell["A6"].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            if (type == (int) PhysicalInventoryReportType.PAR)
            {
                cell["A3"].Value = "REPORT ON THE PHYSICAL COUNT OF PROPERTY, PLANT AND EQUIPMENT";
                cell["M11"].Value = "ARE/PAR No.";
            }
            else
            {
                cell["A3"].Value = "REPORT ON THE PHYSICAL COUNT OF PROPERTY, PLANT AND EQUIPMENT (based on ICS)";
                cell["M11"].Value = "ICS No.";
            }
               
            cell["M8"].Value = assumptionDate.ToString("MMMM dd, yyyy");

            cell["B8"].Value = rank.Rank_Name + " " + employeeDet.Emp_FirstName + " " + employeeDet.Emp_MiddleName.First() + " " + employeeDet.Emp_LastName + " " + employeeDet.Emp_SuffixName + ",";

            //cell["D8"].Value = employeeDet.Emp_Curr_PosDesignationTitle + ",";
            cell["D8"].Value = unitOfWork.Employee.GetPositionTitle(employeeDet.Emp_Id) + ",";

            var row = 12;
            foreach (var item in directorates)
            {
                foreach (var ch in "ABCDEFGHIJKLM")
                {
                    cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                    cell[ch.ToString() + row].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    cell[ch.ToString() + row].Style.Font.Name = "Arial";
                    cell[ch.ToString() + row].Style.Font.Size = 10 * 20;
                }

                var dirRow = row;
                cell["A" + row++].Value = item.Dir_Description;
                cell["A" + dirRow].Style.Font.Weight = ExcelFont.BoldWeight;
                cell["A" + dirRow].Style.Font.Size = 12 * 20;

                worksheet.Cells.GetSubrangeAbsolute(dirRow - 1, 0, dirRow - 1, 2).Merged = true;

                var articles = oUnitOfWork.Report.GetArticlesByDirectorates(item.Dir_Id, endDate);
                foreach (var art in articles)
                {
                    foreach (var ch in "ABCDEFGHIJKLM")
                    {
                        cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                        cell[ch.ToString() + row].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        cell[ch.ToString() + row].Style.Font.Name = "Arial";
                        cell[ch.ToString() + row].Style.Font.Size = 10 * 20;
                    }

                    cell["A" + row].Value = art.PI_Art_Name;
                    cell["B" + row].Value = art.PI_Description;
                    cell["C" + row].Value = art.PI_PropertyNumber;
                    cell["C" + row].Style.WrapText = true;
                    cell["D" + row].Value = art.PI_UnitOfMeasure;
                    if (art.PI_DateAcquired != null)
                        cell["E" + row].Value = art.PI_DateAcquired.Value.ToString("dd-MMM-yy");
                    cell["F" + row].Value = art.PI_UnitValue;                  
                    cell["G" + row].Value = "1";
                    cell["H" + row].Value = "1";
                    cell["I" + row].Value = "";
                    cell["J" + row].Value = "";
                    cell["K" + row].Value = art.PI_Office;
                    cell["L" + row].Value = art.PI_End_User.TrimEnd('/'); 
                    cell["L" + row].Style.WrapText = true;
                    cell["M" + row++].Value = type == (int) PhysicalInventoryReportType.PAR ? art.PI_ARENumber : art.PI_ICSNUmber;                    
                }
            }

            cell["B" + row].Value = "XXXXX NOTHING FOLLOWS XXXXX";
            cell["B" + row].Style.Font.Weight = ExcelFont.BoldWeight;
            cell["B" + row].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            foreach (var ch in "ABCDEFGHIJKLM")
            {
                cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
            }

            cell["E" + (row + 1)].Value = "TOTAL";
            cell["E" + (row + 1)].Style.Font.Weight = ExcelFont.BoldWeight;
            cell["E" + (row + 1)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            foreach (var ch in "ABCDEFGHIJKLM")
            {
                cell[ch.ToString() + (row + 1)].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
            }
            cell["F" + (row + 1)].Formula = "=SUM(F12:F" + row + ")";
            cell["F" + (row + 1)].Style.Font.Weight = ExcelFont.BoldWeight;
            ef.Save(newFile);

            return Path.GetFileName(newFile);
        }

        public string PrintUnserviceable(int employee, int startmonth, DateTime assumptionDate,int endMonth,int year)
        {
            ICISUnitOfWork oUnitOfWork = new CISUnitOfWork(context);
            IHRISUnitOfWork unitOfWork = new HRISUnitOfWork(context);

            var unserviceableList = oUnitOfWork.Report.GetUnserviceable(startmonth, endMonth, year);
            var employeeDet = unitOfWork.Employee.GetEmployeeById(employee);
            var rank = unitOfWork.Rank.GetRankById(employeeDet.Emp_Curr_Rank ?? 0);

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            var newFile =
                $"{applicationPath}{@"\Content\MISC\Generated\IIRUP_"}{DateTime.Now.Ticks}{".xlsx"}";
            var template = $"{applicationPath}{@"\Content\MISC\Template\IIRUP.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["Sheet1"];
            var cell = worksheet.Cells;

            var unit = unitOfWork.Unit.GetUnitById(employeeDet.Emp_Curr_Unit ?? 0);
            cell["I8"].Value = "B F P- " + unit.Province_Name;

            var startMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(startmonth);
            var endMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(endMonth);
            cell["A4"].Value = "For the Period of " + startMonthName + " - " + endMonthName + " " + year;
            cell["M8"].Value = assumptionDate.ToString("MMMM dd, yyyy");

            cell["B8"].Value = rank.Rank_Name + " " + employeeDet.Emp_FirstName + " " + employeeDet.Emp_MiddleName.First() + " " + employeeDet.Emp_LastName + " " + employeeDet.Emp_SuffixName + ",";

            cell["D8"].Value = unitOfWork.Employee.GetPositionTitle(employeeDet.Emp_Id) + ",";

            var row = 12;
            foreach (var item in unserviceableList)
            {
                foreach (var ch in "ABCDEFGHIJKLMN")
                {
                    cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                    cell[ch.ToString() + row].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    cell[ch.ToString() + row].Style.Font.Name = "Arial";
                    cell[ch.ToString() + row].Style.Font.Size = 10 * 20;
                }
                
                cell["A" + row].Value = item.UPI_WMR;
                cell["B" + row].Value = item.UPI_ReportingOffice;

                var unserviceableItemList = oUnitOfWork.Report.GetUnserviceableItem(item.UPI_Id);
                foreach (var unserviceable in unserviceableItemList)
                {
                    foreach (var ch in "ABCDEFGHIJKLMN")
                    {
                        cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                        cell[ch.ToString() + row].Style.Font.Name = "Arial";
                        cell[ch.ToString() + row].Style.Font.Size = 10 * 20;
                       
                    }
                    cell["C" + row].Value = unserviceable.PI_Description;
                    cell["D" + row].Value = 1;
                    cell["E" + row].Style.NumberFormat = "#,##0.00";
                    cell["E" + row].Value = unserviceable.PI_UnitValue;

                    cell["F" + row].Style.NumberFormat = "#,##0.00";
                    cell["F" + row].Formula = "=E" + row +"*D" + row;
                    if (unserviceable.PI_DateAcquired != null)
                        cell["G" + row].Value = unserviceable.PI_DateAcquired.Value.ToString("dd-MMM-yy");

                    cell["J" + row ++].Value = unserviceable.PI_PropertyNumber;
                }
            }
            ef.Save(newFile);

            return Path.GetFileName(newFile);
        }

        public string PrintPhysicalInventorySummary(DateTime endDate, int type)
        {
            ICISUnitOfWork oUnitOfWork = new CISUnitOfWork(context);
            
            var summaryPARReportList = oUnitOfWork.Report.GetPhyicalInventorySummaryReport(endDate, type);
        
            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

            var newFile = "";
            newFile = type == (int) PhysicalInventoryReportType.PAR ? $"{applicationPath}{@"\Content\MISC\Generated\SummaryPAR_"}{DateTime.Now.Ticks}{".xlsx"}" 
                : $"{applicationPath}{@"\Content\MISC\Generated\SummaryICS_"}{DateTime.Now.Ticks}{".xlsx"}";

            var template = $"{applicationPath}{@"\Content\MISC\Template\PhysicalInvetorySummaryReport.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["Sheet1"];
            var cell = worksheet.Cells;

            cell["A3"].Value = "As of " + endDate.ToString("MMMM dd, yyyy");
          
            cell["A2"].Value = type == (int)PhysicalInventoryReportType.PAR ? "Based on PAR" : "Based on ICS";

            var row = 6;
            foreach (var item in summaryPARReportList)
            {
                foreach (var ch in "ABC")
                {
                    cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                    cell[ch.ToString() + row].Style.Font.Name = "Arial";
                    cell[ch.ToString() + row].Style.Font.Size = 10 * 20;
                }

                cell["A" + row].Value = item.IG_Name;
                cell["B" + row].Value = item.IG_Code;
                cell["C" + row].Style.NumberFormat = "#,##0.00";
                cell["C" + row ++].Value = item.TotalCost;
            }
            cell["A" + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
            cell["A" + row].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            cell["A" + row].Value = "TOTAL";
            cell["A" + row].Style.Font.Weight = ExcelFont.BoldWeight;
            cell["B" + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
            cell["C" + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
            cell["C" + row].Style.NumberFormat = "#,##0.00";
            cell["C" + row].Formula = "=SUM(C6:C" + (row -1 ) + ")";
            cell["C" + row].Style.Font.Weight = ExcelFont.BoldWeight;
            ef.Save(newFile);

            return Path.GetFileName(newFile);
        }

        public string PrintInventorySupplies(int employee, DateTime assumptionDate,DateTime endDate)
        {
            ICISUnitOfWork oUnitOfWork = new CISUnitOfWork(context);
            IHRISUnitOfWork unitOfWork = new HRISUnitOfWork(context);

            var inventorySupplyList = oUnitOfWork.Report.GetPhyicalInventorySuppliesReport(endDate);
            var employeeDet = unitOfWork.Employee.GetEmployeeById(employee);
            var rank = unitOfWork.Rank.GetRankById(employeeDet.Emp_Curr_Rank ?? 0);

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            var newFile =
                $"{applicationPath}{@"\Content\MISC\Generated\RPCI_CY_"}{DateTime.Now.Ticks}{".xlsx"}";
            var template = $"{applicationPath}{@"\Content\MISC\Template\InventorySupplies.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["Sheet1"];
            var cell = worksheet.Cells;

            var unit = unitOfWork.Unit.GetUnitById(employeeDet.Emp_Curr_Unit ?? 0);
            cell["F6"].Value = "B F P- " + unit.Province_Name;

            cell["A2"].Value = "As of " + endDate.ToString("MMMM dd, yyyy");

            cell["K6"].Value = assumptionDate.ToString("MMMM dd, yyyy");

            cell["A6"].Value = "For which  " + rank.Rank_Name + " " + employeeDet.Emp_FirstName + " " + employeeDet.Emp_MiddleName.First() + " " + employeeDet.Emp_LastName + " " + employeeDet.Emp_SuffixName + ",";

            cell["C6"].Value = unitOfWork.Employee.GetPositionTitle(employeeDet.Emp_Id) + ",";

            var row = 10;
            foreach (var item in inventorySupplyList)
            {
                foreach (var ch in "ABCDEFGHIJK")
                {
                    cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                    cell[ch.ToString() + row].Style.Font.Name = "Arial";
                    cell[ch.ToString() + row].Style.Font.Size = 8 * 20;
                    cell[ch.ToString() + row].Style.WrapText = true;
                }

                cell["A" + row].Value = item.SI_Art_Name;
                cell["B" + row].Value = item.SI_Description;
                cell["C" + row].Value = item.SI_StockNumber;
                cell["D" + row].Value = item.SI_UnitOfMeasure;
                cell["E" + row].Style.NumberFormat = "#,##0.00";
                cell["E" + row].Value = item.SI_UnitValue;
                cell["F" + row].Value = item.SI_Quantity;
                cell["G" + row].Value = item.SI_OnHand;
                cell["H" + row].Style.NumberFormat = "#,##0.00";
                cell["H" + row].Value = item.SI_TotalAmount;
                cell["I" + row].Value ="";
                cell["J" + row].Value = "";
                cell["K" + row++].Value ="";

            }
            foreach (var ch in "ABCDEFGHIJK")
            {
                cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                cell[ch.ToString() + row].Style.Font.Name = "Arial";
                cell[ch.ToString() + row].Style.Font.Size = 8 * 20;
                cell[ch.ToString() + row].Style.WrapText = true;
            }
            
            cell["A" + row].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            cell["A" + row].Value = "TOTAL";
            cell["A" + row].Style.Font.Weight = ExcelFont.BoldWeight;
            cell["H" + row].Style.NumberFormat = "#,##0.00";
            cell["H" + row].Formula = "=SUM(G10:H" + (row - 1) + ")";
            cell["H" + row].Style.Font.Weight = ExcelFont.BoldWeight;
            ef.Save(newFile);

            return Path.GetFileName(newFile);
        }

        public string PrintSCBANationwide(int preparedBy, int notedBy)
        {
            IHRISUnitOfWork unitOfWork = new HRISUnitOfWork();
            ICISUnitOfWork oUnitOfWork = new CISUnitOfWork(context);

            var scbaNationwideList = oUnitOfWork.Report.GetSCBANationwide();

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

            var newFile = $"{applicationPath}{@"\Content\MISC\Generated\SCBANationwide_"}{DateTime.Now.Ticks}{".xlsx"}";

            var template = $"{applicationPath}{@"\Content\MISC\Template\SCBANationwide.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["SCBANationwide"];
            var cell = worksheet.Cells;

            cell["A3"].Value = "As of " + DateTime.Now.ToString("dd MMMM yyyy");
           
            var row = 6;
            foreach (var item in scbaNationwideList)
            {
                foreach (var ch in "ABC")
                {
                    cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                    cell[ch.ToString() + row].Style.Font.Name = "Arial";
                    cell[ch.ToString() + row].Style.Font.Size = 10 * 20;
                }

                cell["A" + row].Value = item.RegTitle.Replace("Region","");
                cell["B" + row].Value = item.Serviceable;
                cell["C" + row].Value = item.Unserviceable;
                cell["D" + row].Value = item.UnderRepair;
                cell["G" + row].Value = item.SCBAServiceable;
                cell["H" + row++].Value = item.SCBAServiceableButForRepl;
            }
            cell["B27"].Value = "Status of BFP owned Fire Engines was based from Status of BFP Fire Trucks as of " + DateTime.Now.ToString("dd MMMM yyyy");
            cell["B28"].Value = "Serviceable Number of Breathing Apparatus was based on the " + DateTime.Now.ToString("MMMM yyyy") + " report submitted by the Regional Offices.";
            //Prepared By
            var preparedby = unitOfWork.Employee.GetEmployeeById(preparedBy);
            var preparedByRank = unitOfWork.Rank.GetRankById(preparedby.Emp_Curr_Rank ?? 0);

            cell["B35"].Value = preparedby.Emp_FirstName + " " + preparedby.Emp_MiddleName + " " +
                                           preparedby.Emp_LastName + " " + preparedby.Emp_SuffixName;
            cell["B36"].Value = preparedByRank.Rank_Name;
            cell["B37"].Value = unitOfWork.Employee.GetPositionTitle(preparedBy);

            //Noted By
            var notedby = unitOfWork.Employee.GetEmployeeById(notedBy);
            var notedbyByRank = unitOfWork.Rank.GetRankById(notedby.Emp_Curr_Rank ?? 0);
            cell["I35"].Value = notedby.Emp_FirstName + " " + notedby.Emp_MiddleName + " " +
                                           notedby.Emp_LastName + " " + notedby.Emp_SuffixName;
            cell["I36"].Value = notedbyByRank.Rank_Name;         
            cell["I37"].Value = unitOfWork.Employee.GetPositionTitle(notedBy);
            ef.Save(newFile);

            return Path.GetFileName(newFile);
        }

        public string PrintSCBAReport(int preparedBy, int certifiedBy, int notedBy,int id,string type)
        {
            IHRISUnitOfWork unitOfWork = new HRISUnitOfWork();
            ICISUnitOfWork oUnitOfWork = new CISUnitOfWork(context);

            var scbaResultList = new List<SCBAReportModel>();

            if (type == "Region")
             scbaResultList = oUnitOfWork.Report.GetSCBAByRegion(id);
            else if(type == "Province")
                scbaResultList = oUnitOfWork.Report.GetSCBAByProvince(id);
            else
                scbaResultList = oUnitOfWork.Report.GetSCBAByStation(id);

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

            var newFile = type == "Region" ? $"{applicationPath}{@"\Content\MISC\Generated\SCBA_"}{scbaResultList[0].RegionName}{"_"}{DateTime.Now.Ticks}{".xlsx"}" 
                : type == "Province" ? $"{applicationPath}{@"\Content\MISC\Generated\SCBA_"}{scbaResultList[0].Province}{"_"}{DateTime.Now.Ticks}{".xlsx"}"
                : $"{applicationPath}{@"\Content\MISC\Generated\SCBA_"}{scbaResultList[0].MunicipalityName}{"_"}{DateTime.Now.Ticks}{".xlsx"}";

            var template = $"{applicationPath}{@"\Content\MISC\Template\SCBAReport.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["Sheet1"];
            var cell = worksheet.Cells;

            cell["A3"].Value = "As of " + DateTime.Now.ToString("dd MMMM yyyy");
            cell["A5"].Value = type == "Region" ? scbaResultList[0].RegionName.ToUpper() + "    PROVINCE / DISTRICT" : type == "Province" ? scbaResultList[0].Province.ToUpper() + " CITY / MUNICIPALITY"
                : scbaResultList[0].MunicipalityName.ToUpper() + " FIRE STATIONS / SUBSTATIONS";


            var row = 7;
            foreach (var item in scbaResultList)
            {
                foreach (var ch in "ABCDEFGHIJ")
                {
                    cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                    cell[ch.ToString() + row].Style.Font.Name = "Arial";
                    cell[ch.ToString() + row].Style.Font.Size = 10 * 20;
                }

                cell["A" + row].Value = type == "Region" ? item.Province.Replace("District", "").ToUpper() : type == "Province" ? item.MunicipalityName.ToUpper() : item.StationName.ToUpper();
                cell["B" + row].Value = item.Serviceable;
                cell["C" + row].Value = item.Unserviceable;
                cell["D" + row].Value = item.UnderRepair;
                cell["G" + row].Value = item.SCBAServiceable;
                cell["H" + row].Value = item.SCBAServiceableButForRepl;
                cell["E" + row].Formula = "=SUM(B" + row + ":C" + row + ")";
                cell["F" + row].Formula = "=E" + row + "*4";
                cell["I" + row++].Formula = "=F" + (row -1) + "-G" + (row - 1) + "+H" + (row - 1);
            }

            foreach (var ch in "ABCDEFGHIJ")
            {
                cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                cell[ch.ToString() + row].Style.Font.Name = "Arial";
                cell[ch.ToString() + row].Style.Font.Size = 10 * 20;
                cell[ch.ToString() + row].Style.Font.Weight = ExcelFont.BoldWeight;
            }
            cell["A" + row].Value = "TOTAL";
            cell["B" + row].Formula = "=SUM(B7:B" + (row -1) + ")";
            cell["C" + row].Formula = "=SUM(C7:C" + (row - 1) + ")";
            cell["D" + row].Formula = "=SUM(D7:D" + (row - 1) + ")";
            cell["E" + row].Formula = "=SUM(E7:E" + (row - 1) + ")";
            cell["F" + row].Formula = "=SUM(F7:F" + (row - 1) + ")";
            cell["G" + row].Formula = "=SUM(G7:G" + (row - 1) + ")";
            cell["H" + row].Formula = "=SUM(H7:H" + (row - 1) + ")";
            cell["I" + row].Formula = "=SUM(I7:I" + (row - 1) + ")";

            row = row + 1;
            cell["A" + row].Value = "Note:";
            worksheet.Cells.GetSubrangeAbsolute(row - 1, 1, row - 1, 3).Merged = true;
            cell["B" + row++].Value = "For the evaluation of SCBA";
            worksheet.Cells.GetSubrangeAbsolute(row - 1, 1, row - 1, 3).Merged = true;
            cell["B" + row++].Value = "Serviceable: Good Quality";
            worksheet.Cells.GetSubrangeAbsolute(row - 1, 1, row - 1, 3).Merged = true;
            cell["B" + row++].Value = "Serviceable but for replacement: Poor Quality";

            worksheet.Cells.GetSubrangeAbsolute((row+1) - 1, 1, (row + 2) - 1, 3).Merged = true;
            cell["B" + (row+1)].Value = "Standard: 1 Unit Firetruck : 4 Units SCBA";

            row = row + 5;
            //Prepared By    
            cell["A" + row].Value = "Prepared by:";
            var preparedby = unitOfWork.Employee.GetEmployeeById(preparedBy);
            var preparedByRank = unitOfWork.Rank.GetRankById(preparedby.Emp_Curr_Rank ?? 0);

            cell["A"+ (row+2)].Value = preparedby.Emp_FirstName + " " + preparedby.Emp_MiddleName + " " +
                                           preparedby.Emp_LastName + " " + preparedby.Emp_SuffixName;
            cell["A" + (row + 3)].Value = preparedByRank.Rank_Name;
            cell["A" + (row + 4)].Value = unitOfWork.Employee.GetPositionTitle(preparedBy); 

            //Certified By
            cell["E" + row].Value = "Certified by:";
            var certifiedby = unitOfWork.Employee.GetEmployeeById(certifiedBy);
            var certifiedByRank = unitOfWork.Rank.GetRankById(certifiedby.Emp_Curr_Rank ?? 0);

            cell["E" + (row + 2)].Value = certifiedby.Emp_FirstName + " " + certifiedby.Emp_MiddleName + " " +
                                           certifiedby.Emp_LastName + " " + certifiedby.Emp_SuffixName;
            cell["E" + (row + 3)].Value = certifiedByRank.Rank_Name;
            cell["E" + (row + 4)].Value = unitOfWork.Employee.GetPositionTitle(certifiedBy);

            //Noted By
            cell["I" + row].Value = "Noted by:";
            var notedby = unitOfWork.Employee.GetEmployeeById(notedBy);
            var notedbyByRank = unitOfWork.Rank.GetRankById(notedby.Emp_Curr_Rank ?? 0);
            cell["I" + (row + 2)].Value = notedby.Emp_FirstName + " " + notedby.Emp_MiddleName + " " +
                                           notedby.Emp_LastName + " " + notedby.Emp_SuffixName;
            cell["I" + (row + 3)].Value = notedbyByRank.Rank_Name;
            cell["I" + (row + 4)].Value = unitOfWork.Employee.GetPositionTitle(notedBy);
            ef.Save(newFile);

            return Path.GetFileName(newFile);
        }

        public string PrintPPENationwide(int preparedBy, int notedBy)
        {
            IHRISUnitOfWork unitOfWork = new HRISUnitOfWork();
            ICISUnitOfWork oUnitOfWork = new CISUnitOfWork(context);

            var ppeNationwideList = oUnitOfWork.Report.GetPPENationwide();

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

            var newFile = $"{applicationPath}{@"\Content\MISC\Generated\PPENationwide_"}{DateTime.Now.Ticks}{".xlsx"}";

            var template = $"{applicationPath}{@"\Content\MISC\Template\PPENationwide.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["PPENationwide"];
            var cell = worksheet.Cells;

            cell["A3"].Value = "As of " + DateTime.Now.ToString("dd MMMM yyyy");

            var row = 6;
            foreach (var item in ppeNationwideList)
            {
                foreach (var ch in "ABC")
                {
                    cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                    cell[ch.ToString() + row].Style.Font.Name = "Arial";
                    cell[ch.ToString() + row].Style.Font.Size = 10 * 20;
                }

                cell["A" + row].Value = item.RegTitle.Replace("Region", "");
                cell["B" + row].Value = item.OperationPersonnel;
                cell["C" + row].Value = item.FireCoatServiceable;
                cell["D" + row].Value = item.FireCoatServiceableButForRepl;
                cell["F" + row].Value = item.TrouserServiceable;
                cell["G" + row].Value = item.TrouserServiceableButForRepl;
                cell["I" + row].Value = item.BootsServiceable;
                cell["J" + row].Value = item.BootsServiceableButForRepl;
                cell["L" + row].Value = item.GlovesServiceable;
                cell["M" + row].Value = item.GlovesServiceableButForRepl;
                cell["O" + row].Value = item.HelmetServiceable;
                cell["P" + row++].Value = item.HelmetServiceableButForRepl;
            }
            cell["C27"].Value = "Serviceable Number of PPE was based on " + DateTime.Now.ToString("dd MMMM yyyy") + " status report submitted by Regional Offices.";
            //Prepared By
            var preparedby = unitOfWork.Employee.GetEmployeeById(preparedBy);
            var preparedByRank = unitOfWork.Rank.GetRankById(preparedby.Emp_Curr_Rank ?? 0);

            cell["B35"].Value = preparedby.Emp_FirstName + " " + preparedby.Emp_MiddleName + " " +
                                           preparedby.Emp_LastName + " " + preparedby.Emp_SuffixName;
            cell["B36"].Value = preparedByRank.Rank_Name;
            cell["B37"].Value = unitOfWork.Employee.GetPositionTitle(preparedBy);

            //Noted By
            var notedby = unitOfWork.Employee.GetEmployeeById(notedBy);
            var notedbyByRank = unitOfWork.Rank.GetRankById(notedby.Emp_Curr_Rank ?? 0);
            cell["O35"].Value = notedby.Emp_FirstName + " " + notedby.Emp_MiddleName + " " +
                                           notedby.Emp_LastName + " " + notedby.Emp_SuffixName;
            cell["O36"].Value = notedbyByRank.Rank_Name;
            cell["O37"].Value = unitOfWork.Employee.GetPositionTitle(notedBy);
            ef.Save(newFile);

            return Path.GetFileName(newFile);
        }

        public string PrintEquipmentNationwide(int preparedBy, int notedBy)
        {
            IHRISUnitOfWork unitOfWork = new HRISUnitOfWork();
            ICISUnitOfWork oUnitOfWork = new CISUnitOfWork(context);

            var equipmentNationwideList = oUnitOfWork.Report.GetEquipmentNationwide();

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

            var newFile = $"{applicationPath}{@"\Content\MISC\Generated\EquipmentNationwide_"}{DateTime.Now.Ticks}{".xlsx"}";

            var template = $"{applicationPath}{@"\Content\MISC\Template\EquipmentNationwide.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["EquipmentNationwide"];
            var cell = worksheet.Cells;

            cell["A3"].Value = "As of " + DateTime.Now.ToString("dd MMMM yyyy");

            var row = 7;
            foreach (var item in equipmentNationwideList)
            {
                foreach (var ch in "ABC")
                {
                    cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                    cell[ch.ToString() + row].Style.Font.Name = "Arial";
                    cell[ch.ToString() + row].Style.Font.Size = 10 * 20;
                }

                cell["A" + row].Value = item.RegTitle.Replace("Region", "");
                cell["B" + row].Value = item.Serviceable;
                cell["C" + row].Value = item.Unserviceable;
                cell["D" + row].Value = item.UnderRepair;           
                cell["G" + row].Value = item.FireHose15Serviceable;
                cell["H" + row].Value = item.FireHose15ServiceableButForRepl;
                cell["K" + row].Value = item.FireHose25Serviceable;
                cell["L" + row].Value = item.FireHose25ServiceableButForRepl;            
                cell["O" + row].Value = item.FireNozzle15Serviceable;
                cell["P" + row].Value = item.FireNozzle15ServiceableButForRepl;
                cell["S" + row].Value = item.FireNozzle25Serviceable;
                cell["T" + row++].Value = item.FireNozzle25ServiceableButForRepl;
            }
            cell["B28"].Value = "Status of BFP owned Fire Engines was based from Status of BFP Fire Trucks as of " + DateTime.Now.ToString("dd MMMM yyyy") + ".";
            cell["B29"].Value = "Serviceable Number of Breating Firefighting Equipment was based on the " +
                                DateTime.Now.ToString("MMMM yyyy") + " report submitted by the Regional Offices.";
            //Prepared By
            var preparedby = unitOfWork.Employee.GetEmployeeById(preparedBy);
            var preparedByRank = unitOfWork.Rank.GetRankById(preparedby.Emp_Curr_Rank ?? 0);

            cell["B35"].Value = preparedby.Emp_FirstName + " " + preparedby.Emp_MiddleName + " " +
                                           preparedby.Emp_LastName + " " + preparedby.Emp_SuffixName;
            cell["B36"].Value = preparedByRank.Rank_Name;
            cell["B37"].Value = unitOfWork.Employee.GetPositionTitle(preparedBy);

            //Noted By
            var notedby = unitOfWork.Employee.GetEmployeeById(notedBy);
            var notedbyByRank = unitOfWork.Rank.GetRankById(notedby.Emp_Curr_Rank ?? 0);
            cell["S35"].Value = notedby.Emp_FirstName + " " + notedby.Emp_MiddleName + " " +
                                           notedby.Emp_LastName + " " + notedby.Emp_SuffixName;
            cell["S36"].Value = notedbyByRank.Rank_Name;
            cell["S37"].Value = unitOfWork.Employee.GetPositionTitle(notedBy);
            ef.Save(newFile);

            return Path.GetFileName(newFile);
        }

        public string GeneratePowerpoint()
        {
            // If using Professional version, put your serial key below.
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

            var template = $"{applicationPath}{@"\Content\MISC\Template\BFP.pptx"}";

            var presentation = PresentationDocument.Load(template);

            //// Retrieve first slide.
            var slide = presentation.Slides[0];

            //// Retrieve "Title" placeholder and set shape text.
            //Shape shape = slide.Content.Drawings.OfType<Shape>().Where(item => item.DrawingType == DrawingType.Shape).First();
            //var vvv = slide.Content.Drawings.OfType<Shape>().Where(item => item.Name != null).Select(item => item.Name).ToList();
            //var vvvs = slide.Content.Drawings.OfType<Shape>().ToList();

            var shape = slide.Content.Drawings.OfType<Shape>().First(item => item.Name == "TextBox 1" && item.DrawingType == DrawingType.Shape);
            shape.Text.Paragraphs[0].Elements.Clear();
            var run = shape.Text.AddParagraph().AddRun("10000");
            run.Format.Bold = true;


            //shape.Text.Paragraphs[0].Elements.Clear();
            //shape.Text.Paragraphs[0].AddRun("10000");

            // Retrieve third slide.
            slide = presentation.Slides[2];

            // Retrieve "Title" placeholder and set shape text.
            //shape = slide.Content.Drawings.OfType<Shape>().Where(item => item.Placeholder?.PlaceholderType == PlaceholderType.Title).First();
            //shape.Text.Paragraphs[0].AddRun("4th Quarter Financial Highlights");

            // Retrieve a table.
            Table secondTable = slide.Content.Drawings.OfType<GraphicFrame>().Where(item => item.Table.Frame.Name == "Table 1").Select(item => item.Table).First();
            Table firstTable = slide.Content.Drawings.OfType<GraphicFrame>().Where(item => item.Table.Frame.Name == "Table 3").Select(item => item.Table).First();

            // Fill table data.
            firstTable.Rows[1].Cells[1].Text.Paragraphs[0].Elements.OfType<TextRun>().First().Text = "$14.2M";
            firstTable.Rows[1].Cells[2].Text.Paragraphs[0].Elements.OfType<TextRun>().First().Text = "(0.5%)";

            firstTable.Rows[2].Cells[1].Text.Paragraphs[0].Elements.OfType<TextRun>().First().Text = "$1.6M";
            firstTable.Rows[2].Cells[2].Text.Paragraphs[0].Elements.OfType<TextRun>().First().Text = "0.7%";

            secondTable.Rows[3].Cells[1].Text.Paragraphs[0].Elements.OfType<TextRun>().First().Text = "$12.5M";
            secondTable.Rows[3].Cells[2].Text.Paragraphs[0].Elements.OfType<TextRun>().First().Text = "0.3%";

            secondTable.Rows[4].Cells[1].Text.Paragraphs[0].Elements.OfType<TextRun>().First().Text = "$2.3M";
            secondTable.Rows[4].Cells[2].Text.Paragraphs[0].Elements.OfType<TextRun>().First().Text = "(0.2%)";

            var xx = $"{applicationPath}{@"\Content\MISC\Generated\BFP_AGENCY_PROFILE_"}{DateTime.Now.Ticks}{".pptx"}";
            presentation.Save(xx);

            return "";
        }

        public void MergePresentaion()
        {
            // If using Professional version, put your serial key below.
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            var template = $"{applicationPath}{@"\Content\MISC\Template\BFP.pptx"}";

            PresentationDocument presentation = PresentationDocument.Load(template);

            //string pathToFileDirectory = "Resources";
            var template2 = $"{applicationPath}{@"\Content\MISC\Template\Test.pptx"}";

            var sourcePresentation = PresentationDocument.Load(template2);

            // Use context so that references between 
            // shapes and slides are maintained between all cloning operations.
            var context = CloneContext.Create(sourcePresentation, presentation);

            // Clone all drawings from the first slide of another presentation 
            // into the first slide of the current presentation.
            //foreach (var drawing in sourcePresentation.Slides[0].Content.Drawings)
            //    presentation.Slides[0].Content.Drawings.AddClone(drawing, context);

            // Establish explicit mapping between slides so that 
            // hyperlink on the second slide is correctly cloned.
            //context.Set(sourcePresentation.Slides[0], presentation.Slides[0]);

            // Clone the second slide from another presentation.
            presentation.Slides.AddClone(sourcePresentation.Slides[0], context);
            //presentation.Slides.AddClone(sourcePresentation.Slides[1], context);
            //presentation.Slides.AddClone(sourcePresentation.Slides[2], context);

            var xx = $"{applicationPath}{@"\Content\MISC\Generated\Merge_"}{DateTime.Now.Ticks}{".pptx"}";
            presentation.Save(xx);

            //presentation.Save("Cloning.pptx");
        }
    }

    public interface ICISReport
    {
        string PrintPhysicalInventory(int employee, int group, DateTime endDate, DateTime assumptionDate, int type);
        string PrintUnserviceable(int employee, int startmonth, DateTime assumptionDate, int endMonth,int year);
        string PrintPhysicalInventorySummary(DateTime endDate, int type);
        string PrintInventorySupplies(int employee, DateTime assumptionDate, DateTime endDate);
        string PrintSCBANationwide(int preparedBy, int notedBy);
        string PrintSCBAReport(int preparedBy,int certifiedBy, int notedBy,int id, string type);
        string GeneratePowerpoint();
        void MergePresentaion();
        string PrintPPENationwide(int preparedBy, int notedBy);
        string PrintEquipmentNationwide(int preparedBy, int notedBy);
    }
}
