//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EBFP.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblAfterFireOperations
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblAfterFireOperations()
        {
            this.tblAfterFireOperationBreathingApparatusUsed = new HashSet<tblAfterFireOperationBreathingApparatusUsed>();
            this.tblAfterFireOperationDispatchedVehicles = new HashSet<tblAfterFireOperationDispatchedVehicles>();
            this.tblAfterFireOperationExtinguishingAgentsUsed = new HashSet<tblAfterFireOperationExtinguishingAgentsUsed>();
            this.tblAfterFireOperationHoseLineUsed = new HashSet<tblAfterFireOperationHoseLineUsed>();
            this.tblAfterFireOperationOtherToolsUsed = new HashSet<tblAfterFireOperationOtherToolsUsed>();
            this.tblAfterFireOperationPersonnelsOnDuty = new HashSet<tblAfterFireOperationPersonnelsOnDuty>();
            this.tblAfterFireOperationRopesLadderUsed = new HashSet<tblAfterFireOperationRopesLadderUsed>();
        }
    
        public int AFO_Id { get; set; }
        public int AFO_FI_Id { get; set; }
        public int AFO_Unit_Id { get; set; }
        public System.DateTime AFO_FireIncident_Date { get; set; }
        public string AFO_FireIncident_Address { get; set; }
        public System.DateTime AFO_AlarmReceived_Date { get; set; }
        public string AFO_AlarmReceived_Address { get; set; }
        public string AFO_Informant_Name { get; set; }
        public string AFO_Informant_Address { get; set; }
        public Nullable<int> AFO_AlarmReceived_Officer { get; set; }
        public string AFO_AlarmReceived_Remarks { get; set; }
        public string AFO_Classification_Remarks { get; set; }
        public int AFO_Classification { get; set; }
        public int AFO_Motives { get; set; }
        public string AFO_StructureGeneralDescription { get; set; }
        public string AFO_DistanceTravelled { get; set; }
        public string AFO_ServiceBy { get; set; }
        public Nullable<System.DateTime> AFO_LastServiceDate { get; set; }
        public string AFO_Instruction { get; set; }
        public string AFO_FireOperationStory { get; set; }
        public string AFO_ProblemsEncountered { get; set; }
        public string AFO_ObservationsRecommendations { get; set; }
        public int AFO_Created_Emp_Id { get; set; }
        public System.DateTime AFO_CreatedDate { get; set; }
        public Nullable<int> AFO_LastUpdate_Emp_Id { get; set; }
        public Nullable<System.DateTime> AFO_LastUpdateDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblAfterFireOperationBreathingApparatusUsed> tblAfterFireOperationBreathingApparatusUsed { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblAfterFireOperationDispatchedVehicles> tblAfterFireOperationDispatchedVehicles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblAfterFireOperationExtinguishingAgentsUsed> tblAfterFireOperationExtinguishingAgentsUsed { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblAfterFireOperationHoseLineUsed> tblAfterFireOperationHoseLineUsed { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblAfterFireOperationOtherToolsUsed> tblAfterFireOperationOtherToolsUsed { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblAfterFireOperationPersonnelsOnDuty> tblAfterFireOperationPersonnelsOnDuty { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblAfterFireOperationRopesLadderUsed> tblAfterFireOperationRopesLadderUsed { get; set; }
        public virtual tblEmployees tblEmployees { get; set; }
        public virtual tblUnits tblUnits { get; set; }
    }
}
