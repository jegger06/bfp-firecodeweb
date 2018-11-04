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
    
    public partial class tblEmployees
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblEmployees()
        {
            this.tblAfterFireOperationPersonnelsOnDuty = new HashSet<tblAfterFireOperationPersonnelsOnDuty>();
            this.tblAfterFireOperations = new HashSet<tblAfterFireOperations>();
            this.tblEmployeeAppointments = new HashSet<tblEmployeeAppointments>();
            this.tblEmployeeAppointments1 = new HashSet<tblEmployeeAppointments>();
            this.tblEmployeeAppointments2 = new HashSet<tblEmployeeAppointments>();
            this.tblEmployeeChildren = new HashSet<tblEmployeeChildren>();
            this.tblEmployeeCivilServiceEligibilities = new HashSet<tblEmployeeCivilServiceEligibilities>();
            this.tblEmployeeEducationalBackground = new HashSet<tblEmployeeEducationalBackground>();
            this.tblEmployeeLeaveRecords = new HashSet<tblEmployeeLeaveRecords>();
            this.tblEmployeeLeaveRecords1 = new HashSet<tblEmployeeLeaveRecords>();
            this.tblEmployeeMembershipInAssociationOrganizations = new HashSet<tblEmployeeMembershipInAssociationOrganizations>();
            this.tblEmployeeNonAcademicDistinctions = new HashSet<tblEmployeeNonAcademicDistinctions>();
            this.tblEmployeeOtherInformation = new HashSet<tblEmployeeOtherInformation>();
            this.tblEmployeeReferences = new HashSet<tblEmployeeReferences>();
            this.tblEmployeeSpecialSkillsHobbies = new HashSet<tblEmployeeSpecialSkillsHobbies>();
            this.tblEmployeeVoluntaryWorks = new HashSet<tblEmployeeVoluntaryWorks>();
            this.tblEmployeeWorkExperiences = new HashSet<tblEmployeeWorkExperiences>();
            this.tblLogisticAssets = new HashSet<tblLogisticAssets>();
            this.tblPhysicalInventory = new HashSet<tblPhysicalInventory>();
            this.tblUnitSubStation = new HashSet<tblUnitSubStation>();
            this.tblUnitsUserInRole = new HashSet<tblUnitsUserInRole>();
            this.tblUserInRole = new HashSet<tblUserInRole>();
            this.tblUnits1 = new HashSet<tblUnits>();
            this.tblEmployeeServiceAppointments = new HashSet<tblEmployeeServiceAppointments>();
            this.tblEmployeeTrainingPrograms = new HashSet<tblEmployeeTrainingPrograms>();
        }
    
        public int Emp_Id { get; set; }
        public string Emp_Number { get; set; }
        public string Emp_FirstName { get; set; }
        public string Emp_MiddleName { get; set; }
        public string Emp_LastName { get; set; }
        public string Emp_SuffixName { get; set; }
        public Nullable<System.DateTime> Emp_BirthDate { get; set; }
        public string Emp_BirthPlace { get; set; }
        public string Emp_Gender { get; set; }
        public Nullable<int> Emp_CivilStatus { get; set; }
        public string Emp_Citizenship { get; set; }
        public string Emp_Citizenship_Dual { get; set; }
        public string Emp_Citizenship_Country { get; set; }
        public string Emp_Height { get; set; }
        public string Emp_Weight { get; set; }
        public string Emp_BloodType { get; set; }
        public string Emp_GSISNumber { get; set; }
        public string Emp_PAGIBIGNumber { get; set; }
        public string Emp_PHICNumber { get; set; }
        public string Emp_SSSNumber { get; set; }
        public string Emp_Residential_Address { get; set; }
        public string Emp_Residential_HouseNo { get; set; }
        public string Emp_Residential_Street { get; set; }
        public string Emp_Residential_Village { get; set; }
        public string Emp_Residential_Barangay { get; set; }
        public string Emp_Residential_Municipality { get; set; }
        public string Emp_Residential_Province { get; set; }
        public string Emp_Residential_ZipCode { get; set; }
        public string Emp_Residential_PhoneNumber { get; set; }
        public string Emp_Permanent_Address { get; set; }
        public string Emp_Permanent_HouseNo { get; set; }
        public string Emp_Permanent_Street { get; set; }
        public string Emp_Permanent_Village { get; set; }
        public string Emp_Permanent_Barangay { get; set; }
        public string Emp_Permanent_Municipality { get; set; }
        public string Emp_Permanent_Province { get; set; }
        public string Emp_Permanent_ZipCode { get; set; }
        public string Emp_Permanent_PhoneNumber { get; set; }
        public string Emp_EmailAddress { get; set; }
        public string Emp_MobileNumber { get; set; }
        public string Emp_AgencyEmpNumber { get; set; }
        public string Emp_TINNumber { get; set; }
        public string Emp_Spouse_FirstName { get; set; }
        public string Emp_Spouse_MiddleName { get; set; }
        public string Emp_Spouse_LastName { get; set; }
        public string Emp_Spouse_SuffixName { get; set; }
        public string Emp_Spouse_Occupation { get; set; }
        public string Emp_Spouse_EmpBusName { get; set; }
        public string Emp_Spouse_EmpBusAddress { get; set; }
        public string Emp_Spouse_EmpBusPhoneNumber { get; set; }
        public string Emp_Father_FirstName { get; set; }
        public string Emp_Father_MiddleName { get; set; }
        public string Emp_Father_LastName { get; set; }
        public string Emp_Father_SuffixName { get; set; }
        public string Emp_Mother_MaidenName { get; set; }
        public string Emp_Mother_FirstName { get; set; }
        public string Emp_Mother_MiddleName { get; set; }
        public string Emp_Mother_LastName { get; set; }
        public string Emp_ItemNumber { get; set; }
        public string Emp_BadgeNumber { get; set; }
        public Nullable<System.DateTime> Emp_Retired_Date { get; set; }
        public Nullable<System.DateTime> Emp_Service_StartDate { get; set; }
        public Nullable<System.DateTime> Emp_Service_UniformGovtStartDate { get; set; }
        public Nullable<System.DateTime> Emp_Service_GovtStartDate { get; set; }
        public Nullable<System.DateTime> Emp_LastPromotionDate_Temp { get; set; }
        public Nullable<System.DateTime> Emp_LastPromotionDate_Permanent { get; set; }
        public Nullable<System.DateTime> Emp_AssumedOfficerDate { get; set; }
        public Nullable<System.DateTime> Emp_LastTrainingDate { get; set; }
        public Nullable<int> Emp_EducCourse { get; set; }
        public Nullable<int> Emp_HighestEducAttainment { get; set; }
        public Nullable<int> Emp_HighestMandatoryTraining { get; set; }
        public Nullable<int> Emp_DutyStatus { get; set; }
        public Nullable<int> Emp_Curr_ApptStatus { get; set; }
        public Nullable<int> Emp_Curr_JobFunc { get; set; }
        public string Emp_Curr_PosDesignationTitle { get; set; }
        public Nullable<int> Emp_Curr_Rank { get; set; }
        public Nullable<int> Emp_Curr_Unit { get; set; }
        public Nullable<int> Emp_Curr_Eligibility { get; set; }
        public Nullable<int> Emp_Curr_SalaryGrade { get; set; }
        public Nullable<decimal> Emp_Curr_Salary { get; set; }
        public string Emp_Username { get; set; }
        public string Emp_CivilStatus_Other { get; set; }
        public string Emp_Remarks { get; set; }
        public bool Emp_IsDeleted { get; set; }
        public bool IsSynced { get; set; }
        public Nullable<int> LocalDB_Emp_Id { get; set; }
        public Nullable<int> Emp_AccessRole { get; set; }
        public byte[] Emp_Photo { get; set; }
        public Nullable<int> Emp_Religion { get; set; }
        public string Emp_Religion_Others { get; set; }
        public Nullable<int> Emp_MACourse { get; set; }
        public Nullable<int> Emp_Eligibility_Type { get; set; }
        public Nullable<System.DateTime> Emp_DES { get; set; }
        public string Emp_BP_Number { get; set; }
        public string Emp_Tax_Code { get; set; }
        public string Emp_Atm_Number { get; set; }
        public Nullable<int> Emp_SubStation_Id { get; set; }
        public string Emp_PresentAsgmt_DO_BO_RO { get; set; }
        public string Emp_AppointmentStatus_DO_BO_RO { get; set; }
        public Nullable<bool> Emp_IsChecked { get; set; }
        public Nullable<int> Emp_CheckedBy { get; set; }
        public Nullable<System.DateTime> Emp_CheckedDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblAfterFireOperationPersonnelsOnDuty> tblAfterFireOperationPersonnelsOnDuty { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblAfterFireOperations> tblAfterFireOperations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblEmployeeAppointments> tblEmployeeAppointments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblEmployeeAppointments> tblEmployeeAppointments1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblEmployeeAppointments> tblEmployeeAppointments2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblEmployeeChildren> tblEmployeeChildren { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblEmployeeCivilServiceEligibilities> tblEmployeeCivilServiceEligibilities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblEmployeeEducationalBackground> tblEmployeeEducationalBackground { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblEmployeeLeaveRecords> tblEmployeeLeaveRecords { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblEmployeeLeaveRecords> tblEmployeeLeaveRecords1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblEmployeeMembershipInAssociationOrganizations> tblEmployeeMembershipInAssociationOrganizations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblEmployeeNonAcademicDistinctions> tblEmployeeNonAcademicDistinctions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblEmployeeOtherInformation> tblEmployeeOtherInformation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblEmployeeReferences> tblEmployeeReferences { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblEmployeeSpecialSkillsHobbies> tblEmployeeSpecialSkillsHobbies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblEmployeeVoluntaryWorks> tblEmployeeVoluntaryWorks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblEmployeeWorkExperiences> tblEmployeeWorkExperiences { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblLogisticAssets> tblLogisticAssets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPhysicalInventory> tblPhysicalInventory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblUnitSubStation> tblUnitSubStation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblUnitsUserInRole> tblUnitsUserInRole { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblUserInRole> tblUserInRole { get; set; }
        public virtual tblRanks tblRanks { get; set; }
        public virtual tblUnits tblUnits { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblUnits> tblUnits1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblEmployeeServiceAppointments> tblEmployeeServiceAppointments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblEmployeeTrainingPrograms> tblEmployeeTrainingPrograms { get; set; }
    }
}