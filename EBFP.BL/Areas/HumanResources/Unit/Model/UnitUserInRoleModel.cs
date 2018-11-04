
using System.Collections.Generic;
using EBFP.DataAccess;

namespace EBFP.BL.HumanResources
{
    public class UnitUserInRoleModel
    {
        public int Unit_UIR_ID { get; set; }
        public int Unit_UIR_Unit_Id { get; set; }
        public int Unit_UIR_Emp_Id { get; set; }
        public bool Unit_UIR_Assessor { get; set; }
        public bool Unit_UIR_Collector { get; set; }
        public bool Unit_UIR_Encoder { get; set; }
        public bool Unit_UIR_Release { get; set; }
        public bool Unit_UIR_Admin { get; set; }
        public bool Unit_UIR_Inspector { get; set; }
        public bool Unit_UIR_PlanEvaluator { get; set; }
        //public virtual tblEmployees tblEmployees { get; set; }
        //public virtual tblUnits tblUnits { get; set; }
    }
}
