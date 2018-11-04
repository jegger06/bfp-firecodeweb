
namespace EBFP.BL.InspectionOrder
{
    using EBFP.BL.Helper;
    using EBFP.DataAccess;
    using System;

    public class InspectionOrderUnitOfWork : EntityFrameworkBase, IInspectionOrderUnitOfWork, IDisposable
    {
        public InspectionOrderUnitOfWork(EBFPEntities _context)
        {
            base.context_ = _context;
        }

        public InspectionOrderUnitOfWork()
        {

        }

        private IInspectionOrder _InspectionOrder;

        public IInspectionOrder InspectionOrder
        {
            get { return _InspectionOrder ?? (_InspectionOrder = new InspectionOrderBL(context)); }
        }

        private IInspectionOrderInspectors _InspectionOrderInspectors;

        public IInspectionOrderInspectors InspectionOrderInspectors
        {
            get { return _InspectionOrderInspectors ?? (_InspectionOrderInspectors = new InspectionOrderInspectorsBL(context)); }
        }

        private INoticeToComply _NoticeToComply;

        public INoticeToComply NoticeToComply
        {
            get { return _NoticeToComply ?? (_NoticeToComply = new NoticeToComplyBL(context)); }
        }

        private INTCDeficiencies _NTCDeficiencies;

        public INTCDeficiencies NTCDeficiencies
        {
            get { return _NTCDeficiencies ?? (_NTCDeficiencies = new NTCDeficienciesBL(context)); }
        }

        private INTCV _NTCV;

        public INTCV NTCV
        {
            get { return _NTCV ?? (_NTCV = new NTCVBL(context)); }
        }

        private INTCVDeficiencies _NTCVDeficiencies;

        public INTCVDeficiencies NTCVDeficiencies
        {
            get { return _NTCVDeficiencies ?? (_NTCVDeficiencies = new NTCVDeficienciesBL(context)); }
        }

        private IInspectionOrderViolations _InspectionOrderViolations;

        public IInspectionOrderViolations InspectionOrderViolations
        {
            get { return _InspectionOrderViolations ?? (_InspectionOrderViolations = new InspectionOrderViolationsBL(context)); }
        }

        private IOtherViolations _OtherViolations;
        public IOtherViolations OtherViolations
        {
            get { return _OtherViolations ?? (_OtherViolations = new OtherViolationBL(context)); }
        }

        private IAbatementOrder _AbatementOrder;
        public IAbatementOrder AbatementOrder
        {
            get { return _AbatementOrder ?? (_AbatementOrder = new AbatementOrderBL(context)); }
        }

        private IAbatementOrderDeficiencies _AbatementOrderDeficiencies;
        public IAbatementOrderDeficiencies AbatementOrderDeficiencies
        {
            get { return _AbatementOrderDeficiencies ?? (_AbatementOrderDeficiencies = new AbatementOrderDeficienciesBL(context)); }
        }

        private IClosureOrder _ClosureOrder;
        public IClosureOrder ClosureOrder
        {
            get { return _ClosureOrder ?? (_ClosureOrder = new ClosureOrderBL(context)); }
        }

        private IClosureOrderDeficiencies _ClosureOrderDeficiencies;
        public IClosureOrderDeficiencies ClosureOrderDeficiencies
        {
            get { return _ClosureOrderDeficiencies ?? (_ClosureOrderDeficiencies = new ClosureOrderDeficienciesBL(context)); }
        }

        private IViolations _Violations;
        public IViolations Violations
        {
            get { return _Violations ?? (_Violations = new ViolationBL(context)); }
        }

        public void Complete()
        {
            context.SaveChanges();
        }
    }

    public interface IInspectionOrderUnitOfWork
    {
        INTCV NTCV { get; }
        IInspectionOrder InspectionOrder { get; }
        IInspectionOrderInspectors InspectionOrderInspectors { get; }
        INoticeToComply NoticeToComply { get; }
        INTCDeficiencies NTCDeficiencies { get; }
        INTCVDeficiencies NTCVDeficiencies { get; }
        IInspectionOrderViolations InspectionOrderViolations { get; }
        IOtherViolations OtherViolations { get; }
        IAbatementOrder AbatementOrder { get; }
        IAbatementOrderDeficiencies AbatementOrderDeficiencies { get; }
        IClosureOrder ClosureOrder { get; }
        IClosureOrderDeficiencies ClosureOrderDeficiencies { get; }
        IViolations Violations { get; }
        void Complete();
    }
}
