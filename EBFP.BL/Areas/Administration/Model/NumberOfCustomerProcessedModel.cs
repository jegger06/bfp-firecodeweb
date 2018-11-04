using System;

namespace EBFP.BL.Administration
{
    public partial class NumberOfCustomerProcessedModel
    {
        public DateTime? Processed_Date { get; set; }
        public string Processed_Type { get; set; }
        public string Processed_App_Number { get; set; }
        public string Processed_Est_Name { get; set; }
    }    
}
