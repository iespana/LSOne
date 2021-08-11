using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Inventory.Dialogs;
using LSOne.ViewPlugins.Inventory.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Inventory
{
    internal partial class PluginOperations
    {

        #region Events

        public static void ShowVendorsView(object sender, EventArgs args)
        {
            if (TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new Views.VendorsView());
            }
        }

        public static void ShowVendorView(object sender, EventArgs args)
        {
            if (TestSiteService())
            {
                ContextBarClickEventArguments a = (ContextBarClickEventArguments)args;
                RecordIdentifier vendorID = a.Item.Key;

                PluginEntry.Framework.ViewController.Add(new Views.VendorView(vendorID));
            }
        }

        public static void ShowVendorView(RecordIdentifier vendorID, IEnumerable<IDataEntity> recordBrowsingContext)
        {
            if (TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new Views.VendorView(vendorID, recordBrowsingContext));
            }
        }

        public static void ShowVendor(RecordIdentifier id)
        {
            if (TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new Views.VendorView(id));
            }
        }

        #endregion

        public static void NewVendorContact(RecordIdentifier vendorID)
        {
            if (!PluginEntry.DataModel.HasPermission(Permission.VendorEdit))
            {
                return;
            }

            Dialogs.ContactDialog dlg = new Dialogs.ContactDialog(vendorID, ContactRelationTypeEnum.Vendor);
            dlg.ShowDialog();
        }

        public static void EditVendorContact(RecordIdentifier contactID)
        {
            Dialogs.ContactDialog dlg = new Dialogs.ContactDialog(contactID);
            dlg.ShowDialog();
        }


        public static RecordIdentifier NewVendor()
        {
            DialogResult result;
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.VendorEdit))
            {
                Dialogs.NewVendorDialog dlg = new Dialogs.NewVendorDialog();


                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog();

                PluginEntry.Framework.ResumeSearchBarClosing();

                if (result == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.VendorID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "Vendor", dlg.VendorID, null);

                    PluginOperations.ShowVendor(dlg.VendorID);
                }
            }

            return selectedID;
        }

        public static bool RestoreVendor(RecordIdentifier id)
        {
            return RestoreVendors(new List<RecordIdentifier> { id });
        }

        public static bool RestoreVendors(List<RecordIdentifier> toActivate)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.VendorEdit))
            {
                string question = toActivate.Count > 1 ? Properties.Resources.RestoreVendorsQuestion : Properties.Resources.RestoreVendorQuestion;
                string caption = toActivate.Count > 1 ? Properties.Resources.RestoreVendors : Properties.Resources.RestoreVendor;
                if (QuestionDialog.Show(question, caption) == DialogResult.Yes)
                {
                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).RestoreVendors(
                        PluginEntry.DataModel,
                        PluginOperations.GetSiteServiceProfile(),
                        toActivate,
                        true);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(
                        null,
                        toActivate.Count > 1 ? DataEntityChangeType.MultiAdd : DataEntityChangeType.Add,
                        "Vendor",
                        toActivate.Count > 1 ? RecordIdentifier.Empty : toActivate.FirstOrDefault(),
                        toActivate.Count > 1 ? toActivate : null);

                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }

        public static bool DeleteVendor(Vendor vendor)
        {
            return DeleteVendors(new List<Vendor> { vendor });
        }

        public static bool DeleteVendors(List<Vendor> vendorsToDelete)
        {
            bool retValue = false;
            List<RecordIdentifier> toDelete = vendorsToDelete.Select(v => v.ID).ToList();
            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.VendorEdit))
            {
                string question = toDelete.Count > 1 ? Resources.DeleteVendorsQuestion : Resources.DeleteVendorQuestion;
                string caption = toDelete.Count > 1 ? Resources.DeleteVendors : Resources.DeleteVendor;
                if (QuestionDialog.Show(question, caption) == DialogResult.Yes)
                {
                    try
                    {
                        List<RecordIdentifier> linkedVendors = new List<RecordIdentifier>();
                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).DeleteVendorsCanExecute(
                            PluginEntry.DataModel,
                            GetSiteServiceProfile(),
                            toDelete,
                            out linkedVendors,
                            true);

                        if (linkedVendors.Count > 0)
                        {
                            if (linkedVendors.Intersect(toDelete).Count() == toDelete.Count)
                            {
                                MessageDialog.Show(linkedVendors.Count == 1 ? Resources.SelectedVendorCannotBeDeleted : Resources.SelectedVendorsCannotBeDeleted);
                                return false;
                            }
                            else
                            {
                                List<string> vList = vendorsToDelete.Where(v => linkedVendors.Contains(v.ID)).Select(v => v.Text).ToList();
                                var vNames = string.Join(", ", vList);
                                question = string.Format(vList.Count == 1 ? Resources.VendorCannotBeDeleted : Resources.VendorsCannotBeDeleted, vNames);

                                if (QuestionDialog.Show(question, caption) == DialogResult.Yes)
                                {
                                    linkedVendors.ForEach(el => toDelete.Remove(el));
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }

                        //Check if vendors have items linked or are default vendor for items
                        if (!DeleteVendorLinksToItems(toDelete))
                        {
                            return false;
                        }

                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).DeleteVendors(
                            PluginEntry.DataModel,
                            GetSiteServiceProfile(),
                            toDelete,
                            true);
                    }
                    catch (Exception ex)
                    {
                        MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message, Properties.Resources.DeleteVendors);
                        return false;
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(
                        null,
                        toDelete.Count > 1 ? DataEntityChangeType.MultiDelete : DataEntityChangeType.Delete,
                        "Vendor",
                        toDelete.Count > 1 ? RecordIdentifier.Empty : toDelete.FirstOrDefault(),
                        toDelete.Count > 1 ? toDelete : null);

                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }

        private static bool DeleteVendorLinksToItems(List<RecordIdentifier> vendorIDs)
        {
            var inventoryService = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel);
            VendorItemsLinkedType linked = VendorItemsLinkedType.None;
            string question = "";
            string caption;
            if (vendorIDs == null || vendorIDs.Count == 0)
            {
                return true;
            }
            else if (vendorIDs.Count == 1)
            {
                linked = inventoryService.VendorHasLinkedItems(PluginEntry.DataModel,
                                                                GetSiteServiceProfile(),
                                                                vendorIDs[0],
                                                                true);
                caption = Resources.DeleteVendor;

                if (linked == VendorItemsLinkedType.None)
                {
                    return true;
                }

                switch (linked)
                {
                    case VendorItemsLinkedType.DefaultVendor:
                        question = Resources.DeleteVendorQuestionDefaultVendor;
                        break;
                    case VendorItemsLinkedType.VendorItems:
                        question = Resources.DeleteVendorQuestionLinkedItems;
                        break;
                    case VendorItemsLinkedType.DefaultVendorAndVendorItems:
                        question = Resources.DeleteVendorQuestionLinkedAndDefault;
                        break;
                    default:
                        break;
                }

                var vendor = inventoryService.GetVendor(PluginEntry.DataModel, GetSiteServiceProfile(), vendorIDs[0], true);
                question = string.Format(question, vendor.Text);
            }
            else
            {
                foreach (var vendorID in vendorIDs)
                {
                    linked = inventoryService.VendorHasLinkedItems(PluginEntry.DataModel,
                                                                GetSiteServiceProfile(),
                                                                vendorID,
                                                                true);
                    if (linked != VendorItemsLinkedType.None)
                    {
                        break;
                    }
                }

                caption = Resources.DeleteVendors;
                if (linked != VendorItemsLinkedType.None)
                {
                    question = Resources.DeleteVendorsQuestionItemLinks;
                }
            }

            if (linked == VendorItemsLinkedType.None)
            {
                return true;
            }
            if (QuestionDialog.Show(question + ". " + Resources.DeleteVendorQuestionAction + Environment.NewLine + Resources.DoYouWantToContinue, caption) == DialogResult.Yes)
            {
                foreach (var vendorID in vendorIDs)
                {
                    inventoryService.DeleteVendorItemLinks(PluginEntry.DataModel,
                                                            GetSiteServiceProfile(),
                                                            vendorID,
                                                            true);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void NewVendor(object sender, EventArgs args)
        {
            NewVendor();
        }

    }
}
