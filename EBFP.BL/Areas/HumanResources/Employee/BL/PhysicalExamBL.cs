using System.Collections;
using System.Reflection;
using System.Runtime.Remoting;
using System.Web.Helpers;
using System.Web.Script.Serialization;

namespace EBFP.BL.HumanResources
{
    using AutoMapper;
    using EBFP.DataAccess;
    using EBFP.Helper;
    using Helper; 
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class PhysicalExamBL : EntityFrameworkBase, IPhysicalExam
    {
        public PhysicalExamBL(EBFPEntities _context)
        {
            base.context_ = _context;
        }
        

        public PhysicalExamModel SavePhysicalExamDetails(PhysicalExamModel model)
        {
            try
            {
                var physicalExamDetails = context.tblPhysicalExam
                    .Where(a => a.Emp_Id == model.PatientInformation.Emp_Id &&
                                a.PE_Id == model.PatientInformation.PE_Id)
                    .ToList();
                var PE_ID = GetNextPhysicalExamId();

                if (physicalExamDetails.Count == 0)
                {
                    model.PatientInformation.PE_Id = PE_ID;
                }

                foreach (PropertyInfo propertyInfo in model.GetType().GetProperties())
                {
                    if (propertyInfo.CanRead)
                    {
                        object value = propertyInfo.GetValue(model, null);
                        
                        var peCategory = (int) Functions.GetEnumValueFromDescription<PhysicalExam>(propertyInfo.Name);
                        if (peCategory > 0)
                        {
                            var isNew = false;
                            var physicalExam = physicalExamDetails.FirstOrDefault(a => a.PE_CategoryId == peCategory);
                            if (physicalExam == null)
                            {
                                isNew = true;
                                physicalExam = new tblPhysicalExam
                                {
                                    PE_CategoryId = peCategory,
                                    Emp_Id = model.PatientInformation.Emp_Id,
                                    PE_Id = PE_ID
                                };
                            }

                            physicalExam.PE_Date = model.PatientInformation.PE_Service_Date;
                            physicalExam.PE_Details_Json = JsonConvert.SerializeObject(value);

                            if (isNew)
                                context.tblPhysicalExam.Add(physicalExam);

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

        public PhysicalExamListResult GetPhysicalExams(GridInfo gridInfo)
        {
            var employeeId = Convert.ToInt32(gridInfo.searchValue.Decrypt());

            var physicalExamList = context.tblPhysicalExam
                .Where(a => a.Emp_Id == employeeId)
                .Select(a => new {a.PE_Date, a.PE_Id})
                .Distinct()
                .ToList();

            var retPEs = physicalExamList.Select(pe => new PhysicalExamTableModel
            {
                PE_Date = pe.PE_Date,
                sPE_Id = pe.PE_Id.ToNullSafeString().Encrypt(),
                PE_Id = pe.PE_Id
            }).ToList();

            return new PhysicalExamListResult
            {
                PhysicalExamListModel = retPEs,
                DatatableInfo = gridInfo

            };
        }

        public async Task<PhysicalExamModel> PhysicalExamDetails(int PE_Id)
        {
            var namespaceName = "EBFP.BL.HumanResources.";
            var retPhysical = new PhysicalExamModel();
            var details = context.tblPhysicalExam.Where(a => a.PE_Id == PE_Id).ToList();

            foreach (var item in details)
            {
                if (item.PE_CategoryId == null) continue;

                var peCategory = ((PhysicalExam)item.PE_CategoryId).ToString();
                Type type = Type.GetType($"{namespaceName}{peCategory}{"Model"}");
                dynamic dynamicModel = Activator.CreateInstance(type);

                if (peCategory == "DiagnosisList")
                {
                    List<PhysicalExamDiagnosisModel> diagnosis = new List<PhysicalExamDiagnosisModel>();
                    dynamicModel = JsonConvert.DeserializeObject<List<PhysicalExamDiagnosisModel>>(item.PE_Details_Json);
                }
                else
                {
                    dynamicModel = JsonConvert.DeserializeObject(item.PE_Details_Json, dynamicModel.GetType());
                }
               
                    
                foreach (PropertyInfo propertyInfo in retPhysical.GetType().GetProperties())
                {
                    if (propertyInfo.Name == peCategory)
                    {
                        if (propertyInfo.CanRead)
                        {
                            propertyInfo.SetValue(retPhysical, dynamicModel, null);
                        }

                        break;
                    }
                        
                }
            }
            
            return retPhysical;
        }

        public int GetNextPhysicalExamId()
        {
            var maxDetails = context.tblPhysicalExam.OrderByDescending(a => a.PE_Id).FirstOrDefault();
            if (maxDetails == null)
                return 1;

            return maxDetails.PE_Id + 1;
        }

        public bool DeleteByPEID(int physicalExamId)
        {
            var physicalExam = context.tblPhysicalExam
                .Where(a => a.PE_Id == physicalExamId)
                .ToList();

            //physicalExam.Emp_IsDeleted = true;
            context.tblPhysicalExam.RemoveRange(physicalExam);
            context.SaveChanges();

            return true;
        }

    }
}
