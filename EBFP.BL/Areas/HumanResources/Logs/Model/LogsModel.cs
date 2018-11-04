using System;

namespace EBFP.BL.HumanResources
{
    public class LogsModel
    {
        public int Log_Id { get; set; }
        public int Log_Emp_Id { get; set; }
        public string Log_Remarks { get; set; }
        public DateTime Log_UpdatedDate { get; set; }
        public int Log_Updated_Emp_Id { get; set; }
    }
}