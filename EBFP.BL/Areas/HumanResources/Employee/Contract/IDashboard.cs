
using System.Collections.Generic;

namespace EBFP.BL.HumanResources
{
    public interface IDashboard
    {
        List<HRISChartModel> GetDashboardDetails();
        DashboardCounterTotal GetHRISDashboardCounter();
        List<DutyStatusChartModel> GetDutyStatusChartDetails();
    }
}