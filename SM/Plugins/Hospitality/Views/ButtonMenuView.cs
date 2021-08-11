using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.ViewCore;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Enums;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.EventArguments;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.Views
{
    public partial class ButtonMenuView : ViewBase
    {
        private RecordIdentifier posMenuHeaderID = RecordIdentifier.Empty;
        private PosMenuHeader posMenuHeader;

        private TabControl.Tab lookAndFeelTab;
        private TabControl.Tab buttonsTab;

        public ButtonMenuView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close;
        }

        public ButtonMenuView(RecordIdentifier posMenuHeaderID) : this()
        {
            this.posMenuHeaderID = posMenuHeaderID;
            ReadOnly = !PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.EditPosMenus);
        }

        protected override HelpSettings GetOnlineHelpSettings()
        {
            return new HelpSettings("HospButtonMenuView");
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("PosMenuHeader", posMenuHeaderID, Properties.Resources.PosMenuHeader, true));
        }

        public string Description
        {
            get
            {
                return Properties.Resources.PosMenuHeader + ": " + tbID.Text + " - " + tbDescription.Text;
            }
        }

        public override string ContextDescription
        {
            get
            {
                return tbDescription.Text;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.PosMenuHeader;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return posMenuHeaderID;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            if (!isRevert)
            {
                lookAndFeelTab = new TabControl.Tab(Properties.Resources.LookAndFeel, ViewPages.PosMenuLookAndFeelPage.CreateInstance);
                buttonsTab = new TabControl.Tab(Properties.Resources.MenuButtons, ViewPages.PosMenuButtonsPage.CreateInstance);

                tabSheetTabs.AddTab(lookAndFeelTab);
                tabSheetTabs.AddTab(buttonsTab);

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, posMenuHeaderID);
            }

            posMenuHeader = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, posMenuHeaderID);

            tbID.Text = posMenuHeader.ID.StringValue;
            tbDescription.Text = posMenuHeader.Text;
            ntColumns.Value = posMenuHeader.Columns;
            ntRows.Value = posMenuHeader.Rows;
            chkMainMenu.Checked = posMenuHeader.MainMenu;
            cmbAppliesTo.SelectedIndex = (int)posMenuHeader.AppliesTo;
            chkUseNavOperation.Checked = posMenuHeader.UseNavOperation;

            HeaderText = Description;

            tabSheetTabs.SetData(isRevert, posMenuHeaderID, posMenuHeader);
            tabSheetTabs.LoadAllTabs();
        }

        protected override bool SaveData()
        {
            tabSheetTabs.GetData();

            //Check if can add new lines
            int prevTotalLines = posMenuHeader.Rows * posMenuHeader.Columns;
            int curTotalLines = (int)ntColumns.Value * (int)ntRows.Value;
            int actualLines = Providers.PosMenuLineData.GetList(PluginEntry.DataModel, posMenuHeaderID).Count;


            if (curTotalLines > prevTotalLines && actualLines < curTotalLines)
            {
                int remaningSpace = curTotalLines - actualLines;

                if (QuestionDialog.Show(Properties.Resources.AddPosButtonGridMenuLinesQuestion.Replace("#1", Convert.ToString(remaningSpace))) == DialogResult.Yes)
                {
                    for (int i = 0; i < remaningSpace; i++)
                    {
                        PosMenuLine newLine = new PosMenuLine();

                        newLine.Sequence = RecordIdentifier.Empty;
                        newLine.MenuID = posMenuHeaderID;
                        newLine.Text = "";
                        newLine.Operation = RecordIdentifier.Empty;

                        newLine.FontName = posMenuHeader.FontName;
                        newLine.FontSize = posMenuHeader.FontSize;
                        newLine.FontBold = posMenuHeader.FontBold;
                        newLine.ForeColor = posMenuHeader.ForeColor;
                        newLine.BackColor = posMenuHeader.BackColor;
                        newLine.FontItalic = posMenuHeader.FontItalic;
                        newLine.FontCharset = posMenuHeader.FontCharset;
                        newLine.BackColor2 = posMenuHeader.BackColor2;
                        newLine.GradientMode = posMenuHeader.GradientMode;
                        newLine.Shape = posMenuHeader.Shape;
                        newLine.UseHeaderAttributes = true;
                        newLine.UseHeaderFont = true;

                        Providers.PosMenuLineData.Save(PluginEntry.DataModel, newLine);
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "PosMenuHeader", posMenuHeaderID, null);
                }
            }

            posMenuHeader.Text = tbDescription.Text;
            posMenuHeader.MainMenu = chkMainMenu.Checked;
            posMenuHeader.Columns = (int)ntColumns.Value;
            posMenuHeader.Rows = (int)ntRows.Value;
            posMenuHeader.AppliesTo = (PosMenuHeader.AppliesToEnum)cmbAppliesTo.SelectedIndex;
            posMenuHeader.UseNavOperation = chkUseNavOperation.Checked;

            Providers.PosMenuHeaderData.Save(PluginEntry.DataModel, posMenuHeader);
            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "PosMenuHeader", posMenuHeaderID, null);

            return true;
        }

        protected override bool DataIsModified()
        {
            if (tbDescription.Text != posMenuHeader.Text
                || chkMainMenu.Checked != posMenuHeader.MainMenu
                || ntColumns.Value != (double)posMenuHeader.Columns
                || ntRows.Value != (double)posMenuHeader.Rows)
            {
                return true;
            }

            if (tabSheetTabs.IsModified())
                return true;

            return false;
        }

        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if(objectName == "PosMenuLine")
            {
                tabSheetTabs.BroadcastChangeInformation(changeAction, objectName, changeIdentifier, param);
            }

            if(objectName == "PosMenuHeader" && changeAction == DataEntityChangeType.Delete && changeIdentifier == posMenuHeaderID)
            {
                PluginEntry.Framework.ViewController.DiscardCurrentView(this);
            }
        }

        protected override void OnDelete()
        {
            PluginOperations.DeletePosMenu(posMenuHeaderID);
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == base.GetType() + ".Actions")
            {
                ContextBarItem previewAction = new ContextBarItem(Properties.Resources.Preview, PreviewAction);
                arguments.Add(previewAction, 10);
            }

            base.OnSetupContextBarItems(arguments);
        }

        protected override void OnSetupContextBarHeaders(ContextBarHeaderConstructionArguments arguments)
        {
            arguments.Add(new ContextBarHeader(Properties.Resources.Actions, GetType().ToString() + ".Actions"), 200);

            base.OnSetupContextBarHeaders(arguments);
        }

        private void PreviewAction(object sender, ContextBarClickEventArguments args)
        {
            new Dialogs.PosMenuPreviewDialog(posMenuHeaderID).ShowDialog();
        }
    }
}
