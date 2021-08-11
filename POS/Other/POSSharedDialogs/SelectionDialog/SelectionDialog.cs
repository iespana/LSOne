using System;
using System.Windows.Forms;
using LSOne.Controls.EventArguments;
using LSOne.Controls.SupportClasses;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Controls.Dialogs.SelectionDialog
{
    public partial class SelectionDialog : TouchBaseForm
    {
        private enum Buttons
        {
            SelectTop,
            SelectAbove,
            SelectBelow,
            SelectBottom,
            Clear,
            Search,
            Select,
            Cancel
        }

        private ISelectionListView selectionList;
        private bool inputRequired;

        public object SelectedItem { get; set; }

        public bool InputRequired
        {
            get
            {
                return inputRequired;
            }
            set
            {
                if(inputRequired != value)
                {
                    inputRequired = value;
                    InitializeButtons();
                }
            }
        }

        public string BannerText 
        { 
            get { return touchDialogBanner1.BannerText; }
            set { touchDialogBanner1.BannerText = value; }
        }

        public SelectionDialog()
        {
            InitializeComponent();
        }

        public SelectionDialog(ISelectionListView selectionList, string bannerText, bool inputRequired, bool standAlone = false, string initialSearch = "") : this()
        {
            touchKeyboard1.BuddyControl = tbSearch;

            this.selectionList = selectionList;
            this.touchDialogBanner1.BannerText = bannerText;
            this.inputRequired = inputRequired;
            this.ShowInTaskbar = standAlone;
            this.StartPosition = FormStartPosition.CenterScreen;

            InitializeButtons();
            selectionList.SetupListViewHeader(lvSelection);

            lvSelection.ApplyRelativeColumnSize();

            tbSearch.Text = initialSearch;
            Search();
        }

        public void LoadData()
        {
            selectionList.LoadData(lvSelection);
            lvSelection_SelectionChanged(this, EventArgs.Empty);
        }

        private void lvSelection_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (lvSelection.Selection.Count > 0)
            {
                SelectedItem = args.Row.Tag;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void Search()
        {
            if (tbSearch.Text == "")
            {
                touchScrollButtonPanel1.SetButtonEnabled(Conversion.ToStr((int)Buttons.Clear), false);
                LoadData();
                return;
            }

            selectionList.LoadData(lvSelection, tbSearch.Text);
            touchScrollButtonPanel1.SetButtonEnabled(Conversion.ToStr((int)Buttons.Clear), true);
        }

        private void touchKeyboard1_EnterPressed(object sender, EventArgs e)
        {
            Search();
        }

        private void touchKeyboard1_ObtainCultureName(object sender, CultureEventArguments args)
        {
            if (DLLEntry.DataModel.Settings == null)
            {
                return;
            }
            if (((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).UserProfile.KeyboardCode != "")
            {
                args.CultureName = ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).UserProfile.KeyboardCode;
                args.LayoutName = ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).UserProfile.KeyboardLayoutName;
            }
            else
            {
                args.CultureName = ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.KeyboardCode;
                args.LayoutName = ((ISettings)DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication)).Store.KeyboardLayoutName;
            } 
        }

        private void InitializeButtons()
        {
            touchScrollButtonPanel1.Clear();
            touchScrollButtonPanel1.AddButton("", Buttons.SelectTop, "", image: Properties.Resources.Doublearrowupthin_32px);
            touchScrollButtonPanel1.AddButton("", Buttons.SelectAbove, "", image: Properties.Resources.Arrowupthin_32px);
            touchScrollButtonPanel1.AddButton("", Buttons.SelectBelow, "", image: Properties.Resources.Arrowdownthin_32px);
            touchScrollButtonPanel1.AddButton("", Buttons.SelectBottom, "", image: Properties.Resources.Doublearrowdownthin_32px);

            touchScrollButtonPanel1.AddButton(Properties.Resources.Clear, Buttons.Clear, Convert.ToString((int)Buttons.Clear), TouchButtonType.Normal, DockEnum.DockEnd);
            touchScrollButtonPanel1.SetButtonEnabled(Conversion.ToStr((int)Buttons.Clear), false);

            touchScrollButtonPanel1.AddButton(Properties.Resources.Search, Buttons.Search, "", TouchButtonType.Action, DockEnum.DockEnd);
            touchScrollButtonPanel1.AddButton(Properties.Resources.Select, Buttons.Select, Convert.ToString((int)Buttons.Select), TouchButtonType.OK, DockEnum.DockEnd);
            touchScrollButtonPanel1.SetButtonEnabled(Conversion.ToStr((int)Buttons.Select), false);

            if (!InputRequired)
            {
                touchScrollButtonPanel1.AddButton(Properties.Resources.Cancel, Buttons.Cancel, "", TouchButtonType.Cancel, DockEnum.DockEnd);
            }
        }

        private void touchScrollButtonPanel1_Click(object sender, ScrollButtonEventArguments args)
        {
            if (args.Tag is Buttons)
            {
                switch ((Buttons)args.Tag)
                {
                    case Buttons.SelectTop:
                        lvSelection.ScrollPageUp();
                        break;
                    case Buttons.SelectAbove:
                        lvSelection.MoveSelectionUp();
                        break;
                    case Buttons.SelectBelow:
                        lvSelection.MoveSelectionDown();
                        break;
                    case Buttons.SelectBottom:
                        lvSelection.ScrollPageDown();
                        break;
                    case Buttons.Clear:
                        LoadData();
                        tbSearch.Text = "";
                        touchScrollButtonPanel1.SetButtonEnabled(Conversion.ToStr((int)Buttons.Clear), false);
                        break;
                    case Buttons.Search:
                        Search();
                        break;
                    case Buttons.Select:
                        if (lvSelection.Selection.Count > 0)
                        {
                            SelectedItem = lvSelection.Row(lvSelection.Selection.FirstSelectedRow).Tag;
                            DialogResult = DialogResult.OK;
                            Close();
                        }
                        break;
                    case Buttons.Cancel:
                        DialogResult = DialogResult.Cancel;
                        Close();
                        break;
                }
            }
        }

        private void lvSelection_SelectionChanged(object sender, EventArgs e)
        {
            touchScrollButtonPanel1.SetButtonEnabled(Conversion.ToStr((int)Buttons.Select), lvSelection.Selection.Count > 0);
        }
    }
}
