using System;
using EBFP.BL.Helper;
using EBFP.BL.Helper.Mail;
using EBFP.DataAccess;

namespace EBFP.BL.HumanResources
{
    // ** Facade pattern.
    // ** Repository pattern ( could be split up in individual Repositories ).
    public class HRISUnitOfWork : EntityFrameworkBase, IHRISUnitOfWork, IDisposable
    {
        private ICivilServiceEligibility _civilServiceEligibility;

        private ICourse _course;

        private IDashboard _Dashboard;

        private IDutyStatus _dutyStatus;

        private IReligion _Religion;

        private IEducationalBackground _educationalBackground;

        private IEligibility _eligibility;

        private IEmployee _Employee;

        private IEmployeeChildren _employeeChildren;

        private IHRISReport _HRISReport;
        private IJobFunction _jobFunction;

        private ILeave _Leave;

        private IMandatoryTraining _mandatoryTraining;

        private IMedical _Medical;

        private IMembershipInAssociationOrganization _membershipInAssociationOrganization;

        private IMunicipality _Municipality;

        private INonAcademicDistinction _nonAcademicDistinction;

        private IOtherInformation _otherInformation;

        private IPDSExport _PDSExport;

        private IPhysicalExam _PhysicalExam;

        private IProvince _province;

        private IRank _rank;

        private IReference _reference;

        private IRegion _region;
        private IReport _Report;

        private ISalaryGrade _salaryGrade;

        private IServiceRecord _ServiceRecord;

        private ISLL _SLL;

        private ISpecialSkillsHobby _specialSkillsHobby;

        private ITrainingProgram _trainingProgram;

        private IUnit _unit;

        private IUnitUserInRole _UnitUserInRole;


        private IUser _user;
        private IDirectorates _Directorates;


        private IVoluntaryWork _voluntaryWork;

        private IWorkExperience _workExperience;

        private IEmployeeAppointments _EmployeeAppointments;
        private ILogs _Logs;

        public HRISUnitOfWork(EBFPEntities _context)
        {
            context_ = _context;
        }

        public HRISUnitOfWork()
        {
        }

        public IEmployee Employee
        {
            get { return _Employee ?? (_Employee = new EmployeeBL(context)); }
        }

        public IUnitUserInRole UnitUserInRole
        {
            get { return _UnitUserInRole ?? (_UnitUserInRole = new UnitUserInRoleBL(context)); }
        }

        public IEducationalBackground EducationalBackground
        {
            get { return _educationalBackground ?? (_educationalBackground = new EducationalBackgroundBL(context)); }
        }

        public ICivilServiceEligibility CivilServiceEligibility
        {
            get
            {
                return _civilServiceEligibility ?? (_civilServiceEligibility = new CivilServiceEligibilityBL(context));
            }
        }

        public IOtherInformation OtherInformation
        {
            get { return _otherInformation ?? (_otherInformation = new OtherInformationBL(context)); }
        }

        public ITrainingProgram TrainingProgram
        {
            get { return _trainingProgram ?? (_trainingProgram = new TrainingProgramBL(context)); }
        }

        public IVoluntaryWork VoluntaryWork
        {
            get { return _voluntaryWork ?? (_voluntaryWork = new VoluntaryWorkBL(context)); }
        }

        public IWorkExperience WorkExperience
        {
            get { return _workExperience ?? (_workExperience = new WorkExperienceBL(context)); }
        }

        public ISpecialSkillsHobby SpecialSkillsHobby
        {
            get { return _specialSkillsHobby ?? (_specialSkillsHobby = new SpecialSkillsHobbyBL(context)); }
        }

        public INonAcademicDistinction NonAcademicDistinction
        {
            get { return _nonAcademicDistinction ?? (_nonAcademicDistinction = new NonAcademicDistinctionBL(context)); }
        }

        public IMembershipInAssociationOrganization MembershipInAssociationOrganization
        {
            get
            {
                return _membershipInAssociationOrganization ??
                       (_membershipInAssociationOrganization = new MembershipInAssociationOrganizationBL(context));
            }
        }

        public ICourse Course
        {
            get { return _course ?? (_course = new CourseBL(context)); }
        }

        public IDutyStatus DutyStatus
        {
            get { return _dutyStatus ?? (_dutyStatus = new DutyStatusBL(context)); }
        }

        public IEligibility Eligibility
        {
            get { return _eligibility ?? (_eligibility = new EligibilityBL(context)); }
        }

        public IEmployeeChildren EmployeeChildren
        {
            get { return _employeeChildren ?? (_employeeChildren = new EmployeeChildrenBL(context)); }
        }

        public IJobFunction JobFunction
        {
            get { return _jobFunction ?? (_jobFunction = new JobFunctionBL(context)); }
        }

        public IMandatoryTraining MandatoryTraining
        {
            get { return _mandatoryTraining ?? (_mandatoryTraining = new MandatoryTrainingBL(context)); }
        }

        public IRank Rank
        {
            get { return _rank ?? (_rank = new RankBL(context)); }
        }

        public ISalaryGrade SalaryGrade
        {
            get { return _salaryGrade ?? (_salaryGrade = new SalaryGradeBL(context)); }
        }

        public IUnit Unit
        {
            get { return _unit ?? (_unit = new UnitBL(context)); }
        }

        public IUser User
        {
            get { return _user ?? (_user = new UserBL(context)); }
        }

