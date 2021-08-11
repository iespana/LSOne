using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Threading;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Labels;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Labels;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.LabelPrinting.Dialogs
{
    public partial class LabelPrintingDialog : DialogBase
    {
        private readonly LabelTemplate.ContextEnum context;
        private List<LabelTemplate> templates;

        public List<IDataEntity> EntitiesToPrint { get; set; }

        public bool isOrder;
        public int quantity;

        public LabelPrintingDialog()
        {
            InitializeComponent();

            numQuantity.Value = 1;

            EntitiesToPrint = new List<IDataEntity>();

            // List each printer
            cmbPrinters.Items.Clear();
            foreach (var printer in PrinterSettings.InstalledPrinters)
            {
                cmbPrinters.Items.Add(printer);
            }
        }

        public LabelPrintingDialog(LabelTemplate.ContextEnum context, bool isOrder = false, int quantity = 0)
            : this()
        {
            this.context = context;

            this.isOrder = isOrder;
            this.quantity = quantity;

            AddLabels();

            var printer = PluginEntry.PrinterItems;
            var label = PluginEntry.LabelItems;
            if (context == LabelTemplate.ContextEnum.Customers)
            {
                printer = PluginEntry.PrinterCustomers;
                label = PluginEntry.LabelCustomers;
            }

            if (string.IsNullOrEmpty(printer))
            {
                // Find zebra if available
                string zebra = FindZebra();
                if (zebra != null)
                    cmbPrinters.SelectedItem = zebra;
            }
            else
            {
                cmbPrinters.SelectedItem = printer;
            }

            if (!string.IsNullOrEmpty(label))
            {
                cmbLabels.SelectedItem = label;
            }
            if (cmbLabels.SelectedItem == null && cmbLabels.Items.Count > 0)
            {
                cmbLabels.SelectedIndex = 0;
            }

            btnEditLabels.Enabled = PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.LabelTemplatesEdit);

			if (isOrder)
			{
                lblQty.Visible = false;
                numQuantity.Visible = false;
			}
            if(quantity != 0)
			{
                numQuantity.Value = quantity;
			}
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private static string FindZebra()
        {
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                if ((printer.IndexOf("zebra", StringComparison.CurrentCultureIgnoreCase) >= 0) ||
                    (printer.IndexOf("zdesigner", StringComparison.CurrentCultureIgnoreCase) >= 0))
                    return printer;
            }
            return null;
        }

        private void AddLabels()
        {
            templates = Providers.LabelTemplateData.GetList(PluginEntry.DataModel, context);
            if (templates != null)
            {
                cmbLabels.Items.Clear();

                foreach (var label in templates)
                {
                    cmbLabels.Items.Add(label.Text);
                }
            }
        }
        
        private void OnEditLabelsClick(object sender, EventArgs e)
        {
            LabelTemplate newLabelTemplate = PluginOperations.ShowNewLabelTemplateDialog(context);

            if(newLabelTemplate != null)
            {
                AddLabels();
                cmbLabels.SelectedItem = newLabelTemplate.Text;
            }
        }

        private void OnOKClick(object sender, EventArgs e)
        {
            if (EntitiesToPrint == null || EntitiesToPrint.Count == 0)
            {
                MessageDialog.Show(Properties.Resources.NoEntitesPassedToDialog, MessageBoxIcon.Error);
                Close();
                return;
            }

            var labelService = Services.Interfaces.Services.LabelService(PluginEntry.DataModel);
            if (labelService == null)
            {
                MessageDialog.Show(Properties.Resources.LabelPrintingServiceUnavailable, MessageBoxIcon.Error);
                return;
            }

            var labelTemplate = FindLabelTemplate();

            var requests = new List<LabelPrintRequest>();
            foreach (var entity in EntitiesToPrint)
            {
                int numLabels;
				if (isOrder)
				{
                    numLabels = (int)entity.ID.SecondaryID;
                    if(numLabels < 1)
					{
                        continue;
                    }
				}
                else
				{
                    numLabels = Convert.ToInt32(numQuantity.Value);
                }
                var request = new LabelPrintRequest
                    {
                        NumberOfLabels = numLabels,
                        Printer = cmbPrinters.SelectedItem.ToString(),
                        Template = labelTemplate,
                        Entity = entity
                    };

                requests.Add(request);
            }

            if (context == LabelTemplate.ContextEnum.Items)
                PluginEntry.PrinterItems = cmbPrinters.SelectedItem.ToString();
            else
                PluginEntry.PrinterCustomers = cmbPrinters.SelectedItem.ToString();

            if (context == LabelTemplate.ContextEnum.Items)
                PluginEntry.LabelItems = cmbLabels.SelectedItem.ToString();
            else
                PluginEntry.LabelCustomers = cmbLabels.SelectedItem.ToString();

            ThreadPool.QueueUserWorkItem(PrintLabels, requests);
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private static void PrintLabels(object state)
        {
            var requests = (List<LabelPrintRequest>) state;
            var labelService = Services.Interfaces.Services.LabelService(PluginEntry.DataModel);

            var batch = DateTime.Now.ToString("yyyyMMddHHmmss");

            bool retry;
            do
            {
                // Two alternatives here

                #region Add to print queue, print from print queue

                labelService.AddToPrintQueue(PluginEntry.DataModel, batch, requests);

                if (labelService.PrintFromQueue(PluginEntry.DataModel, batch))
                    retry = false;
                else
                {
                    retry = DialogResult.Retry == MessageDialog.Show(labelService.LastErrorMessage, Properties.Resources.ErrorPrintingLabels,
                                               MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                }
                #endregion

                #region Print directly

                /*
                if (labelService.Print(PluginEntry.DataModel, requests))
                    retry = false;
                else
                {
                    retry = DialogResult.Retry == MessageDialog.Show(labelService.LastErrorMessage, Properties.Resources.ErrorPrintingLabels,
                                   MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                }
                */

                #endregion
            } while (retry);
        }

        private LabelTemplate FindLabelTemplate()
        {
            // Find the label template
            if (cmbLabels.SelectedItem != null)
            {
                var selected = cmbLabels.SelectedItem.ToString();
                foreach (var template in templates)
                {
                    if (template.Text == selected)
                    {
                        return template;
                    }
                }
            }
            return null;
        }
    }
}
