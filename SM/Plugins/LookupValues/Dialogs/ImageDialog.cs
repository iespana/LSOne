using LSOne.DataLayer.BusinessObjects.Images;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Image = LSOne.DataLayer.BusinessObjects.Images.Image;

namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    public partial class ImageDialog : DialogBase
    {
        private Image image;
        private bool addingNewImage = true;
        private List<PosStyle> styles;

        public RecordIdentifier ImageID { get; private set; }

        public ImageDialog()
        {
            InitializeComponent();
        }

        public ImageDialog(RecordIdentifier imageID) : this()
        {
            this.ImageID = imageID;

            this.Text = imageID == RecordIdentifier.Empty ? Properties.Resources.NewImageHeader : Properties.Resources.EditImageHeader;
            addingNewImage = imageID == RecordIdentifier.Empty;

            cmbType.Items.Clear();
            foreach (object enumItem in Enum.GetValues(typeof(ImageTypeEnum)))
            {
                cmbType.Items.Add(ImageTypeHelper.ImageTypeEnumToString((ImageTypeEnum)Convert.ToByte(enumItem)));
            }
            cmbType.SelectedIndex = 0;

            if(imageID != RecordIdentifier.Empty)
            {
                addingNewImage = false;

                image = Providers.ImageData.Get(PluginEntry.DataModel, imageID);
                txtDescription.Text = image.Text;
                cmbType.SelectedIndex = (int)image.ImageType;
                
                if(image.BackgroundStyle != null && image.BackgroundStyle.StringValue != string.Empty)
                {
                    PosStyle style = Providers.PosStyleData.Get(PluginEntry.DataModel, image.BackgroundStyle);
                    cmbBackground.SelectedData = style;
                    imageBox.BackColor = Color.FromArgb(style.BackColor);
                }
                
                SetImage(image.Picture);
            }

            btnOK.Enabled = false;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public bool CanSave()
        {
            if (addingNewImage)
            {
                return txtDescription.Text.Trim() != string.Empty && imageBox.BackgroundImage != null;
            }
            else
            {
                return image != null &&
                       txtDescription.Text.Trim() != string.Empty &&
                       (txtDescription.Text.Trim() != image.Text ||
                       cmbType.SelectedIndex != (int)image.ImageType ||
                       imageBox.BackgroundImage != image.Picture ||
                       (cmbBackground.SelectedData != null && cmbBackground.SelectedData.ID != image.BackgroundStyle)); 
            }
        }

        private void SetImage(System.Drawing.Image image)
        {
            imageBox.BackgroundImageLayout = image.Width > imageBox.Width || image.Height > imageBox.Height ? ImageLayout.Zoom : ImageLayout.Center;
            imageBox.BackgroundImage = image;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(Providers.ImageData.Exists(PluginEntry.DataModel, txtDescription.Text.Trim()) && (addingNewImage || txtDescription.Text.Trim() != image.Text))
            {
                errorProvider.SetError(txtDescription, Properties.Resources.ImageDescriptionExists);
                return;
            }

            if(image == null)
            {
                image = new Image();
                image.ID = RecordIdentifier.Empty;
            }

            image.Text = txtDescription.Text.Trim();
            image.ImageType = (ImageTypeEnum)cmbType.SelectedIndex;
            image.Picture = imageBox.BackgroundImage;
            image.BackgroundStyle = cmbBackground.SelectedData?.ID;
            Providers.ImageData.Save(PluginEntry.DataModel, image);
            ImageID = image.ID;
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, ViewCore.Enums.DataEntityChangeType.Edit, "ImageBank", image.ID, null);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = CanSave();
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = CanSave();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.bmp, *.png, *.jpg, *.jpeg;)| *.bmp; *.png;*.jpg;*.jpeg;";

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                txtImage.Text = ofd.FileName;
                SetImage(System.Drawing.Image.FromFile(ofd.FileName));
                btnOK.Enabled = CanSave();
            }
        }

        private void cmbBackground_SelectedDataChanged(object sender, EventArgs e)
        {
            if(styles != null && cmbBackground.SelectedData.ID.StringValue != string.Empty)
            {
                imageBox.BackColor = Color.FromArgb(styles.Single(x => x.ID == cmbBackground.SelectedData.ID).BackColor);
            }
            else
            {
                imageBox.BackColor = Color.FromArgb(-1);
            }

            btnOK.Enabled = CanSave();
        }

        private void cmbBackground_RequestData(object sender, EventArgs e)
        {
            styles = Providers.PosStyleData.GetList(PluginEntry.DataModel);
            cmbBackground.SetData(styles, null);
        }

        private void cmbBackground_RequestClear(object sender, EventArgs e)
        {

        }
    }
}
