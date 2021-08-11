using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Store.ViewPages
{
    public partial class CompanyInfoLogoPage : UserControl, ITabView
    {
        CompanyInfo companyInfo;
        private bool logoPictureChanged = false;

        public CompanyInfoLogoPage()
        {
            InitializeComponent();
            pictureBox1.ContextMenuStrip = new ContextMenuStrip();
            pictureBox1.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ContextMenuStrip menu = pictureBox1.ContextMenuStrip;
            menu.Items.Clear();

            // We can optionally add our own items right here
            ExtendedMenuItem item = new ExtendedMenuItem(
                contextButton1.Context == ButtonType.Add ? Properties.Resources.Add : Properties.Resources.EditCmd,
                100,
                contextButton1_Click)
            {
                Enabled = contextButton1.Enabled,
                Image = ContextButtons.GetAddButtonImage()
        };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.Delete,
                200,
                contextButton2_Click)
            {
                Enabled = contextButton2.Enabled,
                Image = ContextButtons.GetRemoveButtonImage()
        };

            menu.Items.Add(item);

            e.Cancel = (menu.Items.Count == 0);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.CompanyInfoLogoPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            companyInfo = (CompanyInfo)internalContext;

            pictureBox1.Image = companyInfo.CompanyLogo;

            if (companyInfo.CompanyLogo == null)
            {
                this.contextButton1.Context = ButtonType.Add;
                this.contextButton2.Enabled = false;
            }
            else
            {
                this.contextButton1.Context = ButtonType.Edit;
                this.contextButton2.Enabled = true;
            }
        }

        public bool DataIsModified()
        {


            return logoPictureChanged;

            
        }

        public bool SaveData()
        {
            companyInfo.Dirty = true;
            companyInfo.CompanyLogo = pictureBox1.Image;
            logoPictureChanged = false;

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void contextButton1_Click(object sender, EventArgs e)
        {
            //Ask user to select file.
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter =
                Properties.Resources.ImageFiles + " (*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png|" +
                Properties.Resources.PNGfiles + " (*.png)|*.png|" +
                Properties.Resources.JPEGfiles + " (*.jpg)|*.jpg|" +
                Properties.Resources.BMPfiles + "  (*.bmp)|*.bmp";

            DialogResult dlgRes = dlg.ShowDialog(PluginEntry.Framework.MainWindow);
            if (dlgRes != DialogResult.Cancel)
            {
                logoPictureChanged = true;
                //Set image in picture box
                pictureBox1.ImageLocation = dlg.FileName;
                this.contextButton1.Context = ButtonType.Edit;
                this.contextButton2.Enabled = true;
                this.contextButton1.Invalidate();
            }
        }

        private void contextButton2_Click(object sender, EventArgs e)
        {
            //if (QuestionDialog.Show(Properties.Resources.DeleteCompanyLogo, Properties.Resources.DeleteLogo) == DialogResult.Yes)
            //{
                logoPictureChanged = true;
                //Set image in picture box
                pictureBox1.Image = null;
                this.contextButton1.Context = ButtonType.Add;            
                this.contextButton2.Enabled = false;
                this.contextButton1.Invalidate();
            //}
        }

    }
}
