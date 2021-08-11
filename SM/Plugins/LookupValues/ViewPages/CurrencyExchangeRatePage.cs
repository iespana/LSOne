using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Currencies;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.LookupValues.ViewPages
{
    public partial class CurrencyExchangeRatePage : UserControl, ITabView
    {
        bool isCompanyCurrency;
        RecordIdentifier currencyCode;
        WeakReference companyInfoEditor;

        public CurrencyExchangeRatePage()
        {
            IPlugin plugin;

            InitializeComponent();

            lvValues.ContextMenuStrip = new ContextMenuStrip();
            lvValues.ContextMenuStrip.Opening += lvValues_Opening;

            lvValues.SortColumn = 0;
            lvValues.SortedBackwards = false;

            plugin = PluginEntry.Framework.FindImplementor(this, "CanEditCompanyInfo", null);
            companyInfoEditor = (plugin != null) ? new WeakReference(plugin) : null;

        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.CurrencyExchangeRatePage();
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            isCompanyCurrency = (bool)internalContext;
            currencyCode = context;
            LoadLines(currencyCode);
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return true;
        }


        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("ExchangeRates", RecordIdentifier.Empty, Properties.Resources.ExchangeRate, false));
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {

        }

        public void OnClose()
        {
            // Avoid Microsoft memory leak bug in ListViews
            lvValues.SmallImageList = null;
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void lvValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsValueButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.CurrencyEdit) && !isCompanyCurrency;
            btnsValueButtons.RemoveButtonEnabled = (lvValues.SelectedItems.Count > 0) && btnsValueButtons.AddButtonEnabled && !isCompanyCurrency;
            btnsValueButtons.EditButtonEnabled = btnsValueButtons.RemoveButtonEnabled && !isCompanyCurrency;
        }

        private void lvValues_DoubleClick(object sender, EventArgs e)
        {
            if (lvValues.SelectedItems.Count > 0 && btnsValueButtons.EditButtonEnabled)
            {
                btnEditValue_Click(this, EventArgs.Empty);
            }
        }

        void lvValues_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvValues.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.Add + "...",
                    200,
                    new EventHandler(btnAddValue_Click))
            {
                Enabled = btnsValueButtons.AddButtonEnabled,
                Image = ContextButtons.GetAddButtonImage()
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.EditCmd + "...",
                    100,
                    new EventHandler(btnEditValue_Click))
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsValueButtons.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);


            item = new ExtendedMenuItem(
                    Properties.Resources.Delete + "...",
                    300,
                    new EventHandler(btnRemoveValue_Click))
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = btnsValueButtons.RemoveButtonEnabled
            };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("ExchangeRatesList", lvValues.ContextMenuStrip, lvValues);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void LoadLines(RecordIdentifier currencyCode)
        {
            ListViewItem listItem;

            if (currencyCode.IsEmpty)
            {
                return;
            }

            lvValues.Items.Clear();

            List<ExchangeRate> lines = Providers.ExchangeRatesData.GetExchangeRates(PluginEntry.DataModel, currencyCode, lvValues.SortColumn, lvValues.SortedBackwards);

            Currency companyCurrency = Providers.CurrencyData.GetCompanyCurrency(PluginEntry.DataModel);

            if (companyCurrency != null && companyCurrency.ID != "")
            {
                lvValues.Columns[1].Text = Properties.Resources.ExchangeRate + " (# " + companyCurrency.ID + " -> 1 " + currencyCode + ")";
                lvValues.Columns[2].Text = Properties.Resources.POSExchangeRate + " (# " + companyCurrency.ID + " -> 1 " + currencyCode + ")";
            }
            else
            {
                lvValues.Columns[1].Text = Properties.Resources.ExchangeRate;
                lvValues.Columns[2].Text = Properties.Resources.POSExchangeRate;
            }

            foreach (var line in lines)
            {
                listItem = new ListViewItem(((DateTime)line.FromDate).ToShortDateString());
                listItem.SubItems.Add(companyCurrency != null && companyCurrency.ID != "" ? (line.ExchangeRateValue / 100).ToString("F4") : Properties.Resources.CompanyCurrencyMissing);
                listItem.SubItems.Add(companyCurrency != null && companyCurrency.ID != "" ? (line.POSExchangeRateValue / 100).ToString("F4") : Properties.Resources.CompanyCurrencyMissing);

                listItem.Tag = line.ID;
                listItem.ImageIndex = -1;

                lvValues.Add(listItem);
            }

            lvValues.Columns[lvValues.SortColumn].ImageIndex = (lvValues.SortedBackwards ? 1 : 0);

            lvValues_SelectedIndexChanged(this, EventArgs.Empty);

            lvValues.BestFitColumns();

        }

        private void lvValues_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvValues.SortColumn == e.Column)
            {
                lvValues.SortedBackwards = !lvValues.SortedBackwards;
            }
            else
            {
                if (lvValues.SortColumn != -1)
                {
                    // Clear the old sorting
                    lvValues.Columns[lvValues.SortColumn].ImageIndex = 2;
                    lvValues.SortColumn = e.Column;
                }
                lvValues.SortedBackwards = false;
            }

            LoadLines(currencyCode);
        }

        private void btnAddValue_Click(object sender, EventArgs e)
        {
            try
            {
                Dialogs.ExchangeRateDialog dlg = new Dialogs.ExchangeRateDialog(currencyCode);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    LoadLines(currencyCode);
                }
            }
            catch (DataIntegrityException)
            {
                CompanyCurrencyMissingDialog();
            }
            
        }

        private void btnEditValue_Click(object sender, EventArgs e)
        {
            RecordIdentifier currencyCode = ((RecordIdentifier)lvValues.SelectedItems[0].Tag).PrimaryID;
            RecordIdentifier startDate = ((RecordIdentifier)lvValues.SelectedItems[0].Tag).SecondaryID;

            try
            {
                Dialogs.ExchangeRateDialog dlg = new Dialogs.ExchangeRateDialog(currencyCode, (DateTime)startDate);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    LoadLines(currencyCode);
                }
            }
            catch (DataIntegrityException)
            {
                CompanyCurrencyMissingDialog();
            }
        }

        private void CompanyCurrencyMissingDialog()
        {
            if (companyInfoEditor != null)
            {
                if (QuestionDialog.Show(
                        Properties.Resources.CompanyCurrencyMissingInfo,
                        Properties.Resources.CompanyCurrencyMissing) == DialogResult.Yes)
                {
                    ((IPlugin)companyInfoEditor.Target).Message(this, "EditCompanyInfo", null);
                }
            }
            else
            {
                MessageDialog.Show(Properties.Resources.CompanyCurrencyMissing);
            }
        }

        private void btnRemoveValue_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeleteExchangeRateQuestion,
                Properties.Resources.DeleteExchangeRate) == DialogResult.Yes)
            {
                Providers.ExchangeRatesData.Delete(PluginEntry.DataModel, (RecordIdentifier)lvValues.SelectedItems[0].Tag);

                LoadLines(currencyCode);
            }
        }

        
    }
}
