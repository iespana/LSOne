using System;
using System.Collections.Generic;
using LSOne.Controls;
using LSOne.Controls.SupportClasses;

namespace LSOne.Services.Dialogs
{
    public partial class MenuTypeSelectionDialog : TouchBaseForm
    {
        private bool allowMultiSelection;

        public List<string> SelectedMenuTypes { get; }

        public MenuTypeSelectionDialog(List<string> menuTypeNames, string caption, bool allowMultiSelection = false)
        {
            InitializeComponent();

            SelectedMenuTypes = new List<string>();
            
            menuTypeNames.ForEach(name => buttonPanelMenuTypes.AddButton(name, name, name));

            dialogHeader.BannerText = caption;
            this.Height = dialogHeader.Height + buttonPanelMenuTypes.TotalHeightInUse + btnCancel.Height + 27 + 10;

            this.allowMultiSelection = allowMultiSelection;
            if (!allowMultiSelection)
            {
                btnClear.Visible = btnPrintAll.Visible = btnOK.Visible = false;
            }
        }

        private void buttonPanelMenuTypes_Click(object sender, ScrollButtonEventArguments args)
        {
            if (allowMultiSelection)
            {
                ToggleMenuTypeButtonSelected(args.Key);
            }
            else
            {
                SelectedMenuTypes.Add(args.Key);
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
        }

        private void ToggleMenuTypeButtonSelected(string key)
        {
            if (SelectedMenuTypes.Contains(key))
            {
                SelectedMenuTypes.Remove(key);
                buttonPanelMenuTypes.SetButtonType(key, TouchButtonType.Normal);
            }
            else
            {
                SelectedMenuTypes.Add(key);
                buttonPanelMenuTypes.SetButtonType(key, TouchButtonType.OK);
            }

            btnOK.Enabled = (SelectedMenuTypes.Count > 0);
        }

        private void btnPrintAll_Click(object sender, EventArgs e)
        {
            SelectedMenuTypes.Clear();
            SelectedMenuTypes.Add(Properties.Resources.PrintAll);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            foreach (var menuTypeName in SelectedMenuTypes)
            {
                buttonPanelMenuTypes.SetButtonType(menuTypeName, TouchButtonType.Normal);
            }

            SelectedMenuTypes.Clear();
            btnOK.Enabled = false;
        }
    }
}
