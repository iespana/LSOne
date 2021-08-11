using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using System.Drawing;
using LSOne.Utilities.ColorPalette;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    public partial class PosMenuLookAndFeelPage : UserControl, ITabView
    {
        private RecordIdentifier posMenuHeaderID;
        private PosMenuHeader posMenuHeader;

        public PosMenuLookAndFeelPage()
        {
            InitializeComponent();

            buttonProperties.PosMenuHeader.BorderColor = ColorPalette.POSControlBorderColor.ToArgb();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new PosMenuLookAndFeelPage();
        }

        public bool DataIsModified()
        {
            return buttonProperties.IsModified(posMenuHeader) ||
                (int)ntbBorderWidth.Value != posMenuHeader.BorderWidth ||
                (int)ntbMargin.Value != posMenuHeader.Margin ||
                cwBorderColor.SelectedColor.ToArgb() != posMenuHeader.BorderColor;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            posMenuHeaderID = context;
            posMenuHeader = (PosMenuHeader)internalContext;
            ntbBorderWidth.Value = posMenuHeader.BorderWidth;
            ntbMargin.Value = posMenuHeader.Margin;
            cwBorderColor.SelectedColor = Color.FromArgb(posMenuHeader.BorderColor);
            buttonProperties.PosMenuHeader = posMenuHeader;
        }

        public void OnClose()
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public bool SaveData()
        {
            posMenuHeader.StyleID = buttonProperties.PosStyleID;
            posMenuHeader.StyleID = posMenuHeader.StyleID == "" ? RecordIdentifier.Empty : posMenuHeader.StyleID;
            posMenuHeader.BorderWidth = (int)ntbBorderWidth.Value;
            posMenuHeader.Margin = (int)ntbMargin.Value;
            posMenuHeader.BorderColor = cwBorderColor.SelectedColor.ToArgb();
            buttonProperties.ToPosMenuHeader(posMenuHeader);
            return true;
        }

        public void SaveUserInterface()
        {
            
        }

        private void buttonProperties_Modified(object sender, EventArgs e)
        {
            buttonProperties.ToButton(btnMenuButtonPreview, (int)ntbBorderWidth.Value, cwBorderColor.SelectedColor);
        }
    }
}
