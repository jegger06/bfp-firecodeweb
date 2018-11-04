using EBFP.BL.Helper;
using EBFP.Helper;
using System.Collections.Generic;
using System.Web;

namespace EBFP.BL.HumanResources
{
    public class RegionListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<RegionModel> RegionList { get; set; }
    }

    public partial class RegionModel
    {
        public string sReg_Id
        {
            get { return Reg_Id.ToString().Encrypt(); }
        }

        public int Reg_Id { get; set; }
        public string Reg_Title { get; set; }
        public string Reg_Description { get; set; }
        public byte[] Reg_Logo { get; set; }
        public HttpPostedFileBase RegLogo { get; set; }
    }
}
