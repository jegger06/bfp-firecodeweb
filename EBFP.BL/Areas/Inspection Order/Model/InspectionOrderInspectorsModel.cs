namespace EBFP.BL.InspectionOrder
{
    public class InspectionOrderInspectorsModel
    {
        public int Insp_Id { get; set; }
        public string Ref_Insp_Id { get; set; }
        public string Insp_IO_Id { get; set; }
        public int Insp_Emp_Id { get; set; }
        public int Insp_Unit_Id { get; set; }
        public bool IsSynced { get; set; }
        public string Insp_Emp_Name { get; set; }
    }
}