using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    public partial class RestaurantMenuTypeDialog : DialogBase
    {
        RecordIdentifier restaurantMenuTypeID;
        RecordIdentifier restaurantID;
        RestaurantMenuType restaurantMenuType;
        bool editingExisting;
        bool suspendEvents = false;
        //bool initialFocus;

        /// <summary>
        /// Opens the dialog to create a new restaurant menu type
        /// </summary>
        /// <param name="restaurantID">The id of the restaurant</param>
        public RestaurantMenuTypeDialog(RecordIdentifier restaurantID)
        {
            InitializeComponent();

            //initialFocus = false;
            restaurantMenuTypeID = RecordIdentifier.Empty;
            this.restaurantID = restaurantID;
            editingExisting = false;

        }

        /// <summary>
        /// Opens the dialog for editing an existing restaurant menu type
        /// </summary>
        /// <param name="restaurantID">The id of the restaurant</param>
        /// <param name="menuOrder">The id of the menu order</param>
        public RestaurantMenuTypeDialog(RecordIdentifier restaurantID, RecordIdentifier menuOrder)
        {
            InitializeComponent();

            restaurantMenuTypeID = RecordIdentifier.Empty;
            this.restaurantID = restaurantID;

            restaurantMenuType = Providers.RestaurantMenuTypeData.Get(PluginEntry.DataModel, new RecordIdentifier(restaurantID, menuOrder));
            editingExisting = true;
            suspendEvents = true;
            tbRestaurantID.Text = (string)restaurantMenuType.RestaurantID;
            ntbMenuOrder.Value = (int)restaurantMenuType.MenuOrder;
            tbDescription.Text = restaurantMenuType.Text;
            tbCodeOnPos.Text = restaurantMenuType.CodeOnPos;
            suspendEvents = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            tbRestaurantID.Text = (string)restaurantID;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier RestaurantMenuTypeID
        {
            get { return restaurantMenuTypeID; }
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if (suspendEvents)
                return;

            errorProvider1.Clear();

            if (editingExisting)
            {
                btnOK.Enabled = tbDescription.Text.Length > 0 && IsModified();
            }
            else
            {
                btnOK.Enabled = tbDescription.Text.Length > 0;
            }
        }

        private bool IsModified()
        {
            if (tbDescription.Text != restaurantMenuType.Text) return true;
            if ((int)ntbMenuOrder.Value != (int)restaurantMenuType.MenuOrder) return true;
            if (tbCodeOnPos.Text != restaurantMenuType.CodeOnPos) return true;

            return false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (editingExisting)
            {
                if (tbCodeOnPos.Text != restaurantMenuType.CodeOnPos && Providers.RestaurantMenuTypeData.Exists(PluginEntry.DataModel, new RecordIdentifier(restaurantID, (int)ntbMenuOrder.Value), tbCodeOnPos.Text))
                {
                    errorProvider1.SetError(tbCodeOnPos, Properties.Resources.RestaurantMenuTypeCodeOnPosExists);
                    tbCodeOnPos.Focus();
                    return;
                }                

                // If we are modifying the Order field, we need to delete the old record first so we don't create a new record when editing
                if ((int)ntbMenuOrder.Value != restaurantMenuType.MenuOrder)
                {
                    if (Providers.RestaurantMenuTypeData.Exists(PluginEntry.DataModel, new RecordIdentifier(restaurantID, (int)ntbMenuOrder.Value)))
                    {
                        errorProvider1.SetError(ntbMenuOrder, Properties.Resources.RestaurantMenuTypeExists);
                        ntbMenuOrder.Focus();
                        return;
                    }

                    Providers.RestaurantMenuTypeData.Delete(PluginEntry.DataModel, restaurantMenuType.ID);
                }

                restaurantMenuType.RestaurantID = restaurantID;
                restaurantMenuType.MenuOrder = (int)ntbMenuOrder.Value;
                restaurantMenuType.Text = tbDescription.Text;
                restaurantMenuType.CodeOnPos = tbCodeOnPos.Text;

                Providers.RestaurantMenuTypeData.Save(PluginEntry.DataModel, restaurantMenuType);

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "RestaurantMenuType", restaurantMenuType.ID, null);

                restaurantMenuTypeID = restaurantMenuType.ID;
            }
            else
            {
                RecordIdentifier tempMenuTypeID = new RecordIdentifier(restaurantID, (int)ntbMenuOrder.Value);

                if (Providers.RestaurantMenuTypeData.Exists(PluginEntry.DataModel, tempMenuTypeID))
                {
                    errorProvider1.SetError(ntbMenuOrder, Properties.Resources.RestaurantMenuTypeExists);
                    ntbMenuOrder.Focus();
                    return;
                }

                if (Providers.RestaurantMenuTypeData.Exists(PluginEntry.DataModel, tempMenuTypeID, tbCodeOnPos.Text))
                {
                    errorProvider1.SetError(tbCodeOnPos, Properties.Resources.RestaurantMenuTypeCodeOnPosExists);
                    tbCodeOnPos.Focus();
                    return;
                }

                restaurantMenuType = new RestaurantMenuType();
                restaurantMenuType.RestaurantID = restaurantID;
                restaurantMenuType.MenuOrder = (int)ntbMenuOrder.Value;
                restaurantMenuType.Text = tbDescription.Text;
                restaurantMenuType.CodeOnPos = tbCodeOnPos.Text;

                Providers.RestaurantMenuTypeData.Save(PluginEntry.DataModel, restaurantMenuType);

                restaurantMenuTypeID = restaurantMenuType.ID;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
