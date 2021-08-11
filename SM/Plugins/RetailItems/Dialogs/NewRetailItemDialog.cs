using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.RetailItems.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.RetailItems.Dialogs
{ 
    public partial class NewRetailItemDialog : DialogBase
    {
        RetailItem retailItem;
        

        public RecordIdentifier ItemID { get; set; }

        public NewRetailItemDialog(bool allowCreateAnother) : this()
        {
            cbCreateAnother.Enabled = allowCreateAnother;
        }

        public NewRetailItemDialog()
        {
            retailItem = new RetailItem();
            retailItem.ItemType = ItemTypeEnum.Item;
            InitializeComponent();

            // "Test" here bellow would be localized if this was actual case
            tabSheetTabs.AddTab(new TabControl.Tab(Resources.General, DialogPages.NewRetailItemGeneralPage.CreateInstance,true));

            // Allow other plugins to extend this tab panel
            tabSheetTabs.Broadcast(this, RecordIdentifier.Empty); // Empty since we dont got any retail item at this point

            tabSheetTabs.SetData(false, RecordIdentifier.Empty, retailItem);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            // This is the graceful and right thing to do in case if any tab has something that needs UI saving
            tabSheetTabs.SaveUserInterface();

            base.OnClosing(e);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            
            FieldValidationArguments args = tabSheetTabs.AllRequiredFieldsAreValid();

            if (args.Result == FieldValidationArguments.FieldValidationEnum.TabHasMissingFields)
            {
                MessageDialog.Show(string.Format(Resources.TabRequiresInput, args.TabName) , MessageBoxIcon.Error);
            }
            else if (args.Result == FieldValidationArguments.FieldValidationEnum.FieldMissing)
            {
                MessageDialog.Show(string.Format(Resources.FieldRequiresInput, args.TabName, args.ResultDescription) , MessageBoxIcon.Error);
            }
            else if (args.Result == FieldValidationArguments.FieldValidationEnum.OtherInvalid)
            {
                MessageDialog.Show(args.ResultDescription, MessageBoxIcon.Warning);
            }
            else
            {

                SpinnerDialog dlg = new SpinnerDialog(Resources.FewMinutesMessage, () => SaveData());
                dlg.ShowDialog();

                ItemID = retailItem.ID;

                if (cbCreateAnother.Checked)
                {
                    retailItem = new RetailItem();
                    retailItem.ItemType = ItemTypeEnum.Item;
                    bool handled;
                    tabSheetTabs.SendViewPageMessage(this, "CreateAnother", null, out handled);
                    tabSheetTabs.SetData(false, RecordIdentifier.Empty, retailItem);
                    tabSheetTabs.SelectedTab = tabSheetTabs[0];
                }
                else
                {
                    DialogResult = DialogResult.OK;

                    Close();
                }
            }
        }

        private void SaveData()
        {
            // Tell all tabs to put their data onto the RetailItem record
            tabSheetTabs.GetData();

            // Here we would save our main record, in this case saving the RetailItem
            // doing that will make the record get valid ID also note that, which the secondary records can then use to link against.

            // -------------------------------------------------------------------
            // Save the main record here
            Providers.RetailItemData.Save(PluginEntry.DataModel, retailItem);
            // -------------------------------------------------------------------

            // Now we save the secondary records,  which would for example be barcodes. At this time the main record will have ID
            // making it possible for the plugins to write to secondary tables that link against this ID
            tabSheetTabs.SaveSecondaryRecords();
        }

        public ItemTypeEnum ItemType
        {
            get
            {
                if(retailItem != null)
                {
                    return retailItem.ItemType;
                }

                return ItemTypeEnum.Item;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }


        public object OnViewPageMessage(object sender, string message, object param, ref bool handled)
        {
            throw new NotImplementedException();
        }
    }
}
