namespace EBFP.BL.Helper
{
    using DataAccess;
    using EBFP.BL.HumanResources;
    using GemBox.Spreadsheet;
    using System;
    using System.Web;
    public class PDSExport : EntityFrameworkBase , IPDSExport
    {
        public PDSExport(EBFPEntities _context)
        {
            context_ = _context; 
        }

        public void PrintPDS(int employeeID)
        {
            IHRISUnitOfWork oUnitOfWork = new HRISUnitOfWork(context);
            var employeeModel = oUnitOfWork.Employee.EmployeeDetails(employeeID);

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            var newFile =
                $"{applicationPath}{@"\Content\MISC\Generated\unlocked_pds2005_"}{DateTime.Now.Ticks}{".xls"}";
            var template = $"{applicationPath}{@"\Content\MISC\Template\PDS_Template.xls"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheetC1 = ef.Worksheets["C1"];
            var worksheetC1_2nd = ef.Worksheets["C1-1"];
            var worksheetC2 = ef.Worksheets["C2"];
            var worksheetC2_2nd = ef.Worksheets["C2-1"];
            var worksheetC2_3rd = ef.Worksheets["C2-2"];
            var worksheetC3 = ef.Worksheets["C3"];
            var worksheetC3_2nd = ef.Worksheets["C3-1"];
            var worksheetC3_3rd = ef.Worksheets["C3-2"];
            var worksheetC4 = ef.Worksheets["C4"];

            var cellC1 = worksheetC1.Cells;
            var cellC1_2nd = worksheetC1_2nd.Cells;
            var cellC2 = worksheetC2.Cells;
            var cellC2_2nd = worksheetC2_2nd.Cells;
            var cellC2_3rd = worksheetC2_3rd.Cells;
            var cellC3 = worksheetC3.Cells;
            var cellC3_2nd = worksheetC3_2nd.Cells;
            var cellC3_3rd = worksheetC3_3rd.Cells;
            var cellC4 = worksheetC4.Cells;

            #region Sheet 1
            // Personal Information
            cellC1["D10"].Value = employeeModel.Emp_LastName?.ToUpper();
            cellC1["D11"].Value = employeeModel.Emp_FirstName?.ToUpper();
            cellC1["D13"].Value = employeeModel.Emp_MiddleName?.ToUpper();
            cellC1["T12"].Value = employeeModel.Emp_SuffixName?.ToUpper();
            cellC1["D14"].Value = employeeModel.Emp_BirthDate;
            cellC1["D19"].Value = employeeModel.Emp_BirthPlace?.ToUpper();

            //Gender
            if (employeeModel.Emp_Gender == "F")
                cellC1["H21"].Value = "X";
            else
                cellC1["E21"].Value = "X";

            //Citizenship
            if (employeeModel.Emp_Citizenship == "Filipino")
                cellC1["O15"].Value = "X";
            else
            {
                //Dual
                cellC1["R15"].Value = "X";
                if (employeeModel.Emp_Citizenship_Dual == "By Birth")
                    cellC1["R17"].Value = "X";
                else
                    cellC1["T17"].Value = "X";
                //Country
                cellC1["R20"].Value = employeeModel.Emp_Citizenship_Country?.ToUpper();
            }

            //Civil Status
            cellC1["E24"].Value = employeeModel.Emp_CivilStatus == (int)CivilStatus.Single ? "X" : "";
            cellC1["E26"].Value = employeeModel.Emp_CivilStatus == (int)CivilStatus.Widowed ? "X" : "";
            cellC1["E28"].Value = employeeModel.Emp_CivilStatus == (int)CivilStatus.Others ? "X" : "";
            cellC1["H24"].Value = employeeModel.Emp_CivilStatus == (int)CivilStatus.Married ? "X" : "";
            cellC1["H26"].Value = employeeModel.Emp_CivilStatus == (int)CivilStatus.Separated ? "X" : "";
            if (employeeModel.Emp_CivilStatus == (int)CivilStatus.Others)
                cellC1["G28"].Value = employeeModel.Emp_CivilStatus_Other?.ToUpper();

            cellC1["D30"].Value = employeeModel.Emp_Height?.ToUpper();
            cellC1["D32"].Value = employeeModel.Emp_Weight?.ToUpper();
            cellC1["D33"].Value = employeeModel.Emp_BloodType?.ToUpper();
            cellC1["D35"].Value = employeeModel.Emp_GSISNumber?.ToUpper();
            cellC1["D37"].Value = employeeModel.Emp_PAGIBIGNumber?.ToUpper();
            cellC1["D39"].Value = employeeModel.Emp_PHICNumber?.ToUpper();
            cellC1["D40"].Value = employeeModel.Emp_SSSNumber?.ToUpper();
            cellC1["D41"].Value = employeeModel.Emp_TINNumber?.ToUpper();
            cellC1["D42"].Value = employeeModel.Emp_AgencyEmpNumber?.ToUpper();

            //Residential Address
            cellC1["M23"].Value = employeeModel.Emp_Residential_HouseNo?.ToUpper();
            cellC1["S23"].Value = employeeModel.Emp_Residential_Street?.ToUpper();
            cellC1["M26"].Value = employeeModel.Emp_Residential_Village?.ToUpper();
            cellC1["S26"].Value = employeeModel.Emp_Residential_Barangay?.ToUpper();
            cellC1["M30"].Value = employeeModel.Emp_Residential_Municipality?.ToUpper();
            cellC1["S30"].Value = employeeModel.Emp_Residential_Province?.ToUpper();
            cellC1["M32"].Value = employeeModel.Emp_Residential_ZipCode?.ToUpper();

            //Permanent Address
            cellC1["M33"].Value = employeeModel.Emp_Permanent_HouseNo?.ToUpper();
            cellC1["S33"].Value = employeeModel.Emp_Permanent_Street?.ToUpper();
            cellC1["M35"].Value = employeeModel.Emp_Permanent_Village?.ToUpper();
            cellC1["S35"].Value = employeeModel.Emp_Permanent_Barangay?.ToUpper();
            cellC1["M37"].Value = employeeModel.Emp_Permanent_Municipality?.ToUpper();
            cellC1["S37"].Value = employeeModel.Emp_Permanent_Province?.ToUpper();
            cellC1["M39"].Value = employeeModel.Emp_Permanent_ZipCode?.ToUpper();

            cellC1["M40"].Value = employeeModel.Emp_Residential_PhoneNumber?.ToUpper();
            cellC1["M41"].Value = employeeModel.Emp_MobileNumber?.ToUpper();
            cellC1["M42"].Value = employeeModel.Emp_EmailAddress?.ToUpper();

            //Family Background
            cellC1["D44"].Value = employeeModel.Emp_Spouse_LastName?.ToUpper();
            cellC1["D45"].Value = employeeModel.Emp_Spouse_FirstName?.ToUpper();
            cellC1["D47"].Value = employeeModel.Emp_Spouse_MiddleName?.ToUpper();
            cellC1["L46"].Value = employeeModel.Emp_Spouse_SuffixName?.ToUpper();

            cellC1["D48"].Value = employeeModel.Emp_Spouse_Occupation?.ToUpper();
            cellC1["D49"].Value = employeeModel.Emp_Spouse_EmpBusName?.ToUpper();
            cellC1["D50"].Value = employeeModel.Emp_Spouse_EmpBusAddress?.ToUpper();
            cellC1["D51"].Value = employeeModel.Emp_Spouse_EmpBusPhoneNumber?.ToUpper();

            cellC1["D52"].Value = employeeModel.Emp_Father_LastName?.ToUpper();
            cellC1["D53"].Value = employeeModel.Emp_Father_FirstName?.ToUpper();
            cellC1["D55"].Value = employeeModel.Emp_Father_MiddleName?.ToUpper();
            cellC1["L54"].Value = employeeModel.Emp_Father_SuffixName?.ToUpper();
            cellC1["D56"].Value = employeeModel.Emp_Mother_MaidenName?.ToUpper();
            cellC1["D57"].Value = employeeModel.Emp_Mother_LastName?.ToUpper();
            cellC1["D58"].Value = employeeModel.Emp_Mother_FirstName?.ToUpper();
            cellC1["D59"].Value = employeeModel.Emp_Mother_MiddleName?.ToUpper();

            var childCount = 45;
            var childCount2 = 45;
            foreach (var item in employeeModel.EmployeeChildren)
            {
                if (childCount < 59)
                {
                    childCount = (childCount == 46) ? (childCount + 1) : childCount == 54 ? (childCount + 1) : childCount;
                    cellC1["M" + childCount].Value = item.EC_FullName?.ToUpper();
                    cellC1["T" + childCount++].Value = item.EC_BirthDate;
                }
                else if (childCount2 < 59)
                {
                    childCount2 = (childCount2 == 46) ? (childCount2 + 1) : childCount2 == 54 ? (childCount2 + 1) : childCount2;
                    cellC1_2nd["M" + childCount2].Value = item.EC_FullName?.ToUpper();
                    cellC1_2nd["T" + childCount2++].Value = item.EC_BirthDate;

                    childCount++;
                }
            }

            //Educational Background
            var elemCount = 0;
            var secCount = 0;
            var vocCount = 0;
            var colCount = 0;
            var gradCount = 0;
            var educBackSecondPage = false;
            foreach (var item in employeeModel.EducationalBackgrounds)
            {
                var eeb_row = item.EEB_EducType == (int)EducStatus.Elementary ? 64 :
                             item.EEB_EducType == (int)EducStatus.Secondary ? 65 :
                             item.EEB_EducType == (int)EducStatus.Vocational ? 66 :
                             item.EEB_EducType == (int)EducStatus.College ? 67 : 68;

                if (eeb_row == 64)
                    elemCount++;
                else if (eeb_row == 65)
                    secCount++;
                else if (eeb_row == 66)
                    vocCount++;
                else if (eeb_row == 67)
                    colCount++;
                else if (eeb_row == 68)
                    gradCount++;

                if (elemCount == 1 || secCount == 1 || vocCount == 1 || colCount == 1 || gradCount == 1)
                {
                    cellC1["D" + eeb_row].Value = item.EEB_SchoolName?.ToUpper();

                    if (item.EEB_EducType == (int)EducStatus.College || item.EEB_EducType == (int)EducStatus.Graduate)
                        cellC1["K" + eeb_row].Value = oUnitOfWork.Course.GetCourseName(Convert.ToInt32(item.EEB_DegreeCourse))?.ToUpper();
                    else
                        cellC1["K" + eeb_row].Value = item.EEB_DegreeCourse?.ToUpper();

                    cellC1["S" + eeb_row].Value = item.EEB_HighestLevel?.ToUpper();
                    cellC1["N" + eeb_row].Value = item.EEB_StartDate?.ToUpper();
                    cellC1["Q" + eeb_row].Value = item.EEB_EndDate?.ToUpper();
                    cellC1["U" + eeb_row].Value = item.EEB_GraduateYear?.ToUpper();
                    cellC1["V" + eeb_row].Value = item.EEB_Awards?.ToUpper();
                }
                else if (elemCount == 2 || secCount == 2 || vocCount == 2 || colCount == 2 || gradCount == 2)
                {
                    educBackSecondPage = true;
                    cellC1_2nd["D" + eeb_row].Value = item.EEB_SchoolName?.ToUpper();

                    if (item.EEB_EducType == (int)EducStatus.College || item.EEB_EducType == (int)EducStatus.Graduate)
                        cellC1_2nd["K" + eeb_row].Value = oUnitOfWork.Course.GetCourseName(Convert.ToInt32(item.EEB_DegreeCourse))?.ToUpper();
                    else
                        cellC1_2nd["K" + eeb_row].Value = item.EEB_DegreeCourse?.ToUpper();

                    cellC1_2nd["S" + eeb_row].Value = item.EEB_HighestLevel?.ToUpper();
                    cellC1_2nd["N" + eeb_row].Value = item.EEB_StartDate?.ToUpper();
                    cellC1_2nd["Q" + eeb_row].Value = item.EEB_EndDate?.ToUpper();
                    cellC1_2nd["U" + eeb_row].Value = item.EEB_GraduateYear?.ToUpper();
                    cellC1_2nd["V" + eeb_row].Value = item.EEB_Awards?.ToUpper();
                }            
            }

            if (educBackSecondPage == false && childCount < 59)
                ef.Worksheets["C1-1"].Delete();
            #endregion

            #region Sheet 2
            //Civil Service eligibility
            var civilServiceCount = 5;
            var civilServiceCount2 = 5;
            var civilServiceCount3 = 5;
            foreach (var item in employeeModel.CivilServiceEligibilities)
            {
                if (civilServiceCount < 12)
                {
                    cellC2["A" + civilServiceCount].Value =
                        oUnitOfWork.Eligibility.GetEligibilities(Convert.ToInt32(item.ECSE_Title))?.ToUpper();
                    cellC2["F" + civilServiceCount].Value = item.ECSE_Rating?.ToUpper();
                    cellC2["G" + civilServiceCount].Value = item.ECSE_ExamDate;
                    cellC2["I" + civilServiceCount].Value = item.ECSE_ExamPlace?.ToUpper();
                    cellC2["L" + civilServiceCount].Value = item.ECSE_LicNumber?.ToUpper();
                    cellC2["M" + civilServiceCount++].Value = item.ECSE_LicReleaseDate;
                }
                else if (civilServiceCount2 < 12)
                {
                    cellC2_2nd["A" + civilServiceCount2].Value =
                           oUnitOfWork.Eligibility.GetEligibilities(Convert.ToInt32(item.ECSE_Title))?.ToUpper();
                    cellC2_2nd["F" + civilServiceCount2].Value = item.ECSE_Rating?.ToUpper();
                    cellC2_2nd["G" + civilServiceCount2].Value = item.ECSE_ExamDate;
                    cellC2_2nd["I" + civilServiceCount2].Value = item.ECSE_ExamPlace?.ToUpper();
                    cellC2_2nd["L" + civilServiceCount2].Value = item.ECSE_LicNumber?.ToUpper();
                    cellC2_2nd["M" + civilServiceCount2++].Value = item.ECSE_LicReleaseDate;

                    civilServiceCount++;
                }
                else if (civilServiceCount3 < 12)
                {
                    cellC2_3rd["A" + civilServiceCount3].Value =
                           oUnitOfWork.Eligibility.GetEligibilities(Convert.ToInt32(item.ECSE_Title))?.ToUpper();
                    cellC2_3rd["F" + civilServiceCount3].Value = item.ECSE_Rating?.ToUpper();
                    cellC2_3rd["G" + civilServiceCount3].Value = item.ECSE_ExamDate;
                    cellC2_3rd["I" + civilServiceCount3].Value = item.ECSE_ExamPlace?.ToUpper();
                    cellC2_3rd["L" + civilServiceCount3].Value = item.ECSE_LicNumber?.ToUpper();
                    cellC2_3rd["M" + civilServiceCount3++].Value = item.ECSE_LicReleaseDate;

                    civilServiceCount++;
                    civilServiceCount2++;
                }
            }
            //WORK EXPERIENCE
            var workExperienceCount = 18;
            var workExperienceCount2 = 18;
            var workExperienceCount3 = 18;
            foreach (var item in employeeModel.WorkExperiences)
            {
                if (workExperienceCount < 46)
                {
                    cellC2["A" + workExperienceCount].Value = item.EWE_StartDate;
                    if (item.EWE_EndDate == null)
                        cellC2["C" + workExperienceCount].Value = "PRESENT";
                    else
                        cellC2["C" + workExperienceCount].Value = item.EWE_EndDate;

                    cellC2["D" + workExperienceCount].Value = item.EWE_PositionTitle?.ToUpper();
                    cellC2["G" + workExperienceCount].Value = item.EWE_CompanyName?.ToUpper();
                    cellC2["J" + workExperienceCount].Value = item.EWE_MonthlySalary?.ToUpper();
                    cellC2["K" + workExperienceCount].Value = item.EWE_SalaryGrade?.ToUpper();
                    cellC2["L" + workExperienceCount].Value = item.EWE_ApptStatus?.ToUpper();
                    cellC2["M" + workExperienceCount++].Value = item.EWE_GovtService ? "YES" : "NO";
                }
                else if (workExperienceCount2 < 46)
                {               
                    cellC2_2nd["A" + workExperienceCount2].Value = item.EWE_StartDate;
                    if (item.EWE_EndDate == null)
                        cellC2_2nd["C" + workExperienceCount2].Value = "PRESENT";
                    else
                        cellC2_2nd["C" + workExperienceCount2].Value = item.EWE_EndDate;

                    cellC2_2nd["D" + workExperienceCount2].Value = item.EWE_PositionTitle?.ToUpper();
                    cellC2_2nd["G" + workExperienceCount2].Value = item.EWE_CompanyName?.ToUpper();
                    cellC2_2nd["J" + workExperienceCount2].Value = item.EWE_MonthlySalary?.ToUpper();
                    cellC2_2nd["K" + workExperienceCount2].Value = item.EWE_SalaryGrade?.ToUpper();
                    cellC2_2nd["L" + workExperienceCount2].Value = item.EWE_ApptStatus?.ToUpper();
                    cellC2_2nd["M" + workExperienceCount2++].Value = item.EWE_GovtService ? "YES" : "NO";
                    workExperienceCount++;
                }
                else if (workExperienceCount3 < 46)
                {
                    cellC2_3rd["A" + workExperienceCount3].Value = item.EWE_StartDate;
                    if (item.EWE_EndDate == null)
                        cellC2_3rd["C" + workExperienceCount3].Value = "PRESENT";
                    else
                        cellC2_3rd["C" + workExperienceCount3].Value = item.EWE_EndDate;

                    cellC2_3rd["D" + workExperienceCount3].Value = item.EWE_PositionTitle?.ToUpper();
                    cellC2_3rd["G" + workExperienceCount3].Value = item.EWE_CompanyName?.ToUpper();
                    cellC2_3rd["J" + workExperienceCount3].Value = item.EWE_MonthlySalary?.ToUpper();
                    cellC2_3rd["K" + workExperienceCount3].Value = item.EWE_SalaryGrade?.ToUpper();
                    cellC2_3rd["L" + workExperienceCount3].Value = item.EWE_ApptStatus?.ToUpper();
                    cellC2_3rd["M" + workExperienceCount3++].Value = item.EWE_GovtService ? "YES" : "NO";
                    workExperienceCount++;
                    workExperienceCount2++;
                }
            }

            if (workExperienceCount < 46 && civilServiceCount < 12)
                ef.Worksheets["C2-1"].Delete();
            if (workExperienceCount2 < 46 && civilServiceCount2 < 12)
                ef.Worksheets["C2-2"].Delete();
            #endregion

            #region Sheet 3
            //VOLUNTARY WORK
            var voluntaryWordCount = 6;
            var voluntaryWordCount2 = 6;
            var voluntaryWordCount3 = 6;
            foreach (var item in employeeModel.VoluntaryWorks)
            {
                if (voluntaryWordCount < 13)
                {
                    cellC3["A" + voluntaryWordCount].Value = item.EVW_OrgName?.ToUpper();
                    cellC3["E" + voluntaryWordCount].Value = item.EVW_StartDate;
                    cellC3["F" + voluntaryWordCount].Value = item.EVW_EndDate;
                    cellC3["G" + voluntaryWordCount].Value = item.EVW_Hours;
                    cellC3["H" + voluntaryWordCount++].Value = item.EVW_PosNatureWork?.ToUpper();
                }
                else if (voluntaryWordCount2 < 13)
                {
                    cellC3_2nd["A" + voluntaryWordCount2].Value = item.EVW_OrgName?.ToUpper();
                    cellC3_2nd["E" + voluntaryWordCount2].Value = item.EVW_StartDate;
                    cellC3_2nd["F" + voluntaryWordCount2].Value = item.EVW_EndDate;
                    cellC3_2nd["G" + voluntaryWordCount2].Value = item.EVW_Hours;
                    cellC3_2nd["H" + voluntaryWordCount2++].Value = item.EVW_PosNatureWork?.ToUpper();
                    voluntaryWordCount++;
                }
                else if (voluntaryWordCount3 < 13)
                {
                    cellC3_3rd["A" + voluntaryWordCount3].Value = item.EVW_OrgName?.ToUpper();
                    cellC3_3rd["E" + voluntaryWordCount3].Value = item.EVW_StartDate;
                    cellC3_3rd["F" + voluntaryWordCount3].Value = item.EVW_EndDate;
                    cellC3_3rd["G" + voluntaryWordCount3].Value = item.EVW_Hours;
                    cellC3_3rd["H" + voluntaryWordCount3++].Value = item.EVW_PosNatureWork?.ToUpper();
                    voluntaryWordCount++;
                    voluntaryWordCount2++;
                }
            }

            //TRAINING PROGRAMS
            var trainingProgramCount = 19;
            var trainingProgramCount2 = 19;
            var trainingProgramCount3 = 19;
            foreach (var item in employeeModel.TrainingPrograms)
            {
                if (trainingProgramCount < 40)
                {
                    cellC3["A" + trainingProgramCount].Value = item.ETP_TrainingTitle?.ToUpper();
                    cellC3["E" + trainingProgramCount].Value = item.ETP_StartDate;
                    cellC3["F" + trainingProgramCount].Value = item.ETP_EndDate;
                    cellC3["G" + trainingProgramCount].Value = item.ETP_Hours;
                    cellC3["H" + trainingProgramCount].Value = item.ETP_LDType?.ToUpper();
                    cellC3["I" + trainingProgramCount++].Value = item.ETP_ConductSponsor?.ToUpper();

                }
                else if (trainingProgramCount2 < 40)
                {           
                    cellC3_2nd["A" + trainingProgramCount2].Value = item.ETP_TrainingTitle?.ToUpper();
                    cellC3_2nd["E" + trainingProgramCount2].Value = item.ETP_StartDate;
                    cellC3_2nd["F" + trainingProgramCount2].Value = item.ETP_EndDate;
                    cellC3_2nd["G" + trainingProgramCount2].Value = item.ETP_Hours;
                    cellC3_2nd["H" + trainingProgramCount2].Value = item.ETP_LDType?.ToUpper();
                    cellC3_2nd["I" + trainingProgramCount2++].Value = item.ETP_ConductSponsor?.ToUpper();

                    trainingProgramCount++;
                }
                else if (trainingProgramCount3 < 40)
                {
                    cellC3_3rd["A" + trainingProgramCount3].Value = item.ETP_TrainingTitle?.ToUpper();
                    cellC3_3rd["E" + trainingProgramCount3].Value = item.ETP_StartDate;
                    cellC3_3rd["F" + trainingProgramCount3].Value = item.ETP_EndDate;
                    cellC3_3rd["G" + trainingProgramCount3].Value = item.ETP_Hours;
                    cellC3_3rd["H" + trainingProgramCount3].Value = item.ETP_LDType?.ToUpper();
                    cellC3_3rd["I" + trainingProgramCount3++].Value = item.ETP_ConductSponsor?.ToUpper();

                    trainingProgramCount++;
                    trainingProgramCount2++;
                }
            }
            // OTHER INFORMATION
            var specialSkillCount = 43;
            var specialSkillCount2 = 43;
            var specialSkillCount3 = 43;
            foreach (var item in employeeModel.SpecialSkillsHobbies)
            {
                if (specialSkillCount < 50)
                {
                    cellC3["A" + specialSkillCount++].Value = item.ESSH_Title?.ToUpper();
                }
                else if (specialSkillCount2 < 50)
                {
                    cellC3_2nd["A" + specialSkillCount2++].Value = item.ESSH_Title?.ToUpper();
                    specialSkillCount++;
                }
                else if (specialSkillCount3 < 50)
                {
                    cellC3_3rd["A" + specialSkillCount3++].Value = item.ESSH_Title?.ToUpper();
                    specialSkillCount++;
                    specialSkillCount2++;
                }
            }
            var recognitionCount = 43;
            var recognitionCount2 = 43;
            var recognitionCount3 = 43;
            foreach (var item in employeeModel.NonAcademicDistinctions)
            {
                if (recognitionCount < 50)
                {
                    cellC3["C" + recognitionCount++].Value = item.ENAD_Title?.ToUpper();
                }
                else if (recognitionCount2 < 50)
                {
                    cellC3_2nd["C" + recognitionCount2++].Value = item.ENAD_Title?.ToUpper();
                    recognitionCount++;
                }
                else if (recognitionCount3 < 50)
                {
                    cellC3_3rd["C" + recognitionCount3++].Value = item.ENAD_Title?.ToUpper();
                    recognitionCount++;
                    recognitionCount2++;
                }
            }
            var membershipCount = 43;
            var membershipCount2 = 43;
            var membershipCount3 = 43;
            foreach (var item in employeeModel.MembershipInAssociationOrganizations)
            {
                if (membershipCount < 50)
                {
                    cellC3["I" + membershipCount++].Value = item.EMIAO_Title?.ToUpper();
                }
                else if (membershipCount2 < 50)
                {
                    cellC3_2nd["I" + membershipCount2++].Value = item.EMIAO_Title?.ToUpper();
                    membershipCount++;
                }
                else if (membershipCount3 < 50)
                {
                    cellC3_3rd["I" + membershipCount3++].Value = item.EMIAO_Title?.ToUpper();
                    membershipCount++;
                    membershipCount2++;
                }
            }

            if (voluntaryWordCount < 13 && trainingProgramCount < 40 && specialSkillCount < 50 && recognitionCount < 50 && membershipCount < 50)
                ef.Worksheets["C3-1"].Delete();

            if (voluntaryWordCount2 < 13 && trainingProgramCount2 < 40 && specialSkillCount2 < 50 && recognitionCount2 < 50 && membershipCount2 < 50)
                ef.Worksheets["C3-2"].Delete();
            #endregion

            #region Sheet 4
            if (employeeModel.OtherInformation != null)
            {
                var natDegreeCol = employeeModel.OtherInformation.EOI_Rel_NatGovtEmp ? "H" : "J";
                cellC4[natDegreeCol + "5"].Value = "X";

                var locDegreeCol = employeeModel.OtherInformation.EOI_Rel_LocalGovtEmp ? "H" : "J";
                cellC4[locDegreeCol + "8"].Value = "X";
                if (employeeModel.OtherInformation.EOI_Rel_LocalGovtEmp)
                    cellC4["I11"].Value = employeeModel.OtherInformation.EOI_Rel_LocalGovtEmp_Details?.ToUpper();

                var adminOffenseCol = employeeModel.OtherInformation.EOI_AdminOffense ? "H" : "J";
                cellC4[adminOffenseCol + "14"].Value = "X";
                if (employeeModel.OtherInformation.EOI_AdminOffense)
                    cellC4["I17"].Value = employeeModel.OtherInformation.EOI_AdminOffense_Details?.ToUpper();


                var chargeCol = employeeModel.OtherInformation.EOI_Charged ? "H" : "J";
                cellC4[chargeCol + "20"].Value = "X";
                if (employeeModel.OtherInformation.EOI_Charged)
                {
                    cellC4["L22"].Value = employeeModel.OtherInformation.EOI_Charged_Details?.ToUpper();
                    cellC4["L23"].Value = employeeModel.OtherInformation.EOI_Charged_DateFiled;
                    cellC4["L24"].Value = employeeModel.OtherInformation.EOI_Charged_CaseStatus?.ToUpper();
                }

                var convictedCol = employeeModel.OtherInformation.EOI_Convicted ? "H" : "J";
                cellC4[convictedCol + "27"].Value = "X";
                if (employeeModel.OtherInformation.EOI_Convicted)
                    cellC4["I30"].Value = employeeModel.OtherInformation.EOI_Convicted_Details?.ToUpper();

                var separatedCol = employeeModel.OtherInformation.EOI_Separated ? "H" : "J";
                cellC4[separatedCol + "33"].Value = "X";
                if (employeeModel.OtherInformation.EOI_Separated)
                    cellC4["I36"].Value = employeeModel.OtherInformation.EOI_Separated_Details;

                var candidateCol = employeeModel.OtherInformation.EOI_Candidate ? "H" : "J";
                cellC4[candidateCol + "39"].Value = "X";
                if (employeeModel.OtherInformation.EOI_Candidate)
                    cellC4["L41"].Value = employeeModel.OtherInformation.EOI_Candidate_Details?.ToUpper();

                var resignedCol = employeeModel.OtherInformation.EOI_ResignedGovt ? "H" : "J";
                cellC4[resignedCol + "43"].Value = "X";
                if (employeeModel.OtherInformation.EOI_ResignedGovt)
                    cellC4["L45"].Value = employeeModel.OtherInformation.EOI_ResignedGovt_Details?.ToUpper();

                var immigrantCol = employeeModel.OtherInformation.EOI_Immigrant ? "H" : "J";
                cellC4[immigrantCol + "48"].Value = "X";
                if (employeeModel.OtherInformation.EOI_Immigrant)
                    cellC4["L51"].Value = employeeModel.OtherInformation.EOI_Immigrant_Details?.ToUpper();

                var indigentGroupCol = employeeModel.OtherInformation.EOI_IndigentGroup ? "H" : "J";
                cellC4[indigentGroupCol + "56"].Value = "X";
                if (employeeModel.OtherInformation.EOI_IndigentGroup)
                    cellC4["L58"].Value = employeeModel.OtherInformation.EOI_IndigentGroup_Details?.ToUpper();

                var diffAbledpCol = employeeModel.OtherInformation.EOI_DiffAbled ? "H" : "J";
                cellC4[diffAbledpCol + "60"].Value = "X";
                if (employeeModel.OtherInformation.EOI_DiffAbled)
                    cellC4["M62"].Value = employeeModel.OtherInformation.EOI_DiffAbled_Details?.ToUpper();

                var soloParent = employeeModel.OtherInformation.EOI_SoloParent ? "H" : "J";
                cellC4[soloParent + "64"].Value = "X";
                if (employeeModel.OtherInformation.EOI_SoloParent)
                    cellC4["M66"].Value = employeeModel.OtherInformation.EOI_SoloParent_Details?.ToUpper();
            }

            //REFERENCES
            var referenceCount = 70;
            foreach (var item in employeeModel.References)
            {
                if (referenceCount < 73)
                {
                    cellC4["A" + referenceCount].Value = item.ER_FullName?.ToUpper();
                    cellC4["F" + referenceCount].Value = item.ER_Address?.ToUpper();
                    cellC4["G" + referenceCount++].Value = item.ER_PhoneNumber?.ToUpper();
                }
            }
            #endregion

            ef.Save(newFile);
            FileHelper.TransmitandDeleteFile("EmployeeDetails.xls", newFile);
        }

