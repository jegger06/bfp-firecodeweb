﻿
using System;

namespace EBFP.BL.Establishment
{
    public class OtherClearancesModel
    {

        public int OC_Id { get; set; }
        public string Ref_OC_Id { get; set; }
        public string OC_BusinessName { get; set; }
        public string OC_BusinessAddress { get; set; }
        public string OC_OwnerName { get; set; }
        public string OC_OperationAddress { get; set; }
        public Nullable<int> OC_OccupancyType { get; set; }
        public string OC_Fw_Duration { get; set; }
        public string OC_Fw_Supervisor { get; set; }
        public string OC_LPG_InstallationName { get; set; }
        public string OC_LPG_NumberofUnits { get; set; }
        public string OC_LPG_WaterCapacity { get; set; }
        public string OC_LPG_AppliancesInstalled { get; set; }
        public DateTime? OC_LPG_DateInstalled { get; set; }
        public string OC_Hw_NatureofJob { get; set; }
        public string OC_Hw_NameofPersons { get; set; }
        public string OC_Hw_PAI { get; set; }
        public string OC_Hw_ClearanceDuration { get; set; }
        public string OC_Hw_FireWatchmen { get; set; }
        public string OC_Elec_TypeofService { get; set; }
        public string OC_Elec_ProposedLoad { get; set; }
        public string OC_Elec_LightningOutletsNumber { get; set; }
        public string OC_Elec_ElectricalEquipment { get; set; }
        public string OC_Elec_MainDisconnectingMeans { get; set; }
        public string OC_Elec_ServiceEntranceConductors { get; set; }
        public string OC_Conv_TypeOfVehicle { get; set; }
        public string OC_Conv_MotorNumber { get; set; }
        public string OC_Conv_PlateNumber { get; set; }
        public string OC_Conv_ChassisNumber { get; set; }
        public string OC_Conv_DriverName { get; set; }
        public string OC_Conv_LicenseNumber { get; set; }
        public string OC_Conv_TrailerChassisNumber { get; set; }
        public string OC_Conv_TrailerPlateNumber { get; set; }
        public string OC_Conv_Quantity { get; set; }
        public string OC_Conv_Description { get; set; }
        public string OC_Stor_KindOfLiquid { get; set; }
        public string OC_Stor_StoredVolume { get; set; }
        public string OC_FCLS_ConstructionAddress { get; set; }
        public string OC_FCLS_Constructor { get; set; }
        public int OC_Unit_Id { get; set; }
        public int OC_Created_Emp_Id { get; set; }
        public DateTime OC_CreatedDate { get; set; }
        public int OC_LastUpdate_Emp_Id { get; set; }
        public DateTime OC_LastUpdateDate { get; set; }
        public string OC_AuthorizedRepresentative { get; set; }
        public string OC_ContactNumber { get; set; }
        public bool IsSynced { get; set; }
        public string OC_Remarks { get; set; }
    }
}
