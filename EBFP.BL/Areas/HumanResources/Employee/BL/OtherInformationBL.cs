namespace EBFP.BL.HumanResources
{
    using AutoMapper;
    using DataAccess;
    using Queries.Core.Repositories;
    using System.Threading.Tasks;
    using System.Linq;
    using System.Collections.Generic;

    public class OtherInformationBL : Repository<tblEmployeeOtherInformation, OtherInformationModel>, IOtherInformation
    {
        public OtherInformationBL(EBFPEntities context) : base(context)
        {
            CreateMapping();
        }
        public void CreateMapping()
        {
            Mapper.CreateMap<tblEmployeeOtherInformation, OtherInformationModel>();
            Mapper.CreateMap<tblEmployeeOtherInformation, OtherInformationModel>().ReverseMap();
            Mapper.CreateMap<List<tblEmployeeOtherInformation>, List<OtherInformationModel>>().ReverseMap();
            Mapper.CreateMap<List<tblEmployeeOtherInformation>, List<OtherInformationModel>>();
        }

        public OtherInformationModel GetByEmployee(int employeeID)
        {
            var ret = new OtherInformationModel();
            var otherInformationModel = BFPContext.tblEmployeeOtherInformation.Where(a => a.EOI_Emp_Id == employeeID).OrderByDescending(a => a.EOI_Id).FirstOrDefault();
          
            if (otherInformationModel != null)
                Mapper.Map(otherInformationModel, ret);
            else
                ret = null;
            
            //ret = SingleOrDefault(a => a.EOI_Emp_Id == employeeID);
            if (ret != null)
            {
                if (!ret.EOI_Rel_NatGovtEmp)
                    ret.EOI_Rel_NatGovtEmp_Details = string.Empty;

                if (!ret.EOI_Rel_LocalGovtEmp)
                    ret.EOI_Rel_LocalGovtEmp_Details = string.Empty;

                if (!ret.EOI_Charged)
                {
                    ret.EOI_Charged_Details = string.Empty;
                    ret.EOI_Charged_DateFiled = null;
                    ret.EOI_Charged_CaseStatus = string.Empty;
                }

                if (!ret.EOI_AdminOffense)
                    ret.EOI_AdminOffense_Details = string.Empty;

                if (!ret.EOI_Convicted)
                    ret.EOI_Convicted_Details = string.Empty;

                if (!ret.EOI_Separated)
                    ret.EOI_Separated_Details = string.Empty;

                if (!ret.EOI_Candidate)
                    ret.EOI_Candidate_Details = string.Empty;

                if (!ret.EOI_ResignedGovt)
                    ret.EOI_Rel_LocalGovtEmp_Details = string.Empty;

                if (!ret.EOI_Immigrant)
                    ret.EOI_Immigrant_Details = string.Empty;

                if (!ret.EOI_IndigentGroup)
                    ret.EOI_IndigentGroup_Details = string.Empty;

                if (!ret.EOI_DiffAbled)
                    ret.EOI_DiffAbled_Details = string.Empty;

                if (!ret.EOI_SoloParent)
                    ret.EOI_SoloParent_Details = string.Empty;
            }
             
            return ret;
        }
         
        public void Insert(OtherInformationModel model, int employeeID)
        {
            DeleteByemployeeID(employeeID);

            model.EOI_Emp_Id = employeeID;
            var mappedItem = Mapper.Map(model, new tblEmployeeOtherInformation());
            BFPContext.tblEmployeeOtherInformation.Add(mappedItem); 
        }
         
        private void DeleteByemployeeID(int employeeID)
        { 
            RemoveRange(a=> a.EOI_Emp_Id == employeeID); 
        } 
    }
}
