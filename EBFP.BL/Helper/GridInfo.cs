
using EBFP.BL.Administration;
using EBFP.BL.CIS;
using EBFP.BL.Establishment;
using EBFP.BL.HumanResources;
using EBFP.BL.InspectionOrder;
using EBFP.BL.Inventory;

namespace EBFP.BL.Helper
{

    public class GridInfo
    {
        public string page { get; set; }
        public int pages { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public int length { get; set; }
        public int recordsTotal { get; set; }
        public int recordsDisplay { get; set; }
        public bool serverSide { get; set; }
        public string sortOrder { get; set; }
        public string sortColumnName { get; set; }
        public string searchValue { get; set; }
        public int idValue { get; set; }
        public UserRoleSearchModel searchUserRoleModel { get; set; }
        public UserInRoleSearchModel searchUserInRoleModel { get; set; }
        public EmployeeSearchModel searchModel { get; set; }
        public UnitSearchModel searchUnitModel { get; set; }
        public MunicipalitySearchModel searchMunicipalityModel { get; set; }
        public SubStationSearchModel searchSubStationModel { get; set; }
        public StationSearchModel searchStationModel { get; set; }
        public TruckSearchModel searchTruckModel { get; set; }
        public OtherVehicleSearchModel searchOtherVehicleModel { get; set; }
        public RankSearchModel searchRankModel { get; set; }
        public SLLSearchModel searchSLLModel { get; set; }
        public EstablishmentSearchModel searchEstablishmentModel { get; set; }
        public InspectionOrderSearchModel searchInspectionOrderModel { get; set; }
        public InventoryGroupSearchModel searchInventoryGroupModel { get; set; }
        public InventoryArticleSearchModel searchInventoryArticleModel { get; set; }
        public DirectoratesSearchModel searchDirectoratesModel { get; set; }
        public PhysicalInventorySearchModel searchPhysicalInventoryModel { get; set; }
        public InventorySuppliesSearchModel searchSuppliesInventory { get; set; }
        public UnserviceableSearchModel searchUnserviceable { get; set; }
        public EmployeeAppointmentsSearchModel searchEmployeeAppointment { get; set; }
        public PersonnelSearchModel searchPersonnelModel { get; set; }
        public StationDetailsSearchModel searchStationDetailsModel { get; set; }
        public DepositSearchModel searchDepositModel { get; set; }
        public SpoiledOPSSearchModel searchSpoiledOPS { get; set; }
        public SpoiledORSearchModel searchSpoiledOR { get; set; }
        public ORSearchModel searchOR { get; set; }
    }
}
