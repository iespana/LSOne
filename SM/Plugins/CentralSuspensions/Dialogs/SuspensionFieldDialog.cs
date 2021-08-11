using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Infocodes;
using LSOne.DataLayer.DataProviders.Transactions;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.CentralSuspensions.Dialogs
{
    public partial class SuspensionFieldDialog : DialogBase
    {
        
        SuspensionTypeAdditionalInfo suspendedTypeAdditionalInfo;

        RecordIdentifier suspensionfield;
        RecordIdentifier suspendedTransactionTypeId;

        bool suspendEvents;


        public SuspensionFieldDialog()
        {
            suspendedTypeAdditionalInfo = null;

            suspensionfield = RecordIdentifier.Empty;
            
            InitializeComponent(); 
           
            cmdInfocodeSelection.SelectedData = new DataEntity("", "");
            
        }


        //edit
        public SuspensionFieldDialog(RecordIdentifier suspensionfield, RecordIdentifier suspendedTransactionTypeID)
            :this()
        {
            this.suspensionfield = suspensionfield;
            suspendedTransactionTypeId = suspendedTransactionTypeID;
            suspendEvents = true;
           
            suspendedTypeAdditionalInfo = Providers.SuspensionTypeAdditionalInfoData.Get(PluginEntry.DataModel, suspensionfield);

            cmdInfocodeSelection.SelectedData = new DataEntity(suspendedTypeAdditionalInfo.InfoTypeSelectionID, suspendedTypeAdditionalInfo.InfoTypeSelectionDescription);
         
            tbDescription.Text = suspendedTypeAdditionalInfo.Text;
            cmbType.Text = suspendedTypeAdditionalInfo.InfotypeText;
            if (cmbType.Text == Properties.Resources.Infocode)
            {
                cmdInfocodeSelection.Enabled = true;
            }
            cmdInfocodeSelection.SelectedData.Text = suspendedTypeAdditionalInfo.InfoTypeSelectionDescription;
            chkRequired.Checked = suspendedTypeAdditionalInfo.Required;

            suspendEvents = false;
        }

        // new
        public SuspensionFieldDialog(RecordIdentifier suspendedTransactionTypeID)
            :this()
        {
            suspendedTransactionTypeId = suspendedTransactionTypeID;
            suspensionfield = RecordIdentifier.Empty;
        }
        

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier SuspensionFieldID
        {
            get { return suspensionfield; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
           
            
            if (suspendedTypeAdditionalInfo == null)
            {
                suspendedTypeAdditionalInfo = new SuspensionTypeAdditionalInfo();
            }

            suspendedTypeAdditionalInfo.SuspensionTypeID = suspendedTransactionTypeId;
            suspendedTypeAdditionalInfo.Text = tbDescription.Text;
            suspendedTypeAdditionalInfo.Infotype = ((SuspensionTypeAdditionalInfo.InfoTypeEnum)cmbType.SelectedIndex);

            suspendedTypeAdditionalInfo.InfoTypeSelectionID = suspendedTypeAdditionalInfo.Infotype != SuspensionTypeAdditionalInfo.InfoTypeEnum.Infocode ? 
                RecordIdentifier.Empty : cmdInfocodeSelection.SelectedData.ID;
            suspendedTypeAdditionalInfo.Required = chkRequired.Checked;
            
            
           
            Providers.SuspensionTypeAdditionalInfoData.Save(PluginEntry.DataModel, suspendedTypeAdditionalInfo);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
           
            DialogResult = DialogResult.Cancel;
            Close();
        }


        private void CheckEnabled(object sender, EventArgs e)
        {
            if (suspendEvents)
                return;

            if (suspendedTypeAdditionalInfo == null)
            {
                if (cmbType.Text == Properties.Resources.Infocode)
                {
                

                btnOK.Enabled = tbDescription.Text != "" && 
                                cmdInfocodeSelection.SelectedData.ID != "" && 
                                cmbType.SelectedIndex >= 0;                
                cmdInfocodeSelection.Enabled = true;

                }
                else
                {                    
                    cmdInfocodeSelection.Enabled = false;
                    btnOK.Enabled = tbDescription.Text != "" && 
                                    cmbType.SelectedIndex >= 0;                    
                }
            }
            else
            {
                if (cmbType.Text != Properties.Resources.Infocode)
                {
                    cmdInfocodeSelection.Enabled = false;
                    cmdInfocodeSelection.SelectedData = new DataEntity();

                    btnOK.Enabled = tbDescription.Text != "" && cmbType.SelectedIndex >= 0

                       || tbDescription.Text != suspendedTypeAdditionalInfo.Text
                       || (SuspensionTypeAdditionalInfo.InfoTypeEnum)cmbType.SelectedIndex != suspendedTypeAdditionalInfo.Infotype
                       || chkRequired.Checked != suspendedTypeAdditionalInfo.Required;              

                }
                else
                {
                    cmdInfocodeSelection.Enabled = true; 
                    cmdInfocodeSelection.SelectedData = cmdInfocodeSelection.SelectedData;

                    btnOK.Enabled = tbDescription.Text != "" && cmbType.SelectedIndex >= 0 
                        && cmdInfocodeSelection.SelectedData.ID != ""

                        || (tbDescription.Text != suspendedTypeAdditionalInfo.Text
                        || cmdInfocodeSelection.SelectedData.Text != suspendedTypeAdditionalInfo.InfoTypeSelectionDescription
                        || (SuspensionTypeAdditionalInfo.InfoTypeEnum)cmbType.SelectedIndex != suspendedTypeAdditionalInfo.Infotype
                        || chkRequired.Checked != suspendedTypeAdditionalInfo.Required);
                                   
                }
                
            }

            if (cmbType.Text == Properties.Resources.Infocode && cmdInfocodeSelection.SelectedData.Text == "")
            {
                btnOK.Enabled = false;
            }
        }

        private void cmdInfocodeSelection_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled(this,e);
        }

        private void cmdInfocodeSelection_RequestData(object sender, EventArgs e)
        {
            cmdInfocodeSelection.SetData(Providers.InfocodeData.GetList(PluginEntry.DataModel),null); 
        }

        private void chkRequired_CheckedChanged(object sender, EventArgs e)
        {
            CheckEnabled(this, e);
        }

        private void cmdInfocodeSelection_RequestClear(object sender, EventArgs e)
        {
            cmdInfocodeSelection.SelectedData = new DataEntity("","");
        }

       
    }
}
