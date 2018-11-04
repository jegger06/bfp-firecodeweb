using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EBFP.BL.Helper;
using EBFP.DataAccess;
using Newtonsoft.Json;

namespace EBFP.BL.HumanResources
{
    public class MedicalBL : EntityFrameworkBase, IMedical
    {
        public MedicalBL(EBFPEntities _context)
        {
            context_ = _context;
        }

        public async Task<EmployeeMedicalModel> EmployeeMedicalDetails(int Emp_Id)
        {
            var retMedical = new EmployeeMedicalModel();
            var namespaceName = "EBFP.BL.HumanResources.";
            var details = context.tblMedical.Where(a => a.Emp_Id == Emp_Id).ToList();

            foreach (var item in details)
            {
                if (item.Med_CategoryId == null) continue;

                var medCategory = ((Medical) item.Med_CategoryId).ToString();
                var type = Type.GetType($"{namespaceName}{medCategory}{"Model"}");
                dynamic dynamicModel = Activator.CreateInstance(type);
                
                Type listT = typeof(List<>).MakeGenericType(new[] { type });
                dynamic list = Activator.CreateInstance(listT);

                if (item.Med_CategoryId > 3)
                { 
                    var json = item.Med_Details_Json.Replace("[", "").Replace("]", "");

                    if (json != "")
                    {
                        var jsonList = Regex.Split(json, @"\}\,");

                        foreach (var jsonDetails in jsonList.Select(x => x.Contains("}") ? x : ((x == "null") ? "" : $"{x}{"}"}")))
                        {
                            if (!string.IsNullOrWhiteSpace(jsonDetails))
                            {
                                dynamicModel = JsonConvert.DeserializeObject(jsonDetails, dynamicModel.GetType());
                                list.Add(dynamicModel);
                            }
                        }
                    }
                }
                else
                {
                    dynamicModel = JsonConvert.DeserializeObject(item.Med_Details_Json, dynamicModel.GetType());
                }


                foreach (var propertyInfo in retMedical.GetType().GetProperties())
                {
                    if (propertyInfo.Name == medCategory)
                    {
                        if (propertyInfo.CanRead)
                        {

                            if (item.Med_CategoryId > 3)
                            {
                                propertyInfo.SetValue(retMedical, list, null);
                            }
                            else
                            {
                                propertyInfo.SetValue(retMedical, dynamicModel, null);
                            }
                        }

                        break;
                    }
                }
            }

            if (details.Count == 0)
            {
                retMedical = SetEmployeeInfo(retMedical, Emp_Id);
            }


            return retMedical;
        }

        public EmployeeMedicalModel SaveMedicalDetails(EmployeeMedicalModel model)
        {
            try
            {
                var physicalExamDetails = context.tblMedical
                    .Where(a => a.Emp_Id == model.MedicalEmployeeInfo.Emp_Id)
                    .ToList();

                foreach (var propertyInfo in model.GetType().GetProperties())
                {
                    if (propertyInfo.CanRead)
                    {
                        var value = propertyInfo.GetValue(model, null);

                        var medCategory = (int) Functions.GetEnumValueFromDescription<Medical>(propertyInfo.Name);
                        if (medCategory > 0)
                        {
                            var isNew = false;
                            var medical = physicalExamDetails.FirstOrDefault(a => a.Med_CategoryId.Value == medCategory);
                            if (medical == null)
                            {
                                isNew = true;
                                medical = new tblMedical
                                {
                                    Med_CategoryId = medCategory,
                                    Emp_Id = model.MedicalEmployeeInfo.Emp_Id,
                                    Med_Id = model.MedicalEmployeeInfo.Med_Id
                                };
                            }

                            medical.Med_Details_Json = JsonConvert.SerializeObject(value);

                            if (isNew)
                                context.tblMedical.Add(medical);

                            context.SaveChanges();
                        }
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            return model;
        }

        private tblEmployees GetEmployeeByID(int Emp_Id)
        {
            var employee = context.tblEmployees.Find(Emp_Id);
            return employee;
        }

        private EmployeeMedicalModel SetEmployeeInfo(EmployeeMedicalModel model, int Emp_Id)
        {
            var empDetails = GetEmployeeByID(Emp_Id);

            model.MedicalEmployeeInfo.Emp_Id = empDetails.Emp_Id;
            model.MedicalEmployeeInfo.Emp_Age = Functions.GetAge(empDetails.Emp_BirthDate).ToString();
            model.MedicalEmployeeInfo.Emp_Number = empDetails.Emp_Number;
            model.MedicalEmployeeInfo.Emp_FirstName = empDetails.Emp_FirstName;
            model.MedicalEmployeeInfo.Emp_MiddleName = empDetails.Emp_MiddleName;
            model.MedicalEmployeeInfo.Emp_LastName = empDetails.Emp_LastName;
            model.MedicalEmployeeInfo.Emp_SuffixName = empDetails.Emp_SuffixName;
            model.MedicalEmployeeInfo.Emp_BirthDate = empDetails.Emp_BirthDate;
            model.MedicalEmployeeInfo.Emp_Gender = empDetails.Emp_Gender;
            model.MedicalEmployeeInfo.Emp_CivilStatus = empDetails.Emp_CivilStatus.HasValue ? empDetails.Emp_CivilStatus.Value : 0;
            model.MedicalEmployeeInfo.Emp_CivilStatus_Other = empDetails.Emp_CivilStatus_Other;
            model.MedicalEmployeeInfo.Emp_Citizenship = empDetails.Emp_Citizenship;
            model.MedicalEmployeeInfo.Emp_BloodType = empDetails.Emp_BloodType;

            model.MedicalEmployeeInfo.Emp_Curr_Rank = empDetails.Emp_Curr_Rank?.ToString();
            model.MedicalEmployeeInfo.Emp_Curr_Unit = empDetails.Emp_Curr_Unit?.ToString();
            model.MedicalEmployeeInfo.Emp_Curr_JobFunc = empDetails.Emp_Curr_JobFunc?.ToString();
            model.MedicalEmployeeInfo.Emp_MobileNumber = empDetails.Emp_MobileNumber;
            //model.MedicalEmployeeInfo.Emp_Residential_Address = empDetails.Emp_Residential_Address;
            model.MedicalEmployeeInfo.Emp_Residential_Address = FormatResidentialAddress(empDetails);

            return model;
        }

        private string FormatResidentialAddress(tblEmployees emp)
        {
            string address = string.Empty;

            address = emp.Emp_Residential_HouseNo;
            address += !string.IsNullOrWhiteSpace(emp.Emp_Residential_Street) ? " " + emp.Emp_Residential_Street : "";
            address += !string.IsNullOrWhiteSpace(emp.Emp_Residential_Village) ? " " + emp.Emp_Residential_Village : "";
            address += !string.IsNullOrWhiteSpace(emp.Emp_Residential_Barangay) ? " " + emp.Emp_Residential_Barangay: "";
            address += !string.IsNullOrWhiteSpace(emp.Emp_Residential_Municipality) ? " " + emp.Emp_Residential_Municipality + "," : "";
            address += !string.IsNullOrWhiteSpace(emp.Emp_Residential_Province) ? " " + emp.Emp_Residential_Province : "";

            return address;
        }
    }
}