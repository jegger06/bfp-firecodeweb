using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using EBFP.BL.HumanResources;
using EBFP.DataAccess;
using GemBox.Spreadsheet;
using System.Globalization;

namespace EBFP.BL.Helper
{
    public class HRISReport : EntityFrameworkBase, IHRISReport
    {
        public HRISReport(EBFPEntities _context)
        {
            context_ = _context;
        }

        public string PrintLeaveRecord(int employeeId, int processedBy, int preparedBy, int certifiedBy,
            DateTime endDate)
        {
            IHRISUnitOfWork oUnitOfWork = new HRISUnitOfWork(context);
            var employee = oUnitOfWork.Employee.GetEmployeeById(employeeId);
            if (employee.Emp_Service_StartDate != null)
            {
                var leave = oUnitOfWork.Leave.GetLeaveReport(employee.Emp_Service_StartDate.Value, employeeId, endDate);

                var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
                var newFile =
                    $"{applicationPath}{@"\Content\MISC\Generated\LeaveReport_"}{DateTime.Now.Ticks}{".xlsx"}";
                var template = $"{applicationPath}{@"\Content\MISC\Template\LeaveReport.xlsx"}";

                SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
                var ef = ExcelFile.Load(template);
                var worksheet = ef.Worksheets["Sheet1"];
                var cell = worksheet.Cells;

                cell["B5"].Value = employee.Emp_Number;
                cell["B8"].Value = employee.Emp_LastName;
                cell["D8"].Value = employee.Emp_FirstName;
                cell["F8"].Value = employee.Emp_MiddleName;
                cell["K8"].Value = employee.Emp_Service_StartDate.Value.ToString("dd MMMM yyyy");

                var employeeRank =
                    oUnitOfWork.Rank.GetRankById(employee.Emp_Curr_Rank == null ? 0 : employee.Emp_Curr_Rank.Value);

                cell["I11"].Value = oUnitOfWork.Employee.GetPositionTitle(employee.Emp_Id);
                var unit =
                    oUnitOfWork.Unit.GetUnitById(employee.Emp_Curr_Unit == null ? 0 : employee.Emp_Curr_Unit.Value);
                cell["B11"].Value = unit.Unit_StationName + ", " + unit.Province_Name;

                var leaveCount = 16;
                foreach (var item in leave)
                {
                    cell["A" + leaveCount].Value = item.StartDate.ToString("dd MMM yyyy") + "-" +
                                                   item.EndDate.ToString("dd MMM yyyy");
                    cell["B" + leaveCount].Value = item.Particular;

                    var earned = item.Earned;
                    var totalEarned = (int) (Math.Round(earned, 3)*1000.0)/1000.0;
                    cell["C" + leaveCount].Value = totalEarned;

                    cell["D" + leaveCount].Value = item.VacLeaveWithPay;

                    if (leaveCount > 16)
                        cell["E" + leaveCount].Formula = "=SUM(E" + (leaveCount - 1) + ",C" + leaveCount + ") - D" +
                                                         leaveCount;
                    else
                        cell["E" + leaveCount].Formula = "=C" + leaveCount + "-D" + leaveCount;

                    cell["F" + leaveCount].Value = item.VacLeaveWithOutPay;


                    cell["G" + leaveCount].Value = totalEarned;
                    cell["H" + leaveCount].Value = item.SickLeaveWithPay;

                    if (leaveCount > 16)
                        cell["I" + leaveCount].Formula = "=SUM(I" + (leaveCount - 1) + ",G" + leaveCount + ") - H" +
                                                         leaveCount;
                    else
                        cell["I" + leaveCount].Formula = "=G" + leaveCount + "-H" + leaveCount;


                    cell["J" + leaveCount ++].Value = item.SickLeaveWithOutPay;
                }


                var rank = "";
                if (employee.Emp_DutyStatus == (int) DutyStatuses.Retired)
                {
                    rank = "Ret " + employeeRank.Rank_Name;
                    cell["A" + leaveCount].Value = employee.Emp_Retired_Date.Value.ToString("dd MMMM yyyy");
                    cell["A" + leaveCount].Style.Font.Weight = ExcelFont.BoldWeight;

                    worksheet.Cells.GetSubrangeAbsolute(leaveCount - 1, 1, leaveCount - 1, 9).Merged = true;
                    cell["B" + leaveCount].Value = "Retired thru TTPD as pe BO #RET-2016-059 dated " +
                                                   employee.Emp_Retired_Date.Value.ToString("dd MMMM yyyy");
                    cell["B" + leaveCount].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    cell["B" + leaveCount].Style.Font.Italic = true;
                }

                else
                    rank = employeeRank.Rank_Name;

                cell["I8"].Value = rank;

                #region Font Weight

                foreach (var ch in "CDEGHI")
                {
                    cell[ch.ToString() + (leaveCount + 2)].Style.Font.Weight = ExcelFont.BoldWeight;
                    cell[ch.ToString() + (leaveCount + 2)].Value = "---------";
                    cell[ch.ToString() + (leaveCount + 3)].Style.Font.Weight = ExcelFont.BoldWeight;
                }

                #endregion

                //VACATION
                cell["C" + (leaveCount + 3)].Formula = "=SUM(C16:C" + leaveCount + ")";
                cell["D" + (leaveCount + 3)].Formula = "=SUM(D16:D" + leaveCount + ")";
                cell["E" + (leaveCount + 3)].Formula = "=C" + (leaveCount + 3) + "-D" + (leaveCount + 3);

                //SICK
                cell["G" + (leaveCount + 3)].Formula = "=SUM(G16:G" + leaveCount + ")";
                cell["H" + (leaveCount + 3)].Formula = "=SUM(H16:H" + leaveCount + ")";
                cell["I" + (leaveCount + 3)].Formula = "=G" + (leaveCount + 3) + "-H" + (leaveCount + 3);

                cell["A" + (leaveCount + 4)].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black,
                    LineStyle.Thin);
                worksheet.Cells.GetSubrangeAbsolute(leaveCount + 4 - 1, 0, leaveCount + 4 - 1, 10).Merged = true;
                cell["A" + (leaveCount + 4)].Value =
                    "Remarks:  *Nr. Of days being deducted due to payment of Monetization of Leave Credits (MLC) as per Certification by C,CMD dtd 05 April 2016.";

                cell["A" + (leaveCount + 5)].Style.Font.Weight = ExcelFont.BoldWeight;
                cell["A" + (leaveCount + 6)].Style.Font.Weight = ExcelFont.BoldWeight;
                worksheet.Cells.GetSubrangeAbsolute(leaveCount + 5 - 1, 0, leaveCount + 5 - 1, 10).Merged = true;
                cell["A" + (leaveCount + 5)].Value = "RECAPITULATION";
                cell["A" + (leaveCount + 5)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                cell["A" + (leaveCount + 5)].Style.Font.Size = 16*18;

                worksheet.Cells.GetSubrangeAbsolute(leaveCount + 6 - 1, 0, leaveCount + 6 - 1, 10).Merged = true;
                cell["A" + (leaveCount + 6)].Value = "Balance as of " + endDate.ToString("dd MMMM yyyy");
                cell["A" + (leaveCount + 6)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;

                cell["D" + (leaveCount + 8)].Style.Font.Weight = ExcelFont.BoldWeight;
                cell["G" + (leaveCount + 8)].Style.Font.Weight = ExcelFont.BoldWeight;
                cell["D" + (leaveCount + 8)].Value = "EARNED";
                cell["G" + (leaveCount + 8)].Value = "ENJOYED";

                //VACATION LEAVE
                cell["A" + (leaveCount + 10)].Style.Font.Weight = ExcelFont.BoldWeight;
                worksheet.Cells.GetSubrangeAbsolute(leaveCount + 10 - 1, 0, leaveCount + 10 - 1, 1).Merged = true;
                cell["A" + (leaveCount + 10)].Value = "Vacation Leave";

                cell["D" + (leaveCount + 10)].Formula = "=C" + (leaveCount + 3);
                cell["G" + (leaveCount + 10)].Formula = "=G" + (leaveCount + 3);

                //SICK LEAVE
                cell["A" + (leaveCount + 11)].Style.Font.Weight = ExcelFont.BoldWeight;
                worksheet.Cells.GetSubrangeAbsolute(leaveCount + 11 - 1, 0, leaveCount + 11 - 1, 1).Merged = true;
                cell["A" + (leaveCount + 11)].Value = "Sick Leave";

                cell["D" + (leaveCount + 11)].Formula = "=G" + (leaveCount + 3);
                cell["G" + (leaveCount + 11)].Formula = "=H" + (leaveCount + 3);


                //TOTAL
                cell["D" + (leaveCount + 12)].Style.Font.Weight = ExcelFont.BoldWeight;
                cell["G" + (leaveCount + 12)].Style.Font.Weight = ExcelFont.BoldWeight;
                cell["D" + (leaveCount + 12)].Value = "-----------";
                cell["G" + (leaveCount + 12)].Value = "-----------";

                cell["A" + (leaveCount + 13)].Style.Font.Weight = ExcelFont.BoldWeight;
                worksheet.Cells.GetSubrangeAbsolute(leaveCount + 13 - 1, 0, leaveCount + 13 - 1, 1).Merged = true;
                cell["A" + (leaveCount + 13)].Value = "TOTAL";

                cell["D" + (leaveCount + 13)].Style.Font.Weight = ExcelFont.BoldWeight;
                cell["G" + (leaveCount + 13)].Style.Font.Weight = ExcelFont.BoldWeight;
                cell["D" + (leaveCount + 13)].Formula = "=SUM(D" + (leaveCount + 10) + ",D" + (leaveCount + 11) + ")";
                cell["G" + (leaveCount + 13)].Formula = "=SUM(G" + (leaveCount + 10) + ",G" + (leaveCount + 11) + ")";

                //TOTAL ACCRUED
                worksheet.Cells.GetSubrangeAbsolute(leaveCount + 15 - 1, 1, leaveCount + 15 - 1, 3).Merged = true;
                cell["B" + (leaveCount + 15)].Style.Font.Weight = ExcelFont.BoldWeight;
                cell["B" + (leaveCount + 15)].Value = "TOTAL ACCRUED LEAVE:";

                worksheet.Cells.GetSubrangeAbsolute(leaveCount + 15 - 1, 4, leaveCount + 15 - 1, 5).Merged = true;
                cell["E" + (leaveCount + 15)].Style.Font.UnderlineStyle = UnderlineStyle.Single;
                cell["E" + (leaveCount + 15)].Style.Font.Weight = ExcelFont.BoldWeight;
                cell["E" + (leaveCount + 15)].Formula = "=D" + (leaveCount + 13) + "-G" + (leaveCount + 13);


                var lastCount = leaveCount + 15;

                for (var initialCount = 16; initialCount <= lastCount; initialCount++)
                {
                    foreach (var ch in "ABCDEFGHIJK")
                    {
                        cell[ch.ToString() + initialCount].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black,
                            LineStyle.Thin);
                    }
                }

                worksheet.Cells.GetSubrangeAbsolute(lastCount + 1 - 1, 0, lastCount + 1 - 1, 10).Merged = true;
                cell["A" + (lastCount + 1)].Value =
                    "      Per OP MC No. 54, CSC MC No. 10 Series 1968 all accumulated leave earned as of June 16, 1960 is inclusive of Saturdays, Sundays & Holidays";
                cell["A" + (lastCount + 1)].Style.Font.Name = "Times New Roman";
                worksheet.Cells.GetSubrangeAbsolute(lastCount + 2 - 1, 0, lastCount + 2 - 1, 10).Merged = true;
                cell["A" + (lastCount + 2)].Value =
                    "while this leave earned beginning June 17, 1960 is exclusive of Saturdays, Sundays & Holidays.";
                cell["A" + (lastCount + 2)].Style.Font.Name = "Times New Roman";

                cell["A" + (lastCount + 4)].Style.Font.Weight = ExcelFont.BoldWeight;
                worksheet.Cells.GetSubrangeAbsolute(lastCount + 4 - 1, 0, lastCount + 4 - 1, 10).Merged = true;
                cell["A" + (lastCount + 4)].Value =
                    "                             I hereby certify on my official oath that this document/computation based on submitted Itemized Leave Record ";
                cell["A" + (lastCount + 4)].Style.Font.Name = "Times New Roman";

                cell["A" + (lastCount + 5)].Style.Font.Weight = ExcelFont.BoldWeight;
                worksheet.Cells.GetSubrangeAbsolute(lastCount + 5 - 1, 0, lastCount + 5 - 1, 10).Merged = true;
                cell["A" + (lastCount + 5)].Value =
                    "           and Available Records on File is correct and I hold myself liable for any erroneous or unauthorized payment due to this";
                cell["A" + (lastCount + 5)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                cell["A" + (lastCount + 5)].Style.Font.Name = "Times New Roman";

                cell["A" + (lastCount + 6)].Style.Font.Weight = ExcelFont.BoldWeight;
                worksheet.Cells.GetSubrangeAbsolute(lastCount + 6 - 1, 0, lastCount + 6 - 1, 3).Merged = true;
                cell["A" + (lastCount + 6)].Value = "              document/certification.";
                cell["A" + (lastCount + 6)].Style.Font.Name = "Times New Roman";

                cell["A" + (lastCount + 8)].Value = "                                       Issued this " +
                                                    DateTime.Now.Day + "th day of " + DateTime.Now.ToString("MMMM yyyy");
                cell["A" + (lastCount + 8)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                cell["A" + (lastCount + 8)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                cell["A" + (lastCount + 8)].Style.Font.Name = "Times New Roman";

                cell["A" + (lastCount + 10)].Value =
                    "                                       PURPOSE: Terminal Leave Claims";
                cell["A" + (lastCount + 10)].Style.Font.Name = "Times New Roman";
                cell["A" + (lastCount + 10)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                cell["A" + (lastCount + 10)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;

                //Processed by:
                cell["A" + (lastCount + 12)].Value = "Processed by:";

                var processedby = oUnitOfWork.Employee.GetEmployeeById(processedBy);
                var processedByRank =
                    oUnitOfWork.Rank.GetRankById(processedby.Emp_Curr_Rank == null ? 0 : processedby.Emp_Curr_Rank.Value);
                worksheet.Cells.GetSubrangeAbsolute(lastCount + 13 - 1, 0, lastCount + 13 - 1, 1).Merged = true;
                cell["A" + (lastCount + 13)].Value = processedByRank.Rank_Name + " " + processedby.Emp_FirstName + " " +
                                                     processedby.Emp_MiddleName + " " + processedby.Emp_LastName + " " +
                                                     processedby.Emp_SuffixName;
                cell["A" + (lastCount + 13)].Style.Font.Weight = ExcelFont.BoldWeight;
                cell["A" + (lastCount + 13)].Style.Font.Name = "Times New Roman";
                cell["A" + (lastCount + 13)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                cell["A" + (lastCount + 13)].Style.VerticalAlignment = VerticalAlignmentStyle.Top;

                worksheet.Cells.GetSubrangeAbsolute(lastCount + 14 - 1, 0, lastCount + 14 - 1, 1).Merged = true;
                cell["A" + (lastCount + 14)].Value = oUnitOfWork.Employee.GetPositionTitle(processedBy);
                cell["A" + (lastCount + 14)].Style.Font.Name = "Times New Roman";
                cell["A" + (lastCount + 14)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                cell["A" + (lastCount + 14)].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
                worksheet.Rows[lastCount + 13].Height = 53*10;
                cell["A" + (lastCount + 14)].Style.WrapText = true;

                //Prepared By
                worksheet.Cells.GetSubrangeAbsolute(lastCount + 12 - 1, 3, lastCount + 12 - 1, 5).Merged = true;
                cell["D" + (lastCount + 12)].Value = "Prepared/Verified by:";
                cell["D" + (lastCount + 12)].Style.Font.Name = "Times New Roman";

                var preparedby = oUnitOfWork.Employee.GetEmployeeById(preparedBy);
                var prepareByRank =
                    oUnitOfWork.Rank.GetRankById(preparedby.Emp_Curr_Rank == null ? 0 : preparedby.Emp_Curr_Rank.Value);
                worksheet.Cells.GetSubrangeAbsolute(lastCount + 13 - 1, 3, lastCount + 13 - 1, 6).Merged = true;
                cell["E" + (lastCount + 13)].Value = prepareByRank.Rank_Name + " " + preparedby.Emp_FirstName + " " +
                                                     preparedby.Emp_MiddleName + " " + preparedby.Emp_LastName + " " +
                                                     preparedby.Emp_SuffixName;
                cell["E" + (lastCount + 13)].Style.Font.Weight = ExcelFont.BoldWeight;
                cell["E" + (lastCount + 13)].Style.Font.Name = "Times New Roman";
                cell["E" + (lastCount + 13)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                cell["E" + (lastCount + 13)].Style.VerticalAlignment = VerticalAlignmentStyle.Top;

                worksheet.Cells.GetSubrangeAbsolute(lastCount + 14 - 1, 3, lastCount + 14 - 1, 6).Merged = true;
                cell["E" + (lastCount + 14)].Value = oUnitOfWork.Employee.GetPositionTitle(preparedBy);
                cell["E" + (lastCount + 14)].Style.Font.Name = "Times New Roman";
                cell["E" + (lastCount + 14)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                cell["E" + (lastCount + 14)].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
                worksheet.Rows[lastCount + 13].Height = 53*10;
                cell["E" + (lastCount + 14)].Style.WrapText = true;


                //Certified By
                worksheet.Cells.GetSubrangeAbsolute(lastCount + 12 - 1, 8, lastCount + 12 - 1, 10).Merged = true;
                cell["I" + (lastCount + 12)].Value = "Prepared/Verified by:";
                cell["I" + (lastCount + 12)].Style.Font.Name = "Times New Roman";

                var certifiedby = oUnitOfWork.Employee.GetEmployeeById(certifiedBy);
                worksheet.Cells.GetSubrangeAbsolute(lastCount + 13 - 1, 8, lastCount + 13 - 1, 11).Merged = true;
                cell["I" + (lastCount + 13)].Value = certifiedby.Emp_FirstName + " " + certifiedby.Emp_MiddleName + " " +
                                                     certifiedby.Emp_LastName + " " + certifiedby.Emp_SuffixName;
                cell["I" + (lastCount + 13)].Style.Font.Weight = ExcelFont.BoldWeight;
                cell["I" + (lastCount + 13)].Style.Font.Name = "Times New Roman";
                cell["I" + (lastCount + 13)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                cell["I" + (lastCount + 13)].Style.VerticalAlignment = VerticalAlignmentStyle.Top;

                var certifiedByRank = oUnitOfWork.Rank.GetRankById(certifiedby.Emp_Curr_Rank ?? 0);
                worksheet.Cells.GetSubrangeAbsolute(lastCount + 14 - 1, 8, lastCount + 14 - 1, 10).Merged = true;
                cell["I" + (lastCount + 14)].Value = certifiedByRank.Rank_Name + "        " +
                                                     oUnitOfWork.Employee.GetPositionTitle(certifiedBy);
                cell["I" + (lastCount + 14)].Style.Font.Name = "Times New Roman";
                cell["I" + (lastCount + 14)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                cell["I" + (lastCount + 14)].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
                worksheet.Rows[lastCount + 13].Height = 53*10;
                cell["I" + (lastCount + 14)].Style.WrapText = true;
                ef.Save(newFile);

                return Path.GetFileName(newFile);
            }
            return "";
        }

        public string PrintRegionalAuthorizeStrReport()
        {
            IHRISUnitOfWork oUnitOfWork = new HRISUnitOfWork(context);
            var pesonnels = oUnitOfWork.Report.GetPersonnelPerRegion();

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            var newFile =
                $"{applicationPath}{@"\Content\MISC\Generated\RegionalAuthorizeStrength_"}{DateTime.Now.Ticks}{".xlsx"}";
            var template = $"{applicationPath}{@"\Content\MISC\Template\RegionalAuthorizeStrength.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["Sheet1"];

            var cell = worksheet.Cells;

            var row = 6;
            foreach (var item in pesonnels)
            {
                foreach (var ch in "ABCDEFGHI")
                    cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black,
                        LineStyle.Thin);

                cell["A" + row].Value = item.Region.Replace("Region ", "");
                cell["C" + row].Value = item.SFO4;
                cell["D" + row].Value = item.SFO3;
                cell["E" + row].Value = item.SFO2;
                cell["F" + row].Value = item.SFO1;
                cell["G" + row].Value = item.FO3;
                cell["H" + row].Value = item.FO2;
                cell["I" + row].Value = item.FO1;
                cell["J" + row].Formula = "=SUM(C" + row + ":I" + row + ")";
                row++;
            }

            foreach (var ch in "CDEFGHIJ")
                cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);

            var rankTotal = row - 1;
            cell["C" + row].Formula = "=SUM(C6:C" + (row - 1) + ")";
            cell["D" + row].Formula = "=SUM(D6:D" + (row - 1) + ")";
            cell["E" + row].Formula = "=SUM(E6:E" + (row - 1) + ")";
            cell["F" + row].Formula = "=SUM(F6:F" + (row - 1) + ")";
            cell["G" + row].Formula = "=SUM(G6:G" + (row - 1) + ")";
            cell["H" + row].Formula = "=SUM(H6:H" + (row - 1) + ")";
            cell["I" + row].Formula = "=SUM(I6:I" + (row - 1) + ")";
            cell["J" + row].Formula = "=SUM(C" + row + ":I" + row + ")";

            // "GRAND TOTAL"
            var grandTotal = row + 1;

            worksheet.Cells.GetSubrangeAbsolute(grandTotal - 1, 5, grandTotal - 1, 7).Merged = true;

            worksheet.Cells.GetSubrangeAbsolute(grandTotal - 1, 8, grandTotal - 1, 9).Merged = true;

            cell["F" + grandTotal].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
            cell["I" + grandTotal].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
            cell["F" + grandTotal].Value = "GRAND TOTAL";
            cell["F" + grandTotal].Style.Font.Weight = ExcelFont.BoldWeight;
            cell["I" + grandTotal].Formula = "=SUM(C" + row + ":I" + row + ")";
            cell["I" + grandTotal].Style.Font.Weight = ExcelFont.BoldWeight;

            var total = row + 2;
            cell["F2"].Value = "as of " + DateTime.Now.ToString("dd MMMM yyyy");

            // DBM AUTHORIZED

            foreach (var ch in "CDEFGHI")
                cell[ch.ToString() + total].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black,
                    LineStyle.Thin);

            worksheet.Cells.GetSubrangeAbsolute(total - 1, 0, total - 1, 1).Merged = true;
            cell["A" + total].Value = "DBM AUTHORIZED";
            cell["A" + total].Style.Font.Weight = ExcelFont.BoldWeight;
            cell["A" + total].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);

            cell["C" + total].Value = oUnitOfWork.Rank.GetRankDBMById((int) Rank.SFO4);
            cell["D" + total].Value = oUnitOfWork.Rank.GetRankDBMById((int) Rank.SFO3);
            cell["E" + total].Value = oUnitOfWork.Rank.GetRankDBMById((int) Rank.SFO2);
            cell["F" + total].Value = oUnitOfWork.Rank.GetRankDBMById((int) Rank.SFO1);
            cell["G" + total].Value = oUnitOfWork.Rank.GetRankDBMById((int) Rank.FO3);
            cell["H" + total].Value = oUnitOfWork.Rank.GetRankDBMById((int) Rank.FO2);
            cell["I" + total].Value = oUnitOfWork.Rank.GetRankDBMById((int) Rank.FO1);


            worksheet.Cells.GetSubrangeAbsolute(total + 1 - 1, 8, total + 1 - 1, 9).Merged = true;
            cell["I" + (total + 1)].Formula = "=SUM(C" + row + ":I" + row + ")";
            cell["I" + (total + 1)].Style.Font.Weight = ExcelFont.BoldWeight;
            cell["I" + (total + 1)].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);

            var lastRow = total + 2;

            foreach (var ch in "CDEFGHI")
            {
                cell[ch.ToString() + lastRow].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black,
                    LineStyle.Thin);
                cell[ch.ToString() + lastRow].Formula = "=(" + ch + total + "-" + ch + row + ")";
            }
            ef.Save(newFile);

            return Path.GetFileName(newFile);
        }

        public string PrintPersonnelStrengthReport(int preparedBy, int reviewedBy, int notedBy)
        {
            IHRISUnitOfWork oUnitOfWork = new HRISUnitOfWork(context);
            var pesonnels = oUnitOfWork.Report.GetPersonnelPerRegion();

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            var newFile =
                $"{applicationPath}{@"\Content\MISC\Generated\PersonnelNationwide_"}{DateTime.Now.Ticks}{".xlsx"}";
            var template = $"{applicationPath}{@"\Content\MISC\Template\PersonnelNationwide.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["Sheet1"];

            var cell = worksheet.Cells;

            cell["A5"].Value = "as of " + DateTime.Now.ToString("dd MMMM yyyy");

            var row = 7;
            foreach (var item in pesonnels)
            {
                foreach (var ch in "ABCDEFGHIJKLMNOPQRST")
                {
                    cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black,
                        LineStyle.Thin);
                }
                cell["A" + row].Value = item.Region.Replace("Region ", "");
                cell["B" + row].Value = item.DIR;
                cell["C" + row].Value = item.CSUPT;
                cell["D" + row].Value = item.SSUPT;
                cell["E" + row].Value = item.SUPT;
                cell["F" + row].Value = item.CINSP;
                cell["G" + row].Value = item.SINSP;
                cell["H" + row].Value = item.INSP;
                cell["I" + row].Formula = "=SUM(B" + row + ":H" + row + ")";

                cell["J" + row].Value = item.SFO4;
                cell["K" + row].Value = item.SFO3;
                cell["L" + row].Value = item.SFO2;
                cell["M" + row].Value = item.SFO1;
                cell["N" + row].Value = item.FO3;
                cell["O" + row].Value = item.FO2;
                cell["P" + row].Value = item.FO1;
                cell["Q" + row].Formula = "=SUM(J" + row + ":P" + row + ")";
                cell["R" + row].Formula = "=SUM(I" + row + ",Q" + row + ")";
                cell["S" + row].Value = item.NUP;
                cell["T" + row].Formula = "=SUM(R" + row + ",S" + row + ")";
                row++;
            }


            cell["A" + row].Value = "TOTAL";
            foreach (var ch in "ABCDEFGHIJKLMNOPQRST")
            {
                cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                cell[ch.ToString() + row].Style.Font.Weight = ExcelFont.BoldWeight;
            }
            cell["B" + row].Formula = "=SUM(B7:B" + (row - 1) + ")";
            cell["C" + row].Formula = "=SUM(C7:C" + (row - 1) + ")";
            cell["D" + row].Formula = "=SUM(D7:D" + (row - 1) + ")";
            cell["E" + row].Formula = "=SUM(E7:E" + (row - 1) + ")";
            cell["F" + row].Formula = "=SUM(F7:F" + (row - 1) + ")";
            cell["G" + row].Formula = "=SUM(G7:G" + (row - 1) + ")";
            cell["H" + row].Formula = "=SUM(H7:H" + (row - 1) + ")";
            cell["I" + row].Formula = "=SUM(I7:I" + (row - 1) + ")";

            cell["J" + row].Formula = "=SUM(J7:J" + (row - 1) + ")";
            cell["K" + row].Formula = "=SUM(K7:K" + (row - 1) + ")";
            cell["L" + row].Formula = "=SUM(L7:L" + (row - 1) + ")";
            cell["M" + row].Formula = "=SUM(M7:M" + (row - 1) + ")";
            cell["N" + row].Formula = "=SUM(N7:N" + (row - 1) + ")";
            cell["O" + row].Formula = "=SUM(O7:O" + (row - 1) + ")";
            cell["P" + row].Formula = "=SUM(P7:P" + (row - 1) + ")";
            cell["Q" + row].Formula = "=SUM(Q7:Q" + (row - 1) + ")";

            cell["R" + row].Formula = "=SUM(I" + row + ",Q" + row + ")";
            cell["S" + row].Formula = "=SUM(S7:S" + (row - 1) + ")";

            cell["T" + row].Formula = "=SUM(R" + row + ",S" + row + ")";

            worksheet.Cells.GetSubrangeAbsolute(row + 1 - 1, 0, row + 1 - 1, 11).Merged = true;
            cell["A" + (row + 1)].Value =
                "Note:  1. Officers and NUP are based on the monitoring of the Personnerl Accounting Section, PRMD";
            cell["A" + (row + 1)].Style.Font.Italic = true;
            cell["A" + (row + 1)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

            worksheet.Cells.GetSubrangeAbsolute(row + 2 - 1, 0, row + 2 - 1, 11).Merged = true;
            cell["A" + (row + 2)].Value = "            2. NOR are based on submitted Report of Variance by the Regions";
            cell["A" + (row + 2)].Style.Font.Italic = true;
            cell["A" + (row + 2)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;


            //Prepared By
            worksheet.Cells.GetSubrangeAbsolute(row + 4 - 1, 1, row + 4 - 1, 2).Merged = true;
            cell["B" + (row + 4)].Value = "Prepared By:";

            var preparedby = oUnitOfWork.Employee.GetEmployeeById(preparedBy);
            worksheet.Cells.GetSubrangeAbsolute(row + 6 - 1, 1, row + 6 - 1, 3).Merged = true;
            cell["B" + (row + 6)].Value = preparedby.Emp_FirstName + " " + preparedby.Emp_MiddleName + " " +
                                          preparedby.Emp_LastName + " " + preparedby.Emp_SuffixName;
            cell["B" + (row + 6)].Style.Font.Weight = ExcelFont.BoldWeight;

            var preparedByRank = oUnitOfWork.Rank.GetRankById(preparedby.Emp_Curr_Rank ?? 0);
            worksheet.Cells.GetSubrangeAbsolute(row + 7 - 1, 1, row + 7 - 1, 3).Merged = true;
            cell["B" + (row + 7)].Value = preparedByRank.Rank_Name + "        " +
                                          oUnitOfWork.Employee.GetPositionTitle(preparedBy);
            cell["B" + (row + 7)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            cell["B" + (row + 7)].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            worksheet.Rows[row + 6].Height = 53*10;

            //Reviewed By
            worksheet.Cells.GetSubrangeAbsolute(row + 4 - 1, 7, row + 4 - 1, 8).Merged = true;
            cell["H" + (row + 4)].Value = "Reviewed By:";

            var reviewedby = oUnitOfWork.Employee.GetEmployeeById(reviewedBy);
            worksheet.Cells.GetSubrangeAbsolute(row + 6 - 1, 7, row + 6 - 1, 9).Merged = true;
            cell["H" + (row + 6)].Value = reviewedby.Emp_FirstName + " " + reviewedby.Emp_MiddleName + " " +
                                          reviewedby.Emp_LastName + " " + reviewedby.Emp_SuffixName;
            cell["H" + (row + 6)].Style.Font.Weight = ExcelFont.BoldWeight;

            var reviewedByRank = oUnitOfWork.Rank.GetRankById(reviewedby.Emp_Curr_Rank ?? 0);
            worksheet.Cells.GetSubrangeAbsolute(row + 7 - 1, 7, row + 7 - 1, 9).Merged = true;
            cell["H" + (row + 7)].Value = reviewedByRank.Rank_Name + "        " +
                                          oUnitOfWork.Employee.GetPositionTitle(reviewedBy);
            cell["H" + (row + 7)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            cell["H" + (row + 7)].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            worksheet.Rows[row + 6].Height = 53*10;

            //Noted By
            worksheet.Cells.GetSubrangeAbsolute(row + 4 - 1, 16, row + 4 - 1, 17).Merged = true;
            cell["Q" + (row + 4)].Value = "Noted By:";

            var notedby = oUnitOfWork.Employee.GetEmployeeById(notedBy);
            worksheet.Cells.GetSubrangeAbsolute(row + 6 - 1, 16, row + 6 - 1, 18).Merged = true;
            cell["Q" + (row + 6)].Value = notedby.Emp_FirstName + " " + notedby.Emp_MiddleName + " " +
                                          notedby.Emp_LastName + " " + notedby.Emp_SuffixName;
            cell["Q" + (row + 6)].Style.Font.Weight = ExcelFont.BoldWeight;

            var notedByRank = oUnitOfWork.Rank.GetRankById(notedby.Emp_Curr_Rank ?? 0);
            worksheet.Cells.GetSubrangeAbsolute(row + 7 - 1, 16, row + 7 - 1, 18).Merged = true;
            cell["Q" + (row + 7)].Value = notedByRank.Rank_Name + "        " + oUnitOfWork.Employee.GetPositionTitle(notedBy); 
            cell["Q" + (row + 7)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            cell["Q" + (row + 7)].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            worksheet.Rows[row + 6].Height = 53*10;

            ef.Save(newFile);
            return Path.GetFileName(newFile);
        }

        public string PrintActualVsAuthorizedReport(int preparedBy, int reviewedBy, int notedBy)
        {
            IHRISUnitOfWork oUnitOfWork = new HRISUnitOfWork(context);

            var ranks = oUnitOfWork.Rank.GetRanklList();

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            var newFile =
                $"{applicationPath}{@"\Content\MISC\Generated\ActualVsAuthorizedStrength_"}{DateTime.Now.Ticks}{".xlsx"}";
            var template = $"{applicationPath}{@"\Content\MISC\Template\ActualVsAuthorizedStrength.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["Sheet1"];

            var cell = worksheet.Cells;
            cell["B7"].Value = "as of " + DateTime.Now.ToString("dd MMMM yyyy");

            var row = 11;

            foreach (var item in ranks)
            {
                if (row != 18 && row != 26)
                {
                    cell["B" + row].Value = item.Rank;
                    cell["E" + row].Value = item.DBMAuthorized;
                    cell["H" + row].Value = item.ActualStrength;
                }
                else if (row == 18)
                {
                    var rows = row + 1;
                    cell["B" + rows].Value = item.Rank;
                    cell["E" + rows].Value = item.DBMAuthorized;
                    cell["H" + rows].Value = item.ActualStrength;
                    row = rows ++;
                }
                else if (row == 26)
                {
                    var rows = row + 2;
                    cell["B" + rows].Value = item.Rank;
                    cell["E" + rows].Value = item.DBMAuthorized;
                    cell["H" + rows].Value = item.ActualStrength;
                }

                row++;
            }

            //Prepared By
            var preparedby = oUnitOfWork.Employee.GetEmployeeById(preparedBy);
            cell["B36"].Value = preparedby.Emp_FirstName + " " + preparedby.Emp_MiddleName + " " +
                                preparedby.Emp_LastName + " " + preparedby.Emp_SuffixName;

            var preparedByRank = oUnitOfWork.Rank.GetRankById(preparedby.Emp_Curr_Rank ?? 0);
            cell["B37"].Value = preparedByRank.Rank_Name + "        " + oUnitOfWork.Employee.GetPositionTitle(preparedBy);

            //Reviewed By
            var reviewedby = oUnitOfWork.Employee.GetEmployeeById(reviewedBy);
            cell["K36"].Value = reviewedby.Emp_FirstName + " " + reviewedby.Emp_MiddleName + " " +
                                reviewedby.Emp_LastName + " " + reviewedby.Emp_SuffixName;

            var reviewedByRank = oUnitOfWork.Rank.GetRankById(reviewedby.Emp_Curr_Rank ?? 0);
            cell["K37"].Value = reviewedByRank.Rank_Name + "        " + oUnitOfWork.Employee.GetPositionTitle(reviewedBy);

            //Noted By
            var notedby = oUnitOfWork.Employee.GetEmployeeById(notedBy);
            cell["F42"].Value = notedby.Emp_FirstName + " " + notedby.Emp_MiddleName + " " + notedby.Emp_LastName + " " +
                                notedby.Emp_SuffixName;

            var notedByRank = oUnitOfWork.Rank.GetRankById(notedby.Emp_Curr_Rank ?? 0);
            cell["F43"].Value = notedByRank.Rank_Name + "        " + oUnitOfWork.Employee.GetPositionTitle(notedBy);

            ef.Save(newFile);

            return Path.GetFileName(newFile);
        }

        public string PrintServiceRecord(int employeeId, int preparedBy,int verifiedBy, int certifiedBy, string remarks)
        {
            IHRISUnitOfWork oUnitOfWork = new HRISUnitOfWork(context);

            var employee = oUnitOfWork.Employee.GetEmployeeById(employeeId);
            var serviceAppointment = oUnitOfWork.ServiceRecord.GetServiceAppointmentByEmpId(employeeId);

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            var newFile =
                $"{applicationPath}{@"\Content\MISC\Generated\ServiceRecord_"}{DateTime.Now.Ticks}{".xlsx"}";
            var template = $"{applicationPath}{@"\Content\MISC\Template\ServiceRecord.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["Sheet1"];

            var cell = worksheet.Cells;
            var ranks = oUnitOfWork.Rank.GetList();

            var rank = employee.Emp_Curr_Rank > 0
                ? ranks.FirstOrDefault(a => a.Rank_Id == employee.Emp_Curr_Rank).Rank_Name + " "
                : "";
            var empSuffixName = string.IsNullOrEmpty(employee.Emp_SuffixName) || employee.Emp_SuffixName == "n/a" ||
                                employee.Emp_SuffixName == "N/A"
                ? ""
                : employee.Emp_SuffixName;
            var empMiddleName = !string.IsNullOrEmpty(employee.Emp_MiddleName) ? employee.Emp_MiddleName[0].ToString() : "";

            cell["C11"].Value = rank + employee.Emp_FirstName + " " + empMiddleName +", " +
             employee.Emp_LastName + " " + empSuffixName;
                               
            if (employee.Emp_BirthDate != null)
            {
                cell["C13"].Value = employee.Emp_BirthDate.Value.ToString("dd MMMM yyyy");
                var retirementDate = Functions.GetRetirementDate(employee.Emp_BirthDate.Value, employee.Emp_Curr_Rank);
                cell["D15"].Value = retirementDate?.ToString("dd MMMM yyyy") ?? "";
            }
            var birthPace = "";
            if (!string.IsNullOrEmpty(employee.Emp_BirthPlace))
                 birthPace = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(employee.Emp_BirthPlace);
           
            cell["I13"].Value = birthPace;

            cell["D15"].Style.VerticalAlignment = VerticalAlignmentStyle.Top;

            cell["B23"].Value = remarks;

            var row = 24;

            foreach (var item in serviceAppointment)
            {
                foreach (var ch in "BCDEFGH")
                {
                    cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black,
                        LineStyle.Thin);
                }

                if (item.ESA_ApptDate != null)
                    cell["B" + row].Value = item.ESA_ApptDate.Value.ToString("dd MMM yy");
                if (item.ESA_EndDate != null)
                    cell["C" + row].Value = item.ESA_EndDate.Value.ToString("dd MMM yy");
                cell["D" + row].Value = (Rank) item.ESA_Rank + "/" + item.ESA_PosDesignation;
                cell["F" + row].Value = (AppointmentStatuses) Convert.ToInt32(item.ESA_Appt_Status);
                cell["G" + row].Value = "P" + string.Format("{0:n}", item.ESA_SalaryAmt);

                //var unit = oUnitOfWork.Unit.GetUnitById(item.ESA_Unit_Id);
                cell["H" + row++].Value = item.ESA_Office_Entity;
            }

            if (row <= 27)
                row = 28;

            worksheet.Cells.GetSubrangeAbsolute(row + 1 - 1, 1, row + 1 - 1, 14).Merged = true;
            cell["B" + (row + 1)].Value =
                "Issued in compliance with Executive Order No. 54 dated August 10, 1954 and in accordance with Circular No. 58 dated August 10,";
            cell["B" + (row + 1)].Style.Font.Italic = true;
            cell["B" + (row + 1)].Style.Font.Name = "Times New Roman";
            cell["B" + (row + 1)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

            worksheet.Cells.GetSubrangeAbsolute(row + 2 - 1, 1, row + 2 - 1, 6).Merged = true;
            cell["B" + (row + 2)].Value = "1954 of the system.";
            cell["B" + (row + 2)].Style.Font.Italic = true;
            cell["B" + (row + 2)].Style.Font.Name = "Times New Roman";
            cell["B" + (row + 2)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

            worksheet.Cells.GetSubrangeAbsolute(row + 4 - 1, 1, row + 4 - 1, 3).Merged = true;
            cell["B" + (row + 4)].Value = "Prepared by:";
            cell["B" + (row + 4)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

            worksheet.Cells.GetSubrangeAbsolute(row + 4 - 1, 1, row + 4 - 1, 3).Merged = true;
            cell["E" + (row + 4)].Value = "Verified by:";
            cell["E" + (row + 4)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;


            worksheet.Cells.GetSubrangeAbsolute(row + 4 - 1, 6, row + 4 - 1, 8).Merged = true;
            cell["H" + (row + 4)].Value = "Certified By:";
            cell["H" + (row + 4)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

            //Prepared By
            var preparedby = oUnitOfWork.Employee.GetEmployeeById(preparedBy);
            worksheet.Cells.GetSubrangeAbsolute(row + 6 - 1, 1, row + 6 - 1, 3).Merged = true;
            cell["B" + (row + 6)].Value = preparedby.Emp_FirstName + " " + preparedby.Emp_MiddleName + " " +
                                          preparedby.Emp_LastName + " " + preparedby.Emp_SuffixName;
            cell["B" + (row + 6)].Style.Font.Weight = ExcelFont.BoldWeight;


            var preparedByRank = oUnitOfWork.Rank.GetRankById(preparedby.Emp_Curr_Rank ?? 0);
            worksheet.Cells.GetSubrangeAbsolute(row + 7 - 1, 1, row + 7 - 1, 2).Merged = true;
            cell["B" + (row + 7)].Value = preparedByRank.Rank_Name + "        " +
                                           oUnitOfWork.Employee.GetPositionTitle(preparedBy);
            cell["B" + (row + 7)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            cell["B" + (row + 7)].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            worksheet.Rows[row + 6].Height = 53*10;
            cell["B" + (row + 7)].Style.WrapText = true;

            //Verified by
            var verifiedby = oUnitOfWork.Employee.GetEmployeeById(verifiedBy);
            worksheet.Cells.GetSubrangeAbsolute(row + 6 - 1, 1, row + 6 - 1, 3).Merged = true;
            cell["E" + (row + 6)].Value = verifiedby.Emp_FirstName + " " + verifiedby.Emp_MiddleName + " " +
                                          verifiedby.Emp_LastName + " " + verifiedby.Emp_SuffixName;
            cell["E" + (row + 6)].Style.Font.Weight = ExcelFont.BoldWeight;


            var verifiedByRank = oUnitOfWork.Rank.GetRankById(verifiedby.Emp_Curr_Rank ?? 0);
            worksheet.Cells.GetSubrangeAbsolute(row + 7 - 1, 1, row + 7 - 1, 2).Merged = true;
            cell["E" + (row + 7)].Value = verifiedByRank.Rank_Name + "        " +
                                           oUnitOfWork.Employee.GetPositionTitle(verifiedBy);
            cell["E" + (row + 7)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            cell["E" + (row + 7)].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            worksheet.Rows[row + 6].Height = 53 * 10;
            cell["E" + (row + 7)].Style.WrapText = true;

            ////Certified By
            var certifiedby = oUnitOfWork.Employee.GetEmployeeById(certifiedBy);

            worksheet.Cells.GetSubrangeAbsolute(row + 6 - 1, 6, row + 6 - 1, 8).Merged = true;
            cell["H" + (row + 6)].Value = certifiedby.Emp_FirstName + " " + certifiedby.Emp_MiddleName + " " +
                                          certifiedby.Emp_LastName + " " + certifiedby.Emp_SuffixName;
            cell["H" + (row + 6)].Style.Font.Weight = ExcelFont.BoldWeight;


            var certifiedByRank = oUnitOfWork.Rank.GetRankById(certifiedby.Emp_Curr_Rank ?? 0);
            worksheet.Cells.GetSubrangeAbsolute(row + 7 - 1, 6, row + 7 - 1, 7).Merged = true;
            cell["H" + (row + 7)].Value = certifiedByRank.Rank_Name + "        " +
                                          oUnitOfWork.Employee.GetPositionTitle(certifiedBy);
            cell["H" + (row + 7)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            cell["H" + (row + 7)].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            worksheet.Rows[row + 6].Height = 53*10;
            cell["H" + (row + 7)].Style.WrapText = true;

            ef.Save(newFile);

            return Path.GetFileName(newFile);
        }
        public string PrintLongevityPay(int official1, int official2, DateTime startDate, DateTime endDate)
        {
            IHRISUnitOfWork oUnitOfWork = new HRISUnitOfWork(context);

            var longevityPayFor5Years = oUnitOfWork.Report.GetLongevityPay(startDate, endDate,
                (int) LongevityYear.FiveYears);
            var longevityPayFor10Years = oUnitOfWork.Report.GetLongevityPay(startDate, endDate,
                (int) LongevityYear.TenYears);
            var longevityPayFor15Years = oUnitOfWork.Report.GetLongevityPay(startDate, endDate,
                (int) LongevityYear.FifteenYears);
            var longevityPayFor20Years = oUnitOfWork.Report.GetLongevityPay(startDate, endDate,
                (int) LongevityYear.TwentyYears);
            var longevityPayFor25Years = oUnitOfWork.Report.GetLongevityPay(startDate, endDate,
                (int) LongevityYear.TwentyFiveYears);

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            var newFile =
                $"{applicationPath}{@"\Content\MISC\Generated\LongevityPay_"}{DateTime.Now.Ticks}{".xlsx"}";
            var template = $"{applicationPath}{@"\Content\MISC\Template\LongevityPay.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["Sheet1"];

            var cell = worksheet.Cells;
            cell["G7"].Value = DateTime.Now.ToString("MMMM dd yyyy");

            var row = 16;

            // 5 years
            foreach (var item in longevityPayFor5Years)
            {
                worksheet.Cells.GetSubrangeAbsolute(row - 1, 0, row - 1, 3).Merged = true;
                foreach (var ch in "AEFG")
                {
                    cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black,
                        LineStyle.Thin);
                    cell[ch.ToString() + row].Style.Font.Name = "Times New Roman";
                    cell[ch.ToString() + row].Style.Font.Size = 10*20;
                }
                cell["A" + row].Value = item.Rank + " " + item.FullName;
                cell["E" + row].Value = item.UnitName == ", " ? "" : item.UnitName;

                cell["F" + row].Value = item.Batch;
                cell["F" + row].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                cell["G" + row++].Value =
                    item.EffectiveDate.AddYears(+(int) LongevityYear.FiveYears).ToString("MMMM dd yyyy");
            }

            //10 years
            foreach (var item in longevityPayFor10Years)
            {
                worksheet.Cells.GetSubrangeAbsolute(row - 1, 0, row - 1, 3).Merged = true;
                foreach (var ch in "AEFG")
                {
                    cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black,
                        LineStyle.Thin);
                    cell[ch.ToString() + row].Style.Font.Name = "Times New Roman";
                    cell[ch.ToString() + row].Style.Font.Size = 10*20;
                }
                cell["A" + row].Value = item.Rank + " " + item.FullName;
                cell["E" + row].Value = item.UnitName == ", " ? "" : item.UnitName;

                cell["F" + row].Value = item.Batch;
                cell["F" + row].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                cell["G" + row++].Value =
                    item.EffectiveDate.AddYears(+(int) LongevityYear.TenYears).ToString("MMMM dd yyyy");
            }


            //15 years
            foreach (var item in longevityPayFor15Years)
            {
                worksheet.Cells.GetSubrangeAbsolute(row - 1, 0, row - 1, 3).Merged = true;
                foreach (var ch in "AEFG")
                {
                    cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black,
                        LineStyle.Thin);
                    cell[ch.ToString() + row].Style.Font.Name = "Times New Roman";
                    cell[ch.ToString() + row].Style.Font.Size = 10*20;
                }
                cell["A" + row].Value = item.Rank + " " + item.FullName;
                cell["E" + row].Value = item.UnitName == ", " ? "" : item.UnitName;

                cell["F" + row].Value = item.Batch;
                cell["F" + row].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                cell["G" + row++].Value =
                    item.EffectiveDate.AddYears(+(int) LongevityYear.FifteenYears).ToString("MMMM dd yyyy");
            }


            //20 years
            foreach (var item in longevityPayFor20Years)
            {
                worksheet.Cells.GetSubrangeAbsolute(row - 1, 0, row - 1, 3).Merged = true;
                foreach (var ch in "AEFG")
                {
                    cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black,
                        LineStyle.Thin);
                    cell[ch.ToString() + row].Style.Font.Name = "Times New Roman";
                    cell[ch.ToString() + row].Style.Font.Size = 10*20;
                }
                cell["A" + row].Value = item.Rank + " " + item.FullName;
                cell["E" + row].Value = item.UnitName == ", " ? "" : item.UnitName;

                cell["F" + row].Value = item.Batch;
                cell["F" + row].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                cell["G" + row++].Value =
                    item.EffectiveDate.AddYears(+(int) LongevityYear.TwentyYears).ToString("MMMM dd yyyy");
            }


            //25 years
            foreach (var item in longevityPayFor25Years)
            {
                worksheet.Cells.GetSubrangeAbsolute(row - 1, 0, row - 1, 3).Merged = true;
                foreach (var ch in "AEFG")
                {
                    cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black,
                        LineStyle.Thin);
                    cell[ch.ToString() + row].Style.Font.Name = "Times New Roman";
                    cell[ch.ToString() + row].Style.Font.Size = 10*20;
                }
                cell["A" + row].Value = item.Rank + " " + item.FullName;
                cell["E" + row].Value = item.UnitName == ", " ? "" : item.UnitName;

                cell["F" + row].Value = item.Batch;
                cell["F" + row].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                cell["G" + row++].Value =
                    item.EffectiveDate.AddYears(+(int) LongevityYear.TwentyFiveYears).ToString("MMMM dd yyyy");
            }

            //Officials
            worksheet.Cells.GetSubrangeAbsolute(row + 3 - 1, 0, row + 3 - 1, 2).Merged = true;
            cell["A" + (row + 3)].Value = "OFFICIAL:";
            cell["A" + (row + 3)].Style.Font.Name = "Times New Roman";

            var official = oUnitOfWork.Employee.GetEmployeeById(official1);
            worksheet.Cells.GetSubrangeAbsolute(row + 5 - 1, 0, row + 5 - 1, 2).Merged = true;
            cell["A" + (row + 5)].Value = official.Emp_FirstName + " " + official.Emp_MiddleName + " " +
                                          official.Emp_LastName + " " + official.Emp_SuffixName;
            cell["A" + (row + 5)].Style.Font.Weight = ExcelFont.BoldWeight;
            cell["A" + (row + 5)].Style.Font.Name = "Times New Roman";
            cell["A" + (row + 5)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            cell["A" + (row + 5)].Style.VerticalAlignment = VerticalAlignmentStyle.Top;

            var officialRank = oUnitOfWork.Rank.GetRankById(official.Emp_Curr_Rank ?? 0);
            worksheet.Cells.GetSubrangeAbsolute(row + 6 - 1, 0, row + 6 - 1, 2).Merged = true;
            cell["A" + (row + 6)].Value = officialRank.Rank_Name + "        " + oUnitOfWork.Employee.GetPositionTitle(official1); 
            cell["A" + (row + 6)].Style.Font.Name = "Times New Roman";
            cell["A" + (row + 6)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            cell["A" + (row + 6)].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            worksheet.Rows[row + 5].Height = 53*10;
            cell["A" + (row + 6)].Style.WrapText = true;

            //Officials 2 
            var preparedby = oUnitOfWork.Employee.GetEmployeeById(official2);
            worksheet.Cells.GetSubrangeAbsolute(row + 5 - 1, 5, row + 5 - 1, 6).Merged = true;
            cell["F" + (row + 5)].Value = preparedby.Emp_FirstName + " " + preparedby.Emp_MiddleName + " " +
                                          preparedby.Emp_LastName + " " + preparedby.Emp_SuffixName;
            cell["F" + (row + 5)].Style.Font.Weight = ExcelFont.BoldWeight;
            cell["F" + (row + 5)].Style.Font.Name = "Times New Roman";
            cell["F" + (row + 5)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            cell["F" + (row + 5)].Style.VerticalAlignment = VerticalAlignmentStyle.Top;


            var prepareByRank = oUnitOfWork.Rank.GetRankById(preparedby.Emp_Curr_Rank ?? 0);
            worksheet.Cells.GetSubrangeAbsolute(row + 6 - 1, 5, row + 6 - 1, 6).Merged = true;
            cell["F" + (row + 6)].Value = prepareByRank.Rank_Name + "        " + oUnitOfWork.Employee.GetPositionTitle(official2);
            cell["F" + (row + 6)].Style.Font.Name = "Times New Roman";
            cell["F" + (row + 6)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            cell["F" + (row + 6)].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
            worksheet.Rows[row + 5].Height = 53*10;
            cell["F" + (row + 6)].Style.WrapText = true;

            ef.Save(newFile);

            return Path.GetFileName(newFile);
        }

        public string PrintCommLeaveCredits(int retiredEmployee, int processedBy, int verifiedBy, int certifiedBy,
            string remarks)
        {
            IHRISUnitOfWork oUnitOfWork = new HRISUnitOfWork(context);

            var employee = oUnitOfWork.Report.GetCommLeaveCredits(retiredEmployee);

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            var newFile =
                $"{applicationPath}{@"\Content\MISC\Generated\CommutationOfLeaveCredits_"}{DateTime.Now.Ticks}{".xlsx"}";
            var template = $"{applicationPath}{@"\Content\MISC\Template\CommutationOfLeaveCredits.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["Sheet1"];

            var cell = worksheet.Cells;
            cell["C11"].Value = "Ret. " + employee.Rank + " " + employee.FullName;
            cell["D13"].Value = employee.UnitName;

            var earned = employee.Earned;
            var totalEarned = (int) (Math.Round(earned, 3)*1000.0)/1000.0;

            cell["E20"].Value = totalEarned;
            cell["E22"].Value = totalEarned;

            cell["F20"].Value = employee.EnjoyedLeave;
            cell["F22"].Value = employee.EnjoyedSickLeave;
            cell["E15"].Value = employee.DateRetired.ToString("dd MMMM yyyy") + " " + remarks;

            //Processed by
            var processedby = oUnitOfWork.Employee.GetEmployeeById(processedBy);
            var processedByRank = oUnitOfWork.Rank.GetRankById(processedby.Emp_Curr_Rank ?? 0);
            cell["B40"].Value = processedByRank.Rank_Name + " " + processedby.Emp_FirstName + " " +
                                processedby.Emp_MiddleName + " " + processedby.Emp_LastName + " " +
                                processedby.Emp_SuffixName;

            cell["B41"].Value = oUnitOfWork.Employee.GetPositionTitle(processedBy);

            //Verified by
            var verifiedby = oUnitOfWork.Employee.GetEmployeeById(verifiedBy);
            var verifiedByRank = oUnitOfWork.Rank.GetRankById(verifiedby.Emp_Curr_Rank ?? 0);
            cell["E40"].Value = verifiedByRank.Rank_Name + " " + verifiedby.Emp_FirstName + " " +
                                verifiedby.Emp_MiddleName + " " + verifiedby.Emp_LastName + " " +
                                verifiedby.Emp_SuffixName;

            cell["E41"].Value = oUnitOfWork.Employee.GetPositionTitle(verifiedBy);

            //Certified by
            var certifiedby = oUnitOfWork.Employee.GetEmployeeById(certifiedBy);
            var certifiedByRank = oUnitOfWork.Rank.GetRankById(certifiedby.Emp_Curr_Rank ?? 0);
            cell["H40"].Value = certifiedByRank.Rank_Name + " " + certifiedby.Emp_FirstName + " " +
                                certifiedby.Emp_MiddleName + " " + certifiedby.Emp_LastName + " " +
                                certifiedby.Emp_SuffixName;

            cell["H41"].Value = oUnitOfWork.Employee.GetPositionTitle(certifiedBy);

            ef.Save(newFile);

            return Path.GetFileName(newFile);
        }

        public string PrintAgeProfile()
        {
            IHRISUnitOfWork oUnitOfWork = new HRISUnitOfWork(context);

            var ageProfile = oUnitOfWork.Report.GetAgeProfile();

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            var newFile =
                $"{applicationPath}{@"\Content\MISC\Generated\AgeProfile_"}{DateTime.Now.Ticks}{".xlsx"}";
            var template = $"{applicationPath}{@"\Content\MISC\Template\AgeProfile.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["Sheet1"];

            var cell = worksheet.Cells;
            cell["F5"].Value = "(as of " + DateTime.Now.ToString("MMMM yyyy") + ")";

            var row = 9;
            foreach (var item in ageProfile)
            {
                cell["B" + row].Value = item.RankName;
                cell["C" + row].Value = item.TwentyOneTwentyFive;
                cell["D" + row].Value = item.TwentySixThirty;
                cell["E" + row].Value = item.ThirtyOneThirtyFive;
                cell["F" + row].Value = item.ThirtySixFourty;
                cell["G" + row].Value = item.FourtyOneFourtyFive;
                cell["H" + row].Value = item.FourtySixFifty;
                cell["I" + row++].Value = item.FiftyOneFiftyFive;
            }

            ef.Save(newFile);

            return Path.GetFileName(newFile);
        }

        public string PrintJobFuntionDistribution()
        {
            IHRISUnitOfWork oUnitOfWork = new HRISUnitOfWork(context);
            var pesonnels = oUnitOfWork.Report.GetPersonnelPerRegion();

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            var newFile =
                $"{applicationPath}{@"\Content\MISC\Generated\JobFunctionDistribution_"}{DateTime.Now.Ticks}{".xlsx"}";
            var template = $"{applicationPath}{@"\Content\MISC\Template\JobFunctionDistribution.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["Sheet1"];

            var cell = worksheet.Cells;

            cell["A3"].Value =
                "Job Function Distribution of BFP Uniformed Personnel Based on the Actual Strenght Nationwide as of " +
                DateTime.Now.ToString("MMMM yyyy");

            var row = 7;
            foreach (var item in pesonnels)
            {
                foreach (var ch in "ABCDEFGHIJKLMNOPQRST")
                {
                    cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black,
                        LineStyle.Thin);
                }
                cell["A" + row].Value = item.Region;
                cell["B" + row].Value = item.DIR;
                cell["C" + row].Value = item.CSUPT;
                cell["D" + row].Value = item.SSUPT;
                cell["E" + row].Value = item.SUPT;
                cell["F" + row].Value = item.CINSP;
                cell["G" + row].Value = item.SINSP;
                cell["H" + row].Value = item.INSP;
                cell["I" + row].Formula = "=SUM(B" + row + ":H" + row + ")";

                cell["J" + row].Value = item.SFO4;
                cell["K" + row].Value = item.SFO3;
                cell["L" + row].Value = item.SFO2;
                cell["M" + row].Value = item.SFO1;
                cell["N" + row].Value = item.FO3;
                cell["O" + row].Value = item.FO2;
                cell["P" + row].Value = item.FO1;
                cell["Q" + row].Formula = "=SUM(J" + row + ":P" + row + ")";
                cell["R" + row].Value = item.GeneralAdmin;
                cell["S" + row].Value = item.Operations;
                cell["T" + row].Formula = "=SUM(I" + row + ",Q" + row + ")";
                row++;
            }

            cell["A" + row].Value = "TOTAL";

            foreach (var ch in "ABCDEFGHIJKLMNOPQRST")
            {
                cell[ch.ToString() + row].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                cell[ch.ToString() + row].Style.Font.Weight = ExcelFont.BoldWeight;
            }
            cell["B" + row].Formula = "=SUM(B7:B" + (row - 1) + ")";
            cell["C" + row].Formula = "=SUM(C7:C" + (row - 1) + ")";
            cell["D" + row].Formula = "=SUM(D7:D" + (row - 1) + ")";
            cell["E" + row].Formula = "=SUM(E7:E" + (row - 1) + ")";
            cell["F" + row].Formula = "=SUM(F7:F" + (row - 1) + ")";
            cell["G" + row].Formula = "=SUM(G7:G" + (row - 1) + ")";
            cell["H" + row].Formula = "=SUM(H7:H" + (row - 1) + ")";
            cell["I" + row].Formula = "=SUM(I7:I" + (row - 1) + ")";

            cell["J" + row].Formula = "=SUM(J7:J" + (row - 1) + ")";
            cell["K" + row].Formula = "=SUM(K7:K" + (row - 1) + ")";
            cell["L" + row].Formula = "=SUM(L7:L" + (row - 1) + ")";
            cell["M" + row].Formula = "=SUM(M7:M" + (row - 1) + ")";
            cell["N" + row].Formula = "=SUM(N7:N" + (row - 1) + ")";
            cell["O" + row].Formula = "=SUM(O7:O" + (row - 1) + ")";
            cell["P" + row].Formula = "=SUM(P7:P" + (row - 1) + ")";
            cell["Q" + row].Formula = "=SUM(Q7:Q" + (row - 1) + ")";
            cell["R" + row].Formula = "=SUM(R7:R" + (row - 1) + ")";
            cell["S" + row].Formula = "=SUM(S7:S" + (row - 1) + ")";
            cell["T" + row].Formula = "=SUM(I" + row + ",Q" + row + ")";

            ef.Save(newFile);
            return Path.GetFileName(newFile);
        }

        public string PrintRatioOfFemaleFighters()
        {
            IHRISUnitOfWork oUnitOfWork = new HRISUnitOfWork(context);

            var femaleRatioPerRankPerReg = oUnitOfWork.Report.GetRatioOfFemaleFireFighters();

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            var newFile =
                $"{applicationPath}{@"\Content\MISC\Generated\RatioOfFemaleFireFighters_"}{DateTime.Now.Ticks}{".xlsx"}";
            var template = $"{applicationPath}{@"\Content\MISC\Template\RatioOfFemaleFireFighters.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["Sheet1"];
            var cell = worksheet.Cells;

            cell["A4"].Value = "As of " + DateTime.Now.ToString("MMMM yyyy");

            var officersRow = 8;
            foreach (var item in femaleRatioPerRankPerReg)
            {
                cell["A" + officersRow].Value = item.Region;
                cell["B" + officersRow].Value = item.DIR_Male;
                cell["C" + officersRow].Value = item.DIR_Female;

                cell["E" + officersRow].Value = item.CSUPT_Male;
                cell["F" + officersRow].Value = item.CSUPT_Female;

                cell["H" + officersRow].Value = item.SSUPT_Male;
                cell["I" + officersRow].Value = item.SSUPT_Female;

                cell["K" + officersRow].Value = item.SUPT_Male;
                cell["L" + officersRow].Value = item.SUPT_Female;

                cell["N" + officersRow].Value = item.CINSP_Male;
                cell["O" + officersRow].Value = item.CINSP_Female;

                cell["Q" + officersRow].Value = item.SINSP_Male;
                cell["R" + officersRow].Value = item.SINSP_Female;

                cell["T" + officersRow].Value = item.INSP_Male;
                cell["U" + officersRow ++].Value = item.INSP_Female;
            }
            var NORRow = 36;
            foreach (var item in femaleRatioPerRankPerReg)
            {
                cell["A" + NORRow].Value = item.Region;
                cell["B" + NORRow].Value = item.SFO4_Male;
                cell["C" + NORRow].Value = item.SFO4_Female;

                cell["E" + NORRow].Value = item.SFO3_Male;
                cell["F" + NORRow].Value = item.SFO3_Female;

                cell["H" + NORRow].Value = item.SFO2_Male;
                cell["I" + NORRow].Value = item.SFO2_Female;

                cell["K" + NORRow].Value = item.SFO1_Male;
                cell["L" + NORRow].Value = item.SFO1_Female;

                cell["N" + NORRow].Value = item.FO3_Male;
                cell["O" + NORRow].Value = item.FO3_Female;

                cell["Q" + NORRow].Value = item.FO2_Male;
                cell["R" + NORRow].Value = item.FO2_Female;

                cell["T" + NORRow].Value = item.FO1_Male;
                cell["U" + NORRow++].Value = item.FO1_Female;
            }

            ef.Save(newFile);

            return Path.GetFileName(newFile);
        }

        public string PrintAlphaList()
        {
            IHRISUnitOfWork oUnitOfWork = new HRISUnitOfWork(context);

            var employeeList = oUnitOfWork.Employee.GetEmployees();

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            var newFile =
                $"{applicationPath}{@"\Content\MISC\Generated\AlphaList_"}{DateTime.Now.Ticks}{".xlsx"}";
            var template = $"{applicationPath}{@"\Content\MISC\Template\AlphaTemplate.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["Sheet1"];

            var cell = worksheet.Cells;

            var row = 2;
            var ranks = oUnitOfWork.Rank.GetList();
            var units = oUnitOfWork.Unit.GetList();
            foreach (var item in employeeList)
            {
                var rankName = "";
                if (item.Emp_Curr_Rank != null & item.Emp_Curr_Rank > 0)
                    rankName = ranks.FirstOrDefault(a => a.Rank_Id == item.Emp_Curr_Rank).Rank_Name;
                cell["A" + row].Value = rankName;

                cell["B" + row].Value = item.Emp_LastName;
                cell["C" + row].Value = item.Emp_FirstName;
                cell["D" + row].Value = item.Emp_MiddleName;
                cell["E" + row].Value = item.Emp_SuffixName;
                cell["F" + row].Value = item.Emp_Number;

                var unitCode = "";
                if (item.Emp_Curr_Unit != null & item.Emp_Curr_Unit > 0)
                    unitCode = units.FirstOrDefault(a => a.Unit_Id == item.Emp_Curr_Unit).Unit_Code;
                cell["G" + row].Value = unitCode;

                if (item.Emp_DES != null)
                    cell["H" + row].Value = item.Emp_DES.Value.ToString("dd-MMM-yyyy");
                if (item.Emp_BirthDate != null)
                    cell["I" + row].Value = item.Emp_BirthDate.Value.ToString("dd-MMM-yyyy");
                cell["J" + row].Value = item.Emp_TINNumber;
                cell["K" + row].Value = item.Emp_BP_Number;
                cell["L" + row].Value = item.Emp_PHICNumber;
                cell["M" + row].Value = item.Emp_PAGIBIGNumber;
                cell["N" + row].Value = item.Emp_Tax_Code;
                cell["O" + row++].Value = item.Emp_Atm_Number;
            }

            ef.Save(newFile);

            return Path.GetFileName(newFile);
        }

        public string DownloadAlphaTemplate(List<string> list)
        {
            var fieldList = list;

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            var newFile =
                $"{applicationPath}{@"\Content\MISC\Generated\Alpha_Template_"}{DateTime.Now.Ticks}{".xlsx"}";
            var template = $"{applicationPath}{@"\Content\MISC\Template\AlphaListTemplate.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["Sheet1"];

            var cell = worksheet.Cells;

            if (fieldList != null && fieldList.Count > 0)
            {
                var str = "";
                var char1 = 'A';
                var char2 = 'A';
                foreach (var item in fieldList)
                {
                    var column = Functions.GetNextColumn(char1.ToString());

                    if ((column == "AA" || str.Length == 2) && str != "AZ")
                    {
                        if (column != "AA" && !string.IsNullOrEmpty(str) && str[0].ToString() != "B")
                        {
                            var nextDouble = Functions.GetNextColumn(char2.ToString());
                            column = 'A' + nextDouble;
                            char2++;
                            str = column;
                        }
                        else if (string.IsNullOrEmpty(str))
                        {
                            str = "AA";
                        }
                    }
                    if ((str == "BA" || str.Length == 2) && str.Length == 2 && str[0].ToString() != "A" && str != "BZ")
                    {
                        if (str != "BA")
                        {
                            var nextDouble = Functions.GetNextColumn(char2.ToString());
                            column = 'B' + nextDouble;
                            char2++;

                            str = column;
                        }
                        else
                        {
                            column = "BA";
                        }
                    }
                    cell[column + "1"].Value = item;
                    cell[column + "1"].Style.VerticalAlignment = VerticalAlignmentStyle.Top;
                    cell[column + "1"].Style.Font.Name = "Arial";
                    cell[column + "1"].Style.Font.Size = 10*20;
                    if (str == "BA")
                    {
                        str = "BB";
                        char2 = 'A';
                    }
                    if (str == "AZ")
                    {
                        str = "BA";
                        char2 = 'A';
                    }
                    char1++;
                }
            }

            ef.Save(newFile);

            return Path.GetFileName(newFile);
        }

        public string ExportAlpha(List<EmployeeModel> employeeList, string fileName)
        {
            IHRISUnitOfWork oUnitOfWork = new HRISUnitOfWork(context);
            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            var newFile =
                $"{applicationPath}{@"\Content\MISC\Generated\AlphaList_"}{DateTime.Now.Ticks}{".xlsx"}";
            var template = $"{applicationPath}{@"\Content\MISC\Generated\" + fileName}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["Sheet1"];
            var cell = worksheet.Cells;
            var columns = GetIncludedColumn(cell);
            var row = 2;

            var ranks = new List<RankModel>();
            if (columns.HasEmp_Curr_Rank)
                ranks = oUnitOfWork.Rank.GetList();
            var units = oUnitOfWork.Unit.GetList();
            var religions = new List<ReligionModel>();
            if (columns.HasEmp_Religion)
                religions = oUnitOfWork.Religion.GetList();
            var salaryGrades = new List<SalaryGradeModel>();
            if (columns.HasEmp_Curr_SalaryGrade)
                salaryGrades = oUnitOfWork.SalaryGrade.GetList();
            var designations = new List<JobFuntionModel>();
            if (columns.HasEmp_Curr_JobFunc)
                designations = oUnitOfWork.JobFunction.GetList();
            var courses = new List<CourseModel>();
            if (columns.HasEmp_EducCourse || columns.HasEmp_MACourse)
                courses = oUnitOfWork.Course.GetList();
            var eligibilities = new List<EligibilityModel>();
            if (columns.HasEmp_Curr_Eligibility)
                eligibilities = oUnitOfWork.Eligibility.GetList();
            var trainings = new List<MandatoryTrainingModel>();
            if (columns.HasEmp_Curr_Eligibility)
                trainings = oUnitOfWork.MandatoryTraining.GetList();

            if (employeeList.Count > 0)
                foreach (var item in employeeList)
                {
                    cell["A" + row].Value = item.Emp_Number;
                    cell["B" + row].Value = item.Emp_LastName;
                    cell["C" + row].Value = item.Emp_FirstName;
                    cell["D" + row].Value = item.Emp_MiddleName;
                    cell["E" + row].Value = item.Emp_SuffixName;
                    var unitCode = "";
                    if (item.Emp_Curr_Unit != null & item.Emp_Curr_Unit > 0)
                        unitCode = units.FirstOrDefault(a => a.Unit_Id == item.Emp_Curr_Unit).Unit_Code;
                    cell["F" + row].Value = unitCode;
                    if (item.Emp_DES != null)
                        cell["G" + row].Value = item.Emp_DES.Value.ToString("dd-MMM-yyyy");
                    cell["H" + row].Value = item.Emp_BP_Number;
                    cell["I" + row].Value = item.Emp_Tax_Code;
                    cell["J" + row].Value = item.Emp_Atm_Number;
                    if (columns.HasEmp_BirthPlace)
                        cell[columns.Column_Emp_BirthPlace + row].Value = item.Emp_BirthPlace;
                    if (columns.HasEmp_BirthDate)
                    {
                        if (item.Emp_BirthDate != null)
                            cell[columns.Column_Emp_BirthDate + row].Value =
                                item.Emp_BirthDate.Value.ToString("dd-MMM-yyyy");
                    }
                    if (columns.HasEmp_CivilStatus)
                        cell[columns.Column_Emp_CivilStatus + row].Value = item.Emp_CivilStatus != null &&
                                                                           item.Emp_CivilStatus > 0
                            ? ((CivilStatus) item.Emp_CivilStatus).ToDescription()
                            : "";

                    if (columns.HasEmp_Citizenship)
                    {
                        cell[columns.Column_Emp_Citizenship + row].Value = item.Emp_Citizenship;
                        if (item.Emp_Citizenship == "Dual Citizenship")
                        {
                            cell[columns.Column_Emp_Bybirth_ByNaturalization + row].Value = item.Emp_Citizenship_Dual;
                            cell[columns.Column_Emp_Citizenship_Country + row].Value = item.Emp_Citizenship_Country;
                        }
                    }
                    if (columns.HasEmp_Gender)
                        cell[columns.Column_Emp_Gender + row].Value = item.Emp_Gender;
                    if (columns.HasEmp_Height)
                        cell[columns.Column_Emp_Height + row].Value = item.Emp_Height;
                    if (columns.HasEmp_Weight)
                        cell[columns.Column_Emp_Weight + row].Value = item.Emp_Weight;
                    if (columns.HasEmp_BloodType)
                        cell[columns.Column_Emp_BloodType + row].Value = item.Emp_BloodType;
                    if (columns.HasEmp_GSISNumber)
                        cell[columns.Column_Emp_GSISNumber + row].Value = item.Emp_GSISNumber;
                    if (columns.HasEmp_PAGIBIGNumber)
                        cell[columns.Column_Emp_PAGIBIGNumber + row].Value = item.Emp_PAGIBIGNumber;
                    if (columns.HasEmp_PHICNumber)
                        cell[columns.Column_Emp_PHICNumber + row].Value = item.Emp_PHICNumber;
                    if (columns.HasEmp_SSSNumber)
                        cell[columns.Column_Emp_SSSNumber + row].Value = item.Emp_SSSNumber;
                    if (columns.HasEmp_TINNumber)
                        cell[columns.Column_Emp_TINNumber + row].Value = item.Emp_TINNumber;
                    if (columns.HasEmp_Religion)
                    {
                        if (item.Emp_Religion != null & item.Emp_Religion > 0)
                        {
                            var religion = item.Emp_Religion == 29
                                ? ""
                                : religions.FirstOrDefault(a => a.Religion_Id == item.Emp_Religion).Religion_Name;
                            cell[columns.Column_Emp_Religion + row].Value = religion;
                            cell[columns.Column_Emp_OtherReligion + row].Value = item.Emp_Religion_Others;
                        }
                    }
                    if (columns.HasEmp_EmailAddress)
                        cell[columns.Column_Emp_EmailAddress + row].Value = item.Emp_EmailAddress;
                    if (columns.HasEmp_MobileNumber)
                        cell[columns.Column_Emp_MobileNumber + row].Value = item.Emp_MobileNumber;
                    if (columns.HasEmp_AgencyEmpNumber)
                        cell[columns.Column_Emp_AgencyEmpNumber + row].Value = item.Emp_AgencyEmpNumber;
                    if (columns.HasEmp_Residential_HouseNo)
                        cell[columns.Column_Emp_Residential_HouseNo + row].Value = item.Emp_Residential_HouseNo;
                    if (columns.HasEmp_Residential_Street)
                        cell[columns.Column_Emp_Residential_Street + row].Value = item.Emp_Residential_Street;
                    if (columns.HasEmp_Residential_Village)
                        cell[columns.Column_Emp_Residential_Village + row].Value = item.Emp_Residential_Village;
                    if (columns.HasEmp_Residential_Barangay)
                        cell[columns.Column_Emp_Residential_Barangay + row].Value = item.Emp_Residential_Barangay;
                    if (columns.HasEmp_Residential_Municipality)
                        cell[columns.Column_Emp_Residential_Municipality + row].Value = item.Emp_Residential_Municipality;
                    if (columns.HasEmp_Residential_Province)
                        cell[columns.Column_Emp_Residential_Province + row].Value = item.Emp_Residential_Province;
                    if (columns.HasEmp_Residential_ZipCode)
                        cell[columns.Column_Emp_Residential_ZipCode + row].Value = item.Emp_Residential_ZipCode;
                    if (columns.HasEmp_Residential_PhoneNumber)
                        cell[columns.Column_Emp_Residential_PhoneNumber + row].Value = item.Emp_Residential_PhoneNumber;
                    if (columns.HasEmp_Permanent_HouseNo)
                        cell[columns.Column_Emp_Permanent_HouseNo + row].Value = item.Emp_Permanent_HouseNo;
                    if (columns.HasEmp_Permanent_Street)
                        cell[columns.Column_Emp_Permanent_Street + row].Value = item.Emp_Permanent_Street;
                    if (columns.HasEmp_Permanent_Village)
                        cell[columns.Column_Emp_Permanent_Village + row].Value = item.Emp_Permanent_Village;
                    if (columns.HasEmp_Permanent_Barangay)
                        cell[columns.Column_Emp_Permanent_Barangay + row].Value = item.Emp_Permanent_Barangay;
                    if (columns.HasEmp_Permanent_Municipality)
                        cell[columns.Column_Emp_Permanent_Municipality + row].Value = item.Emp_Permanent_Municipality;
                    if (columns.HasEmp_Permanent_Province)
                        cell[columns.Column_Emp_Permanent_Province + row].Value = item.Emp_Permanent_Province;
                    if (columns.HasEmp_Permanent_ZipCode)
                        cell[columns.Column_Emp_Permanent_ZipCode + row].Value = item.Emp_Permanent_ZipCode;
                    if (columns.HasEmp_Permanent_PhoneNumber)
                        cell[columns.Column_Emp_Permanent_PhoneNumber + row].Value = item.Emp_Permanent_PhoneNumber;

                    //BFP Information
                    if (columns.HasEmp_Service_GovtStartDate)
                        cell[columns.Column_Emp_Service_GovtStartDate + row].Value =
                            item.Emp_Service_GovtStartDate?.ToString("dd-MMM-yyyy") ?? "";
                    if (columns.YearsGovtServiceStartDate)
                        cell[columns.Column_YearsGovtServiceStartDate + row].Value =
                            Functions.GetAge(item.Emp_Service_GovtStartDate);
                    if (columns.HasEmp_Service_UniformGovtStartDate)
                        cell[columns.Column_Emp_Service_UniformGovtStartDate + row].Value =
                            item.Emp_Service_UniformGovtStartDate?.ToString("dd-MMM-yyyy") ?? "";
                    if (columns.YearsUniformedFireService)
                        cell[columns.Column_YearsUniformedFireService + row].Value =
                            Functions.GetAge(item.Emp_Service_StartDate);
                    if (columns.HasEmp_Service_StartDate)
                        cell[columns.Column_Emp_Service_StartDate + row].Value =
                            item.Emp_Service_StartDate?.ToString("dd-MMM-yyyy") ?? "";
                    if (columns.MandatoryRetirementDate)
                        cell[columns.Column_MandatoryRetirementDate + row].Value =
                            Functions.GetRetirementDate(item.Emp_BirthDate, item.Emp_Curr_Rank)?.ToString("dd-MMM-yyyy") ??
                            "";
                    if (columns.UniformedOptionalRetirementDate)
                        cell[columns.Column_UniformedOptionalRetirementDate + row].Value =
                            Functions.GetOptionalRetirementDate(item.Emp_Service_GovtStartDate, 20)?
                                .ToString("dd-MMM-yyyy") ?? "";
                    if (columns.NonUniformedOptionalRetirementDate)
                        cell[columns.Column_NonUniformedOptionalRetirementDate + row].Value =
                            Functions.GetOptionalRetirementDate(item.Emp_Service_GovtStartDate, 15)?
                                .ToString("dd-MMM-yyyy") ?? "";
                    if (columns.HasEmp_LastPromotionDate_Temp)
                        cell[columns.Column_Emp_LastPromotionDate_Temp + row].Value = item.Emp_LastPromotionDate_Temp?.ToString("dd-MMM-yyyy") ?? "";
                    if (columns.HasEmp_LastPromotionDate_Permanent)
                        cell[columns.Column_Emp_LastPromotionDate_Permanent + row].Value = item.Emp_LastPromotionDate_Permanent?.ToString("dd-MMM-yyyy") ?? "";
                    if (columns.HasEmp_AssumedOfficerDate)
                        cell[columns.Column_Emp_AssumedOfficerDate + row].Value = item.Emp_AssumedOfficerDate?.ToString("dd-MMM-yyyy") ?? "";
                    if (columns.HasEmp_LastTrainingDate)
                        cell[columns.Column_Emp_LastTrainingDate + row].Value = item.Emp_LastTrainingDate?.ToString("dd-MMM-yyyy") ?? "";
                    if (columns.HasEmp_ItemNumber)
                        cell[columns.Column_Emp_ItemNumber + row].Value = item.Emp_ItemNumber;
                    if (columns.HasEmp_BadgeNumber)
                        cell[columns.Column_Emp_BadgeNumber + row].Value = item.Emp_BadgeNumber;
                    if (columns.HasEmp_Curr_Rank)
                    {
                        var rankName = "";
                        if (item.Emp_Curr_Rank != null & item.Emp_Curr_Rank > 0)
                            rankName = ranks.FirstOrDefault(a => a.Rank_Id == item.Emp_Curr_Rank).Rank_Name;
                        cell[columns.Column_Emp_Curr_Rank + row].Value = rankName;
                    }
                    if (columns.HasEmp_PresentAsgmt_DO_BO_RO)
                        cell[columns.Column_Emp_PresentAsgmt_DO_BO_RO + row].Value = item.Emp_PresentAsgmt_DO_BO_RO;
                    if (columns.HasEmp_Curr_ApptStatus)
                        cell[columns.Column_Emp_Curr_ApptStatus + row].Value = item.Emp_Curr_ApptStatus != null &&
                                                                               item.Emp_Curr_ApptStatus > 0
                            ? ((AppointmentStatuses) item.Emp_Curr_ApptStatus).ToDescription()
                            : "";
                    if (columns.HasEmp_AppointmentStatus_DO_BO_RO)
                        cell[columns.Column_Emp_AppointmentStatus_DO_BO_RO + row].Value = item.Emp_AppointmentStatus_DO_BO_RO;
                    if (columns.HasEmp_DutyStatus)
                        cell[columns.Column_Emp_DutyStatus + row].Value = item.Emp_DutyStatus != null &&
                                                                          item.Emp_DutyStatus > 0
                            ? ((DutyStatuses) item.Emp_DutyStatus).ToDescription()
                            : "";
                    if (columns.HasEmp_Curr_SalaryGrade)
                    {
                        var salaryGrade = "";
                        if (item.Emp_Curr_SalaryGrade != null & item.Emp_Curr_SalaryGrade > 0)
                            salaryGrade = salaryGrades.FirstOrDefault(a => a.SalaryGrade_Id == item.Emp_Curr_SalaryGrade)
                                    .SalaryGrade_Name;
                        cell[columns.Column_Emp_Curr_SalaryGrade + row].Value = salaryGrade;
                    }
                    if (columns.HasEmp_Curr_JobFunc)
                    {
                        if (item.Emp_Curr_JobFunc != null & item.Emp_Curr_JobFunc > 0)
                        {
                            var presentDesignation = designations.FirstOrDefault(a => a.JobFunc_Id == item.Emp_Curr_JobFunc);
                            cell[columns.Column_Emp_Curr_JobFunc + row].Value = string.Concat("[",
                                presentDesignation.JobFunc_Code, "] ", presentDesignation.JobFunc_Name);
                        }
                    }
                    //if (columns.HasEmp_Curr_PosDesignationTitle)
                    //    cell[columns.Column_Emp_Curr_PosDesignationTitle + row].Value = oUnitOfWork.Employee.GetPositionTitle(item.Emp_Id);
                    if (columns.HasEmp_Remarks)
                        cell[columns.Column_Emp_Remarks + row].Value = item.Emp_Remarks;
                    if (columns.HasEmp_EducCourse)
                    {
                        var baseCourse = "";
                        if (item.Emp_EducCourse != null & item.Emp_EducCourse > 0)
                            baseCourse =
                                courses.FirstOrDefault(a => a.Course_Id == item.Emp_EducCourse && a.Course_Category == (int) CourseType.Base)
                                    .Course_Name;
                        cell[columns.Column_Emp_EducCourse + row].Value = baseCourse;
                    }
                    if (columns.HasEmp_MACourse)
                    {
                        var graduateCourse = "";
                        if (item.Emp_MACourse != null & item.Emp_MACourse > 0)
                            graduateCourse =
                                courses.FirstOrDefault(a => a.Course_Id == item.Emp_MACourse &&
                                        a.Course_Category == (int) CourseType.Masteral).Course_Name;
                        cell[columns.Column_Emp_MACourse + row].Value = graduateCourse;
                    }
                    if (columns.HasEmp_HighestEducAttainment)
                        cell[columns.Column_Emp_HighestEducAttainment + row].Value = item.Emp_HighestEducAttainment !=
                                                                                     null &&
                                                                                     item.Emp_HighestEducAttainment > 0
                            ? ((EducAttaintmentLevel) item.Emp_HighestEducAttainment).ToDescription()
                            : "";
                    if (columns.HasEmp_Eligibility_Type)
                        cell[columns.Column_Emp_Eligibility_Type + row].Value = item.Emp_Eligibility_Type != null &&
                                                                                item.Emp_Eligibility_Type > 0
                            ? ((EligibilityType) item.Emp_Eligibility_Type).ToDescription()
                            : "";
                    if (columns.HasEmp_Curr_Eligibility)
                    {
                        var eligibility = "";
                        if (item.Emp_Curr_Eligibility != null & item.Emp_Curr_Eligibility > 0)
                            eligibility =
                                eligibilities.FirstOrDefault(a => a.Eligiblity_Id == item.Emp_Curr_Eligibility)
                                    .Eligibity_Name;
                        cell[columns.Column_Emp_Curr_Eligibility + row].Value = eligibility;
                    }
                    if (columns.HasEmp_HighestMandatoryTraining)
                    {
                        var mandatoryTraining = "";
                        if (item.Emp_HighestMandatoryTraining != null & item.Emp_HighestMandatoryTraining > 0)
                            mandatoryTraining =
                                trainings.FirstOrDefault(a => a.Training_Id == item.Emp_HighestMandatoryTraining)
                                    .Training_Name;
                        cell[columns.Column_Emp_HighestMandatoryTraining + row].Value = mandatoryTraining;
                    }
                    row++;
                }

            ef.Save(newFile);
            File.Delete(template);
            return Path.GetFileName(newFile);
        }

        private string GetExcelColumnName(int columnNumber)
        {
            var dividend = columnNumber;
            var columnName = string.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1)%26;
                columnName = Convert.ToChar(65 + modulo) + columnName;
                dividend = (dividend - modulo)/26;
            }

            return columnName;
        }

        public AlphaColumnListModel GetIncludedColumn(CellRange cell)
        {
            int startRow, startColumn;
            var model = new AlphaColumnListModel();
            if (cell.FindText("Date of birth", false, false, out startRow, out startColumn))
            {
                model.HasEmp_BirthDate = true;
                model.Column_Emp_BirthDate = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("Place of Birth", false, false, out startRow, out startColumn))
            {
                model.HasEmp_BirthPlace = true;
                model.Column_Emp_BirthPlace = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("Civil Status", false, false, out startRow, out startColumn))
            {
                model.HasEmp_CivilStatus = true;
                model.Column_Emp_CivilStatus = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("Citizenship", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Citizenship = true;
                model.Column_Emp_Citizenship = GetExcelColumnName(startColumn + 1);
                model.Column_Emp_Bybirth_ByNaturalization = GetExcelColumnName(startColumn + 2);
                model.Column_Emp_Citizenship_Country = GetExcelColumnName(startColumn + 3);
            }
            if (cell.FindText("Sex", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Gender = true;
                model.Column_Emp_Gender = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("Height", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Height = true;
                model.Column_Emp_Height = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("Weight", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Weight = true;
                model.Column_Emp_Weight = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("Blood Type", false, false, out startRow, out startColumn))
            {
                model.HasEmp_BloodType = true;
                model.Column_Emp_BloodType = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("GSIS ID NO", false, false, out startRow, out startColumn))
            {
                model.HasEmp_GSISNumber = true;
                model.Column_Emp_GSISNumber = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("PAG-IBIG ID NO", false, false, out startRow, out startColumn))
            {
                model.HasEmp_PAGIBIGNumber = true;
                model.Column_Emp_PAGIBIGNumber = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("PHILHEALTH NO", false, false, out startRow, out startColumn))
            {
                model.HasEmp_PHICNumber = true;
                model.Column_Emp_PHICNumber = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("SSS NO", false, false, out startRow, out startColumn))
            {
                model.HasEmp_SSSNumber = true;
                model.Column_Emp_SSSNumber = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("TIN", false, false, out startRow, out startColumn))
            {
                model.HasEmp_TINNumber = true;
                model.Column_Emp_TINNumber = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("Religion", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Religion = true;
                model.Column_Emp_Religion = GetExcelColumnName(startColumn + 1);
                model.Column_Emp_OtherReligion = GetExcelColumnName(startColumn + 2);
            }
            if (cell.FindText("E-MAIL ADDRESS", false, false, out startRow, out startColumn))
            {
                model.HasEmp_EmailAddress = true;
                model.Column_Emp_EmailAddress = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("CELLPHONE NO", false, false, out startRow, out startColumn))
            {
                model.HasEmp_MobileNumber = true;
                model.Column_Emp_MobileNumber = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("AGENCY EMPLOYEE NO", false, false, out startRow, out startColumn))
            {
                model.HasEmp_AgencyEmpNumber = true;
                model.Column_Emp_AgencyEmpNumber = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("RESIDENTIAL House/Block/Lot No", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Residential_HouseNo = true;
                model.Column_Emp_Residential_HouseNo = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("RESIDENTIAL Street", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Residential_Street = true;
                model.Column_Emp_Residential_Street = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("RESIDENTIAL Subdivision/Village", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Residential_Village = true;
                model.Column_Emp_Residential_Village = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("RESIDENTIAL Barangay", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Residential_Barangay = true;
                model.Column_Emp_Residential_Barangay = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("RESIDENTIAL City/Municipality", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Residential_Municipality = true;
                model.Column_Emp_Residential_Municipality = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("RESIDENTIAL Province", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Residential_Province = true;
                model.Column_Emp_Residential_Province = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("RESIDENTIAL Zip Code", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Residential_ZipCode = true;
                model.Column_Emp_Residential_ZipCode = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("RESIDENTIAL Telephone No", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Residential_PhoneNumber = true;
                model.Column_Emp_Residential_PhoneNumber = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("PERMANENT House/Block/Lot No.", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Permanent_HouseNo = true;
                model.Column_Emp_Permanent_HouseNo = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("PERMANENT Street", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Permanent_Street = true;
                model.Column_Emp_Permanent_Street = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("PERMANENT Subdivision/Village", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Permanent_Village = true;
                model.Column_Emp_Permanent_Village = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("PERMANENT Barangay", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Permanent_Barangay = true;
                model.Column_Emp_Permanent_Barangay = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("PERMANENT City/Municipality", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Permanent_Municipality = true;
                model.Column_Emp_Permanent_Municipality = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("PERMANENT PermanentProvince", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Permanent_Province = true;
                model.Column_Emp_Permanent_Province = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("PERMANENT Zip Code", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Permanent_ZipCode = true;
                model.Column_Emp_Permanent_ZipCode = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("PERMANENT Telephone No", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Permanent_PhoneNumber = true;
                model.Column_Emp_Permanent_PhoneNumber = GetExcelColumnName(startColumn + 1);
            }
            //BFP Information
            if (cell.FindText("DATE ENTERED GOVERNMENT SERVICE", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Service_GovtStartDate = true;
                model.Column_Emp_Service_GovtStartDate = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("YEARS IN GOVERNMENT SERVICE", false, false, out startRow, out startColumn))
            {
                model.YearsGovtServiceStartDate = true;
                model.Column_YearsGovtServiceStartDate = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("DATE ENTERED UNIFORMED SERVICE TO OTHER GOVERNMENT AGENCY/IES", false, false,
                out startRow, out startColumn))
            {
                model.HasEmp_Service_UniformGovtStartDate = true;
                model.Column_Emp_Service_UniformGovtStartDate = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("DATE ENTERED UNIFORMED FIRE SERVICE", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Service_StartDate = true;
                model.Column_Emp_Service_StartDate = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("YEARS IN UNIFORMED FIRE SERVICE", false, false, out startRow, out startColumn))
            {
                model.YearsUniformedFireService = true;
                model.Column_YearsUniformedFireService = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("MANDATORY RETIREMENT", false, false, out startRow, out startColumn))
            {
                model.MandatoryRetirementDate = true;
                model.Column_MandatoryRetirementDate = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("UNIFORMED OPTIONAL RETIREMENT", false, false, out startRow, out startColumn))
            {
                model.UniformedOptionalRetirementDate = true;
                model.Column_UniformedOptionalRetirementDate = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("NON UNIFORMED SERVICE OPTIONAL RETIREMENT", false, false, out startRow, out startColumn))
            {
                model.NonUniformedOptionalRetirementDate = true;
                model.Column_NonUniformedOptionalRetirementDate = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("DATE OF LAST PROMOTION (TEMPORARY STATUS)", false, false, out startRow, out startColumn))
            {
                model.HasEmp_LastPromotionDate_Temp = true;
                model.Column_Emp_LastPromotionDate_Temp = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("DATE OF LAST PROMOTION (PERMANENT STATUS)", false, false, out startRow, out startColumn))
            {
                model.HasEmp_LastPromotionDate_Permanent = true;
                model.Column_Emp_LastPromotionDate_Permanent = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("DATE ASSUMED OFFICER", false, false, out startRow, out startColumn))
            {
                model.HasEmp_AssumedOfficerDate = true;
                model.Column_Emp_AssumedOfficerDate = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("LAST TRAINING DATE", false, false, out startRow, out startColumn))
            {
                model.HasEmp_LastTrainingDate = true;
                model.Column_Emp_LastTrainingDate = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("ITEM NUMBER", false, false, out startRow, out startColumn))
            {
                model.HasEmp_ItemNumber = true;
                model.Column_Emp_ItemNumber = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("BADGE NUMBER", false, false, out startRow, out startColumn))
            {
                model.HasEmp_BadgeNumber = true;
                model.Column_Emp_BadgeNumber = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("PRESENT RANK", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Curr_Rank = true;
                model.Column_Emp_Curr_Rank = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("PRESENT ASSIGNMENT DO/BO/RO", false, false, out startRow, out startColumn))
            {
                model.HasEmp_PresentAsgmt_DO_BO_RO = true;
                model.Column_Emp_PresentAsgmt_DO_BO_RO = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("APPOINTMENT STATUS", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Curr_ApptStatus = true;
                model.Column_Emp_Curr_ApptStatus = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("Appointment Status DO/BO/RO", false, false, out startRow, out startColumn))
            {
                model.HasEmp_AppointmentStatus_DO_BO_RO = true;
                model.Column_Emp_AppointmentStatus_DO_BO_RO = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("DUTY STATUS", false, false, out startRow, out startColumn))
            {
                model.HasEmp_DutyStatus = true;
                model.Column_Emp_DutyStatus = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("SALARY GRADE", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Curr_SalaryGrade = true;
                model.Column_Emp_Curr_SalaryGrade = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("PRESENT DESIGNATION", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Curr_JobFunc = true;
                model.Column_Emp_Curr_JobFunc = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("SPECIFY DESIGNATION", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Curr_PosDesignationTitle = true;
                model.Column_Emp_Curr_PosDesignationTitle = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("REMARKS", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Remarks = true;
                model.Column_Emp_Remarks = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("BASE COURSE", false, false, out startRow, out startColumn))
            {
                model.HasEmp_EducCourse = true;
                model.Column_Emp_EducCourse = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("GRADUATE STUDIES", false, false, out startRow, out startColumn))
            {
                model.HasEmp_MACourse = true;
                model.Column_Emp_MACourse = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("HIGHEST EDUCATIONAL ATTAINMENT", false, false, out startRow, out startColumn))
            {
                model.HasEmp_HighestEducAttainment = true;
                model.Column_Emp_HighestEducAttainment = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("ELIGIBITY TYPE", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Eligibility_Type = true;
                model.Column_Emp_Eligibility_Type = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("HIGHEST ELIGIBILITY", false, false, out startRow, out startColumn))
            {
                model.HasEmp_Curr_Eligibility = true;
                model.Column_Emp_Curr_Eligibility = GetExcelColumnName(startColumn + 1);
            }
            if (cell.FindText("HIGHEST MANDATORY TRAINING", false, false, out startRow, out startColumn))
            {
                model.HasEmp_HighestMandatoryTraining = true;
                model.Column_Emp_HighestMandatoryTraining = GetExcelColumnName(startColumn + 1);
            }
            return model;
        }

        public string PrintRetiringPersonnelReport(string type, int? month)
        {
            IHRISUnitOfWork oUnitOfWork = new HRISUnitOfWork(context);

            var pesonnels = oUnitOfWork.Report.GetRetiringPersonnelPerYear(type, month);

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            var newFile =
                $"{applicationPath}{@"\Content\MISC\Generated\RetiringPersonnel_"}{DateTime.Now.Ticks}{".xlsx"}";
            var template = $"{applicationPath}{@"\Content\MISC\Template\RetiringPersonnelTemplate.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["Sheet1"];

            var cell = worksheet.Cells;
            var row = 2;

            if(pesonnels.Count > 0)
            foreach (var item in pesonnels.OrderBy(a=> a.RetirementDate))
            {
                cell["A" + row].Value = item.AccountNumber;
                cell["B" + row].Value = item.Rank;
                cell["C" + row].Value = item.FirstName;
                cell["D" + row].Value = item.MiddleName;
                cell["E" + row].Value = item.LastName;
                cell["F" + row].Value = item.SuffixName;
                cell["G" + row++].Value = item.RetirementDate.ToString("dd MMMM yyyy");
            }

            ef.Save(newFile);
            return Path.GetFileName(newFile);
        }

        public string PrintPersonnelPerRegionReport(int regionId)
        {
            IHRISUnitOfWork oUnitOfWork = new HRISUnitOfWork(context);
            var pesonnels = oUnitOfWork.Report.GetPersonnelNumberPerRegion(regionId);

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            var newFile =
                $"{applicationPath}{@"\Content\MISC\Generated\PersonnelPerRegion_"}{DateTime.Now.Ticks}{".xlsx"}";
            var template = $"{applicationPath}{@"\Content\MISC\Template\PersonnelPerRegion.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["Sheet1"];

            var cell = worksheet.Cells;

            cell["A5"].Value = "as of " + DateTime.Now.ToString("dd MMMM yyyy");

            var row = 7;
        
                cell["A" + row].Value = pesonnels.Region.Replace("Region ", "");
                cell["B" + row].Value = pesonnels.DIR;
                cell["C" + row].Value = pesonnels.CSUPT;
                cell["D" + row].Value = pesonnels.SSUPT;
                cell["E" + row].Value = pesonnels.SUPT;
                cell["F" + row].Value = pesonnels.CINSP;
                cell["G" + row].Value = pesonnels.SINSP;
                cell["H" + row].Value = pesonnels.INSP;
                cell["I" + row].Formula = "=SUM(B" + row + ":H" + row + ")";

                cell["J" + row].Value = pesonnels.SFO4;
                cell["K" + row].Value = pesonnels.SFO3;
                cell["L" + row].Value = pesonnels.SFO2;
                cell["M" + row].Value = pesonnels.SFO1;
                cell["N" + row].Value = pesonnels.FO3;
                cell["O" + row].Value = pesonnels.FO2;
                cell["P" + row].Value = pesonnels.FO1;
                cell["Q" + row].Formula = "=SUM(J" + row + ":P" + row + ")";
                cell["R" + row].Formula = "=SUM(I" + row + ",Q" + row + ")";
                cell["S" + row].Value = pesonnels.NUP;
                cell["T" + row].Formula = "=SUM(R" + row + ",S" + row + ")";

            ef.Save(newFile);
            return Path.GetFileName(newFile);
        }

        public string PrintPersonnelPerProvinceReport(int provinceId)
        {
            IHRISUnitOfWork oUnitOfWork = new HRISUnitOfWork(context);
            var pesonnels = oUnitOfWork.Report.GetPersonnelNumberPerProvince(provinceId);

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            var newFile =
                $"{applicationPath}{@"\Content\MISC\Generated\PersonnelPerProvince_"}{DateTime.Now.Ticks}{".xlsx"}";
            var template = $"{applicationPath}{@"\Content\MISC\Template\PersonnelPerProvince.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["Sheet1"];

            var cell = worksheet.Cells;

            cell["A5"].Value = "as of " + DateTime.Now.ToString("dd MMMM yyyy");

            var row = 7;

            cell["A" + row].Value = pesonnels.Province;
            cell["B" + row].Value = pesonnels.DIR;
            cell["C" + row].Value = pesonnels.CSUPT;
            cell["D" + row].Value = pesonnels.SSUPT;
            cell["E" + row].Value = pesonnels.SUPT;
            cell["F" + row].Value = pesonnels.CINSP;
            cell["G" + row].Value = pesonnels.SINSP;
            cell["H" + row].Value = pesonnels.INSP;
            cell["I" + row].Formula = "=SUM(B" + row + ":H" + row + ")";

            cell["J" + row].Value = pesonnels.SFO4;
            cell["K" + row].Value = pesonnels.SFO3;
            cell["L" + row].Value = pesonnels.SFO2;
            cell["M" + row].Value = pesonnels.SFO1;
            cell["N" + row].Value = pesonnels.FO3;
            cell["O" + row].Value = pesonnels.FO2;
            cell["P" + row].Value = pesonnels.FO1;
            cell["Q" + row].Formula = "=SUM(J" + row + ":P" + row + ")";
            cell["R" + row].Formula = "=SUM(I" + row + ",Q" + row + ")";
            cell["S" + row].Value = pesonnels.NUP;
            cell["T" + row].Formula = "=SUM(R" + row + ",S" + row + ")";

            ef.Save(newFile);
            return Path.GetFileName(newFile);
        }
        public string PrintPersonnelPerStationReport(int station)
        {
            IHRISUnitOfWork oUnitOfWork = new HRISUnitOfWork(context);
            var pesonnels = oUnitOfWork.Report.GetPersonnelNumberPerStation(station);

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            var newFile =
                $"{applicationPath}{@"\Content\MISC\Generated\PersonnelPerStation_"}{DateTime.Now.Ticks}{".xlsx"}";
            var template = $"{applicationPath}{@"\Content\MISC\Template\PersonnelPerStation.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheet = ef.Worksheets["Sheet1"];

            var cell = worksheet.Cells;

            cell["A5"].Value = "as of " + DateTime.Now.ToString("dd MMMM yyyy");

            var row = 7;

            cell["A" + row].Value = pesonnels.Station;
            cell["B" + row].Value = pesonnels.DIR;
            cell["C" + row].Value = pesonnels.CSUPT;
            cell["D" + row].Value = pesonnels.SSUPT;
            cell["E" + row].Value = pesonnels.SUPT;
            cell["F" + row].Value = pesonnels.CINSP;
            cell["G" + row].Value = pesonnels.SINSP;
            cell["H" + row].Value = pesonnels.INSP;
            cell["I" + row].Formula = "=SUM(B" + row + ":H" + row + ")";

            cell["J" + row].Value = pesonnels.SFO4;
            cell["K" + row].Value = pesonnels.SFO3;
            cell["L" + row].Value = pesonnels.SFO2;
            cell["M" + row].Value = pesonnels.SFO1;
            cell["N" + row].Value = pesonnels.FO3;
            cell["O" + row].Value = pesonnels.FO2;
            cell["P" + row].Value = pesonnels.FO1;
            cell["Q" + row].Formula = "=SUM(J" + row + ":P" + row + ")";
            cell["R" + row].Formula = "=SUM(I" + row + ",Q" + row + ")";
            cell["S" + row].Value = pesonnels.NUP;
            cell["T" + row].Formula = "=SUM(R" + row + ",S" + row + ")";

            ef.Save(newFile);
            return Path.GetFileName(newFile);
        }
    }

    public interface IHRISReport
    {
        string PrintLeaveRecord(int employeeId, int processedBy, int preparedBy, int certifiedBy, DateTime endDate);
        string PrintRegionalAuthorizeStrReport();
        string PrintPersonnelStrengthReport(int preparedBy, int reviewedBy, int notedBy);
        string PrintActualVsAuthorizedReport(int preparedBy, int reviewedBy, int notedBy);
        string PrintServiceRecord(int employeeId, int preparedBy, int verifiedBy, int certifiedBy, string remarks);
        string PrintLongevityPay(int official1, int official2, DateTime startDate, DateTime endDate);

        string PrintCommLeaveCredits(int retiredEmployee, int processedBy, int verifiedBy, int certifiedBy,
            string remarks);

        string PrintAgeProfile();
        string PrintJobFuntionDistribution();
        string PrintRatioOfFemaleFighters();
        string PrintAlphaList();
        string DownloadAlphaTemplate(List<string> list);
        string ExportAlpha(List<EmployeeModel> employeeList, string filename);
        string PrintRetiringPersonnelReport(string type, int? month);
        string PrintPersonnelPerRegionReport(int regionId);
        string PrintPersonnelPerProvinceReport(int provinceId);
        string PrintPersonnelPerStationReport(int station);
    }
}