        public void PrintPDSx(int employeeID)
        {
            IHRISUnitOfWork oUnitOfWork = new HRISUnitOfWork(context);
           var employeeModel = oUnitOfWork.Employee.EmployeeDetails(employeeID);

            var applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            var newFile =
                $"{applicationPath}{@"\Content\MISC\Generated\unlocked_pds2005_"}{DateTime.Now.Ticks}{".xlsx"}";
            var template = $"{applicationPath}{@"\Content\MISC\Template\unlocked_pds2005.xlsx"}";

            SpreadsheetInfo.SetLicense("E0YU-J000-0000-000K");
            var ef = ExcelFile.Load(template);
            var worksheetC1 = ef.Worksheets["C1"];
            var worksheetC2 = ef.Worksheets["C2"];
            var worksheetC3 = ef.Worksheets["C3"];
            var worksheetC4 = ef.Worksheets["C4"];

            var cellC1 = worksheetC1.Cells;
            var cellC2 = worksheetC2.Cells;
            var cellC3 = worksheetC3.Cells;
            var cellC4 = worksheetC4.Cells;
            // Personal Information
            cellC1["C7"].Value = employeeModel.Emp_LastName;
            cellC1["C8"].Value = employeeModel.Emp_FirstName;
            cellC1["C9"].Value = employeeModel.Emp_MiddleName;
            cellC1["C11"].Value = employeeModel.Emp_BirthPlace;
            cellC1["C12"].Value = employeeModel.Emp_Gender == "F" ? "Female" : "Male";

            cellC1["C16"].Value = employeeModel.Emp_Citizenship;
            cellC1["C17"].Value = employeeModel.Emp_Height;
            cellC1["C18"].Value = employeeModel.Emp_Weight;
            cellC1["C19"].Value = employeeModel.Emp_BloodType;
            cellC1["C20"].Value = employeeModel.Emp_GSISNumber;
            cellC1["C21"].Value = employeeModel.Emp_PAGIBIGNumber;
            cellC1["C22"].Value = employeeModel.Emp_PHICNumber;
            cellC1["C23"].Value = employeeModel.Emp_SSSNumber;

            cellC1["E10"].Value = employeeModel.Emp_BirthDate;

            //cellC1["I10"].Value = employeeModel.Emp_Residential_Address;
            cellC1["I14"].Value = employeeModel.Emp_Residential_PhoneNumber;
            cellC1["I13"].Value = employeeModel.Emp_Residential_ZipCode;
            //cellC1["I15"].Value = employeeModel.Emp_Permanent_Address;
            cellC1["I19"].Value = employeeModel.Emp_Permanent_PhoneNumber;
            cellC1["I18"].Value = employeeModel.Emp_Permanent_ZipCode;
            cellC1["I20"].Value = employeeModel.Emp_EmailAddress;
            cellC1["I21"].Value = employeeModel.Emp_MobileNumber;
            cellC1["I22"].Value = employeeModel.Emp_AgencyEmpNumber;
            cellC1["I23"].Value = employeeModel.Emp_TINNumber;
            cellC1["N9"].Value = employeeModel.Emp_SuffixName;

            if (employeeModel.Emp_CivilStatus == 1)
                cellC1["D13"].Value = "Single";
            else if (employeeModel.Emp_CivilStatus == 2)
                cellC1["E13"].Value = "Widowed";
            else if (employeeModel.Emp_CivilStatus == 3)
                cellC1["D14"].Value = "Married";
            else if (employeeModel.Emp_CivilStatus == 4)
                cellC1["E14"].Value = "Separated";
            else if (employeeModel.Emp_CivilStatus == 5)
                cellC1["D15"].Value = "Annulled";
            else
                cellC1["E15"].Value = employeeModel.Emp_CivilStatus_Other;

            //Family Background
            cellC1["C25"].Value = employeeModel.Emp_Spouse_LastName;
            cellC1["C26"].Value = employeeModel.Emp_Spouse_FirstName;
            cellC1["C27"].Value = employeeModel.Emp_Spouse_MiddleName;
            cellC1["C28"].Value = employeeModel.Emp_Spouse_Occupation;
            cellC1["C29"].Value = employeeModel.Emp_Spouse_EmpBusName;
            cellC1["C30"].Value = employeeModel.Emp_Spouse_EmpBusAddress;
            cellC1["C31"].Value = employeeModel.Emp_Spouse_EmpBusPhoneNumber;

            cellC1["D33"].Value = employeeModel.Emp_Father_LastName;
            cellC1["D34"].Value = employeeModel.Emp_Father_FirstName;
            cellC1["D35"].Value = employeeModel.Emp_Father_MiddleName;
            cellC1["D37"].Value = employeeModel.Emp_Mother_LastName;
            cellC1["D38"].Value = employeeModel.Emp_Mother_FirstName;
            cellC1["D39"].Value = employeeModel.Emp_Mother_MiddleName;

            var childCount = 26;
            foreach (var item in employeeModel.EmployeeChildren)
            {
                if (childCount < 39)
                {
                    cellC1["H" + childCount].Value = item.EC_FullName;
                    cellC1["M" + childCount++].Value = item.EC_BirthDate;
                }
            }

            var collegeCount = 0;
            var graduateCount = 0;
            //Educational Background
            foreach (var item in employeeModel.EducationalBackgrounds)
            {
                if (item.EEB_EducType == 1)
                {
                    cellC1["C44"].Value = item.EEB_SchoolName;
                    cellC1["F44"].Value = item.EEB_DegreeCourse;
                    cellC1["I44"].Value = item.EEB_HighestLevel;
                    cellC1["L44"].Value = item.EEB_StartDate;
                    cellC1["M44"].Value = item.EEB_EndDate;
                    cellC1["N44"].Value = item.EEB_Awards;
                }
                else if (item.EEB_EducType == 2)
                {
                    cellC1["C45"].Value = item.EEB_SchoolName;
                    cellC1["F45"].Value = item.EEB_DegreeCourse;
                    cellC1["I45"].Value = item.EEB_HighestLevel;
                    cellC1["L45"].Value = item.EEB_StartDate;
                    cellC1["M45"].Value = item.EEB_EndDate;
                    cellC1["N45"].Value = item.EEB_Awards;
                }
                else if (item.EEB_EducType == 3)
                {
                    cellC1["C46"].Value = item.EEB_SchoolName;
                    cellC1["F46"].Value = item.EEB_DegreeCourse;
                    cellC1["I46"].Value = item.EEB_HighestLevel;
                    cellC1["L46"].Value = item.EEB_StartDate;
                    cellC1["M46"].Value = item.EEB_EndDate;
                    cellC1["N46"].Value = item.EEB_Awards;
                }
                else if (item.EEB_EducType == 4)
                {
                    if (collegeCount == 0)
                    {
                        cellC1["C47"].Value = item.EEB_SchoolName;
                        cellC1["F47"].Value = oUnitOfWork.Course.GetCourseName(Convert.ToInt32(item.EEB_DegreeCourse));
                        cellC1["I47"].Value = item.EEB_HighestLevel;
                        cellC1["L47"].Value = item.EEB_StartDate;
                        cellC1["M47"].Value = item.EEB_EndDate;
                        cellC1["N47"].Value = item.EEB_Awards;
                    }
                    else if (collegeCount == 1)
                    {
                        cellC1["C48"].Value = item.EEB_SchoolName;
                        cellC1["F48"].Value = oUnitOfWork.Course.GetCourseName(Convert.ToInt32(item.EEB_DegreeCourse));
                        cellC1["I48"].Value = item.EEB_HighestLevel;
                        cellC1["L48"].Value = item.EEB_StartDate;
                        cellC1["M48"].Value = item.EEB_EndDate;
                        cellC1["N48"].Value = item.EEB_Awards;
                    }
                    collegeCount++;
                }
                else if (item.EEB_EducType == 5)
                {
                    if (graduateCount == 0)
                    {
                        cellC1["C49"].Value = item.EEB_SchoolName;
                        cellC1["F49"].Value = oUnitOfWork.Course.GetCourseName(Convert.ToInt32(item.EEB_DegreeCourse));
                        cellC1["H49"].Value = item.EEB_GraduateYear;
                        cellC1["L49"].Value = item.EEB_StartDate;
                        cellC1["M49"].Value = item.EEB_EndDate;
                        cellC1["N49"].Value = item.EEB_Awards;
                    }
                    else if (graduateCount == 1)
                    {
                        cellC1["C50"].Value = item.EEB_SchoolName;
                        cellC1["F50"].Value = oUnitOfWork.Course.GetCourseName(Convert.ToInt32(item.EEB_DegreeCourse));
                        cellC1["H50"].Value = item.EEB_GraduateYear;
                        cellC1["L50"].Value = item.EEB_StartDate;
                        cellC1["M50"].Value = item.EEB_EndDate;
                        cellC1["N50"].Value = item.EEB_Awards;
                    }
                    graduateCount++;
                }
            }

            //Cell 2

            //Civil Service eligibility
            var civilServiceCount = 5;
            foreach (var item in employeeModel.CivilServiceEligibilities)
            {
                if (civilServiceCount < 12)
                {
                    cellC2["A" + civilServiceCount].Value =
                        oUnitOfWork.Eligibility.GetEligibilities(Convert.ToInt32(item.ECSE_Title));
                    cellC2["F" + civilServiceCount].Value = item.ECSE_Rating;
                    cellC2["G" + civilServiceCount].Value = item.ECSE_ExamDate;
                    cellC2["I" + civilServiceCount].Value = item.ECSE_ExamPlace;
                    cellC2["L" + civilServiceCount].Value = item.ECSE_LicNumber;
                    cellC2["M" + civilServiceCount++].Value = item.ECSE_LicReleaseDate;
                }
            }
            //WORK EXPERIENCE
            var workExperienceCount = 17;
            foreach (var item in employeeModel.WorkExperiences)
            {
                if (workExperienceCount < 37)
                {
                    cellC2["A" + workExperienceCount].Value = item.EWE_StartDate;
                    cellC2["C" + workExperienceCount].Value = item.EWE_EndDate;
                    cellC2["D" + workExperienceCount].Value = item.EWE_PositionTitle;
                    cellC2["G" + workExperienceCount].Value = item.EWE_CompanyName;
                    cellC2["J" + workExperienceCount].Value = item.EWE_MonthlySalary;
                    cellC2["K" + workExperienceCount].Value = item.EWE_SalaryGrade;
                    cellC2["L" + workExperienceCount].Value = item.EWE_ApptStatus;
                    cellC2["M" + workExperienceCount++].Value = item.EWE_GovtService ? "Yes" : "No";
                }
            }

            //Cell 3

            //VOLUNTARY WORK
            var voluntaryWordCount = 6;
            foreach (var item in employeeModel.VoluntaryWorks)
            {
                if (voluntaryWordCount < 11)
                {
                    cellC3["A" + voluntaryWordCount].Value = item.EVW_OrgName;
                    cellC3["E" + voluntaryWordCount].Value = item.EVW_StartDate;
                    cellC3["F" + voluntaryWordCount].Value = item.EVW_EndDate;
                    cellC3["G" + voluntaryWordCount].Value = item.EVW_Hours;
                    cellC3["H" + voluntaryWordCount++].Value = item.EVW_PosNatureWork;
                }
            }

            //TRAINING PROGRAMS
            var trainingProgramCount = 16;
            foreach (var item in employeeModel.TrainingPrograms)
            {
                if (trainingProgramCount < 31)
                {
                    cellC3["A" + trainingProgramCount].Value = item.ETP_TrainingTitle;
                    cellC3["E" + trainingProgramCount].Value = item.ETP_StartDate;
                    cellC3["F" + trainingProgramCount].Value = item.ETP_EndDate;
                    cellC3["G" + trainingProgramCount].Value = item.ETP_Hours;
                    cellC3["H" + trainingProgramCount++].Value = item.ETP_ConductSponsor;
                }
            }
            // OTHER INFORMATION
            var specialSkillCount = 34;
            foreach (var item in employeeModel.SpecialSkillsHobbies)
            {
                if (specialSkillCount < 39)
                {
                    cellC3["A" + specialSkillCount++].Value = item.ESSH_Title;
                }
            }
            var recognitionCount = 34;
            foreach (var item in employeeModel.NonAcademicDistinctions)
            {
                if (recognitionCount < 39)
                {
                    cellC3["C" + recognitionCount++].Value = item.ENAD_Title;
                }
            }
            var membershipCount = 34;
            foreach (var item in employeeModel.MembershipInAssociationOrganizations)
            {
                if (membershipCount < 39)
                {
                    cellC3["H" + membershipCount++].Value = item.EMIAO_Title;
                }
            }

            //Cell 4       
            if (employeeModel.OtherInformation != null)
            {
                cellC4["E4"].Value = employeeModel.OtherInformation.EOI_Rel_NatGovtEmp ? "Yes" : "No";
                cellC4["E5"].Value = "If YES, give details: " + Environment.NewLine +
                                     employeeModel.OtherInformation.EOI_Rel_NatGovtEmp_Details;
                //cellC4["E5"].Style.Font.UnderlineStyle = UnderlineStyle.Single;
                cellC4["E8"].Value = employeeModel.OtherInformation.EOI_Rel_LocalGovtEmp ? "Yes" : "No";
                cellC4["E9"].Value = "If YES, give details: " + Environment.NewLine +
                                     employeeModel.OtherInformation.EOI_Rel_LocalGovtEmp_Details;
                cellC4["E12"].Value = employeeModel.OtherInformation.EOI_Charged ? "Yes" : "No";
                cellC4["E13"].Value = "If YES, give details: " + Environment.NewLine +
                                      employeeModel.OtherInformation.EOI_Charged_Details;
                cellC4["E14"].Value = employeeModel.OtherInformation.EOI_AdminOffense ? "Yes" : "No";
                cellC4["E15"].Value = "If YES, give details: " + Environment.NewLine +
                                      employeeModel.OtherInformation.EOI_AdminOffense_Details;
                cellC4["E18"].Value = employeeModel.OtherInformation.EOI_Convicted ? "Yes" : "No";
                cellC4["E19"].Value = "If YES, give details: " + Environment.NewLine +
                                      employeeModel.OtherInformation.EOI_Convicted_Details;
                cellC4["E23"].Value = employeeModel.OtherInformation.EOI_Separated ? "Yes" : "No";
                cellC4["E24"].Value = "If YES, give details: " + Environment.NewLine +
                                      employeeModel.OtherInformation.EOI_Separated_Details;
                cellC4["E26"].Value = employeeModel.OtherInformation.EOI_Separated ? "Yes" : "No";
                cellC4["E27"].Value = "If YES, give details: " + Environment.NewLine +
                                      employeeModel.OtherInformation.EOI_Candidate_Details;
                cellC4["E32"].Value = employeeModel.OtherInformation.EOI_IndigentGroup ? "Yes" : "No";
                cellC4["E33"].Value = "If YES, please specify: " + employeeModel.OtherInformation.EOI_IndigentGroup_Details;
                cellC4["E34"].Value = employeeModel.OtherInformation.EOI_DiffAbled ? "Yes" : "No";
                cellC4["E35"].Value = "If YES, please specify: " + employeeModel.OtherInformation.EOI_DiffAbled_Details;
                cellC4["E36"].Value = employeeModel.OtherInformation.EOI_SoloParent ? "Yes" : "No";
                cellC4["E37"].Value = "If YES, please specify: " + employeeModel.OtherInformation.EOI_SoloParent_Details;
            }

            //REFERENCES
            var referenceCount = 41;
            foreach (var item in employeeModel.References)
            {
                if (referenceCount < 44)
                {
                    cellC4["A" + referenceCount].Value = item.ER_FullName;
                    cellC4["D" + referenceCount].Value = item.ER_Address;
                    cellC4["E" + referenceCount++].Value = item.ER_PhoneNumber;
                }
            }
            ef.Save(newFile);
            FileHelper.TransmitandDeleteFile("EmployeeDetails.xlsx", newFile);
        }
    }

    public interface IPDSExport
    {
        void PrintPDS(int employeeID);
    }
}
