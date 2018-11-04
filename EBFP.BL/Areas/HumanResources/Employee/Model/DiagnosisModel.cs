using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBFP.BL.HumanResources
{
    public partial class DiagnosisListModel
    {
        public DiagnosisListModel()
        {
           // this.DiagnosisList = new List<DiagnosisModel>();
        }

        public string sPE_Id { get; set; }
        public int PE_Id { get; set; }
        public Nullable<System.DateTime> PE_Date { get; set; }
        public int PE_CategoryId { get; set; }
        public string PE_Details_Json { get; set; }

        //public List<DiagnosisModel> DiagnosisList { get; set; }
    }

}