        public IReference Reference
        {
            get { return _reference ?? (_reference = new ReferenceBL(context)); }
        }

        public IRegion Region
        {
            get { return _region ?? (_region = new RegionBL(context)); }
        }

        public IProvince Province
        {
            get { return _province ?? (_province = new ProvinceBL(context)); }
        }

        public IPhysicalExam PhysicalExam
        {
            get { return _PhysicalExam ?? (_PhysicalExam = new PhysicalExamBL(context)); }
        }

        public IMedical Medical
        {
            get { return _Medical ?? (_Medical = new MedicalBL(context)); }
        }

        public ILeave Leave
        {
            get { return _Leave ?? (_Leave = new LeaveBL(context)); }
        }

        public IPDSExport PDSExport
        {
            get { return _PDSExport ?? (_PDSExport = new PDSExport(context)); }
        }

        public IReport Report
        {
            get { return _Report ?? (_Report = new ReportBL(context)); }
        }

        public IMunicipality Municipality
        {
            get { return _Municipality ?? (_Municipality = new MunicipalityBL(context)); }
        }

        public IServiceRecord ServiceRecord
        {
            get { return _ServiceRecord ?? (_ServiceRecord = new ServiceRecordBL(context)); }
        }

        public IHRISReport HRISReport
        {
            get { return _HRISReport ?? (_HRISReport = new HRISReport(context)); }
        }

        public ISLL SLL
        {
            get { return _SLL ?? (_SLL = new SLLBL(context)); }
        }

        public IDashboard Dashboard
        {
            get { return _Dashboard ?? (_Dashboard = new DashboardBL(context)); }
        }

        public IDirectorates Directorates
        {
            get { return _Directorates ?? (_Directorates = new DirectoratesBL(context)); }
        }

        public IEmployeeAppointments EmployeeAppointments
        {
            get { return _EmployeeAppointments ?? (_EmployeeAppointments = new EmployeeAppointmentsBL(context)); }
        }

        public IReligion Religion
        {
            get { return _Religion ?? (_Religion = new ReligionBL(context)); }
        }

        public ILogs Logs
        {
            get { return _Logs ?? (_Logs = new LogsBL(context)); }
        }
        private IHRISUpload _HRISUpload;
        public IHRISUpload HRISUpload
        {
            get
            {
                return _HRISUpload ?? (_HRISUpload = new HRISUpload(context));
            }
        }

        private IMail _IMail;
        public IMail Mail
        {
            get { return _IMail ?? (_IMail = new Mail(context)); }
        }

        private ISpecifyDesignation _ISpecifyDesignation;
        public ISpecifyDesignation SpecifyDesignation
        {
            get { return _ISpecifyDesignation ?? (_ISpecifyDesignation = new SpecifyDesignationBL(context)); }
        }

        public void Complete()
        {
            context.SaveChanges();
        }


        //public void Dispose()
        //{
        //    context.Dispose();
        //    Functions.ResetStaticVariables();
        //}

        //private bool disposedValue = false; // To detect redundant calls 
        //~HRISUnitOfWork()
        //{
        //    if (!disposedValue)
        //    {
        //        context.Dispose();
        //        Functions.ResetStaticVariables();
        //        disposedValue = true;
        //    }
        //}

        #region IDisposable Support

        private bool disposedValue; // To detect redundant calls 

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                    Functions.ResetStaticVariables();
                }

                disposedValue = true;
            }
        }

        //~HRISUnitOfWork()
        //{
        //    Dispose(true);
        //}

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }

    public interface IHRISUnitOfWork
    {
        ICivilServiceEligibility CivilServiceEligibility { get; }
        IEducationalBackground EducationalBackground { get; }
        IEmployee Employee { get; }
        IMembershipInAssociationOrganization MembershipInAssociationOrganization { get; }
        INonAcademicDistinction NonAcademicDistinction { get; }
        IOtherInformation OtherInformation { get; }
        ISpecialSkillsHobby SpecialSkillsHobby { get; }
        ITrainingProgram TrainingProgram { get; }
        IVoluntaryWork VoluntaryWork { get; }
        IWorkExperience WorkExperience { get; }
        ICourse Course { get; }
        IDutyStatus DutyStatus { get; }
        IEligibility Eligibility { get; }
        IEmployeeChildren EmployeeChildren { get; }
        IJobFunction JobFunction { get; }
        IMandatoryTraining MandatoryTraining { get; }
        IRank Rank { get; }
        ISalaryGrade SalaryGrade { get; }
        IUnit Unit { get; }
        IUser User { get; }
        IReference Reference { get; }
        IRegion Region { get; }
        IProvince Province { get; }
        IUnitUserInRole UnitUserInRole { get; }
        IPhysicalExam PhysicalExam { get; }
        IMedical Medical { get; }
        ILeave Leave { get; }
        IPDSExport PDSExport { get; }
        IMunicipality Municipality { get; }
        IServiceRecord ServiceRecord { get; }
        IHRISReport HRISReport { get; }
        IReport Report { get; }
        IDashboard Dashboard { get; }
        ISLL SLL { get; }
        IDirectorates Directorates { get; }
        IEmployeeAppointments EmployeeAppointments { get; }
        ILogs Logs { get; }
        IReligion Religion { get; }
        IHRISUpload HRISUpload { get; }
        IMail Mail { get; }
        ISpecifyDesignation SpecifyDesignation { get; }
        void Complete();
    }
}