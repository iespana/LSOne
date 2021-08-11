//style class here

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.BusinessObjects.UserInterface;
using LSOne.DataLayer.BusinessObjects.UserInterface.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.DataLayer.DataProviders.UserInterface;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.GUI;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.UserInterfaceStyles.Dialogs
{
    public partial class NewStyleDialog : DialogBase
    {
        private PosStyle posStyle;

        public NewStyleDialog()
        {
            posStyle = null;

            InitializeComponent();

            Dictionary<Guid, ContextStyleDescriptor> contexts = PluginEntry.Framework.GetPluginContextStyleDescriptors();

            var sortedContexts = contexts.Values.OrderBy(context => context.ToString());
            LoadCopyFrom();
        }

        public NewStyleDialog(Guid contextGuid)
            : this()
        {

        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public PosStyle PosStyle
        {
            get
            {
                return posStyle;
            }
        }

        private void btnOK_Click(object ender, EventArgs e)
        {
            StyleDialog dlg;
            posStyle = new PosStyle();
            if (cmbCopyFromExistingStyle.SelectedIndex != -1 && cmbCopyFromExistingStyle.SelectedItem is PosStyle)
            {
                PosStyle copyFromStyle = cmbCopyFromExistingStyle.SelectedItem as PosStyle;
                posStyle = copyFromStyle;
            }
           
            posStyle.ID = RecordIdentifier.Empty;
            posStyle.Guid = Guid.Empty;
            posStyle.Text = tbDescription.Text;
            posStyle.IsSystemStyle = false;
            
            dlg = new StyleDialog(posStyle);

            if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }


            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void LoadCopyFrom()
        {
            cmbCopyFromExistingStyle.Items.Clear();

            List<PosStyle> styles = Providers.PosStyleData.GetList(PluginEntry.DataModel, "NAME ASC");

            foreach (var style in styles)
            {
                cmbCopyFromExistingStyle.Items.Add(style);
            }

            cmbCopyFromExistingStyle.SelectedIndex = -1;
        }

        private void CheckEnabled(object sender, EventArgs args)
        {
            btnOK.Enabled = tbDescription.Text != "";
        }


        private void cmbCopyFrom_RequestData(object sender, EventArgs e)
        {
            var styleProfiles = Providers.PosStyleData.GetList(PluginEntry.DataModel, "NAME ASC");
        }
    }
}

