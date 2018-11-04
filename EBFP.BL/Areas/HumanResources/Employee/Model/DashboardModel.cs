namespace EBFP.BL.HumanResources
{
    public class HRISChartModel
    {
        public string RankName { get; set; }
        public int FemaleCount { get; set; }
        public int MaleCount { get; set; }
    }
 
    public class DashboardCounterTotal
    {
        public int Unit { get; set; }
        public int TotalStrength { get; set; }
        public int Female { get; set; }
        public int Male { get; set; }
    }


    public class DutyStatusChartModel
    {
        public string Duty_Status { get; set; }
        public int DutyStatusCount { get; set; }
    }

}