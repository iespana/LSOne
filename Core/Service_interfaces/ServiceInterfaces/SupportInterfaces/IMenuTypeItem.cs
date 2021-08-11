using System;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface IMenuTypeItem : ISerializable, ICloneable
    {
        string CodeOnPos { get; set; }
        string Description { get; set; }
        string DisplayText { get; }
        bool Exists { get; }
        string MenuOrder { get; set; }
        bool NotExists { get; }
        string RestaurantID { get; set; }
    }
}
