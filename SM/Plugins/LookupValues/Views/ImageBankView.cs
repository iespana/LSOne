using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.Controls;
using LSOne.ViewPlugins.LookupValues.Properties;
using LSOne.DataLayer.GenericConnector.Enums;
using Image = LSOne.DataLayer.BusinessObjects.Images.Image;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.BusinessObjects.Images;
using LSOne.Controls.Rows;
using LSOne.ViewCore.Dialogs;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.LookupValues.Views
{
    public partial class ImageBankView : ViewBase
    {
        private static Guid BarSettingID = new Guid("AFD96453-6972-4430-B7E6-555448848B4F");
        private RecordIdentifier selectedID = "";
        private Setting searchBarSetting;
        private int maxRowHeight = 200;
        private ImageTypeEnum? initialImageTypeFilter = null;

        public ImageBankView()
        {
            InitializeComponent();

            Attributes =
                    ViewAttributes.Help |
                    ViewAttributes.Close |
                    ViewAttributes.ContextBar;

            HeaderText = Resources.ImageBank;

            lvImageBank.ContextMenuStrip = new ContextMenuStrip();
            lvImageBank.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            lvImageBank.SetSortColumn(0, true);

            searchBar.BuddyControl = lvImageBank;
            searchBar.FocusFirstInput();

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageImageBank);
        }

        public ImageBankView(ImageTypeEnum imageType)
            : this()
        {
            initialImageTypeFilter = imageType;
        }

        protected override string LogicalContextName
        {
            get
            {
                return Resources.ImageBank;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            ShowProgress((sender1, e1) => LoadItems(null), GetLocalizedSearchingText());
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if(objectName == "ImageBank")
            {
                ShowProgress((sender1, e1) => LoadItems(changeIdentifier), GetLocalizedSearchingText());
            }
        }

        private void LoadItems(RecordIdentifier imgToSelect)
        {
            string description = null;
            bool descriptionBeginsWith = true;
            ImageTypeEnum? imageType = null;
            string sortBy = string.Empty;

            RecordIdentifier selectedImg = imgToSelect ?? selectedID;
            lvImageBank.ClearRows();
            selectedID = selectedImg;

            sortBy = GetSortString();

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

            foreach(Image img in images)
            {
                Row row = new Row();

                row.Height = GetRowHeight(img.Picture.Height); ;
                row.AddText(img.Text);
                row.AddText(ImageTypeHelper.ImageTypeEnumToString(img.ImageType));
                row.AddCell(new Controls.Cells.ImageCell(img.Picture, img.BackColor, 10));

                row.Tag = img;
                lvImageBank.AddRow(row);

                if (selectedID == img.ID)
                {
                    lvImageBank.Selection.Set(lvImageBank.RowCount - 1);
                }
            }

            lvImageBank_SelectionChanged(this, EventArgs.Empty);
            lvImageBank.AutoSizeColumns(true);
            lvImageBank.ApplyRelativeColumnSize();
            lvImageBank.ShowRowOnScreen = true;
            HideProgress();
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

        private string GetSortString()
        {
            if (lvImageBank.SortColumn == null)
            {
                lvImageBank.SetSortColumn(0, true);
            }

            int sortColumnIndex = lvImageBank.Columns.IndexOf(lvImageBank.SortColumn);

            string sortBy = "ORDER BY ";

            switch(sortColumnIndex)
            {
                case 0: //Description
                    sortBy += "i.DESCRIPTION " + (lvImageBank.SortedAscending ? "ASC" : "DESC");
                    break;
                case 1: //Type
                    sortBy += "i.TYPEOFIMAGE " + (lvImageBank.SortedAscending ? "ASC" : "DESC");
                    break;
                default:
                    sortBy = string.Empty;
                    break;
            }

            return sortBy;
        }

        #region Events

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvImageBank.ContextMenuStrip;

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

            PluginEntry.Framework.ContextMenuNotify("ImageBankList", lvImageBank.ContextMenuStrip, lvImageBank);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvImageBank_HeaderClicked(object sender, Controls.EventArguments.ColumnEventArgs args)
        {
            if (args.ColumnNumber == 2) //Image column
                return;

            if (lvImageBank.SortColumn == args.Column)
            {
                lvImageBank.SetSortColumn(args.Column, !lvImageBank.SortedAscending);
            }
            else
            {
                lvImageBank.SetSortColumn(args.Column, true);
            }

            ShowProgress((sender1, e1) => LoadItems(null), GetLocalizedSearchingText());
        }

        private void lvImageBank_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnsContextButtons_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void lvImageBank_SelectionChanged(object sender, EventArgs e)
        {
            selectedID = (lvImageBank.Selection.Count == 1) ? ((Image)lvImageBank.Selection[0].Tag).ID : RecordIdentifier.Empty;

            btnsContextButtons.EditButtonEnabled = lvImageBank.Selection.Count == 1 && PluginEntry.DataModel.HasPermission(Permission.ManageImageBank);
            btnsContextButtons.RemoveButtonEnabled = lvImageBank.Selection.Count == 1 && !((Image)lvImageBank.Selection[0].Tag).IsImageUsed && PluginEntry.DataModel.HasPermission(Permission.ManageImageBank);
        }

        private void btnsContextButtons_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.AddEditImage(RecordIdentifier.Empty);
        }

        private void btnsContextButtons_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.AddEditImage(selectedID);
        }

        private void btnsContextButtons_RemoveButtonClicked(object sender, EventArgs e)
        {
            if(QuestionDialog.Show(Resources.DeleteImageQuestion) == DialogResult.Yes)
            {
                Providers.ImageData.Delete(PluginEntry.DataModel, selectedID);
                LoadData(false);
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
            ShowProgress((sender1, e1) => LoadItems(null), GetLocalizedSearchingText());
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
            searchBar.AddCondition(new ConditionType(Resources.ImageType, "Type", ConditionType.ConditionTypeEnum.ComboBox, imageTypes, initialImageTypeFilter != null ? (int)initialImageTypeFilter : 0, 0, false));

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

        private void lvImageBank_ClientSizeChanged(object sender, EventArgs e)
        {
            lvImageBank.ApplyRelativeColumnSize();
        }
    }
}
