using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Peripherals.Dialogs;

namespace LSOne.Peripherals.Interfaces
{
    internal interface IHardwareValidator
    {
        string OPOSType{ get; set; }
        bool ValiddateInput();
        void LoadProfile(HardwareProfile profile);
        string AutoDetectOPOS(DetectionDialog dlg );
        void SetDetectedDevice();
        void EnableTest(bool enabled);
        string  DetectedDevice { get; set; }
    }
}
