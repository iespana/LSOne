using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Terminals;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Terminals;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;

namespace LSOne.ViewPlugins.Terminals.Dialogs
{
    public partial class NewGroup : DialogBase
    {
        TerminalGroup terminalGroup;

        public NewGroup(RecordIdentifier terminalGroupId) 
            : this()
        {
            terminalGroup = Providers.TerminalGroupData.Get(PluginEntry.DataModel, terminalGroupId);

            tbDescription.Text = terminalGroup.Text;
        }
        
        public NewGroup()
        {
            InitializeComponent();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if (terminalGroup != null)
            {
                btnOK.Enabled = (tbDescription.Text != terminalGroup.Text) && tbDescription.Text != "";
            }
            else
            {
                btnOK.Enabled = tbDescription.Text != "";

            }
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            if (terminalGroup == null)
            {
               terminalGroup =  new TerminalGroup();
                
            }

            terminalGroup.Text = tbDescription.Text;

            Providers.TerminalGroupData.Save(PluginEntry.DataModel,terminalGroup);

            DialogResult = DialogResult.OK;
            Close();

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "TerminalGroup", null, null);
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
