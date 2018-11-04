
using EBFP.Helper;

namespace EBFP.BL.HumanResources
{
    using DataAccess;
    using Helper;
    using System;
    using System.Collections.Generic;
    using System.Web;

    public class UnitListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<UnitModel> UnitList { get; set; }
    }

    public class UnitModel
    {
        public UnitModel()
        {
            this.UnitUserInRoleModel = new List<UnitUserInRoleModel>();
        }
        public string sUnit_Id
        {
            get
            {
                var encryptID = this.Unit_Id.ToString().Encrypt();
                return encryptID;
            }
        }
        public int Unit_Id { get; set; }
        public string Unit_Code { get; set; }
        public string Unit_StationName { get; set; }
        public string Unit_Address { get; set; }
        public string Unit_PhoneNumber { get; set; }
        public string Unit_Description { get; set; }
        public int Unit_ProvDistrict { get; set; }
        public int Unit_Reg_Id { get; set; }
        public int Unit_Municipality_Id { get; set; }
        public int? Unit_FireMarshall_Emp_Id { get; set; }
        public int? Unit_Assessor_Emp_Id { get; set; }
        public int? Unit_Collector_Emp_Id { get; set; }
        public int? Unit_Encoder_Emp_Id { get; set; }
        public string Province_Name { get; set; }
        public string CityMunicipality_Name { get; set; }
        public string Reg_Description { get; set; }
        public int? Unit_Category { get; set; }
        public string Unit_Street { get; set; }
        public string Unit_Barangays { get; set; }
        public string Unit_Coordinates { get; set; }
        public string Unit_PhoneNumbers { get; set; }
        public string Unit_CellphoneNumbers { get; set; }
        public string Unit_Email { get; set; }
        public int? Unit_BuildingStatus { get; set; }
        public int? Unit_BuildingOwner { get; set; }
        public int? Unit_LotStatus { get; set; }
        public int? Unit_LotOwner { get; set; }
        public decimal? Unit_FloorArea { get; set; }
        public decimal? Unit_LotArea { get; set; }
        public int? Unit_NumberStorey { get; set; }
        public DateTime? Unit_ActivationDate { get; set; }
        public string Unit_Remarks { get; set; }
        public string Unit_FireMarshall_Position { get; set; }
        public string Unit_FireMarshall_Name { get; set; }
        public string Unit_FireMarshall_CellphoneNumber { get; set; }
        public int? Unit_ChiefFSES_Emp_Id { get; set; }
        public byte[] Unit_FireMarshall_Signature { get; set; }
        public HttpPostedFileBase FireMarshallImage { get; set; }
        public byte[] Unit_ChiefFSES_Signature { get; set; }
        public HttpPostedFileBase ChiefFSESImage { get; set; }
        public string Unit_CategoryName
        {
            get
            {
                return Unit_Category == (int) StationCategory.Station ? "Station"
                     : Unit_Category == (int) StationCategory.Office ? "Office" : "";
            }
        }

        public List<UnitUserInRoleModel> UnitUserInRoleModel { get; set; }
    }
    public partial class SelectionUnitModel
    {
        public SelectionUnitModel()
        {
            Units = new List<UnitModel>();
        }
        public int Reg_Id { get; set; }
        public string Reg_Description { get; set; }
        public List<UnitModel> Units { get; set; }
    }


    public class PositiveNegativeReportModel
    {
        public int ProvinceId { get; set; }
        public int RegionId { get; set; }
        public int UnitId { get; set; }
    }

    public class UnitSearchModel
    {
        public string UnitCode { get; set; }
        public int UnitId { get; set; }
        public int RegionId { get; set; }
        public int ProvinceId { get; set; }
        public int MunicipalityId { get; set; }
        public string StationName { get; set; }
        public string UnitDescription { get; set; }
        public int FireMashallId { get; set; }
        public int UnitCategory { get; set; }
        public bool IsSearch { get; set; }
    }
}
