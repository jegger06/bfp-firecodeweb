
namespace EBFP.BL.Inventory
{
    using EBFP.BL.Helper;
    using EBFP.DataAccess;
    using System;

    public class InventoryUnitOfWork : EntityFrameworkBase, IInventoryUnitOfWork, IDisposable
    {
        public InventoryUnitOfWork(EBFPEntities _context)
        {
            base.context_ = _context;
        }

        public InventoryUnitOfWork()
        {

        }
        private ISubStation _SubStation;

        public ISubStation SubStation
        {
            get { return _SubStation ?? (_SubStation = new SubStationBL(context)); }
        }

        private IStation _Station;

        public IStation Station
        {
            get { return _Station ?? (_Station = new StationBL(context)); }
        }

        private IMunicipality _IMunicipality;

        public IMunicipality Municipality
        {
            get { return _IMunicipality ?? (_IMunicipality = new MunicipalityBL(context)); }
        }

        private ITruck _ITruck;

        public ITruck Truck
        {
            get { return _ITruck ?? (_ITruck = new TruckBL(context)); }
        }

        private IFireRecord _IFireRecord;

        public IFireRecord FireRecord
        {
            get { return _IFireRecord ?? (_IFireRecord = new FireRecordBL(context)); }
        }

        private IPopulation _IPopulation;

        public IPopulation Population
        {
            get { return _IPopulation ?? (_IPopulation = new PopulationBL(context)); }
        }

        private ITruckModel _ITruckModel;

        public ITruckModel TruckModel
        {
            get { return _ITruckModel ?? (_ITruckModel = new TruckModelBL(context)); }
        }

        private IOtherVehicle _IOtherVehicle;

        public IOtherVehicle OtherVehicle
        {
            get { return _IOtherVehicle ?? (_IOtherVehicle = new OtherVehicleBL(context)); }
        }

        private IOVModel _IOVModel;

        public IOVModel OVModel
        {
            get { return _IOVModel ?? (_IOVModel = new OVModelBL(context)); }
        }

        private IPersonnel _IPersonnel;

        public IPersonnel Personnel
        {
            get { return _IPersonnel ?? (_IPersonnel = new PersonnelBL(context)); }
        }

        public void Complete()
        {
            context.SaveChanges();
        }
    }


    public interface IInventoryUnitOfWork
    {
        IMunicipality Municipality { get; }
        ISubStation SubStation { get; }
        IStation Station { get; }
        ITruck Truck { get; }
        ITruckModel TruckModel { get; }
        IFireRecord FireRecord { get; }
        IPopulation Population { get; }
        IOtherVehicle OtherVehicle { get; }
        IOVModel OVModel { get; }
        IPersonnel Personnel { get; }
        void Complete();
    }
}
