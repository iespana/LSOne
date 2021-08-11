using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.DataProviders
{
    public interface IDataProviderBase<BusinessObject> where BusinessObject : class //, new()
    {
        //BusinessObject Get(IConnectionManager entry, RecordIdentifier ID);
        //List<BusinessObject> GetAll(IConnectionManager entry, RecordIdentifier storeID);

        //List<BusinessObject> GetList(IConnectionManager entry, RecordIdentifier ID);

        //List<BusinessObject> GetList(IConnectionManager entry, RecordIdentifier ID, BarCodeSorting sortBy, bool backwardsSort);
    }
}
