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
        internal static void ShowReasonCodesView(RecordIdentifier reasonCodeID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ReasonCodesView(reasonCodeID));
        }

        public static void ShowReasonCodesView(object sender, EventArgs args)
        {
            if (TestSiteService())
            {
                PluginEntry.Framework.ViewController.Add(new Views.ReasonCodesView());
            }
        }

        internal static void NewReasonCode()
        {
            ReasonCodeDialog dlg = new ReasonCodeDialog(ReasonCodeDialogBehaviour.Add, new List<RecordIdentifier>());
            dlg.ShowDialog();
        }

        internal static void EditReasonCode(RecordIdentifier reasonId)
        {
            ReasonCodeDialog dlg = new ReasonCodeDialog(ReasonCodeDialogBehaviour.Edit, new List<RecordIdentifier> { reasonId });
            dlg.ShowDialog();
        }

        internal static void EditReasonCodes(List<RecordIdentifier> reasonIds)
        {
            ReasonCodeDialog dlg = new ReasonCodeDialog(ReasonCodeDialogBehaviour.MultiEdit, reasonIds);
            dlg.ShowDialog();
        }

        internal static void DeleteReasonCode(RecordIdentifier reasonId)
        {
            DeleteReasonCodes(new List<RecordIdentifier> { reasonId });
        }

        internal static void DeleteReasonCodes(List<RecordIdentifier> reasonIds)
        {
            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.ManageParkedInventory))
            {
                string question = reasonIds.Count > 1 ? Resources.DeleteReasonCodesQuestion : Resources.DeleteReasonCodeQuestion;
                string caption = reasonIds.Count > 1 ? Resources.DeleteReasonCodes : Resources.DeleteReasonCode;
                if (QuestionDialog.Show(question, caption) == DialogResult.Yes)
                {
                    try
                    {
                        foreach (RecordIdentifier reasonId in reasonIds)
                        {
                            Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).DeleteReasonCode(PluginEntry.DataModel,
                                                                                                                    GetSiteServiceProfile(),
                                                                                                                    reasonId,
                                                                                                                    true);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message, Resources.DeleteReasonCode);
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(
                        null,
                        reasonIds.Count > 1 ? DataEntityChangeType.MultiDelete : DataEntityChangeType.Delete,
                        "ReasonCode",
                        reasonIds.Count > 1 ? RecordIdentifier.Empty : reasonIds.FirstOrDefault(),
                        reasonIds.Count > 1 ? reasonIds : null);
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();
        }
    }
}
