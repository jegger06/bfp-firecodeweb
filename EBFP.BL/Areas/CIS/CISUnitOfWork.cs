using EBFP.BL.Helper;
using EBFP.DataAccess;
using System;
using System.Data.Entity;

namespace EBFP.BL.CIS
{
    public class CISUnitOfWork : EntityFrameworkBase, ICISUnitOfWork, IDisposable
    {
        private IInventoryArticle _InventoryArticle;

        private IInventoryGroup _InventoryGroup;

        private IPhysicalInventory _PhysicalInventory;

        public CISUnitOfWork(EBFPEntities _context)
        {
            context_ = _context;
        }

        public CISUnitOfWork()
        {
        }

        public IInventoryGroup InventoryGroup
        {
            get { return _InventoryGroup ?? (_InventoryGroup = new InventoryGroupBL(context)); }
        }

        public IInventoryArticle InventoryArticle
        {
            get { return _InventoryArticle ?? (_InventoryArticle = new InventoryArticleBL(context)); }
        }

        public IPhysicalInventory PhysicalInventory
        {
            get { return _PhysicalInventory ?? (_PhysicalInventory = new PhysicalInventoryBL(context)); }
        }

        private IInventorySupplies _InventorySupplies;
        public IInventorySupplies InventorySupplies
        {
            get
            {
                return _InventorySupplies ?? (_InventorySupplies = new InventorySuppliesBL(context));
            }
        }

        private IInventoryOutSupplies _InventoryOutSupplies;
        public IInventoryOutSupplies InventoryOutSupplies
        {
            get
            {
                return _InventoryOutSupplies ?? (_InventoryOutSupplies = new InventoryOutSuppliesBL(context));
            }
        }

        private IUnserviceable _Unserviceable;
        public IUnserviceable Unserviceable
        {
            get
            {
                return _Unserviceable ?? (_Unserviceable = new UnserviceableBL(context));
            }
        }


        private ICISReport _CISReport;
        public ICISReport CISReport
        {
            get
            {
                return _CISReport ?? (_CISReport = new CISReport(context));
            }
        }

        private IReport _Report;
        public IReport Report
        {
            get
            {
                return _Report ?? (_Report = new ReportBL(context));
            }
        }

        private IDashboard _Dashboard;
        public IDashboard Dashboard
        {
            get
            {
                return _Dashboard ?? (_Dashboard = new DashboardBL(context));
            }
        }


        public void Complete()
        {
            context.SaveChanges();
        }

    }

    public interface ICISUnitOfWork
    {
        IInventoryGroup InventoryGroup { get; }
        IInventoryArticle InventoryArticle { get; }
        IPhysicalInventory PhysicalInventory { get; }
        IInventorySupplies InventorySupplies { get; }
        IInventoryOutSupplies InventoryOutSupplies { get; }
        IUnserviceable Unserviceable { get; }
        ICISReport CISReport { get; }
        IReport Report { get; }
        IDashboard Dashboard { get; }
        void Complete();
    }
}
