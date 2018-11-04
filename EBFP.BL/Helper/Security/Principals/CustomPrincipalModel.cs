using EBFP.BL.HumanResources;

namespace EBFP.Helper
{
    public class CustomPrincipalModel
    {
        public CustomPrincipalModel()
        {

        }

        public CustomPrincipalModel(EmployeeModel oModel)
        {

            UserId = oModel.Emp_Id;
            FirstName = oModel.Emp_FirstName;
            LastName = oModel.Emp_LastName;
            EmployeeUnitId = oModel.Emp_Curr_Unit ?? 0;
            RoleID = oModel.RoleID;
            RegionID = oModel.RegionID;
            ProvinceID = oModel.ProvinceID;
            MunicipalityID = oModel.MunicipalityID;
            RankName = oModel.Emp_Rank_Txt;
            RoleName = oModel.RoleName;
            Impersonate = oModel.Impersonate;
        }

        public int? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string RankName { get; set; }
        public int EmployeeUnitId { get; set; }
        public int MunicipalityID { get; set; }
        public int ProvinceID { get; set; }
        public int RegionID { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public bool Impersonate { get; set; }
    } 
}
