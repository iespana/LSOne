using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewCore.Controls;

namespace LSOne.ViewPlugins.BarCodes.DialogPages
{
    public partial class TestPage2 : System.Windows.Forms.UserControl, IDialogTabViewWithRequiredFields
    {
        public event EventHandler RequiredInputValidate;

        public TestPage2()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new TestPage2();
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
            // Here would be the right place to save to Barcode table since at this point we have ID in the main record
        }

        public void SaveUserInterface()
        {
            
        }

        public bool RequiredFieldsAreValid
        {
            get { return textBox1.Text != "" && textBox2.Text != ""; }
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if(RequiredInputValidate != null)
            {
                RequiredInputValidate(this, EventArgs.Empty);
            }
        }

        
    }
}
