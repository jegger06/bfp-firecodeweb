
namespace EBFP.BL.Administration
{
    using EBFP.BL.Helper;
    using EBFP.DataAccess;
    using System;

    // ** Facade pattern.
    // ** Repository pattern ( could be split up in individual Repositories ).
    public class AdministrationUnitOfWork : EntityFrameworkBase, IAdministrationUnitOfWork, IDisposable
    {

        public AdministrationUnitOfWork(EBFPEntities _context)
        {
            base.context_ = _context;
        }

        public AdministrationUnitOfWork()
        {

        }

        private IRoleAccess _RoleAccess;
        public IRoleAccess RoleAccess
        {
            get
            {
                return _RoleAccess ?? (_RoleAccess = new RoleAccessBL(context));
            }
        }

        private IUserInRole _UserInRole;
        public IUserInRole UserInRole
        {
            get
            {
                return _UserInRole ?? (_UserInRole = new UsernInRoleBL(context));
            }
        }

        private IUserRole _UserRole;
        public IUserRole UserRole
        {
            get
            {
                return _UserRole ?? (_UserRole = new UserRoleBL(context));
            }
        }

        private IBPLOPayment _BPLOPayments;
        public IBPLOPayment BPLOPayments
        {
            get
            {
                return _BPLOPayments ?? (_BPLOPayments = new BPLOPaymentBL(context));
            }
        }


        private IDeposits _Deposits;
        public IDeposits Deposits
        {
            get
            {
                return _Deposits ?? (_Deposits = new DepositsBL(context));
            }
        }

        private ISpoiledOPS _SpoiledOPS;
        public ISpoiledOPS SpoiledOPS
        {
            get
            {
                return _SpoiledOPS ?? (_SpoiledOPS = new SpoiledOPSBL(context));
            }
        }

        private ISpoiledOR _SpoiledOR;
        public ISpoiledOR SpoiledOR
        {
            get
            {
                return _SpoiledOR ?? (_SpoiledOR = new SpoiledORBL(context));
            }
        }

        private IOPSSeries _OPSSeries;
        public IOPSSeries OPSSeries
        {
            get
            {
                return _OPSSeries ?? (_OPSSeries = new OPSSeriesBL(context));
            }
        }

        private IORSeries _ORSeries;
        public IORSeries ORSeries
        {
            get
            {
                return _ORSeries ?? (_ORSeries = new ORSeriesBL(context));
            }
        }

        private IMembership _Membership;
        public IMembership Membership
        {
            get
            {
                return _Membership ?? (_Membership = new MembershipBL(context));
            }
        }

        private IApplicationPayment _ApplicationPayment;
        public IApplicationPayment ApplicationPayment
        {
            get
            {
                return _ApplicationPayment ?? (_ApplicationPayment = new ApplicationPaymentBL(context));
            }
        }
        public void Complete()
        {
            context.SaveChanges();
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls 
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

        //~AdministrationUnitOfWork()
        //{
        //    Dispose(true);
        //}

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion 
    }

    public interface IAdministrationUnitOfWork
    {
        IUserRole UserRole { get; }
        IUserInRole UserInRole { get; }
        IRoleAccess RoleAccess { get; }
        IBPLOPayment BPLOPayments { get; }
        IDeposits Deposits { get; }
        ISpoiledOPS SpoiledOPS { get; }
        ISpoiledOR SpoiledOR { get; }
        IOPSSeries OPSSeries { get; }
        IORSeries ORSeries { get; }
        IMembership Membership { get; }
        IApplicationPayment ApplicationPayment { get; }
        void Complete();
    }
}
