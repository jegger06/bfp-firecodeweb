using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using EBFP.BL.Helper;
using EBFP.BL.HumanResources;
using EBFP.Helper;

namespace EBFP.BL.Inventory
{
    public class SubStationListResult
    {
        public GridInfo DatatableInfo { get; set; }
        public List<SubStationModel> SubStationList { get; set; }
    }
    public class SubStationSearchModel
    {
        public string Sub_Station_Code { get; set; }
        public string Sub_Station_Name { get; set; }
        public int Sub_Unit_Id { get; set; }
        public int Sub_BuildingOwner { get; set; }
        public int Sub_LotOwner { get; set; }
        public int Sub_LotStatus { get; set; }
        public int Sub_BuildingStatus { get; set; }
        public int Municipality_Id { get; set; }
    }

    public class SubStationModel
    {
        public SubStationModel()
        {
            this.SubStationEmployees = new List<SubStationEmployeeModel>();
        }

        public string sSub_Id
        {
            get
            {
                return Sub_Id.ToString().Encrypt();
            }
        }
       

        public int Sub_Id { get; set; }
        public int Sub_Unit_Id { get; set; }
        [Required]
        public string Sub_Station_Code { get; set; }
        [Required]
        public string Sub_Station_Name { get; set; }
        public string Sub_Street { get; set; }
        public string Sub_Barangay { get; set; }
        public string Sub_Coordinate { get; set; }
        public string Sub_PhoneNumber { get; set; }
        public string Sub_CellNumber { get; set; }
        public string Sub_Email { get; set; }
        public int? Sub_BuildingStatus { get; set; }
        public int? Sub_BuildingOwner { get; set; }
        public int? Sub_LotOwner { get; set; }
        public int? Sub_LotStatus { get; set; }
        public decimal? Sub_FloorArea { get; set; }

        private string _sSub_FloorArea = "";
        public string sSub_FloorArea
        {
            get {
                return string.IsNullOrWhiteSpace(_sSub_FloorArea) ?
                    Sub_FloorArea.ToString() : 
                    _sSub_FloorArea; }
            set
            {
                this.Sub_FloorArea = Functions.ConvertToSafeDecimal(value);
            }
        }

        public decimal? Sub_LotArea { get; set; }
        private string _sSub_LotArea = "";
        public string sSub_LotArea
        {
            get
            {
                return string.IsNullOrWhiteSpace(_sSub_LotArea) ?
                    Sub_LotArea.ToString() :
                    _sSub_LotArea;
            }
            set
            {
                this.Sub_LotArea = Functions.ConvertToSafeDecimal(value);
            }
        }

        public int? Sub_NumberStorey { get; set; }
        public DateTime? Sub_ActiviationDate { get; set; }
        public string Sub_Remarks { get; set; }
        public string Sub_FireMarshall_Position { get; set; }
        //public string Sub_FireMarshall_Name { get; set; }
        public string Sub_FireMarshall_CellNumber { get; set; }

        public int Sub_FireMarshall_EmpId { get; set; }
        public string Sub_Unit_Name { get; set; }

        public string Sub_BuildingStatus_Text => Sub_BuildingStatus != null ? ((BuildingStatus) Sub_BuildingStatus).ToDescription() : "";
        public string Sub_BuildingOwner_Text => Sub_BuildingOwner != null ? ((BuildingOwner)Sub_BuildingOwner).ToDescription() : "";
        public string Sub_LotOwner_Text => Sub_LotOwner != null ? ((LotOwner)Sub_LotOwner).ToDescription() : "";
        public string Sub_LotStatus_Text => Sub_LotStatus  != null ? ((LotStatus)Sub_LotStatus).ToDescription() : "";


        public string NSCB { get; set; }
        public string MunicipalityName { get; set; }
        public int MunicipalityId { get; set; }
        public int RegionId { get; set; }
        public int ProvinceId { get; set; }
        public bool Sub_WithBuilding { get; set; }

        public List<SubStationEmployeeModel> SubStationEmployees { get; set; }
}

    public class SelectionFireMarshallModel
    {
        public string FireMarshall_Id { get; set; }
        public string FireMarshall_Fullname { get; set; }
        public string FireMarshall_Position { get; set; }
        public string FireMarshall_ContactNumber { get; set; }
        public string FireMarshall_Designation { get; set; }
    }

    public class SubStationEmployeeModel
    {
        public int Unit_Id { get; set; }
        public int SubStation_Id { get; set; }
        public int? Emp_Id { get; set; }
        public bool toDelete { get; set; }
    }
}
