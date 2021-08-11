using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    public partial class PosColorDialog : DialogBase
    {
        RecordIdentifier selectedID;
        bool lockEvents;
        PosColor color;

        public PosColorDialog()
        {
            InitializeComponent();

            color = null;
            lockEvents = false;
            selectedID = RecordIdentifier.Empty;

            lvColors.ContextMenuStrip = new ContextMenuStrip();
            lvColors.ContextMenuStrip.Opening += new CancelEventHandler(lvColors_Opening);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadList();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Avoid the Microsoft memory leak error on ListViews
            lvColors.SmallImageList = null;
        }

        private void LoadList()
        {
            List<PosColor> colors;
            ListViewItem item;

            lvColors.Items.Clear();

            colors = Providers.PosColorData.GetAllColors(PluginEntry.DataModel);

            foreach (PosColor color in colors)
            {
                item = new ListViewItem((string)color.ColorCode);
                item.SubItems.Add(color.Text);
                item.Tag = color.ColorCode;

                lvColors.Add(item);
            }

            lvColors.BestFitColumns();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnClose.Enabled = true;

            if (selectedID == RecordIdentifier.Empty)
            {
                lvColors.Items.RemoveAt(lvColors.Items.Count - 1);
            }

            lvColors.Enabled = true;
            btnsAddRemove.AddButtonEnabled= true;

            lvColors_SelectedIndexChanged(this, EventArgs.Empty);

            btnCancel.Enabled = false;
            btnSave.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            color = new PosColor();

            if (selectedID != RecordIdentifier.Empty)
            {
                color.ID = selectedID;
            }
            
            color.Text = tbName.Text;
            color.Bold = chkBold.Checked;
            color.Color = colorWellColorValue.SelectedColor.ToArgb();

            Providers.PosColorData.Save(PluginEntry.DataModel, color);

            lvColors.SelectedItems[0].Text = (string)color.ID;
            lvColors.SelectedItems[0].SubItems[1].Text = tbName.Text;
            selectedID = color.ID;
            lvColors.SelectedItems[0].Tag = selectedID;

            lvColors.SelectedItems[0].Selected = false;

            btnClose.Enabled = true;
            lvColors.Enabled = true;            
            btnsAddRemove.AddButtonEnabled = true;

            tbName.Text = "";
            chkBold.Checked = false;
            colorWellColorValue.SelectedColor = Color.White;

            lvColors_SelectedIndexChanged(this, EventArgs.Empty);

            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            
            RemoveEditMode();
        }

        private void lvColors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvColors.SelectedItems.Count > 0)
            {
                selectedID = (RecordIdentifier)lvColors.SelectedItems[0].Tag;

                lblName.Visible = lblBold.Visible = lblColorValue.Visible = true;
                tbName.Visible =  chkBold.Visible = colorWellColorValue.Visible = true;
                
                btnSave.Visible = btnCancel.Visible = true;

                lblNoSelection.Visible = false;

                lockEvents = true;

                if (selectedID == RecordIdentifier.Empty)
                {
                    tbName.Focus();

                    btnCancel.Enabled = true;
                    btnSave.Enabled = false;

                    tbName.Text = "";
                    chkBold.Checked = false;
                    colorWellColorValue.SelectedColor = Color.White;

                    SetEditMode();
                    
                    btnsAddRemove.RemoveButtonEnabled = false;
                }
                else
                {
                    color = Providers.PosColorData.GetColor(PluginEntry.DataModel, selectedID);

                    tbName.Text = color.Text;
                    chkBold.Checked = color.Bold;
                    colorWellColorValue.SelectedColor = Color.FromArgb(color.Color);


                    btnCancel.Enabled = false;
                    btnSave.Enabled = false;
                    
                    btnsAddRemove.RemoveButtonEnabled = true;
                }

                lockEvents = false;
            }
            else
            {
                lblName.Visible = lblBold.Visible = lblColorValue.Visible = false;
                tbName.Visible = chkBold.Visible = colorWellColorValue.Visible = false;
                btnSave.Visible = btnCancel.Visible = false;

                lblNoSelection.Visible = true;

                btnsAddRemove.RemoveButtonEnabled = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // We do this check in case if the event originates from escape key
            if (!btnClose.Enabled)
            {
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void btnsAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            ListViewItem item = new ListViewItem("");
            item.SubItems.Add("");
            item.Tag = RecordIdentifier.Empty;

            lvColors.Add(item);
            item.Selected = true;
            lvColors.EnsureVisible(lvColors.Items.Count - 1);

            btnCancel.Enabled = true;
        }

        private void SetEditMode()
        {            
            btnsAddRemove.AddButtonEnabled = false;            
            btnsAddRemove.RemoveButtonEnabled = false;
            btnClose.Enabled = false;

            lvColors.Enabled = false;
        }

        private void RemoveEditMode()
        {
            btnsAddRemove.AddButtonEnabled = true;
            btnsAddRemove.RemoveButtonEnabled = true;
            btnClose.Enabled = true;

            lvColors.Enabled = true;
        }

        private void CheckSaveEnabled(object sender, EventArgs args)
        {
            if (lockEvents) return;

            if (selectedID == RecordIdentifier.Empty)
            {
                // Creating new item
                btnSave.Enabled = tbName.Text.Length > 0;
                btnCancel.Enabled = true;
            }
            else
            {
                // Editing existing
                btnSave.Enabled = btnCancel.Enabled = (tbName.Text.Length > 0) && (tbName.Text != color.Text || chkBold.Checked != color.Bold || colorWellColorValue.SelectedColor.ToArgb() != color.Color);
            }

            if (btnSave.Enabled && lvColors.Enabled)
            {
                SetEditMode();
            }
            else if (btnSave.Enabled == false && lvColors.Enabled == false && selectedID != RecordIdentifier.Empty)
            {
                RemoveEditMode();
            }
        }

        private void btnsAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            RecordIdentifier colorID = (RecordIdentifier)lvColors.SelectedItems[0].Tag;

            if (Providers.PosColorData.ColorIsInUse(PluginEntry.DataModel, colorID))
            {
                MessageDialog.Show(Properties.Resources.PosColorIsInUse);
                return;
            }

            if (QuestionDialog.Show(
                Properties.Resources.DeleteColorQuestion,
                Properties.Resources.DeleteColor) == DialogResult.Yes)
            {

                Providers.PosColorData.Delete(PluginEntry.DataModel, colorID);

                lvColors.Items.Remove(lvColors.SelectedItems[0]);

                lvColors_SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        void lvColors_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuitem item;
            ContextMenuStrip menu;

            menu = lvColors.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here

            item = new ExtendedMenuitem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(btnsAddRemove_AddButtonClicked));

            item.Enabled = btnsAddRemove.AddButtonEnabled;

            item.Image = btnsAddRemove.AddButtonImage;

            menu.Items.Add(item);

            item = new ExtendedMenuitem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnsAddRemove_RemoveButtonClicked));

            item.Image = btnsAddRemove.RemoveButtonImage;

            item.Enabled = (lvColors.SelectedItems.Count != 0);

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("PosColorList", lvColors.ContextMenuStrip, lvColors);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void tbID_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            CheckSaveEnabled(this, EventArgs.Empty);
        }

        private void colorWellColorValue_Load(object sender, EventArgs e)
        {

        }
        
    }
}
