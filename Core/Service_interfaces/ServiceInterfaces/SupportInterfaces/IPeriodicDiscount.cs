namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface IPeriodicDiscount
    {
        void Add(System.Data.DataTable periodDiscountData, string itemId);
        void AddBarcode(string id);
        void AddDepartment(string id);
        void AddItem(string id);
        void AddRetailGroup(string id);
        void Clear();
        System.Data.DataTable Get(string itemId, string retailGroupId, string departmentId);
        System.Data.DataTable GetBarcode(string barcode);
        System.Data.DataTable GetDepartment(string departmentID);
        System.Data.DataTable GetItem(string itemId);
        System.Data.DataTable GetRetailGroup(string groupID);
    }
}
