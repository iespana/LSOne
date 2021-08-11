using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.SiteService.Dialogs
{
    public partial class ShorthandDialog : DialogBase
    {
        private RecordIdentifier profileID;
        private ShorthandItem orgShortHandItem;

        public ShorthandDialog()
        {
            InitializeComponent();
            orgShortHandItem = new ShorthandItem();
        }

        public ShorthandDialog(RecordIdentifier profileID) : this()
        {
            this.profileID = profileID;
            chkCreateAnother.Checked = true;
        }

        public ShorthandDialog(ShorthandItem shorthandItem) : this(shorthandItem.ProfileID)
        {
            orgShortHandItem = shorthandItem;
            tbShorthand.Text = shorthandItem.Text;
            chkCreateAnother.Visible = false;
            chkCreateAnother.Checked = false;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            
            if (Providers.ShortHandItemData.Exists(PluginEntry.DataModel, profileID, tbShorthand.Text))
            {
                errorProvider1.SetError(tbShorthand, Properties.Resources.ThisShorthandTextAlreadyExists);
                return;
            }

            ShorthandItem item = new ShorthandItem
            {
                ID = orgShortHandItem.ID,
                ProfileID = profileID,
                Text = tbShorthand.Text
            };

            Providers.ShortHandItemData.Save(PluginEntry.DataModel, item);
            PluginEntry.Framework.ViewController.NotifyDataChanged(null, chkCreateAnother.Visible ? DataEntityChangeType.Add : DataEntityChangeType.Edit, "ShortHandItem", profileID, item);

            if (chkCreateAnother.Checked)
            {
                tbShorthand.Text = "";
                tbShorthand.Focus();
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void ShorthandDialog_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && ActiveControl is TextBox)
            {
                if (!string.IsNullOrEmpty(tbShorthand.Text))
                {
                    btnOK_Click(sender, e);
                }
            }
        }

        private void tbShorthand_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            btnOK.Enabled = !string.IsNullOrEmpty(tbShorthand.Text);
        }

        
    }
}
