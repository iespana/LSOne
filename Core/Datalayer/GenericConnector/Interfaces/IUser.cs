using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.GenericConnector.Interfaces
{
    public interface IUser
    {
        RecordIdentifier ID { get; set; }

        string Text { get; set; }

        string UserName { get; }

        bool IsValid { get; }

        bool ActiveDirectoryUser { get; }

        int DaysUntilPasswordExpires { get; }

        bool ForcePasswordChange { get; }

        bool SessionClosed { get; set; }

        RecordIdentifier StaffID { get; set; }
    }
}
