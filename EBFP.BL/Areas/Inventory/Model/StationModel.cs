using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using EBFP.BL.Helper;
using EBFP.BL.HumanResources;
using EBFP.Helper;

namespace EBFP.BL.Inventory
{
    public class StationListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<StationModel> StationList { get; set; }
    }

    public class StationSearchModel
    {
        public string Unit_Code { get; set; }
        public string Unit_StationName { get; set; }
        public int Unit_Category { get; set; }
        public int Unit_Id { get; set; }
        public int Unit_BuildingOwner { get; set; }
        public int Unit_LotOwner { get; set; }
        public int Unit_LotStatus { get; set; }
        public int Unit_BuildingStatus { get; set; }
        public int Municipality_Id { get; set; }
    }

    public class StationModel
    {
        public string sUnit_Id
        {
            get
            {
                return Unit_Id.ToString().Encrypt();
            }
        }

        public int Unit_Id { get; set; }
        [Required]
        public string Unit_Code { get; set; }
        [Required]
        public string Unit_StationName { get; set; }
        public string Unit_Address { get; set; }
        public string Unit_PhoneNumber { get; set; }
        public string Unit_Description { get; set; }
        public int Unit_ProvDistrict { get; set; }
        public int Unit_Reg_Id { get; set; }
        public int Unit_Municipality_Id { get; set; }
        public Nullable<int> Unit_FireMarshall_Emp_Id { get; set; }
        public Nullable<int> Unit_Assessor_Emp_Id { get; set; }
        public Nullable<int> Unit_Collector_Emp_Id { get; set; }
        public Nullable<int> Unit_Encoder_Emp_Id { get; set; }
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
        public Nullable<int> Unit_BuildingStatus { get; set; }
        public Nullable<int> Unit_BuildingOwner { get; set; }
        public Nullable<int> Unit_LotStatus { get; set; }
        public Nullable<int> Unit_LotOwner { get; set; }
        public Nullable<decimal> Unit_FloorArea { get; set; }

        private string _sUnit_FloorArea = "";
        public string sUnit_FloorArea
        {
            get
            {
                return string.IsNullOrWhiteSpace(_sUnit_FloorArea) ?
                    Unit_FloorArea.ToString() :
                    _sUnit_FloorArea;
            }
            set
            {
                this.Unit_FloorArea = Functions.ConvertToSafeDecimal(value);
            }
        }

        public Nullable<decimal> Unit_LotArea { get; set; }
        private string _sUnit_LotArea = "";
        public string sUnit_LotArea
        {
            get
            {
                return string.IsNullOrWhiteSpace(_sUnit_LotArea) ?
                    Unit_LotArea.ToString() :
                    _sUnit_LotArea;
            }
            set
            {
                this.Unit_LotArea = Functions.ConvertToSafeDecimal(value);
            }
        }
        public Nullable<int> Unit_NumberStorey { get; set; }
        public Nullable<System.DateTime> Unit_ActivationDate { get; set; }
        public string Unit_Remarks { get; set; }
        public string Unit_FireMarshall_Position { get; set; }
        public string Unit_FireMarshall_Name { get; set; }
        public string Unit_FireMarshall_CellphoneNumber { get; set; }

        public string Unit_CategoryName
        {
            get
            {
                return Unit_Category == (int)StationCategory.Station ? "Station"
                     : Unit_Category == (int)StationCategory.Office ? "Office" : "";
            }
        }

        public string Unit_BuildingStatus_Text => Unit_BuildingStatus != null ? ((BuildingStatus)Unit_BuildingStatus).ToDescription() : "";
        public string Unit_BuildingOwner_Text => Unit_BuildingOwner != null ? ((BuildingOwner)Unit_BuildingOwner).ToDescription() : "";
        public string Unit_LotOwner_Text => Unit_LotOwner != null ? ((LotOwner)Unit_LotOwner).ToDescription() : "";
        public string Unit_LotStatus_Text => Unit_LotStatus != null ? ((LotStatus)Unit_LotStatus).ToDescription() : "";


        public string NSCB { get; set; }
        public string MunicipalityName { get; set; }
        public int RegionId { get; set; }
        public int ProvinceId { get; set; }
    }

    public class StationDetailsSearchModel
    {
        public int Station_Id { get; set; }
        public int? SubStation_Id { get; set; }
    }

}
