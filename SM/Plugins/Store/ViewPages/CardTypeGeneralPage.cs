using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Store.ViewPages
{
    public partial class CardTypeGeneralPage : UserControl, IDialogTabViewWithRequiredFields
    {

        WeakReference parentTabControl;
        StoreCardType cardType;

        public CardTypeGeneralPage()
        {
            InitializeComponent();
        }

        public CardTypeGeneralPage(TabControl parentTabControl)
            : this()
        {
            this.parentTabControl = new WeakReference(parentTabControl);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.CardTypeGeneralPage((TabControl)sender);
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {            
            cardType = (StoreCardType)internalContext;
            
            chkModulusCheck.Checked = cardType.CheckModulus;
            chkExpiryDate.Checked = cardType.CheckExpiredDate;
            chkInternalauthorization.Checked = cardType.ProcessLocally;
            chkAllowManualEntering.Checked = cardType.AllowManualInput;
        }

        public bool DataIsModified()
        {            
            if (chkAllowManualEntering.Checked != cardType.AllowManualInput) return true;
            if (chkExpiryDate.Checked != cardType.CheckExpiredDate) return true;
            if (chkInternalauthorization.Checked != cardType.ProcessLocally) return true;
            if (chkModulusCheck.Checked != cardType.CheckModulus) return true;

            return false;
        }

        public bool SaveData()
        {
            cardType.AllowManualInput = chkAllowManualEntering.Checked;
            cardType.CheckExpiredDate = chkExpiryDate.Checked;
            cardType.CheckModulus = chkModulusCheck.Checked;
            cardType.ProcessLocally = chkInternalauthorization.Checked;

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            //throw new NotImplementedException();
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            //throw new NotImplementedException();
        }

        public void SaveSecondaryRecords()
        {

        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void CheckEnabled(object sender, EventArgs args)
        {
            //TODO refacture this to use the RequiredInputValidate mechanishm !
            if (parentTabControl.IsAlive)
            {
                ((TabControl)parentTabControl.Target).OnEnabledStateChanged();
            }

            if(RequiredInputValidate != null)
            {
                RequiredInputValidate(this, EventArgs.Empty);
            }
        }

        void IDialogTabViewWithRequiredFields.RequiredFieldsAreValid(FieldValidationArguments args)
        {
            if(DataIsModified())
            {
                args.Result = FieldValidationArguments.FieldValidationEnum.Valid;
            }
            else
            {
                args.Result = FieldValidationArguments.FieldValidationEnum.OtherInvalid;
            }
        }


        #region IDialogTabViewWithRequiredFields Members

        public event EventHandler RequiredInputValidate;


        #endregion


        
    }
}
