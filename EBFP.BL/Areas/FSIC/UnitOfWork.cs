
using EBFP.BL.FSIC;

namespace EBFP.BL.Administration
{
    using EBFP.BL.Helper;
    using EBFP.DataAccess;
    using System;

    // ** Facade pattern.
    // ** Repository pattern ( could be split up in individual Repositories ).
    public class UnitOfWork : EntityFrameworkBase, IUnitOfWork, IDisposable
    {

        public UnitOfWork(EBFPEntities _context)
        {
            base.context_ = _context;
        }

        public UnitOfWork()
        {

        }

        private IFireSafetyInspectionCertificate _FireSafetyInspectionCertificate;
        public IFireSafetyInspectionCertificate FireSafetyInspectionCertificate
        {
            get
            {
                return _FireSafetyInspectionCertificate ?? (_FireSafetyInspectionCertificate = new FireSafetyInspectionCertificate(context));
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

    public interface IUnitOfWork
    {
        IFireSafetyInspectionCertificate FireSafetyInspectionCertificate { get; }
        void Complete();
    }
}
