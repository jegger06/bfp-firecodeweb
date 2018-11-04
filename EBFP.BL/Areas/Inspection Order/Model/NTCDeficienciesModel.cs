
using System;

namespace EBFP.BL.InspectionOrder
{
    public class NTCDeficienciesModel
    {
        public int NTCD_Id { get; set; }
        public string NTCD_NTC_Id { get; set; }
        public string NTCD_Deficiency { get; set; }
        public string NTCD_Compliance { get; set; }
        public string NTCD_GracePeriod { get; set; }
        public bool NTCD_Complied { get; set; }
        public string NTCD_Remarks { get; set; }
        public string Ref_NTCD_Id { get; set; }
        public int NTCD_Unit_Id { get; set; }
        public decimal? NTCD_Fine { get; set; }
    }
}
