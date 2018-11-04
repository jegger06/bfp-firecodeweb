using System;

namespace EBFP.BL.Administration
{
    public partial class UsedORModel
    {
        public long OR_Number { get; set; }
        public decimal OR_Amount { get; set; }
        public DateTime OR_Date { get; set; }
        public string OR_Type { get; set; }
        public string OR_App_Number { get; set; }
        public string OR_Est_Name { get; set; }
        public string OR_BAN_Number { get; set; }
        public string OR_Est_Address { get; set; }

    }    
}
