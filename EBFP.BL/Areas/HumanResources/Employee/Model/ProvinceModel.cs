using EBFP.DataAccess;

namespace EBFP.BL.HumanResources
{
    public partial class ProvinceModel
    {
        public int Province_Id { get; set; }
        public string Province_Name { get; set; }
        public int Region_Id { get; set; }

        public virtual tblRegions tblRegions { get; set; }
    }
}
