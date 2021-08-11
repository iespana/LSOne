using System;
using System.Collections.Generic;
using System.Drawing;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.Hospitality.Views
{
    public partial class ButtonView : ViewBase
    {
        private RecordIdentifier posMenuLineID = RecordIdentifier.Empty;
        private PosMenuLine posMenuLine;
        private PosMenuLine.ParameterTypeEnum curParameterType;
        private PosMenuHeader parentPosMenuHeader;

        private TabControl.Tab generalTab;
        private TabControl.Tab fontTab;
        private TabControl.Tab attributesTab;
        private TabControl.Tab glyphsTab;

        public ButtonView(RecordIdentifier posMenuLineID)
            : this()
        {
            this.posMenuLineID = posMenuLineID;
            this.ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.EditPosMenus);
        }

        public ButtonView()
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

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("PosMenuLine", posMenuLineID, Properties.Resources.PosMenuButton, true));
        }

        public string Description
        {
            get
            {
                return Properties.Resources.PosMenuButton + ": " + tbKeyNo.Text + " - " + tbDescription.Text;
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
                return Properties.Resources.PosMenuButton;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return posMenuLineID;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            if (!isRevert)
            {
                generalTab = new TabControl.Tab(Properties.Resources.General, ViewPages.HospitalityPosMenuLineGeneralPage.CreateInstance);
                fontTab = new TabControl.Tab(Properties.Resources.Font, ViewPages.HospitalityPosMenuLineFontPage.CreateInstance);
                attributesTab = new TabControl.Tab(Properties.Resources.Attributes, ViewPages.HospitalityPosMenuLineAttributesPage.CreateInstance);
                glyphsTab = new TabControl.Tab(Properties.Resources.Glyphs, ViewPages.HospitalityPosMenuLineGlyphsPage.CreateInstance);

                tabSheetTabs.AddTab(generalTab);
                tabSheetTabs.AddTab(fontTab);
                tabSheetTabs.AddTab(attributesTab);
                tabSheetTabs.AddTab(glyphsTab);

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, posMenuLineID);
            }

            posMenuLine = Providers.PosMenuLineData.Get(PluginEntry.DataModel, posMenuLineID);
            parentPosMenuHeader = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, posMenuLine.MenuID);

            tbKeyNo.Text = posMenuLine.KeyNo.ToString();
            tbDescription.Text = posMenuLine.Text;
            cmbOperation.SelectedData = Providers.HospitalityOperationData.Get(PluginEntry.DataModel, posMenuLine.Operation) ?? new DataEntity(RecordIdentifier.Empty, "");            
            curParameterType = posMenuLine.ParameterType;            

            switch (curParameterType)
            {
                case PosMenuLine.ParameterTypeEnum.None:
                    cmbParameter.SelectedData = Providers.PosParameterSetupData.Get(PluginEntry.DataModel,new RecordIdentifier(posMenuLine.Operation, posMenuLine.Parameter)) ?? new DataEntity("", "");
                    break;

                case PosMenuLine.ParameterTypeEnum.SubMenu:
                    cmbParameter.SelectedData = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, posMenuLine.Parameter) ?? new DataEntity("","");
                    break;
            }

            HeaderText = Description;
            
            tabSheetTabs.SetData(isRevert, posMenuLineID, posMenuLine);

            tabSheetTabs.LoadAllTabs();
        }

        protected override bool DataIsModified()
        {
            posMenuLine.Dirty = false;

            bool tabsModified = tabSheetTabs.IsModified();

            posMenuLine.Dirty = posMenuLine.Dirty |
                (tbDescription.Text != posMenuLine.Text) |
                (cmbOperation.SelectedData.ID != posMenuLine.Operation) |
                (curParameterType != posMenuLine.ParameterType);
            
            switch (curParameterType)
            {
                case PosMenuLine.ParameterTypeEnum.None:
                    posMenuLine.Dirty = posMenuLine.Dirty | (cmbParameter.SelectedData != null && cmbParameter.SelectedData.ID != "" ? cmbParameter.SelectedData.ID[1] != posMenuLine.Parameter : posMenuLine.Parameter != "");
                    break;

                case PosMenuLine.ParameterTypeEnum.SubMenu:
                    posMenuLine.Dirty = posMenuLine.Dirty | (cmbParameter.SelectedData.ID.ToString() != posMenuLine.Parameter);
                    break;
            }


            return tabsModified | posMenuLine.Dirty;
        }

        protected override bool SaveData()
        {           
            tabSheetTabs.GetData();

            if (posMenuLine.Dirty)
            {          
                posMenuLine.Text = tbDescription.Text;
                posMenuLine.Operation = cmbOperation.SelectedData != null ? cmbOperation.SelectedData.ID : 0;
                posMenuLine.ParameterType = curParameterType;

                switch (curParameterType)
                {
                    case PosMenuLine.ParameterTypeEnum.None:
                        posMenuLine.Parameter = cmbParameter.SelectedData != null && cmbParameter.SelectedData.ID != "" ? cmbParameter.SelectedData.ID[1].ToString() : "";
                        break;

                    case PosMenuLine.ParameterTypeEnum.SubMenu:
                        posMenuLine.Parameter = cmbParameter.SelectedData != null && cmbParameter.SelectedData.ID != "" ? cmbParameter.SelectedData.ID.ToString() : "";
                        break;
                }                                

                Providers.PosMenuLineData.Save(PluginEntry.DataModel, posMenuLine);

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "PosMenuLine", posMenuLine.ID, null);
            }
            return true;
        }

        protected override void OnDelete()
        {
            PluginOperations.DeletePosMenuLine(posMenuLineID);
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.ViewPosMenus, null, new ContextbarClickEventHandler((ContextbarClickEventHandler)PluginOperations.ShowPosMenusListView)), 200);
            }
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "PosMenuLine":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == posMenuLineID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    break;

                case "PreviewMenuButton":
                    SetPreviewButton((PosMenuLine)param);
                    break;

            }

            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }

        private void SetPreviewButton(PosMenuLine posMenuLineInfo)
        {
            btnMenuButtonPreview.Text = posMenuLineInfo.Text;

            #region General
            btnMenuButtonPreview.Transparent = posMenuLineInfo.Transparent;     
            #endregion

            #region Font
            // Set the font        
            if (posMenuLineInfo.UseHeaderFont && !String.IsNullOrEmpty(parentPosMenuHeader.FontName))
            {
                FontStyle style = FontStyle.Regular;
                
                if (parentPosMenuHeader.FontBold) { style = style | FontStyle.Bold; }
                if (parentPosMenuHeader.FontItalic) { style = style | FontStyle.Italic; }

                btnMenuButtonPreview.Font = new Font(
                    parentPosMenuHeader.FontName,
                    parentPosMenuHeader.FontSize == 0 ? 1 : (float)parentPosMenuHeader.FontSize,
                    style,
                    btnMenuButtonPreview.Font.Unit,
                    Convert.ToByte(parentPosMenuHeader.FontCharset));
            }
            else if(!String.IsNullOrEmpty(posMenuLineInfo.FontName))
            {
                FontStyle style = FontStyle.Regular;

                if (posMenuLineInfo.FontBold) { style = style | FontStyle.Bold; }
                if (posMenuLineInfo.FontItalic) { style = style | FontStyle.Italic; }
                if (posMenuLineInfo.FontStrikethrough) { style = style | FontStyle.Strikeout; }
                if (posMenuLineInfo.FontUnderline) { style = style | FontStyle.Underline; }

                btnMenuButtonPreview.Font = new Font(
                    posMenuLineInfo.FontName,
                    posMenuLineInfo.FontSize == 0 ? 1 : (float)posMenuLineInfo.FontSize,
                    style,
                    btnMenuButtonPreview.Font.Unit,
                    Convert.ToByte(posMenuLineInfo.FontCharset));
            }
            #endregion

            #region Attributes
            if (posMenuLineInfo.UseHeaderAttributes)
            {
                btnMenuButtonPreview.GradientMode = parentPosMenuHeader.GradientMode;
                btnMenuButtonPreview.ButtonColor = Color.FromArgb(parentPosMenuHeader.BackColor);
                btnMenuButtonPreview.ButtonColor2 = Color.FromArgb(parentPosMenuHeader.BackColor2);
                btnMenuButtonPreview.Shape = parentPosMenuHeader.Shape;
            }
            else
            {
                btnMenuButtonPreview.GradientMode = posMenuLineInfo.GradientMode;
                btnMenuButtonPreview.ButtonColor = Color.FromArgb(posMenuLineInfo.BackColor);
                btnMenuButtonPreview.ButtonColor2 = Color.FromArgb(posMenuLineInfo.BackColor2);
                btnMenuButtonPreview.Shape = posMenuLineInfo.Shape;
            }
            #endregion

            #region Glyphs
            // Glyph 1
            string glyphFontName = "";
            btnMenuButtonPreview.Glyph1Color = Color.FromArgb(posMenuLineInfo.GlyphTextForeColor);
            btnMenuButtonPreview.Glyph1Offset = posMenuLineInfo.GlyphOffSet;
            float glyphFontSize = posMenuLineInfo.GlyphTextFontSize == 0 ? 1 : posMenuLineInfo.GlyphTextFontSize;

            if ((int)posMenuLineInfo.Glyph < 13 && (int)posMenuLineInfo.Glyph > 0)
            {
                // Windings characters
                glyphFontName = "Wingdings";
                btnMenuButtonPreview.Glyph1Font = new Font(glyphFontName, glyphFontSize);
                btnMenuButtonPreview.Glyph1Text = Convert.ToString(GetWindingsGlyphChar(posMenuLineInfo.Glyph));
            }
            else if ((int)posMenuLineInfo.Glyph >= 13 && (int)posMenuLineInfo.Glyph < 25)
            {
                // F buttons
                glyphFontName = "Arial Narrow";
                btnMenuButtonPreview.Glyph1Font = new Font(glyphFontName, glyphFontSize);
                btnMenuButtonPreview.Glyph1Text = posMenuLineInfo.Glyph.ToString();
            }
            else if (posMenuLineInfo.Glyph == PosMenuLine.GlyphEnum.Text)
            {
                // Normal text
                glyphFontName = posMenuLineInfo.GlyphTextFont;
                btnMenuButtonPreview.Glyph1Font = new Font(glyphFontName, glyphFontSize);
                btnMenuButtonPreview.Glyph1Text = posMenuLineInfo.GlyphText;
            }
            else if (posMenuLineInfo.Glyph == PosMenuLine.GlyphEnum.None)
            {
                // Clear all text from the glyph
                btnMenuButtonPreview.Glyph1Text = "";
            }


            // Glyph 2
            glyphFontName = "";
            btnMenuButtonPreview.Glyph2Color = Color.FromArgb(posMenuLineInfo.GlyphText2ForeColor);
            btnMenuButtonPreview.Glyph2Offset = posMenuLineInfo.Glyph2OffSet;
            glyphFontSize = posMenuLineInfo.GlyphText2FontSize == 0 ? 1 : posMenuLineInfo.GlyphText2FontSize;

            if ((int)posMenuLineInfo.Glyph2 < 13 && (int)posMenuLineInfo.Glyph2 > 0)
            {
                // Windings characters
                glyphFontName = "Wingdings";
                btnMenuButtonPreview.Glyph2Font = new Font(glyphFontName, glyphFontSize);
                btnMenuButtonPreview.Glyph2Text = Convert.ToString(GetWindingsGlyphChar(posMenuLineInfo.Glyph2));
            }
            else if ((int)posMenuLineInfo.Glyph2 >= 13 && (int)posMenuLineInfo.Glyph2 < 25)
            {
                // F buttons
                glyphFontName = "Arial Narrow";
                btnMenuButtonPreview.Glyph2Font = new Font(glyphFontName, glyphFontSize);
                btnMenuButtonPreview.Glyph2Text = posMenuLineInfo.Glyph2.ToString();
            }
            else if (posMenuLineInfo.Glyph2 == PosMenuLine.GlyphEnum.Text)
            {
                // Normal text
                glyphFontName = posMenuLineInfo.GlyphText2Font;
                btnMenuButtonPreview.Glyph2Font = new Font(glyphFontName, glyphFontSize);
                btnMenuButtonPreview.Glyph2Text = posMenuLineInfo.GlyphText2;
            }
            else if (posMenuLineInfo.Glyph2 == PosMenuLine.GlyphEnum.None)
            {
                // Clear all text from the glyph
                btnMenuButtonPreview.Glyph2Text = "";
            }

            // Glyph 3
            glyphFontName = "";
            btnMenuButtonPreview.Glyph3Color = Color.FromArgb(posMenuLineInfo.GlyphText3ForeColor);
            btnMenuButtonPreview.Glyph3Offset = posMenuLineInfo.Glyph3OffSet;
            glyphFontSize = posMenuLineInfo.GlyphText3FontSize == 0 ? 1 : posMenuLineInfo.GlyphText3FontSize;

            if ((int)posMenuLineInfo.Glyph3 < 13 && (int)posMenuLineInfo.Glyph3 > 0)
            {
                // Windings characters
                glyphFontName = "Wingdings";
                btnMenuButtonPreview.Glyph3Font = new Font(glyphFontName, glyphFontSize);
                btnMenuButtonPreview.Glyph3Text = Convert.ToString(GetWindingsGlyphChar(posMenuLineInfo.Glyph3));
            }
            else if ((int)posMenuLineInfo.Glyph3 >= 13 && (int)posMenuLineInfo.Glyph3 < 25)
            {
                // F buttons
                glyphFontName = "Arial Narrow";
                btnMenuButtonPreview.Glyph3Font = new Font(glyphFontName, glyphFontSize);
                btnMenuButtonPreview.Glyph3Text = posMenuLineInfo.Glyph3.ToString();
            }
            else if (posMenuLineInfo.Glyph3 == PosMenuLine.GlyphEnum.Text)
            {
                // Normal text
                glyphFontName = posMenuLineInfo.GlyphText3Font;
                btnMenuButtonPreview.Glyph3Font = new Font(glyphFontName, glyphFontSize);
                btnMenuButtonPreview.Glyph3Text = posMenuLineInfo.GlyphText3;
            }
            else if (posMenuLineInfo.Glyph3 == PosMenuLine.GlyphEnum.None)
            {
                // Clear all text from the glyph
                btnMenuButtonPreview.Glyph3Text = "";
            }

            // Glyph 4
            glyphFontName = "";
            btnMenuButtonPreview.Glyph4Color = Color.FromArgb(posMenuLineInfo.GlyphText4ForeColor);
            btnMenuButtonPreview.Glyph4Offset = posMenuLineInfo.Glyph4OffSet;
            glyphFontSize = posMenuLineInfo.GlyphText4FontSize == 0 ? 1 : posMenuLineInfo.GlyphText4FontSize;

            if ((int)posMenuLineInfo.Glyph4 < 13 && (int)posMenuLineInfo.Glyph4 > 0)
            {
                // Windings characters
                glyphFontName = "Wingdings";
                btnMenuButtonPreview.Glyph4Font = new Font(glyphFontName, glyphFontSize);
                btnMenuButtonPreview.Glyph4Text = Convert.ToString(GetWindingsGlyphChar(posMenuLineInfo.Glyph4));
            }
            else if ((int)posMenuLineInfo.Glyph4 >= 13 && (int)posMenuLineInfo.Glyph4 < 25)
            {
                // F buttons
                glyphFontName = "Arial Narrow";
                btnMenuButtonPreview.Glyph4Font = new Font(glyphFontName, glyphFontSize);
                btnMenuButtonPreview.Glyph4Text = posMenuLineInfo.Glyph4.ToString();
            }
            else if (posMenuLineInfo.Glyph4 == PosMenuLine.GlyphEnum.Text)
            {
                // Normal text
                glyphFontName = posMenuLineInfo.GlyphText4Font;
                btnMenuButtonPreview.Glyph4Font = new Font(glyphFontName, glyphFontSize);
                btnMenuButtonPreview.Glyph4Text = posMenuLineInfo.GlyphText4;
            }
            else if (posMenuLineInfo.Glyph4 == PosMenuLine.GlyphEnum.None)
            {
                // Clear all text from the glyph
                btnMenuButtonPreview.Glyph4Text = "";
            }
            #endregion                                    
        }

        private char GetWindingsGlyphChar(PosMenuLine.GlyphEnum glyph)
        {
            int charNumber = 0;

            switch (glyph)
            {
                case PosMenuLine.GlyphEnum.One:
                    charNumber = 232;
                    break;

                case PosMenuLine.GlyphEnum.Two:
                    charNumber = 238;
                    break;

                case PosMenuLine.GlyphEnum.Three:
                    charNumber = 224;
                    break;

                case PosMenuLine.GlyphEnum.Four:
                    charNumber = 230;
                    break;

                case PosMenuLine.GlyphEnum.Five:
                    charNumber = 236;
                    break;

                case PosMenuLine.GlyphEnum.Six:
                    charNumber = 248;
                    break;

                case PosMenuLine.GlyphEnum.Seven:
                    charNumber = 177;
                    break;

                case PosMenuLine.GlyphEnum.Eight:
                    charNumber = 162;
                    break;

                case PosMenuLine.GlyphEnum.Nine:
                    charNumber = 112;
                    break;

                case PosMenuLine.GlyphEnum.Ten:
                    charNumber = 117;
                    break;

                case PosMenuLine.GlyphEnum.Eleven:
                    charNumber = 118;
                    break;

                case PosMenuLine.GlyphEnum.Twelve:
                    charNumber = 164;
                    break;
            }

            return (char)charNumber;
        }

        private void ClearData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void cmbOperation_RequestData(object sender, EventArgs e)
        {
            var operationList = Providers.HospitalityOperationData.GetList(PluginEntry.DataModel);
            cmbOperation.SetData(operationList, null);
        }

        private void cmbOperation_SelectedDataChanged(object sender, EventArgs e)
        {
            if (cmbOperation.SelectedData.ID != RecordIdentifier.Empty)
            {
                HospitalityOperation operation = (HospitalityOperation)cmbOperation.SelectedData;
                curParameterType = (PosMenuLine.ParameterTypeEnum)((int)operation.ParameterType);                
            }
        }

        private void cmbParameter_RequestData(object sender, EventArgs e)
        {
            switch (curParameterType)
            {
                case PosMenuLine.ParameterTypeEnum.None:
                    cmbParameter.SetWidth(350);

                    cmbParameter.SetHeaders(new string[] {
                        Properties.Resources.OperationID,
                        Properties.Resources.Code,
                        Properties.Resources.Description},
                        new int[] { 0, 1, 2 });

                    var listOfParameters = Providers.PosParameterSetupData.GetList(PluginEntry.DataModel, cmbOperation.SelectedData.ID);

                    cmbParameter.SetData(listOfParameters, null);
                    break;

                case PosMenuLine.ParameterTypeEnum.SubMenu:                    
                    cmbParameter.SetData(Providers.PosMenuHeaderData.GetList(PluginEntry.DataModel, MenuTypeEnum.Hospitality), null);
                    break;
            }
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            PosMenuLine copy = PluginOperations.CopyLine(posMenuLine);

            copy.Text = tbDescription.Text;

            SetPreviewButton(copy);
        }

        private void FormatData_Operation(object sender, DropDownFormatDataArgs e)
        {
            if (((HospitalityOperation)e.Data).ID == RecordIdentifier.Empty || ((HospitalityOperation)e.Data).ID == "")
            {
                e.TextToDisplay = "";
                //e.TextToDisplay = ((DataEntity)e.Data).Text;
            }
            else
            {
                e.TextToDisplay = (string)((HospitalityOperation)e.Data).Text;
            }
        }

        private void cmbParameter_FormatData(object sender, DropDownFormatDataArgs e)
        {  
            if (((DataEntity)e.Data).ID == "" || ((DataEntity)e.Data).ID == RecordIdentifier.Empty)
            {
                e.TextToDisplay = "";
            }
            else
            {
                e.TextToDisplay = ((DataEntity)e.Data).ID.ToString() + " - " + ((DataEntity)e.Data).Text;

                switch (curParameterType)
                {
                    case PosMenuLine.ParameterTypeEnum.None:
                        e.TextToDisplay = ((DataEntity)e.Data).ID[1].ToString() + " - " + ((DataEntity)e.Data).Text;
                        break;

                    case PosMenuLine.ParameterTypeEnum.SubMenu:
                        e.TextToDisplay = ((DataEntity)e.Data).ID.ToString() + " - " + ((DataEntity)e.Data).Text;
                        break;
                }
            }
        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();

            btnMenuButtonPreview.Dispose();

            base.OnClose();
        }
    }
}
