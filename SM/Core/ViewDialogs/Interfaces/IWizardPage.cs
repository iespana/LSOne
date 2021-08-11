using System.Windows.Forms;

namespace LSOne.ViewCore.Dialogs.Interfaces
{
    public interface IWizardPage
    {
        bool HasForward { get; }

        bool HasFinish { get; }

        Control PanelControl { get; }

        void Display();

        IWizardPage RequestNextPage();

        bool NextButtonClick(ref bool canUseFromForwardStack);

        void ResetControls();
    }
}
