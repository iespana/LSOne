using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Images;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.LookupValues.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Image = LSOne.DataLayer.BusinessObjects.Images.Image;

namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    public partial class ImageBankSelectDialog : DialogBase
    {
        public Image Image;
        private RecordIdentifier imageID = null;
        private static Guid BarSettingID = new Guid("DEBE6AC2-7FF3-4295-92E4-C52E5E675E71");
        private RecordIdentifier selectedID = "";
        private Setting searchBarSetting;
        private int maxRowHeight = 200;
        private ImageTypeEnum? defaultImageType = null;

        public ImageBankSelectDialog(ImageTypeEnum defaultImageType)
        {
            this.defaultImageType = defaultImageType;

            InitializeComponent();

            lvImages.ContextMenuStrip = new ContextMenuStrip();
            lvImages.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            lvImages.SetSortColumn(0, true);

            searchBar.BuddyControl = lvImages;
            searchBar.FocusFirstInput();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        #region Events

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvImages.ContextMenuStrip;

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                    Resources.EditCmd,
                    100,
                    btnsContextButtons_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsContextButtons.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Resources.Add,
                   200,
                   btnsContextButtons_AddButtonClicked)
            {
                Enabled = btnsContextButtons.AddButtonEnabled,
                Image = ContextButtons.GetAddButtonImage()
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Delete,
                    300,
                    btnsContextButtons_RemoveButtonClicked)
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = btnsContextButtons.RemoveButtonEnabled
            };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("ImageBankList", lvImages.ContextMenuStrip, lvImages);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void LoadItems(RecordIdentifier imageID)
        {
            string description = null;
            bool descriptionBeginsWith = true;
            ImageTypeEnum? imageType = null;
            string sortBy = string.Empty;

            RecordIdentifier currentlySelectedID = imageID ?? selectedID;
            lvImages.ClearRows();
            selectedID = currentlySelectedID;

            sortBy = "ORDER BY i.DESCRIPTION " + (lvImages.SortedAscending ? "ASC" : "DESC");

            List<SearchParameterResult> results = searchBar.SearchParameterResults;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Description":
                        description = result.StringValue;
                        descriptionBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;

                    case "Type":
                        switch (result.ComboSelectedIndex)
                        {
                            case 0:
                                imageType = null;
                                break;
                            case 1:
                                imageType = ImageTypeEnum.Button;
                                break;
                            case 2:
                                imageType = ImageTypeEnum.Logo;
                                break;
                            case 3:
                                imageType = ImageTypeEnum.ReceiptLogo;
                                break;
                            case 4:
                                imageType = ImageTypeEnum.Inventory;
                                break;
                            case 5:
                                imageType = ImageTypeEnum.Other;
                                break;
                        }
                        break;
                }
            }

            List<Image> images = Providers.ImageData.SearchList(PluginEntry.DataModel, description, descriptionBeginsWith, imageType, sortBy);

            foreach (Image img in images)
            {
                Row row = new Row();

                row.Height = GetRowHeight(img.Picture.Height);
                row.AddText(img.Text);
                row.AddCell(new Controls.Cells.ImageCell(img.Picture, img.BackColor, 10));

                row.Tag = img;
                lvImages.AddRow(row);

                if (selectedID == img.ID)
                {
                    lvImages.Selection.Set(lvImages.RowCount - 1);
                }
            }

            lvImages_SelectionChanged(this, EventArgs.Empty);
            lvImages.ShowRowOnScreen = true;
        }

        private short GetRowHeight(int imageHeight)
        {
            int diff = imageHeight + 10 > maxRowHeight ? imageHeight + 10 - maxRowHeight : 0;
            int padding = diff > 10 ? 10 : diff;
            short height = (short)(imageHeight > maxRowHeight ? maxRowHeight : imageHeight + padding);

            if (height < 22) //min height
                height = 22;

            return height;
        }

        private void lvImages_HeaderClicked(object sender, Controls.EventArguments.ColumnEventArgs args)
        {
            if (lvImages.SortColumn == args.Column)
            {
                lvImages.SetSortColumn(args.Column, !lvImages.SortedAscending);
            }
            else
            {
                lvImages.SetSortColumn(args.Column, true);
            }

            LoadItems(null);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Image = (Image)lvImages.Selection[0].Tag;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void lvImages_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnsContextButtons_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void lvImages_SelectionChanged(object sender, EventArgs e)
        {
            selectedID = (lvImages.Selection.Count == 1) ? ((Image)lvImages.Selection[0].Tag).ID : RecordIdentifier.Empty;

            btnsContextButtons.EditButtonEnabled = lvImages.Selection.Count == 1 && PluginEntry.DataModel.HasPermission(Permission.ManageImageBank);
            btnsContextButtons.RemoveButtonEnabled = lvImages.Selection.Count == 1 && !((Image)lvImages.Selection[0].Tag).IsImageUsed && PluginEntry.DataModel.HasPermission(Permission.ManageImageBank);
            btnOK.Enabled = lvImages.Selection.Count == 1;
        }

        private void btnsContextButtons_AddButtonClicked(object sender, EventArgs e)
        {
            imageID = PluginOperations.AddEditImage(RecordIdentifier.Empty);
            LoadItems(imageID);
        }

        private void btnsContextButtons_EditButtonClicked(object sender, EventArgs e)
        {
            imageID = PluginOperations.AddEditImage(selectedID);
            LoadItems(imageID);
        }

        private void btnsContextButtons_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(Resources.DeleteImageQuestion) == DialogResult.Yes)
            {
                Providers.ImageData.Delete(PluginEntry.DataModel, selectedID);
                LoadItems(null);
            }
        }

        #endregion

        #region SearchBar

        private void searchBar_LoadDefault(object sender, EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, BarSettingID, SettingType.UISetting, "");

            if (searchBarSetting != null && searchBarSetting.LongUserSetting != "")
            {
                searchBar.LoadFromSetupString(searchBarSetting.LongUserSetting);
            }
        }

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            LoadItems(null);
        }

        private void searchBar_SearchOptionChanged(object sender, EventArgs e)
        {
            searchBar_SearchClicked(sender, e);
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            List<object> imageTypes = new List<object>
            {
                Resources.ImageTypeAll, Resources.ImageTypeButton, Resources.ImageTypeLogo, Resources.ImageTypeReceiptLogo, Resources.ImageTypeInventory, Resources.ImageTypeOther
            };

            searchBar.AddCondition(new ConditionType(Resources.ImageDescription, "Description", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Resources.ImageType, "Type", ConditionType.ConditionTypeEnum.ComboBox, imageTypes, GetImageTypeSearchIndex(), 0, false));

            searchBar_LoadDefault(this, EventArgs.Empty);
        }

        private void searchBar_SaveAsDefault(object sender, EventArgs e)
        {
            string layoutString = searchBar.LayoutStringWithData;

            if (searchBarSetting.LongUserSetting != layoutString)
            {
                searchBarSetting.LongUserSetting = layoutString;
                searchBarSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, BarSettingID, SettingsLevel.User, searchBarSetting);
            }
        }

        #endregion

        private int GetImageTypeSearchIndex()
        {
            if (defaultImageType == null)
                return 0;

            switch (defaultImageType)
            {
                case ImageTypeEnum.Button:
                    return 1;
                case ImageTypeEnum.Logo:
                    return 2;
                case ImageTypeEnum.ReceiptLogo:
                    return 3;
                case ImageTypeEnum.Inventory:
                    return 4;
                case ImageTypeEnum.Other:
                    return 5;
                default:
                    return 0;
            }
        }

        private void ImageBankSelectDialog_Load(object sender, EventArgs e)
        {
            LoadItems(null);
        }
    }
}
