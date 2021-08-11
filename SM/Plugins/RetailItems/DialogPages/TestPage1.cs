using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.RetailItems.DialogPages
{
    public partial class TestPage1 : System.Windows.Forms.UserControl, IDialogTabViewWithRequiredFields
    {
        public event EventHandler RequiredInputValidate;

        public TestPage1()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new TestPage1();
        }

        public bool DialogCheckEnabled
        {
            get { return true ; }
        }

        public bool DataIsModified()
        {
            // This will never be called since we are on dialog
            return false;
        }

        public void GetAuditDescriptors(List<ViewCore.AuditDescriptor> contexts)
        {
            // This will never be called since we are on dialog
        }

        public void LoadData(bool isRevert, Utilities.DataTypes.RecordIdentifier context, object internalContext)
        {
            
        }

        public void OnClose()
        {
            
        }

        public void OnDataChanged(ViewCore.Enums.DataEntityChangeType changeHint, string objectName, Utilities.DataTypes.RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public bool SaveData()
        {
            return true;
        }

        public void SaveSecondaryRecords()
        {

        }

        public void SaveUserInterface()
        {
            
        }

        public bool RequiredFieldsAreValid
        {
            get { return textBox1.Text != ""; }
        }


        private void CheckEnabled(object sender, EventArgs e)
        {
            // Here we somehow want to trigger checkenabled on the parent
            if (RequiredInputValidate != null)
            {
                RequiredInputValidate(this, EventArgs.Empty);
            }
        }
    }
}
