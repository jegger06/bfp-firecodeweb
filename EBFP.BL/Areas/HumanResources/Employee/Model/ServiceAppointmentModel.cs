

namespace EBFP.BL.HumanResources
{
    using Helper;
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class ServiceAppointmentModel
    {
        public int ESA_Id { get; set; }
        public string ESA_PosDesignation { get; set; }
        public int ESA_Rank { get; set; }
        public int ESA_DutyStatus { get; set; }
        public string ESA_Appt_Status { get; set; }
        public int ESA_SalaryGrade { get; set; }
        public decimal ESA_SalaryAmt { get; set; }

        private string _sESA_SalaryAmt = "";
        [Required]
        public string sESA_SalaryAmt
        {
            get
            {
                return string.IsNullOrWhiteSpace(_sESA_SalaryAmt) ?
                    ESA_SalaryAmt.ToString() :
                    _sESA_SalaryAmt;
            }
            set
            {
                this.ESA_SalaryAmt = Functions.ConvertToSafeDecimal(value);
            }
        }
        public int ESA_JobFuncCategory { get; set; }
        public int? ESA_JobFuncSubCategory { get; set; }
        public int ESA_ApptType { get; set; }
        public DateTime? ESA_ApptDate { get; set; }
        public DateTime? ESA_EndDate { get; set; }
        public string ESA_Office_Entity { get; set; }
        public int ESA_Emp_Id { get; set; }
        public string ESA_LeaveWithoutPay { get; set; }
        public string ESA_OtherRank { get; set; }
        public string ESA_Authority { get; set; }
    }
}
