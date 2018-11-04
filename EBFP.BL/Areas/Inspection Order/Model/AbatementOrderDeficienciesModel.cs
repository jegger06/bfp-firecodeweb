
namespace EBFP.BL.InspectionOrder
{
    using System;
    public class AbatementOrderDeficienciesModel
    {
        public int AOD_Id { get; set; }
        public string Ref_AOD_Id { get; set; }
        public string AOD_AO_Id { get; set; }
        public string AOD_Deficiency { get; set; }
        public string AOD_Compliance { get; set; }
        public string AOD_GracePeriod { get; set; }
        public bool AOD_Complied { get; set; }
        public string AOD_Remarks { get; set; }
        public int AOD_Unit_Id { get; set; }
        public bool IsSynced { get; set; }
        public decimal? AOD_Fine { get; set; }
    }
}
