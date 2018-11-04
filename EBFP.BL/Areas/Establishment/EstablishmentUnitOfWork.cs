
namespace EBFP.BL.Establishment
{
    using EBFP.BL.Helper;
    using EBFP.DataAccess;
    using System;

    public class EstablishmentUnitOfWork : EntityFrameworkBase, IEstablishmentUnitOfWork, IDisposable
    {
        public EstablishmentUnitOfWork(EBFPEntities _context)
        {
            base.context_ = _context;
        }

        public EstablishmentUnitOfWork()
        {

        }

        private IEstablishment _Establishment;

        public IEstablishment Establishment
        {
            get { return _Establishment ?? (_Establishment = new EstablishmentBL(context)); }
        }

        private INonCommercialEstablishment _NonCommercialEstablishment;

        public INonCommercialEstablishment NonCommercialEstablishment
        {
            get { return _NonCommercialEstablishment ?? (_NonCommercialEstablishment = new NonCommercialEstablishmentBL(context)); }
        }

        private IOtherClearances _OtherClearances;
        public IOtherClearances OtherClearances
        {
            get
            {
                return _OtherClearances ?? (_OtherClearances = new OtherClearancesBL(context));
            }
        }

        public void Complete()
        {
            context.SaveChanges();
        }
    }


    public interface IEstablishmentUnitOfWork
    {
        IEstablishment Establishment { get; }
        INonCommercialEstablishment NonCommercialEstablishment { get; }
        IOtherClearances OtherClearances { get; }
        void Complete();
    }
}
