

using System;
using System.Collections.Generic;

namespace EBFP.BL.HumanResources
{
    public partial class OtherInformationModel
    {
        public int EOI_Id { get; set; }
        public bool EOI_Rel_NatGovtEmp { get; set; }
        public string EOI_Rel_NatGovtEmp_Details { get; set; }
        public bool EOI_Rel_LocalGovtEmp { get; set; }
        public string EOI_Rel_LocalGovtEmp_Details { get; set; }
        public bool EOI_Charged { get; set; }
        public string EOI_Charged_Details { get; set; }
        public DateTime? EOI_Charged_DateFiled { get; set; }
        public string EOI_Charged_CaseStatus { get; set; }
        public bool EOI_AdminOffense { get; set; }
        public string EOI_AdminOffense_Details { get; set; }
        public bool EOI_Convicted { get; set; }
        public string EOI_Convicted_Details { get; set; }
        public bool EOI_Separated { get; set; }
        public string EOI_Separated_Details { get; set; }
        public bool EOI_Candidate { get; set; }
        public string EOI_Candidate_Details { get; set; }
        public bool EOI_ResignedGovt { get; set; }
        public string EOI_ResignedGovt_Details { get; set; }
        public bool EOI_Immigrant { get; set; }
        public string EOI_Immigrant_Details { get; set; }
        public bool EOI_IndigentGroup { get; set; }
        public string EOI_IndigentGroup_Details { get; set; }
        public bool EOI_DiffAbled { get; set; }
        public string EOI_DiffAbled_Details { get; set; }
        public bool EOI_SoloParent { get; set; }
        public string EOI_SoloParent_Details { get; set; }
        public int EOI_Emp_Id { get; set; }

        public static implicit operator OtherInformationModel(List<OtherInformationModel> v)
        {
            throw new NotImplementedException();
        }
    }
}
