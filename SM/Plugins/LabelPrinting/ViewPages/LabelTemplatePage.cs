using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Labels;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.GUI;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.LabelPrinting.ViewPages
{
    public partial class LabelTemplatePage : UserControl, ITabView
    {
        private bool imageChanged;
        private LabelTemplate labelTemplate;
        ContextMenuStrip contextMenu;

        public LabelTemplatePage()
        {
            InitializeComponent();

            if (components == null)
                components = new Container();
            contextMenu = new ContextMenuStrip(components);
            contextMenu.ItemClicked += OnMacroMenuItemClicked;

            txtDescription.ContextMenuStrip = contextMenu;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new LabelTemplatePage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            labelTemplate = (LabelTemplate)internalContext;

            txtDescription.Text = labelTemplate.Description;
            txtTemplate.Text = labelTemplate.Template;
            txtCodepage.Text = labelTemplate.CodePage;
            if (labelTemplate.SampleImage != null)
                picSample.Image = labelTemplate.SampleImage;

            var service = Services.Interfaces.Services.LabelService(PluginEntry.DataModel);
            if (service == null)
            {
                MessageDialog.Show(Properties.Resources.LabelPrintingServiceUnavailable, MessageBoxIcon.Error);
                return;
            }

            List<string> macros = null;
            if (labelTemplate.Context == LabelTemplate.ContextEnum.Items)
                macros = service.GetAvailableMacros<RetailItem>();
            else if (labelTemplate.Context == LabelTemplate.ContextEnum.Customers)
                macros = service.GetAvailableMacros<Customer>();

            if (macros == null)
                return;

            contextMenu.Items.Clear();
            foreach (var macro in macros)
                contextMenu.Items.Add(macro);
        }

        public bool DataIsModified()
        {
            if (txtDescription.Text != labelTemplate.Description) return true;
            if (txtTemplate.Text != labelTemplate.Template) return true;
            if (txtCodepage.Text != labelTemplate.CodePage) return true;
            if (imageChanged) return true;

            return false;
        }

        public bool SaveData()
        {
            if (!string.IsNullOrEmpty(txtCodepage.Text))
            {
                var encoding = LabelTemplate.ParseCodepage(txtCodepage.Text);
                if (encoding == null)
                {
                    MessageDialog.Show(Properties.Resources.CodepageNotFound, MessageBoxIcon.Error);
                    return false;
                }
            }

            labelTemplate.Description = txtDescription.Text;
            labelTemplate.Template = txtTemplate.Text;
            labelTemplate.CodePage = txtCodepage.Text;
            labelTemplate.SampleImage = picSample.Image;

            if (!labelTemplate.Template.TrimEnd(' ', '\t').EndsWith(Environment.NewLine))
            {
                if (DialogResult.Yes ==
                    MessageDialog.Show(Properties.Resources.AddNewLine, Properties.Resources.PossiblyInvalidTemplate,
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    labelTemplate.Template = labelTemplate.Template.TrimEnd(' ', '\t') + Environment.NewLine;
                    txtTemplate.Text = labelTemplate.Template;
                }
            }

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

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void btnSetSample_Click(object sender, EventArgs e)
        {
            var image = ImageUtils.BrowseForImage(PluginEntry.Framework.MainWindow);
            if (image != null)
            {
                imageChanged = true;
                labelTemplate.SampleImage = image;
                picSample.Image = image;
            }
        }

        private void btnGetMacros_Click(object sender, EventArgs e)
        {
            contextMenu.Show(Cursor.Position);
        }

        private void OnMacroMenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            (sender as ContextMenuStrip).ItemClicked -= OnMacroMenuItemClicked;
            txtTemplate.Paste(e.ClickedItem.Text);
        }
    }
}
