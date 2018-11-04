
namespace EBFP.BL.InspectionOrder
{
    using System;
    public class ClosureOrderDeficienciesModel
    {
        public int COD_Id { get; set; }
        public string Ref_COD_Id { get; set; }
        public string COD_CO_Id { get; set; }
        public string COD_Deficiency { get; set; }
        public string COD_Compliance { get; set; }
        public string COD_GracePeriod { get; set; }
        public bool COD_Complied { get; set; }
        public string COD_Remarks { get; set; }
        public int COD_Unit_Id { get; set; }
        public bool IsSynced { get; set; }
        public decimal? COD_Fine { get; set; }
    }
}
