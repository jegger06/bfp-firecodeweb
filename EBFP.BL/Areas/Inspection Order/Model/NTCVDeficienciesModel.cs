
namespace EBFP.BL.InspectionOrder
{
    public class NTCVDeficienciesModel
    {
        public int NTCVD_Id { get; set; }
        public string NTCVD_NTCV_Id { get; set; }
        public string NTCVD_Deficiency { get; set; }
        public string NTCVD_Compliance { get; set; }
        public string NTCVD_GracePeriod { get; set; }
        public bool NTCVD_Complied { get; set; }
        public string NTCVD_Remarks { get; set; }
        public string Ref_NTCVD_Id { get; set; }
        public int NTCVD_Unit_Id { get; set; }
        public decimal? NTCVD_Fine { get; set; }
    }
}
