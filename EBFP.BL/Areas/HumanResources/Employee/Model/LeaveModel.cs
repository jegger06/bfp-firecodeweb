using System.Collections.Generic;
using EBFP.BL.Helper;
using EBFP.DataAccess;
using EBFP.Helper;

namespace EBFP.BL.HumanResources
{
    public class LeaveListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<LeaveModel> LeaveListModel { get; set; }
    }

    public class LeaveModel
    {
        public string sELR_Id
        {
            get
            {
                var encryptID = this.ELR_Id.ToString().Encrypt();
                return encryptID;
            }
        }

        public int ELR_Id { get; set; }
        public int ELR_Emp_Id { get; set; }
        public int ELR_LeaveType { get; set; }
        public string ELR_LeaveType_Desc { get; set; }
        public System.DateTime ELR_StartDate { get; set; }
        public System.DateTime ELR_EndDate { get; set; }
        public decimal ELR_TotalDays { get; set; }
        public bool ELR_WithPay { get; set; }
        public string ELR_Remarks { get; set; }
        public System.DateTime ELR_InputDate { get; set; }
        public int ELR_Input_Emp_Id { get; set; }
        public int ELR_ChargeTo { get; set; }

        public string Emp_Number { get; set; }
        public string Emp_FirstName { get; set; }
        public string Emp_MiddleName { get; set; }
        public string Emp_LastName { get; set; }
        public string Emp_Rank { get; set; }
    }

    public class LeaveCreditsModel
    {
        public double EarnedVacationLeave { get; set; }
        public double EarnedSickLeave { get; set; }
        public double EnjoyedVacationLeave { get; set; }
        public double EnjoyedSickLeave { get; set; }
        public double RemainingVacationLeave { get; set; }
        public double RemainingSickLeave { get; set; }

        public double TotalEarnedLeave => (EarnedVacationLeave + EarnedSickLeave);
        public double TotalEnjoyedLeave => (EnjoyedVacationLeave + EnjoyedSickLeave);
        public double TotalRemainingLeave => (RemainingVacationLeave + RemainingSickLeave);
    }
}
